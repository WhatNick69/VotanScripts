using UnityEngine;
using UnityEngine.UI;
using PlayerBehaviour;
using VotanUI;
using ShopSystem;
using System;

namespace CraftSystem
{
	/// <summary>
    /// Кнопка брони
    /// </summary>
    public class ArmorButton 
        : MonoBehaviour, IRepositoryObject
	{
        #region переменные
        [SerializeField, Tooltip("Рамка предмета")]
        Image squareItem;
        [SerializeField, Tooltip("Кнопка предмета")]
        Image buttonImage;
        [SerializeField, Tooltip("Подсветка предмета")]
        Image highlighting;
        [SerializeField,Tooltip("Изображение предмета")]
		Image logo;
		[SerializeField,Tooltip("Название предмета")]
		Text nameWeapon;
		[SerializeField, Tooltip("Стоимость предмета в золоте")]
		Text moneyCost;
        [SerializeField, Tooltip("Стоимость предмета в гемах")]
        Text gemsCost;

        private static Color highlightingColor = new Color(0.352f,1,0.588f,1);
        private static Color buttonActiveColor = new Color(0.487f, 0.331f, 0, 1);
        private static Color buttonDeactiveColor = new Color(0.487f, 0.331f, 0, 0.682f);

        string weight;
        public int numberButton;
        bool isMayToBuy;

        ArmoryClass armoryClass;
		ArmoryClass armoryClassShop;
        ArmorCraft armCraft;
        Shop shop;
        #endregion

        #region Свойства
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

		public void SetMoneyCost(long str)
		{
			moneyCost.text = str.ToString();
		}

        public void SetGemsCost(long str)
        {
            gemsCost.text = str.ToString();
        }

        public void SetLogo(Sprite sprt)
		{
			logo.sprite = sprt;
		}
        #endregion

        /// <summary>
        /// Установить номер предмета в инвентаре (нажать по предмету)
        /// </summary>
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

        public void SetNumberTemp()
        {
            switch (armoryClass)
            {
                case ArmoryClass.Cuirass:
                    armCraft.CuirassItemNumber = numberButton;
                    //armCraft.HelmetItemNumberTemp = -1;
                    //armCraft.ShieldItemNumberTemp = -1;
                    armCraft.DisableListHighlightingInventory(0);
                    HighlightingControl(true,false);
                    MenuSoundManager.PlaySoundStatic(1);
                    break;

                case ArmoryClass.Helmet:
                    armCraft.HelmetItemNumber = numberButton;
                    //armCraft.CuirassItemNumberTemp = -1;
                    //armCraft.ShieldItemNumberTemp = -1;
                    armCraft.DisableListHighlightingInventory(1);
                    HighlightingControl(true,false);
                    MenuSoundManager.PlaySoundStatic(1);
                    break;

                case ArmoryClass.Shield:
                    armCraft.ShieldItemNumber = numberButton;
                    //armCraft.HelmetItemNumberTemp = -1;
                    //armCraft.CuirassItemNumberTemp = -1;
                    armCraft.DisableListHighlightingInventory(2);
                    HighlightingControl(true,false);
                    MenuSoundManager.PlaySoundStatic(1);
                    break;
            }
        }

        /// <summary>
        /// Установить номер предмета в магазине (нажать по предмету)
        /// </summary>
        public void SetNumberItemShop()
        {
            switch (armoryClassShop)
            {
                case ArmoryClass.Cuirass:
                    shop.CuirassItemNumber = numberButton;
                    shop.ShowItemParameters(0);
                    shop.DisableListHighlightingShop(0);
                    HighlightingControl(true);
                    MenuSoundManager.PlaySoundStatic(1);
                    break;

                case ArmoryClass.Helmet:
                    shop.HelmetItemNumber = numberButton;
                    shop.ShowItemParameters(1);
                    shop.DisableListHighlightingShop(1);
                    HighlightingControl(true);
                    MenuSoundManager.PlaySoundStatic(1);
                    break;

                case ArmoryClass.Shield:
                    shop.ShieldItemNumber = numberButton;
                    shop.ShowItemParameters(2);
                    shop.DisableListHighlightingShop(2);
                    HighlightingControl(true);
                    MenuSoundManager.PlaySoundStatic(1);
                    break;
            }
        }

        /// <summary>
        /// Контроль подсветки
        /// </summary>
        /// <param name="flag"></param>
        public void HighlightingControl(bool flag,bool isHaveButton=true)
        {
            if (flag)
            {
                squareItem.color = highlightingColor;
                if (isHaveButton)
                    buttonImage.color = buttonActiveColor;
                highlighting.enabled = true;
                isMayToBuy = true;
            }
            else
            {
                squareItem.color = Color.white;
                if (isHaveButton)
                    buttonImage.color = buttonDeactiveColor;
                highlighting.enabled = false;
                isMayToBuy = false;
            }
        }

		/// <summary>
        /// Купить выбранный предмет в магазине
        /// </summary>
        public void BuyNumberItemShop()
		{
            if (isMayToBuy)
            {
                switch (armoryClassShop)
                {
                    case ArmoryClass.Cuirass:
                        shop.CuirassItemNumber = numberButton;
                        if (shop.IsEnoughUserResources(0)
                            && shop.BuyCuirass())
                        {
                            MenuSoundManager.PlaySoundStatic(1);
                            HighlightingControl(false);
                            shop.ShowNeedUIElements(0, false);
                            armCraft.RestartCuirassWindow();
                        }
                        break;
                    case ArmoryClass.Helmet:
                        shop.HelmetItemNumber = numberButton;
                        if (shop.IsEnoughUserResources(1)
                            && shop.BuyHelmet())
                        {
                            MenuSoundManager.PlaySoundStatic(1);
                            HighlightingControl(false);
                            shop.ShowNeedUIElements(0, false);
                            armCraft.RestartHelmetWindow();
                        }
                        break;
                    case ArmoryClass.Shield:
                        shop.ShieldItemNumber = numberButton;
                        if (shop.IsEnoughUserResources(2)
                            && shop.BuyShield())
                        {
                            MenuSoundManager.PlaySoundStatic(1);
                            HighlightingControl(false);
                            shop.ShowNeedUIElements(0, false);
                            armCraft.RestartShieldWindow();
                        }
                        break;
                }
            }
            else
            {
                switch (armoryClassShop)
                {
                    case ArmoryClass.Cuirass:
                        shop.CuirassItemNumber = numberButton;
                        shop.ShowItemParameters(0);
                        shop.DisableListHighlightingShop(0, false);
                        HighlightingControl(true);
                        break;
                    case ArmoryClass.Helmet:
                        shop.HelmetItemNumber = numberButton;
                        shop.ShowItemParameters(1);
                        shop.DisableListHighlightingShop(1, false);
                        HighlightingControl(true);
                        break;
                    case ArmoryClass.Shield:
                        shop.ShieldItemNumber = numberButton;
                        shop.ShowItemParameters(2);
                        shop.DisableListHighlightingShop(2, false);
                        HighlightingControl(true);
                        break;
                }
            }
        }
    }
}
