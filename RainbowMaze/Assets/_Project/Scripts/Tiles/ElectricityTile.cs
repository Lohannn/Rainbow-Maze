using Unity.VisualScripting;
using UnityEngine;

public class ElectricityTile : PuzzleTile
{
    public override bool IsPassable => true;

    [SerializeField] private AudioClip electricShock;

    private TileAudioManager audioManager;

    private void Start()
    {
        audioManager = GetComponent<TileAudioManager>();
        MainCamera = Camera.main.GetComponent<CameraManager>();
    }

    public override void PlayerEntered(Player player)
    {
        StartCoroutine(MainCamera.ElectricSchockEffect());
        audioManager.PlaySound(electricShock);
        player.isDamaged = true;
        player.Move(-player.LastMove, true); //Move o player para a direção oposta do movimento anterior, ou seja, para trás
    }
}
