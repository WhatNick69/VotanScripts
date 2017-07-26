using AbstractBehaviour;
using System.Collections.Generic;
using UnityEngine;
using VotanInterfaces;
using MovementEffects;
using VotanLibraries;
using GameBehaviour;

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
        private int isCuttingWeapon;

        public override void Start()
        {
            base.Start();
            WeaponTypeToBool();
        }

        /// <summary>
        /// Обновление с заданной частотой
        /// </summary>
        public void FixedUpdate()
        {
            for (int i = 0; i < StaticStorageWithEnemies.GetCountListOfEnemies(); i++)
            {
				if (StaticStorageWithEnemies.IsNonNullElement(i))
				{
					if (StaticStorageWithEnemies.DistanceBetweenPlayerAndEnemy(playerComponentsControl.PlayerObject
                        .position, i) <= 3f)
					{
						if (!attackList.Contains(StaticStorageWithEnemies.
                            GetFromListByIndex(i))) attackList.Add
                                (StaticStorageWithEnemies.GetFromListByIndex(i));
					}
				}
				else
				{
                    StaticStorageWithEnemies.RemoveFromListByIndex(i);			
				}
            }

            if (!playerComponentsControl.PlayerFight.IsDefensing)
            {
                if (playerComponentsControl.PlayerFight.IsRotating)
                {
                    AttackToEnemy(LibraryStaticFunctions.AttackToEnemyDamage
                        (playerComponentsControl.PlayerWeapon.Damage,
                        playerComponentsControl.PlayerWeapon.SpinSpeed,
                        playerComponentsControl.PlayerWeapon.OriginalSpinSpeed),
                    playerComponentsControl.PlayerWeapon.AttackType);
                }
                else if (playerComponentsControl.PlayerFight.IsFighting)
                {
                    AttackToEnemy(LibraryStaticFunctions.AttackToEnemyDamage
                       (playerComponentsControl.PlayerWeapon.Damage,
                       playerComponentsControl.PlayerWeapon.SpinSpeed,
                       playerComponentsControl.PlayerWeapon.OriginalSpinSpeed,true),
                   playerComponentsControl.PlayerWeapon.AttackType);
                }
            }
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
					attackList.Remove(attackList[i]);
                    continue;
				}
				else if (IsAttackEnemy(i))
                {
                    if (playerComponentsControl.PlayerConditions.IsAlive)
                    {
                        if (attackList[i].EnemyConditions.GetDamage
                            (damage, playerComponentsControl.PlayerWeapon.GemPower
                            , playerComponentsControl.PlayerWeapon))
                        {
                            playerComponentsControl.PlayerSounder.
                                PlayWeaponHitAudio(isCuttingWeapon);
                        }
                    }
                }
            }
        }

        private void WeaponTypeToBool()
        {
            if (playerComponentsControl.PlayerWeapon.WeaponType == WeaponType.Cutting)
            {
                isCuttingWeapon = 1;
            }
            else
            {
                isCuttingWeapon = 0;
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
			return ((LibraryPhysics.BushInPlane(attackList[i].
                ReturnPosition(4), playerPoint.position,
						playerFinishGunPoint.position, oldFinishGunPoint) ||
                        LibraryPhysics.BushInLine(attackList[i].ReturnPosition(0), 
                        attackList[i].ReturnPosition(1),
							PlayerFinishGunPoint.position, PlayerStartGunPoint.position)) && 
							Mathf.Abs(playerPoint.position.y 
                            - attackList[i].ReturnPosition(0).y) < 1.6f);
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

