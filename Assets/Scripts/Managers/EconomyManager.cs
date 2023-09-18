using System.Collections.Generic;
using Map;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// Manages the in-game economy, including money and campsite income.
    /// </summary>
    public class EconomyManager : MonoBehaviour
    {
        [SerializeField] private int money = 0;
        [SerializeField] private int naturePoints = 0;

        /// <summary>
        /// Gets the current amount of money.
        /// </summary>
        public int Money => money;

        public int NaturePoints => naturePoints;

        /// <summary>
        /// Receives income from campsites and deducts penalties for burning tiles.
        /// </summary>
        public void ReceiveIncome()
        {
            int deltaMoney = 0;
            TileManager tileManager = GameManager.GetInstance().TileManager;
            
            foreach (Tile tile in tileManager.Tiles)
            {
                if(tile.state != TileState.Neutral)
                    continue;
                
                deltaMoney += tile.data.revenue;
            }

            deltaMoney -= GameManager.GetInstance().FirePenalty * tileManager.BurningTiles.Count;

            money += deltaMoney;
        }

        public void ModifyMoney(int amount)
        {
            money += amount;
        }
        
        public void ModifyNaturePoints(int amount)
        {
            naturePoints += amount;
        }
        
        /// <summary>
        /// Removes a specified amount of money.
        /// </summary>
        /// <param name="amount">The amount of money to remove.</param>
        public void RemoveMoney(int amount)
        {
            money -= amount;
        }
    }
}
