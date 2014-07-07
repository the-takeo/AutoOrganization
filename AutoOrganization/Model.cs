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
    class Model
    {
        [DataMember]
        DataTable dtPreset_;

        [DataMember]
        string evernoteAuthToken_;

        Evernote evernote_;

        public Model(string evernoteAuthToken)
        {
            dtPreset_ = new DataTable("Presets");
            dtPreset_.Columns.Add("ID", typeof(string));
            dtPreset_.Columns.Add("TargetNotebook", typeof(string));
            dtPreset_.Columns.Add("TargetTags", typeof(string));
            dtPreset_.Columns.Add("TargetURL", typeof(string));
            dtPreset_.Columns.Add("IsMoveNotebook", typeof(bool));
            dtPreset_.Columns.Add("MoveToNotebook", typeof(string));
            dtPreset_.Columns.Add("AddTags", typeof(string));
            dtPreset_.Columns.Add("IsAddTags", typeof(bool));

            dtPreset_.PrimaryKey = new DataColumn[] { dtPreset_.Columns["ID"] };

            dtPreset_.AcceptChanges();

            evernoteAuthToken_ = evernoteAuthToken;

            evernote_ = new Evernote(evernoteAuthToken_);
        }

        public DataTable Presets
        {
            get { return dtPreset_; }
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

        public void RefleshEvernoteClass()
        {
            evernote_ = new Evernote(EvernoteAuthToken);
        }

        public void AddNewPreset()
        {
            var newPreset = dtPreset_.NewRow();

            bool isContain = true;
            int count = 0;

            while (isContain)
            {
                isContain = false;

                for (int j = 0; j < dtPreset_.Rows.Count; j++)
                {
                    if (dtPreset_.Rows[j]["ID"].ToString() == "新規プリセット" + count.ToString())
                    {
                        isContain = true;
                        break;
                    }
                }

                if (isContain == false)
                {
                    newPreset["ID"] = "新規プリセット" + count.ToString();
                    break;
                }

                count++;
            }

            newPreset["TargetNotebook"] = evernote_.DefaultNotebookName;
            newPreset["TargetTags"] = string.Empty;
            newPreset["TargetURL"] = string.Empty;
            newPreset["IsMoveNotebook"] = false;
            newPreset["MoveToNotebook"] = evernote_.DefaultNotebookName;
            newPreset["IsAddTags"] = false;
            newPreset["AddTags"] = string.Empty;

            dtPreset_.Rows.Add(newPreset);

            dtPreset_.AcceptChanges();

            Save();
        }

        public void UpdatePreset(int index,string targetNotebook,string targetTags,string targetURL,
            bool isMoveNotebook,string moveToNotebook,bool isAddTags,string AddTags)
        {
            DataRow dr=dtPreset_.Rows[index];

            dr["TargetNotebook"] = targetNotebook;
            dr["TargetTags"] = targetTags;
            dr["TargetURL"] = targetURL;
            dr["IsMoveNotebook"] = isMoveNotebook;
            dr["MoveToNotebook"] = moveToNotebook;
            dr["IsAddTags"] = isAddTags;
            dr["AddTags"] = AddTags;

            dtPreset_.AcceptChanges();

            Save();
        }

        public void Save()
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(Model));
            FileStream fs = new FileStream(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + @"\AutoSettings.xml", FileMode.Create);
            serializer.WriteObject(fs, this);
            fs.Close();
        }
    }
}
