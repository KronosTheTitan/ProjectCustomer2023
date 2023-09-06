using UnityEngine;
using Map;
using UnityEditor;

[CustomEditor(typeof(HexGrid))]
public class ObjectBuilderEditor : Editor
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
