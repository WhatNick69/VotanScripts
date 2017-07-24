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
            if (isMayToDamage && (LibraryPhysics.BushInLine
               (enemyStartGunPoint.position, enemyFinishGunPoint.position,
                playerTarget.GetPlayerPoint(0),
                playerTarget.GetPlayerPoint(1)) ||
                LibraryPhysics.BushInLine
                   (enemyStartGunPoint.position, enemyFinishGunPoint.position,
                playerTarget.GetPlayerPoint(2),
                playerTarget.GetPlayerPoint(3))) ||
                (LibraryPhysics.BushInLine
               (enemyStartGunPointSecond.position, enemyFinishGunPointSecond.position,
                playerTarget.GetPlayerPoint(0),
                playerTarget.GetPlayerPoint(1)) ||
                LibraryPhysics.BushInLine
                   (enemyStartGunPointSecond.position, enemyFinishGunPointSecond.position,
                playerTarget.GetPlayerPoint(2),
                playerTarget.GetPlayerPoint(3)))
                )
            {
                Timing.RunCoroutine(CoroutineMayDoDamage());
                return true;
            }
            else
            {
                return false;
            }
        }

        public override IEnumerator<float> CoroutineMayDoDamage()
        {
            isMayToDamage = false;
            yield return Timing.WaitForSeconds(attackLatency + 0.5f);
        }
    }
}
