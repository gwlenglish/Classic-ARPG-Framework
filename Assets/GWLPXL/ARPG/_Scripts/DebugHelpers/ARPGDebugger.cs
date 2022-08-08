
using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Types.com;
using UnityEngine;
using System.Text;
namespace GWLPXL.ARPGCore.DebugHelpers.com
{



    public static class ARPGDebugger
    {
        public static StringBuilder SB = new StringBuilder();
        public static void DebugMessage(StringBuilder message, Object context)
        {

            if (DungeonMaster.Instance == null) return;
            DebugMessage(message.ToString(), context);
            message.Clear();

        }
        public static void DebugMessage(string message, Object context)
        {

            if (DungeonMaster.Instance == null) return;
            switch (DungeonMaster.Instance.Debug)
            {
                case DebugMessages.Enabled:
                    if (context == null)
                    {
                        Debug.Log(message);
                    }
                    else
                    {
                        Debug.Log(message, context);
                    }
             
                    break;
            }

        }
        public static void CombatDebugMessage(string message, Object context)
        {

            if (DungeonMaster.Instance == null) return;
            switch (DungeonMaster.Instance.Debug)
            {
                case DebugMessages.Enabled:
                    if (context == null)
                    {
                        Debug.Log(message);
                    }
                    else
                    {
                        Debug.Log(message, context);
                    }
       
                    break;
            }

        }

        public static string GetColorForInventory(string toColorize)
        {
            return "<color=green>" + toColorize + "</color>";
        }
        public static string GetColorForError(string toColorize)
        {
            return "<color=yellow>" + toColorize + "</color>";
        }
        public static string GetColorForResist(string toColorize)
        {
            return "<color=blue>" + toColorize + "</color>";
        }
        public static string GetColorForDamage(string toColorize)
        {
            return "<color=red>" + toColorize + "</color>";
        }
        public static string GetColorForChain(string toColorize)
        {
            return "<color=yellow>" + toColorize + "</color>";
        }
        public static string GetColorForSOTs(string toColorize)
        {
            return "<color=brown >" + toColorize + "</color>";
        }
    }

}