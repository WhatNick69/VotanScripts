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
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyFirstBoss
        : KnightEnemy
    {
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
            Timing.RunCoroutine(UpdateDistanceBetweenThisAndPlayer());
        }

        private IEnumerator<float> UpdateDistanceBetweenThisAndPlayer()
        {
            while (EnemyConditions.IsAlive)
            {
                if (EnemyMove.PlayerObjectTransformForFollow)
                {
                    tempDistance = Vector3.Distance
                        (transform.position, EnemyMove.PlayerObjectTransformForFollow.position);
                    if (tempDistance <= distanceForGolfAttack)
                    {
                        Debug.Log("state 0");
                        currentAttackState = 0;
                    }
                    else if (tempDistance <= distanceForSamuraAttack)
                    {
                        Debug.Log("state 1");
                        currentAttackState = 1;
                    }
                    else if (tempDistance <= distanceForFirstAttack)
                    {
                        Debug.Log("state 2");
                        currentAttackState = 2;
                    }
                    else if (tempDistance <= distanceForLongAttack)
                    {
                        Debug.Log("state 3");
                        currentAttackState = 3;
                    }
                }
                yield return Timing.WaitForSeconds(timeForAttackStatesUpdate);
            }
        }

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

        private void LongAttackMethod()
        {
          
        }

        private void SamuraAttackMethod()
        {
           
        }

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

        public override IEnumerator<float> UpdateAttackState()
        {
            yield return Timing.WaitForSeconds(1);
            while (EnemyConditions.IsAlive)
            {
                if (EnemyMove.IsStopped)
                {
                    if (EnemyMove.PlayerObjectTransformForFollow)
                    {
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
                                LongAttackMethod();
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
