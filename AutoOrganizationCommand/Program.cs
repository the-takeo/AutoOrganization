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

            FileStream fs = new FileStream(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + @"\AutoSettings.xml", FileMode.Open);

            Model model_ = (Model)serializer.ReadObject(fs);

            fs.Close();

            Console.ReadKey();

            foreach (var arg in args)
            {
                DataRow dr = model_.Presets.Rows.Find(arg);

                bool isMoveNotebook;
                string moveNotebook;
                bool isAddTags;
                string addTags;

                model_.ActionParams(arg, out isMoveNotebook, out moveNotebook, out isAddTags, out addTags);

                foreach (var noteGuid in model_.GetFilteredNoteGuids(arg))
                {
                    model_.Evernote.DoAction(noteGuid, isMoveNotebook, moveNotebook, isAddTags, addTags);
                }
            }
        }
    }
}
