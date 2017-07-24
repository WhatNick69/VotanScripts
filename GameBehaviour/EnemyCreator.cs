using AbstractBehaviour;
using EnemyBehaviour;
using MovementEffects;
using System.Collections.Generic;
using UnityEngine;
using System;
using VotanLibraries;

namespace GameBehaviour
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
        [SerializeField, Tooltip("Время между волнами"), Range(0.5f, 10)]
        private float timeToWave;
        [SerializeField, Tooltip("Random радиус"), Range(3, 25)]
        private float randomRadius;
        [SerializeField, Tooltip("Количество одновременных противников"), Range(5, 50)]
        private int oneTimeEnemies;
        [SerializeField,Tooltip("Точка респауна врагов")]
        private Transform respawnPoint;

        private int tempEnemiesForWave;
        #endregion

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
                        yield return Timing.WaitForSeconds(0.1f);
                    }
                }

                GrowNumberOfEnemiesForNextWave();
                w++;
                e = 0;
                yield return Timing.WaitForSeconds(timeToWave);
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
            GameObject enemyObjNew = Instantiate(enemyList[RandomEnemyChoice()]);
            enemyObjNew.transform.parent = respawnPoint;
            enemyObjNew.GetComponent<EnemyMove>().RandomRadius = randomRadius;
			enemyObjNew.transform.position = respawnPoint.transform.position;
            enemyObjNew.name = "Enemy" + k;
            StaticStorageWithEnemies.AddToList
                (enemyObjNew.GetComponent<AbstractEnemy>());
        }

        /// <summary>
        /// Случайный выбор противника
        /// </summary>
        /// <returns></returns>
        private int RandomEnemyChoice()
        {
            return LibraryStaticFunctions.rnd.Next(0, 2);
        }

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Start()
        { 
            Timing.RunCoroutine(CoroutineInstantiate());	
		}
    }
}
