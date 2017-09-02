using AbstractBehaviour;
using System.Collections.Generic;
using UnityEngine;
using VotanInterfaces;
using MovementEffects;
using VotanLibraries;
using VotanGameplay;
using System;

namespace PlayerBehaviour
{
	/// <summary>
	/// Компонент-атака для персонажа
	/// </summary>
	public class PlayerAttack
		: AbstractAttack, IPlayerAttack
	{
        #region Переменные и ссылки
        [SerializeField, Tooltip("Хранитель компонентов")]
		private PlayerComponentsControl playerComponentsControl;
		[SerializeField, Tooltip("Частота обновления листа с врагами"), Range(0.05f, 0.5f)]
		private float updateListFrequency;
		[SerializeField, Tooltip("Частота обновления атаки"), Range(0.01f, 0.5f)]
		private float updateAttackFrequency;
		private AbstractEnemy tempEnemy;
		private Transform transformOfPlayer;
		private AbstractEnemy[] listEnemy;

		private PlayerWeapon playerWeapon;
		private PlayerFight playerFight;
		private int isCuttingWeapon;
        #endregion

        /// <summary>
        /// Инициализация
        /// </summary>
        public void Start()
		{
			transformOfPlayer = playerComponentsControl.PlayerObject;
			playerWeapon = playerComponentsControl.PlayerWeapon;
			playerFight = playerComponentsControl.PlayerFight;
			WeaponTypeToBool();
		}

		/// <summary>
		/// Получить ссылку на всех врагов
		/// </summary>
		public void GetReferenceToEnemyArray()
		{
			listEnemy = new AbstractEnemy[StaticStorageWithEnemies.ListEnemy.Length];
			Array.Copy(StaticStorageWithEnemies.ListEnemy,
				listEnemy, StaticStorageWithEnemies.ListEnemy.Length);
			StartCoroutines();
		}

		/// <summary>
		/// Запустить корутины
		/// </summary>
		public void StartCoroutines()
		{
			Timing.RunCoroutine(CoroutineForAttackUpdate());
		}

		/// <summary>
		/// Проверить, является ли враг активным (он жив и он есть)
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		private bool CheckEnemyActiveInArray(int i)
		{
			return listEnemy[i].gameObject.activeSelf &&
				listEnemy[i].EnemyConditions.IsAlive
				? true : false;
		}

		/// <summary>
		/// Корутина на обновление попадания по врагу
		/// </summary>
		/// <returns></returns>
		public IEnumerator<float> CoroutineForAttackUpdate()
		{
			while (playerComponentsControl.PlayerConditions.IsAlive)
			{
				yield return Timing.WaitForSeconds(updateAttackFrequency);
				if (!playerFight.IsDefensing)
				{
					if (playerFight.IsRotating)
					{
						AttackToEnemy(LibraryStaticFunctions.
							AttackToEnemyDamage(playerWeapon)
							, playerWeapon.GemType, false);
					}
					else if (playerFight.IsFighting)
					{
						AttackToEnemy(LibraryStaticFunctions.AttackToEnemyDamageLongAttack
						   (playerWeapon), playerWeapon.GemType, true);
					}
				}
				oldFinishGunPoint = finishGunPoint.position;
			}
		}

		/// <summary>
		/// Метод, который обрабатывает событие, 
		/// которое возникает при возникновении 
		/// критического удара по врагу.
		/// </summary>
		public float CritChanceAttackEvent(float tempDamage)
		{
			if (LibraryStaticFunctions.IsCritHit())
			{
				tempDamage = LibraryStaticFunctions.
					DamageWithCrit(tempDamage, playerWeapon.CritChanceStrenght);
			}

			return tempDamage;
			//do something event
		}

		/// <summary>
		/// Атакуем врага
		/// </summary>
		/// <param name="damage"></param>
		/// <param name="dmgType"></param>
		public void AttackToEnemy(float damage, GemType dmgType, bool isSuperAttack)
		{
			for (int i = 0; i < listEnemy.Length; i++)
			{
				if (CheckEnemyActiveInArray(i) && IsAttackEnemy(i))
				{
					if (listEnemy[i].EnemyConditions.GetDamage
						(CritChanceAttackEvent(damage), playerWeapon.GemPower
						, playerWeapon, isSuperAttack))
					{
						playerComponentsControl.PlayerSounder.
							PlayWeaponHitAudio(isCuttingWeapon);
					}
				}
			}
		}

		/// <summary>
		/// Тип оружия в булево значение
		/// </summary>
		private void WeaponTypeToBool()
		{
			if (playerWeapon.WeaponType == WeaponType.Cutting)
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
			return ((LibraryPhysics.IsAttackEnemy(PlayerPoint.position, finishGunPoint.position,
				listEnemy[i].ReturnEnemyPosition()) &&
				Mathf.Abs(PlayerPoint.position.y
					   - listEnemy[i].ReturnEnemyPosition().y) < 2));
		}

		/// <summary>
		/// Получить точку персонажа
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public Vector3 GetPlayerPoint() // возвращает точки персонажа игрока
		{
			return PlayerPoint.position;
		}

		/// <summary>
		/// Корутина на осуществление урона по врагу
		/// </summary>
		/// <returns></returns>
		public IEnumerator<float> CoroutineMayDoDamage()
		{
			IsMayToDamage = false;
			yield return Timing.WaitForSeconds(attackLatency);
			IsMayToDamage = true;
		}
	}
}


