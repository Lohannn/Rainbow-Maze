using UnityEngine;

public class LemoniceTile : PuzzleTile
{
    public override bool IsPassable => true;

    public override void PlayerEntered(Player player)
    {
        if (!player.IsDamaged)
        {
            player.ChangeScent(Player.PlayerScent.Lemon);
            player.Move(player.LastMove, true);
        }
        else
        {
            player.IsDamaged = false;
        }
    }
}
