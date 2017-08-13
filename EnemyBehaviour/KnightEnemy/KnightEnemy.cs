using AbstractBehaviour;
using UnityEngine;
using UnityEngine.AI;
using VotanInterfaces;
using MovementEffects;
using System.Collections.Generic;
using GameBehaviour;
using VotanLibraries;

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
        protected float refreshLatency;
        protected float movingSpeed;

        /// <summary>
        /// Инициализация
        /// </summary>
        public virtual void Awake()
        {
            AbstractObjectSounder = 
                GetComponent<AbstractSoundStorage>();
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
        /// Рестарт врага
        /// </summary>
        public override void RestartEnemy()
        {
            enemyConditions.RestartEnemyConditions(); // рестарт состояний врага
            enemyAttack.RestartEnemyAttack(); // рестарт способности врага к битве
            enemyMove.RestartEnemyMove(); // рестарт способносит врага к движению
            downInterfaceRotater.RestartDownInterfaceRotater(); // рестарт поворота интерфейса
            enemyAnimationsController.RestartEnemyAnimationsController(); // рестарт аниматора
            FireEffect.RestartFire();

            movingSpeed = EnemyMove.AgentSpeed / 5;

            Timing.RunCoroutine(UpdateAttackState());
        }

        /// <summary>
        /// Обновление
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator<float> UpdateAttackState()
        {
            yield return Timing.WaitForSeconds(1);
            while (EnemyConditions.IsAlive)
            {
                if (EnemyMove.IsStopped)
                {
                    if (EnemyMove.PlayerObjectTransformForFollow)
                    {
                        EnemyAttack.EventStartAttackAnimation(true);

                        if (EnemyAttack.AttackToPlayer())
                        {
                            AbstractObjectSounder.PlayWeaponHitAudio
                                (EnemyOpponentChoiser.PlayerConditionsTarget.
                                GetDamage(EnemyAttack.DmgEnemy));
                            EnemyAnimationsController.SetState(0, false);
                            EnemyAnimationsController.SetSpeedAnimationByRunSpeed(0.5f);
                        }
                    }
                    else
                    {
                        EnemyAnimationsController.LowSpeedAnimation();
                        EnemyAnimationsController.SetState(0, false);
                    }
                }
                else
                {
                    if (!enemyAttack.IsMayToDamage)
                    {
                        EnemyAnimationsController.SetState(0, true);
                        EnemyAnimationsController.SetState(1, false);

                        if (!EnemyConditions.IsFrozen)
                            EnemyMove.DependenceAnimatorSpeedOfVelocity();
                    }
                }
                yield return Timing.WaitForSeconds(refreshLatency);
            }
        }
    }
}
