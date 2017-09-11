using UnityEngine;
using System.Collections.Generic;
using MovementEffects;
using VotanUI;
using UnityEngine.UI;
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

		private GameObject weapon;
		private GameObject weaponGamObj;

		string weaponPrefix = "Prefabs/Weapons/";
        string weaponPostfix = "_Weapon";

		private List<Weapon> weaponList;
        private IRepositoryObject[] weaponInventoryElements;

		public int weaponItemNumber;
        private int weaponItemNumberTemp = -1;

		[SerializeField]
		WeaponPrefabs WP;
        [SerializeField]
        Shop shop;
        Inventory inventory;
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
		}

		/// <summary>
		/// Вызывать для открытия окна с оружием
		/// </summary>
		public void WeaponWindow()
		{
			weaponWindow.SetActive(true);

            inventory.ArmorCraftComponent.CloseAllWindows();
            inventory.ItemsSkillsCraft.CloseAllWindows();

            PStats.WeaponPage();
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
            PStats.GemPower = weaponList[x].GemPower;
            PStats.InventoryImageGem.color = Shop.GetColorFromGemType(weaponList[x].DamageTypeGem);

            PStats.NewStatsForWeapon();

            Inventory.SaveInventoryNumber(3, weaponItemNumber);
        }

        /// <summary>
        /// Экипировать оружие
        /// </summary>
        public void EquipWeapon()
        {
            if (weaponItemNumberTemp != -1)
            {
                SetWeaponItemNumber(weaponItemNumberTemp);
                weaponItemNumberTemp = -1;
            }
        }

		/// <summary>
		/// Обновляет окно оружия. Вызывать при покупке
		/// </summary>
		public void RestartWeaponWindow()
		{
            for (int i = 0; i < weaponRepository.transform.childCount; i++)
                Destroy(weaponRepository.transform.GetChild(i).gameObject);

            weaponList = new List<Weapon>();
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
		public void CheckWeaponScroll()
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

                if (intemNumbWeapon >= 0 && intemNumbWeapon < weaponList.Count)
                {
                    PStats.NewHeadDamage = weaponList[(int)intemNumbWeapon].DamageBase;
                    PStats.NewCritChance = weaponList[(int)intemNumbWeapon].CriticalChance;
                    PStats.NewGemPower = weaponList[(int)intemNumbWeapon].GemPower;
                    PStats.InventoryImageGem.color = Shop.GetColorFromGemType(weaponList[(int)intemNumbWeapon].DamageTypeGem);
                }
			}
			normPosWeapon = scrollRectWeaponRepository.horizontalNormalizedPosition;
			PStats.NewStatsForWeapon();
		}
		#endregion

		/// <summary>
		/// Вызывать при запуске арены
		/// </summary>
		public void PlayArenaWeapon()
        {
            if (WP == null)
                WP = GameObject.Find("GetPrefabs").GetComponent<WeaponPrefabs>();

			WP.Weapon = weaponList[weaponItemNumber].gameObject;
		}

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Start() // ____________start__________
        {
            PStats = GetComponent<PlayerStats>();
            inventory = GetComponent<Inventory>();

            weaponList = new List<Weapon>();

            Timing.RunCoroutine(WeaponCorutine());
        }

        /// <summary>
        /// Загрузить экипированный инвентарь оружия
        /// </summary>
        /// <returns></returns>
        private bool LoadArmorInventory()
        {
            int[] armorNumbers = Inventory.LoadInventoryNumbers();
            if (armorNumbers.Length == 0)
            {
                return false;
            }
            else
            {
                weaponItemNumber = armorNumbers[3];
                weaponInventoryElements[weaponItemNumber].HighlightingControl(true, false);
                PStats.HeadDamage = weaponList[weaponItemNumber].DamageBase;
                PStats.CritChance = weaponList[weaponItemNumber].CriticalChance;
                PStats.GemPower = weaponList[weaponItemNumber].GemPower;
                PStats.InventoryImageGem.color = Shop.GetColorFromGemType(weaponList[weaponItemNumber].DamageTypeGem);
                return true;
            }
        }

        /// <summary>
        /// Отключить подсветку у элементов оружия
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

            GameObject item;
            WeaponButton button;
            for (int i = 0; i < elements.Length; i++)
			{
				weaponGamObj = (GameObject)tempObjects[i];
				item = Instantiate(itemWeapon);
				button = item.GetComponent<WeaponButton>();

                weaponInventoryElements[i] = button;
                weaponList.Add(weaponGamObj.GetComponent<Weapon>());

                button.SetWeaponCraft(this);
                button.SetShop(shop);
                button.SetNumber(i);

                button.SetName(weaponList[i].HeadName);
                button.SetMoneyCost(weaponList[i].MoneyCost /4);
                button.SetLogo (weaponList[i].ItemImage);

				item.transform.SetParent(weaponRepository.transform, false);
			}

            scrollRectWeaponRepository =
                weaponRepository.transform.parent.GetComponent<ScrollRect>();

            while (!LoadArmorInventory())
            {
                yield return Timing.WaitForSeconds(0.5f);
            }

            yield return Timing.WaitForSeconds(0);
        }

        /// <summary>
        /// Проверить локальное сохранение на пустоту
        /// </summary>
        /// <returns></returns>
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
