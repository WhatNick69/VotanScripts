using MovementEffects;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VotanLibraries;
using ShopSystem;
using VotanUI;

namespace CraftSystem
{
    /// <summary>
    /// Крафт брони
    /// </summary>
    public class ArmorCraft
		: MonoBehaviour
	{
		#region Переменные
		[SerializeField]
		private GameObject cuirassRepository;
		[SerializeField]
		private GameObject helmetRepository;
		[SerializeField]
		private GameObject shieldRepository;
		[SerializeField]
		private GameObject itemArmor;

		[SerializeField]
		GameObject helmetWindow;
		[SerializeField]
		GameObject helmetUpadateButton; 
		[SerializeField]
		GameObject cuirassWindow;
		[SerializeField]
		GameObject cuirassUpadateButton; 
		[SerializeField]
		GameObject shieldWindow;
		[SerializeField]
		GameObject shieldUpadateButton;

		private GameObject cuirass;
		private GameObject helmet;
		private GameObject shield;

		string cuirassPrefix = "Prefabs/Armor/Cuirass/";
        string cuirassPostfix = "_C";
		string helmetPrefix = "Prefabs/Armor/Helmet/";
        string helmetPostfix = "_H";
		string shieldPrefix = "Prefabs/Armor/Shield/";
        string shieldPostfix = "_S";

        private List<PartArmoryInformation> cuirassList;
		private List<PartArmoryInformation> helmetList;
		private List<PartArmoryInformation> shieldList;

        private IRepositoryObject[] cuirassInventoryElements, 
            helmetInventoryElements,
            shieldInventoryElements;

        [SerializeField]
		private int cuirassItemNumber;
        private int cuirassItemNumberTemp = -1;
		[SerializeField]
		private int helmetItemNumber;
        private int helmetItemNumberTemp = -1;
        [SerializeField]
		private int shieldItemNumber;
        private int shieldItemNumberTemp = -1;

        ArmorPrefabs AP;
		PlayerStats PStats;
        Inventory inventory;
        Shop shop;

        ScrollRect scrollRectHelmetRepository;
        ScrollRect scrollRectShieldRepository;
        ScrollRect scrollRectCuirasseRepository;

		float intemNumbCuirass;
		float intemNumbHelmet;
		float intemNumbShield;
		float normPosCuirass;
		float normPosHelmet;
		float normPosShield;
		#endregion

		#region Свойства
		public int HelmetItemNumber
        {
            get
            {
                return helmetItemNumber;
            }

            set
            {
                helmetItemNumber = value;
				PStats.HelmetArmor = helmetList[value].ArmoryValue;
				PStats.HelmetWeight = helmetList[value].WeightArmory;
                PStats.NewStatsForHelmet();

                Inventory.SaveInventoryNumber(0, helmetItemNumber);
            }
        }

        public int CuirassItemNumber
        {
            get
            {
                return cuirassItemNumber;
            }

            set
            {
                cuirassItemNumber = value;
				PStats.CuirassArmor = cuirassList[value].ArmoryValue;
				PStats.CuirassWeight = cuirassList[value].WeightArmory;
                PStats.NewStatsForCuirass();

                Inventory.SaveInventoryNumber(2, helmetItemNumber);
            }
        }

        public int ShieldItemNumber
        {
            get
            {
                return shieldItemNumber;
            }

            set
            {
                shieldItemNumber = value;
				PStats.ShieldArmor = shieldList[value].ArmoryValue;
				PStats.ShieldWeight = shieldList[value].WeightArmory;
                PStats.NewStatsForShield();

                Inventory.SaveInventoryNumber(1, helmetItemNumber);
            }
        }

        public int CuirassItemNumberTemp
        {
            get
            {
                return cuirassItemNumberTemp;
            }

            set
            {
                cuirassItemNumberTemp = value;
            }
        }

        public int ShieldItemNumberTemp
        {
            get
            {
                return shieldItemNumberTemp;
            }

            set
            {
                shieldItemNumberTemp = value;
            }
        }

        public int HelmetItemNumberTemp
        {
            get
            {
                return helmetItemNumberTemp;
            }

            set
            {
                helmetItemNumberTemp = value;
            }
        }

        public GameObject GetCuirassPrafab()
		{
			return cuirass;
		}

		public GameObject GetHeadPrafab()
		{
			return helmet;
		}

		public GameObject GetGemPrafab()
		{
			return shield;
		}
        #endregion

        /// <summary>
        /// ======================= Инициализация =======================
        /// </summary>
        private void Start()
        {
            PStats = GetComponent<PlayerStats>();
            inventory = GetComponent<Inventory>();
            shop = GetComponent<Shop>();

            cuirassList = new List<PartArmoryInformation>();
            helmetList = new List<PartArmoryInformation>();
            shieldList = new List<PartArmoryInformation>();

            LoadArmorInventory();

            Timing.RunCoroutine(ShieldCorutine());
            Timing.RunCoroutine(HelmetCorutine());
            Timing.RunCoroutine(CuirassCorutine());
        }

        /// <summary>
        /// Загрузить экипированный инвентарь брони
        /// </summary>
        private void LoadArmorInventory()
        {
            int[] armorNumbers = Inventory.LoadInventoryNumbers();
            helmetItemNumber = armorNumbers[0];
            shieldItemNumber = armorNumbers[1];
            cuirassItemNumber = armorNumbers[2];
        }

        /// <summary>
        /// Запускать при старте игры, обязательно!
        /// </summary>
        public void PlayArenaArmor()
        {
            if (AP == null)
                AP = GameObject.Find("GetPrefabs").GetComponent<ArmorPrefabs>();

            AP.Cuirass = cuirassList[cuirassItemNumber].gameObject;
            AP.Helmet = helmetList[helmetItemNumber].gameObject;
            AP.Shield = shieldList[shieldItemNumber].gameObject;
        }

        /// <summary>
        /// Экипировать оружие
        /// </summary>
        public void EquipItem()
        {
            if (cuirassItemNumberTemp != -1)
            {
                CuirassItemNumber = cuirassItemNumberTemp;
                cuirassItemNumberTemp = -1;
                shieldItemNumberTemp = -1;
                helmetItemNumberTemp = -1;
                MenuSoundManager.PlaySoundStatic(1);
            }
            else if (helmetItemNumberTemp != -1)
            {
                HelmetItemNumber = helmetItemNumberTemp;
                cuirassItemNumberTemp = -1;
                shieldItemNumberTemp = -1;
                helmetItemNumberTemp = -1;
                MenuSoundManager.PlaySoundStatic(1);
            }
            else if (shieldItemNumberTemp != -1)
            {
                ShieldItemNumber = shieldItemNumberTemp;
                cuirassItemNumberTemp = -1;
                shieldItemNumberTemp = -1;
                helmetItemNumberTemp = -1;
                MenuSoundManager.PlaySoundStatic(1);
            }
        }

        /// <summary>
        /// Отключить подсветку у элементов брони
        /// </summary>
        /// <param name="numberItemType"></param>
        public void DisableListHighlightingInventory(int numberItemType)
        {
            switch (numberItemType)
            {
                case 0: //c
                    for (int i = 0; i < cuirassInventoryElements.Length; i++)
                            cuirassInventoryElements[i].HighlightingControl(false,false);
                    break;

                case 1: //h
                    for (int i = 0; i < helmetInventoryElements.Length; i++)
                            helmetInventoryElements[i].HighlightingControl(false,false);
                    break;

                case 2: //s
                    for (int i = 0; i < shieldInventoryElements.Length; i++)
                            shieldInventoryElements[i].HighlightingControl(false,false);
                    break;
            }
        }


        /// <summary>
        /// Проверка предметов в инвентаре кирасс
        /// </summary>
        public void CheckCuirassScroll()
        {
            if (normPosCuirass != scrollRectCuirasseRepository.horizontalNormalizedPosition)
            {
                if (cuirassItemNumber !=
                    Mathf.Round(scrollRectCuirasseRepository.horizontalNormalizedPosition * (cuirassList.Count - 1)))
                {
                    intemNumbCuirass = Mathf.Round(scrollRectCuirasseRepository.horizontalNormalizedPosition 
                        * (cuirassList.Count - 1));
                }
                else
                {
                    intemNumbCuirass = cuirassItemNumber;
                }

                if (intemNumbCuirass >= 0 && intemNumbCuirass < cuirassList.Count)
                {
                    PStats.NewCuirassArmor = cuirassList[(int)intemNumbCuirass].ArmoryValue;
                    PStats.NewCuirassWeight = cuirassList[(int)intemNumbCuirass].WeightArmory;
                }
            }
            PStats.NewStatsForCuirass();
            normPosCuirass = scrollRectCuirasseRepository.horizontalNormalizedPosition;
        }

        /// <summary>
        /// Проверка предметов в инвентаре щитов
        /// </summary>
        public void CheckShieldScroll()
        {
            if (normPosShield != scrollRectShieldRepository.horizontalNormalizedPosition)
            {
                if (shieldItemNumber != Mathf.Round(scrollRectShieldRepository.horizontalNormalizedPosition
                    * (shieldList.Count - 1)))
                {
                    intemNumbShield = Mathf.Round(scrollRectShieldRepository.horizontalNormalizedPosition
                        * (shieldList.Count - 1));
                }
                else
                {
                    intemNumbShield = shieldItemNumber;
                }

                if (intemNumbShield >= 0 && intemNumbShield < shieldList.Count)
                {
                    PStats.NewShieldArmor = shieldList[(int)intemNumbShield].ArmoryValue;
                    PStats.NewShieldWeight = shieldList[(int)intemNumbShield].WeightArmory;
                }
            }
            PStats.NewStatsForShield();
            normPosShield = scrollRectShieldRepository.horizontalNormalizedPosition;
        }

        /// <summary>
        /// Проверка предметов в инвентаре шлемов
        /// </summary>
        public void CheckHelmetScroll()
        {
            if (normPosHelmet != scrollRectHelmetRepository.horizontalNormalizedPosition)
            {
                if (helmetItemNumber != Mathf.Round(scrollRectHelmetRepository.horizontalNormalizedPosition
                    * (helmetList.Count - 1)))
                {
                    intemNumbHelmet = Mathf.Round(scrollRectHelmetRepository.horizontalNormalizedPosition
                        * (helmetList.Count - 1));
                }
                else
                {
                    intemNumbHelmet = HelmetItemNumber;
                }

                if (intemNumbHelmet >= 0 && intemNumbHelmet < helmetList.Count)
                {
                    PStats.NewHelmetArmor = helmetList[(int)intemNumbHelmet].ArmoryValue;
                    PStats.NewHelmetWeight = helmetList[(int)intemNumbHelmet].WeightArmory;
                }
            }
            PStats.NewStatsForHelmet();
            normPosHelmet = scrollRectHelmetRepository.horizontalNormalizedPosition;
        }

        #region Работа с окнами
        /// <summary>
        /// Вызывает окно с лнтой шлемов
        /// </summary>
        public void HelmetWindow()
        {
            shieldWindow.SetActive(false);
            cuirassWindow.SetActive(false);
            helmetWindow.SetActive(true);

            inventory.WeaponCraftComponent.CloseAllWindows();
            inventory.ItemsSkillsCraft.CloseAllWindows();

            PStats.ArmorPage();
            scrollRectHelmetRepository.horizontalNormalizedPosition = 0;
        }

        /// <summary>
        /// Вызывает окно с лнтой кирас
        /// </summary>
        public void CuirassWindow()
        {
            shieldWindow.SetActive(false);
            helmetWindow.SetActive(false);
            cuirassWindow.SetActive(true);

            inventory.WeaponCraftComponent.CloseAllWindows();
            inventory.ItemsSkillsCraft.CloseAllWindows();

            PStats.ArmorPage();
            scrollRectCuirasseRepository.horizontalNormalizedPosition = 0;
        }

        /// <summary>
        /// Вызывает окно с лнтой щитов
        /// </summary>
        public void ShieldWindow()
        {
            helmetWindow.SetActive(false);
            cuirassWindow.SetActive(false);
            shieldWindow.SetActive(true);

            inventory.WeaponCraftComponent.CloseAllWindows();
            inventory.ItemsSkillsCraft.CloseAllWindows();

            PStats.ArmorPage();
            scrollRectShieldRepository.horizontalNormalizedPosition = 0;
        }

        /// <summary>
        /// Закрыть все окна (шлем, кирасу, щит)
        /// </summary>
        public void CloseAllWindows()
        {
            helmetWindow.SetActive(false);
            cuirassWindow.SetActive(false);
            shieldWindow.SetActive(false);
        }

        /// <summary>
        /// Обновление окон
        /// </summary>
        public void UpdateWindow()
        {
            if (cuirassWindow.activeSelf)
            {
                cuirassUpadateButton.SetActive(true);
                helmetUpadateButton.SetActive(false);
                shieldUpadateButton.SetActive(false);
            }
            else if (helmetWindow.activeSelf)
            {
                helmetUpadateButton.SetActive(true);
                shieldUpadateButton.SetActive(false);
                cuirassUpadateButton.SetActive(false);
            }
            else
            {
                shieldUpadateButton.SetActive(true);
                cuirassUpadateButton.SetActive(false);
                helmetUpadateButton.SetActive(false);
            }
        }

        /// <summary>
        /// Обновляет окно шлемов. Вызывать при покупке
        /// </summary>
        public void RestartHelmetWindow()
        {
            for (int i = 0; i < helmetRepository.transform.childCount; i++)
                Destroy(helmetRepository.transform.GetChild(i).gameObject);

            helmetList = new List<PartArmoryInformation>();
            Timing.RunCoroutine(HelmetCorutine());
        }

        /// <summary>
        /// Обновляет окно кирас. Вызывать при покупке
        /// </summary>
        public void RestartCuirassWindow()
        {
            for (int i = 0; i < cuirassRepository.transform.childCount; i++)
                Destroy(cuirassRepository.transform.GetChild(i).gameObject);

            cuirassList = new List<PartArmoryInformation>();
            Timing.RunCoroutine(CuirassCorutine());
        }

        /// <summary>
        /// Обновляет окно щитов. Вызывать при покупке
        /// </summary>
        public void RestartShieldWindow()
        {
            for (int i = 0; i < shieldRepository.transform.childCount; i++)
                Destroy(shieldRepository.transform.GetChild(i).gameObject);

            shieldList = new List<PartArmoryInformation>();
            Timing.RunCoroutine(ShieldCorutine());
        }
        #endregion

        #region Корутины
        /// <summary>
        /// Создает и настраивает кнопу в ленте - ШИТ
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> ShieldCorutine()
		{
            string str = CheckEmptyShieldLocalSave();
            int[] elements = LibraryObjectsWorker.StringSplitter(str, '_');
            object[] tempObjects = new object[elements.Length];
            shieldInventoryElements = new IRepositoryObject[tempObjects.Length];

            for (int i = 0; i < elements.Length; i++)
                tempObjects[i] = Resources.Load(shieldPrefix + elements[i] + shieldPostfix);

            GameObject gemGgamObj;
            GameObject item;
            ArmorButton button;
            for (int i = 0; i < tempObjects.Length; i++)
			{
				gemGgamObj = (GameObject)tempObjects[i];
				item = Instantiate(itemArmor);
				button = item.GetComponent<ArmorButton>();

                shieldList.Add(gemGgamObj.GetComponent<PartArmoryInformation>());
                shieldInventoryElements[i] = button;

                button.SetShop(shop);
                button.SetArmorCraft(this);
				button.SetNumber(i);
                button.ArmoryClass = shieldList[i].ArmoryType;
				button.Weight = shieldList[i].WeightArmory.ToString();

				button.SetName(shieldList[i].ArmoryName);
                button.SetMoneyCost(shieldList[i].MoneyCost / 4);
                button.SetLogo (shieldList[i].ImageArm);

				item.transform.SetParent(shieldRepository.transform, false);
			}
            scrollRectShieldRepository = 
                shieldRepository.transform.parent.GetComponent<ScrollRect>();

            shieldInventoryElements[shieldItemNumber].HighlightingControl(true, false);
            PStats.ShieldArmor = shieldList[shieldItemNumber].ArmoryValue;
            PStats.ShieldWeight = shieldList[shieldItemNumber].WeightArmory;
            yield return 0;
		}

        /// <summary>
        /// Проверить сохранение щитов на пустоту
        /// </summary>
        /// <returns></returns>
        private string CheckEmptyShieldLocalSave()
        {
            string str = PlayerPrefs.GetString("shieldArray");
            if (str == null || str == "")
            {
                str = "0_";
                PlayerPrefs.SetString("shieldArray", str);
            }
            return str;
        }

		/// <summary>
		/// Создает и настраивает кнопу в ленте - КИРАСА
		/// </summary>
		/// <returns></returns>
		private IEnumerator<float> CuirassCorutine()
		{
            string str = CheckEmptyCuirassLocalSave();
            int[] elements = LibraryObjectsWorker.StringSplitter(str, '_');
            object[] tempObjects = new object[elements.Length];
            cuirassInventoryElements = new IRepositoryObject[tempObjects.Length];

            for (int i = 0; i < elements.Length; i++)
                tempObjects[i] = Resources.Load(cuirassPrefix + elements[i] + cuirassPostfix);

            GameObject gemGgamObj;
            GameObject item;
            ArmorButton button;
            for (int i = 0; i < tempObjects.Length; i++)
			{
				gemGgamObj = (GameObject)tempObjects[i];
				item = Instantiate(itemArmor);
				button = item.GetComponent<ArmorButton>();

                cuirassInventoryElements[i] = button;
                cuirassList.Add(gemGgamObj.GetComponent<PartArmoryInformation>());

                button.SetShop(shop);
                button.SetArmorCraft(this);
				button.SetNumber(i);
                button.ArmoryClass = cuirassList[i].ArmoryType;
				button.Weight = cuirassList[i].WeightArmory.ToString();

				button.SetName(cuirassList[i].ArmoryName);
                button.SetMoneyCost(cuirassList[i].MoneyCost / 4);
                button.SetLogo (cuirassList[i].ImageArm);

				item.transform.SetParent(cuirassRepository.transform, false);
			}
            scrollRectCuirasseRepository =
                cuirassRepository.transform.parent.GetComponent<ScrollRect>();

            cuirassInventoryElements[cuirassItemNumber].HighlightingControl(true, false);
            PStats.CuirassArmor = cuirassList[cuirassItemNumber].ArmoryValue;
            PStats.CuirassWeight = cuirassList[cuirassItemNumber].WeightArmory;
            yield return 0;
		}

        /// <summary>
        /// Проверить сохранение кирасс на пустоту
        /// </summary>
        /// <returns></returns>
        private string CheckEmptyCuirassLocalSave()
        {
            string str = PlayerPrefs.GetString("cuirassArray");
            if (str == null || str == "")
            {
                str = "0_";
                PlayerPrefs.SetString("cuirassArray", str);
            }
            return str;
        }

        /// <summary>
        /// Создает и настраивает кнопу в ленте - ШЛЕМ
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> HelmetCorutine()
		{
            string str = CheckEmptyHelmetLocalSave();
            int[] elements = LibraryObjectsWorker.StringSplitter(str, '_');
            object[] tempObjects = new object[elements.Length];
            helmetInventoryElements = new IRepositoryObject[tempObjects.Length];

            for (int i = 0; i < elements.Length; i++)
                tempObjects[i] = Resources.Load(helmetPrefix + elements[i] + helmetPostfix);

            GameObject gemGgamObj;
            GameObject item;
            ArmorButton button;
            for (int i = 0; i < tempObjects.Length; i++)
			{
				gemGgamObj = (GameObject)tempObjects[i];
				item = Instantiate(itemArmor);
				button = item.GetComponent<ArmorButton>();

                helmetInventoryElements[i] = button;
                helmetList.Add(gemGgamObj.GetComponent<PartArmoryInformation>());

                button.SetShop(shop);
                button.SetArmorCraft(this);
				button.SetNumber(i);
                button.ArmoryClass = helmetList[i].ArmoryType;
				button.Weight = helmetList[i].WeightArmory.ToString();

                button.SetName(helmetList[i].ArmoryName);
				button.SetMoneyCost(helmetList[i].MoneyCost / 4);
				button.SetLogo (helmetList[i].ImageArm);

				item.transform.SetParent(helmetRepository.transform, false);
			}
            scrollRectHelmetRepository =
                helmetRepository.transform.parent.GetComponent<ScrollRect>();

            helmetInventoryElements[helmetItemNumber].HighlightingControl(true, false);
            PStats.HelmetArmor = helmetList[helmetItemNumber].ArmoryValue;
            PStats.HelmetWeight = helmetList[helmetItemNumber].WeightArmory;
            yield return 0;
		}

        /// <summary>
        /// Проверить сохранение шлемов на пустоту
        /// </summary>
        /// <returns></returns>
        private string CheckEmptyHelmetLocalSave()
        {
            string str = PlayerPrefs.GetString("helmetArray");
            if (str == null || str == "")
            {
                str = "0_";
                PlayerPrefs.SetString("helmetArray", str);
            }
            return str;
        }
        #endregion
    }
}
