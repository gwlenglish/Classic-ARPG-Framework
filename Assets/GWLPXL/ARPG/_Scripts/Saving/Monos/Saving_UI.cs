using GWLPXL.ARPGCore.com;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace GWLPXL.ARPGCore.Saving.com
{


    public class Saving_UI : MonoBehaviour, ISaveCanvas
    {
        public IUseSaveCanvas MyUser { get; set; }
        public bool FreezeDungeon = true;
        #region fields
        [SerializeField]
        GameObject saveFileObjPrefab = null;
        [SerializeField]
        Transform saveFileObjParent = null;
        [SerializeField]
        GameObject mainPanel = null;
        [SerializeField]
        GameObject saveButtonObj = null;
        [SerializeField]
        GameObject loadConfirmObj = null;

        ISaveSystem saveSystem = null;
        string saveFileName = null;
        readonly string defaultName = "Save";
        int defaultNameAppend = 0;
        FileInfo activeFileInfo = null;
        Dictionary<FileInfo, GameObject> saveFiles = new Dictionary<FileInfo, GameObject>();

        #endregion

        #region private
        void Awake()
        {
            EnableMainPanel(false);
            saveSystem = GetComponent<ISaveSystem>();
           
            RefreshSaveFilesObjs();
            EnableConfirm(false);
        }

        //void Update()
        //{
        //    if (MyUser == null) return;
        //    if (MyUser.ToggleCanvas())
        //    {
        //        EnableMainPanel(!mainPanel.activeInHierarchy);
        //    }
        //}
        void EnableSaveFileObj(bool isEnabled)
        {
            saveButtonObj.SetActive(isEnabled);
        }

        void RefreshSaveFilesObjs()
        {
            if (saveSystem == null) return;

            foreach (var kvp in saveFiles)
            {
                if (kvp.Value != null)
                {
                    Destroy(kvp.Value);
                }
            }
            saveFiles.Clear();
            DirectoryInfo info = new DirectoryInfo(saveSystem.GetSaveDirectory());
            FileInfo[] fileInfo = info.GetFiles();
            foreach (FileInfo file in fileInfo)
            {
                saveFiles.TryGetValue(file, out GameObject value);
                if (value == null)
                {
                    GameObject saveObj = Instantiate(saveFileObjPrefab, saveFileObjParent);
                    SaveInfo save = saveObj.GetComponent<SaveInfo>();
                    save.SetSaveInfo(file, this);
                    saveFiles.Add(file, saveObj);
                    //ARPGDebugger.DebugMessage("Save file name: " + file.Name);

                }

            }
        }
        #endregion

        #region public
        public void DeleteActiveFile()
        {
            if (saveSystem == null) return;
            if (activeFileInfo != null)
            {

                DirectoryInfo info = new DirectoryInfo(saveSystem.GetSaveDirectory());
                string path = saveSystem.GetSaveDirectory() + "/" + activeFileInfo.Name + activeFileInfo.Extension;
                //ARPGDebugger.DebugMessage("To Delete Save file: " + path);
                if (File.Exists(path))
                {
                    File.Delete(path);
                    MakeActiveFile(null);
                    RefreshSaveFilesObjs();
                }

            }
        }
        public void ConfirmLoadSaveFile()
        {
            if (saveSystem == null) return;

            saveSystem.LoadGame(activeFileInfo.Name);
        }
        public void MakeActiveFile(FileInfo key)
        {
            activeFileInfo = key;
            if (activeFileInfo == null)
            {
                EnableConfirm(false);
            }

        }
        public void EnableMainPanel(bool isEnabled)
        {
            mainPanel.SetActive(isEnabled);
            if (FreezeDungeon && mainPanel.activeInHierarchy)
            {
                DungeonMaster.Instance.GetDungeonUISceneRef().SetDungeonState(0);
            }
            else if (FreezeDungeon && !mainPanel.activeInHierarchy)
            {
                DungeonMaster.Instance.GetDungeonUISceneRef().SetDungeonState(1);

            }

        }
        public void EnableConfirm(bool isEnabled)
        {
            loadConfirmObj.SetActive(isEnabled);
        }
        public void UpdateSaveFileName(string newName)
        {
            if (saveSystem == null) return;

            saveFileName = newName;
            if (string.IsNullOrEmpty(saveFileName))
            {
                saveSystem.SetSaveFileName(defaultName + "_" + defaultNameAppend.ToString());
            }
            else
            {
                saveSystem.SetSaveFileName(saveFileName);
            }
        }
        public void SaveGame()
        {
            if (saveSystem == null) return;
            if (saveSystem.IsSaving()) return;

            saveSystem.SaveGame(DungeonMaster.Instance.GetPlayerPersist());
            EnableConfirm(false);
            EnableSaveFileObj(false);
            StartCoroutine(WaitForSave());
        }

        #endregion
        IEnumerator WaitForSave()
        {
            yield return new WaitUntil(() => saveSystem.IsSaving() == false);
            RefreshSaveFilesObjs();
            EnableSaveFileObj(true);
        }

        public void SetUser(IUseSaveCanvas _user)
        {
            MyUser = _user;
        }

        public bool GetCanvasEnabled()
        {
            return mainPanel.activeInHierarchy;

        }

        public void TogglePlayerSaveCanvas()
        {
            EnableMainPanel(!mainPanel.activeInHierarchy);
            if (mainPanel.activeInHierarchy == true)
            {
               
            }
        }
    }
}