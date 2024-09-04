using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{

    public AudioSource[] audioSource;

    /// <summary>
    /// Plays the sound from the given AudioSource based on its list index
    /// </summary>
    /// <param name="soundNo">Index number of wanted AudioSource</param>
    public void PlaySound(int soundNo)
    {
        audioSource[soundNo].Play();
    }

    /// <summary>
    /// Stops any given AudioSource based on its list index
    /// </summary>
    /// <param name="soundNo">Index number of wanted AudioSource</param>
    public void StopSound(int soundNo)
    { 
        audioSource[soundNo].Stop(); 
    }
}
