
using System.IO;
using TMPro;
using UnityEngine;
namespace GWLPXL.ARPGCore.Saving.com
{


    public class SaveInfo : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI filetext = null;
        FileInfo myFile = null;
        Saving_UI ui = null;
        public void SetSaveInfo(FileInfo _myfile, Saving_UI _ui)
        {
            filetext.text = _myfile.Name;
            myFile = _myfile;
            ui = _ui;
        }

        public void MakeMyFileActive()
        {
            ui.MakeActiveFile(myFile);
            if (myFile != null)
            {
                ConfirmLoad();
            }
        }
        void ConfirmLoad()
        {
            ui.EnableConfirm(true);
        }
    }
}
