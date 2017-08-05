using GameBehaviour;
using MovementEffects;
using System.Collections.Generic;
using UnityEngine;
using VotanInterfaces;
using VotanLibraries;

namespace EnemyBehaviour
{
    /// <summary>
    /// Класс противника "Бешеный"
    /// </summary>
    public class CrazyEnemy
        : KnightEnemy
    {
        #region Переменные
        [SerializeField, Tooltip("Модель бешеного врага")]
        private Transform crazyEnemyModel;
        [SerializeField, Tooltip("Скорость вращения")]
        private float fightRotatingSpeed;
        [SerializeField, Tooltip("Частота вращения"),Range(0.01f,0.1f)]
        private float frequencyOfFightRotating;
        private float angle;
        private Quaternion localQuar = new Quaternion(0, 0, 0,0);
        private float tempAngleForSound;
        #endregion

        #region Свойства
        public float FightRotatingSpeed
        {
            get
            {
                return fightRotatingSpeed;
            }

            set
            {
                fightRotatingSpeed = value;
            }
        }
        #endregion

        /// <summary>
        /// Переопределенный метод для поиска ссылок
        /// </summary>
        public override void Awake()
        {
            AbstractObjectSounder =
                GetComponent<CrazySounder>();
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
                LibraryStaticFunctions.DeepFind(transform,"ElectricStack")
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

            frequencyOfFightRotating = 0.025f;
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Обновить врага после того, как тот был возвращен и взят из стэка
        /// </summary>
        public override void RestartEnemy()
        {
            enemyConditions.RestartEnemyConditions(); // рестарт состояний врага
            enemyAttack.RestartEnemyAttack(); // рестарт способности врага к битве
            enemyMove.RestartEnemyMove(); // рестарт способносит врага к движению
            downInterfaceRotater.RestartDownInterfaceRotater(); // рестарт поворота интерфейса
            enemyAnimationsController.RestartEnemyAnimationsController(); // рестарт аниматора
            movingSpeed = EnemyMove.AgentSpeed / 5;
            fightRotatingSpeed = LibraryStaticFunctions.GetRangeValue(fightRotatingSpeed, 0.2f);

            Timing.RunCoroutine(CoroutineForFightRotating());
            Timing.RunCoroutine(UpdateAttackState());
        }

        /// <summary>
        /// Обновление состояний атаки
        /// </summary>
        /// <returns></returns>
        public override IEnumerator<float> UpdateAttackState()
        {
            yield return Timing.WaitForSeconds(1);
            while (EnemyConditions.IsAlive)
            {
                if (EnemyMove.IsStopped && EnemyConditions.IsMayGetDamage)
                {
                    if (EnemyMove.PlayerObjectTransformForFollow)
                    {
                        EnemyAttack.EventStartAttackAnimation(false);

                        if (EnemyAttack.AttackToPlayer())
                        {
                            AbstractObjectSounder.PlayWeaponHitAudio
                                (EnemyOpponentChoiser.PlayerConditionsTarget.
                                GetDamage(EnemyAttack.DmgEnemy));
                            EnemyAnimationsController.SetState(0, false);
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
                    if (!enemyAttack.IsMayToDamage || 
                        !EnemyMove.PlayerObjectTransformForFollow)
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

        /// <summary>
        /// Проиграть звук кручения оружия
        /// </summary>
        private void PlaySpinSpeedAudio()
        {
            tempAngleForSound += fightRotatingSpeed;
            if (Mathf.Abs(tempAngleForSound) >= 14000)
            {
                AbstractObjectSounder.PlaySpinAudio(fightRotatingSpeed);
                tempAngleForSound = 0;
            }
        }

        /// <summary>
        /// Покадровое обновление
        /// </summary>
        private IEnumerator<float> CoroutineForFightRotating()
        {
            yield return Timing.WaitForSeconds(1);
            while (EnemyConditions.IsAlive)
            {
                if (EnemyConditions.IsMayGetDamage &&
                    !EnemyConditions.IsFrozen &&
                    EnemyMove.IsStopped &&
                    EnemyMove.PlayerObjectTransformForFollow)
                {
                    if (EnemyConditions.IsBurned)
                        crazyEnemyModel.Rotate(Vector3.up * Time.deltaTime * fightRotatingSpeed / 2);
                    else
                        crazyEnemyModel.Rotate(Vector3.up * Time.deltaTime * fightRotatingSpeed);

                    PlaySpinSpeedAudio();
                    EnemyAnimationsController.SetState(1, true);
                    yield return Timing.WaitForSeconds(frequencyOfFightRotating);
                }
                else
                {
                    EnemyAnimationsController.SetState(1, false);
                    yield return Timing.WaitForSeconds(frequencyOfFightRotating*5);
                }
            }
        }
    }
}
