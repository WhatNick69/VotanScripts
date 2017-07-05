using AbstractBehaviour;
using EnemyBehaviour;
using MovementEffects;
using PlayerBehaviour;
using System.Collections.Generic;
using UnityEngine;

namespace GameBehaviour
{
    /// <summary>
    /// Создатель противников
    /// </summary>
    public class EnemyCreator 
        : MonoBehaviour
    {
		[SerializeField]
		private GameObject enemy;
        [SerializeField, Tooltip("Количество врагов для генерации"), Range(1, 500)]
        private float players;
        [SerializeField,Tooltip("Время, между генерации противника"),Range(0.5f,5)]
        private float timeToInstantiate;
        [SerializeField, Tooltip("Random радиус"), Range(3, 25)]
        private float randomRadius;

        [SerializeField]
        private Transform respawnPoint;

        /// <summary>
        /// Корутина для создания врагов
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineInstantiate()
        {
            int k = 0;
            while (k < players)
            {
                yield return Timing.WaitForSeconds(timeToInstantiate);

                InstantiateOnServer();
				k++;
            }
        }

        /// <summary>
        /// Инстанцирование врага на сцене
        /// </summary>
        private void InstantiateOnServer()
        {
            GameObject enemyObjNew = Instantiate(enemy);
            enemyObjNew.GetComponent<EnemyMove>().RandomRadius = randomRadius;
			enemyObjNew.transform.position = respawnPoint.transform.position;

            AllPlayerManager.CmdAddEnemyToList(enemyObjNew);
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
