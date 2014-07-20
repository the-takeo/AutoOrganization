using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using AutoOrganization;
using System.IO;

namespace AutoOrganizationCommand
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Runtime.Serialization.DataContractSerializer serializer = new System.Runtime.Serialization.DataContractSerializer(typeof(Model));

            FileStream fs = new FileStream(@"Settings.xml", FileMode.Open);

            Model model_ = (Model)serializer.ReadObject(fs);

            fs.Close();

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
    }
}
