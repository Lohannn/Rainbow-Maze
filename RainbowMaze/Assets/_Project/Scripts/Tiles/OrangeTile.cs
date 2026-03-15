using UnityEngine;

public class OrangeTile : PuzzleTile
{
    public override bool IsPassable => true;

    public override void PlayerEntered(Player player)
    {
        player.ChangeScent(Player.PlayerScent.Orange);
    }
}
