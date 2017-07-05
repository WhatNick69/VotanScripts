using AbstractBehaviour;
using System.Collections.Generic;
using UnityEngine;
using VotanInterfaces;
using MovementEffects;
using Playerbehaviour;

namespace PlayerBehaviour
{
    /// <summary>
    /// Компонент-атака для персонажа
    /// </summary>
    public class PlayerAttack
        : AbstractAttack, IPlayerAttack
    {
        [SerializeField, Tooltip("Хранитель компонентов")]
        private PlayerComponentsControl playerComponentsControl;

		/// <summary>
		/// Обновление с заданной частотой
		/// </summary>
		public void FixedUpdate()
        {
            for (int i = 0; i < listEnemy.Count; i++)
            {
				if (listEnemy[i])
				{
					if (Vector3.Distance(playerComponentsControl.PlayerObject
                        .position, listEnemy[i].transform.position) < 3)
					{
						if (!attackList.Contains(listEnemy[i])) attackList.Add(listEnemy[i]);
					}
				}
				else
				{
					listEnemy.Remove(listEnemy[i]);
					
				}
            }
            AttackToEnemy(playerComponentsControl.PlayerWeapon.Damage, playerComponentsControl.PlayerWeapon.AttackType);
		}

        /// <summary>
        /// Атакуем врага
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="dmgType"></param>
        public void AttackToEnemy(float damage, DamageType dmgType)
        {
            for (int i = 0; i < attackList.Count; i++)
            {
				if (!attackList[i] || Vector3.Distance(playerComponentsControl.PlayerObject
                    .position, attackList[i].transform.position) > 3 ||
						(attackList[i].EnemyConditions.ReturnHealth() <= 0))
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
                        if (playerComponentsControl.PlayerConditions.IsAlive)
                            attackList[i].EnemyConditions.GetDamage(damage, dmgType, playerComponentsControl.PlayerWeapon); 
                    }
                }
            }
        }

        /// <summary>
        /// Получить точку персонажа
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Vector3 GetPlayerPoint(int index) // возвращает точки персонажа игрока
        {
            switch (index)
            {
                case 0:
                    return playerRightPoint.position;
                case 1:
                    return playerLeftPoint.position;
                case 2:
                    return playerFacePoint.position;
                case 3:
                    return playerBackPoint.position;
            }
            return Vector3.zero;
        }

        /// <summary>
        /// Корутина на осуществление урона по врагу
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

