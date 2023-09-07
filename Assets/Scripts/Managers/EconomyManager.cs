using System.Collections.Generic;
using Map;
using UnityEngine;

namespace Managers
{
    public class EconomyManager : MonoBehaviour
    {
        [SerializeField] private int money = 0;

        public int Money => money;

        [SerializeField] private List<HexTile> campsites;
        public List<HexTile> Campsites => campsites;

        public void ReceiveIncome()
        {
            if(campsites.Count == 0)
                return;
            
            foreach (HexTile hexTile in campsites)
            {
                if(hexTile.state != HexTile.TileState.Neutral)
                    continue;
                money += GameManager.GetInstance().Difficulty.CampsiteIncome;
            }

            foreach (HexTile tile in GameManager.GetInstance().FireManager.BurningTiles)
            {
                money -= GameManager.GetInstance().Difficulty.FirePenalty;
            }
        }

        public void RemoveMoney(int amount)
        {
            money -= amount;
        }
    }
}