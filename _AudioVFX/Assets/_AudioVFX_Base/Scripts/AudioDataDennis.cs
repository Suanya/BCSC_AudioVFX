using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*

 * *** Denni's Frequency Bands ***

 * [0] 20-90 Hz: Kickdrum
 * [1] 90-450 Hz: Bässe
 * [2] 450-2300 Hz: Melodie und Klatschen
 * [3] 2300-20000 Hz: Zischen und Zwitschern

 */

[RequireComponent(typeof(AudioSource))]

public class AudioDataDennis : MonoBehaviour
{
    private AudioSource _audioSource;

    public static float[] _samplesLeft = new float[512];
    public static float[] _samplesRight = new float[512];

    public static float[] _freqBand = new float[4];
    float[] _bandBuffer = new float[4];
    float[] _bufferDecrease = new float[4];

    float[] _freqBandHighest = new float[4];
    public static float[] _audioBand = new float[4];
    public static float[] _audioBandBuffer = new float[4];

    public float Amplitude, AmplitudeBuffer;

    float _AmplitudeHighest;
    public float _audioProfile;

    public enum _channel { Stereo, Left, Right };
    public _channel channel = new _channel();


    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        //_audioSource.clip = Microphone.Start("H4", true, 10, 44100); //Built-in Microphone
        //_audioSource.Play();
        AudioProfile(_audioProfile);
    }

    // Update is called once per frame
    void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        BandBuffer();
        CreateAudioBands();
        GetAmplitude();

        //Debug.Log(_freqBand[0]);
    }

    void AudioProfile(float audioProfile)
    {
        for(int i = 0; i < 4; i++)
        {
            _freqBandHighest[i] = audioProfile;
        }
    }

    void GetSpectrumAudioSource()
    {
        _audioSource.GetSpectrumData(_samplesLeft, 0, FFTWindow.Blackman);
        _audioSource.GetSpectrumData(_samplesRight, 0, FFTWindow.Blackman);
    }

    void MakeFrequencyBands()
    {
        int count = 0;

        for(int i = 0; i < 4; i++)
        {
            if (i == 0)
            {

                float average = 0;
                int sampleCount = 2;



                for (int j = 0; j < sampleCount; j++)
                {
                    if (channel == _channel.Stereo)
                    {
                        average += _samplesLeft[count] + _samplesRight[count] * (count + 1);
                    }
                    if (channel == _channel.Left)
                    {
                        average += _samplesLeft[count] * (count + 1);
                    }
                    if (channel == _channel.Right)
                    {
                        average += _samplesRight[count] * (count + 1);
                    }
                    count++;
                }
                average /= count;
                _freqBand[i] = average * 10;

              
            }

            if (i == 1)
            {

                float average = 0;
                int sampleCount = 8;



                for (int j = 0; j < sampleCount; j++)
                {
                    if (channel == _channel.Stereo)
                    {
                        average += _samplesLeft[count] + _samplesRight[count] * (count + 1);
                    }
                    if (channel == _channel.Left)
                    {
                        average += _samplesLeft[count] * (count + 1);
                    }
                    if (channel == _channel.Right)
                    {
                        average += _samplesRight[count] * (count + 1);
                    }
                    count++;
                }
                average /= count;
                _freqBand[i] = average * 10;

               
            }

            if (i == 2)
            {

                float average = 0;
                int sampleCount = 44;



                for (int j = 0; j < sampleCount; j++)
                {
                    if (channel == _channel.Stereo)
                    {
                        average += _samplesLeft[count] + _samplesRight[count] * (count + 1);
                    }
                    if (channel == _channel.Left)
                    {
                        average += _samplesLeft[count] * (count + 1);
                    }
                    if (channel == _channel.Right)
                    {
                        average += _samplesRight[count] * (count + 1);
                    }
                    count++;
                }
                average /= count;
                _freqBand[i] = average * 10;

          
            }

            if (i == 3)
            {

                float average = 0;
                int sampleCount = 456;



                for (int j = 0; j < sampleCount; j++)
                {
                    if (channel == _channel.Stereo)
                    {
                        average += _samplesLeft[count] + _samplesRight[count] * (count + 1);
                    }
                    if (channel == _channel.Left)
                    {
                        average += _samplesLeft[count] * (count + 1);
                    }
                    if (channel == _channel.Right)
                    {
                        average += _samplesRight[count] * (count + 1);
                    }
                    count++;
                }
                average /= count;
                _freqBand[i] = average * 10;

            }
        }
    }

    //This creates a smooth downfall when the amplitude is lower than the previous value, this is the impression that
    //the audio signal is pushing up the blocks and there's almost like an air cushion inside of them as they ease down
    void BandBuffer()
    {
        for(int g = 0; g < 4; g++)
        {
            if(_freqBand[g] > _bandBuffer[g])
            {
                _bandBuffer[g] = _freqBand[g];
                _bufferDecrease[g] = 0.005f;
            }
            if (_freqBand[g] < _bandBuffer[g])
            {
                _bandBuffer[g] -= _bufferDecrease[g];
                _bufferDecrease[g] *= 1.2f;
            }
        }
    }
    
    void CreateAudioBands()
    {
        for(int i = 0; i < 4; i++)
        {
            if (_freqBand[i] > _freqBandHighest[i])
            {
                _freqBandHighest[i] = _freqBand[i];
            }
            _audioBand[i] = (_freqBand[i] / _freqBandHighest[i]);
            _audioBandBuffer[i] = (_bandBuffer[i] / _freqBandHighest[i]);
        }
    }

    void GetAmplitude()
    {
        float _CurrentAmplitude = 0;
        float _CurrentAmplitudeBuffer = 0;

        for (int i = 0; i < 4; i++)
        {
            _CurrentAmplitude += _audioBand[i];
            _CurrentAmplitudeBuffer += _audioBandBuffer[i];
        }

        if (_CurrentAmplitude > _AmplitudeHighest)
        {
            _AmplitudeHighest = _CurrentAmplitude;
        }

        Amplitude = _CurrentAmplitude / _AmplitudeHighest;
        AmplitudeBuffer = _CurrentAmplitudeBuffer / _AmplitudeHighest;
    }
}
