using System;
using UnityEngine;

public class WaterTile : PuzzleTile
{
    public override bool IsPassable => true;

    public WaterTile() { }
    
    public WaterTile(GridManager grid, Vector2Int cellPosition)
    {
        Board = grid;
        CellPosition = cellPosition;
    }

    public override void PlayerEntered(Player player)
    {
        Vector2Int[] directions = new Vector2Int[]
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        foreach (var direction in directions)
        {
            if (Board.GetCellData(CellPosition + direction).ContainedObject is ElectricityTile)
            {
                print("O player entrou na WaterTile, mas tem uma ElectricityTile ao lado, movendo para trás!");
                player.Move(-player.LastMove);
                return;
            }
        }

        if (player.CurrentScent == Player.PlayerScent.Orange)
        {
            print("O player entrou na WaterTile, mas tem o cheiro de laranja, movendo para trás!");
            player.Move(-player.LastMove);
        }
        else if (player.CurrentScent == Player.PlayerScent.Lemon)
        {
            print("O player entrou na WaterTile, limpando seu cheiro!");
            player.ChangeScent(Player.PlayerScent.Clean);
        }
    }
}
