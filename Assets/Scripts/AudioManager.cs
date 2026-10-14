using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [Header("Sound effects")]
    [SerializeField] AudioClip catchClip;
    [SerializeField] AudioClip hurtClip;
    [SerializeField] AudioClip gameOverClip;
    [Range(0f, 1f)] [SerializeField] float volume = 0.8f;

    [Header("Optional: stops when the game ends")]
    [SerializeField] AudioSource music;

    AudioSource source;

    void Awake() => source = GetComponent<AudioSource>();

    void OnEnable()
    {
        GameEvents.OnItemCaught += HandleCaught;
        GameEvents.OnGameOver += HandleGameOver;
    }

    void OnDisable()
    {
        GameEvents.OnItemCaught -= HandleCaught;
        GameEvents.OnGameOver -= HandleGameOver;
    }

    void HandleCaught(ItemType type)
    {
        Play(type == ItemType.Good ? catchClip : hurtClip);
    }

    void HandleGameOver()
    {
        if (music != null) music.Stop();
        Play(gameOverClip);
    }

    void Play(AudioClip clip)
    {
        if (clip != null) source.PlayOneShot(clip, volume);
    }
}