using AbstractBehaviour;
using MovementEffects;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace VotanGameplay
{
    /// <summary>
    /// Менеджер игры. Ставит музыку.
    /// </summary>
    public class GameManager
        : MonoBehaviour
    {
        #region Переменные
        [SerializeField, Tooltip("Точки смерти для врага")]
        private static Transform[] deadPointsForEnemy;

        private static AudioSource gameAudioSource;
        private static bool isGameOver;
        private static bool isWin;
        private static float gameMusicCachedPitch;
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
            GetAllDeadPoints();
            AbstractSoundStorage.LoadAllStaticSounds();
            Timing.RunCoroutine(CoroutineForCheckIfMusicIsEnded());
        }

        /// <summary>
        /// Получить все посмертные позиции для врагов (для босса)
        /// </summary>
        private void GetAllDeadPoints()
        {
            deadPointsForEnemy = new Transform[transform.Find("DeadPoints").GetChildCount()];
            for (int i = 0; i < deadPointsForEnemy.Length; i++)
                deadPointsForEnemy[i] = transform.Find("DeadPoints").
                    GetChild(i).GetComponent<Transform>();
        }

        /// <summary>
        /// Получить ближайшую позицию для предсмертной конвульсии врага
        /// </summary>
        /// <param name="deadPosition"></param>
        /// <returns></returns>
        public static Transform GetClosestDeadPositionForEnemy(Vector3 deadPosition)
        {
            float distance = float.MaxValue;
            float tempDistance = float.MaxValue;
            Transform closestPosition = null;

            for (int i = 0; i < deadPointsForEnemy.Length; i++)
            {
                tempDistance = Vector3.Distance
                    (deadPosition, deadPointsForEnemy[i].position);
                if (tempDistance <= distance)
                {
                    distance = tempDistance;
                    closestPosition = deadPointsForEnemy[i];
                }
            }
            return closestPosition;
        }

        /// <summary>
        /// Событие окончания игры
        /// </summary>
        public static void EventGameOver()
        {
            Timing.RunCoroutine(CoroutineForWaiting(false));
        }

        /// <summary>
        /// Событие выигрыша игры
        /// </summary>
        public static void EventWinOver()
        {
            Timing.RunCoroutine(CoroutineForWaiting(true));        
        }

        /// <summary>
        /// Выключить музыку
        /// </summary>
        /// <param name="isPause"></param>
        public static void DisableMusic(bool isPause)
        {
            if (isPause)
                Timing.RunCoroutine(CoroutineForLerpEnableMusic(false, 0));
            else
                Timing.RunCoroutine(CoroutineForLerpEnableMusic(true, gameMusicCachedPitch));
        }

        /// <summary>
        /// Корутина для плавного отключения музыки
        /// </summary>
        /// <param name="auSource"></param>
        /// <param name="enable"></param>
        /// <param name="inVolume"></param>
        /// <returns></returns>
        private static IEnumerator<float> CoroutineForLerpEnableMusic
            (bool enable,float inVolume)
        {
            int i = 0;
            if (enable)
            {
                if (!Joystick.IsDragPause)
                {
                    while (gameAudioSource.pitch < inVolume)
                    {
                        i++;
                        gameAudioSource.pitch = Mathf.Lerp(gameAudioSource.pitch, inVolume, 0.05f * i);
                        yield return Timing.WaitForOneFrame;
                    }
                    gameAudioSource.pitch = inVolume;
                }
            }
            else
            {
                while (gameAudioSource.pitch > 0)
                {
                    i++;
                    gameAudioSource.pitch = Mathf.Lerp(gameAudioSource.pitch, 0, 0.05f * i);
                    yield return Timing.WaitForOneFrame;
                }
                gameAudioSource.pitch = 0;
            }
        }

        /// <summary>
        /// Работа с музыкой во время выхода босса
        /// </summary>
        /// <param name="toPause"></param>
        public static void BossMusicWork(bool toPause)
        {
            if (toPause)
            {
                Timing.RunCoroutine(CoroutineForLerpEnableMusic(false, 0));
            }
            else
            {
                gameMusicCachedPitch = 1 + 0.15f;
                Timing.RunCoroutine(CoroutineForLerpEnableMusic(true, gameMusicCachedPitch));
            }
        }

        /// <summary>
        /// Корутина для ожидания
        /// </summary>
        /// <param name="isWinOrGameOver"></param>
        /// <returns></returns>
        private static IEnumerator<float> CoroutineForWaiting(bool isWinOrGameOver)
        {
            gameAudioSource.Stop();
            gameMusicCachedPitch = 1;
            gameAudioSource.pitch = gameMusicCachedPitch;
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
        }
    }
}
