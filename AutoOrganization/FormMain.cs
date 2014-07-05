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
    public partial class FormMain : Form
    {
        Model model_;
        List<string> EvernoteNotebooks = new List<string>();

        public FormMain()
        {
            InitializeComponent();

            model_ = new Model();
            cbTargetNotebook.Items.AddRange(model_.Evernote.Notebooknames.ToArray());
            cbMoteToNotebook.Items.AddRange(model_.Evernote.Notebooknames.ToArray());

            if (model_.Presets.Rows.Count == 0)
                model_.AddNewPreset();

            _refleshLbPresets();

            lbPreset.SelectedIndex = 0;
        }

        private void _refleshLbPresets()
        {
            lbPreset.Items.Clear();
            for (int i = 0; i < model_.Presets.Rows.Count; i++)
			{
                lbPreset.Items.Add(model_.Presets.Rows[i]["ID"].ToString());
			}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            model_.Evernote.testNote();            
        }

        private void btnAddPreset_Click(object sender, EventArgs e)
        {
            model_.AddNewPreset();
            _refleshLbPresets();
        }

        private void lbPreset_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = ((ListBox)sender).SelectedIndex;

            cbTargetNotebook.SelectedItem = model_.Presets.Rows[selectedIndex]["TargetNotebook"];
            tbTargetTags.Text = model_.Presets.Rows[selectedIndex]["TargetTags"].ToString();
            chbMoveToNotebook.Checked = (bool)model_.Presets.Rows[selectedIndex]["IsMoveNotebook"];
            cbMoteToNotebook.SelectedItem = model_.Presets.Rows[selectedIndex]["MoveToNotebook"];
            chbAddTags.Checked = (bool)model_.Presets.Rows[selectedIndex]["IsAddTags"];
            tbAddTags.Text = model_.Presets.Rows[selectedIndex]["AddTags"].ToString();
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

        private void tbTargetTags_Leave(object sender, EventArgs e)
        {
            _updatePreset();
        }

        private void tbAddTags_Leave(object sender, EventArgs e)
        {
            _updatePreset();
        }

        private void _updatePreset()
        {
            if (cbTargetNotebook.SelectedItem == null || cbMoteToNotebook.SelectedItem == null)
                return;

            model_.UpdatePreset(lbPreset.SelectedIndex, cbTargetNotebook.SelectedItem.ToString(),
                tbTargetTags.Text, chbMoveToNotebook.Checked, cbMoteToNotebook.SelectedItem.ToString(),
                chbAddTags.Checked, tbAddTags.Text);
        }
    }
}
