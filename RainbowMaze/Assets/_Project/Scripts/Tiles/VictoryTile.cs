using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryTile : PuzzleTile
{
    public override bool IsPassable => true;

    public override void PlayerEntered(Player player)
    {
        print("Você venceu!");
        PlayerData.UpdateData(player.Steps);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
