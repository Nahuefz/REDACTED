using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Managers.Audio
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }
        public Dictionary<AudioMixerGroup, float> MixerSettings = new Dictionary<AudioMixerGroup, float>();

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public AudioSource Play(AudioData audioData, bool loop)
        {
            if (audioData == null || audioData.Clip == null) return null;

            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = audioData.Clip;
            source.outputAudioMixerGroup = audioData.Mixer;
            source.volume = audioData.Volume;
            source.loop = loop;

            source.Play();

            if (!loop)
            {
                StartCoroutine(DestroySourceWhenFinished(source));
            }

            return source;
        }

        private System.Collections.IEnumerator DestroySourceWhenFinished(AudioSource source)
        {
            yield return new WaitForSeconds(source.clip.length);
            if (source != null) Destroy(source);
        }
    }
}
