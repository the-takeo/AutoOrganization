using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;

namespace AutoOrganization
{
    [DataContract]
    public class Model
    {
        [DataMember]
        DataTable dtAction_;

        [DataMember]
        string evernoteAuthToken_;

        Evernote evernote_;

        public Model(string evernoteAuthToken)
        {
            dtAction_ = new DataTable("Actions");
            dtAction_.Columns.Add("ID", typeof(string));
            dtAction_.Columns.Add("TargetNotebook", typeof(string));
            dtAction_.Columns.Add("TargetTags", typeof(string));
            dtAction_.Columns.Add("TargetURL", typeof(string));
            dtAction_.Columns.Add("IsMoveNotebook", typeof(bool));
            dtAction_.Columns.Add("MoveToNotebook", typeof(string));
            dtAction_.Columns.Add("AddTags", typeof(string));
            dtAction_.Columns.Add("IsAddTags", typeof(bool));

            dtAction_.PrimaryKey = new DataColumn[] { dtAction_.Columns["ID"] };

            dtAction_.AcceptChanges();

            evernoteAuthToken_ = evernoteAuthToken;

            evernote_ = new Evernote(evernoteAuthToken_);
        }

        public DataTable Actions
        {
            get { return dtAction_; }
        }

        public Evernote Evernote
        {
            get 
            {
                if(evernote_==null)
                {
                    evernote_ = new Evernote(evernoteAuthToken_);
                }

                return evernote_; 
            }
        }

        public string EvernoteAuthToken
        {
            get { return evernoteAuthToken_; }
            set { evernoteAuthToken_ = value; }
        }

        public List<string> IDs
        {
            get { return Actions.AsEnumerable().Select(n => n["ID"].ToString()).ToList(); }
        }

        public void RefleshEvernoteClass()
        {
            evernote_ = new Evernote(EvernoteAuthToken);
        }

        public void AddNewAction()
        {
            var newAction = dtAction_.NewRow();

            int count = 1;

            while (true)
            {
                if (dtAction_.AsEnumerable().Select(n => n["ID"].ToString()).Contains("新規プリセット" + count.ToString()) == false)
                {
                    newAction["ID"] = "新規プリセット" + count.ToString();
                    break;
                }

                count++;
            }

            newAction["TargetNotebook"] = evernote_.DefaultNotebookName;
            newAction["TargetTags"] = string.Empty;
            newAction["TargetURL"] = string.Empty;
            newAction["IsMoveNotebook"] = false;
            newAction["MoveToNotebook"] = evernote_.DefaultNotebookName;
            newAction["IsAddTags"] = false;
            newAction["AddTags"] = string.Empty;

            dtAction_.Rows.Add(newAction);

            dtAction_.AcceptChanges();

            Save();
        }

        public void DeleteAction(int index)
        {
            dtAction_.Rows.RemoveAt(index);

            dtAction_.AcceptChanges();

            Save();
        }

        public void UpdateAction(int index,string targetNotebook,string targetTags,string targetURL,
            bool isMoveNotebook,string moveToNotebook,bool isAddTags,string AddTags)
        {
            DataRow dr=dtAction_.Rows[index];

            dr["TargetNotebook"] = targetNotebook;
            dr["TargetTags"] = targetTags;
            dr["TargetURL"] = targetURL;
            dr["IsMoveNotebook"] = isMoveNotebook;
            dr["MoveToNotebook"] = moveToNotebook;
            dr["IsAddTags"] = isAddTags;
            dr["AddTags"] = AddTags;

            dtAction_.AcceptChanges();

            Save();
        }

        public void RenameAction(int index,string newName)
        {
            DataRow dr = dtAction_.Rows[index];

            dr["ID"] = newName;

            dtAction_.AcceptChanges();

            Save();
        }

        public void Save()
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(Model));
            FileStream fs = new FileStream(@"Settings.xml", FileMode.Create);
            serializer.WriteObject(fs, this);
            fs.Close();
        }

        public List<string> GetFilteredNoteGuids(string actionName)
        {
            DataRow dr = Actions.Rows.Find(actionName);

            string targetNotebook = dr["TargetNotebook"].ToString();
            string targetTags = dr["TargetTags"].ToString();
            string targetURL = dr["TargetURL"].ToString();

            return Evernote.GetFilteredNoteGuids(targetNotebook, targetTags, targetURL);
        }

        public void ActionParams(string actionName, out bool isMoveNotebook, out string moveNotebook, out bool isAddTags, out string addTags)
        {
            DataRow dr = Actions.Rows.Find(actionName);

            isMoveNotebook = (bool)dr["IsMoveNotebook"];
            moveNotebook = dr["MoveToNotebook"].ToString();
            isAddTags = (bool)dr["IsAddTags"];
            addTags = dr["AddTags"].ToString();
        }
    }
}
