using AbstractBehaviour;
using GameBehaviour;
using MovementEffects;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using VotanGameplay;
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
        IFirstBossMove iFirstBossMove;
        FirstBossAttack firstBossAttack;
        private float distanceForFirstAttack;
        private float distanceForKneeAttack;
        private float distanceForSamuraAttack;
        private float distanceForGolfAttack;
        private float tempDistance;

        private int currentAttackState;

        public IFirstBossMove IFirstBossMove
        {
            get
            {
                return iFirstBossMove;
            }

            set
            {
                iFirstBossMove = value;
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
            IFirstBossMove =
                GetComponent<IFirstBossMove>();
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
            distanceForKneeAttack = distanceForFirstAttack / 2;
            distanceForGolfAttack = distanceForFirstAttack / 3;

            distanceForSamuraAttack = distanceForFirstAttack / 1.25f;

            movingSpeed = EnemyMove.AgentSpeed / 5;

            FireEffect.RestartFire();

            Timing.RunCoroutine(UpdateAttackState());
            DynamicGameobjectsManager.FireEventDynamicObject(0);
        }

        /// <summary>
        /// Обновлять дистанцию между боссом и игроком
        /// </summary>
        /// <returns></returns>
        private void UpdateDistanceBetweenThisAndPlayer()
        {
            if (EnemyMove.PlayerObjectTransformForFollow 
            && enemyAttack.IsMayToPlayAttackAnimation)
            {
                EnemyAnimationsController.DisableAllStates();
                tempDistance = Vector3.Distance
                    (transform.position, EnemyMove.PlayerObjectTransformForFollow.position);
                if (tempDistance <= distanceForGolfAttack)
                {
                    currentAttackState = 0;
                }
                else if (tempDistance <= distanceForKneeAttack)
                {
                    currentAttackState = 1;
                }
                else if (tempDistance <= distanceForSamuraAttack)
                {
                    currentAttackState = 2;
                }
                else if (tempDistance <= distanceForFirstAttack)
                {
                    currentAttackState = 3;
                }
            }
        }

        /// <summary>
        /// Обычная атака сверху вниз.
        /// </summary>
        private void FirstAttackMethod()
        {
            firstBossAttack.EventStartAttackAnimation(true);
            if (firstBossAttack.AttackToPlayer(true))
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
        private void KneeAttackMethod()
        {
            firstBossAttack.EventStartNockbackAnimation(true);
            if (firstBossAttack.AttackToPlayer(false))
            {
                AbstractObjectSounder.PlayWeaponHitAudio
                    (EnemyOpponentChoiser.PlayerConditionsTarget.
                    GetDamage(EnemyAttack.DmgEnemy));

                EnemyMove.PlayerCollisionComponent.AddDamageForceToPlayer
                    (firstBossAttack.StartKneePoint.forward,true);
                EnemyAnimationsController.DisableAllStates();
                EnemyAnimationsController.SetSpeedAnimationByRunSpeed(0.5f);
            }
            else
            {
                EnemyAnimationsController.SetSpeedAnimationByRunSpeed(0.1f);
            }
        }

        /// <summary>
        /// Крученый удар.
        /// </summary>
        private void SamuraAttackMethod()
        {
            firstBossAttack.EventStartSamuraAnimation(true);
            if (firstBossAttack.AttackToPlayer(true))
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
        /// Удар, откидывающий персонажа.
        /// </summary>
        private void GolfAttackMethod()
        {
            firstBossAttack.EventStartGolfAnimation(true);
            if (firstBossAttack.AttackToPlayer(true))
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
                                KneeAttackMethod();
                                break;
                            case 2:
                                SamuraAttackMethod();
                                break;
                            case 3:
                                FirstAttackMethod();
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
                        EnemyAnimationsController.SetState(3, false);
                        EnemyAnimationsController.SetState(4, false);

                        if (!EnemyConditions.IsFrozen)
                            EnemyMove.DependenceAnimatorSpeedOfVelocity();
                    }
                }
                yield return Timing.WaitForSeconds(refreshLatency);
            }
        }
    }
}
