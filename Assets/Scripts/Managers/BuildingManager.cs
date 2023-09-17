using System;
using Managers.BuildTools;
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
        public Tile targetedTile;

        [SerializeField] private TileType[] potentialTiles;
        [SerializeField] private TileType selectedTile;

        [SerializeField] private BulldozerTool bulldozer;
        [SerializeField] private ExtinguisherTool extinguisherTool;
        [SerializeField] private RandomTileTool randomTileTool;
        [SerializeField] private ReviveTileTool reviveTileTool;
        
        private BuildTool _selectedTool;

        private void Update()
        {
            if (_selectedTool == null)
                return;

            if (!Input.GetMouseButton(0))
                return;

            if (targetedTile == null)
                return;

            if (_selectedTool.UseTool(targetedTile))
                DeselectTool();
        }

        private void DeselectTool()
        {
            _selectedTool.OnDeselect();
            _selectedTool = null;
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
        }

        /// <summary>
        /// Places a selected tile on the targeted tile if it's empty.
        /// </summary>
        /// <param name="tile">The targeted hex tile to place the tile on.</param>
        public void PlaceTile(Tile tile)
        {
            if (tile == null)
                return;

            if (tile.state != TileState.Empty)
                return;
            
            tile.state = TileState.Neutral;
            tile.data = selectedTile;

            tile.UpdateGFX();
        }

        private void ExtinguishTile()
        {
        }
    }
}
