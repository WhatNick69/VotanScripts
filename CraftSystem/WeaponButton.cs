using UnityEngine;
using UnityEngine.UI;
using ShopSystem;
using VotanUI;
using System;

namespace CraftSystem
{
    /// <summary>
    /// Кнопка оружия
    /// </summary>
    public class WeaponButton 
        : MonoBehaviour, IRepositoryObject
    {
        #region Переменные
        [SerializeField, Tooltip("Рамка предмета")]
        Image squareItem;
        [SerializeField, Tooltip("Кнопка предмета")]
        Image buttonImage;
        [SerializeField, Tooltip("Подсветка предмета")]
        Image highlighting;
        [SerializeField, Tooltip("Изображение предмета")]
        Image logo;
        [SerializeField, Tooltip("Название предмета")]
        Text nameWeapon;
        [SerializeField, Tooltip("Стоимость предмета в золоте")]
        Text moneyCost;
        [SerializeField, Tooltip("Стоимость предмета в гемах")]
        Text gemsCost;

        private int numberButton;
        string critChance;
        Shop shop;
        WeaponCraft wepCraft;

        private static Color highlightingColor = new Color(0.352f, 1, 0.588f, 1);
        private static Color buttonActiveColor = new Color(0.487f, 0.331f, 0, 1);
        private static Color buttonDeactiveColor = new Color(0.487f, 0.331f, 0, 0.682f);
        bool isMayToBuy;
        #endregion

        /// <summary>
        /// Установить номер предмета в инвентаре (нажать по предмету)
        /// </summary>
        public void GetNumber()
        {
            wepCraft.SetWeaponItemNumber(numberButton);
        }

        /// <summary>
        /// Установить номер предмета в магазине (нажать по предмету)
        /// </summary>
        public void SetNumberItemShop()
        {
            shop.WeaponItemNumber = numberButton;
            shop.ShowItemParameters(3);
            shop.DisableListHighlightings(3);
            HighlightingControl(true);
            MenuSoundManager.PlaySoundStatic(1);
        }

        /// <summary>
        /// Купить выбранный предмет в магазине
        /// </summary>
        public void BuyNumberItemShop()
        {
            if (isMayToBuy)
            {
                shop.WeaponItemNumber = numberButton;
                if (shop.IsEnoughUserResources(3)
                    && shop.BuyWeapon())
                {
                    MenuSoundManager.PlaySoundStatic(1);
                    HighlightingControl(false);
                    shop.ShowNeedUIElements(0, false);
                    wepCraft.RestartWeaponWindow();
                }
            }
            else
            {
                shop.WeaponItemNumber = numberButton;
                shop.ShowItemParameters(3);
                shop.DisableListHighlightings(3, false);
                HighlightingControl(true);
            }
        }

        public void HighlightingControl(bool flag)
        {
            if (flag)
            {
                squareItem.color = highlightingColor;
                buttonImage.color = buttonActiveColor;
                highlighting.enabled = true;
                isMayToBuy = true;
            }
            else
            {
                squareItem.color = Color.white;
                buttonImage.color = buttonDeactiveColor;
                highlighting.enabled = false;
                isMayToBuy = false;
            }
        }

        #region Какие-то методы
        public void SetNumber(int x)
        {
            numberButton = x;
        }

        public void SetWeaponCraft(WeaponCraft WP)
        {
            wepCraft = WP;
        }

		public void SetShop(Shop sh)
		{
			shop = sh;
		}

		public void SetName(string str)
        {
            nameWeapon.text = str;
        }

        public void SetLogo(Sprite sprt)
        {
            logo.sprite = sprt;
        }

        public void SetMoneyCost(long str)
        {
            moneyCost.text = str.ToString();
        }

        public void SetGemsCost(long str)
        {
            gemsCost.text = str.ToString();
        }
        #endregion
    }
}