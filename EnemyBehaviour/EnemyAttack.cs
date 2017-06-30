using AbstractBehaviour;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyBehaviour
{
    /// <summary>
    /// Компонент-атака для врага
    /// </summary>
    class EnemyAttack 
        : AbstractAttack
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
        /// Инициализация
        /// </summary>
        void Start()
        {
            listEnemy = new List<AbstractEnemy>();
            attackList = new List<AbstractEnemy>();
        }
    }
}
