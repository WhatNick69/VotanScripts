using MovementEffects;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyBehaviour
{
    class FirstBossAttack
        : EnemyAttack
    {
        [SerializeField, Tooltip("Оружие босса")]
        private Transform swordOfEnemy;

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

        public void EventStartGolfAnimation(bool isStop)
        {
            if (!isMayToPlayAttackAnimation) return;
            Timing.RunCoroutine(CoroutineForGolfAttack(isStop));
        }

        private IEnumerator<float> CoroutineForGolfAttack(bool isStop)
        {
            isMayToPlayAttackAnimation = false;
            if (isStop)
                enemyAbstract.EnemyMove.DisableAgent();
            enemyAbstract.EnemyAnimationsController.SetState(0, false);
            enemyAbstract.EnemyAnimationsController.SetSpeedAnimationByRunSpeed(0.5f);
            enemyAbstract.EnemyAnimationsController.SetState(2, true);
            yield return Timing.WaitForSeconds(attackLatency);
            isMayToPlayAttackAnimation = true;
        }
    }
}
