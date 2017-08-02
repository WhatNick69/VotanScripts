using AbstractBehaviour;
using EnemyBehaviour;
using MovementEffects;
using System.Collections.Generic;
using UnityEngine;
using System;
using VotanLibraries;
using VotanInterfaces;
using PlayerBehaviour;

namespace VotanGameplay
{
    /// <summary>
    /// Создатель противников.
    /// Реализовано при помощи стэка.
    /// Стэк реализован при помощи массива.
    /// </summary>
    public class EnemyCreator 
        : MonoBehaviour
    {
        #region Переменные
        [SerializeField]
		private GameObject[] enemyList;
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
        #endregion

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Start()
        {
            stackLenght = (int)(oneTimeEnemies * 1.2f);
            StaticStorageWithEnemies.CountOfEnemies = 0;
            StaticStorageWithEnemies.ListEnemy = new AbstractEnemy[stackLenght];
            InitialisationOfEnemies();
            GiveEnemyArrayReferenceToAllPlayers();
            Timing.RunCoroutine(CoroutineEnemiesSpawn());
        }

        /// <summary>
        /// Предоставить ссылку на массив врагом всем игрокам
        /// </summary>
        private void GiveEnemyArrayReferenceToAllPlayers()
        {
            for (int i = 0;i<AllPlayerManager.PlayerList.Count;i++)
            {
                AllPlayerManager.PlayerList[i].
                    GetComponent<PlayerComponentsControl>().PlayerAttack.GetReferenceToEnemyArray();
            }
        }

        /// <summary>
        /// Инициализация всех врагов. Тут можно шаманить с их количеством и качеством.
        /// </summary>
        private void InitialisationOfEnemies()
        {
            for (int i = 0; i < stackLenght; i++)
            {
                RandomEnemyChoice();
                enemyObjNew = Instantiate(enemyList[tempEnemyIndexNumber]);
                enemyObjNew.GetComponent<IEnemyBehaviour>().EnemyNumber = i;
                enemyObjNew.transform.parent = respawnPoint;
                enemyObjNew.GetComponent<EnemyMove>().RandomRadius = randomRadius;
                enemyObjNew.transform.position = respawnPoint.transform.position;
                enemyObjNew.name = enemyObjNew.name + "#" + i;

                StaticStorageWithEnemies.ListEnemy[i] = enemyObjNew.GetComponent<AbstractEnemy>();
            }
        }

        /// <summary>
        /// Случайный выбор противника для инстанса
        /// </summary>
        /// <returns></returns>
        private void RandomEnemyChoice()
        {
            tempEnemyIndexNumber =
                LibraryStaticFunctions.rnd.Next(0, enemyList.Length);
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
        /// Корутина для создания врагов
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineEnemiesSpawn()
        {
            int e = 0;
            int w = 0;
            while (w < waves)
            {
                while (e < perWaveEnemiesNumber)
                {
                    if (StaticStorageWithEnemies.CountOfEnemies < oneTimeEnemies)
                    {
                        yield return Timing.WaitForSeconds(timeToInstantiate);

                        EnemyRestart();
                        e++;
                    }
                    else
                    {
                        yield return Timing.WaitForSeconds(timeToInstantiate/2);
                    }
                }

                if (StaticStorageWithEnemies.CountOfEnemies == 0)
                {
                    GrowNumberOfEnemiesForNextWave();
                    w++;
                    e = 0;
                }
                yield return Timing.WaitForSeconds(timeToInstantiate/2);
            }
            GameManager.IsWin = true;
            SendToPlayersCallOfWin();
        }

        /// <summary>
        /// Послать всем игрокам сигнал о победе
        /// </summary>
        private void SendToPlayersCallOfWin()
        {
            foreach (GameObject player in AllPlayerManager.PlayerList)
            {
                player.GetComponent<PlayerUI>().EventWin();
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
        /// Задать параметры для моба
        /// </summary>
        /// <param name="number">Номер противника в стэке</param>
        private void SetEnemyParameters(int number)
        {
            switch (StaticStorageWithEnemies.ListEnemy[number].EnemyType)
            {
                // ОБЫЧНЫЙ РЫЦАРЬ
                case EnemyType.Knight:
                    StaticStorageWithEnemies.ListEnemy[number].GetComponent<IEnemyBehaviour>().
                        EnemyMove.PreDistanceForAttack = 0.5f;   // Предупредительная дистанция для атаки
                    StaticStorageWithEnemies.ListEnemy[number]
                        .GetComponent<IEnemyBehaviour>().EnemyMove.AgentSpeed = 3; // Скорость передвижения моба
                    StaticStorageWithEnemies.ListEnemy[number]
                        .GetComponent<IEnemyBehaviour>().EnemyAttack.DmgEnemy = 20; // Урон моба
                    StaticStorageWithEnemies.ListEnemy[number]
                        .GetComponent<IEnemyBehaviour>().EnemyConditions.SetHealthParameter
                        (LibraryStaticFunctions.GetRangeValue(200, 0.1f)); // Установить жизни мобу

                    StaticStorageWithEnemies.ListEnemy[number]
               .        GetComponent<IEnemyBehaviour>().EnemyConditions.PhysicResistance = 
                        LibraryStaticFunctions.GetRangeValue(0.1f,0.2f); // Сопротивление к физической атаке (от 0 до 1)
                    StaticStorageWithEnemies.ListEnemy[number]
                        .GetComponent<IEnemyBehaviour>().EnemyConditions.FireResistance = 0; // Сопротивление к огненной атаке (от 0 до 1)
                    StaticStorageWithEnemies.ListEnemy[number]
                        .GetComponent<IEnemyBehaviour>().EnemyConditions.ElectricResistance = 0; // Сопротивление к электрической атаке
                    StaticStorageWithEnemies.ListEnemy[number].
                        GetComponent<IEnemyBehaviour>().EnemyConditions.FrostResistance = 0; // Сопротивление к ледяной атаке

                    StaticStorageWithEnemies.ListEnemy[number].
                        GetComponent<IEnemyBehaviour>().ScoreAddingEffect.ScoreBonus = 150; // задаем количество очков
                    break;

                // БЕШЕНЫЙ РЫЦАРЬ
                case EnemyType.Crazy:
                    StaticStorageWithEnemies.ListEnemy[number].GetComponent<CrazyEnemy>().
                        FightRotatingSpeed = 800; // Скорость вращения поехавшего
                    StaticStorageWithEnemies.ListEnemy[number].GetComponent<IEnemyBehaviour>().
                        EnemyMove.PreDistanceForAttack = 1.5f;                     // Предупредительная дистанция для атаки)
                    StaticStorageWithEnemies.ListEnemy[number]
                        .GetComponent<IEnemyBehaviour>().EnemyMove.AgentSpeed = 4; // Скорость передвижения моба
                    StaticStorageWithEnemies.ListEnemy[number]
                        .GetComponent<IEnemyBehaviour>().EnemyAttack.DmgEnemy = 10; // Урон моба
                    StaticStorageWithEnemies.ListEnemy[number]
                        .GetComponent<IEnemyBehaviour>().EnemyConditions.SetHealthParameter
                        (LibraryStaticFunctions.GetRangeValue(300, 0.1f)); // Установить жизни мобу

                    StaticStorageWithEnemies.ListEnemy[number]
                        .GetComponent<IEnemyBehaviour>().EnemyConditions.PhysicResistance =
                        LibraryStaticFunctions.GetRangeValue(0.1f, 0.2f); ; // Сопротивление к физической атаке (от 0 до 1)
                    StaticStorageWithEnemies.ListEnemy[number]
                        .GetComponent<IEnemyBehaviour>().EnemyConditions.FireResistance =
                        LibraryStaticFunctions.GetRangeValue(0.1f, 0.2f); ; // Сопротивление к огненной атаке (от 0 до 1)
                    StaticStorageWithEnemies.ListEnemy[number]
                        .GetComponent<IEnemyBehaviour>().EnemyConditions.ElectricResistance =
                        LibraryStaticFunctions.GetRangeValue(0.1f, 0.2f); ; // Сопротивление к электрической атаке
                    StaticStorageWithEnemies.ListEnemy[number].
                        GetComponent<IEnemyBehaviour>().EnemyConditions.FrostResistance =
                        LibraryStaticFunctions.GetRangeValue(0.1f, 0.2f); ; // Сопротивление к ледяной атаке

                    StaticStorageWithEnemies.ListEnemy[number].
                        GetComponent<IEnemyBehaviour>().ScoreAddingEffect.ScoreBonus = 300; // задаем количество очков
                    break;

                // ПЕРВЫЙ БОСС
                case EnemyType.FirstBoss:
                    StaticStorageWithEnemies.ListEnemy[number]
                        .GetComponent<IEnemyBehaviour>().EnemyConditions.PhysicResistance = 0.3f;
                    StaticStorageWithEnemies.ListEnemy[number]
                        .GetComponent<IEnemyBehaviour>().EnemyConditions.FireResistance = 0.3f;
                    StaticStorageWithEnemies.ListEnemy[number]
                        .GetComponent<IEnemyBehaviour>().EnemyConditions.ElectricResistance = 0.3f;
                    StaticStorageWithEnemies.ListEnemy[number].
                        GetComponent<IEnemyBehaviour>().EnemyConditions.FrostResistance = 0.3f;
                    StaticStorageWithEnemies.ListEnemy[number].
                        GetComponent<IEnemyBehaviour>().ScoreAddingEffect.ScoreBonus = 5000;
                    break;
            }
            StaticStorageWithEnemies.ListEnemy[number]
                .GetComponent<IEnemyInStack>().RestartEnemy();
        }
    }
}
