using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic; // for using lists

[RequireComponent(typeof(AudioSource))]
public class MicrophoneInput : MonoBehaviour
{
    public AudioSource _audioSource;
    public string microphone;

    private List<string> options = new List<string>();

    public int _audioSampleRate = 44100;
    public int _samples = 8192;

   

    

    // Start is called before the first frame update
    void Start()
    {
        foreach (var device in Microphone.devices)
        {
            Debug.Log("Name: " + device);
        }

        // get components we need
        _audioSource = GetComponent<AudioSource>();

        // initialize input with default mic
        UpdateMicrophone();
    }

    /*
    private void Update()
    {
        Debug.Log("isPlaying " + _audioSource.isPlaying);
    }
    */

    void UpdateMicrophone()
    {
        _audioSource.Stop();

        // start recording the audioClip from the mic
        _audioSource.clip = Microphone.Start(microphone, true, 1, _audioSampleRate);
        _audioSource.loop = true;

        // mute the sound with an Audio Mixer group because we don't want the player to hear it
        Debug.Log(Microphone.IsRecording(microphone).ToString());

        if (Microphone.IsRecording(microphone))
        {
            // check that mic is recording, otherwise you'll get stuck in an infinit loop waiting for it to stop
            while (!(Microphone.GetPosition(microphone) > 0))
            {
                // wait until the recording has started
            }

            Debug.Log("recording started with " + microphone);

            // start playing the audioSource
            _audioSource.Play();      
        }
        else
        {
            // microphone NOT working
            Debug.Log(microphone + "NOT working!");
        }
    }

   
    
}
