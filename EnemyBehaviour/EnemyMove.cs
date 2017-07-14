using AbstractBehaviour;
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
    /// Движение врага.
    /// По земле.
    /// </summary>
    public class EnemyMove
        : MonoBehaviour, IAIMoving
    {
        #region Переменные
        private NavMeshAgent agent;
        private Transform playerObjectTransformForFollow;
        private Vector3 randomPosition;
        private PlayerConditions playerConditions;
        [SerializeField, Tooltip("Частота обновления позиции врага"),Range(0.1f,3)]
        private float frequencySearching;
        [SerializeField, Tooltip("Частота интерполяции и отдыха"), Range(0.01f, 3)]
        private float frequencyResting;
        [SerializeField, Tooltip("Скорость врага"), Range(1, 5)]
        private float agentSpeed;
        [SerializeField, Tooltip("Коэффициент анимации при движении"), Range(0.5f,5)]
        private float speedCoefficient;
        [SerializeField, Tooltip("Враг")]
        private AbstractEnemy abstractEnemy;

        private float randomRadius;
        private float rotationSpeed;

        private bool isStopped;
        private Quaternion lerpRotationQuar;
        private float angularLookSpeed;

        private Vector3 currentVector;
        private Vector3 tempVector;
        #endregion

        #region Свойства
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

        public Transform PlayerObjectTransformForFollow
        {
            get
            {
                return playerObjectTransformForFollow;
            }

            set
            {
                playerObjectTransformForFollow = value;
                if (!playerObjectTransformForFollow)
                {
                    isStopped = true;
                    abstractEnemy.EnemyAnimationsController.DisableAllStates();
                    abstractEnemy.EnemyAnimationsController.SetSpeedAnimationByRunSpeed(1);
                }
            }
        }

        public NavMeshAgent Agent
        {
            get
            {
                return agent;
            }

            set
            {
                agent = value;
            }
        }
        #endregion

        /// <summary>
        /// Задаем скорость
        /// </summary>
        private void RandomSpeedSet()
        {
            agent.speed = LibraryStaticFunctions.GetRangeValue(agentSpeed, 0.25f);
            agentSpeed = agent.speed;
            rotationSpeed = agent.angularSpeed;
        }

        /// <summary>
        /// Зависимость скорости воспроизведения 
        /// анимации врага от скорости его движения
        /// </summary>
        public void DependenceAnimatorSpeedOfVelocity()
        {
            if (!isStopped)
            {
               currentVector = transform.position;
               abstractEnemy.EnemyAnimationsController
                   .SetSpeedAnimationByRunSpeed(Vector3.Distance
                   (currentVector,tempVector)* speedCoefficient);
               tempVector = currentVector;
            }
        }

        /// <summary>
        /// Проверить, достиг ли враг точки прибытия
        /// </summary>
        /// <returns></returns>
        public bool CheckStopped(bool isRandom = false)
        {
            if (isRandom)
            {
                randomPosition.y = transform.position.y;
                isStopped = Vector3.Distance(transform.position, randomPosition)
                    <= agent.stoppingDistance ? true : false;
            }
            else
            {
                isStopped = Vector3.Distance
                    (transform.position, playerObjectTransformForFollow.position)
                    <= agent.stoppingDistance ? true : false;
                if (!isStopped && PlayerObjectTransformForFollow)
                    abstractEnemy.EnemyAttack.IsMayToPlayAttackAnimation = true;
            }
            return isStopped;
        }

        /// <summary>
        /// Установить новую скорость для агента
        /// </summary>
        /// <param name="newValue"></param>
        public void SetNewSpeedOfNavMeshAgent(float newValue,float newRotSpeed=0)
        {
            if (!abstractEnemy.EnemyConditions.IsAlive) return;
            agent.speed = newValue;
            if (newRotSpeed != 0)
                agent.angularSpeed = newRotSpeed;
        }

        /// <summary>
        /// Получить игрока и его компонент
        /// </summary>
        public bool GetPlayerAndComponent()
        {
            playerObjectTransformForFollow = 
                abstractEnemy.EnemyOpponentChoiser.GetRandomTransformOfPlayer();

            if (playerObjectTransformForFollow != null)
            {
                abstractEnemy.EnemyAttack.InFightMode = true;

                abstractEnemy.EnemyAttack.PlayerTarget = 
                    playerObjectTransformForFollow.GetComponent<PlayerAttack>();

                if (!GetComponent<AbstractAttack>()) return false;
                AbstractAttack absA = GetComponent<AbstractAttack>();
                absA.SetPlayerPoint(0, abstractEnemy.
                    EnemyAttack.PlayerTarget.PlayerPosition(0));
                absA.SetPlayerPoint(1, abstractEnemy.
                    EnemyAttack.PlayerTarget.PlayerPosition(1));
                absA.SetPlayerPoint(2, abstractEnemy.
                    EnemyAttack.PlayerTarget.PlayerPosition(2));
                absA.SetPlayerPoint(3, abstractEnemy.
                    EnemyAttack.PlayerTarget.PlayerPosition(3));

                playerConditions = playerObjectTransformForFollow.
                    GetComponent<PlayerConditions>();
                playerObjectTransformForFollow = playerObjectTransformForFollow.
                    GetComponent<PlayerComponentsControl>().PlayerModel;

                return true;
            }
            else
            {
                abstractEnemy.EnemyAttack.InFightMode = false;
                return false;
            }
        }

        /// <summary>
        /// Инициализация
        /// </summary>
        public void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            GetPlayerAndComponent();

            angularLookSpeed = Time.deltaTime * 8;
            abstractEnemy.EnemyConditions.IsAlive = true;
            randomPosition.y = 3;
            RandomSpeedSet();
            tempVector = transform.position;
            Timing.RunCoroutine(CoroutineForSearchingByPlayerObject());
            Timing.RunCoroutine(CoroutineForRotating());
        }

        /// <summary>
        /// Движение противника. Корутина.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator<float> CoroutineForSearchingByPlayerObject()
        {
            agent.enabled = true;
            while (abstractEnemy.EnemyConditions.IsAlive)
            {
                if (playerObjectTransformForFollow != null)
                {
                    if (playerConditions.IsAlive)
                    {
                        if (agent != null && agent.enabled)
                        {
                            agent.SetDestination
                                (playerObjectTransformForFollow.position);
                            if (CheckStopped())
                                LookAtPlayerObject();
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
                    CheckStopped(true);
                    SetRandomPosition();
                    yield return Timing.WaitForSeconds
                        ((float)LibraryStaticFunctions.rnd.NextDouble()
                        * 10 + 10);
                }
            }
            isStopped = true;
        }

        /// <summary>
        /// Поворот
        /// </summary>
        public virtual IEnumerator<float> CoroutineForRotating()
        {
            while (abstractEnemy.EnemyConditions.IsAlive)
            {
                if (PlayerObjectTransformForFollow != null)
                {
                    if (isStopped)
                        transform.rotation =
                        Quaternion.Lerp(transform.rotation
                            , lerpRotationQuar, angularLookSpeed);
                }
                else
                {
                    CheckStopped(true);
                }
                yield return Timing.WaitForSeconds(frequencyResting);
            }
        }

        /// <summary>
        /// Поворот
        /// </summary>
        public void LookAtPlayerObject()
        {
            if (!abstractEnemy.EnemyConditions.IsFrozen)
            {
                lerpRotationQuar = Quaternion.LookRotation
                (playerObjectTransformForFollow.position - transform.position);
                lerpRotationQuar.z = 0;
                lerpRotationQuar.x = 0;
            }
        }

        /// <summary>
        /// Установить случайную позицию для врага во время отдыха
        /// </summary>
        public void SetRandomPosition()
        {
            if (agent != null && agent.enabled)
            {
                abstractEnemy.EnemyAnimationsController.DisableAllStates();
                abstractEnemy.EnemyAnimationsController.SetState(0, true);
                abstractEnemy.EnemyAttack.IsMayToPlayAttackAnimation = false;
                abstractEnemy.EnemyAnimationsController.
                    SetSpeedAnimationByRunSpeed(AgentSpeed/3);
                randomPosition.x = LibraryStaticFunctions.
                    GetRandomAxisOfEnemyRest(randomRadius);
                randomPosition.z = LibraryStaticFunctions.GetRandomAxisOfEnemyRest
                    (randomRadius - Math.Abs(randomPosition.x));
                agent.SetDestination(randomPosition);
            }
        }
    }
}
