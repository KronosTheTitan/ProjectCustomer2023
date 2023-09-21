using System;
using Map;
using UnityEngine;
using UnityEngine.UI;
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

        [SerializeField] private Image buttonImage;
        [SerializeField] private Sprite unknownSprite;
        public override bool CanSelect()
        {
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

            if (!CanSelect()) // Check enough resources to place tile
            {
                FlashToggle();
                ToggleOff();
                return false;
            }

            target.state = TileState.Neutral;
            target.data = selectedTile;
            
            GameManager.GetInstance().EconomyManager.ModifyNaturePoints(target.GetNaturePoints());
            
            target.UpdateGFX();
            target.PlayPlaceSound();
            GameManager.GetInstance().TileManager.InvokeOnPlacedTile();

            tilesPlaced++;

            return true;
        }

        public override void OnDeselect()
        {
            selectedTile = null;
            buttonImage.sprite = unknownSprite;
            
            Debug.Log("Deselected the random tile tool");
        }

        public override void OnSelect()
        {
            selectedTile = potentialTiles[Random.Range(0, potentialTiles.Length - 1)];

            if (tilesPlaced % campsiteInterval == 0f)
                selectedTile = campsite;
            
            buttonImage.sprite = selectedTile.sprite;
        }

        public override void Charge(Tile tile)
        {
            GameManager.GetInstance().EconomyManager.ModifyMoney(-Cost);
        }
    }
}