using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Player player;

    [Header("Grid Settings")]
    [SerializeField] private int width;
    [SerializeField] private int height;

    [Header("Tile Settings")]
    [SerializeField] private Tile floorTile;
    [SerializeField] private Tile wallTile;

    [SerializeField] private GameObject[] puzzleTilesPrefabs;

    [Header("Puzzle Tile Identifiers")]
    private static readonly int BLOCK_TILE = 0;
    private static readonly int PATH_TILE = 1;
    private static readonly int LEMONICE_TILE = 2;
    private static readonly int ORANGE_FILE = 3;
    private static readonly int ELECTRICITY_TILE = 4;
    private static readonly int WATER_TILE = 5;
    private static readonly int VICTORY_TILE = 6;

    private Tilemap tilemap;
    private Grid grid;
    private CellData[,] gridData;

    private void Start()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        grid = GetComponentInChildren<Grid>();
        gridData = new CellData[width, height]; //Criando o Array 2D para armazenar as Tiles com o comprimento e a largura do Grid

        // Tile: Coordenada Y
        for (int y = 0; y < height; y++)
        {
            // Tile: Coordenada X
            for (int x = 0; x < width; x++)
            {
                Tile currentTile;
                gridData[x, y] = new CellData(); //Adiciona o CellData para entregar o estado da Tiel

                if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                {
                    //Caso seja as bordas do Grid, adiciona um tile de parede e define que não é passável
                    currentTile = wallTile;
                    gridData[x, y].IsPassable = false;
                }
                else
                {
                    //Caso não, adiciona um tile de piso e define que é passável
                    currentTile = floorTile;
                    gridData[x, y].IsPassable = true;
                }

                //Adiciona de fato a Tile ao mapa
                tilemap.SetTile(new Vector3Int(x, y, 0), currentTile);
            }
        }

        //Corrige a posição do Player para que fique dentro de uma tile do grid
        player.Spawn(this, new Vector2Int(5, 2));

        //TESTE DOS TILES
        #region Teste de Tiles
        SpawnTileForTest(5, 3, BLOCK_TILE);
        SpawnTileForTest(5, 4, PATH_TILE);
        SpawnTileForTest(7, 4, LEMONICE_TILE);
        SpawnTileForTest(7, 5, LEMONICE_TILE);
        SpawnTileForTest(7, 6, LEMONICE_TILE);
        SpawnTileForTest(7, 3, ORANGE_FILE);
        SpawnTileForTest(8, 3, WATER_TILE);
        SpawnTileForTest(3, 3, ELECTRICITY_TILE);
        SpawnTileForTest(3, 4, WATER_TILE);
        #endregion
    }

    private void SpawnTileForTest(int x, int y, int prefabIndex)
    {
        CellData data = gridData[x, y]; 

        PuzzleTile newPuzzleTile = (Instantiate(
            puzzleTilesPrefabs[prefabIndex],
            CellToWorldConverter(new Vector2Int(x, y)),
            Quaternion.identity
            ).GetComponent<PuzzleTile>());
        data.ContainedObject = newPuzzleTile;
        data.ContainedObject.CellPosition = new Vector2Int(x, y);
        data.ContainedObject.Board = this;
        data.IsPassable = newPuzzleTile.IsPassable;
    }

    //Converte a coordenada de uma Célula/Tile para posição do mundo (x, y, z)
    public Vector3 CellToWorldConverter(Vector2Int cellIndex)
    {
        return grid.GetCellCenterWorld((Vector3Int)cellIndex);
    }

    //Retorna as informações da Célula/Tile entregue 
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
