using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoOrganization
{
    class Model
    {
        DataTable dtPreset_;
        Evernote evernote_;

        public Model()
        {
            dtPreset_ = new DataTable("Presets");
            dtPreset_.Columns.Add("ID", typeof(string));
            dtPreset_.Columns.Add("TargetNotebook", typeof(string));
            dtPreset_.Columns.Add("TargetTags", typeof(string));
            dtPreset_.Columns.Add("IsMoveNotebook", typeof(bool));
            dtPreset_.Columns.Add("MoveToNotebook", typeof(string));
            dtPreset_.Columns.Add("AddTags", typeof(string));
            dtPreset_.Columns.Add("IsAddTags", typeof(bool));

            dtPreset_.PrimaryKey = new DataColumn[] { dtPreset_.Columns["ID"] };

            dtPreset_.AcceptChanges();
        }

        public DataTable Presets
        {
            get { return dtPreset_; }
        }

        public Evernote Evernote
        {
            get { return evernote_; }
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
            newPreset["IsMoveNotebook"] = false;
            newPreset["MoveToNotebook"] = evernote_.DefaultNotebookName;
            newPreset["IsAddTags"] = false;
            newPreset["AddTags"] = string.Empty;

            dtPreset_.Rows.Add(newPreset);
        }

        public void UpdatePreset(int index,string targetNotebook,string targetTags,bool isMoveNotebook,
            string moveToNotebook,bool isAddTags,string AddTags)
        {
            DataRow dr=dtPreset_.Rows[index];

            dr["TargetNotebook"] = targetNotebook;
            dr["TargetTags"] = targetTags;
            dr["IsMoveNotebook"] = isMoveNotebook;
            dr["MoveToNotebook"] = moveToNotebook;
            dr["IsAddTags"] = isAddTags;
            dr["AddTags"] = AddTags;

            dtPreset_.AcceptChanges();
        }
    }
}
