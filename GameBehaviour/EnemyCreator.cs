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
        private PlayerAttack playerAttack;
        [SerializeField]
        private List<AbstractEnemy> listEnemy;
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
                listEnemy = playerAttack.ReturnList();
                //listEnemy[k].transform.position = new Vector3(Random.Range(-7, 7), 1.5f, Random.Range(-7, 7));
                listEnemy[k].transform.position = respawnPoint.transform.position;
                listEnemy[k].AbsAttack.SetPlayerPoint(0, playerAttack.PlayerPosition(0));
				listEnemy[k].AbsAttack.SetPlayerPoint(1, playerAttack.PlayerPosition(1));
				listEnemy[k].AbsAttack.SetPlayerPoint(2, playerAttack.PlayerPosition(2));
				listEnemy[k].AbsAttack.SetPlayerPoint(3, playerAttack.PlayerPosition(3));
				listEnemy[k].enAnim = enemy.GetComponent<Animator>();
				k++;
            }
        }

        /// <summary>
        /// Инстанцирование врага на сцене
        /// </summary>
        private void InstantiateOnServer()
        {
            GameObject enemyObjNew = Instantiate(enemy);
            playerAttack.AddEnemyToList(enemyObjNew.GetComponent<AbstractEnemy>());
            enemyObjNew.GetComponent<EnemyMove>().RandomRadius = randomRadius;
        }

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Start()
        {
            listEnemy = new List<AbstractEnemy>();
            playerAttack = GameObject.FindWithTag("Player").GetComponent<PlayerAttack>();
            Timing.RunCoroutine(CoroutineInstantiate());
        }
    }
}
