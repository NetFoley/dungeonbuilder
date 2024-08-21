using UnityEngine;

[System.Serializable]
public class HexTile
{
    public Vector3 position;
    public GameObject tilePrefab;  // The prefab used for this tile
    public GameObject instance;    // The instance of the tile in the scene
}