using MovementEffects;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using VotanLibraries;

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
        [SerializeField, Tooltip("Скорость врага"), Range(1, 5)]
        private float agentSpeed;

        /// <summary>
        /// Задаем 
        /// </summary>
        private void RandomSpeedSet()
        {
            agent.speed = agentSpeed + (float)((LibraryPlayerPosition.rnd.NextDouble() * 2 - 1) * agentSpeed * 0.1f);
            Debug.Log(gameObject.name + " my speed is: " + agent.speed.ToString());
        }

        public void SetNewSpeedOfNavMeshAgent(float newValue)
        {
            agent.speed = newValue;
        }

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            playerUnitTransform = GameObject.FindWithTag("Player").transform;
            isAlive = true;
            RandomSpeedSet();
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
