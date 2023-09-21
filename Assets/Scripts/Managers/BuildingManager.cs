using Managers.BuildTools;
using Map;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Managers
{
    /// <summary>
    /// Manages the placement and interaction of building tiles on the game map.
    /// </summary>
    public class BuildingManager : MonoBehaviour
    {
        /// <summary>
        /// The currently targeted hex tile.
        /// </summary>
        public Tile targetedTile;

        [SerializeField] private BulldozerTool bulldozerTool;
        [SerializeField] private ExtinguisherTool extinguisherTool;
        [SerializeField] private RandomTileTool randomTileTool;
        [SerializeField] private ReviveTileTool reviveTileTool;

        private BuildTool _selectedTool;

        private void Start ()
        {
            GameManager.GetInstance().OnNextTurn += DeselectToolCancel;
        }

        private void Update()
        {
            GetTargetedTile();
            
            if (_selectedTool == null) // No tool currently selected
                return;

            if (Input.GetKeyUp(KeyCode.Escape))
            {
                DeselectToolCancel();
                return;
            }

            if (!Input.GetMouseButton(0)) // No mouse input
                return;

            if (targetedTile == null) // No tile targeted by mouse
                return;

            if (_selectedTool.UseTool(targetedTile))
                DeselectToolSuccess();
        }

        private void GetTargetedTile()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            targetedTile = null;

            if(!Physics.Raycast(ray, out hit))
                return;

            Tile target = hit.collider.gameObject.GetComponent<Tile>();

            if (target == null)
                return;

            targetedTile = target;
        }

        private void DeselectToolCancel()
        {
            if (_selectedTool == null)
                return;

            _selectedTool.OnDeselect();
            _selectedTool.ToggleOff();
            _selectedTool = null;
            Debug.Log("deselect cancel");
        }

        private void DeselectToolSuccess()
        {
            _selectedTool.Charge(targetedTile);
            _selectedTool.OnDeselect();
            _selectedTool.ToggleOff();
            _selectedTool = null;
            Debug.Log("deselect success");
        }

        /// <summary>
        /// Selects a random tile from the potential tiles list and prepares to place it.
        /// </summary>
        public void SelectPlaceRandomTile(bool toolSelect)
        {
            if (!toolSelect)
            {
                DeselectToolCancel();
                return;
            }

            randomTileTool.OnSelect();
            _selectedTool = randomTileTool;
        }

        /// <summary>
        /// Selects the bulldozer tool to remove tiles.
        /// </summary>
        public void SelectBulldozer(bool toolSelect)
        {
            if (!toolSelect)
            {
                DeselectToolCancel();
                return;
            }

            _selectedTool = bulldozerTool;
        }

        /// <summary>
        /// Selects the extinguisher tool to extinguish burning tiles.
        /// </summary>
        public void SelectExtinguisher(bool toolSelect)
        {
            if (!toolSelect)
            {
                DeselectToolCancel();
                return;
            }

            _selectedTool = extinguisherTool;
        }

        /// <summary>
        /// Selects the revive tool to revive burned tiles.
        /// </summary>
        public void SelectReviver(bool toolSelect)
        {
            if (!toolSelect)
            {
                DeselectToolCancel();
                return;
            }

            _selectedTool = reviveTileTool;
        }
    }
}