using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AbstractBehaviour;

namespace PlayerBehaviour
{
	/// <summary>
    /// Интерфейс для описания оружия
    /// </summary>
    interface IWeapon
	{
        DamageType attackType { get; set; }
        string typeName1 { get; set; }
		string typeName2 { get; set; }
		string weaponName { get; set; }
		string gripName { get; set; }
		GameObject player { get; set; }

		int damage { get; set; }
		float spinSpeed { get; set; }

		void SetWeapon(int damage, DamageType attackType, float spinSpeed);
		void SetWeapon(int damage, DamageType attackType);
		void SetWeapon(int damage);
	}

	/// <summary>
    /// Класс, описывающий поведение оружия
    /// </summary>
    public class PlayerWeapon 
        : MonoBehaviour, IWeapon
	{
		public DamageType attackType { get; set; }
        public string typeName1 { get; set; }
		public string typeName2 { get; set; }
		public string weaponName { get; set; }
		public string gripName { get; set; }
		[SerializeField]
		public GameObject player { get; set; }

		public int damage { get; set; }
		public float spinSpeed { get; set; }
		
		/// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="attackType"></param>
        /// <param name="spinSpeed"></param>
        public PlayerWeapon(int damage, DamageType attackType, float spinSpeed) 
		{
			this.damage = damage;
			this.attackType = attackType;
			this.spinSpeed = spinSpeed;
			player = GameObject.FindWithTag("Player");
			player.GetComponent<PlayerFight>().GetWeaponParameters(damage, attackType , spinSpeed);
		}

		/// <summary>
        /// Задать характеристики оружия через урон, тип урона и скорость
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="attackType"></param>
        /// <param name="spinSpeed"></param>
        public void SetWeapon(int damage, DamageType attackType, float spinSpeed)
		{
			this.damage = damage;
			this.attackType = attackType;
			this.spinSpeed = spinSpeed;
		}

		/// <summary>
        /// Задать характеристики оружия через урон и тип урона
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="attackType"></param>
        public void SetWeapon(int damage, DamageType attackType)
		{
			this.damage = damage;
			this.attackType = attackType;
		}

		/// <summary>
        /// Задать характеристики оружия через урон
        /// </summary>
        /// <param name="damage"></param>
        public void SetWeapon(int damage)
		{
			this.damage = damage;
		}
	}
}
