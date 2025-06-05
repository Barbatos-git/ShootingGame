using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioClip normalBGM;
    public AudioClip bossBGM;
    private AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayBossBGM()
    {
        if (bossBGM != null)
        {
            audioSource.clip = bossBGM;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void PlayNormalBGM()
    {
        if (normalBGM != null)
        {
            audioSource.clip = normalBGM;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
}
