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

        /// <summary>
        /// Gets the current amount of money.
        /// </summary>
        public int Money => money;

        [SerializeField] private List<HexTile> campsites;

        /// <summary>
        /// Gets the list of campsites.
        /// </summary>
        public List<HexTile> Campsites => campsites;

        /// <summary>
        /// Receives income from campsites and deducts penalties for burning tiles.
        /// </summary>
        public void ReceiveIncome()
        {
            if (campsites.Count == 0)
                return;

            foreach (HexTile hexTile in campsites)
            {
                if (hexTile.state != HexTile.TileState.Neutral)
                    continue;
                money += GameManager.GetInstance().Difficulty.CampsiteIncome;
            }

            foreach (HexTile tile in GameManager.GetInstance().FireManager.BurningTiles)
            {
                money -= GameManager.GetInstance().Difficulty.FirePenalty;
            }
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
