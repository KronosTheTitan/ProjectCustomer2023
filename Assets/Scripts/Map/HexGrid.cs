using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Map
{
    public class HexGrid : MonoBehaviour
    {
        [SerializeField] private int width;
        [SerializeField] private int height;
    
        [SerializeField] private List<HexTile> _generatedTiles = new List<HexTile>();
        private HexTile[,] _grid;

        [SerializeField] private HexTile baseTile;
        [SerializeField] private float offsetXAxis = 0.866025f;
        [SerializeField] private float offsetYAxis = 1.5f;

        public void GenerateTiles()
        {
            ClearGrid();
        
            _grid = new HexTile[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector3 position;

                    if (x % 2f == 0f)
                    {
                        position = new Vector3(x * offsetXAxis, 0, y * offsetYAxis);
                    }
                    else
                    {
                        position = new Vector3(x * offsetXAxis, 0, y * offsetYAxis + offsetYAxis / 2);
                    }
                    
                    HexTile tile = Instantiate(baseTile, position, quaternion.identity ,transform);
                    _generatedTiles.Add(tile);

                    _grid[x, y] = tile;

                    try
                    {
                        ConnectTiles(tile,_grid[x - 1, y]);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    try
                    {
                        if (x % 2f == 0f)
                        {
                            ConnectTiles(tile, _grid[x - 1, y - 1]);
                        }
                        else
                        {
                            ConnectTiles(tile, _grid[x - 1, y + 1]);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    try
                    {
                        ConnectTiles(tile, _grid[x, y - 1]);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    tile.name = new Vector2(x, y).ToString();
                }
            }
        }

        public void ClearGrid()
        {
            while (_generatedTiles.Count > 0)
            {
                HexTile tile = _generatedTiles[0];
                _generatedTiles.Remove(_generatedTiles[0]);
                
                DestroyImmediate(tile.gameObject);
            }
        
            _grid = null;
            
            _generatedTiles.Clear();
        }

        private void ConnectTiles(HexTile a, HexTile b)
        {
            a.adjacentTiles.Add(b);
            b.adjacentTiles.Add(a);
        }
    }
}