using UnityEngine;

public class LemoniceTile : PuzzleTile
{
    public override bool IsPassable => true;

    public override void PlayerEntered(Player player)
    {
        if (!player.isDamaged)
        {
            player.ChangeScent(Player.PlayerScent.Lemon);
            player.Move(player.LastMove, true);
        }
        else
        {
            player.isDamaged = false;
        }
    }
}
