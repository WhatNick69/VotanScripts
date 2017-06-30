using AbstractBehaviour;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerBehaviour
{
    /// <summary>
    /// Компонент-атака для персонажа
    /// </summary>
    public class PlayerAttack
        : AbstractAttack 
    {
        private PlayerFight playerFight;

        /// <summary>
        /// Инициализация
        /// </summary>
        void Start()
        {
            listEnemy = new List<AbstractEnemy>();
            attackList = new List<AbstractEnemy>();
            playerFight = GetComponent<PlayerFight>();
        }

        /// <summary>
        /// Обновление с заданной частотой
        /// </summary>
        private void FixedUpdate()
        {
            for (int i = 0; i < listEnemy.Count; i++)
            {
                if (listEnemy[i])
                {
                    if (AttackRange(transform.position, listEnemy[i].transform.position) < 3)
                    {
                        if (!attackList.Contains(listEnemy[i])) attackList.Add(listEnemy[i]);
                    }
                }
            }
			EnemyAttack(playerFight.MyWeapon.Damage, playerFight.MyWeapon.AttackType);
		}
    }
}                                                                      

