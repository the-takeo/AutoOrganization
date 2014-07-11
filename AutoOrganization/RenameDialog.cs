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
    public partial class RenameDialog : Form
    {
        DataRow dr_;
        List<string> names_;
        string oldName_;

        public string NewName;

        public RenameDialog(string oldName,List<string> names)
        {
            InitializeComponent();

            this.ControlBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            tbOldName.Text = oldName;

            oldName_ = oldName;
            names_ = names;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string newName = tbNewname.Text;

            if (names_.Contains(newName))
            {
                MessageBox.Show(Resource.StillExistID);
                return;
            }

            NewName = newName;
            this.DialogResult = DialogResult.OK;

            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        
    }
}
