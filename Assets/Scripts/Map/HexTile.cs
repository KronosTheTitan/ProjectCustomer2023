using System;
using System.Collections.Generic;
using Managers;
using Unity.Mathematics;
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
        public int stateLastChangedDuringTurn;

        public GameObject gfx;

        private void Start()
        {
            GameManager.GetInstance().OnNextTurn += OnNextTurn;

            gameObject.SetActive(false);
            
            foreach (HexTile tile in adjacentTiles)
            {
                if (tile.state != TileState.Empty)
                    gameObject.SetActive(true);
            }
            
            if(state != TileState.Empty)
                gameObject.SetActive(true);
        }

        public void OnNextTurn()
        {
            switch (state)
            {
                case TileState.Burning:
                    if (stateLastChangedDuringTurn + data.burnTime < GameManager.GetInstance().TurnNumber)
                    {
                        if (data.CanRecover)
                        {
                            Extinguish();
                        }
                        else
                        {
                            state = TileState.Empty;
                            data = GameManager.GetInstance().BuildingManager.Empty;
                        }
                        UpdateGFX();
                    }
                    break;
                case TileState.Recovering:
                    if (stateLastChangedDuringTurn + data.recoveryTime < GameManager.GetInstance().TurnNumber)
                    {
                        state = TileState.Neutral;
                        stateLastChangedDuringTurn = GameManager.GetInstance().TurnNumber;
                        UpdateGFX();
                    }
                    break;
            }
        }

        /// <summary>
        /// Lights a tile on fire if possible
        /// </summary>
        public void Ignite()
        {
            if(!data.IsBurnable)
                return;
            if(state == TileState.Burning)
                return;
            GameManager.GetInstance().FireManager.BurningTiles.Add(this);
            stateLastChangedDuringTurn = GameManager.GetInstance().TurnNumber;
            state = TileState.Burning;
            
            UpdateGFX();
        }

        public void Extinguish()
        {
            if (state != TileState.Burning)
                return;

            GameManager.GetInstance().FireManager.BurningTiles.Remove(this);
            stateLastChangedDuringTurn = GameManager.GetInstance().TurnNumber;

            if (data.CanRecover)
                state = TileState.Recovering;
            else
                state = TileState.Neutral;
            
            UpdateGFX();
        }

        public void UpdateGFX()
        {
            Destroy(gfx);

            if (state == TileState.Burning)
            {
                gfx = Instantiate(data.gfxBurning, transform.position,quaternion.identity , transform);
            }

            if (state == TileState.Recovering)
            {
                gfx = Instantiate(data.gfxRecovering, transform.position,quaternion.identity , transform);
            }

            if (state == TileState.Neutral || state == TileState.Empty)
            {
                gfx = Instantiate(data.gfxNeutral, transform.position,quaternion.identity , transform);
            }
            
            if (state != TileState.Empty)
                foreach (HexTile tile in adjacentTiles) 
                { 
                    tile.gameObject.SetActive(true); 
                }
        }
        
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