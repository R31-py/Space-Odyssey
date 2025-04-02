using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private AudioSource bossMusic;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        if (!backgroundMusic.isPlaying)
        {
            bossMusic.Stop();
            backgroundMusic.Play();
        }
    }

    public void PlayBossMusic()
    {
        if (!bossMusic.isPlaying)
        {
            backgroundMusic.Stop();
            bossMusic.Play();
        }
    }
}