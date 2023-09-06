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

        public void ReceiveIncome()
        {
            foreach (HexTile hexTile in campsites)
            {
                money += GameManager.GetInstance().Difficulty.CampsiteIncome;
            }
        }
    }
}
