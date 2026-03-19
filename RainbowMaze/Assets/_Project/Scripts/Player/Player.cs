using UnityEngine;

public class Player : MonoBehaviour
{
    public enum PlayerScent { Clean, Orange, Lemon }

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Sprite[] playerSprites;

    public int Steps { get; private set; }
    public bool CanMove { private get; set; } = true;
    public bool isDamaged { get; set; }

    private GridManager grid;
    private Vector2Int currentGridPosition;

    private Vector3 targetPosition;
    private bool isMoving;

    public Vector2Int LastMove { get; private set; }
    public PlayerScent CurrentScent { get; private set; } = PlayerScent.Clean;

    private SpriteRenderer spriteRenderer;
    private PlayerAudioManager audioManager;
    [SerializeField] private UIManager mainCanvas;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioManager = GetComponent<PlayerAudioManager>();
    }

    private void Update()
    {
        //Só libera a movimentação caso tenha algum comando
        if (isMoving)
        {
            //Move o jogador para a posição alvo
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            //Para evitar que o jogador fique preso entre Tiles, caso a distãncia dele seja próxima o suficiente da posição alvo, 
            //o teleporta para a posição exata
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                CellData currentCellData = grid.GetCellData(currentGridPosition);

                transform.position = targetPosition;
                isMoving = false; //Desliga a movimentação

                if (currentCellData.ContainedObject != null)
                {
                    currentCellData.ContainedObject.GetComponent<PuzzleTile>().PlayerEntered(this);
                }
            }
        }
    }

    //Coloca o player na Célula/Tile definida
    public void Spawn(GridManager board, Vector2Int cell)
    {
        Steps = 0;

        mainCanvas.UpdateSteps(Steps);

        grid = board;
        currentGridPosition = cell;

        transform.position = (Vector3)grid.CellToWorldConverter(currentGridPosition);
    }

    //Move o jogador para 1 tile dependendo da direção
    public void Move(Vector2Int direction, bool hasSpecialEffect = false)
    {
        if (isMoving || !CanMove) return; //Caso já esteja se movendo, não irá receber novos comandos

        Vector2Int targetTile = currentGridPosition + direction; //Armazena aonde seria a próxima tile que o player se moveria
        CellData targetCellData = grid.GetCellData(targetTile); //Pega a informação da Tile

        if (targetCellData != null && targetCellData.IsPassable)
        {
            //Caso seja uma tile passável, troca a atual posição do player para essa tile e muda a posição alvo e
            //libera a movimentação
            currentGridPosition = targetTile;
            targetPosition = grid.CellToWorldConverter(currentGridPosition);
            isMoving = true;
            LastMove = direction;

            if (isDamaged && targetCellData.ContainedObject is not LemoniceTile)
            {
                isDamaged = false;
            }

            if (!hasSpecialEffect) {
                audioManager.PlaySound(audioManager.MOVE);
                Steps++;
                mainCanvas.UpdateSteps(Steps);
            } 
        }
        else if(!targetCellData.IsPassable)
        {
            audioManager.PlaySound(audioManager.CANT_PASS);
        }
    }

    //Muda o sprite do player dependendo do cheiro
    public void ChangeScent(PlayerScent scent)
    {
        if (CurrentScent == scent) return; //Caso o novo cheiro seja o mesmo do atual, não faça nada
        
        switch (scent)
        {
            case PlayerScent.Clean:
                CurrentScent = PlayerScent.Clean;
                spriteRenderer.sprite = playerSprites[(int)PlayerScent.Clean];
                break;
            case PlayerScent.Orange:
                CurrentScent = PlayerScent.Orange;
                spriteRenderer.sprite = playerSprites[(int)PlayerScent.Orange];
                break;
            case PlayerScent.Lemon:
                CurrentScent = PlayerScent.Lemon;
                spriteRenderer.sprite = playerSprites[(int)PlayerScent.Lemon];
                break;
        }

        audioManager.PlaySound(audioManager.SCENT_CHANGE);
    }
}
