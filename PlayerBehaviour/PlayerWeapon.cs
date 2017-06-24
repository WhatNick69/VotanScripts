using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerBehaviour
{
	interface IWeapon
	{
		string TypeName1 { get; set; }
		string TypeName2 { get; set; }
		string WeponName { get; set; }
		string GripName { get; set; }

		// Типы урона оружия зависящие от камня
		// Использовать эти значения
		//  FrostDamage, // 0
		//	FireDamage, // 1
		//	PowerfulDamage, // 2
		//	ElectricDamage // 3

		int AttackType { get; set; }
		int Damage { get; set; }
		float SpinSpeed { get; set; }

		void SetWeapon(int damage, int attackType, float spinSpeed);
		void SetWeapon(int damage, int attackType);
		void SetWeapon(int damage);
	}

	public class PlayerWeapon : MonoBehaviour, IWeapon
	{

		public string TypeName1 { get; set; }
		public string TypeName2 { get; set; }
		public string WeponName { get; set; }
		public string GripName { get; set; }

		// Типы урона оружия зависящие от камня
		// Использовать эти значения
		//  FrostDamage, // 0
		//	FireDamage, // 1
		//	PowerfulDamage, // 2
		//	ElectricDamage // 3

		public int AttackType { get; set; }
		public int Damage { get; set; }
		public float SpinSpeed { get; set; }

		public PlayerWeapon(int damage, int attackType, float spinSpeed) // Конструктор
		{
			Damage = damage;
			AttackType = attackType;
			SpinSpeed = spinSpeed;
		}

		// Прегрузки метода котрый измняет показатели оружия 
		public void SetWeapon(int damage, int attackType, float spinSpeed)
		{
			Damage = damage;
			AttackType = attackType;
			SpinSpeed = spinSpeed;
		}

		public void SetWeapon(int damage, int attackType)
		{
			Damage = damage;
			AttackType = attackType;
		}

		public void SetWeapon(int damage)
		{
			Damage = damage;
		}
	}
}
