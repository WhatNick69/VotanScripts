using AbstractBehaviour;
using System.Collections.Generic;
using VotanLibraries;
using UnityEngine;
using VotanInterfaces;

namespace PlayerBehaviour
{
    /// <summary>
    /// Компонент-атака для персонажа
    /// </summary>
    public class PlayerAttack
        : AbstractAttack, IPlayerAttack
    {
        private PlayerFight playerFight;
		private Transform playerObject;

		/// <summary>
		/// Инициализация
		/// </summary>
		void Start()
        {
			playerObject = LibraryPlayerPosition.PlayerObjectTransform;
			listEnemy = new List<AbstractEnemy>();
            attackList = new List<AbstractEnemy>();
            playerFight = GetComponent<PlayerFight>();
            Debug.Log(playerFight);
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
					if (Vector3.Distance(playerObject.position, listEnemy[i].transform.position) < 3)
					{
						if (!attackList.Contains(listEnemy[i])) attackList.Add(listEnemy[i]);
					}
				}
				else
				{
					listEnemy.Remove(listEnemy[i]);
					
				}
            }
            AttackToEnemy(playerFight.MyWeapon.Damage, playerFight.MyWeapon.AttackType);
		}

        /// <summary>
        /// Атака врага
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="dmgType"></param>
        public void AttackToEnemy(float damage, DamageType dmgType)
        {
            for (int i = 0; i < attackList.Count; i++)
            {
				if (!attackList[i] || Vector3.Distance(playerObject.position, attackList[i].transform.position) > 3 ||
						(attackList[i].ReturnHealth() <= 0))
				{
					attackList.Remove(attackList[i]); continue;
				}
				else
                {
					
					if (Bush(playerStartGunPoint.position,
                        playerFinishGunPoint.position, attackList[i].ReturnPosition(0),
                        attackList[i].ReturnPosition(1)) ||
                        Bush(playerStartGunPoint.position,
                        playerFinishGunPoint.position, attackList[i].ReturnPosition(2)
                        , attackList[i].ReturnPosition(3)))
                    {
                        if (LibraryPlayerPosition.PlayerConditions.IsAlive)
                            attackList[i].GetDamage(damage, dmgType, playerFight.MyWeapon); 
                    }
                }
            }
        }
    }
}                                                                      

