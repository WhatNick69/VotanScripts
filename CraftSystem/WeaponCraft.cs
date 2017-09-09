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
		[SerializeField, Tooltip("кнопка оружия")]
        private GameObject itemWeapon;
        [SerializeField]
		GameObject weaponWindow;
		[SerializeField]
		GameObject weaponUpadateButton; // Отображать вместе с weaponWindow

		private GameObject weapon;
		private GameObject weaponGamObj;

		string weaponPrefix = "Prefabs/Weapons/";
        string weaponPostfix = "_Weapon";

		private List<Weapon> weaponList;
        private IRepositoryObject[] weaponInventoryElements;

        private GameObject[] weaponArray;
		private int[] weaponStats;

		public int weaponItemNumber;
        private int weaponItemNumberTemp = -1;

		[SerializeField]
		WeaponPrefabs WP;
        [SerializeField]
        Shop shop;
        ArmorCraft AC;
		PlayerStats PStats;

        ScrollRect scrollRectWeaponRepository;

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

			weaponWindow.SetActive(false);
			weaponUpadateButton.SetActive(false);
		}

		/// <summary>
		/// Вызывать для открытия окна с оружием
		/// </summary>
		public void WeaponWindow()
		{
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
		private void CheckScroll()
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

                if (intemNumbWeapon >= 0)
                {
                    PStats.NewHeadDamage = weaponList[(int)intemNumbWeapon].DamageBase;
                    PStats.NewCritChance = weaponList[(int)intemNumbWeapon].CriticalChance;
                }
			}
			normPosWeapon = scrollRectWeaponRepository.horizontalNormalizedPosition;
			PStats.NewStats();
		}
		#endregion

		/// <summary>
		/// Вызывать при запуске арены
		/// </summary>
		public void PlayArenaWeapon()
        {
            if (WP == null)
                WP = GameObject.Find("GetPrefabs").GetComponent<WeaponPrefabs>();

			WP.Weapon = weaponArray[weaponItemNumber];
		}

		/// <summary>
        /// Инициализация
        /// </summary>
        private void Awake() // ____________start__________
		{
			PStats = GetComponent<PlayerStats>();
            AC = GetComponent<ArmorCraft>();

			weaponList = new List<Weapon>();

			weaponArray = new GameObject[Resources.LoadAll("Prefabs/Weapons").Length];
			weaponStats = new int[3];

			Timing.RunCoroutine(WeaponCorutine());
		}

		/// <summary>
        /// Обновление списков
        /// </summary>
        private void FixedUpdate()
		{
			CheckScroll();
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
            string str = CheckEmptyWeaponLocalSave();
            int[] elements = LibraryObjectsWorker.StringSplitter(str, '_');
            object[] tempObjects = new object[elements.Length];
            weaponInventoryElements = new IRepositoryObject[tempObjects.Length];
            for (int i = 0; i < elements.Length; i++)
                tempObjects[i] = Resources.Load(weaponPrefix + elements[i] + weaponPostfix);

            for (int i = 0; i < elements.Length; i++)
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
            yield return Timing.WaitForSeconds(0);
        }

        private string CheckEmptyWeaponLocalSave()
        {
            string str = PlayerPrefs.GetString("weaponArray");
            if (str == null || str == "")
            {
                str = "0_";
                PlayerPrefs.SetString("weaponArray",str);
            }
            return str;
        }
    }
}
