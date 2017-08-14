using System.Collections.Generic;
using MovementEffects;
using UnityEngine;
using VotanLibraries;

namespace EnemyBehaviour
{
    /// <summary>
    /// Компонент-атака для врага, который
    /// может бить с двух рук
    /// </summary>
    public class EnemyTwoHandleAttack
        : EnemyAttack
    {
        [SerializeField] // точки оружия врага
        protected Transform enemyStartGunPointSecond, 
            enemyFinishGunPointSecond;

        /// <summary>
        /// Двойная атака
        /// </summary>
        /// <returns></returns>
        public override bool AttackToPlayer()
        {
			if (IsMayToDamage && (LibraryPhysics.IsAttackEnemy(startGunPoint.position,
				finishGunPoint.position, playerTarget.GetPlayerPoint()) || 
				LibraryPhysics.IsAttackEnemy(enemyStartGunPointSecond.position, enemyFinishGunPointSecond.position,
				playerTarget.GetPlayerPoint())))
            {
                Timing.RunCoroutine(CoroutineMayDoDamage());
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Выключаем и включаем просчет
        /// </summary>
        /// <returns></returns>
        public override IEnumerator<float> CoroutineMayDoDamage()
        {
            IsMayToDamage = false;
            yield return Timing.WaitForSeconds(attackLatency);
            IsMayToDamage = true;
        }
    }
}
