using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AbstractBehaviour;
using Playerbehaviour;
using VotanUI;

namespace PlayerBehaviour
{
	public class PlayerWeaponConstructor : MonoBehaviour
	{
		[SerializeField] // точка приаязанная к провой руке(кости), отождествлят оружие
		GameObject weapon;
		PlayerComponentsControl plComponents;
		PlayerWeapon plWeapon;

		float weaponWeight;
		float weaponDamage;
		float weaponDefence;
		DamageType weaponDamType;
		float weaponSpinSpeed;

		Grip gripClass;
		Head headClass;
		Gem gemClass;
		GameObject grip;
		GameObject head;
		GameObject gem;

		Vector3 attackPoint;
		Vector3 headPosition;
		Vector3 headRotate = new Vector3(35, 0, 0);
		Vector3 gripPoint;

		Vector3 shortGripPosition = new Vector3();
		Vector3 shortGripRotate = new Vector3(35, 0, 0);

		Vector3 midleGripPosition = new Vector3(-30, 0, 0);
		Vector3 midleGripRotate = new Vector3(35, 0, 0);		

		Vector3 longGripPosition = new Vector3();
		Vector3 longGripRotate = new Vector3(35, 0, 0);

		Vector3 verilongGripPosition = new Vector3();
		Vector3 verilongGripRotate = new Vector3(35, 0, 0);

		private int gripType;

		private void WeaponSettings()
		{
			switch (gripClass.GetGripType())
			{
				case 0:
					gripPoint = shortGripPosition;
					headPosition = new Vector3(); //
					attackPoint = new Vector3(-0.6f, 10.5f, 50);
					break;
				case 1:
					gripPoint = midleGripPosition;
					headPosition = new Vector3(-60, 0, 0);
					attackPoint = new Vector3(-60, 0, 0);
					break;
				case 2:
					gripPoint = longGripPosition;
					headPosition = new Vector3(); //
					
					break;
				case 3:
					gripPoint = verilongGripPosition;
					headPosition = new Vector3(); //
					
					break;
				default:
					gripPoint = Vector3.zero; break;
			}
		}

		private void SetWeaponStats()
		{
			weaponWeight = gripClass.GetWeight() + headClass.GetWeight();
			weaponDamage = weaponWeight;
			weaponSpinSpeed = weaponWeight/2 + gripClass.GetBonusSpinSpeed() + headClass.GetBonusSpinSpeed();
			weaponDefence = weaponWeight * 0.25f;
			weaponDamType = gemClass.GetDamageType();

			plComponents.PlayerWeapon.SetWeaponParameters(weaponDamage, weaponDefence, weaponDamType, weaponSpinSpeed, weaponWeight);
		}

		private void Awake()
		{
			plComponents = GameObject.FindWithTag("Player").GetComponent<PlayerComponentsControl>();
			grip = Instantiate(GameObject.Find("GetWeaponPrefabs").GetComponent<WeaponCraft>().GetGripPrafab());
			head = Instantiate(GameObject.Find("GetWeaponPrefabs").GetComponent<WeaponCraft>().GetHeadPrafab());
			gem = Instantiate(GameObject.Find("GetWeaponPrefabs").GetComponent<WeaponCraft>().GetGemPrafab());
			gripClass = grip.GetComponent<Grip>();
			headClass = head.GetComponent<Head>();
			gemClass = gem.GetComponent<Gem>();

			WeaponSettings();

			grip.transform.parent = weapon.transform;
			grip.transform.localPosition = gripPoint;
			grip.transform.localEulerAngles = midleGripRotate;
			grip.transform.localScale = new Vector3(1,1,1);

			head.transform.parent = weapon.transform;
			head.transform.localPosition = headPosition;
			head.transform.localEulerAngles = headRotate;
			head.transform.localScale = new Vector3(1, 1, 1);

		    gem.transform.parent = weapon.transform;
		    gem.transform.localPosition = headPosition;
			gem.transform.localEulerAngles = headRotate;
		    gem.transform.localScale = new Vector3(1, 1, 1);

			plComponents.PlayerAttack.SetPlayerGanLocalPoint(attackPoint);
			SetWeaponStats();
		}

	}
	
}
