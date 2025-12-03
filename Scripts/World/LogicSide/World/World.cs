using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class World : MonoBehaviour
{
    public static World Instance { get; private set; }
    public const int WorldSize = 128;
    private Tile[,] tiles;
    
    public TerrainSO[] terrains;
    private Dictionary<string, TerrainSO> terrainByName;

    public TileBase colliderTile;
    public Tilemap terrainCollider;
    public Tilemap buildingCollider;

    private void Awake()
    {
        #region Singleton Awake
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        #endregion

        // Terrain Dictionary
        terrainByName = new Dictionary<string, TerrainSO>();
        foreach (var t in terrains)
        {
            terrainByName[t.terrainName] = t;
        }
        
        GenerateWorld();
    }

    private void GenerateWorld()
    {
        tiles = new Tile[WorldSize, WorldSize];
        const int border = 15;

        for (int x = 0; x < WorldSize; x++)
        {
            for (int y = 0; y < WorldSize; y++)
            {
                TerrainSO terrainSo =
                    x <= border || y <= border || x >= WorldSize - border || y >= WorldSize - border
                        ? terrainByName["Stone"]
                        : terrainByName["Dirt"];

                tiles[x, y] = new Tile(new Vector2Int(x, y), terrainSo, null);

                // Set Terrain Colliders
                if (terrainSo.solid)
                    terrainCollider.SetTile(new Vector3Int(x, y, 0), colliderTile);
                else
                    terrainCollider.SetTile(new Vector3Int(x, y, 0), null);

            }
        }
    }

    public Tile[,] GetTiles() => tiles;

    public Tile GetTile(int x, int y) => tiles[x, y];
}