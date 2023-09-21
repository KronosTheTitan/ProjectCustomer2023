using System;
using Map;
using UnityEngine;

namespace Managers.BuildTools
{
    [Serializable]
    public class BulldozerTool : BuildTool
    {
        [SerializeField] private TileType emptyType;
        public override bool CanSelect()
        {
            if(Cost > GameManager.GetInstance().EconomyManager.Money) 
                return false;

            return true;
        }

        public override bool UseTool(Tile target)
        {
            if (target.state != TileState.Neutral && target.state != TileState.Burned)
                return false;

            target.data = emptyType;
            target.state = TileState.Empty;
            
            target.UpdateGFX();
            target.PlayPlaceSound();
            GameManager.GetInstance().TileManager.InvokeOnRemovedTile();

            return true;
        }

        public override void OnDeselect()
        {
            
        }

        public override void Charge(Tile tile)
        {
            GameManager.GetInstance().EconomyManager.ModifyMoney(-Cost);
        }
    }
}