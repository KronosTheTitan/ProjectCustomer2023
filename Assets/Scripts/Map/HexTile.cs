using System;
using System.Collections.Generic;
using UnityEngine;

public class HexTile : MonoBehaviour
{
    public List<HexTile> adjacentTiles;

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