using MovementEffects;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace EnemyBehaviour
{
    class FirstBossAttack
        : EnemyAttack
    {
        [SerializeField, Tooltip("Оружие босса")]
        private Transform swordOfEnemy;
        [SerializeField, Tooltip("Ссылка на босса")]
        private EnemyFirstBoss enemyFirstBoss;

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

        public override IEnumerator<float> CoroutineForAttack(bool isStop)
        {
            isMayToPlayAttackAnimation = false;
            enemyFirstBoss.IsAttackSeted = true;

            if (isStop)
                enemyAbstract.EnemyMove.DisableAgent();
            enemyAbstract.EnemyAnimationsController.SetState(0, false);
            enemyAbstract.EnemyAnimationsController.SetSpeedAnimationByRunSpeed(0.5f);
            enemyAbstract.EnemyAnimationsController.SetState(1, true);
            yield return Timing.WaitForSeconds(attackLatency);

            enemyFirstBoss.IsAttackSeted = false;
            isMayToPlayAttackAnimation = true;
        }

        public void EventStartGolfAnimation(bool isStop)
        {
            if (!isMayToPlayAttackAnimation) return;
            Timing.RunCoroutine(CoroutineForGolfAttack(isStop));
        }

        private IEnumerator<float> CoroutineForGolfAttack(bool isStop)
        {
            isMayToPlayAttackAnimation = false;
            enemyFirstBoss.IsAttackSeted = true;

            if (isStop)
                enemyAbstract.EnemyMove.DisableAgent();
            enemyAbstract.EnemyAnimationsController.SetState(0, false);
            enemyAbstract.EnemyAnimationsController.SetSpeedAnimationByRunSpeed(0.5f);
            enemyAbstract.EnemyAnimationsController.SetState(2, true);
            yield return Timing.WaitForSeconds(attackLatency);

            enemyFirstBoss.IsAttackSeted = false;
            isMayToPlayAttackAnimation = true;
        }

        /// <summary>
        /// Анимация самурайского удара со стороны босса
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
            enemyAbstract.EnemyAnimationsController.SetSpeedAnimationByRunSpeed(0.5f);
            enemyAbstract.EnemyAnimationsController.SetState(3, true);
            yield return Timing.WaitForSeconds(attackLatency);

            enemyFirstBoss.IsAttackSeted = false;
            isMayToPlayAttackAnimation = true;
        }

        public void EventStartNockbackAnimation(bool isStop)
        {
            if (!isMayToPlayAttackAnimation) return;
            Timing.RunCoroutine(CoroutineForNockbackAttack(isStop));
        }

        private IEnumerator<float> CoroutineForNockbackAttack(bool isStop)
        {
            isMayToPlayAttackAnimation = false;
            enemyFirstBoss.IsAttackSeted = true;

            if (isStop)
                enemyAbstract.EnemyMove.DisableAgent();
            enemyAbstract.EnemyAnimationsController.SetState(0, false);
            enemyAbstract.EnemyAnimationsController.SetSpeedAnimationByRunSpeed(0.5f);
            enemyAbstract.EnemyAnimationsController.SetState(4, true);
            yield return Timing.WaitForSeconds(attackLatency);

            enemyFirstBoss.IsAttackSeted = false;
            isMayToPlayAttackAnimation = true;
        }
    }
}
