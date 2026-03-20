using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Player;

public class BFSMazeValidator : MonoBehaviour
{

    // Mini classe para representar o estado do jogador durante a busca, incluindo sua posição e o cheiro atual
    public struct PhantomPlayerState
    {
        public Vector2Int Position;
        public PlayerScent Scent;

        public PhantomPlayerState(Vector2Int position, Player.PlayerScent scent)
        {
            Position = position;
            Scent = scent;
        }
    }

    // Verifica se o labirinto atual é solucionável
    public bool IsMazeSolvable(Vector2Int start, int goalY, GridManager grid)
    {
        // Usando uma fila para o BFS e um HashSet para rastrear os estados visitados
        Queue<PhantomPlayerState> queue = new Queue<PhantomPlayerState>();
        HashSet<PhantomPlayerState> visited = new HashSet<PhantomPlayerState>();

        // Coloca na fila o estado inicial do jogador, começando com o cheiro limpo e já marca aquela combinação de
        // posição + estado como visitado
        queue.Enqueue(new PhantomPlayerState(start, PlayerScent.Clean));
        visited.Add(new PhantomPlayerState(start, PlayerScent.Clean));

        // Enquanto houver estados para explorar na fila, continua o processo
        while (queue.Count > 0)
        {
            // Pega o próximo estado da fila para explorar
            PhantomPlayerState currentState = queue.Dequeue();

            if (currentState.Position.y == goalY)
            {
                return true;
            }

            // Para cada tile vizinha válida, verifica se aquela combinação de posição + estado do cheiro já foi visitada
            foreach (var neighbour in GetValidNeighbours(currentState, grid))
            {
                if (!visited.Contains(new PhantomPlayerState(neighbour.Position, neighbour.Scent)))
                {
                    visited.Add(new PhantomPlayerState(neighbour.Position, neighbour.Scent));
                    queue.Enqueue(neighbour);
                }
            }
        }

        return false;
    }

    #if UNITY_EDITOR
    // Feita com total ajuda de IA para debuggar o BFS, desenhando as linhas no mundo para mostrar o processo de busca
        // (Queria apenas entender o erro pois ainda não havia masterizado o sistema de pathfinding)
    // prompt: Tem como eu fazer uma ferramenta de debug onde eu posso acompanhar cada teste do Pathfinding? (Após conversas analisando
    // o meu código escrito)
    public IEnumerator DebugPathfinder(Vector2Int start, int goalY, GridManager grid)
    {
        print($"Starting BFS Pathfinding Debug at position: {start}...");
        print($"Confirming starting tile: {grid.GetCellData(start).GridPosition}");

        Queue<PhantomPlayerState> queue = new Queue<PhantomPlayerState>();
        HashSet<PhantomPlayerState> visited = new HashSet<PhantomPlayerState>();

        // ==========================================================
        // CORREÇÃO: Usando a sua função para achar o lugar real no mundo!
        // ==========================================================
        Vector3 startWorldPos = grid.CellToWorldConverter(start);
        startWorldPos.z = -0.5f; // Puxa um pouco para a frente da câmera

        Debug.DrawLine(startWorldPos + new Vector3(-0.4f, -0.4f, 0), startWorldPos + new Vector3(0.4f, 0.4f, 0), Color.magenta, 100f);
        Debug.DrawLine(startWorldPos + new Vector3(-0.4f, 0.4f, 0), startWorldPos + new Vector3(0.4f, -0.4f, 0), Color.magenta, 100f);

        queue.Enqueue(new PhantomPlayerState(start, PlayerScent.Clean));
        visited.Add(new PhantomPlayerState(start, PlayerScent.Clean));

        while (queue.Count > 0)
        {
            PhantomPlayerState currentState = queue.Dequeue();

            // CORREÇÃO AQUI TAMBÉM
            Vector3 posicaoMundo = grid.CellToWorldConverter(currentState.Position);
            posicaoMundo.z = -0.5f;
            Debug.DrawLine(posicaoMundo, posicaoMundo + Vector3.up * 0.5f, Color.red, 1f);

            if (currentState.Position.y == goalY)
            {
                print("Goal reached at: " + currentState.Position);
                yield break;
            }

            yield return new WaitForSeconds(0.1f);

            foreach (var neighbour in GetValidNeighbours(currentState, grid))
            {
                if (!visited.Contains(new PhantomPlayerState(neighbour.Position, neighbour.Scent)))
                {
                    visited.Add(new PhantomPlayerState(neighbour.Position, neighbour.Scent));
                    queue.Enqueue(neighbour);

                    // E CORREÇÃO AQUI!
                    Vector3 neighbourWorldPos = grid.CellToWorldConverter(neighbour.Position);
                    neighbourWorldPos.z = -0.5f;
                    Debug.DrawLine(neighbourWorldPos, neighbourWorldPos + Vector3.up * 0.5f, Color.green, 1f);
                }
            }
        }

        print("No path to goal found.");
    }
    #endif

    // Verifica os vizinhos em volta e checa se são válidos para o jogador se mover
    private List<PhantomPlayerState> GetValidNeighbours(PhantomPlayerState state, GridManager grid)
    {
        //Cria uma lista para armazenar os vizinhos válidos
        List<PhantomPlayerState> neighbours = new List<PhantomPlayerState>();

        Vector2Int[] directions = new Vector2Int[]
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        foreach (var direction in directions)
        {
            //Define a posição do vizinho e estado atual do jogador
            Vector2Int newPosition = state.Position + direction;
            PlayerScent newScent = state.Scent;
            CellData cellData = grid.GetCellData(newPosition);

            //Se não tiver cell, não for passável ou ser considerado perigoso, ignora esta
            //repetição do while e parte direto para a próxima
            if (cellData == null || !cellData.IsPassable || IsDangerousTile(newPosition, newScent, grid)) continue;

            PuzzleTile tile = cellData.ContainedObject;

            if (tile is OrangeTile)
            {
                newScent = PlayerScent.Orange;
                neighbours.Add(new PhantomPlayerState(newPosition, newScent));
            }
            else if (tile is LemoniceTile)
            {
                newScent = PlayerScent.Lemon;

                Vector2Int slide = newPosition;

                //Enquanto tiver tiles de Lemonice, continua deslizando o jogador
                while (true)
                {
                    //Define a próxima posição do jogador
                    Vector2Int nextSlide = slide + direction;
                    CellData nextCell = grid.GetCellData(nextSlide);

                    //Se não for uma cell válida, para no ultimo tile de Lemonice (o que é seguro)
                    if (cellData == null || !nextCell.IsPassable || IsDangerousTile(nextSlide, newScent, grid)) break;

                    //Atualiza a posição do jogador para o próximo tile
                    slide = nextSlide;
                    PuzzleTile nextTile = nextCell.ContainedObject;

                    if (nextTile is OrangeTile) //se ele caiu num tile de laranja, atualiza estado
                    {
                        newScent = PlayerScent.Orange;
                    }

                    if(nextTile is not LemoniceTile)
                    {
                        break;
                    }
                }

                neighbours.Add(new PhantomPlayerState(slide, newScent));
            }
            else
            {
                neighbours.Add(new PhantomPlayerState(newPosition, newScent));
            }
        }

        return neighbours;
    }

    // Verifica se a tile passada é perigosa atualmente
    private bool IsDangerousTile(Vector2Int position, PlayerScent scent, GridManager grid)
    {
        CellData cell = grid.GetCellData(position);

        if (cell != null)
        {
            if (cell.ContainedObject == null)
            {
                return false;
            }

            if (cell.ContainedObject is ElectricityTile)
            {
                return true;
            }
            else if (cell.ContainedObject is WaterTile)
            {
                if (cell.HasElectricAdjacent || scent == PlayerScent.Orange)
                {
                    return true;
                }
            }
        }

        return false;
    }
}