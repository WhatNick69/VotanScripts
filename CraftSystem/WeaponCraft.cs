using UnityEngine;
using System.Collections.Generic;
using MovementEffects;
using VotanUI;
using UnityEngine.UI;
using VotanInterfaces;
using VotanLibraries;
using ShopSystem;

namespace CraftSystem
{
    /// <summary>
    /// Система крафта оружия
    /// </summary>
    public class WeaponCraft
		: MonoBehaviour
	{
		#region Переменные
		[SerializeField]
		private GameObject weaponRepository;
		[SerializeField]
		private GameObject itemRepository;
		[SerializeField]
		private GameObject skillsRepository;

		[SerializeField, Tooltip("кнопка оружия")]
		private GameObject itemWeapon;
		[SerializeField, Tooltip("кнопка скила")]
		private GameObject itemSkill;
		[SerializeField, Tooltip("кнопка итема")]
		private GameObject itemItm;

		[SerializeField]
		GameObject weaponWindow;
		[SerializeField]
		GameObject weaponUpadateButton; // Отображать вместе с weaponWindow
		[SerializeField]
		GameObject skillWindow;
		[SerializeField]
		GameObject itemWindow;

		private GameObject weapon;
		private GameObject weaponGamObj;
		private GameObject skillGamObj;
		private GameObject itemGamObj;

		string headPrefix = "Prefabs/Weapons/Weapon_";
		string skillPrefix = "Prefabs/Skills/";
		string itemPrefix = "Prefabs/Items/";

		private List<ISkill> skillList;
		private List<IItem> itemList;
		private List<Weapon> weaponList;
        private IRepositoryObject[] weaponInventoryElements;

        private GameObject[] weaponArray;
		private int[] weaponStats;

		[SerializeField, Tooltip("Закинуть сюда все зелья")]
		private List<GameObject> itemArray;

		public int weaponItemNumber;
        private int weaponItemNumberTemp = -1;

		private int itemItemNumberOne;
		private int itemItemNumberTwo;
		private int itemItemNumberThree;

		private int skillItemNumberOne;
		private int skillItemNumberTwo;
		private int skillItemNumberThree;

		[SerializeField]
		WeaponPrefabs WP;
		[SerializeField]
		ItemSkillPrefabs ItemSkillPef;
		WeaponCraft WC;
        [SerializeField]
        Shop shop;
        ArmorCraft AC;
		PlayerStats PStats;

        ScrollRect scrollRectWeaponRepository;
		ScrollRect scrollRectSkillRepository;
		ScrollRect scrollRectItemRepository;

		float intemNumbWeapon;
		float normPosWeapon;

        public int WeaponItemNumberTemp
        {
            get
            {
                return weaponItemNumberTemp;
            }

            set
            {
                weaponItemNumberTemp = value;
            }
        }
        #endregion

        #region Свойства
        public void CloseAllWindows()
		{
			skillWindow.SetActive(false);
			itemWindow.SetActive(false);
			weaponWindow.SetActive(false);
			weaponUpadateButton.SetActive(false);
		}

		/// <summary>
		/// Вызывать для открытия окна с оружием
		/// </summary>
		public void WeaponWindow()
		{
			skillWindow.SetActive(false);
			itemWindow.SetActive(false);
			weaponWindow.SetActive(true);
			weaponUpadateButton.SetActive(true);

            AC.CloseAllWindows();

            PStats.WeaponPage();
			scrollRectWeaponRepository.horizontalNormalizedPosition = 0;
        }

		/// <summary>
		/// Вызывать для открытия окна со скилами
		/// </summary>
		public void SkillWindow()
		{
			skillWindow.SetActive(true);
			itemWindow.SetActive(false);
			weaponWindow.SetActive(false);

            AC.CloseAllWindows();

            weaponUpadateButton.SetActive(false);
			PStats.SkillPage();
			scrollRectWeaponRepository.horizontalNormalizedPosition = 0;
		}

		/// <summary>
		/// Вызывать для открытия окна с итемами
		/// </summary>
		public void ItemWindow()
		{
			skillWindow.SetActive(false);
			itemWindow.SetActive(true);
			weaponWindow.SetActive(false);
			weaponUpadateButton.SetActive(false);

            AC.CloseAllWindows();

            PStats.SkillPage();
			scrollRectWeaponRepository.horizontalNormalizedPosition = 0;
		}
		/// <summary>
		/// При экипировке элемента оружия, его характеристики
		/// отправляются в таблицу
		/// </summary>
		/// <param name="x"></param>
		public void SetWeaponItemNumber(int x)
		{
            MenuSoundManager.PlaySoundStatic(1);
            weaponItemNumber = x;
			PStats.HeadDamage = weaponList[x].DamageBase;
			PStats.CritChance = weaponList[x].CriticalChance;
		}

        public void EquipWeapon()
        {
            if (weaponItemNumberTemp != -1)
            {
                SetWeaponItemNumber(weaponItemNumberTemp);
                weaponItemNumberTemp = -1;
            }
        }

		public void SetSkillItemNumber(int numberButton)
		{
			if (skillItemNumberOne == -1)
			{
				MenuSoundManager.PlaySoundStatic(1);
				skillItemNumberOne = numberButton;
				PStats.SetSkillImg(skillList[numberButton].SkillImage.sprite, 0);
			}
			else if (skillItemNumberTwo == -1)
			{
				MenuSoundManager.PlaySoundStatic(1);
				skillItemNumberTwo = numberButton;
				PStats.SetSkillImg(skillList[numberButton].SkillImage.sprite, 1);
			}
			else if (skillItemNumberThree == -1)
			{
				MenuSoundManager.PlaySoundStatic(1);
				skillItemNumberThree = numberButton;
				PStats.SetSkillImg(skillList[numberButton].SkillImage.sprite, 2);
			}
		}

		public void SetItemNumber(int numberButton)
		{
			if (itemItemNumberOne == -1)
			{
				MenuSoundManager.PlaySoundStatic(1);
				itemItemNumberOne = numberButton;
				PStats.SetItemImg(itemList[numberButton].ItemImage.sprite, 0);
			}
			else if (itemItemNumberTwo == -1)
			{
				MenuSoundManager.PlaySoundStatic(1);
				itemItemNumberTwo = numberButton;
				PStats.SetItemImg(itemList[numberButton].ItemImage.sprite, 1);
			}
			else if (itemItemNumberThree == -1)
			{
				MenuSoundManager.PlaySoundStatic(1);
				itemItemNumberThree = numberButton;
				PStats.SetItemImg(itemList[numberButton].ItemImage.sprite, 2);
			}
		}

		/// <summary>
		/// Улучшает выбранное оружие
		/// </summary>
		public void UpdateWeapon()
		{
			weaponStats = LibraryObjectsWorker.StringSplitter(PlayerPrefs.GetString("weapon_" + weaponItemNumber), '_');
			if (weaponStats[0] < 3)
			{
				weaponStats[0] = weaponStats[0] + 1;
				PlayerPrefs.SetString("weapon_" + weaponItemNumber, 
					(weaponStats[0] + "_" + weaponStats[1] + "_" + weaponStats[2]).ToString());
				PlayerPrefs.Save();
			}
		}

		/// <summary>
		/// Вызывать для удаления умения из 1й ячейки 
		/// </summary>
		public void RemoveSkillOne()
		{
			skillItemNumberOne = -1;
			PStats.SetSkillImg(0);
		}

		/// <summary>
		/// Вызывать для удаления умения из 2й ячейки 
		/// </summary>
		public void RemoveSkillTwo()
		{
			skillItemNumberTwo = -1;
			PStats.SetSkillImg(1);
		}

		/// <summary>
		/// Вызывать для удаления умения из 3й ячейки 
		/// </summary>
		public void RemoveSkillThree()
		{
			skillItemNumberThree = -1;
			PStats.SetSkillImg(2);
		}

		/// <summary>
		///  Вызывать для удаления зелья из 1й ячейки 
		/// </summary>
		public void RemoveItemOne()
		{
			itemItemNumberOne = -1;
			PStats.SetItemImg(0);
		}

		/// <summary>
		///  Вызывать для удаления зелья из 2й ячейки 
		/// </summary>
		public void RemoveItemTwo()
		{
			itemItemNumberTwo = -1;
			PStats.SetItemImg(1);
		}

		/// <summary>
		/// Вызывать для удаления зелья из 3й ячейки 
		/// </summary>
		public void RemoveItemThree()
		{
			itemItemNumberThree = -1;
			PStats.SetItemImg(2);
		}

		/// <summary>
		/// Обновляет окно оружия. Вызывать при покупке
		/// </summary>
		public void RestartWeaponWindow()
		{
			int k = LibraryObjectsWorker.StringSplitter
				(PlayerPrefs.GetString("weaponArray"), '_').Length - 1;

			for (int i = 0; i < k; i++)
			{
				GameObject d = weaponRepository.transform.GetChild(0).gameObject;
				weaponRepository.transform.GetChild(0).SetParent(null);
				Destroy(d);
				weaponArray[i] = null;
				weaponList.RemoveAt(0);
			}
			Timing.RunCoroutine(WeaponCorutine());
		}

		/// <summary>
		/// 1. Двинул ли игрок ленту с элементаи оружия
		/// 2. Сменился элемент на следующий или нет? Если да, то меняет номер элемента
		/// кторый отправится в таблицу с характеристиками
		/// 
		/// - После этого, в таблицу отправляются значения элементов, которые находятся
		/// по центру окна прокрутки (определяется предыдущими проверками)
		/// - В самом кенце сохраняется позиция ленты, для проверки в следющем кадре 1го условия
		/// </summary>
		private void ChekScroll()
		{
			if (normPosWeapon != scrollRectWeaponRepository.horizontalNormalizedPosition)
			{
				if (weaponItemNumber != Mathf.Round(scrollRectWeaponRepository.horizontalNormalizedPosition * (weaponList.Count - 1)))
				{
					intemNumbWeapon = Mathf.Round(scrollRectWeaponRepository.horizontalNormalizedPosition * (weaponList.Count - 1));
				}
				else
				{
					intemNumbWeapon = weaponItemNumber;
				}

				PStats.NewHeadDamage = weaponList[(int)intemNumbWeapon].DamageBase;
				PStats.NewCritChance = weaponList[(int)intemNumbWeapon].CriticalChance;
			}
			normPosWeapon = scrollRectWeaponRepository.horizontalNormalizedPosition;
			PStats.NewStats();
		}
		#endregion

		/// <summary>
		/// Вызывать при запуске арены
		/// </summary>
		public void PlayArena()
        {
            if (WP == null)
                WP = GameObject.Find("GetPrefabs").GetComponent<WeaponPrefabs>();

			WP.Weapon = weaponArray[weaponItemNumber];
			if (skillItemNumberOne >= 0)
				ItemSkillPef.FirstSkill = (GameObject)Resources.Load(skillPrefix + skillItemNumberOne);
			if(skillItemNumberTwo >= 0)
				ItemSkillPef.SecondSkill = (GameObject)Resources.Load(skillPrefix + skillItemNumberTwo);
			if (skillItemNumberThree >= 0)
				ItemSkillPef.ThirdSkill = (GameObject)Resources.Load(skillPrefix + skillItemNumberThree);
			if (itemItemNumberOne >= 0)
				ItemSkillPef.FirstItem = itemArray[itemItemNumberOne];
			if (itemItemNumberTwo >= 0)
				ItemSkillPef.SecondItem = itemArray[itemItemNumberTwo];
			if (itemItemNumberThree >= 0)
				ItemSkillPef.ThirdItem = itemArray[itemItemNumberThree];
		}

		/// <summary>
        /// Инициализация
        /// </summary>
        private void Awake() // ____________start__________
		{
			PStats = GetComponent<PlayerStats>();
			WC = GetComponent<WeaponCraft>();
            AC = GetComponent<ArmorCraft>();

			weaponList = new List<Weapon>();
			skillList = new List<ISkill>();
			itemList = new List<IItem>();
			weaponArray = new GameObject[Resources.LoadAll("Prefabs/Weapons").Length];
			weaponStats = new int[3];
			skillItemNumberOne = -1;
			skillItemNumberTwo = -1;
			skillItemNumberThree = -1;
			itemItemNumberOne = -1;
			itemItemNumberTwo = -1;
			itemItemNumberThree = -1;

			Timing.RunCoroutine(WeaponCorutine());
			Timing.RunCoroutine(SkillCorutine());
			Timing.RunCoroutine(ItemsCorutine());
		}

		/// <summary>
        /// Обновление списков
        /// </summary>
        private void FixedUpdate()
		{
			ChekScroll();
		}

        /// <summary>
        /// Отключить подсветку у элементов брони
        /// </summary>
        /// <param name="numberItemType"></param>
        public void DisableListHighlightingInventory()
        {
            for (int i = 0; i < weaponInventoryElements.Length; i++)
                weaponInventoryElements[i].HighlightingControl(false, false);
        }

        /// <summary>
        /// Запускать для отображения элементов оружия в ленте
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> WeaponCorutine()
		{
            string str = PlayerPrefs.GetString("weaponArray");
            int[] elements = LibraryObjectsWorker.StringSplitter(str, '_');
            object[] tempObjects = new object[elements.Length];
            weaponInventoryElements = new IRepositoryObject[tempObjects.Length];
            for (int i = 0; i < elements.Length; i++)
                tempObjects[i] = Resources.Load(headPrefix + elements[i]);

            for (int i = 0; i < tempObjects.Length; i++)
			{
				weaponGamObj = (GameObject)tempObjects[i];
				GameObject item = Instantiate(itemWeapon);
				WeaponButton button = item.GetComponent<WeaponButton>();

                weaponArray[i] = weaponGamObj;
                weaponInventoryElements[i] = button;
                weaponList.Add(weaponGamObj.GetComponent<Weapon>());

                button.SetWeaponCraft(this);
                button.SetShop(shop);
                button.SetNumber(i);

                button.SetName(weaponList[i].HeadName);
                button.SetMoneyCost(weaponList[i].MoneyCost);
                button.SetLogo (weaponList[i].ItemImage);

				item.transform.SetParent(weaponRepository.transform, false);
			}

            scrollRectWeaponRepository =
                weaponRepository.transform.parent.GetComponent<ScrollRect>();
			yield return 0.02f;
		}

		/// <summary>
        /// Корутина для загрузки умений
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> SkillCorutine()
		{
            object[] tempObjects = Resources.LoadAll(skillPrefix);

            for (int i = 0; i < tempObjects.Length; i++)
			{
                GameObject skillGamObj = (GameObject)tempObjects[i];
				skillList.Add(skillGamObj.GetComponent<ISkill>());
				GameObject item = Instantiate(itemSkill);
				ItemOrSkillButton button = item.GetComponent<ItemOrSkillButton>();
				button.SetWeaponCraft(WC);
				button.SetNumber(i);
				button.NameSkill.text = skillList[i].SkillName;
				button.TutorialSkill.text = skillList[i].SkillTutorial;
				button.SetImage(skillList[i].SkillImage.sprite);
				item.transform.SetParent(skillsRepository.transform, false);
			}
			scrollRectSkillRepository =
				skillsRepository.transform.parent.GetComponent<ScrollRect>();
			yield return 0;
		}

		/// <summary>
        /// Корутина для загрудки предметов
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> ItemsCorutine()
		{
			int count = itemArray.Count;

			for (int i = 0; i < count; i++)
			{
                GameObject itemGamObj = itemArray[i];
				itemList.Add(itemGamObj.GetComponent<IItem>());
				GameObject item = Instantiate(itemItm);
				ItemOrSkillButton button = item.GetComponent<ItemOrSkillButton>();
				button.SetWeaponCraft(WC);
				button.SetNumber(i);
				button.NameSkill.text = itemList[i].ItemName;
				button.TutorialSkill.text = itemList[i].ItemTutorial;
				button.SetImage (itemList[i].ItemImage.sprite);
				item.transform.SetParent(itemRepository.transform, false);
			}
			scrollRectSkillRepository =
				skillsRepository.transform.parent.GetComponent<ScrollRect>();
			yield return 0.02f;
		}
	}
}
