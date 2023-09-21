﻿using System;
using Map;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers.BuildTools
{
    [Serializable]
    public class RandomTileTool : BuildTool
    {
        [SerializeField] private TileType[] potentialTiles;
        [SerializeField] private TileType selectedTile;
        [SerializeField] private TileType campsite;
        [SerializeField] private int campsiteInterval;
        [SerializeField] private int tilesPlaced;

        public override bool CanPlaceTile()
        {
            selectedTile = potentialTiles[Random.Range(0, potentialTiles.Length - 1)];

            if (tilesPlaced % campsiteInterval == 0f)
                selectedTile = campsite;
            
            int money = GameManager.GetInstance().EconomyManager.Money;
            if (money < Cost)
                return false;
            
            return true;
        }

        public override bool UseTool(Tile target)
        {
            if (target == null) // Check no tile
                return false;

            if (target.state != TileState.Empty) // Check empty tile
                return false;

            if (!CanPlaceTile()) // Check enough resources to place tile
            {
                FlashToggle();
                ToggleOff();
                return false;
            }

            target.state = TileState.Neutral;
            target.data = selectedTile;
            
            GameManager.GetInstance().EconomyManager.ModifyNaturePoints(target.GetNaturePoints());
            
            target.UpdateGFX();
            GameManager.GetInstance().TileManager.InvokeOnPlacedTile();

            tilesPlaced++;

            return true;
        }

        public override void OnDeselect()
        {
            selectedTile = null;
        }

        public override void Charge(Tile tile)
        {
            GameManager.GetInstance().EconomyManager.ModifyMoney(-Cost);
        }
    }
}