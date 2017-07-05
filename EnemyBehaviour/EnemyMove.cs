using AbstractBehaviour;
using MovementEffects;
using Playerbehaviour;
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
        #region Переменные
        private NavMeshAgent agent;
        private Transform playerObjectTransformForFollow;
        private PlayerConditions playerConditions;
        private bool isAlive;
        [SerializeField, Tooltip("Частота обновления позиции врага"),Range(0.1f,3)]
        private float frequencySearching;
        [SerializeField, Tooltip("Скорость врага"), Range(1, 5)]
        private float agentSpeed;
        [SerializeField, Tooltip("Враг")]
        private AbstractEnemy abstractEnemy;

        private float randomRadius;
        private float rotationSpeed;

        private bool isStopped;
        private Quaternion lerpRotationQuar;
        private float angularSpeedForLerpRotation;
        #endregion

        #region Свойства
        /// <summary>
        /// Возвращает дистанцию остановки до игрока
        /// </summary>
        /// <returns></returns>
        private bool CheckStopped()
        {
            isStopped =  Vector3.Distance(transform.position, playerObjectTransformForFollow.position) 
                <= agent.stoppingDistance ? true : false;
            return isStopped;
        }

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

        public bool IsStopped
        {
            get
            {
                return isStopped;
            }

            set
            {
                isStopped = value;
            }
        }
        #endregion

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
            if (!isAlive) return;
            agent.speed = newValue;
            if (newRotSpeed != 0)
                agent.angularSpeed = newRotSpeed;
        }

        /// <summary>
        /// Получить игрока и его компонент
        /// </summary>
        private bool GetPlayerAndComponent()
        {
            playerObjectTransformForFollow = abstractEnemy.EnemyOpponentChoiser.GetRandomTransformOfPlayer();


            if (playerObjectTransformForFollow != null)
            {
                abstractEnemy.EnemyAttack.PlayerTarget = playerObjectTransformForFollow.GetComponent<PlayerAttack>();

                if (!GetComponent<AbstractAttack>()) return false;
                AbstractAttack absA = GetComponent<AbstractAttack>();
                absA.SetPlayerPoint(0, abstractEnemy.EnemyAttack.PlayerTarget.PlayerPosition(0));
                absA.SetPlayerPoint(1, abstractEnemy.EnemyAttack.PlayerTarget.PlayerPosition(1));
                absA.SetPlayerPoint(2, abstractEnemy.EnemyAttack.PlayerTarget.PlayerPosition(2));
                absA.SetPlayerPoint(3, abstractEnemy.EnemyAttack.PlayerTarget.PlayerPosition(3));

                playerConditions = playerObjectTransformForFollow.GetComponent<PlayerConditions>();
                playerObjectTransformForFollow = playerObjectTransformForFollow.
                    GetComponent<PlayerComponentsControl>().PlayerModel;

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            GetPlayerAndComponent();

            isAlive = true;
            angularSpeedForLerpRotation = agent.angularSpeed/3000;
            RandomSpeedSet();
            Timing.RunCoroutine(CoroutineForSearchingByPlayerObject());
        }

        /// <summary>
        /// Движение противника. Корутина.
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForSearchingByPlayerObject()
        {
            agent.enabled = true;
            while (isAlive)
            {
                if (playerObjectTransformForFollow != null)
                {
                    if (playerConditions.IsAlive)
                    {
                        if (agent != null)
                        {
                            agent.SetDestination
                                (playerObjectTransformForFollow.position);
                            if (CheckStopped()) LookAtPlayerObject();
                        }
                        yield return Timing.WaitForSeconds(frequencySearching);
                    }
                    else
                    {
                        if (!GetPlayerAndComponent())
                        {
                            SetRandomPosition();
                            yield return Timing.WaitForSeconds
                                ((float)LibraryStaticFunctions.rnd.NextDouble()
                                * 10 + 10);
                        }
                        else
                        {
                            yield return Timing.WaitForSeconds(frequencySearching);
                        }
                    }
                }
                else
                {
                    SetRandomPosition();
                    yield return Timing.WaitForSeconds
                        ((float)LibraryStaticFunctions.rnd.NextDouble()
                        * 10 + 10);
                }
            }
        }

        public void Update()
        {
            if (isStopped) transform.rotation =
                    Quaternion.Lerp(transform.rotation
                    , lerpRotationQuar, angularSpeedForLerpRotation);
        }

        public void LookAtPlayerObject()
        {
            lerpRotationQuar = Quaternion.LookRotation(playerObjectTransformForFollow.position-transform.position);
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
