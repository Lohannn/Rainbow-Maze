using System;
using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryTile : PuzzleTile
{
    public override bool IsPassable => true;

    [SerializeField] private AudioClip victory;

    private TileAudioManager audioManager;

    private void Start()
    {
        audioManager = GetComponent<TileAudioManager>();
    }

    public override void PlayerEntered(Player player)
    {
        StartCoroutine(VictoryDelay(player));
    }

    private IEnumerator VictoryDelay(Player player)
    {
        player.CanMove = false;
        audioManager.PlaySound(victory);
        PlayerData.UpdateData(player.Steps);
        yield return new WaitForSeconds(victory.length + 0.3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
