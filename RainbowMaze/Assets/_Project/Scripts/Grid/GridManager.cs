using System.Collections.Generic;
using System.Linq;
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
    private Vector2Int[] mazeTilesPositions;
    private BFSMazeValidator mazeValidator;

    private void Start()
    {
        mazeTilesPositions = Enumerable.Range(1, width - 2).SelectMany(x => Enumerable.Range(4, 10).Select(y => new Vector2Int(x, y))).ToArray();
        tilemap = GetComponentInChildren<Tilemap>();
        grid = GetComponentInChildren<Grid>();
        mazeValidator = GetComponent<BFSMazeValidator>();
        gridData = new CellData[width, height]; //Criando o Array 2D para armazenar as Tiles com o comprimento e a largura do Grid

        // Tile: Coordenada Y
        for (int y = 0; y < height; y++)
        {
            // Tile: Coordenada X
            for (int x = 0; x < width; x++)
            {
                Vector2Int currentTilePosition = new Vector2Int(x, y);
                Tile currentTile;
                gridData[x, y] = new CellData(); //Adiciona o CellData para entregar o estado da Tiel
                gridData[x, y].GridPosition = currentTilePosition;

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
        SpawnTile(5, 16, VICTORY_TILE);

        // GERADOR DO LABIRINTO
        bool mazeIsValid = false;
        int attempts = 0;

        // O grid irá gerar aleatoriamente as tiles do labirinto enquanto não tiver um puzzle solucionável ou caso demore demais
        while (!mazeIsValid || attempts < 200) 
        {
            CreateMaze();
            //Libera o "Fantasma" para andar no labirinto e retorna true quando ele achar a primeira saída
            mazeIsValid = mazeValidator.IsMazeSolvable(new Vector2Int(5, 2), 14, this); 

            if (!mazeIsValid) //Se não for solucionável, limpa o labirinto e gera outro
            {
                ClearMaze();
            }

            attempts++;
        }
        print($"Labirinto gerado após {attempts} tentativa(s).");
        if (attempts >= 200)
        {
            print("Falha em gerar labirinto");
        }

        //Corrige a posição do Player para que fique dentro de uma tile do grid
        player.Spawn(this, new Vector2Int(5, 2));

        //TESTE DOS TILES
        #region Teste de Tiles
        //SpawnTile(5, 3, BLOCK_TILE);
        //SpawnTile(5, 4, PATH_TILE);
        //SpawnTile(7, 4, LEMONICE_TILE);
        //SpawnTile(7, 5, LEMONICE_TILE);
        //SpawnTile(7, 6, LEMONICE_TILE);
        //SpawnTile(7, 7, ELECTRICITY_TILE);
        //SpawnTile(7, 3, ORANGE_FILE);
        //SpawnTile(8, 3, WATER_TILE);
        //SpawnTile(3, 3, ELECTRICITY_TILE);
        //SpawnTile(3, 4, WATER_TILE);
        #endregion
    }

    // Comandos para teste no Editor
    #if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) //Reinicia a posição e estado do player
        {
            player.Spawn(this, new Vector2Int(5, 2));
            player.ChangeScent(Player.PlayerScent.Clean);
        }

        if (Input.GetKeyDown(KeyCode.Space)) //Cria labirinto e libera o "Fantasma" para Debug
        {
            CreateMaze();
            StartCoroutine(mazeValidator.DebugPathfinder(new Vector2Int(5, 2), 14, this));
        }

        if (Input.GetKeyDown(KeyCode.Backspace)) //Limpa labirinto
        {
            ClearMaze();
        }

        if (Input.GetKeyDown(KeyCode.Period)) //Retorna a posição inicial do player para teste
        {
            print(GetCellData(new Vector2Int(5,2)).GridPosition);
        }
    }
    #endif

    private void SpawnTile(int x, int y, int prefabIndex)
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

    private void CreateMaze()
    {
        foreach (Vector2Int tilePosition in mazeTilesPositions)
        {
            SpawnTile(tilePosition.x, tilePosition.y, Random.Range(0, puzzleTilesPrefabs.Length - 1));
        }

        //Repassa pelo labirinto criado para verificar os tiles de agua perigosos e facilitar o pathfinding
        foreach (Vector2Int tilePosition in mazeTilesPositions) 
        {
            CellData cellData = GetCellData(tilePosition);

            if (cellData.ContainedObject is WaterTile waterTile)
            {
                cellData.HasElectricAdjacent = waterTile.CheckForElectricAdjacents();
            }
        }
    }

    private void ClearMaze()
    {
        foreach (Vector2Int tilePosition in mazeTilesPositions)
        {
            CellData cellData = GetCellData(tilePosition);
            if (cellData.ContainedObject != null)
            {
                Destroy(cellData.ContainedObject.gameObject);
                cellData.ContainedObject = null;
                cellData.IsPassable = true;
                cellData.HasElectricAdjacent = false;
            }
        }
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
