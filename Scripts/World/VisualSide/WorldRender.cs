using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(World))]
public class WorldRenderer : MonoBehaviour
{
    public static WorldRenderer Instance { get; private set; }

    private World world;

    public Tilemap terrainTilemap;
    public Tilemap buildingTilemap;

    void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        world = World.Instance;
        RenderAll();
    }

    private void RenderAll()
    {
        var tiles = world.GetTiles();

        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                SetTileVisual(x, y, tiles[x, y]);
            }
        }
    }

    public void SetTileVisual(int x, int y, Tile tile)
    {
        Vector3Int pos = new Vector3Int(x, y, 0);

        //Terrain visual
        terrainTilemap.SetTile(pos, tile.TerrainSo.sprite);

        if (tile.building == null)
            buildingTilemap.SetTile(pos, null);
        else
            buildingTilemap.SetTile(pos, tile.building.block.sprite);
    }
}