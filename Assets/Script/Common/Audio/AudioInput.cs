﻿using Texture.Texture;
using UnityEngine;
using UnityEngine.Serialization;

namespace Script.Common.Audio
{
    public class AudioInput : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private float[] spectrum;
        [SerializeField] private ModifyTexture mod;
        
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = Microphone.Start(null, true, 10, 44100);
            audioSource.loop = true;
            //while (Microphone.GetPosition(null) < 0) { }
            audioSource.Play();
        }

        // Update is called once per frame
        void Update()
        {
            //AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);
            spectrum =  audioSource.GetSpectrumData(spectrum.Length, 0,FFTWindow.Hamming);
            mod.spectrum = spectrum;
        }
    }
}
