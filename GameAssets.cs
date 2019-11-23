using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace piojonichan
{
    //public struct 

    public static class SoundManager
    {

        public enum Sound
        {
            PlayerMove,
            PlayerAttack,
            EnemyHit,
            EnemyDie,
            Treasure,

        }

        private static Dictionary<Sound, float> soundTimerDictionary;
        private static GameObject OneShotGameObject;
        private static AudioSource OneShotAudioSource;

        public static void Initialize()
        {
            soundTimerDictionary = new Dictionary<Sound, float>();
            soundTimerDictionary[Sound.PlayerMove] = 0f;
        }

        public static void PlaySound(Sound misound)
        {
            if (CanPlaySound(misound))
            {
                if (OneShotGameObject == null) 
                {
                    OneShotGameObject = new GameObject("Sound");
                    OneShotAudioSource = OneShotGameObject.AddComponent<AudioSource>();
                }
                OneShotAudioSource.PlayOneShot(GetAudioClip(misound));
            }
        }

        private static bool CanPlaySound(Sound sound)
        {
            switch (sound) {
                default:
                    return true;
                case Sound.PlayerMove:
                    if (soundTimerDictionary.ContainsKey(sound))
                    {
                        float lastTimePlayed = soundTimerDictionary[sound];
                        float PlayerMoveTimerMax = 0.5f;
                        if (lastTimePlayed + PlayerMoveTimerMax < Time.time)
                        {
                            soundTimerDictionary[sound] = Time.time;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return true;
                    }
                    //break;
            }
        }

        private static AudioClip GetAudioClip(Sound sound)
        {
            foreach (SoundAudioClip soundAudioClip in  GameAssets.i.soundAudioClipArray)
            {
                if (soundAudioClip.sound == sound)
                {
                    return soundAudioClip.audioClip;
                }
            }
            Debug.LogError("sound "  + sound.ToString() +  " not found");
            return null;
        }

    }

    //public struct  

    public class GameAssets : MonoBehaviour
    {
        private static GameAssets _i;
        public static GameAssets i
        {
            get
            {
                if (_i == null) _i = Instantiate(Resources.Load<GameAssets>("GameAssets"));
                return _i;
            }
        }

        public SoundAudioClip[] soundAudioClipArray;

    }

    [System.Serializable]
    public class SoundAudioClip
    {
        public SoundManager.Sound sound;
        public AudioClip audioClip;
    }


}
