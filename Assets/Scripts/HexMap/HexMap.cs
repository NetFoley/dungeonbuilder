using UnityEngine;
using System.Collections.Generic;

public class HexMap : MonoBehaviour
{
    public List<HexTile> tiles = new List<HexTile>();

    public float hexRadius = 1f;  // The radius of each hex tile

    // Convert axial coordinates to world position
    public Vector3 HexToWorldPosition(int q, int r)
    {
        float x = hexRadius * Mathf.Sqrt(3f) * (q + r / 2f);
        float z = hexRadius * 1.5f * r;
        return new Vector3(x, 0, z);
    }

    // Create a new hex tile at the specified position
    public void CreateHexTile(int q, int r, GameObject tilePrefab)
    {
        Vector3 position = HexToWorldPosition(q, r);

        HexTile newTile = new HexTile
        {
            position = position,
            tilePrefab = tilePrefab,
            instance = Instantiate(tilePrefab, position, Quaternion.identity, transform)
        };

        tiles.Add(newTile);
    }

    // Remove a hex tile at a specified position
    public void RemoveHexTile(HexTile tile)
    {
        if (tile.instance != null)
        {
            DestroyImmediate(tile.instance);
        }
        tiles.Remove(tile);
    }

    // Find a tile at a given world position
    public HexTile FindTileAtPosition(Vector3 position)
    {
        return tiles.Find(tile => Vector3.Distance(tile.position, position) < 0.1f);
    }

    // Clear all tiles from the map
    public void ClearMap()
    {
        foreach (var tile in tiles)
        {
            if (tile.instance != null)
            {
                DestroyImmediate(tile.instance);
            }
        }
        tiles.Clear();
    }
}