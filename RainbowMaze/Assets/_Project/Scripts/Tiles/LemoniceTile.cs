using UnityEngine;

public class LemoniceTile : PuzzleTile
{
    public override bool IsPassable => true;

    public override void PlayerEntered(Player player)
    {
        print("O player entrou na LemoniceTile, agora tem cheiro de limão e move pra frente!");

        player.ChangeScent(Player.PlayerScent.Lemon);
        player.Move(player.LastMove, true);
    }
}
