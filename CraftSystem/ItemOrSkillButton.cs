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

		public Image Logo
		{
			get
			{
				return logo;
			}

			set
			{
				logo = value;
			}
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

		public void GetNumber()
		{
			wepCraft.SetSkillItemNumber(numberButton);
		}

		public void SetWeaponCraft(WeaponCraft WP)
		{
			wepCraft = WP;
		}
	}
}
