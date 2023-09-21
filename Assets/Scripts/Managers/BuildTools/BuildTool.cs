using System;
using System.Collections;
using Map;
using UnityEngine;
using UnityEngine.UI;

namespace Managers.BuildTools
{
    [Serializable]
    public abstract class BuildTool
    {
        [SerializeField] private Toggle toggleButton;
        [SerializeField] private int cost;
        [SerializeField] private float failFlashDuration = 2.0f;
        [SerializeField] private float failFlashInterval = 0.1f;
        [SerializeField] private Color failFlashColor = Color.red;
        public int Cost => cost;
        public abstract bool CanSelect();
        public abstract bool UseTool(Tile target);
        public abstract void OnDeselect();
        public abstract void Charge(Tile tile);

        /// <summary>
        /// Sets the toggle to on
        /// </summary>
        public void ToggleOn()
        {
            if (toggleButton != null)
            {
                toggleButton.isOn = true;
            }
        }

        /// <summary>
        /// Sets the toggle to off
        /// </summary>
        public void ToggleOff()
        {
            if (toggleButton != null)
            {
                toggleButton.isOn = false;
            }
        }

        /// <summary>
        /// Enables interaction with the toggle
        /// </summary>
        public void ToggleEnable()
        {
            if (toggleButton != null)
            {
                toggleButton.interactable = true;
            }
        }

        /// <summary>
        /// Disables interaction with the toggle
        /// </summary>
        public void ToggleDisable()
        {
            if (toggleButton != null)
            {
                toggleButton.interactable = false;
            }
        }

        /// <summary>
        /// Flashes toggle button with set fields.
        /// </summary>
        public void FlashToggle()
        {
            GameManager.GetInstance().UIManager.FlashUI(toggleButton.targetGraphic, failFlashColor, failFlashDuration, failFlashInterval);
        }
    }
}