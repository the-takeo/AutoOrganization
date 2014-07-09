﻿using System;
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
        /// Notebook移動のみという操作が今のところ無いので未使用
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
        /// Tag付加のみという操作が今のところ無いので未使用
        /// </summary>
        /// <param name="note">対象Note</param>
        /// <param name="tagName">付加するTag名称</param>
        private void AddTag(Note note, string tagName)
        {
            string tagGuid = Tags[tagName];

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
        public List<string> GetTargetNoteGuids(string notebookName, string tagName = null, string words = null)
        {
            NoteFilter noteFilter = new NoteFilter();
            noteFilter.NotebookGuid = Notebooks[notebookName];

            if (tagName != null)
                noteFilter.TagGuids.Add(Tags[tagName]);

            if (words != null)
                noteFilter.Words = words;

            var notes = noteStore_.findNotesMetadata(authToken_, noteFilter, 0, 1000, new NotesMetadataResultSpec());
            var result = new List<string>();
            foreach (var note in notes.Notes)
            {
                result.Add(note.Guid);
            }

            return result;
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
                noteFilter.TagGuids = new List<string>(from targetTag in Tags where targetTags.Split(',').Contains(targetTag.Key) select targetTag.Value);

            var notesMetadataResultSpec = new NotesMetadataResultSpec();
            notesMetadataResultSpec.IncludeAttributes = true;

            var targetNotes = noteStore_.findNotesMetadata(authToken_, noteFilter, 0, 1000, notesMetadataResultSpec);

            IEnumerable<string> targetNoteGuidList;
            if (string.IsNullOrEmpty(targetURL) == false)
                targetNoteGuidList = from note in targetNotes.Notes where (note.Attributes.SourceURL != null) && note.Attributes.SourceURL.Contains(targetURL) select note.Guid;
            else
                targetNoteGuidList = from note in targetNotes.Notes select note.Guid;

            return new List<string>(targetNoteGuidList);
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
                targetNote.NotebookGuid = notebooks_[moveNotebook];

            if (isAddTags)
                targetNote.TagGuids.AddRange(from addTag in Tags where addTags.Split(',').Contains(addTag.Key) select addTag.Value);

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
