using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HoopStack
{
    public enum SFX
    {
        circleInsertSfx,
        socketSfx,
    }
    public class BGMUSIC : MonoBehaviour
    {
        public static BGMUSIC instance;
        private static bool musicInScene;
        [SerializeField] AudioSource soundEffectsAS;
        [SerializeField] AudioSource gameMusicAS;
        [SerializeField] List<AudioClip> sfx;

        private void Awake()
        {
            if (!musicInScene)
            {
                DontDestroyOnLoad(this.gameObject);
                musicInScene = true;
            }
            else
                Destroy(this.gameObject);

            if (instance == null)
                instance = this;
        }

        public void PlaySoundEffect(SFX sound)
        {
            soundEffectsAS.clip = sfx.First(d => d.name == sound.ToString());
            soundEffectsAS.Play();
        }

        public AudioSource GetSfxAudioSource()
        {
            return soundEffectsAS;
        }
        public AudioSource GetGameMusicAudioSource()
        {
            return gameMusicAS;
        }
    }
}
