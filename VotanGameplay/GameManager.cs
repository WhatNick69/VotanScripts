using AbstractBehaviour;
using MovementEffects;
using System.Collections.Generic;
using UnityEngine;

namespace VotanGameplay
{
    /// <summary>
    /// Менеджер игры. Ставит музыку.
    /// </summary>
    public class GameManager
        : MonoBehaviour
    {
        #region Переменные
        private static AudioSource gameAudioSource;
        private static bool isGameOver;
        private static bool isWin;
        #endregion

        #region Свойства
        public static bool IsWin
        {
            get
            {
                return isWin;
            }

            set
            {
                isWin = value;
                if (isWin)
                    EventWinOver();
            }
        }

        public static bool IsGameOver
        {
            get
            {
                return isGameOver;
            }

            set
            {
                isGameOver = value;
                if (isGameOver)
                    EventGameOver();
            }
        }
        #endregion

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Awake()
        {
            isGameOver = false;
            isWin = false;

            gameAudioSource =
                GetComponent<AudioSource>();
            gameAudioSource.clip = null;
            gameAudioSource.playOnAwake = false;
            AbstractSoundStorage.LoadAllStaticSounds();

            Timing.RunCoroutine(CoroutineForCheckIfMusicIsEnded());
        }

        /// <summary>
        /// Событие окончания игры
        /// </summary>
        public static void EventGameOver()
        {
            Timing.RunCoroutine(CoroutineForWaiting(false));
        }

        public static void EventWinOver()
        {
            Timing.RunCoroutine(CoroutineForWaiting(true));        
        }

        public static void DisableMusic(bool isPause)
        {
            if (isPause)
                gameAudioSource.pitch = 0;
            else
                gameAudioSource.pitch = 1;
        }

        private static IEnumerator<float> CoroutineForWaiting(bool isWinOrGameOver)
        {
            gameAudioSource.Stop();
            if (isWinOrGameOver)
            {
                yield return Timing.WaitForSeconds(1);
                AbstractSoundStorage.WinOverMusic(gameAudioSource);
            }
            else
            {
                yield return Timing.WaitForSeconds(0.3f);
                AbstractSoundStorage.GameOverMusic(gameAudioSource);
            }
        }

        /// <summary>
        /// Корутина на установление музыки
        /// </summary>
        /// <returns></returns>
        private static IEnumerator<float> CoroutineForCheckIfMusicIsEnded()
        {
            yield return Timing.WaitForSeconds(1);
            AbstractSoundStorage.GameplayMusic(gameAudioSource);

            while (true)
            {
                if (!gameAudioSource.isPlaying && 
                    !isGameOver && 
                    !isWin)
                {
                    AbstractSoundStorage.GameplayMusic(gameAudioSource);
                }
                yield return Timing.WaitForSeconds(0.5f);
            }
            Debug.Log("ALL!");
        }
    }
}
