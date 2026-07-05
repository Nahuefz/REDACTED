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

        public AudioSource Play(AudioData audioData, Vector3 position = default)
        {
            if (audioData == null || audioData.Clip == null) return null;

            GameObject tempAudioObj = new GameObject("Audio_" + audioData.name);
            tempAudioObj.transform.position = position;

            AudioSource source = tempAudioObj.AddComponent<AudioSource>();
            source.clip = audioData.Clip;
            source.outputAudioMixerGroup = audioData.Mixer;
            source.volume = audioData.Volume;

            source.loop = audioData.IsLoop;
            source.spatialBlend = audioData.SpatialBlend;

            source.rolloffMode = AudioRolloffMode.Linear;
            source.minDistance = 1f;
            source.maxDistance = 15f;

            source.Play();

            if (!audioData.IsLoop)
            {
                StartCoroutine(DestroySourceWhenFinished(tempAudioObj, source.clip.length));
            }

            return source;
        }
        private System.Collections.IEnumerator DestroySourceWhenFinished(GameObject obj, float delay)
        {
            yield return new WaitForSeconds(delay);
            if (obj != null) Destroy(obj);
        }
    }
}
