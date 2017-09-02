using System;
using AbstractBehaviour;
using UnityEngine;
using UnityEngine.AI;
using GameBehaviour;
using VotanInterfaces;
using VotanLibraries;
using System.Collections.Generic;
using MovementEffects;

namespace EnemyBehaviour
{
    /// <summary>
    /// Класс противника "Арбалетчик"
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class CrossbowmanEnemy
        : AbstractEnemy
    {
        #region Переменные
        [SerializeField, Tooltip("Частота обновления состояний для атаки"), Range(0.01f, 0.5f)]
        protected float refreshLatency;
        protected float movingSpeed;
        protected CrossbowmanAttack crossbowmanAttack;
        #endregion

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
            crossbowmanAttack =
                GetComponent<CrossbowmanAttack>();
            EnemyAttack = crossbowmanAttack;
            EnemyConditions =
                GetComponent<EnemyConditions>();
            EnemyMove =
                GetComponent<IAIMoving>();
            DownInterfaceRotater =
                GetComponent<DownInterfaceRotater>();

            IceEffect =
                LibraryObjectsWorker.DeepFind(transform, "IceStack")
                .GetComponent<IIceEffect>();
            ElectricEffect =
                LibraryObjectsWorker.DeepFind(transform, "ElectricStack")
                .GetComponent<IElectricEffect>();
            Physicffect =
                LibraryObjectsWorker.DeepFind(transform, "PhysicStack")
                .GetComponent<IPhysicEffect>();
            ScoreAddingEffect =
                LibraryObjectsWorker.DeepFind(transform, "ScoreStack")
                .GetComponent<IScoreAddingEffect>();
            FireEffect =
                LibraryObjectsWorker.DeepFind(transform, "FireStack")
                .GetComponent<IFireEffect>();

            gameObject.SetActive(false);
        }

        /// <summary>
        /// Рестарт врага
        /// </summary>
        public override void RestartEnemy()
        {
            enemyConditions.RestartEnemyConditions(); // рестарт состояний врага
            crossbowmanAttack.RestartEnemyAttack(); // рестарт способности врага к битве
            crossbowmanAttack.CrossbowWeapon.DmgValue = crossbowmanAttack.DmgEnemy;
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
            while (EnemyConditions.IsAlive)
            {
                yield return Timing.WaitForSeconds(refreshLatency);
                if (EnemyMove.IsStopped)
                {
                    if (EnemyMove.PlayerObjectTransformForFollow)
                    {
                        if (crossbowmanAttack.IsReloaded
                            && crossbowmanAttack.IsTrueAngle
                            && crossbowmanAttack.isMayToPlayAttackAnimation)
                        {
                            crossbowmanAttack.EventStartAttackAnimation(true);
                        }
                        else
                        {
                            EnemyAnimationsController.SetState(0, false);
                            EnemyAnimationsController.SetSpeedAnimationByRunSpeed(0.5f);

                            if (crossbowmanAttack.CrossbowWeapon.IsNotEmptyAmmunation()
                                && !crossbowmanAttack.IsReloaded)
                                EnemyAnimationsController.SetState(3, true);
                        }
                    }
                    else
                    {
                        EnemyAnimationsController.SetState(0, false);
                    }
                }
                else
                {
                    if (!crossbowmanAttack.IsMayToDamage)
                    {
                        EnemyAnimationsController.SetState(0, true);
                        EnemyAnimationsController.SetState(1, false);
                    }
                    if (!EnemyConditions.IsFrozen)
                    {
                        EnemyMove.DependenceAnimatorSpeedOfVelocity();
                    }
                }
            }
        }
    }
}
