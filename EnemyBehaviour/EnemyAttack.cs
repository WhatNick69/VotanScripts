using AbstractBehaviour;
using System.Collections.Generic;
using UnityEngine;
using System;
using VotanInterfaces;
using VotanLibraries;
using MovementEffects;

namespace EnemyBehaviour
{
    /// <summary>
    /// Компонент-атака для врага
    /// </summary>
    class EnemyAttack 
        : AbstractAttack, IEnemyAttack
    {
        [SerializeField, Tooltip("Урон от удара врага")]
        private float dmgEnemy;

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

        /// <summary>
        /// Атакуем персонажа
        /// </summary>
        /// <returns></returns>
        public bool AttackToPlayer()
        {
            if (isMayToDamage && (Bush(enemyStartGunPoint.position, enemyFinishGunPoint.position,
                 LibraryPlayerPosition.GetPlayerPoint(0), LibraryPlayerPosition.GetPlayerPoint(1)) ||
                 Bush(enemyStartGunPoint.position, enemyFinishGunPoint.position,
                 LibraryPlayerPosition.GetPlayerPoint(2), LibraryPlayerPosition.GetPlayerPoint(3))))
            {
                Timing.RunCoroutine(CoroutineMayDoDamageForPlayer());
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Инициализация
        /// </summary>
        void Start()
        {
            listEnemy = new List<AbstractEnemy>();
            attackList = new List<AbstractEnemy>();
        }
    }
}
