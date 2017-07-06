using AbstractBehaviour;
using MovementEffects;
using PlayerBehaviour;
using System.Collections.Generic;
using UnityEngine;
using VotanInterfaces;
using VotanLibraries;

namespace EnemyBehaviour
{
    /// <summary>
    /// Компонент-атака для врага
    /// </summary>
    public class EnemyAttack 
        : AbstractAttack, IEnemyAttack
    {
        [SerializeField, Tooltip("Урон от удара врага")]
        private float dmgEnemy;
        private PlayerAttack playerTarget;

        public float DmgEnemy
        {
            get
            {
                return dmgEnemy;
            }

            set
            {
                dmgEnemy = value;
            }
        }

        public PlayerAttack PlayerTarget
        {
            get
            {
                return playerTarget;
            }

            set
            {
                playerTarget = value;
            }
        }

        /// <summary>
        /// Атакуем персонажа
        /// </summary>
        /// <returns></returns>
        public bool AttackToPlayer()
        {
            if (isMayToDamage && (BushInPlane(enemyStartGunPoint.position, enemyFinishGunPoint.position,
                 playerTarget.GetPlayerPoint(0), 
                 playerTarget.GetPlayerPoint(1)) ||
                 BushInPlane(enemyStartGunPoint.position, enemyFinishGunPoint.position,
                 playerTarget.GetPlayerPoint(2), 
                 playerTarget.GetPlayerPoint(3))))
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
        /// Корутина для нанесения урона по персонажу
        /// </summary>
        /// <returns></returns>
        public IEnumerator<float> CoroutineMayDoDamage()
        {
            isMayToDamage = false;
            yield return Timing.WaitForSeconds(attackLatency);
            isMayToDamage = true;
        }
    }
}
