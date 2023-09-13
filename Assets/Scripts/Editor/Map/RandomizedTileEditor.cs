using UnityEditor;
using UnityEngine;

namespace Map
{
    /// <summary>
    /// Custom editor for the RandomizedTile class.
    /// </summary>
    [CustomEditor(typeof(RandomizedTile))]
    public class RandomizedTileEditor : UnityEditor.Editor
    {
        /// <summary>
        /// Overrides the default Inspector GUI to include custom buttons for clearing and generating random objects on top of tiles.
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            RandomizedTile tile = (RandomizedTile)target;

            GUILayout.Space(10);

            if (GUILayout.Button("Generate Objects"))
            {
                tile.GenerateObjects();
            }

            if (GUILayout.Button("Clear Objects"))
            {
                tile.ClearSpawnedObjects();
            }
        }
    }
}