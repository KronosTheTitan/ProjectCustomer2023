using System;
using Map;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Managers
{
    /// <summary>
    /// Manages the placement and interaction of building tiles on the game map.
    /// </summary>
    public class BuildingManager : MonoBehaviour
    {
        /// <summary>
        /// The currently targeted hex tile.
        /// </summary>
        public HexTile targetedTile;

        [SerializeField] private TileType empty;
        [SerializeField] private TileType campsite;

        /// <summary>
        /// Gets the TileType for empty tiles.
        /// </summary>
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
            if (selectedOption == BuildOptions.None)
                return;

            if (!Input.GetMouseButton(0))
                return;

            if (targetedTile == null)
                return;

            switch (selectedOption)
            {
                case BuildOptions.PlaceTile:
                    PlaceTile(targetedTile);
                    break;
                case BuildOptions.BulldozeTile:
                    BulldozeTile();
                    break;
                case BuildOptions.ExtinguishTile:
                    ExtinguishTile();
                    break;
            }
        }

        /// <summary>
        /// Selects a random tile from the potential tiles list and prepares to place it.
        /// </summary>
        public void SelectPlaceRandomTile()
        {
            int money = GameManager.GetInstance().EconomyManager.Money;
            int cost = GameManager.GetInstance().Difficulty.TileCost;
            if (money < cost)
                return;

            GameManager.GetInstance().EconomyManager.RemoveMoney(cost);

            selectedTile = potentialTiles[Random.Range(0, potentialTiles.Length - 1)];
            selectedOption = BuildOptions.PlaceTile;
        }

        /// <summary>
        /// Selects the bulldozer tool to remove tiles.
        /// </summary>
        public void SelectBulldozer()
        {
            int money = GameManager.GetInstance().EconomyManager.Money;
            int cost = GameManager.GetInstance().Difficulty.BulldozeCost;
            if (money < cost)
                return;

            GameManager.GetInstance().EconomyManager.RemoveMoney(cost);

            selectedOption = BuildOptions.BulldozeTile;
        }

        /// <summary>
        /// Selects the extinguisher tool to extinguish burning tiles.
        /// </summary>
        public void SelectExtinguisher()
        {
            int money = GameManager.GetInstance().EconomyManager.Money;
            int cost = GameManager.GetInstance().Difficulty.ExtinguishCost;
            if (money < cost)
                return;

            GameManager.GetInstance().EconomyManager.RemoveMoney(cost);

            selectedOption = BuildOptions.ExtinguishTile;
        }

        /// <summary>
        /// Places a selected tile on the targeted tile if it's empty.
        /// </summary>
        /// <param name="tile">The targeted hex tile to place the tile on.</param>
        public void PlaceTile(HexTile tile)
        {
            if (tile == null)
                return;

            if (tile.state == TileState.Empty)
            {
                tile.state = TileState.Neutral;
                tile.data = selectedTile;

                if (tile.data.IsBurnable)
                    GameManager.GetInstance().FireManager.BurnableTiles.Add(tile);

                if (tile.data == campsite)
                    GameManager.GetInstance().EconomyManager.Campsites.Add(tile);

                tile.UpdateGFX();

                // Reset the selected option after placing the tile.
                selectedOption = BuildOptions.None;
            }
            else
            {
                if (tile.data.IsBurnable)
                    GameManager.GetInstance().FireManager.BurnableTiles.Add(tile);

                if (tile.data == campsite)
                    GameManager.GetInstance().EconomyManager.Campsites.Add(tile);
            }
        }

        private void BulldozeTile()
        {
            if (targetedTile.state != TileState.Neutral && targetedTile.state != TileState.Recovering)
                return;

            if (targetedTile.data == campsite)
                GameManager.GetInstance().EconomyManager.Campsites.Remove(targetedTile);

            if (targetedTile.data.IsBurnable)
                GameManager.GetInstance().FireManager.BurnableTiles.Remove(targetedTile);

            targetedTile.data = empty;
            targetedTile.state = TileState.Empty;

            targetedTile.UpdateGFX();

            selectedOption = BuildOptions.None;
        }

        private void ExtinguishTile()
        {
            if (targetedTile.state != TileState.Burning)
                return;

            targetedTile.Extinguish();
            selectedOption = BuildOptions.None;
        }
    }
}
