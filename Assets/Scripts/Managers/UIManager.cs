using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text moneyText;
        [SerializeField] private TMP_Text natureText;
        [SerializeField] private TMP_Text turnText;

        private Color originalColor;
        private Graphic coroutineUIElement;
        private Coroutine flashingCoroutine;

        void Update()
        {
            moneyText.text = GameManager.GetInstance().EconomyManager.Money.ToString();
            natureText.text = GameManager.GetInstance().EconomyManager.NaturePoints.ToString();
            turnText.text = GameManager.GetInstance().TurnNumber.ToString();
        }

        public void FlashUI(Graphic uiElement, Color flashColor, float duration = 2.0f, float flashInterval = 0.1f)
        {
            if (flashingCoroutine != null)
            {
                // Stop the previous coroutine and reset the color
                StopCoroutine(flashingCoroutine);
                coroutineUIElement.color = originalColor;
            }

            flashingCoroutine = StartCoroutine(FlashColor(uiElement, flashColor, duration, flashInterval));
        }

        private IEnumerator FlashColor(Graphic uiElement, Color flashColor, float duration, float flashInterval)
        {
            float startTime = Time.time;
            coroutineUIElement = uiElement;
            originalColor = uiElement.color;

            while (Time.time - startTime < duration)
            {
                float lerpFactor = Mathf.PingPong((Time.time - startTime) / flashInterval, 1.0f);
                uiElement.color = Color.Lerp(originalColor, flashColor, lerpFactor);

                yield return null;
            }

            uiElement.color = originalColor;
            flashingCoroutine = null;
        }
    }
}
