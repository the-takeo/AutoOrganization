using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Thrift.Protocol;
using Thrift.Transport;
using Evernote.EDAM.Type;
using Evernote.EDAM.UserStore;
using Evernote.EDAM.NoteStore;
using System.Drawing;
using System.Text;
using System.Collections;

namespace AutoOrganization
{
    /// <summary>
    /// Evernote操作関連のクラス
    /// </summary>
    public class Evernote
    {
        const string EvernoteHost = "sandbox.evernote.com";
        string authToken_;
        UserStore.Client userStore_;
        NoteStore.Client noteStore_;

        Dictionary<string, string> notebooks_;
        Dictionary<string, string> tags_;

        /// <summary>
        /// Evernote操作関連のクラスのコンストラクタ
        /// </summary>
        /// <param name="evernoteToken">EvernoteToken</param>
        public Evernote(string evernoteToken)
        {
            authToken_ = evernoteToken;

            Uri userStoreUrl = new Uri("https://" + EvernoteHost + "/edam/user");
            TTransport userStoreTransport = new THttpClient(userStoreUrl);
            TProtocol userStoreProtocol = new TBinaryProtocol(userStoreTransport);
            userStore_ = new UserStore.Client(userStoreProtocol);

            String noteStoreUrl = userStore_.getNoteStoreUrl(authToken_);

            TTransport noteStoreTransport = new THttpClient(new Uri(noteStoreUrl));
            TProtocol noteStoreProtocol = new TBinaryProtocol(noteStoreTransport);
            noteStore_ = new NoteStore.Client(noteStoreProtocol);
        }

        /// <summary>
        /// Evernoteのユーザー名
        /// </summary>
        public string GetEvernoteUserName
        {
            get { return userStore_.getUser(authToken_).Username; }
        }

        /// <summary>
        /// EvernoteのNotebookリスト
        /// </summary>
        public List<string> Notebooknames
        {
            get { return new List<string>(Notebooks.Keys.ToArray().Reverse()); }
        }

        /// <summary>
        /// デフォルトのNotebookの名前
        /// </summary>
        public string DefaultNotebookName
        {
            get { return noteStore_.getDefaultNotebook(authToken_).Name; }
        }

        /// <summary>
        /// EvernoteのNotebook情報
        /// </summary>
        private Dictionary<string, string> Notebooks
        {
            get
            {
                if (notebooks_ == null)
                {
                    List<Notebook> notebooks = noteStore_.listNotebooks(authToken_);

                    Dictionary<string, string> notebookNames = new Dictionary<string, string>();

                    foreach (var notebook in notebooks)
                    {
                        notebookNames.Add(notebook.Name, notebook.Guid);
                    }

                    notebooks_ = notebookNames;
                }

                return notebooks_;
            }
        }

        /// <summary>
        /// EvernoteのTag情報
        /// </summary>
        public Dictionary<string, string> Tags
        {
            get
            {
                if (tags_ == null)
                {
                    RefleshTags();
                }

                return tags_;
            }
        }

        /// <summary>
        /// Tag情報のキャッシュを更新する
        /// </summary>
        private void RefleshTags()
        {
            List<Tag> tags = noteStore_.listTags(authToken_);

            Dictionary<string, string> tagNames = new Dictionary<string, string>();

            foreach (var tag in tags)
            {
                tagNames.Add(tag.Name, tag.Guid);
            }

            tags_ = tagNames;
        }

        /// <summary>
        /// 新規Tagを追加する
        /// </summary>
        /// <param name="newTagName">追加Tag名称</param>
        public void AddNewTag(string newTagName)
        {
            Tag newTag = new Tag();
            newTag.Name = newTagName;

            noteStore_.createTag(authToken_, newTag);

            RefleshTags();
        }

        /// <summary>
        /// 条件に沿ったNoteのGuidのリストを返す
        /// </summary>
        /// <param name="targetNotebook">対象Notebook</param>
        /// <param name="targetTags">対象Tag</param>
        /// <param name="targetURL">対象URL</param>
        /// <returns>条件に沿ったNoteのGuidリスト</returns>
        public List<string> GetFilteredNoteGuids(string targetNotebook, string targetTags, string targetURL)
        {
            NoteFilter noteFilter = new NoteFilter();
            noteFilter.NotebookGuid = Notebooks[targetNotebook];

            if (string.IsNullOrEmpty(targetTags) == false)
            {
                noteFilter.TagGuids = new List<string>(from targetTag in Tags where targetTags.Split(',').Contains(targetTag.Key) select targetTag.Value);

                //指定したTagが存在しない場合、該当なしなのでからのリストを返す
                if (noteFilter.TagGuids.Count == 0) return new List<string>();
            }

            var notesMetadataResultSpec = new NotesMetadataResultSpec();
            notesMetadataResultSpec.IncludeAttributes = true;

            var result = new List<string>();
            var targetNotes = noteStore_.findNotesMetadata(authToken_, noteFilter, 0, 10000, notesMetadataResultSpec);

            // findNotesMetadataは一度に250件までしかNOteを取得できないので、分割して取得する。
            int count = (targetNotes.TotalNotes / 250) + 1;
            for (int i = 0; i < count; i++)
            {
                targetNotes = noteStore_.findNotesMetadata(authToken_, noteFilter, 250 * i, 10000, notesMetadataResultSpec);

                IEnumerable<string> targetNoteGuidList;
                if (string.IsNullOrEmpty(targetURL) == false)
                    targetNoteGuidList = from note in targetNotes.Notes where (note.Attributes.SourceURL != null) &&
                                             note.Attributes.SourceURL.Contains(targetURL) select note.Guid;
                else
                    targetNoteGuidList = from note in targetNotes.Notes select note.Guid;

                result.AddRange(targetNoteGuidList);
            }

            return result;
        }

        /// <summary>
        /// 条件に基づいた操作を行う
        /// </summary>
        /// <param name="noteGuid">対象NoteのGuid</param>
        /// <param name="isMoveNotebook">Notebookを移動するか</param>
        /// <param name="moveNotebook">移動先Notebook名称</param>
        /// <param name="isAddTags">Tagを付加するか</param>
        /// <param name="addTags">付加するTag名称</param>
        /// <returns>更新に成功したか</returns>
        public bool DoAction(string noteGuid, bool isMoveNotebook, string moveNotebook, bool isAddTags, string addTags)
        {
            var targetNote = noteStore_.getNote(authToken_, noteGuid, false, false, false, false);

            if (isMoveNotebook)
                targetNote.NotebookGuid = Notebooks[moveNotebook];

            if (isAddTags)
            {
                if (targetNote.TagGuids == null)
                    targetNote.TagGuids = new List<string>();

                targetNote.TagGuids.AddRange(from addTag in Tags where addTags.Split(',').Contains(addTag.Key) select addTag.Value);
            }
            try
            {
                noteStore_.updateNote(authToken_, targetNote);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
