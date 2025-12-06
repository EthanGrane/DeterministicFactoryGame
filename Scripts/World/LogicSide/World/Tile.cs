using UnityEngine;

public class Tile
{
    public Vector2Int position;
    public TerrainSO terrainSO;
    public Building building; // Null = any building on this tile
    
    public int x { get { return position.x; } }
    public int y { get { return position.y; } }

    public Tile(Vector2Int position, TerrainSO terrainSo, Building building)
    {
        this.position = position;
        this.terrainSO = terrainSo;
        this.building = building;
    }
}