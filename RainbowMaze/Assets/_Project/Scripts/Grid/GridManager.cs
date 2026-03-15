using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Player player;

    [SerializeField] private int width;
    [SerializeField] private int height;

    [SerializeField] private Tile floorTile;
    [SerializeField] private Tile wallTile;

    private Tilemap tilemap;
    private Grid grid;

    private CellData[,] gridData;

    private void Start()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        grid = GetComponentInChildren<Grid>();
        gridData = new CellData[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Tile currentTile;
                gridData[x, y] = gameObject.AddComponent<CellData>();

                if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                {
                    currentTile = wallTile;
                    gridData[x, y].Passable = false;
                }
                else
                {
                    currentTile = floorTile;
                    gridData[x, y].Passable = true;
                }

                tilemap.SetTile(new Vector3Int(x, y, 0), currentTile);
            }
        }

        player.Spawn(this, new Vector2Int(5, 2));
    }

    public Vector3 CellToWorldConverter(Vector2Int cellIndex)
    {
        return grid.GetCellCenterWorld((Vector3Int)cellIndex);
    }

    public CellData GetCellData(Vector2Int cellIndex)
    {
        if (cellIndex.x < 0 || cellIndex.x >= width ||
            cellIndex.y < 0 || cellIndex.y >= height)
        {
            return null;
        }

        return gridData[cellIndex.x, cellIndex.y];
    }
}
