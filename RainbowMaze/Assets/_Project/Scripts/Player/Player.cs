using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private GridManager grid;
    private Vector2Int currentGridPosition;

    private Vector3 targetPosition;
    private bool isMoving;

    private void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;
                isMoving = false;
            }
        }
    }

    public void Move(Vector2Int direction)
    {
        if (isMoving) return;

        Vector2Int targetTile = currentGridPosition + direction;
        CellData targetCellData = grid.GetCellData(targetTile);

        if (targetCellData != null && targetCellData.Passable)
        {
            currentGridPosition = targetTile;
            targetPosition = grid.CellToWorldConverter(currentGridPosition);
        }

        isMoving = true;
    }

    public void Spawn(GridManager board, Vector2Int cell)
    {
        grid = board;
        currentGridPosition = cell;

        transform.position = (Vector3)grid.CellToWorldConverter(currentGridPosition);
    }
}
