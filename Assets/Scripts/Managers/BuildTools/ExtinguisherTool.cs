using System;
using Map;
using UnityEngine;

namespace Managers.BuildTools
{
    [Serializable]
    public class ExtinguisherTool : BuildTool
    {
        public override bool CanSelect()
        {
            if(Cost > GameManager.GetInstance().EconomyManager.Money) 
                return false;

            return true;
        }

        public override bool UseTool(Tile target)
        {
            if (target.state != TileState.Burning)
                return false;

            target.Extinguish();

            return true;
        }

        public override void OnDeselect()
        {
            
        }

        public override void Charge(Tile tile)
        {
            int localCost = Cost;
            foreach (Tile neighbor in tile.adjacentTiles)
            {
                if (neighbor.data.isWaterSource)
                {
                    localCost /= 2;
                    break;
                }
            }
            GameManager.GetInstance().EconomyManager.ModifyMoney(-localCost);
        }
    }
}