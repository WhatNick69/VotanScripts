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
        private static Transform[] deadPointsForEnemy;
        private static Transform[] sniperPointsForEnemy;
        private static bool[] sniperPointsForEnemyBool;

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
            gameMusicCachedPitch = 1;

            gameAudioSource =
                GetComponent<AudioSource>();
            gameAudioSource.clip = null;
            gameAudioSource.playOnAwake = false;

            GetAllDeadPoints();
            GetAllSniperPoints();

            AbstractSoundStorage.LoadAllStaticSounds();
            Timing.RunCoroutine(CoroutineForCheckIfMusicIsEnded());
        }

        /// <summary>
        /// Получить все посмертные позиции для врагов (для босса)
        /// </summary>
        private void GetAllDeadPoints()
        {
            Transform deadPointsParent = transform.Find("DeadPoints");
            deadPointsForEnemy = new Transform[deadPointsParent.childCount];
            for (int i = 0; i < deadPointsForEnemy.Length; i++)
                deadPointsForEnemy[i] = deadPointsParent.
                    GetChild(i).GetComponent<Transform>();
        }

        /// <summary>
        /// Получить все снайперские позиции для врагов-снайперов
        /// </summary>
        private void GetAllSniperPoints()
        {
            Transform sniperPointsParent = transform.Find("SniperPoints");
            sniperPointsForEnemy = new Transform[sniperPointsParent.childCount];
            sniperPointsForEnemyBool = new bool[sniperPointsParent.childCount];
            for (int i = 0; i < sniperPointsForEnemy.Length; i++)
                sniperPointsForEnemy[i] = sniperPointsParent.
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
        /// Освободить точку стрельбы
        /// </summary>
        /// <param name="myPositionNumber"></param>
        public static void OpenPosition(int myPositionNumber)
        {
            if (myPositionNumber >= 0 && myPositionNumber < sniperPointsForEnemyBool.Length)
                sniperPointsForEnemyBool[myPositionNumber] = false;
        }

        /// <summary>
        /// Получить ближайшую позицию для стрельбы, между игроком и врагом
        /// </summary>
        /// <param name="enemyPosition">Позиция врага</param>
        /// <param name="playerPosition">Позиция игрока</param>
        /// <param name="minDistanceBetweenPositionAndPlayer">Минимальная дистанция между игроком и позицией</param>
        /// <param name="maxDistanceBetweenPlayerAndEnemy">Максимальная дистанция между игроком и врагом</param>
        /// <param name="myPositionNumber">Номер текущей моей занятой позиции</param>
        /// <returns></returns>
        public static Vector3 GetClosestSniperPositionForEnemy
            (Vector3 enemyPosition, Vector3 playerPosition, float minDistanceBetweenPositionAndPlayer, float maxDistanceBetweenPlayerAndEnemy, ref int myPositionNumber)
        {
            int tempNumber = 0;
            float distance = float.MaxValue;
            float tempDistanceBetweenEnemyAndPosition = 0;
            float tempDistanceBetweenPlayerAndPosition = 0;
            Vector3 closestPosition = Vector3.zero;

            for (int i = 0; i < sniperPointsForEnemy.Length; i++)
            {
                // если позиция свободна
                tempDistanceBetweenPlayerAndPosition = Vector3.Distance
                    (playerPosition, sniperPointsForEnemy[i].position);

                // если расстояние до позиции больше минимума и меньше максимума
                if (tempDistanceBetweenPlayerAndPosition >= minDistanceBetweenPositionAndPlayer
                    && tempDistanceBetweenPlayerAndPosition <= maxDistanceBetweenPlayerAndEnemy)
                {
                    tempDistanceBetweenEnemyAndPosition = Vector3.Distance
                        (enemyPosition, sniperPointsForEnemy[i].position);

                    // ищем минимальный путь 
                    if (tempDistanceBetweenEnemyAndPosition < distance
                        && (!sniperPointsForEnemyBool[i] || (myPositionNumber == i 
                        && sniperPointsForEnemyBool[myPositionNumber])))
                    {
                        distance = tempDistanceBetweenEnemyAndPosition;
                        closestPosition = sniperPointsForEnemy[i].position;
                        tempNumber = i;
                    }
                }
            }

            if (closestPosition != Vector3.zero)
                sniperPointsForEnemyBool[tempNumber] = true;

            if (myPositionNumber != tempNumber)
            {
                if (myPositionNumber > -1)
                    sniperPointsForEnemyBool[myPositionNumber] = false;

                if (closestPosition == Vector3.zero)
                    myPositionNumber = -1;
                else
                    myPositionNumber = tempNumber;
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
