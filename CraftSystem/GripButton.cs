using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CraftSystem
{
	public class GripButton : MonoBehaviour
	{

		int numberButton;
		[SerializeField]
		WeaponCraft wepCraft;
		[SerializeField]
		Text nameWeapon;
		[SerializeField]
		Text weight;
		[SerializeField]
		Text spinSpeedBous;
		[SerializeField]
		Text defence;
		[SerializeField]
		Text type;


		public void SetNumber(int x)
		{
			numberButton = x;
		}

		public void GetNumber()
		{
			wepCraft.SetGripItemNumber(numberButton);
		}

		public void SetWeaponCraft(WeaponCraft WP)
		{
			wepCraft = WP;
		}

		public void SetName(string str)
		{
			nameWeapon.text = str;
		}

		public void SetWeight(string str)
		{
			weight.text = str;
		}

		public void SetSpinBous(string str)
		{
			spinSpeedBous.text = str;
		}

		public void SetDefence(string str)
		{
			defence.text = str;
		}

		public void SetType(string str)
		{
			type.text = str;
		}
	}
}
