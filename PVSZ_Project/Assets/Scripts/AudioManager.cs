using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    public AudioSource music_source;
    public AudioSource eff_source;
    
    //-sounds
    public AudioClip mainscene;
    
    public AudioClip shooter_shoot;
    public AudioClip alien_melee;
    
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
    }
    
    void Start()
    {
        music_source.clip = mainscene;
        music_source.loop = true;
        music_source.Play();
    }
    
    // TODO(sftl): better handle multiple effects trying to be played at a simmilar time
    // maybe remember the last time end the effect that was played end decide wether
    // to play next sound or not. Also add multiple sound sources for sounds.
    public void Play_ShooterShoot()
    {
        eff_source.clip = shooter_shoot;
        eff_source.Play();
    }
    
    public void Play_AlienMelee()
    {
        eff_source.clip = alien_melee;
        eff_source.Play();
    }
}