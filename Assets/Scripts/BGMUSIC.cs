using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HoopStack
{
    // Enum to represent different sound effects
    public enum SFX
    {
        CircleInsertSfx,
        SocketSfx,
    }

    public class BGMUSIC : MonoBehaviour
    {
        // Singleton instance of BGMUSIC
        public static BGMUSIC Instance;

        // Flag to ensure only one instance of music persists across scenes
        private static bool musicInScene;

        // Serialized fields for assigning sound effect and music audio sources in the Unity Editor
        [SerializeField] private AudioSource soundEffectsAS;
        [SerializeField] private AudioSource gameMusicAS;

        // List to store sound effect audio clips
        [SerializeField] private List<AudioClip> sfx;

        private void Awake()
        {
            // Ensure that only one music object persists across scenes
            if (!musicInScene)
            {
                DontDestroyOnLoad(this.gameObject);
                musicInScene = true;
            }
            else
            {
                Destroy(this.gameObject);
                return;
            }

            // Initialize the singleton instance
            if (Instance == null)
                Instance = this;
        }

        /// <summary>
        /// Plays the specified sound effect.
        /// </summary>
        /// <param name="sound">The sound effect to play.</param>
        public void PlaySoundEffect(SFX sound)
        {
            // Find the audio clip that matches the specified sound effect and play it
            AudioClip clip = sfx.FirstOrDefault(d => d.name == sound.ToString());
            if (clip != null)
            {
                soundEffectsAS.clip = clip;
                soundEffectsAS.Play();
            }
            else
            {
                Debug.LogWarning($"Sound effect '{sound}' not found in the SFX list.");
            }
        }

        /// <summary>
        /// Gets the AudioSource for sound effects.
        /// </summary>
        /// <returns>The AudioSource used for sound effects.</returns>
        public AudioSource GetSfxAudioSource()
        {
            return soundEffectsAS;
        }

        /// <summary>
        /// Gets the AudioSource for background game music.
        /// </summary>
        /// <returns>The AudioSource used for game music.</returns>
        public AudioSource GetGameMusicAudioSource()
        {
            return gameMusicAS;
        }
    }
}
