using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance_ = null;
    public static SoundManager Instance
    {
        get
        {
            if(instance_ == null)
            {
                instance_ = new SoundManager();
            }
            return instance_;
        }
    }
    private SoundManager()
    {

    }

    public AudioSource[] audioSources_ = new AudioSource[(int)Sound.MaxCount];
    public Dictionary<string, AudioClip> dicAudioClips_ = new Dictionary<string, AudioClip>();

    public AudioClip world;
    public AudioClip level;
    public List<AudioClip> moveSFX;
    
    public AudioClip LevelEnter;
    public AudioClip LevelClear;
    public AudioClip RuleSetup;

    void Awake()
    {
        if(instance_ == null)
        {
            instance_ = this;
        }
        else if(instance_ != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        string[] soundNames = System.Enum.GetNames(typeof(Sound));
        for(int i = 0; i < soundNames.Length - 1; i++)
        {
            GameObject go = new GameObject {name = soundNames[i]};
            audioSources_[i] = go.AddComponent<AudioSource>();
            go.transform.parent = transform;
        }
        audioSources_[(int)Sound.Bgm].loop = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        GFunc.Log($"{SoundManager.Instance.ToString()}");
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Clear()
    {
        foreach(AudioSource audioSource in audioSources_)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }

        dicAudioClips_.Clear();
    }
    public void Play(AudioClip audioClip, Sound type = Sound.Effect, float pitch = 1.0f)
    {
        if(audioClip == null)
            return;
        
        if(type == Sound.Bgm)
        {
            AudioSource audioSource = audioSources_[(int)Sound.Bgm];
            if(audioSource.isPlaying)
                audioSource.Stop();
            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else
        {
            AudioSource audioSource = audioSources_[(int)Sound.Effect];
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);
        }
    }
    public void Play(string path, Sound type = Sound.Effect, float pitch = 1.0f)
    {
        AudioClip audioClip = GetOrAddAudioClip(path, type);
        Play(audioClip, type, pitch);
    }
    AudioClip GetOrAddAudioClip(string path, Sound type = Sound.Effect)
    {
        if(path.Contains("Sounds/") == false)
            path = $"Sounds/{path}";

        AudioClip audioClip = null;
        
        if(type == Sound.Bgm)
        {
            audioClip = Resources.Load<AudioClip>(path);
        }
        else
        {
            if(dicAudioClips_.TryGetValue(path, out audioClip) == false)
            {
                audioClip = Resources.Load<AudioClip>(path);
                dicAudioClips_.Add(path, audioClip);
            }
        }

        if(audioClip = null)
        {
            GFunc.Log($"AudioClip Missing ! {path}");
        }

        return audioClip;
    }
    public void PlayerMove()
    {
        int rd = Random.Range(0, moveSFX.Count);
        Play(SoundManager.instance_.moveSFX[rd], Sound.Effect);
    }
}
