using AbstractBehaviour;
using GameBehaviour;
using MovementEffects;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using VotanInterfaces;
using VotanLibraries;

namespace EnemyBehaviour
{
    /// <summary>
    /// Описывает поведение босса с первой локации
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyFirstBoss
        : KnightEnemy
    {
        #region Переменные
        [SerializeField, Tooltip("Время обновления дистанции между врагом и игроком")
            ,Range(0.05f,0.5f)]
        private float timeForAttackStatesUpdate;
        FirstBossAttack firstBossAttack;
        private float distanceForFirstAttack;
        private float distanceForLongAttack;
        private float distanceForSamuraAttack;
        private float distanceForGolfAttack;
        private float tempDistance;
        private int currentAttackState;

        private bool isAttackSeted;
        #endregion

        #region Свойства
        public bool IsAttackSeted
        {
            get
            {
                return isAttackSeted;
            }

            set
            {
                isAttackSeted = value;
            }
        }
        #endregion

        /// <summary>
        /// Инициализация
        /// </summary>
        public override void Awake()
        {
            AbstractObjectSounder =
                GetComponent<AbstractSoundStorage>();
            EnemyOpponentChoiser =
                GetComponent<EnemyOpponentChoiser>();
            EnemyAnimationsController =
                GetComponent<EnemyAnimationsController>();
            firstBossAttack =
                GetComponent<FirstBossAttack>();
            EnemyAttack = firstBossAttack;
            EnemyConditions =
                GetComponent<EnemyConditions>();
            EnemyMove =
                GetComponent<EnemyMove>();
            DownInterfaceRotater =
                GetComponent<DownInterfaceRotater>();

            IceEffect =
                LibraryStaticFunctions.DeepFind(transform, "IceStack")
                .GetComponent<IIceEffect>();
            ElectricEffect =
                LibraryStaticFunctions.DeepFind(transform, "ElectricStack")
                .GetComponent<IElectricEffect>();
            Physicffect =
                LibraryStaticFunctions.DeepFind(transform, "PhysicStack")
                .GetComponent<IPhysicEffect>();
            ScoreAddingEffect =
                LibraryStaticFunctions.DeepFind(transform, "ScoreStack")
                .GetComponent<IScoreAddingEffect>();
            FireEffect =
                LibraryStaticFunctions.DeepFind(transform, "FireStack")
                .GetComponent<IFireEffect>();

            gameObject.SetActive(false);
        }

        /// <summary>
        /// Перезагрузить босса
        /// </summary>
        public override void RestartEnemy()
        {
            enemyConditions.RestartEnemyConditions(); // рестарт состояний врага
            firstBossAttack.RestartEnemyAttack(); // рестарт способности врага к битве
            enemyMove.RestartEnemyMove(); // рестарт способносит врага к движению
            downInterfaceRotater.RestartDownInterfaceRotater(); // рестарт поворота интерфейса
            enemyAnimationsController.RestartEnemyAnimationsController(); // рестарт аниматора

            distanceForFirstAttack = enemyMove.Agent.stoppingDistance;
            distanceForGolfAttack = distanceForFirstAttack / 3;
            movingSpeed = EnemyMove.AgentSpeed / 5;

            FireEffect.RestartFire();

            Timing.RunCoroutine(UpdateAttackState());
            //Timing.RunCoroutine(UpdateDistanceBetweenThisAndPlayer());
        }

        /// <summary>
        /// Обновлять дистанцию между боссом и игроком
        /// </summary>
        /// <returns></returns>
        private void UpdateDistanceBetweenThisAndPlayer()
        {
            //while (EnemyConditions.IsAlive)
            //{
                if (EnemyMove.PlayerObjectTransformForFollow
                    && !isAttackSeted)
                {
                    tempDistance = Vector3.Distance
                        (transform.position, EnemyMove.PlayerObjectTransformForFollow.position);
                    if (tempDistance <= distanceForGolfAttack)
                    {
                        Debug.Log("state 0");
                    isAttackSeted = true;
                    currentAttackState = 0;
                    }
                    else if (tempDistance <= distanceForSamuraAttack)
                    {
                        Debug.Log("state 1");
                    isAttackSeted = true;
                    currentAttackState = 1;
                    }
                    else if (tempDistance <= distanceForFirstAttack)
                    {
                        Debug.Log("state 2");
                    isAttackSeted = true;
                    currentAttackState = 2;
                    }
                    else if (tempDistance <= distanceForLongAttack)
                    {
                        Debug.Log("state 3");
                    isAttackSeted = true;
                        currentAttackState = 3;
                    }
                }
               // yield return Timing.WaitForSeconds(timeForAttackStatesUpdate);
            //}
        }

        /// <summary>
        /// Обычная атака сверху вниз.
        /// </summary>
        private void FirstAttackMethod()
        {
            firstBossAttack.EventStartAttackAnimation(true);
            if (EnemyAttack.AttackToPlayer())
            {
                AbstractObjectSounder.PlayWeaponHitAudio
                    (EnemyOpponentChoiser.PlayerConditionsTarget.
                    GetDamage(EnemyAttack.DmgEnemy));
                EnemyAnimationsController.DisableAllStates();
                EnemyAnimationsController.SetSpeedAnimationByRunSpeed(0.5f);
            }
            else
            {
                EnemyAnimationsController.SetSpeedAnimationByRunSpeed(0.1f);
            }
        }

        /// <summary>
        /// Длинная атака. 
        /// Со спадом на колено.
        /// </summary>
        private void NockbackAttackMethod()
        {
            firstBossAttack.EventStartNockbackAnimation(true);
        }

        /// <summary>
        /// Крученый удар.
        /// </summary>
        private void SamuraAttackMethod()
        {
            firstBossAttack.EventStartSamuraAnimation(true);
        }

        /// <summary>
        /// Удар, откидывающий персонажа.
        /// </summary>
        private void GolfAttackMethod()
        {
            firstBossAttack.EventStartGolfAnimation(true);
            if (firstBossAttack.AttackToPlayer())
            {
                AbstractObjectSounder.PlayWeaponHitAudio
                    (EnemyOpponentChoiser.PlayerConditionsTarget.
                    GetDamage(EnemyAttack.DmgEnemy));

                EnemyMove.PlayerCollisionComponent.AddDamageForceToPlayer
                    (firstBossAttack.SwordOfEnemy.forward);
                EnemyAnimationsController.DisableAllStates();
                EnemyAnimationsController.SetSpeedAnimationByRunSpeed(0.5f);
            }
            else
            {
                EnemyAnimationsController.SetSpeedAnimationByRunSpeed(0.1f);
            }
        }

        /// <summary>
        /// Обновлять тип атаки босса
        /// </summary>
        /// <returns></returns>
        public override IEnumerator<float> UpdateAttackState()
        {
            yield return Timing.WaitForSeconds(1);
            while (EnemyConditions.IsAlive)
            {
                if (EnemyMove.IsStopped)
                {
                    if (EnemyMove.PlayerObjectTransformForFollow)
                    {
                        UpdateDistanceBetweenThisAndPlayer();
                        switch (currentAttackState)
                        {
                            case 0:
                                GolfAttackMethod();
                                break;
                            case 1:
                                SamuraAttackMethod();
                                break;
                            case 2:
                                FirstAttackMethod();
                                break;
                            case 3:
                                NockbackAttackMethod();
                                break;
                        }
                    }
                    else
                    {
                        EnemyAnimationsController.DisableAllStates();
                        EnemyAnimationsController.SetSpeedAnimationByRunSpeed(0.1f);
                    }
                }
                else
                {
                    if (!firstBossAttack.IsMayToDamage)
                    {
                        EnemyAnimationsController.SetState(0, true);
                        EnemyAnimationsController.SetState(1, false);
                        EnemyAnimationsController.SetState(2, false);

                        if (!EnemyConditions.IsFrozen)
                            EnemyMove.DependenceAnimatorSpeedOfVelocity();
                    }
                }
                yield return Timing.WaitForSeconds(refreshLatency);
            }
        }
    }
}
