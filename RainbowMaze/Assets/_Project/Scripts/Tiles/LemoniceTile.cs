using UnityEngine;

public class LemoniceTile : PuzzleTile
{
    public override bool IsPassable => true;

    public override void PlayerEntered(Player player)
    {

        player.ChangeScent(Player.PlayerScent.Lemon);
        player.Move(player.LastMove, true);
    }
}
