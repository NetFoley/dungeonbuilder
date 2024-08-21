using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HexMap))]
public class HexMapEditor : Editor
{
    private HexMap hexMap;
    private GameObject selectedPrefab;

    void OnEnable()
    {
        hexMap = (HexMap)target;
    }

    void OnSceneGUI()
    {
        HandleInput();
        DrawHexGrid();
    }

    void HandleInput()
    {
        Event e = Event.current;

        if (e.type == EventType.MouseDown && e.button == 0 && selectedPrefab != null)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

            // Raycast against the grid plane
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            if (plane.Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                PlaceTile(hitPoint);
                e.Use(); // Mark the event as used, so Unity doesn't process it further
            }
        }
    }

    void PlaceTile(Vector3 hitPoint)
    {
        // Convert the hit point into hex grid coordinates
        int q = Mathf.RoundToInt(hitPoint.x / (hexMap.hexRadius * Mathf.Sqrt(3f)));
        int r = Mathf.RoundToInt(hitPoint.z / (hexMap.hexRadius * 1.5f));
        Vector3 hexPosition = hexMap.HexToWorldPosition(q, r);

        // Check if there's already a tile at this position
        HexTile existingTile = hexMap.FindTileAtPosition(hexPosition);

        if (existingTile != null)
        {
            // Remove existing tile
            hexMap.RemoveHexTile(existingTile);
        }
        else
        {
            // Place a new tile
            hexMap.CreateHexTile(q, r, selectedPrefab);
        }

        EditorUtility.SetDirty(hexMap);
    }

    void DrawHexGrid()
    {
        foreach (var tile in hexMap.tiles)
        {
            Handles.color = Color.yellow;
            Handles.DrawWireDisc(tile.position, Vector3.up, hexMap.hexRadius);
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GUILayout.Space(10);
        GUILayout.Label("Hex Tile Placement", EditorStyles.boldLabel);

        selectedPrefab = (GameObject)EditorGUILayout.ObjectField("Selected Prefab", selectedPrefab, typeof(GameObject), false);

        if (GUILayout.Button("Clear Map"))
        {
            hexMap.ClearMap();
            EditorUtility.SetDirty(hexMap);
        }
    }
}
