using System;
using Map;

namespace Managers.BuildTools
{
    [Serializable]
    public class ExtinguisherTool : BuildTool
    {
        public override bool CanSelect()
        {
            if(GameManager.GetInstance().Difficulty.ExtinguishCost > GameManager.GetInstance().EconomyManager.Money) 
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
    }
}