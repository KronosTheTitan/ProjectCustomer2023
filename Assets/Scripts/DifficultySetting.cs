using System;
using System.Collections.Generic;
using Map;
using UnityEngine;

[Serializable]
struct TileChancePair
{
    public TileType type;
    
    [Range(0,100)]
    public int chance;
}

[CreateAssetMenu(fileName = "New Difficulty", menuName = "ProjectCustomer/Difficulty")]
public class DifficultySetting : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] private int campsiteIncome;
    [SerializeField] private int firePenalty;
    [SerializeField] private int tileCost;
    [SerializeField] private int bulldozeCost;
    [SerializeField] private int extinguishCost;
    [SerializeField] private int gracePeriod;
    [SerializeField, Range(0, 100)] private int randomFireChance;
    public int CampsiteIncome => campsiteIncome;
    public int FirePenalty => firePenalty;
    public int TileCost => tileCost;
    public int BulldozeCost => bulldozeCost;
    public int ExtinguishCost => extinguishCost;
    public int RandomFireChance => randomFireChance;
    public int GracePeriod => gracePeriod;

    [SerializeField] private List<TileChancePair> fireSpreadChancesSetup;
    public readonly Dictionary<TileType, int> FireSpreadChances = new Dictionary<TileType, int>();
    
    public void OnBeforeSerialize()
    {
        
    }

    public void OnAfterDeserialize()
    {
        foreach (TileChancePair pair in fireSpreadChancesSetup)
        {
            if(FireSpreadChances.ContainsKey(pair.type))
                continue;
            FireSpreadChances.Add(pair.type, pair.chance);
        }
    }
}