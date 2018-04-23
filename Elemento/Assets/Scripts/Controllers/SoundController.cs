using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class SoundController : MonoBehaviourSingleton<SoundController>
    {
        public AudioClip Explosion;
        public AudioClip GameOver;
        public AudioClip GameWon;
        public AudioClip Popup;
        public AudioClip Scan;
        public AudioClip Build;
        public AudioClip Water;

        public AudioSource ControlledAudioSource;

        public void PlaySound(AudioClip sound)
        {
            var volume = GameManager.Instance.SoundVolume;

            var audioSource = ControlledAudioSource;
            if (audioSource != null)
            {
                audioSource.volume = volume;
                audioSource.loop = false;
                audioSource.Stop();
                audioSource.clip = sound;
                audioSource.Play();
            }
        }
    }
}
