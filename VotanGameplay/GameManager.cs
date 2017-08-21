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
        [SerializeField, Tooltip("Точки смерти для врага")]
        private static Transform[] deadPointsForEnemy;

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
        /// Получить все посмертные позиции для врагов (для босса)
        /// </summary>
        private void GetAllDeadPoints()
        {
            deadPointsForEnemy = new Transform[transform.Find("DeadPoints").GetChildCount()];
            for (int i = 0; i < deadPointsForEnemy.Length; i++)
                deadPointsForEnemy[i] = transform.Find("DeadPoints").
                    GetChild(i).GetComponent<Transform>();

            Debug.Log(deadPointsForEnemy.Length);
        }

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
