﻿using UnityEngine;
using UnityEngine.UI;
using PlayerBehaviour;

namespace CraftSystem
{
	public class ArmorButton : MonoBehaviour
	{
		int numberButton;
		[SerializeField]
		Image logo;
		ArmorCraft armCraft;
		[SerializeField]
		Text nameWeapon;
		[SerializeField]
		Text armor;
		
		public void SetNumber(int x)
		{
			numberButton = x;
		}

		public void SetArmorCraft(ArmorCraft AC)
		{
			armCraft = AC;
		}

		public void SetName(string str)
		{
			nameWeapon.text = str;
		}

		public void SetArmor(string str)
		{
			armor.text = str;
		}

		public void SetLogo(Sprite sprt)
		{
			logo.sprite = sprt;
		}
	}
}
