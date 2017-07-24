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
        #endregion

        /// <summary>
        /// Переопределенный метод для поиска ссылок
        /// </summary>
        public override void Awake()
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
            Physicffect =
                transform.GetChild(1).GetChild(2).GetComponent<IPhysicEffect>();
            ScoreAddingEffect =
                transform.GetChild(1).GetChild(3).GetComponent<IScoreAddingEffect>();
            FireEffect =
                transform.GetChild(1).GetChild(4).GetChild(0).GetComponent<IFireEffect>();
            Timing.RunCoroutine(UpdateAttackState());
        }

        /// <summary>
        /// Инициализация
        /// </summary>
        public override void Start()
        {
            base.Start();
            fightRotatingSpeed = LibraryStaticFunctions.GetRangeValue(fightRotatingSpeed, 0.2f);
            Timing.RunCoroutine(CoroutineForFightRotating());
        }

        /// <summary>
        /// Обновление состояний атаки
        /// </summary>
        /// <returns></returns>
        protected override IEnumerator<float> UpdateAttackState()
        {
            yield return Timing.WaitForSeconds(1);
            while (EnemyConditions.IsAlive)
            {
                if (EnemyMove.IsStopped && EnemyConditions.IsMayGetDamage)
                {
                    if (EnemyMove.PlayerObjectTransformForFollow)
                    {
                        EnemyAttack.EventStartAttackAnimation();

                        if (EnemyAttack.AttackToPlayer())
                        {
                            EnemyOpponentChoiser.PlayerConditionsTarget.GetDamage(EnemyAttack.DmgEnemy);
                            EnemyAnimationsController.SetState(0, false);
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

        /// <summary>
        /// Покадровое обновление
        /// </summary>
        private IEnumerator<float> CoroutineForFightRotating()
        {
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
