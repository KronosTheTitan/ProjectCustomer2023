using TMPro;
using UnityEngine;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text moneyText;
        [SerializeField] private TMP_Text natureText;
        
        void Update()
        {
            moneyText.text = "$" + GameManager.GetInstance().EconomyManager.Money;
            natureText.text = GameManager.GetInstance().EconomyManager.NaturePoints.ToString();
        }
    }
}
