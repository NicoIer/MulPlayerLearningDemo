using Nico.Design;
using Nico.Network.Singleton;
using UnityEngine;

namespace Kitchen.Music
{
    public class MusicManager : MonoSingleton<MusicManager>
    {
        [SerializeField] private AudioSource musicAudioSource;

        public void ChangeVolume(float volume)
        {
            if (volume > 1)
            {
                volume = 0;
            }

            if (volume < 0)
            {
                volume = 0;
            }
            musicAudioSource.volume = volume;
        }

        public float GetVolume()
        {
            return musicAudioSource.volume;
        }
    }
}