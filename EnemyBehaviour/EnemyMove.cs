using MovementEffects;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyBehaviour
{
    /// <summary>
    /// Движение врага
    /// </summary>
    public class EnemyMove
        : MonoBehaviour
    {
        private NavMeshAgent agent;
        private Transform playerUnitTransform;
        private bool isAlive;
        [SerializeField, Tooltip("Частота обновления позиции врага"),Range(0.1f,3)]
        private float frequencySearching;

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            playerUnitTransform = GameObject.FindWithTag("Player").transform;
            isAlive = true;
            Timing.RunCoroutine(CoroutineForSearchingByPlayerObject());
        }

        /// <summary>
        /// Движение противника. Корутина.
        /// </summary>
        /// <returns></returns>
        IEnumerator<float> CoroutineForSearchingByPlayerObject()
        {
            while (isAlive)
            {
                if (agent != null)
                {
                    agent.SetDestination(playerUnitTransform.position);
                }
                yield return Timing.WaitForSeconds(frequencySearching);
            }
        }
    }
}
