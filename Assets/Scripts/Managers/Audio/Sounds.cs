using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Managers.Audio
{
    [Serializable]
    public class Sounds
    {
        public string name;
        public SoundTypes nameType;
        public AudioClip soundClip;
        public AudioMixerGroup audioMixer;
        [Range(0f,1f)] public float volume = 1f;
        [Range(1f,3f)] public float pitch = 1f;
        public bool isLoop;
        [HideInInspector] public AudioSource source;
    }
}