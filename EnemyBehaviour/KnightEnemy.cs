using AbstractBehaviour;
using UnityEngine;
using UnityEngine.AI;
using VotanInterfaces;
using MovementEffects;
using System.Collections.Generic;
using System;

namespace EnemyBehaviour
{
    /// <summary>
    /// Класс противника "Рыцарь"
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class KnightEnemy
        : AbstractEnemy
    {
        [SerializeField,Tooltip("Частота обновления состояний для атаки"),Range(0.01f,0.5f)]
        private float refreshLatency;
        private float movingSpeed;

        /// <summary>
        /// Инициализация
        /// </summary>
        public void Awake()
        {
            EnemyOpponentChoiser =
                GetComponent<EnemyOpponentChoiser>();
            EnemyAnimationsController = 
                GetComponent<EnemyAnimationsController>();
            EnemyAttack = 
                GetComponent<EnemyAttack>();
            EnemyConditions = 
                GetComponent<EnemyConditions>();
            EnemyMove = 
                GetComponent<EnemyMove>();
            IceEffect =
                transform.GetChild(1).GetChild(0).GetComponent<IIceEffect>();
            ElectricEffect =
                transform.GetChild(1).GetChild(1).GetComponent<IElectricEffect>();
            FireEffect =
                transform.GetChild(1).GetChild(2).GetComponent<IFireEffect>();
            Physicffect =
                transform.GetChild(1).GetChild(3).GetComponent<IPhysicEffect>();
            ScoreAddingEffect =
                transform.GetChild(1).GetChild(4).GetComponent<IScoreAddingEffect>();
            Timing.RunCoroutine(UpdateAttackState());
        }

        public void Start()
        {
            movingSpeed = EnemyMove.AgentSpeed / 5;
        }

        /// <summary>
        /// Обновление
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> UpdateAttackState()
        {
            yield return Timing.WaitForSeconds(1);
            while (EnemyConditions.IsAlive)
            {
                if (EnemyMove.IsStopped)
                {
                    if (EnemyMove.PlayerObjectTransformForFollow)
                    {
                        EnemyAttack.EventStartAttackAnimation();

                        if (EnemyAttack.AttackToPlayer())
                        {
                            EnemyOpponentChoiser.PlayerConditionsTarget.GetDamage(EnemyAttack.DmgEnemy);
                            EnemyAnimationsController.SetState(0, false);
                            //EnemyAnimationsController.SetState(1, true);
                            EnemyAnimationsController.SetSpeedAnimationByRunSpeed(0.5f);
                        }
                    }
                    else
                    {
                        EnemyAnimationsController.SetState(0, false);
                    }
                }
                else
                {
                    EnemyAnimationsController.SetState(0, true);
                    EnemyAnimationsController.SetState(1, false);
                    if (!EnemyConditions.IsFrozen)
                        EnemyMove.DependenceAnimatorSpeedOfVelocity();
                }
                yield return Timing.WaitForSeconds(refreshLatency);
            }
        }
    }
}
