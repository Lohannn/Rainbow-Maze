using System;
using UnityEngine;

public class WaterTile : PuzzleTile
{
    public override bool IsPassable => true;

    [SerializeField] private AudioClip waterSplash;
    [SerializeField] private AudioClip electricShock;
    [SerializeField] private AudioClip piranhaBite;

    private TileAudioManager audioManager;

    private void Start()
    {
        audioManager = GetComponent<TileAudioManager>();
        MainCamera = Camera.main.GetComponent<CameraManager>();
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
                StartCoroutine(MainCamera.ElectricSchockEffect());
                audioManager.PlaySound(electricShock);
                player.isDamaged = true;
                player.Move(-player.LastMove, true);
                return;
            }
        }

        if (player.CurrentScent == Player.PlayerScent.Orange)
        {
            audioManager.PlaySound(piranhaBite);
            player.isDamaged = true;
            player.Move(-player.LastMove, true);
            return;
        }
        else if (player.CurrentScent == Player.PlayerScent.Lemon)
        {
            player.ChangeScent(Player.PlayerScent.Clean);
        }

        audioManager.PlaySound(waterSplash);
    }
}
