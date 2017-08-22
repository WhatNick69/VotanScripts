using UnityEngine;
using UnityEngine.UI;
using PlayerBehaviour;

namespace CraftSystem
{
    public class WeaponButton 
        : MonoBehaviour
    {
        int numberButton;
        [SerializeField]
        Image logo;
        WeaponCraft wepCraft;
        [SerializeField]
        Text nameWeapon;
        [SerializeField]
        Sprite fireLogo;
        [SerializeField]
        Sprite frostLogo;
        [SerializeField]
        Sprite electricLogo;
        [SerializeField]
        Sprite powerfulLogo;
        [SerializeField]
        Image gemTypeImg;
        string critChance;

        public void SetNumber(int x)
        {
            numberButton = x;
        }

        public void GetNumber()
        {
            wepCraft.SetWeaponItemNumber(numberButton);
        }

        public void SetWeaponCraft(WeaponCraft WP)
        {
            wepCraft = WP;
        }

        public void SetName(string str)
        {
            nameWeapon.text = str;
        }

        public void SetLogo(Sprite sprt)
        {
            logo.sprite = sprt;
        }

        public void SetGemType(GemType DT)
        {
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
    }
}