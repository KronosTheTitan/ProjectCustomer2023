using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class HexTile : MonoBehaviour
    {
        public enum TileState
        {
            Neutral,
            Burning,
            Recovering,
            Empty
        }
        
        public List<HexTile> adjacentTiles;

        public TileType data;
        public TileState state = TileState.Empty;
        
        private void OnDrawGizmos()
        {
            foreach (HexTile hexTile in adjacentTiles)
            {
                if (hexTile.adjacentTiles.Contains(this))
                    Gizmos.color = Color.green ;
                else
                    Gizmos.color = Color.red;

                Gizmos.DrawLine(transform.position, hexTile.transform.position);
            }
        }
    }
}