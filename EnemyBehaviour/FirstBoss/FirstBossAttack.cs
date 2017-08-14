using MovementEffects;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace EnemyBehaviour
{
    /// <summary>
    /// Описывает поведение атаки для босса первой локации
    /// </summary>
    public class FirstBossAttack
        : EnemyAttack
    {
        #region Переменные
        [SerializeField, Tooltip("Оружие босса")]
        private Transform swordOfEnemy;
        [SerializeField, Tooltip("Ссылка на босса")]
        private EnemyFirstBoss enemyFirstBoss;
        #endregion

        #region Свойства
        public Transform SwordOfEnemy
        {
            get
            {
                return swordOfEnemy;
            }

            set
            {
                swordOfEnemy = value;
            }
        }
        #endregion

        /// <summary>
        /// Корутина на обычную атаку
        /// </summary>
        /// <param name="isStop"></param>
        /// <returns></returns>
        public override IEnumerator<float> CoroutineForAttack(bool isStop)
        {
            isMayToPlayAttackAnimation = false;
            enemyFirstBoss.IsAttackSeted = true;

            if (isStop)
                enemyAbstract.EnemyMove.DisableAgent();
            enemyAbstract.EnemyAnimationsController.SetState(0, false);
            enemyAbstract.EnemyAnimationsController.SetState(2, false);
            enemyAbstract.EnemyAnimationsController.SetState(3, false);
            enemyAbstract.EnemyAnimationsController.SetState(4, false);
            enemyAbstract.EnemyAnimationsController.SetSpeedAnimationByRunSpeed(0.5f);
            enemyAbstract.EnemyAnimationsController.SetState(1, true);
            yield return Timing.WaitForSeconds(attackLatency);

            enemyFirstBoss.IsAttackSeted = false;
            isMayToPlayAttackAnimation = true;
        }

        /// <summary>
        /// Метод для гольф-атаки, отталкивающей игрока
        /// </summary>
        /// <param name="isStop"></param>
        public void EventStartGolfAnimation(bool isStop)
        {
            if (!isMayToPlayAttackAnimation) return;
            Timing.RunCoroutine(CoroutineForGolfAttack(isStop));
        }

        /// <summary>
        /// Корутина для гольф-атаки
        /// </summary>
        /// <param name="isStop"></param>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForGolfAttack(bool isStop)
        {
            isMayToPlayAttackAnimation = false;
            enemyFirstBoss.IsAttackSeted = true;

            if (isStop)
                enemyAbstract.EnemyMove.DisableAgent();
            enemyAbstract.EnemyAnimationsController.SetState(0, false);
            enemyAbstract.EnemyAnimationsController.SetState(1, false);
            enemyAbstract.EnemyAnimationsController.SetState(3, false);
            enemyAbstract.EnemyAnimationsController.SetState(4, false);
            enemyAbstract.EnemyAnimationsController.SetSpeedAnimationByRunSpeed(0.5f);
            enemyAbstract.EnemyAnimationsController.SetState(2, true);
            yield return Timing.WaitForSeconds(attackLatency);

            enemyFirstBoss.IsAttackSeted = false;
            isMayToPlayAttackAnimation = true;
        }

        /// <summary>
        /// Метод дл самурайской атаки, отталкивающей игрока
        /// </summary>
        /// <param name="isStop"></param>
        public void EventStartSamuraAnimation(bool isStop)
        {
            if (!isMayToPlayAttackAnimation) return;
            Timing.RunCoroutine(CoroutineForSamuraAttack(isStop));
        }

        /// <summary>
        /// Корутина на запуск самурайского удара
        /// </summary>
        /// <param name="isStop"></param>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForSamuraAttack(bool isStop)
        {
            isMayToPlayAttackAnimation = false;
            enemyFirstBoss.IsAttackSeted = true;

            if (isStop)
                enemyAbstract.EnemyMove.DisableAgent();
            enemyAbstract.EnemyAnimationsController.SetState(0, false);
            enemyAbstract.EnemyAnimationsController.SetState(1, false);
            enemyAbstract.EnemyAnimationsController.SetState(2, false);
            enemyAbstract.EnemyAnimationsController.SetState(4, false);
            enemyAbstract.EnemyAnimationsController.SetSpeedAnimationByRunSpeed(0.5f);
            enemyAbstract.EnemyAnimationsController.SetState(3, true);
            yield return Timing.WaitForSeconds(attackLatency);

            enemyFirstBoss.IsAttackSeted = false;
            isMayToPlayAttackAnimation = true;
        }

        /// <summary>
        /// Метод для длинной, нокаутирующей атаки
        /// </summary>
        /// <param name="isStop"></param>
        public void EventStartNockbackAnimation(bool isStop)
        {
            if (!isMayToPlayAttackAnimation) return;
            Timing.RunCoroutine(CoroutineForNockbackAttack(isStop));
        }

        /// <summary>
        /// Корутина для длинной, нокаутирующей атаки
        /// </summary>
        /// <param name="isStop"></param>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForNockbackAttack(bool isStop)
        {
            isMayToPlayAttackAnimation = false;
            enemyFirstBoss.IsAttackSeted = true;

            if (isStop)
                enemyAbstract.EnemyMove.DisableAgent();
            enemyAbstract.EnemyAnimationsController.SetState(0, false);
            enemyAbstract.EnemyAnimationsController.SetState(1, false);
            enemyAbstract.EnemyAnimationsController.SetState(2, false);
            enemyAbstract.EnemyAnimationsController.SetState(3, false);
            enemyAbstract.EnemyAnimationsController.SetSpeedAnimationByRunSpeed(0.5f);
            enemyAbstract.EnemyAnimationsController.SetState(4, true);
            yield return Timing.WaitForSeconds(attackLatency);

            enemyFirstBoss.IsAttackSeted = false;
            isMayToPlayAttackAnimation = true;
        }
    }
}
