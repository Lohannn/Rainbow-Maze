using UnityEngine;

public abstract class PuzzleTile : MonoBehaviour
{
    public abstract bool IsPassable { get; }

    public GridManager Board { protected get; set; }
    public Vector2Int CellPosition { protected get; set; }
    public CameraManager MainCamera { protected get; set; }

    public virtual void PlayerEntered(Player player)
    {

    }
}