using UnityEngine;

public class ElectricityTile : PuzzleTile
{
    public override bool IsPassable => true;

    public override void PlayerEntered(Player player)
    {
        print("O player entrou na ElectricityTile, movendo para trás!");

        player.Move(-player.LastMove); //Move o player para a direção oposta do movimento anterior, ou seja, para trás
    }
}
