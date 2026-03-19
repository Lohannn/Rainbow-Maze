using UnityEngine;

public class TileAudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioClips;

    public readonly int WATER_TILE = 0;
    public readonly int ELECTRICITY_TILE = 1;
    public readonly int PIRANHA_TILE = 2;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(int clipIndex)
    {
        audioSource.PlayOneShot(audioClips[clipIndex]);
    }
}
