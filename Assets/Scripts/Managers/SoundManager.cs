using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [System.Serializable]
    public struct Sound
    {
        public AudioClip audioClip;
        public float volume;
    }

    [SerializeField] private AudioSource audioSource;

    public static SoundManager instance;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }


    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleSound();
        }

    }

    public void PlaySound(Sound soundToPlay) 
    {
        audioSource.PlayOneShot(soundToPlay.audioClip,soundToPlay.volume);
    }

    public void PlayMusic(Sound[] listOfMusic) 
    {
        StartCoroutine(PlayMusicOnLoop(listOfMusic));
    }

    private IEnumerator PlayMusicOnLoop(Sound[] listOfMusic) 
    {
        int index = 0;
        float musicDuration;

        while (true)
        {
            musicDuration = listOfMusic[0].audioClip.length;

            PlaySound(listOfMusic[0]);

            yield return new WaitForSeconds(musicDuration);

            index++;
            if (index >= listOfMusic.Length)
            {
                index = 0;
            }
        }
    }











    public void ToggleSound() 
    {
        if (AudioListener.volume == 0f)
        {
            AudioListener.volume = 1f;
        }
        else
        {
            AudioListener.volume = 0f;
        }
    }

    public void TurnOnSound() 
    {
        AudioListener.volume = 1f;
    }

    public void TurnOffSound() 
    {
        AudioListener.volume = 0f;
    }
}
