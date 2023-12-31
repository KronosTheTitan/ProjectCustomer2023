﻿using System;
using Map;
using UnityEngine;

namespace Managers.BuildTools
{
    [Serializable]
    public class ReviveTileTool : BuildTool
    {
        public override bool CanSelect()
        {
            if(Cost > GameManager.GetInstance().EconomyManager.NaturePoints) 
                return false;

            return true;
        }

        public override bool UseTool(Tile target)
        {
            if (target.state != TileState.Burning)
                return false;

            if (!CanSelect())
            {
                FlashToggle();
                ToggleOff();
                return false;
            }

            target.Revive();

            return true;
        }

        public override void OnDeselect()
        {

        }

        public override void OnSelect()
        {
            
        }

        public override void Charge(Tile tile)
        {
            GameManager.GetInstance().EconomyManager.ModifyNaturePoints(-Cost);
        }
    }
}