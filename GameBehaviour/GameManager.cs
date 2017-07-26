using AbstractBehaviour;
using MovementEffects;
using System.Collections.Generic;
using UnityEngine;

namespace GameBehaviour
{
    /// <summary>
    /// Менеджер игры. Ставит музыку.
    /// </summary>
    public class GameManager
        : MonoBehaviour
    {
        private static AudioSource gameAudioSource;

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Start()
        {
            gameAudioSource =
                GetComponent<AudioSource>();
            AbstractSoundStorage.LoadAllStaticSounds();
            AbstractSoundStorage.GameplayMusic(gameAudioSource);
            Timing.RunCoroutine(CoroutineForCheckIfMusicIsEnded());
        }

        /// <summary>
        /// Событие окончания игры
        /// </summary>
        private static void EventGameOver()
        {
            AbstractSoundStorage.GameOverMusic(gameAudioSource);
        }

        /// <summary>
        /// Корутина на установление музыки
        /// </summary>
        /// <returns></returns>
        private static IEnumerator<float> CoroutineForCheckIfMusicIsEnded()
        {
            while (!AllPlayerManager.IsGameOver)
            {
                if (!gameAudioSource.isPlaying)
                {
                    AbstractSoundStorage.GameplayMusic(gameAudioSource);
                }
                yield return Timing.WaitForSeconds(0.5f);
            }
            EventGameOver();
        }
    }
}
