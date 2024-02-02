using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
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
