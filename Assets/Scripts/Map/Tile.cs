using System;
using System.Collections.Generic;
using Managers;
using Unity.Mathematics;
using UnityEngine;

namespace Map
{
    /// <summary>
    /// The possible states of the hexagonal tile.
    /// </summary>
    public enum TileState
    {
        Neutral,
        Burning,
        Burned,
        Empty
    }

    /// <summary>
    /// Represents a hexagonal tile on the game map.
    /// </summary>
    public class Tile : MonoBehaviour
    {
        /// <summary>
        /// List of adjacent hexagonal tiles.
        /// </summary>
        public List<Tile> adjacentTiles;

        /// <summary>
        /// The data associated with this tile.
        /// </summary>
        public TileType data;

        /// <summary>
        /// The current state of the tile.
        /// </summary>
        public TileState state = TileState.Empty;

        /// <summary>
        /// The turn number during which the state was last changed.
        /// </summary>
        public int stateLastChangedDuringTurn;

        /// <summary>
        /// The visual representation of the tile.
        /// </summary>
        public GameObject gfx;

        private void Start()
        {
            GameManager.GetInstance().OnNextTurn += OnNextTurn;

            gameObject.SetActive(false);

            foreach (Tile tile in adjacentTiles)
            {
                if (tile.state != TileState.Empty)
                    gameObject.SetActive(true);
            }

            if (state != TileState.Empty)
                gameObject.SetActive(true);
        }

        /// <summary>
        /// Called when a new turn begins.
        /// </summary>
        public void OnNextTurn()
        {
            switch (state)
            {
                case TileState.Burning:
                    if (stateLastChangedDuringTurn + data.burnTime < GameManager.GetInstance().TurnNumber)
                    {
                        if (data.canRecover)
                        {
                            state = TileState.Burned;
                        }
                        else
                        {
                            state = TileState.Empty;
                            data = null;
                        }
                        UpdateGFX();
                    }
                    break;
            }
        }

        /// <summary>
        /// Lights a tile on fire if possible.
        /// </summary>
        public void Ignite()
        {
            if (!data.isBurnable)
                return;
            if (state == TileState.Burning)
                return;
            stateLastChangedDuringTurn = GameManager.GetInstance().TurnNumber;
            state = TileState.Burning;

            UpdateGFX();
        }

        /// <summary>
        /// Extinguishes the fire on the tile.
        /// </summary>
        public void Extinguish()
        {
            if (state != TileState.Burning)
                return;
            stateLastChangedDuringTurn = GameManager.GetInstance().TurnNumber;

            state = TileState.Neutral;

            UpdateGFX();
        }

        /// <summary>
        /// Updates the visual representation of the tile.
        /// </summary>
        public void UpdateGFX()
        {

            if (state == TileState.Burning)
            {
                gfx = Instantiate(data.gfxBurning, transform.position, quaternion.identity, transform);
            }

            if (state == TileState.Burned)
            {
                gfx = Instantiate(data.gfxBurned, transform.position, quaternion.identity, transform);
            }
            
            if (state == TileState.Neutral || state == TileState.Empty)
            {
                Destroy(gfx);
                gfx = Instantiate(data.gfxNeutral, transform.position, quaternion.identity, transform);
            }

            if (state != TileState.Empty)
                foreach (Tile tile in adjacentTiles) tile.gameObject.SetActive(true);
        }

        private void OnDrawGizmos()
        {
            foreach (Tile hexTile in adjacentTiles)
            {
                if (hexTile.adjacentTiles.Contains(this))
                    Gizmos.color = Color.green;
                else
                    Gizmos.color = Color.red;

                Gizmos.DrawLine(transform.position, hexTile.transform.position);
            }
        }

        private void OnMouseEnter()
        {
            GameManager.GetInstance().BuildingManager.targetedTile = this;
        }

        private void OnMouseExit()
        {
            GameManager.GetInstance().BuildingManager.targetedTile = null;
        }
    }
}
