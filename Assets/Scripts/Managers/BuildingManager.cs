using System;
using Map;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Managers
{
    public class BuildingManager : MonoBehaviour
    {
        public HexTile targetedTile;
        [SerializeField] private TileType empty;
        [SerializeField] private TileType campsite;
        public TileType Empty => empty;

        [SerializeField] private TileType[] potentialTiles;
        [SerializeField] private TileType selectedTile;
        
        private enum BuildOptions
        {
            PlaceTile,
            BulldozeTile,
            ExtinguishTile,
            None
        }

        [SerializeField] private BuildOptions selectedOption = BuildOptions.None;
        
        private void Update()
        {
            if(selectedOption == BuildOptions.None)
                return;
            
            if(!Input.GetMouseButton(0))
                return;
            
            if(targetedTile == null)
                return;

            switch (selectedOption)
            {
                case BuildOptions.PlaceTile:
                    PlaceTile();
                    break;
                case BuildOptions.BulldozeTile:
                    BulldozeTile();
                    break;
                case BuildOptions.ExtinguishTile:
                    ExtinguishTile();
                    break;
            }
        }

        public void SelectPlaceRandomTile()
        {
            int money = GameManager.GetInstance().EconomyManager.Money;
            int cost = GameManager.GetInstance().Difficulty.TileCost;
            if(money < cost)
                return;
            
            GameManager.GetInstance().EconomyManager.RemoveMoney(cost);
            
            selectedTile = potentialTiles[Random.Range(0, potentialTiles.Length - 1)];
            selectedOption = BuildOptions.PlaceTile;
        }

        public void SelectBulldozer()
        {
            int money = GameManager.GetInstance().EconomyManager.Money;
            int cost = GameManager.GetInstance().Difficulty.BulldozeCost;
            if(money < cost)
                return;
            
            GameManager.GetInstance().EconomyManager.RemoveMoney(cost);
            
            selectedOption = BuildOptions.BulldozeTile;
        }

        public void SelectExtinguisher()
        {
            int money = GameManager.GetInstance().EconomyManager.Money;
            int cost = GameManager.GetInstance().Difficulty.ExtinguishCost;
            if(money < cost)
                return;
            
            GameManager.GetInstance().EconomyManager.RemoveMoney(cost);
            
            selectedOption = BuildOptions.ExtinguishTile;
        }

        private void PlaceTile()
        {
            if(targetedTile.state != HexTile.TileState.Empty)
                return;
            
            targetedTile.state = HexTile.TileState.Neutral;
            targetedTile.data = selectedTile;

            if(targetedTile.data.IsBurnable)
                GameManager.GetInstance().FireManager.BurnableTiles.Add(targetedTile);
            
            if (targetedTile.data == campsite)
                GameManager.GetInstance().EconomyManager.Campsites.Add(targetedTile);
            
            targetedTile.UpdateGFX();

            selectedOption = BuildOptions.None;
        }
        
        private void BulldozeTile()
        {
            if(targetedTile.state != HexTile.TileState.Neutral && targetedTile.state != HexTile.TileState.Recovering)
                return;

            if (targetedTile.data == campsite)
                GameManager.GetInstance().EconomyManager.Campsites.Remove(targetedTile);

            if(targetedTile.data.IsBurnable)
                GameManager.GetInstance().FireManager.BurnableTiles.Remove(targetedTile);
            
            targetedTile.data = empty;
            targetedTile.state = HexTile.TileState.Empty;
            
            targetedTile.UpdateGFX();

            selectedOption = BuildOptions.None;
        }

        private void ExtinguishTile()
        {
            if(targetedTile.state != HexTile.TileState.Burning)
                return;
            
            targetedTile.Extinguish();
            selectedOption = BuildOptions.None;
        }
    }
}