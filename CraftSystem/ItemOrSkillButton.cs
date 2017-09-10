using ShopSystem;
using System;
using UnityEngine;
using UnityEngine.UI;
using VotanUI;

namespace CraftSystem
{
    /// <summary>
    /// Кнопка для умения, либо предмета
    /// </summary>
    public class ItemOrSkillButton 
        : MonoBehaviour, IRepositoryObject
    {
        #region Переменные и ссылки
        int numberButton;
        [SerializeField]
		Image logo;
        [SerializeField, Tooltip("Рамка предмета")]
        Image squareItem;
        [SerializeField, Tooltip("Подсветка предмета")]
        Image highlighting;
        [SerializeField, Tooltip("Кнопка предмета")]
        Image buttonImage;
        ItemsSkillsCraft itemsSkillsCraft;
        Shop shop;
		[SerializeField]
		Text nameSkill;
		[SerializeField]
		string tutorialSkill;
        [SerializeField]
        Text moneyCost;
        [SerializeField]
        Text gemsCost;
        private bool isMayToBuy;

        private static Color highlightingColor = new Color(0.352f, 1, 0.588f, 1);
        private static Color buttonActiveColor = new Color(0.487f, 0.331f, 0, 1);
        private static Color buttonDeactiveColor = new Color(0.487f, 0.331f, 0, 0.682f);
        #endregion

        #region Свойства
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

		public string TutorialSkill
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

        public Text GemsCost
        {
            get
            {
                return gemsCost;
            }

            set
            {
                gemsCost = value;
            }
        }

        public Text MoneyCost
        {
            get
            {
                return moneyCost;
            }

            set
            {
                moneyCost = value;
            }
        }

        public void SetNumber(int x)
		{
			numberButton = x;
		}

		public void GetNumberItem()
		{
            itemsSkillsCraft.SetItemNumber(numberButton);
		}

        public void GetNumberSkillTemp()
        {
            itemsSkillsCraft.SetSkillItemNumber(numberButton);
            itemsSkillsCraft.DisableListHighlightingInventory();
            HighlightingControl(true, false);
            MenuSoundManager.PlaySoundStatic(1);
        }

		public void SetItemSkillsCraft(ItemsSkillsCraft itemSC)
		{
            itemsSkillsCraft = itemSC;
		}
        #endregion

        public void GetNumberSkill()
        {
            shop.SkillItemNumber = numberButton;
            itemsSkillsCraft.DisableListHighlightingInventory();
            shop.ShowItemParameters(4);
            HighlightingControl(true, false);
            MenuSoundManager.PlaySoundStatic(1);
        }

        public void HighlightingControl(bool flag, bool isHaveButton = true)
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

        public void SetShop(Shop shopComponent)
        {
            shop = shopComponent;
        }
    }
}
