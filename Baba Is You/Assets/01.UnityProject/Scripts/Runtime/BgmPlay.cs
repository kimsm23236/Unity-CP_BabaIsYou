using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmPlay : MonoBehaviour
{
    public AudioClip TitleBgm;
    // Start is called before the first frame update
    void Awake()
    {
        SoundManager.Instance.ToString();
    }
    void Start()
    {
        SoundManager.Instance.Play(TitleBgm, Sound.Bgm);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
