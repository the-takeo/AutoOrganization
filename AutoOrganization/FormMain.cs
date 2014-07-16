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

                FileStream fs = new FileStream(@"Settings.xml", FileMode.Open);

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

            if (model_.Actions.Rows.Count == 0)
                model_.AddNewAction();

            _refleshLbActions();

            lbAction.SelectedIndex = 0;

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void btnAddAction_Click(object sender, EventArgs e)
        {
            model_.AddNewAction();
            _refleshLbActions();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            model_.DeleteAction(lbAction.SelectedIndex);
            _refleshLbActions();
        }

        private void lbAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            isChangeSelectedBySystem = true;

            int selectedIndex = ((ListBox)sender).SelectedIndex;

            cbTargetNotebook.SelectedItem = model_.Actions.Rows[selectedIndex]["TargetNotebook"];
            tbTargetTags.Text = model_.Actions.Rows[selectedIndex]["TargetTags"].ToString();
            tbTargetUrl.Text = model_.Actions.Rows[selectedIndex]["TargetURL"].ToString();
            chbMoveToNotebook.Checked = (bool)model_.Actions.Rows[selectedIndex]["IsMoveNotebook"];
            cbMoteToNotebook.SelectedItem = model_.Actions.Rows[selectedIndex]["MoveToNotebook"];
            chbAddTags.Checked = (bool)model_.Actions.Rows[selectedIndex]["IsAddTags"];
            tbAddTags.Text = model_.Actions.Rows[selectedIndex]["AddTags"].ToString();

            isChangeSelectedBySystem = false;
        }

        private void cbTargetNotebook_SelectedIndexChanged(object sender, EventArgs e)
        {
            _updateAction();
        }

        private void chbMoveToNotebook_CheckedChanged(object sender, EventArgs e)
        {
            _updateAction();
        }

        private void cbMoteToNotebook_SelectedIndexChanged(object sender, EventArgs e)
        {
            _updateAction();
        }

        private void chbAddTags_CheckedChanged(object sender, EventArgs e)
        {
            _updateAction();
        }

        private void tbTargetUrl_Leave(object sender, EventArgs e)
        {
            _updateAction();
        }

        private void tbTargetTags_Leave(object sender, EventArgs e)
        {
            _updateAction();
        }

        private void tbAddTags_Leave(object sender, EventArgs e)
        {
            _updateAction();
        }

        private void btnDoSelectedAction_Click(object sender, EventArgs e)
        {
            string actionName=lbAction.SelectedItem.ToString();

            List<string> targetNoteGuids = model_.GetFilteredNoteGuids(actionName);

            if (MessageBox.Show(string.Format(Resource.ConfirmAction, targetNoteGuids.Count.ToString()), Resource.ConfirmActionTitle, MessageBoxButtons.OKCancel) != DialogResult.OK)
                return;

            bool isMoveNotebook;
            string moveNotebook;
            bool isAddTags;
            string addTags;

            model_.ActionParams(actionName, out isMoveNotebook, out moveNotebook, out isAddTags, out addTags);

            //既存Tagでない場合、Tagの追加を行う
            if (isAddTags)
            {
                foreach (var addTag in addTags.Split(','))
                {
                    if (model_.Evernote.Tags.ContainsKey(addTag) == false)
                        model_.Evernote.AddNewTag(addTag);
                }
            }

            ProcessDialog processDialog = new ProcessDialog(targetNoteGuids, isMoveNotebook, moveNotebook, isAddTags, addTags, model_.Evernote);
            processDialog.ShowDialog();
        }

        /// <summary>
        /// 指定したアクションを実行する
        /// </summary>
        /// <param name="dr">指定アクション情報</param>
        public void DoAction(DataRow dr)
        {
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

            //既存Tagでない場合、Tagの追加を行う
            if (isAddTags)
            {
                foreach (var addTag in addTags.Split(','))
                {
                    if (model_.Evernote.Tags.ContainsKey(addTag) == false)
                        model_.Evernote.AddNewTag(addTag);
                }
            }

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
        private void _refleshLbActions()
        {
            int selectedIndex = lbAction.SelectedIndex;

            lbAction.Items.Clear();
            for (int i = 0; i < model_.Actions.Rows.Count; i++)
            {
                lbAction.Items.Add(model_.Actions.Rows[i]["ID"].ToString());
            }

            if (lbAction.Items.Count == 0)
            {
                btnDeleteAction.Enabled = false;
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

            else if (selectedIndex >= lbAction.Items.Count)
                lbAction.SelectedIndex = selectedIndex - 1;
            else
                lbAction.SelectedIndex = selectedIndex;

            btnDeleteAction.Enabled = true;
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
        private void _updateAction()
        {
            if (isChangeSelectedBySystem)
                return;

            if (cbTargetNotebook.SelectedItem == null || cbMoteToNotebook.SelectedItem == null)
                return;

            model_.UpdateAction(lbAction.SelectedIndex, cbTargetNotebook.SelectedItem.ToString(),
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

            EvernoteOA oauth = new EvernoteOA(EvernoteOA.HostService.Production);

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

        private void lbAction_DoubleClick(object sender, EventArgs e)
        {
            int index = ((ListBox)sender).SelectedIndex;

            RenameDialog rd = new RenameDialog(model_.Actions.Rows[index]["ID"].ToString(), model_.IDs);
            rd.ShowDialog();

            if (rd.DialogResult == DialogResult.OK)
            {
                model_.RenameAction(index, rd.NewName);
                _refleshLbActions();
            }
        }

        private void abountApplicationAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox ab = new AboutBox();
            ab.ShowDialog();
        }
    }
}
