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
    /// Создатель противников
    /// </summary>
    public class EnemyCreator 
        : MonoBehaviour
    {
        #region Переменные
        [SerializeField]
		private GameObject[] enemyList;
        [SerializeField, Tooltip("Количество врагов для генерации"), Range(1, 500)]
        private int perWaveEnemiesNumber;
        [SerializeField, Tooltip("Количество врагов для генерации"), Range(1, 500)]
        private int sumEnemiesNumber;
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
        GameObject enemyObjNew;
        #endregion

        private void Start()
        {
            Timing.RunCoroutine(CoroutineInstantiate());
        }

        /// <summary>
        /// Корутина для создания врагов
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineInstantiate()
        {
            int e = 0;
            int w = 0;
            while (w < waves)
            {
                while (e < perWaveEnemiesNumber)
                {
                    if (StaticStorageWithEnemies.GetCountListOfEnemies() <= oneTimeEnemies)
                    {
                        yield return Timing.WaitForSeconds(timeToInstantiate);

                        InstantiateOnServer(e);
                        e++;
                    }
                    else
                    {
                        yield return Timing.WaitForSeconds(timeToInstantiate/2);
                    }
                }

                if (StaticStorageWithEnemies.GetCountListOfEnemies() == 0)
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

        private void SendToPlayersCallOfWin()
        {
            foreach (GameObject player in AllPlayerManager.PlayerList)
            {
                player.GetComponent<PlayerUI>().EventWin();
            }
        }

        /// <summary>
        /// Увеличиваем количество проитвников для следующей волны
        /// </summary>
        private void GrowNumberOfEnemiesForNextWave()
        {
            perWaveEnemiesNumber = Convert.ToInt32(perWaveEnemiesNumber * 1.2f);
            if (perWaveEnemiesNumber > sumEnemiesNumber) perWaveEnemiesNumber = sumEnemiesNumber;
        }

        /// <summary>
        /// Инстанцирование врага на сцене
        /// </summary>
        private void InstantiateOnServer(int k)
        {
            RandomEnemyChoice();
            enemyObjNew = Instantiate(enemyList[tempEnemyIndexNumber]);
            SetEnemyParameters();

            enemyObjNew.transform.parent = respawnPoint;
            enemyObjNew.GetComponent<EnemyMove>().RandomRadius = randomRadius;
			enemyObjNew.transform.position = respawnPoint.transform.position;
            enemyObjNew.name = enemyObjNew.name+"#" + k;
            StaticStorageWithEnemies.AddToList
                (enemyObjNew.GetComponent<AbstractEnemy>());
        }

        /// <summary>
        /// Задать параметры для моба.
        /// </summary>
        private void SetEnemyParameters()
        {
            enemyObjNew.GetComponent<IEnemyBehaviour>().
                EnemyMove.AgentSpeed = 4; // Скорость передвижения моба
            enemyObjNew.GetComponent<IEnemyBehaviour>().
                EnemyAttack.DmgEnemy = 15; // Урон моба
            enemyObjNew.GetComponent<IEnemyBehaviour>().
                EnemyConditions.SetHealthParameter(100); // Установить жизни мобу
            enemyObjNew.GetComponent<IEnemyBehaviour>().
                EnemyConditions.PhysicResistance = 0.1f; // Сопротивление к физической атаке (от 0 до 1)
            enemyObjNew.GetComponent<IEnemyBehaviour>().
                EnemyConditions.FireResistance = 0.1f; // Сопротивление к огненной атаке (от 0 до 1)
            enemyObjNew.GetComponent<IEnemyBehaviour>().
                EnemyConditions.ElectricResistance = 0.1f; // Сопротивление к электрической атаке
            enemyObjNew.GetComponent<IEnemyBehaviour>().
                EnemyConditions.FrostResistance = 0.1f; // Сопротивление к ледяной атаке


            switch (tempEnemyIndexNumber)
            {
                case 0: // РЫЦАРЬ
                    enemyObjNew.GetComponent<IEnemyBehaviour>().
                        EnemyMove.PreDistanceForAttack = 0.5f;
                    // Предупредительная дистанция для атаки (чтобы бил заранее, окда? ;) )
                    break;
                case 1: // ПОЕХАВШИЙ БЕРСЕРКЕР
                    enemyObjNew.GetComponent<CrazyEnemy>().
                        FightRotatingSpeed = 800; // Скорость вращения поехавшего
                    enemyObjNew.GetComponent<IEnemyBehaviour>().
                        EnemyMove.PreDistanceForAttack = 1.5f; 
                    // Предупредительная дистанция для атаки)
                    break;
            }
        }

        /// <summary>
        /// Случайный выбор противника
        /// </summary>
        /// <returns></returns>
        private void RandomEnemyChoice()
        {
            tempEnemyIndexNumber = 
                LibraryStaticFunctions.rnd.Next(0, enemyList.Length);
        }
    }
}
