using GWLPXL.ARPGCore.Types.com;

namespace GWLPXL.ARPGCore.Items.com
{

    public abstract class SocketItem : Item
    {
        public SocketableVars SocketableVariables;

        public override string GetUserDescription()
        {
            return SocketableVariables.Description;
        }
        public virtual SocketTypes GetSocketType()
        {
            return SocketableVariables.Type;
        }
        public override string GetBaseItemName()
        {
            return SocketableVariables.BaseName;
        }

        public override string GetGeneratedItemName()
        {

            return SocketableVariables.GeneratedName;
        }

        public override ItemType GetItemType()
        {
            return ItemType.EquipmentSocketable;
        }

        public override int GetStackingAmount()
        {
            return SocketableVariables.StackingAmount;
        }


        public override bool IsStacking()
        {
            return SocketableVariables.StackingAmount > 1;
        }

        public override void SetGeneratedItemName(string newName)
        {

            SocketableVariables.GeneratedName = newName;
        }
    }

    [System.Serializable]
    public class SocketableVars
    {
        public string BaseName = string.Empty;
        public int StackingAmount = 5;
        [UnityEngine.HideInInspector]
        public string GeneratedName = string.Empty;
        public SocketTypes Type = SocketTypes.Any;
        [UnityEngine.TextArea(3,5)]
        public string Description = string.Empty;

        public SocketableVars(SocketTypes type, string basename, int stacking = 1)
        {
            Type = type;
            BaseName = basename;
            StackingAmount = stacking;
        }
    }
}