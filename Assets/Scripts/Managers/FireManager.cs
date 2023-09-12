using System.Collections.Generic;
using Map;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// Manages the spread of fire in the game.
    /// </summary>
    public class FireManager : MonoBehaviour
    {
        [SerializeField] private List<HexTile> burningTiles = new List<HexTile>();
        [SerializeField] private List<HexTile> burnableTiles = new List<HexTile>();

        /// <summary>
        /// Gets a list of tiles that are currently burning.
        /// </summary>
        public List<HexTile> BurningTiles => burningTiles;

        /// <summary>
        /// Gets a list of tiles that are susceptible to burning.
        /// </summary>
        public List<HexTile> BurnableTiles => burnableTiles;

        /// <summary>
        /// Attempts to spread the fire to neighboring tiles.
        /// </summary>
        /// <returns>True if the fire spreads to at least one tile, false otherwise.</returns>
        public bool SpreadFire()
        {
            List<HexTile> potentialFireSpreads = new List<HexTile>();
            bool returnValue = false;

            List<HexTile> borderingTiles = new List<HexTile>();

            // Collect all tiles adjacent to burning tiles.
            foreach (HexTile tile in burningTiles)
            {
                borderingTiles.AddRange(tile.adjacentTiles);
            }

            // Identify tiles that are burnable.
            foreach (HexTile tile in borderingTiles)
            {
                if (tile.data.IsBurnable != true)
                    continue;

                potentialFireSpreads.Add(tile);
            }

            // Attempt to ignite tiles based on fire spread chances.
            foreach (HexTile tile in potentialFireSpreads)
            {
                if (tile.state == HexTile.TileState.Empty)
                    continue;
                if (tile.state == HexTile.TileState.Recovering)
                    continue;

                int roll = Random.Range(0, 100);
                if (roll >= GameManager.GetInstance().Difficulty.FireSpreadChances[tile.data])
                    continue;

                tile.Ignite();
                returnValue = true;
            }

            return returnValue;
        }

        /// <summary>
        /// Initiates a random fire on burnable tiles.
        /// </summary>
        public void StartRandomFire()
        {
            if (GameManager.GetInstance().TurnNumber < GameManager.GetInstance().Difficulty.GracePeriod)
                return;

            foreach (HexTile tile in burnableTiles)
            {
                int roll = Random.Range(0, 100);
                if (roll >= GameManager.GetInstance().Difficulty.RandomFireChance)
                    continue;

                tile.Ignite();
            }
        }
    }
}
