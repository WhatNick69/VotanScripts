using AbstractBehaviour;
using System.Collections.Generic;
using UnityEngine;
using VotanInterfaces;
using MovementEffects;
using Playerbehaviour;
using VotanLibraries;

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
                        .position, listEnemy[i].transform.position) <= 3f)
					{
						if (!attackList.Contains(listEnemy[i])) attackList.Add(listEnemy[i]);
					}
				}
				else
				{
					listEnemy.Remove(listEnemy[i]);
					
				}
            }
            if (playerComponentsControl.PlayerFight.IsFighting 
                || playerComponentsControl.PlayerFight.IsRotating)
                    AttackToEnemy(playerComponentsControl.PlayerWeapon.Damage, 
                    playerComponentsControl.PlayerWeapon.AttackType);

			oldFinishGunPoint = playerFinishGunPoint.position;
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
					if (IsAttackEnemy(i))
                    {
						if (playerComponentsControl.PlayerConditions.IsAlive)
						      attackList[i].EnemyConditions.GetDamage(damage, dmgType, playerComponentsControl.PlayerWeapon); 
                    }
                }
            }
        }

		/// <summary>
		/// Проверка на столкновение оружия с противником
		/// вынес в отдельный метод для удобства 
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		private bool IsAttackEnemy(int i)
		{
			return ((LibraryPhysics.BushInPlane(attackList[i].ReturnPosition(4), playerPoint.position,
						playerFinishGunPoint.position, oldFinishGunPoint) ||
                        LibraryPhysics.BushInLine(attackList[i].ReturnPosition(0), attackList[i].ReturnPosition(1),
							PlayerFinishGunPoint.position, PlayerStartGunPoint.position)) && 
							Mathf.Abs(playerPoint.position.y - attackList[i].ReturnPosition(0).y) < 1.6f);
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

