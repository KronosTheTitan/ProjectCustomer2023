using System.Collections.Generic;
using Map;
using UnityEngine;

namespace Managers
{
    public class TileManager : MonoBehaviour
    {
        [SerializeField] private List<Tile> tiles;
        public Tile[] Tiles => tiles.ToArray();
        
        [SerializeField] private List<Tile> burningTiles;
        public Tile[] BurningTiles => burningTiles.ToArray();
        
        /// <summary>
        /// Attempts to spread the fire to neighboring tiles.
        /// </summary>
        /// <returns>True if the fire spreads to at least one tile, false otherwise.</returns>
        public bool SpreadFire()
        {
            List<Tile> potentialFireSpreads = new List<Tile>();
            bool returnValue = false;

            List<Tile> borderingTiles = new List<Tile>();

            // Collect all tiles adjacent to burning tiles.
            foreach (Tile tile in burningTiles)
            {
                borderingTiles.AddRange(tile.adjacentTiles);
            }

            // Identify tiles that are burnable.
            foreach (Tile tile in borderingTiles)
            {
                if (tile.data.isBurnable != true)
                    continue;

                potentialFireSpreads.Add(tile);
            }

            // Attempt to ignite tiles based on fire spread chances.
            foreach (Tile tile in potentialFireSpreads)
            {
                if (tile.state == TileState.Empty)
                    continue;
                if (tile.state == TileState.Burning)
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

            foreach (Tile tile in tiles)
            {
                if(!tile.data.isBurnable)
                    continue;
                
                int roll = Random.Range(0, 100);
                if (roll >= GameManager.GetInstance().Difficulty.RandomFireChance)
                    continue;

                tile.Ignite();
            }
        }

        public int GetCampsiteNumber()
        {
            int output = 0;
            foreach (Tile tile in tiles)
            {
                if(tile.state != TileState.Neutral)
                    continue;
                
                if (tile.data.revenue != 0)
                    output++;
            }

            return output;
        }
    }
}