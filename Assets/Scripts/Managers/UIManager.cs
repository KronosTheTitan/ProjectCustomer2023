using TMPro;
using UnityEngine;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text moneyText;
        
        void Update()
        {
            moneyText.text = "$" + GameManager.GetInstance().EconomyManager.Money;
        }
    }
}
