using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CraftSystem
{
	public class ItemOrSkillButton : MonoBehaviour
	{

		int numberButton;
		[SerializeField]
		Image logo;
		WeaponCraft wepCraft;
		[SerializeField]
		Text nameSkill;
		[SerializeField]
		Text tutorialSkill;


		public void SetImage(Sprite img)
		{
			logo.sprite = img;
		}

		public Text NameSkill
		{
			get
			{
				return nameSkill;
			}

			set
			{
				nameSkill = value;
			}
		}

		public Text TutorialSkill
		{
			get
			{
				return tutorialSkill;
			}

			set
			{
				tutorialSkill = value;
			}
		}

		public void SetNumber(int x)
		{
			numberButton = x;
		}

		public void GetNumberSkill()
		{
			wepCraft.SetSkillItemNumber(numberButton);
		}

		public void GetNumberItem()
		{
			wepCraft.SetItemNumber(numberButton);
		}

		public void SetWeaponCraft(WeaponCraft WP)
		{
			wepCraft = WP;
		}
	}
}
