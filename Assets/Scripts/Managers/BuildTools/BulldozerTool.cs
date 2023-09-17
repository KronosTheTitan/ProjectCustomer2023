using System;
using Map;
using UnityEngine;

namespace Managers.BuildTools
{
    [Serializable]
    public class BulldozerTool : BuildTool
    {
        public override bool CanSelect()
        {
            if(GameManager.GetInstance().Difficulty.BulldozeCost > GameManager.GetInstance().EconomyManager.Money) 
                return false;

            return true;
        }

        public override bool UseTool(Tile target)
        {
            if (target.state != TileState.Neutral && target.state != TileState.Burned)
                return false;

            target.data = null;
            target.state = TileState.Empty;

            target.UpdateGFX();

            return true;
        }

        public override void OnDeselect()
        {
            
        }
    }
}