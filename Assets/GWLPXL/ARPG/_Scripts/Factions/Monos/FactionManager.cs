using GWLPXL.ARPGCore.com;
using GWLPXL.ARPGCore.Saving.com;
using GWLPXL.ARPGCore.Types.com;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GWLPXL.ARPGCore.Factions.com
{


    public class FactionManager : MonoBehaviour
    {
        public FactionRelations FactionRelationsTemplate = null;
        FactionRelations runtime = null;
        public static FactionManager Instance => instance;
        static FactionManager instance;


        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                if (FactionRelationsTemplate == null)
                {
                    FactionRelationsTemplate = ScriptableObject.CreateInstance<FactionRelations>();
                }
                SetRuntime(FactionRelationsTemplate);
            }
            else
            {
                Destroy(this);
            }

        }

        void UpdateRelationTier(FactionValue forValue)
        {
            List<RelationshipTierData> _temp = new List<RelationshipTierData>();
            for (int i = 0; i < runtime.Tiers.Length; i++)
            {
                _temp.Add(runtime.Tiers[i].Data);
            }
            _temp.Sort((p1, p2) => p1.MinValue.CompareTo(p2.MinValue));
            for (int i = 0; i < _temp.Count; i++)
            {
                if (forValue.Value >= _temp[i].MinValue && forValue.Value < _temp[i].MaxValue)
                {
                    forValue.RelationTier = _temp[i];
                    break;
                }
            }
        }


        public void ModifyPlayerRep(RelationChange[] changes)
        {
            PlayerSceneReference[] players = DungeonMaster.Instance.GetAllSceneReferences();
            for (int i = 0; i < players.Length; i++)
            {
                IFactionMember faction = players[i].SceneRef.GetComponent<IFactionMember>();
                if (faction == null) continue;

                for (int j = 0; j < changes.Length; j++)
                {
                    if (changes[j].AmountToChange > 0)
                    {
                        faction.IncreaseRep(changes[j].ForFaction, changes[j].AmountToChange);
                    }
                    else if (changes[i].AmountToChange < 0)
                    {
                        faction.DecreaseRep(changes[j].ForFaction, changes[j].AmountToChange);
                    }
                }

            }

        }
        public int GetRepValue(FactionTypes actor, FactionTypes withFaction)
        {
            int value = 0;
            for (int i = 0; i < runtime.Relations.Length; i++)
            {
                if (runtime.Relations[i].Primary == actor)
                {
                    //found it
                    for (int j = 0; j < runtime.Relations[i].Values.Length; j++)
                    {
                        if (runtime.Relations[i].Values[j].Faction == withFaction)
                        {
                            //decrease it
                            value = runtime.Relations[i].Values[j].Value;
                            break;
                        }
                    }
                }
            }
            return value;
        }
        public void DecreaseFactionRep(FactionTypes actor, FactionTypes decreaseRepWith, int amountToDecrease)
        {
            for (int i = 0; i < runtime.Relations.Length; i++)
            {
                if (runtime.Relations[i].Primary == actor)
                {
                    //found it
                    for (int j = 0; j < runtime.Relations[i].Values.Length; j++)
                    {
                        if (runtime.Relations[i].Values[j].Faction == decreaseRepWith)
                        {
                            //decrease it
                            runtime.Relations[i].Values[j].Value -= amountToDecrease;
                            UpdateRelationTier(runtime.Relations[i].Values[j]);
                        }
                    }
                }
            }
        }

        public void IncreaseFactionRep(FactionTypes actor, FactionTypes increaseRepWith, int amountRepToGain)
        {
            for (int i = 0; i < runtime.Relations.Length; i++)
            {
                if (runtime.Relations[i].Primary == actor)
                {
                    //found it
                    for (int j = 0; j < runtime.Relations[i].Values.Length; j++)
                    {
                        if (runtime.Relations[i].Values[j].Faction == increaseRepWith)
                        {
                            //increase it
                            runtime.Relations[i].Values[j].Value += amountRepToGain;
                            UpdateRelationTier(runtime.Relations[i].Values[j]);

                        }
                    }
                }
            }
        }

        public void SetRuntime(FactionRelations template)
        {
            runtime = ScriptableObject.Instantiate(template);
        }
    }
}