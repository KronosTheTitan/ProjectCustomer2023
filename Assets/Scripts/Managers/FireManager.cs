using System.Collections.Generic;
using Map;
using UnityEngine;

namespace Managers
{
    public class FireManager : MonoBehaviour
    {
        [SerializeField] private List<HexTile> burningTiles = new List<HexTile>();

        public void SpreadFire()
        {
            List<HexTile> potentialFireSpreads = new List<HexTile>();

            foreach (HexTile hexTile in burningTiles)
            {
                foreach (HexTile option in hexTile.adjacentTiles)
                {
                    
                }
            }
        }
    }
}
