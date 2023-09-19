using Managers.BuildTools;
using Map;
using UnityEngine;

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

        private void Update()
        {
            GetTargetedTile();
            
            if (_selectedTool == null)
                return;

            if (Input.GetKeyUp(KeyCode.Escape))
            {
                DeselectToolCancel();
                return;
            }

            if (!Input.GetMouseButton(0))
                return;

            if (targetedTile == null)
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
            _selectedTool.OnDeselect();
            _selectedTool = null;
        }

        private void DeselectToolSuccess()
        {
            _selectedTool.Charge(targetedTile);
            _selectedTool.OnDeselect();
            _selectedTool = null;
        }

        /// <summary>
        /// Selects a random tile from the potential tiles list and prepares to place it.
        /// </summary>
        public void SelectPlaceRandomTile()
        {
            if(!randomTileTool.CanSelect())
                return;

            _selectedTool = randomTileTool;
        }

        /// <summary>
        /// Selects the bulldozer tool to remove tiles.
        /// </summary>
        public void SelectBulldozer()
        {
            if(!bulldozerTool.CanSelect())
                return;

            _selectedTool = bulldozerTool;
        }

        /// <summary>
        /// Selects the extinguisher tool to extinguish burning tiles.
        /// </summary>
        public void SelectExtinguisher()
        {
            if(!extinguisherTool.CanSelect())
                return;

            _selectedTool = extinguisherTool;
        }
    }
}