using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayerBehaviour;

namespace CraftSystem
{
	public class GemButton : MonoBehaviour
	{
		int numberButton;
		[SerializeField]
		Text nameWeapon;
		[SerializeField]
		Text gemPower;
		[SerializeField]
		Text gemType;
		[SerializeField]
		WeaponCraft wepCraft;

		public void SetNumber(int x)
		{
			numberButton = x;
		}

		public void GetNumber()
		{
			wepCraft.SetGemItemNumber(numberButton);
		}

		public void SetWeaponCraft(WeaponCraft WP)
		{
			wepCraft = WP;
		}

		public void SetName(string str)
		{
			nameWeapon.text = str;
		}

		public void SetGemPower(string str)
		{
			gemPower.text = str;
		}

		public void SetGemType(DamageType DT)
		{
			gemType.text = DT.ToString();
		}
	}
}
