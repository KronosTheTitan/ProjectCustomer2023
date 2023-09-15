using Managers;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Map
{
    /// <summary>
    /// Represents a hexagonal grid.
    /// </summary>
    public class HexGrid : MonoBehaviour
    {
        public List<HexTile> randomTilesToGenerate = new List<HexTile>();

        [SerializeField] private int width;
        [SerializeField] private int height;

        [SerializeField] private List<HexTile> _generatedTiles = new List<HexTile>();
        private HexTile[,] _grid;

        [SerializeField] private HexTile baseTile;

        [SerializeField] private float offsetXAxis = 0.866025f; // Horizontal offset between hex tiles.
        [SerializeField] private float offsetYAxis = 1.5f;     // Vertical offset between hex tiles.

        private void Start ()
        {
            foreach (HexTile tile in _generatedTiles)
            {
                GameManager.GetInstance().BuildingManager.PlaceTile(tile);
            }
        }

        /// <summary>
        /// Generates hexagonal tiles and populates the grid.
        /// </summary>
        public void GenerateTiles()
        {
            GenerateGrid(() => baseTile);
        }

        /// <summary>
        /// Generates hexagonal tiles with random specified tile types and populates the grid.
        /// </summary>
        public void GenerateRandomGrid()
        {
            GenerateGrid(() => randomTilesToGenerate[Random.Range(0, randomTilesToGenerate.Count)]);
        }

        /// <summary>
        /// Generates a hexagonal grid of tiles and populates it using the provided tile factory.
        /// </summary>
        /// <param name="tileFactory">A delegate that creates a HexTile instance.</param>
        private void GenerateGrid(Func<HexTile> tileFactory)
        {
            ClearGrid();
            _grid = new HexTile[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector3 position;

                    // Calculate the position of the tile based on hexagonal grid coordinates.
                    if (x % 2f == 0f)
                    {
                        position = new Vector3(x * offsetXAxis, 0, y * offsetYAxis);
                    }
                    else
                    {
                        position = new Vector3(x * offsetXAxis, 0, y * offsetYAxis + offsetYAxis / 2);
                    }

                    // Create a new HexTile using the provided factory method.
                    HexTile tile = Instantiate(tileFactory(), position, Quaternion.identity, transform);
                    _generatedTiles.Add(tile);

                    _grid[x, y] = tile;

                    // Connect the neighboring tiles.
                    TryConnectTiles(tile, x, y, x % 2f == 0f);

                    // Set the name of the tile based on its grid coordinates.
                    tile.name = new Vector2(x, y).ToString();
                }
            }
        }

        /// <summary>
        /// Tries to connect the given tile with its adjacent tiles.
        /// </summary>
        /// <param name="tile">The tile to connect.</param>
        /// <param name="x">The x-coordinate of the tile in the grid.</param>
        /// <param name="y">The y-coordinate of the tile in the grid.</param>
        /// <param name="isEvenColumn">Indicates if the current column is even.</param>
        private void TryConnectTiles(HexTile tile, int x, int y, bool isEvenColumn)
        {
            try
            {
                // Connect the tile with its left neighbor.
                ConnectTiles(tile, _grid[x - 1, y]);

                if (isEvenColumn)
                {
                    // Connect the tile with its top-left neighbor in even columns.
                    ConnectTiles(tile, _grid[x - 1, y - 1]);
                }
                else
                {
                    // Connect the tile with its top-right neighbor in odd columns.
                    ConnectTiles(tile, _grid[x - 1, y + 1]);
                }

                // Connect the tile with its top neighbor.
                ConnectTiles(tile, _grid[x, y - 1]);
            }
            catch (Exception e)
            {
                // Handle any exceptions that may occur during connection (e.g., out-of-bounds).
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Clears the grid and destroys generated tiles.
        /// </summary>
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

        /// <summary>
        /// Connects two hexagonal tiles by adding them to each other's adjacent tiles list.
        /// </summary>
        /// <param name="a">The first hexagonal tile.</param>
        /// <param name="b">The second hexagonal tile.</param>
        private void ConnectTiles(HexTile a, HexTile b)
        {
            a.adjacentTiles.Add(b);
            b.adjacentTiles.Add(a);
        }
    }
}
