using Map;
using UnityEditor;
using UnityEngine;

namespace Editor.Map
{
    [CustomEditor(typeof(HexGrid))]
    public class ObjectBuilderEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            HexGrid hexGrid = (HexGrid)target;
            if(GUILayout.Button("Clear!"))
            {
                hexGrid.ClearGrid();
            }
            if(GUILayout.Button("Generate!"))
            {
                hexGrid.GenerateTiles();
            }
        }
    }
}
