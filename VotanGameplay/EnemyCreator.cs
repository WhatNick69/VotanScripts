using AbstractBehaviour;
using EnemyBehaviour;
using MovementEffects;
using System.Collections.Generic;
using UnityEngine;
using System;
using VotanLibraries;
using VotanInterfaces;

namespace VotanGameplay
{
    /// <summary>
    /// Создатель противников.
    /// Реализовано при помощи стэка.
    /// Стэк реализован при помощи массива.
    /// 
    /// Реализована логика для режима хардкора.
    /// </summary>
    public class EnemyCreator 
        : MonoBehaviour
    {
        #region Переменные
        [SerializeField,Tooltip("Лист типичных врагов")]
		private GameObject[] enemyList;
        [SerializeField, Tooltip("Это хардкор режим?")]
        private bool isHadrcoreMode;
        [SerializeField, Tooltip("Количество врагов для генерации"), Range(1, 500)]
        private int perWaveEnemiesNumber;
        [SerializeField, Tooltip("Количество волн"), Range(1, 500)]
        private int waves;
        [SerializeField,Tooltip("Время, между генерации противника"),Range(0.5f,5)]
        private float timeToInstantiate;
        [SerializeField, Tooltip("Random радиус"), Range(3, 25)]
        private float randomRadius;
        [SerializeField, Tooltip("Количество одновременных противников"), Range(5, 50)]
        private int oneTimeEnemies;
        [SerializeField,Tooltip("Точка респауна врагов")]
        private Transform respawnPoint;

        private int tempEnemiesForWave;
        private int tempEnemyIndexNumber;
        private GameObject enemyObjNew;
        private int stackLenght;

        private bool isMayToStartNewWave;
        private bool isTimerStillRunning;

        private float nextWaveBoostTimer;
        private float hardcoreMultiplier;
        #endregion

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Start()
        {
            InitiaisationOfPlayMode();
            stackLenght = (int)(oneTimeEnemies * 1.2f);
            StaticStorageWithEnemies.CountOfEnemies = 0;
            StaticStorageWithEnemies.ListEnemy = new AbstractEnemy[stackLenght];
            Timing.RunCoroutine(CoroutineInitialisationOfEnemies()); // первый шаг
        }

        /// <summary>
        /// Инициализация режима игры (хардкор, либо обычный)
        /// </summary>
        private void InitiaisationOfPlayMode()
        {
            if (isHadrcoreMode)
            {
                nextWaveBoostTimer = 7.5f;
                hardcoreMultiplier = 0.25f;
                perWaveEnemiesNumber = 
                    (int)(SetParameterOfEnemy(perWaveEnemiesNumber,hardcoreMultiplier));
                timeToInstantiate = 
                    SetParameterOfEnemy(timeToInstantiate, -hardcoreMultiplier);
            }
            else
            {
                nextWaveBoostTimer = 10;
                hardcoreMultiplier = 0;
            }
        }

        /// <summary>
        /// Таймер на запуск новой волны
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineTimerToStartNewWave()
        {
            isTimerStillRunning = true;
            yield return Timing.WaitForSeconds
                (perWaveEnemiesNumber* nextWaveBoostTimer);
            isMayToStartNewWave = true;
            isTimerStillRunning = false;
        }

        /// <summary>
        /// Корутина для создания врагов
        /// 
        /// Третий шаг
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineEnemiesSpawn()
        {
            int e = 0;
            int w = 0;
            while (w < waves)
            {
                if (!isTimerStillRunning)
                    Timing.RunCoroutine(CoroutineTimerToStartNewWave());

                while (e < perWaveEnemiesNumber)
                {
                    if (StaticStorageWithEnemies.CountOfEnemies < oneTimeEnemies)
                    {
                        yield return Timing.WaitForSeconds(LibraryStaticFunctions.
                            GetRangeValue(timeToInstantiate,0.1f));

                        EnemyRestart();
                        e++;
                    }
                    else
                    {
                        yield return Timing.WaitForSeconds(timeToInstantiate / 2);
                    }
                }

                if (StaticStorageWithEnemies.CountOfEnemies == 0 
                    || (isMayToStartNewWave && w != waves-1))
                {
                    GrowNumberOfEnemiesForNextWave();
                    w++;
                    e = 0;
                    isMayToStartNewWave = false;
                }
                yield return Timing.WaitForSeconds(timeToInstantiate / 2);
            }
            BossRestart();
        }

        /// <summary>
        /// Инициализация всех врагов. Тут можно шаманить с их количеством и качеством.
        /// 
        /// Первый шаг
        /// </summary>
        private IEnumerator<float> CoroutineInitialisationOfEnemies()
        {
            for (int i = 0; i < stackLenght; i++)
            {
                if (i == stackLenght - 1)
                {
                    enemyObjNew = Instantiate(enemyList[enemyList.Length-1]);
                    enemyObjNew.GetComponent<IEnemyBehaviour>().EnemyNumber = i;
                    enemyObjNew.transform.parent = respawnPoint;
                    enemyObjNew.GetComponent<EnemyMove>().RandomRadius = randomRadius;
                    enemyObjNew.transform.position = respawnPoint.transform.position;
                    enemyObjNew.name = enemyObjNew.name + "#" + i;
                }
                else
                {
                    RandomEnemyChoice();
                    enemyObjNew = Instantiate(enemyList[tempEnemyIndexNumber]);
                    enemyObjNew.GetComponent<IEnemyBehaviour>().EnemyNumber = i;
                    enemyObjNew.transform.parent = respawnPoint;
                    enemyObjNew.GetComponent<EnemyMove>().RandomRadius = randomRadius;
                    enemyObjNew.transform.position = respawnPoint.transform.position;
                    enemyObjNew.name = enemyObjNew.name + "#" + i;
                }
                StaticStorageWithEnemies.ListEnemy[i] = enemyObjNew.GetComponent<AbstractEnemy>();
                yield return Timing.WaitForSeconds(Time.deltaTime);
            }
            GiveEnemyArrayReferenceToAllPlayers(); 
            Timing.RunCoroutine(CoroutineEnemiesSpawn()); 
        }

        /// <summary>
        /// Предоставить ссылку на массив врагом всем игрокам
        /// 
        /// Второй шаг
        /// </summary>
        private void GiveEnemyArrayReferenceToAllPlayers()
        {
            for (int i = 0; i < AllPlayerManager.PlayerComponentsList.Count; i++)
            {
                AllPlayerManager.PlayerComponentsList[i]
                    .PlayerAttack.GetReferenceToEnemyArray();
            }
        }

        /// <summary>
        /// Случайный выбор противника для инстанса
        /// </summary>
        /// <returns></returns>
        private void RandomEnemyChoice()
        {
            tempEnemyIndexNumber =
                UnityEngine.Random.Range(0, enemyList.Length-1);
        }

        /// <summary>
        /// Вернуть врага в стэк
        /// </summary>
        /// <param name="enemyNumber">Номер врага в стэке</param>
        public static void ReturnEnemyToStack(int enemyNumber)
        {
            StaticStorageWithEnemies.ListEnemy[enemyNumber].gameObject.SetActive(false);
            StaticStorageWithEnemies.CountOfEnemies--;
        }

        /// <summary>
        /// Рестартировать врага
        /// </summary>
        private void EnemyRestart()
        {
            int emptyEnemyNumber = StaticStorageWithEnemies.GetNumberOfEmptyEnemy();
            SetEnemyParameters(emptyEnemyNumber);

            StaticStorageWithEnemies.ListEnemy[emptyEnemyNumber]
                .transform.position = respawnPoint.transform.position;
            StaticStorageWithEnemies.ListEnemy[emptyEnemyNumber]
                .gameObject.SetActive(true);
            StaticStorageWithEnemies.CountOfEnemies++;
        }

        /// <summary>
        /// Рестартировать босса
        /// </summary>
        private void BossRestart()
        {
            SetEnemyParameters(StaticStorageWithEnemies.ListEnemy.Length-1);
            BossInPlay();
        }

        /// <summary>
        /// Вступление босса
        /// </summary>
        private void BossInPlay()
        {
            StaticStorageWithEnemies.ListEnemy[StaticStorageWithEnemies.ListEnemy.Length-1]
                .transform.position = respawnPoint.transform.position;
            StaticStorageWithEnemies.ListEnemy[StaticStorageWithEnemies.ListEnemy.Length-1]
                .gameObject.SetActive(true);
        }

        /// <summary>
        /// Послать всем игрокам сигнал о победе
        /// </summary>
        public static void SendToPlayersCallOfWin()
        {
            GameManager.IsWin = true;
            for (int i = 0;i< AllPlayerManager.PlayerComponentsList.Count;i++)
            {
                AllPlayerManager.PlayerComponentsList[i].PlayerUI.EventWin();
            }
        }

        /// <summary>
        /// Увеличиваем количество противников для следующей волны
        /// </summary>
        private void GrowNumberOfEnemiesForNextWave()
        {
            perWaveEnemiesNumber = Convert.ToInt32(perWaveEnemiesNumber * 1.2f);
        }     

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="multiplier"></param>
        /// <returns></returns>
        private float SetParameterOfEnemy(float value,float multiplier)
        {
            return value * (1 + multiplier);
        }

        /// <summary>
        /// Задать параметры для моба
        /// </summary>
        /// <param name="number">Номер противника в стэке</param>
        private void SetEnemyParameters(int number)
        {
            IEnemyBehaviour enemyBehaviour = 
                StaticStorageWithEnemies.ListEnemy[number].GetComponent<IEnemyBehaviour>();
            switch (enemyBehaviour.EnemyType)
            {
                // ОБЫЧНЫЙ РЫЦАРЬ
                case EnemyType.Knight:
                    enemyBehaviour.EnemyMove.PreDistanceForAttack = 0.1f;
                    enemyBehaviour.EnemyMove.AgentSpeed =
                        SetParameterOfEnemy(2.5f,hardcoreMultiplier); // Скорость передвижения моба
                    enemyBehaviour.EnemyAttack.DmgEnemy =
                        SetParameterOfEnemy(10,hardcoreMultiplier); // Урон моба
                    enemyBehaviour.EnemyConditions.SetHealthParameter
                        (LibraryStaticFunctions.GetRangeValue
                        (SetParameterOfEnemy(100,hardcoreMultiplier), 0.1f)); // Установить жизни мобу

                    enemyBehaviour.EnemyConditions.PhysicResistance = 
                        LibraryStaticFunctions.GetRangeValue
                        (SetParameterOfEnemy(0.1f,hardcoreMultiplier), 0.2f); // Сопротивление к физической атаке (от 0 до 1)
                    enemyBehaviour.EnemyConditions.FireResistance = 0; // Сопротивление к огненной атаке (от 0 до 1)
                    enemyBehaviour.EnemyConditions.ElectricResistance = 0; // Сопротивление к электрической атаке
                    enemyBehaviour.EnemyConditions.FrostResistance = 0; // Сопротивление к ледяной атаке

                    enemyBehaviour.ScoreAddingEffect.ScoreBonus = 
                        (int)SetParameterOfEnemy(200,hardcoreMultiplier*2); // задаем количество очков
                    break;

                // БЕШЕНЫЙ РЫЦАРЬ
                case EnemyType.Crazy:
                    StaticStorageWithEnemies.ListEnemy[number].GetComponent<CrazyEnemy>().
                        FightRotatingSpeed = 
                        SetParameterOfEnemy(800,hardcoreMultiplier); // Скорость вращения поехавшего
                    // Предупредительная дистанция для атаки
                    enemyBehaviour.EnemyMove.PreDistanceForAttack = 1.5f;
                    enemyBehaviour.EnemyMove.AgentSpeed = 
                        SetParameterOfEnemy(3,hardcoreMultiplier); // Скорость передвижения моба
                    enemyBehaviour.EnemyAttack.DmgEnemy =
                        SetParameterOfEnemy(5,hardcoreMultiplier); // Урон моба
                    enemyBehaviour.EnemyConditions.SetHealthParameter
                        (LibraryStaticFunctions.GetRangeValue
                        (SetParameterOfEnemy(150,hardcoreMultiplier), 0.1f)); // Установить жизни мобу

                    enemyBehaviour.EnemyConditions.PhysicResistance =
                        LibraryStaticFunctions.GetRangeValue
                        (SetParameterOfEnemy(0.1f,hardcoreMultiplier), 0.15f); // Сопротивление к физической атаке (от 0 до 1)
                    enemyBehaviour.EnemyConditions.FireResistance =
                        LibraryStaticFunctions.GetRangeValue
                        (SetParameterOfEnemy(0.1f, hardcoreMultiplier), 0.1f);  // Сопротивление к огненной атаке (от 0 до 1)
                    enemyBehaviour.EnemyConditions.ElectricResistance =
                        LibraryStaticFunctions.GetRangeValue
                        (SetParameterOfEnemy(0.1f, hardcoreMultiplier), 0.05f); // Сопротивление к электрической атаке
                    enemyBehaviour.EnemyConditions.FrostResistance =
                        LibraryStaticFunctions.GetRangeValue
                        (SetParameterOfEnemy(0.1f, hardcoreMultiplier), 0.1f);  // Сопротивление к ледяной атаке

                    enemyBehaviour.ScoreAddingEffect.ScoreBonus = 
                        (int)SetParameterOfEnemy(400, hardcoreMultiplier*2); // задаем количество очков
                    break;

                // АЛЕБАРДИЙЩИК
                case EnemyType.Halberdier:
                    // Предупредительная дистанция для атаки
                    enemyBehaviour.EnemyMove.PreDistanceForAttack = 2;
                    enemyBehaviour.EnemyMove.AgentSpeed =
                        SetParameterOfEnemy(2.5f, hardcoreMultiplier); // Скорость передвижения моба
                    enemyBehaviour.EnemyAttack.DmgEnemy =
                        SetParameterOfEnemy(20, hardcoreMultiplier); // Урон моба
                    enemyBehaviour.EnemyConditions.SetHealthParameter
                        (LibraryStaticFunctions.GetRangeValue
                        (SetParameterOfEnemy(200, hardcoreMultiplier), 0.1f)); // Установить жизни мобу

                    enemyBehaviour.EnemyConditions.PhysicResistance =
                        LibraryStaticFunctions.GetRangeValue
                        (SetParameterOfEnemy(0.1f, hardcoreMultiplier), 0.2f); // Сопротивление к физической атаке (от 0 до 1)
                    enemyBehaviour.EnemyConditions.FireResistance =
                        LibraryStaticFunctions.GetRangeValue
                        (SetParameterOfEnemy(0.1f, hardcoreMultiplier), 0.2f);  // Сопротивление к огненной атаке (от 0 до 1)
                    enemyBehaviour.EnemyConditions.ElectricResistance =
                        LibraryStaticFunctions.GetRangeValue
                        (SetParameterOfEnemy(0.1f, hardcoreMultiplier), 0.2f); // Сопротивление к электрической атаке
                    enemyBehaviour.EnemyConditions.FrostResistance =
                        LibraryStaticFunctions.GetRangeValue
                        (SetParameterOfEnemy(0.1f, hardcoreMultiplier), 0.2f);  // Сопротивление к ледяной атаке

                    enemyBehaviour.ScoreAddingEffect.ScoreBonus =
                        (int)SetParameterOfEnemy(600, hardcoreMultiplier * 2); // задаем количество очков
                    break;

                // ПЕРВЫЙ БОСС
                case EnemyType.FirstBoss:
                    // Скорость передвижения моба
                    enemyBehaviour.EnemyMove.AgentSpeed =
                        SetParameterOfEnemy(2, hardcoreMultiplier);
                    enemyBehaviour.EnemyAttack.DmgEnemy =
                        SetParameterOfEnemy(40,hardcoreMultiplier); // Урон моба
                    enemyBehaviour.EnemyConditions.SetHealthParameter
                        (LibraryStaticFunctions.GetRangeValue
                        (SetParameterOfEnemy(300,hardcoreMultiplier), 0.05f)); // Установить жизни мобу

                    enemyBehaviour.EnemyConditions.PhysicResistance = 
                        SetParameterOfEnemy(0.3f,hardcoreMultiplier);
                    enemyBehaviour.EnemyConditions.FireResistance =
                        SetParameterOfEnemy(0.3f, hardcoreMultiplier);
                    enemyBehaviour.EnemyConditions.ElectricResistance = 
                        SetParameterOfEnemy(0.3f, hardcoreMultiplier);
                    enemyBehaviour.EnemyConditions.FrostResistance = 
                        SetParameterOfEnemy(0.3f, hardcoreMultiplier);
                    enemyBehaviour.ScoreAddingEffect.ScoreBonus = 
                        (int)SetParameterOfEnemy(3000, hardcoreMultiplier*2); 
                    break;
            }
            StaticStorageWithEnemies.ListEnemy[number]
                .GetComponent<IEnemyInStack>().RestartEnemy();
        }
    }
}
