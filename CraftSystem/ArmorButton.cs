using UnityEngine;
using UnityEngine.UI;
using PlayerBehaviour;

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
		ArmoryClass ac;
		Image logo;

		public void SetNumber(int x)
		{
			numberButton = x;
		}

		public void GetNumber()
		{
			switch (ac)
			{
				case ArmoryClass.Cuirass:
					armCraft.SetCuirassItemNumber(numberButton);
					break;
				case ArmoryClass.Helmet:
					armCraft.SetHelmetItemNumber(numberButton);
					break;
				case ArmoryClass.Shield:
					armCraft.SetShieldItemNumber(numberButton);
					break;
			}
		}

		public void SetArmorCraft(ArmorCraft AC)
		{
			armCraft = AC;
		}

		public void SetArmorClass(ArmoryClass ac)
		{
			this.ac = ac;
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
