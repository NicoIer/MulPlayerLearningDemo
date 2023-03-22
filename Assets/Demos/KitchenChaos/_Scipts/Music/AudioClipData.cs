using UnityEngine;

namespace Kitchen.Music
{
    [CreateAssetMenu(fileName = "AudioClipData", menuName = "ScriptableObjects/AudioClipData", order = 0)]
    public class AudioClipData : ScriptableObject
    {
        public AudioClip[] chop;
        public AudioClip[] deliveryFail;
        public AudioClip[] deliverySuccess;
        public AudioClip[] footStep;
        public AudioClip[] drop;
        public AudioClip[] pickUp;
        public AudioClip[] stoveSizzle;
        public AudioClip[] trash;
        public AudioClip[] warning;
    }
}