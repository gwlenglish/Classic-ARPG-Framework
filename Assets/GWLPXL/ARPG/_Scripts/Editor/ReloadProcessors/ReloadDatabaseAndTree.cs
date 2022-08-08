using System.Linq;
using GWLPXL.ARPG._Scripts.Editor.ArpgTree.com;
using GWLPXL.ARPG._Scripts.Editor.com;
using GWLPXL.ARPG._Scripts.Editor.Data.com;
using GWLPXL.ARPGCore.Saving.com;
using GWLPXL.ARPGCore.Statics.com;

namespace GWLPXL.ARPG._Scripts.Editor.ReloadProcessors.com
{
    public class ReloadDatabaseAndTree: IReloadProcessor
    {
        public void Reload()
        {
            DatabaseHandler.ReloadDatabase(Database.GetMyObject());
            var current = Tree.Childs.Skip(1).ToList();
            var newList = Enumerable.Range(0, Database.GetAllNames().Length)
                .Select(s => Database.GetDatabaseObjectBySlotIndex(s)).ToList();

            var deleted = current
                .Where(w => !newList.Contains(((ArpgItemDataContainer)w.DataContainer).Object)).ToList();
            var currentObjects = current.Select(s => ((ArpgItemDataContainer) s.DataContainer).Object).ToList();
            var added = newList.Where(w => !currentObjects.Contains(w)).ToList();
            
            Tree.Childs.RemoveAll(f => deleted.Contains(f));
            
            Root.AddToItemAsChildArray(Tree,
                added.Where(w => w != null)
                    .Select(s => new ArpgItemDataContainer(s, Database.GetSearchFolders()[0], this)).ToArray(),
                obj => ArpgEditorHelper.GetNameOfArpgObject(obj.Object),
                ArpgTreeData.DefaultDragAndDropFunc, ArpgTreeData.DefaultOnRightClickFunc);
        }

        public ArpgTreeItem Tree { get; set; }
        public IDatabase Database { get; set; }
        public ArpgTreeView Root { get; set; }
    }
}