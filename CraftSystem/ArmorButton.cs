using UnityEngine;
using UnityEngine.UI;

namespace CraftSystem
{
    public class ArmorButton : MonoBehaviour
	{
		int numberButton;
		[SerializeField]
		ArmorCraft armCraft;
		[SerializeField]
		Text nameWeapon;
		[SerializeField]
		Text armor;

		public void SetNumber(int x)
		{
			numberButton = x;
		}

		public void GetNumber()
		{
			armCraft.SetShieldItemNumber(numberButton);
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
	}
}
