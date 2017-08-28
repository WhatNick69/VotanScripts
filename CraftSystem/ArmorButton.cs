using UnityEngine;
using UnityEngine.UI;
using PlayerBehaviour;
using VotanUI;
using ShopSystem;

namespace CraftSystem
{
	public class ArmorButton : MonoBehaviour
	{
		int numberButton;
		[SerializeField]
		Image logo;
		ArmorCraft armCraft;
		Shop shop;
		[SerializeField]
		Text nameWeapon;
		[SerializeField]
		Text armor;
		string weight;
        ArmoryClass armoryClass;
		ArmoryClass armoryClassShop;

		public ArmoryClass ArmoryClass
        {
            get
            {
                return armoryClass;
            }

            set
            {
                armoryClass = value;
            }
        }

		public string Weight
		{
			get
			{
				return weight;
			}

			set
			{
				weight = value;
			}
		}

		public ArmoryClass ArmoryClassShop
		{
			get
			{
				return armoryClassShop;
			}

			set
			{
				armoryClassShop = value;
			}
		}

		public void SetNumber(int x)
		{
			numberButton = x;
		}

		public void SetArmorCraft(ArmorCraft AC)
		{
			armCraft = AC;
		}

		public void SetShop(Shop sh)
		{
			shop = sh;
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

        public void SetNumberItem()
        {
            MenuSoundManager.PlaySoundStatic(1);
            switch (armoryClass)
            {
                case ArmoryClass.Cuirass:
                    armCraft.CuirassItemNumber = numberButton;
                    break;
                case ArmoryClass.Helmet:
                    armCraft.HelmetItemNumber = numberButton;
                    break;
                case ArmoryClass.Shield:
                    armCraft.ShieldItemNumber = numberButton;
                    break;
            }
        }

		public void SetNumberItemShop()
		{
			MenuSoundManager.PlaySoundStatic(1);
			switch (armoryClassShop)
			{
				case ArmoryClass.Cuirass:
					shop.CuirassItemNumber = numberButton;
					break;
				case ArmoryClass.Helmet:
					shop.HelmetItemNumber = numberButton;
					break;
				case ArmoryClass.Shield:
					shop.ShieldItemNumber = numberButton;
					break;
			}
		}
	}
}
