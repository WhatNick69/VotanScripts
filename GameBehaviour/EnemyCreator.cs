using AbstractBehaviour;
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
        private List<AbstractEnemy> listEnemy;
        private int k = 0;
        [SerializeField,Tooltip("Время, между генерации противника"),Range(0.5f,5)]
        private float timeToInstantiate;

        /// <summary>
        /// Корутина для создания врагов
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineInstantiate()
        {
            while (k < 500)
            {
                yield return Timing.WaitForSeconds(timeToInstantiate);

                playerAttack.AddEnemyToList(Instantiate(enemy).GetComponent<AbstractEnemy>());
                listEnemy = playerAttack.ReturnList();
                listEnemy[k].transform.position = new Vector3(Random.Range(-7, 7), 1.5f, Random.Range(-7, 7));
				listEnemy[k].SetPlayerPoint(0, playerAttack.PlayerPosition(0));
				listEnemy[k].SetPlayerPoint(1, playerAttack.PlayerPosition(1));
				listEnemy[k].SetPlayerPoint(2, playerAttack.PlayerPosition(2));
				listEnemy[k].SetPlayerPoint(3, playerAttack.PlayerPosition(3));
				
				k++;
            }
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
