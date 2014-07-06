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

namespace AutoOrganization
{
    /// <summary>
    /// Evernote操作関連のクラス
    /// </summary>
    class Evernote
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
        private Dictionary<string, string> Tags
        {
            get
            {
                if (tags_ == null)
                {
                    List<Tag> tags = noteStore_.listTags(authToken_);

                    Dictionary<string, string> tagNames = new Dictionary<string, string>();

                    foreach (var tag in tags)
                    {
                        tagNames.Add(tag.Name, tag.Guid);
                    }

                    tags_ = tagNames;
                }

                return tags_;
            }
        }

        /// <summary>
        /// Notebookを移動する
        /// </summary>
        /// <param name="note">対象Note</param>
        /// <param name="toNotebookName">移動先Notebook名称</param>
        private void MoveNote(Note note, string toNotebookName)
        {
            note.NotebookGuid = Notebooks[toNotebookName];

            noteStore_.updateNote(authToken_, note);
        }

        /// <summary>
        /// Tagを付加する
        /// </summary>
        /// <param name="note">対象Note</param>
        /// <param name="tagName">付加するTag名称</param>
        private void AddTag(Note note,string tagName)
        {
            string tagGuid=Tags[tagName];

            if (note.TagGuids.Contains(tagGuid) == false)
                note.TagGuids.Add(Tags[tagName]);

            noteStore_.updateNote(authToken_, note);
        }

        /// <summary>
        /// フィルタしたNoteのGuidのリストを返す
        /// </summary>
        /// <param name="notebookName">対象Notebook</param>
        /// <param name="tagName">対象Tag</param>
        /// <param name="words">対象ワード</param>
        /// <returns>NoteのGuidリスト</returns>
        public List<string> GetTargetNoteGuids(string notebookName,string tagName=null,string words=null)
        {
            NoteFilter noteFilter = new NoteFilter();
            noteFilter.NotebookGuid = Notebooks[notebookName];

            if (tagName != null)
                noteFilter.TagGuids.Add(Tags[tagName]);
            
            if (words != null)
                noteFilter.Words = words;

            var notes = noteStore_.findNotes(authToken_, noteFilter, 0, 1000);
            var result = new List<string>();
            foreach (var note in notes.Notes)
            {
                result.Add(note.Guid);
            }

            return result;
        }

        /// <summary>
        /// 条件に基づいた操作を行う
        /// </summary>
        /// <param name="targetNotebook">対象Notebook</param>
        /// <param name="targetTags">対象Tag</param>
        /// <param name="isMoveNotebook">Notebookを移動するか</param>
        /// <param name="MoveNotebook">移動先Notebook名称</param>
        /// <param name="isAddTags">Tagを付加するか</param>
        /// <param name="addTags">付加するTag名称</param>
        /// <returns>操作が行われたNote数（操作前と変化がなかったNoteを含む）</returns>
        public int DoAction(string targetNotebook, string targetTags, bool isMoveNotebook,
            string MoveNotebook, bool isAddTags, string addTags)
        {
            NoteFilter noteFilter = new NoteFilter();
            noteFilter.NotebookGuid = Notebooks[targetNotebook];
            noteFilter.TagGuids = new List<string>() { Tags[targetTags] };

            var targetNotes = noteStore_.findNotes(authToken_, noteFilter, 0, 1000);

            foreach (var targetNote in targetNotes.Notes)
            {
                if (isMoveNotebook)
                    targetNote.NotebookGuid = notebooks_[MoveNotebook];

                if (isAddTags)
                    targetNote.TagGuids.Add(Tags[addTags]);

                noteStore_.updateNote(authToken_, targetNote);
            }

            return targetNotes.Notes.Count;
        }
    }
}
