using UnityEngine;
using UnityEngine.UI;

namespace CraftSystem
{
    public class HeadButton : MonoBehaviour
	{
		int numberButton;
		[SerializeField]
		Image logo;
		WeaponCraft wepCraft;
		[SerializeField]
		Text nameWeapon;
		[SerializeField]
		Text weight;
		[SerializeField]
		Text damage;
		[SerializeField]
		Text spinSpeedBous;
		[SerializeField]
		Text type;

		public void SetNumber(int x)
		{
			numberButton = x;
		}

		public void GetNumber()
		{
			wepCraft.SetHeadItemNumber(numberButton);
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

		public void SetDamage(string str)
		{
			damage.text = str;
		}

		public void SetType(string str)
		{
			type.text = str;
		}

		public void SetLogo(Sprite sprt)
		{
			logo.sprite = sprt;
		}
	}
}