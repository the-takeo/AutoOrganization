using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoOrganization
{
    public partial class ProcessDialog : Form
    {
        List<string> targetNoteGuids_ = new List<string>();
        bool isMoveNotebook_ = false;
        string moveNotebook_ = string.Empty;
        bool isAddTags_ = false;
        string addTags_ = string.Empty;

        Evernote evernote_;

        int processedCount = 0;

        public ProcessDialog(List<string> targetNoteGuids, bool ismoveNotebook, string moveNotebook, bool isAddTags, string addTags,Evernote evernote)
        {
            InitializeComponent();

            this.ControlBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            targetNoteGuids_ = targetNoteGuids;
            isMoveNotebook_ = ismoveNotebook;
            moveNotebook_ = moveNotebook;
            isAddTags_ = isAddTags;
            addTags_ = addTags;

            evernote_ = evernote;

            lblProgress.Text = string.Format(Resource.ProgressMsg, targetNoteGuids_.Count, processedCount.ToString());

            backgroundWorker.RunWorkerAsync();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            processedCount = 0;

            for (int i = 0; i < targetNoteGuids_.Count; i++)
            {
                evernote_.DoAction(targetNoteGuids_[i], isMoveNotebook_, moveNotebook_, isAddTags_, addTags_);
                
                processedCount++;
                backgroundWorker.ReportProgress((i * 100 / targetNoteGuids_.Count));
            }
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbProcess.Value = e.ProgressPercentage;
            lblProgress.Text = string.Format(Resource.ProgressMsg, targetNoteGuids_.Count, processedCount.ToString());
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show(string.Format(Resource.ActionResultMsg, processedCount.ToString()), Resource.ActionResultMsgTitle);

            Close();
        }
    }
}
