using System.Collections.Generic;
using Map;
using UnityEngine;

namespace Managers
{
    public class FireManager : MonoBehaviour
    {
        [SerializeField] private List<HexTile> burningTiles = new List<HexTile>();
        [SerializeField] private List<HexTile> burnableTiles = new List<HexTile>();
        public List<HexTile> BurningTiles => burningTiles;
        public List<HexTile> BurnableTiles => burnableTiles;

        public bool SpreadFire()
        {
            List<HexTile> potentialFireSpreads = new List<HexTile>();

            bool returnValue = false;

            List<HexTile> borderingTiles = new List<HexTile>();
            
            foreach (HexTile tile in burningTiles)
            {
                borderingTiles.AddRange(tile.adjacentTiles);
            }
            
            foreach (HexTile tile in borderingTiles)
            {
                if(tile.data.IsBurnable != true)
                    continue;
                    
                potentialFireSpreads.Add(tile);
            }

            foreach (HexTile tile in potentialFireSpreads)
            {
                if(tile.state == HexTile.TileState.Empty)
                    continue;
                if(tile.state == HexTile.TileState.Recovering)
                    continue;
                
                
                int roll = Random.Range(0, 100);
                if (roll >= GameManager.GetInstance().Difficulty.FireSpreadChances[tile.data])
                    continue;
                
                tile.Ignite(); 
                returnValue = true;
                
            }

            return returnValue;
        }

        public void StartRandomFire()
        {
            if(GameManager.GetInstance().TurnNumber < GameManager.GetInstance().Difficulty.GracePeriod)
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