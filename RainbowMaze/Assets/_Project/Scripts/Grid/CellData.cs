using UnityEngine;

[System.Serializable]
public class CellData
{
    public bool IsPassable { get; set; }
    public PuzzleTile ContainedObject;
    public Vector2Int GridPosition { get; set; }
    public bool HasElectricAdjacent { get; set; }
}
