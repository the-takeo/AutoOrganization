using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoOrganization
{
    public partial class FormMain : Form
    {
        Model model_;
        List<string> EvernoteNotebooks = new List<string>();

        bool isChangeSelectedBySystem = false;

        public FormMain()
        {
            InitializeComponent();

            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == false)
                MessageBox.Show(Resource.OffLineMsg);

            try
            {
                System.Runtime.Serialization.DataContractSerializer serializer = new System.Runtime.Serialization.DataContractSerializer(typeof(Model));

                FileStream fs = new FileStream(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + @"\AutoSettings.xml", FileMode.Open);

                model_ = (Model)serializer.ReadObject(fs);

                fs.Close();
            }

            //ライブラリデータが存在しない場合、新たにログインしライブラリデータを作成する
            catch
            {
                string token;

                if (loginEvernote(out token))
                    model_ = new Model(token);
                else
                    Close();
            }

            try
            {
                //接続確認
                var userName = model_.Evernote.GetEvernoteUserName;

            }

            //接続ができない場合、再度ログインし、ライブラリデータのログイン情報のみ更新する
            catch
            {
                string token;

                if (loginEvernote(out token))
                {
                    model_.EvernoteAuthToken = token;
                    model_.RefleshEvernoteClass();
                }
                else
                {
                    Close();
                }
            }

            cbTargetNotebook.Items.AddRange(model_.Evernote.Notebooknames.ToArray());
            cbMoteToNotebook.Items.AddRange(model_.Evernote.Notebooknames.ToArray());

            if (model_.Presets.Rows.Count == 0)
                model_.AddNewPreset();

            _refleshLbPresets();

            lbPreset.SelectedIndex = 0;

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void btnAddPreset_Click(object sender, EventArgs e)
        {
            model_.AddNewPreset();
            _refleshLbPresets();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            model_.DeletePreset(lbPreset.SelectedIndex);
            _refleshLbPresets();
        }

        private void lbPreset_SelectedIndexChanged(object sender, EventArgs e)
        {
            isChangeSelectedBySystem = true;

            int selectedIndex = ((ListBox)sender).SelectedIndex;

            cbTargetNotebook.SelectedItem = model_.Presets.Rows[selectedIndex]["TargetNotebook"];
            tbTargetTags.Text = model_.Presets.Rows[selectedIndex]["TargetTags"].ToString();
            tbTargetUrl.Text = model_.Presets.Rows[selectedIndex]["TargetURL"].ToString();
            chbMoveToNotebook.Checked = (bool)model_.Presets.Rows[selectedIndex]["IsMoveNotebook"];
            cbMoteToNotebook.SelectedItem = model_.Presets.Rows[selectedIndex]["MoveToNotebook"];
            chbAddTags.Checked = (bool)model_.Presets.Rows[selectedIndex]["IsAddTags"];
            tbAddTags.Text = model_.Presets.Rows[selectedIndex]["AddTags"].ToString();

            isChangeSelectedBySystem = false;
        }

        private void cbTargetNotebook_SelectedIndexChanged(object sender, EventArgs e)
        {
            _updatePreset();
        }

        private void chbMoveToNotebook_CheckedChanged(object sender, EventArgs e)
        {
            _updatePreset();
        }

        private void cbMoteToNotebook_SelectedIndexChanged(object sender, EventArgs e)
        {
            _updatePreset();
        }

        private void chbAddTags_CheckedChanged(object sender, EventArgs e)
        {
            _updatePreset();
        }

        private void tbTargetUrl_Leave(object sender, EventArgs e)
        {
            _updatePreset();
        }

        private void tbTargetTags_Leave(object sender, EventArgs e)
        {
            _updatePreset();
        }

        private void tbAddTags_Leave(object sender, EventArgs e)
        {
            _updatePreset();
        }

        private void btnDoSelectedAction_Click(object sender, EventArgs e)
        {
            DataRow dr = model_.Presets.Rows[lbPreset.SelectedIndex];

            string targetNotebook = dr["TargetNotebook"].ToString();
            string targetTags = dr["TargetTags"].ToString();
            string targetURL = dr["TargetURL"].ToString();

            bool isMoveNotebook = (bool)dr["IsMoveNotebook"];
            string moveNotebook = dr["MoveToNotebook"].ToString();
            bool isAddTags = (bool)dr["IsAddTags"];
            string addTags = dr["AddTags"].ToString();

            List<string> targetNoteGuids = model_.Evernote.GetFilteredNoteGuids(targetNotebook, targetTags, targetURL);

            if (MessageBox.Show(string.Format(Resource.ConfirmAction, targetNoteGuids.Count.ToString()), Resource.ConfirmActionTitle, MessageBoxButtons.OKCancel) != DialogResult.OK)
                return;

            ProcessDialog processDialog = new ProcessDialog(targetNoteGuids, isMoveNotebook, moveNotebook, isAddTags, addTags, model_.Evernote);
            processDialog.ShowDialog();
        }

        private void logInIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string token;
            loginEvernote(out token);
        }

        /// <summary>
        /// ActionリストをModelに基づいて更新する
        /// </summary>
        private void _refleshLbPresets()
        {
            int selectedIndex = lbPreset.SelectedIndex;

            lbPreset.Items.Clear();
            for (int i = 0; i < model_.Presets.Rows.Count; i++)
            {
                lbPreset.Items.Add(model_.Presets.Rows[i]["ID"].ToString());
            }

            if (lbPreset.Items.Count == 0)
            {
                btnDelete.Enabled = false;
                cbTargetNotebook.Enabled = false;
                tbTargetTags.Enabled = false;
                tbTargetUrl.Enabled = false;
                cbMoteToNotebook.Enabled = false;
                chbMoveToNotebook.Enabled = false;
                tbAddTags.Enabled = false;
                chbAddTags.Enabled = false;
                btnDoSelectedAction.Enabled = false;
                return;
            }

            else if (selectedIndex >= lbPreset.Items.Count)
                lbPreset.SelectedIndex = selectedIndex - 1;
            else
                lbPreset.SelectedIndex = selectedIndex;

            btnDelete.Enabled = true;
            cbTargetNotebook.Enabled = true;
            tbTargetTags.Enabled = true;
            tbTargetUrl.Enabled = true;
            cbMoteToNotebook.Enabled = true;
            chbMoveToNotebook.Enabled = true;
            tbAddTags.Enabled = true;
            chbAddTags.Enabled = true;
            btnDoSelectedAction.Enabled = true;
        }

        /// <summary>
        /// 選択されているActionを入力されている情報に基いて更新する
        /// </summary>
        private void _updatePreset()
        {
            if (isChangeSelectedBySystem)
                return;

            if (cbTargetNotebook.SelectedItem == null || cbMoteToNotebook.SelectedItem == null)
                return;

            model_.UpdatePreset(lbPreset.SelectedIndex, cbTargetNotebook.SelectedItem.ToString(),
                tbTargetTags.Text,tbTargetUrl.Text, chbMoveToNotebook.Checked, cbMoteToNotebook.SelectedItem.ToString(),
                chbAddTags.Checked, tbAddTags.Text);
        }

        /// <summary>
        /// Evernoteにログインする
        /// </summary>
        /// <param name="token">EvernoteToken</param>
        /// <returns>ログインの成否</returns>
        private bool loginEvernote(out string token)
        {
            token = string.Empty;

            EvernoteOA oauth = new EvernoteOA(EvernoteOA.HostService.Sandbox);

            if (oauth.doAuth(ConsumerKey.consumerKey, ConsumerKey.consumerSecret))
            {
                token = oauth.OAuthToken;
                return true;
            }
            else
                return false;
        }

        private void closeCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void lbPreset_DoubleClick(object sender, EventArgs e)
        {
            int index = ((ListBox)sender).SelectedIndex;

            RenameDialog rd = new RenameDialog(model_.Presets.Rows[index]["ID"].ToString(), model_.IDs);
            rd.ShowDialog();

            if (rd.DialogResult == DialogResult.OK)
            {
                model_.RenameAction(index, rd.NewName);
                _refleshLbPresets();
            }
        }

        private void abountApplicationAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox ab = new AboutBox();
            ab.ShowDialog();
        }
    }
}
