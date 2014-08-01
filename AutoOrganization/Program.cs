using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoOrganization
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                try
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new FormMain());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            //引数がある場合、自動実行を行う。
            else
            {
                bool isAlert = true;

                if (args.Contains("-h"))
                    isAlert = false;

                try
                {
                    using (FileStream fs = new FileStream(@"Settings.xml", FileMode.Open))
                    {
                        DataContractSerializer serializer = new DataContractSerializer(typeof(Model));

                        Model model_ = (Model)serializer.ReadObject(fs);

                        //IDの存在確認
                        foreach (var arg in args)
                        {
                            if (model_.IDs.Contains(arg) == false)
                                throw new ApplicationException(string.Format(Resource.NotFoundActionID, arg));
                        }

                        foreach (var arg in args)
                        {
                            bool isMoveNotebook;
                            string moveNotebook;
                            bool isAddTags;
                            string addTags;

                            model_.GetActionParams(arg, out isMoveNotebook, out moveNotebook, out isAddTags, out addTags);

                            foreach (var noteGuid in model_.GetFilteredNoteGuidsByActionName(arg))
                            {
                                model_.Evernote.DoAction(noteGuid, isMoveNotebook, moveNotebook, isAddTags, addTags);
                            }
                        }
                    }

                    if (isAlert)
                        MessageBox.Show(Resource.AllActionDone);
                }
                catch(Exception ex)
                {
                    if (isAlert)
                        MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
