using UnityEngine;
using UnityEngine.UI;

namespace CraftSystem
{
    /// <summary>
    /// Кнопка для умения, либо предмета
    /// </summary>
    public class ItemOrSkillButton 
        : MonoBehaviour
	{

		int numberButton;
		[SerializeField]
		Image logo;
		ItemsSkillsCraft itemsSkillsCraft;
		[SerializeField]
		Text nameSkill;
		[SerializeField]
		Text tutorialSkill;

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

		public void GetNumberSkill()
		{
            itemsSkillsCraft.SetSkillItemNumber(numberButton);
		}

		public void GetNumberItem()
		{
            itemsSkillsCraft.SetItemNumber(numberButton);
		}

		public void SetItemSkillsCraft(ItemsSkillsCraft itemSC)
		{
            itemsSkillsCraft = itemSC;
		}
	}
}
