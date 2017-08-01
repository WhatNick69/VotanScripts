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
        [SerializeField, Tooltip("Хранитель компонентов")]
        private PlayerComponentsControl playerComponentsControl;
        [SerializeField, Tooltip("Частота обновления листа с врагами"),Range(0.05f,0.5f)]
        private float updateListFrequency;
        [SerializeField, Tooltip("Частота обновления атаки"), Range(0.01f, 0.5f)]
        private float updateAttackFrequency;
        private AbstractEnemy tempEnemy;
        private Transform transformOfPlayer;
        [SerializeField]
        private AbstractEnemy[] listEnemy;

        private PlayerWeapon playerWeapon;
        private int isCuttingWeapon;

        /// <summary>
        /// Инициализация
        /// </summary>
        public void Start()
        {        
            transformOfPlayer = playerComponentsControl.PlayerObject;
            playerWeapon = playerComponentsControl.PlayerWeapon;
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
                if (!playerComponentsControl.PlayerFight.IsDefensing)
                {
                    if (playerComponentsControl.PlayerFight.IsRotating)
                    {
                        AttackToEnemy(LibraryStaticFunctions.AttackToEnemyDamage
                            (playerWeapon.Damage,playerWeapon.SpinSpeed,
                            playerWeapon.OriginalSpinSpeed),
                            playerWeapon.AttackType,false);
                    }
                    else if (playerComponentsControl.PlayerFight.IsFighting)
                    {
                        AttackToEnemy(LibraryStaticFunctions.AttackToEnemyDamageLongAttack
                           (playerWeapon),playerWeapon.AttackType,true);
                    }
                }
                oldFinishGunPoint = playerFinishGunPoint.position;   
            }
        }

        /// <summary>
        /// Атакуем врага
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="dmgType"></param>
        public void AttackToEnemy(float damage, DamageType dmgType,bool isSuperAttack)
        {
            for (int i = 0; i < listEnemy.Length; i++)
            {
                if (CheckEnemyActiveInArray(i) && IsAttackEnemy(i))
                {
                    if (listEnemy[i].EnemyConditions.GetDamage
                        (damage, playerWeapon.GemPower
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
			return ((LibraryPhysics.BushInPlane(listEnemy[i].
                ReturnPosition(4), playerPoint.position,
						playerFinishGunPoint.position, oldFinishGunPoint) ||
                        LibraryPhysics.BushInLine(listEnemy[i].ReturnPosition(0),
                        listEnemy[i].ReturnPosition(1),
							PlayerFinishGunPoint.position, PlayerStartGunPoint.position)) && 
							Mathf.Abs(playerPoint.position.y 
                            - listEnemy[i].ReturnPosition(0).y) < 1.6f);
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

