using GameBehaviour;
using MovementEffects;
using PlayerBehaviour;
using System;
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
        private Transform playerObjectTransformForFollow;
        private bool isAlive;
        [SerializeField, Tooltip("Частота обновления позиции врага"),Range(0.1f,3)]
        private float frequencySearching;
        [SerializeField, Tooltip("Скорость врага"), Range(1, 5)]
        private float agentSpeed;

        private float randomRadius;
        private float rotationSpeed;

        public float RandomRadius
        {
            get
            {
                return randomRadius;
            }

            set
            {
                randomRadius = value;
            }
        }

        public float AgentSpeed
        {
            get
            {
                return agentSpeed;
            }

            set
            {
                agentSpeed = value;
            }
        }

        public float RotationSpeed
        {
            get
            {
                return rotationSpeed;
            }

            set
            {
                rotationSpeed = value;
            }
        }

        /// <summary>
        /// Задаем 
        /// </summary>
        private void RandomSpeedSet()
        {
            agent.speed = LibraryStaticFunctions.GetPlusMinusVal(agentSpeed, 0.25f);
            agentSpeed = agent.speed;
            rotationSpeed = agent.angularSpeed;
        }

        /// <summary>
        /// Установить новую скорость для агента
        /// </summary>
        /// <param name="newValue"></param>
        public void SetNewSpeedOfNavMeshAgent(float newValue,float newRotSpeed=0)
        {
            agent.speed = newValue;
            if (newRotSpeed != 0)
                agent.angularSpeed = newRotSpeed;
        }

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            playerObjectTransformForFollow = GameObject.FindWithTag("Player")
                .GetComponent<PlayerController>().PlayerObjectTransform;
            isAlive = true;
            RandomSpeedSet();
            Timing.RunCoroutine(CoroutineForSearchingByPlayerObject());
        }

        /// <summary>
        /// Движение противника. Корутина.
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForSearchingByPlayerObject()
        {
            while (isAlive)
            {
                if (LibraryPlayerPosition.PlayerConditions.IsAlive)
                {
                    if (agent != null)
                        agent.SetDestination(playerObjectTransformForFollow.position);
                    yield return Timing.WaitForSeconds(frequencySearching);
                }
                else
                {
                    SetRandomPosition();
                    yield return Timing.WaitForSeconds
                        ((float)LibraryStaticFunctions.rnd.NextDouble() * 10 + 10);
                }
            }
        }

        /// <summary>
        /// Установить случайную позицию для врага во время отдыха
        /// </summary>
        private void SetRandomPosition()
        {
            if (agent != null)
            {
                float x = LibraryStaticFunctions.GetRandomAxisOfEnemyRest(randomRadius);
                float z = LibraryStaticFunctions.GetRandomAxisOfEnemyRest
                    (randomRadius - Math.Abs(x));
                agent.SetDestination(new Vector3(x,3,z));
            }
        }
    }
}
