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
		Sprite fireLogo;
		[SerializeField]
		Sprite frostLogo;
		[SerializeField]
		Sprite electricLogo;
		[SerializeField]
		Sprite powerfulLogo;
		[SerializeField]
		Image logo;
		[SerializeField]
		Image gemTypeImg;
		[SerializeField]
		Text nameWeapon;
		[SerializeField]
		Text gemPower;
		[SerializeField]
		Text gemType;
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

		public void SetGemType(GemType DT)
		{
			gemType.text = DT.ToString();
			switch (DT)
			{
				case GemType.Electric:
					gemTypeImg.sprite = electricLogo;
					break;

				case GemType.Fire:
					gemTypeImg.sprite = fireLogo;
					break;

				case GemType.Frozen:
					gemTypeImg.sprite = frostLogo;
					break;

				case GemType.Powerful:
					gemTypeImg.sprite = powerfulLogo;
					break;

			}
		}

		public void SetLogo(Sprite sprt)
		{
			logo.sprite = sprt;
		}
	}
}
