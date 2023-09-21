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

        [SerializeField] private AudioSource audioSource;

        private void Awake()
        {
            gameObject.SetActive(false);

            foreach (Tile tile in adjacentTiles)
            {
                if (tile.state != TileState.Empty)
                    gameObject.SetActive(true);
            }

            if (state != TileState.Empty)
                gameObject.SetActive(true);
        }

        private void Start()
        {
            GameManager.GetInstance().OnNextTurn += OnNextTurn;
        }

        /// <summary>
        /// Called when a new turn begins.
        /// </summary>
        public void OnNextTurn()
        {
            if(state != TileState.Burning)
                return;
            
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
                    GameManager.GetInstance().TileManager.InvokeOnRemovedTile();
                }
                UpdateGFX();
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
            PlayIgnitionSound();
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
            PlayExtinguishSound();
        }

        public void Revive()
        {
            if (state != TileState.Burned)
                return;
            stateLastChangedDuringTurn = GameManager.GetInstance().TurnNumber;

            state = TileState.Neutral;

            UpdateGFX();
            PlayPlaceSound();
        }

        /// <summary>
        /// Updates the visual representation of the tile.
        /// </summary>
        public void UpdateGFX()
        {

            if (state == TileState.Burning)
            {
                Destroy(gfx);
                gfx = Instantiate(data.gfxBurning, transform.position, quaternion.identity, transform);
            }

            if (state == TileState.Burned)
            {
                Destroy(gfx);
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

        public int GetNaturePoints()
        {
            int output = 0;

            foreach (Tile tile in adjacentTiles)
            {
                if(tile.state != TileState.Neutral)
                    continue;
                
                if(!tile.data.isNaturePointSource)
                    continue;

                output += data.natureYield;
            }
            
            return output;
        }

        public void PlayPlaceSound()
        {
            audioSource.clip = data.placeSound;
            audioSource.Play();
        }

        public void PlayIgnitionSound()
        {
            //audioSource.clip = data.igniteSound;
            //audioSource.Play();
        }

        public void PlayExtinguishSound()
        {
            audioSource.clip = data.extinguishSound;
            audioSource.Play();
        }
    }
}