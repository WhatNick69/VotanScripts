using CraftSystem;
using MovementEffects;
using PlayerBehaviour;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VotanLibraries;

namespace ShopSystem
{
    /// <summary>
    /// Крафт брони
    /// </summary>
    public class Shop
		: MonoBehaviour
	{
		#region Переменные
		[SerializeField]
		GameObject inventory;
		[SerializeField, Tooltip("Оружие")]
		private GameObject weaponWind;
		[SerializeField, Tooltip("Броня")]
		private GameObject armorWind;
        [SerializeField, Tooltip("Панель кнопок магазина с броней")]
        private GameObject buttonsPanelStoreArmory;
        [SerializeField, Tooltip("Панель кнопок магазина с оружием")]
        private GameObject buttonsPanelStoreWeapons;
        [SerializeField, Tooltip("Общая для магазина панель кнопок")]
        private GameObject buttonsPanelStoreCommon;
        [SerializeField]
		private GameObject cuirassRepository;
		[SerializeField]
		private GameObject helmetRepository;
		[SerializeField]
		private GameObject shieldRepository;
		[SerializeField]
		private GameObject weaponRepository;

		[SerializeField, Tooltip("элемент брони")]
		private GameObject itemArmor;
		[SerializeField, Tooltip("элемент оружия")]
		private GameObject itemWeapon;

		[SerializeField]
		GameObject helmetWindow;
		[SerializeField]
		GameObject cuirassWindow;
		[SerializeField]
		GameObject shieldWindow;
		[SerializeField]
		GameObject weaponWindow;

		private GameObject cuirass;
		private GameObject helmet;
		private GameObject shield;
		private GameObject weapon;

		string cuirassPrefix = "Prefabs/Armor/Cuirass/";
        string cuirassPostfix = "_C";
        string helmetPrefix = "Prefabs/Armor/Helmet/";
        string helmetPostfix = "_H";
        string shieldPrefix = "Prefabs/Armor/Shield/";
        string shieldPostfix = "_S";
        string weaponPrefix = "Prefabs/Weapons/";
        string weaponPostfix = "Weapon_";

        private List<PartArmoryInformation> cuirassList;
		private List<PartArmoryInformation> helmetList;
		private List<PartArmoryInformation> shieldList;
        private List<Weapon> weaponList;

        private IRepositoryObject[] cuirassElements, helmetElements, 
            shieldElements, weaponElements;

		private int cuirassItemNumber;
		private int helmetItemNumber;
		private int shieldItemNumber;
		private int weaponItemNumber;

		ArmorPrefabs armorPrefabs;
		Shop shopComponent;
		[SerializeField]
		WeaponCraft weaponCraft;
		[SerializeField]
		ArmorCraft armorCraft;
        UserResources userResources;

        [SerializeField]
        private Text textShopName, textShopWeight, textShopArmor, 
            textShopDamage, textShopCritChance, textShopGemPower;
        private GameObject shopNameObj, shopWeightObj, shopArmorObj,
            shopDamageObj, shopCritChanceObj, shopGemPowerObj;
        [SerializeField]
        private Image imageShopGem;
        [SerializeField]
        private Color colorFrozen, colorFire, colorElectric, colorPowerful;

        ScrollRect scrollRectHelmetRepository;
		ScrollRect scrollRectShieldRepository;
		ScrollRect scrollRectCuirasseRepository;
		ScrollRect scrollRectWeaponRepository;
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
			}
		}

		public int WeaponItemNumber
		{
			get
			{
				return weaponItemNumber;
			}

			set
			{
				weaponItemNumber = value;
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
        /// ========================= Инициализация =========================
        /// </summary>
        private void Start() // ____________start__________
        {
            shopComponent = GetComponent<Shop>();
            userResources = GameObject.Find("GetPrefabs").GetComponent<UserResources>();
            userResources.RefreshResources();

            cuirassList = new List<PartArmoryInformation>();
            helmetList = new List<PartArmoryInformation>();
            shieldList = new List<PartArmoryInformation>();
            weaponList = new List<Weapon>();

            shopNameObj = textShopName.transform.parent.gameObject;
            shopWeightObj = textShopWeight.transform.parent.gameObject;
            shopArmorObj = textShopArmor.transform.parent.gameObject;
            shopDamageObj = textShopDamage.transform.parent.gameObject;
            shopCritChanceObj = textShopCritChance.transform.parent.gameObject;
            shopGemPowerObj = textShopGemPower.transform.parent.gameObject;

            Timing.RunCoroutine(ShieldCorutine());
            Timing.RunCoroutine(HelmetCorutine());
            Timing.RunCoroutine(CuirassCorutine());
            Timing.RunCoroutine(WeaponCorutine());
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

            UnshowAllUIElementsInShop();
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

            UnshowAllUIElementsInShop();
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

            UnshowAllUIElementsInShop();
            scrollRectShieldRepository.horizontalNormalizedPosition = 0;
		}

        /// <summary>
        /// Общее окно магазина
        /// </summary>
        public void CommonStoreWindow()
        {
            armorWind.SetActive(false);
            weaponWind.SetActive(false);
            buttonsPanelStoreArmory.SetActive(false);
            buttonsPanelStoreWeapons.SetActive(false);
            buttonsPanelStoreCommon.SetActive(true);

            UnshowAllUIElementsInShop();
        }

		/// <summary>
		/// Открыть окно с оружием
		/// </summary>
		public void WeaponWindow()
		{
            buttonsPanelStoreWeapons.SetActive(true);
            buttonsPanelStoreCommon.SetActive(false);
            armorWind.SetActive(false);
			weaponWind.SetActive(true);

            UnshowAllUIElementsInShop();
        }

		/// <summary>
		/// Открыть окно с бронёй
		/// </summary>
		public void ArmorWindow()
		{
            buttonsPanelStoreArmory.SetActive(true);
            buttonsPanelStoreCommon.SetActive(false);
            weaponWind.SetActive(false);
			armorWind.SetActive(true);

            UnshowAllUIElementsInShop();
        }
        #endregion

        #region Покупка вещей
        /// <summary>
        /// Отключить подсветку у всех элементов листа
        /// </summary>
        /// <param name="numberItemType"></param>
        public void DisableListHighlightingShop(int numberItemType,bool isDisableAll=false)
        {
            if (isDisableAll)
            {
                switch (numberItemType)
                {
                    case 0: //c
                        for (int i = 0; i < cuirassElements.Length; i++)
                            cuirassElements[i].HighlightingControl(false);
                        break;
                    case 1: //h
                        for (int i = 0; i < helmetElements.Length; i++)
                            helmetElements[i].HighlightingControl(false);
                        break;
                    case 2: //s
                        for (int i = 0; i < shieldElements.Length; i++)
                            shieldElements[i].HighlightingControl(false);
                        break;
                    case 3: //w
                        for (int i = 0; i < weaponElements.Length; i++)
                            weaponElements[i].HighlightingControl(false);
                        break;
                }
            }
            else
            {
                switch (numberItemType)
                {
                    case 0: //c
                        for (int i = 0; i < cuirassElements.Length; i++)
                            if (i != cuirassItemNumber) cuirassElements[i].HighlightingControl(false);
                        break;
                    case 1: //h
                        for (int i = 0; i < helmetElements.Length; i++)
                            if (i != helmetItemNumber) helmetElements[i].HighlightingControl(false);
                        break;
                    case 2: //s
                        for (int i = 0; i < shieldElements.Length; i++)
                            if (i != shieldItemNumber) shieldElements[i].HighlightingControl(false);
                        break;
                    case 3: //w
                        for (int i = 0; i < weaponElements.Length; i++)
                            if (i != weaponItemNumber) weaponElements[i].HighlightingControl(false);
                        break;
                }
            }
        }

        /// <summary>
        /// Отобразить параметры предмета в магазине
        /// </summary>
        /// <param name="numberItemType"></param>
        public void ShowItemParameters(int numberItemType)
        {
            switch (numberItemType)
            {
                case 0: //c
                    textShopName.text = cuirassList[cuirassItemNumber].ArmoryName;
                    textShopArmor.text = cuirassList[cuirassItemNumber].ArmoryValue.ToString();
                    textShopWeight.text = cuirassList[cuirassItemNumber].WeightArmory.ToString();
                    ShowNeedUIElements(0);
                    break;

                case 1: //h
                    textShopName.text = helmetList[helmetItemNumber].ArmoryName;
                    textShopArmor.text = helmetList[helmetItemNumber].ArmoryValue.ToString();
                    textShopWeight.text = helmetList[helmetItemNumber].WeightArmory.ToString();
                    ShowNeedUIElements(1);
                    break;

                case 2: //s
                    textShopName.text = shieldList[shieldItemNumber].ArmoryName;
                    textShopArmor.text = shieldList[shieldItemNumber].ArmoryValue.ToString();
                    textShopWeight.text = shieldList[shieldItemNumber].WeightArmory.ToString();
                    ShowNeedUIElements(2);
                    break;

                case 3: //w
                    textShopName.text = weaponList[weaponItemNumber].HeadName;
                    textShopDamage.text = weaponList[weaponItemNumber].DamageBase.ToString();
                    textShopCritChance.text = weaponList[weaponItemNumber].CriticalChance.ToString();
                    textShopGemPower.text = weaponList[weaponItemNumber].GemPower.ToString();
                    ShowNeedUIElements(3);
                    SetColorOfShopImageGem();
                    break;
            }
        }

        /// <summary>
        /// Скрыть все элементы UI в магазине
        /// </summary>
        public void UnshowAllUIElementsInShop()
        {
            ShowNeedUIElements(0, false);
        }

        /// <summary>
        /// Отображать необходимые параметры предмета 
        /// (оружию - ущерб, броне - прочность)
        /// </summary>
        public void ShowNeedUIElements(int numberItemType,bool isActive=true)
        {
            if (isActive)
            {
                shopNameObj.SetActive(true);
                switch (numberItemType)
                {
                    case 0: //c
                        textShopName.enabled = true;
                        textShopArmor.enabled = true;
                        textShopWeight.enabled = true;

                        shopWeightObj.SetActive(true);
                        shopArmorObj.SetActive(true);
                        shopDamageObj.SetActive(false);
                        shopCritChanceObj.SetActive(false);
                        shopGemPowerObj.SetActive(false);

                        textShopDamage.enabled = false;
                        textShopCritChance.enabled = false;
                        textShopGemPower.enabled = false;
                        break;
                    case 1: //h
                        textShopName.enabled = true;
                        textShopArmor.enabled = true;
                        textShopWeight.enabled = true;

                        shopWeightObj.SetActive(true);
                        shopArmorObj.SetActive(true);
                        shopDamageObj.SetActive(false);
                        shopCritChanceObj.SetActive(false);
                        shopGemPowerObj.SetActive(false);

                        textShopDamage.enabled = false;
                        textShopCritChance.enabled = false;
                        textShopGemPower.enabled = false;
                        break;
                    case 2: //s
                        textShopName.enabled = true;
                        textShopArmor.enabled = true;
                        textShopWeight.enabled = true;

                        shopWeightObj.SetActive(true);
                        shopArmorObj.SetActive(true);
                        shopDamageObj.SetActive(false);
                        shopCritChanceObj.SetActive(false);
                        shopGemPowerObj.SetActive(false);

                        textShopDamage.enabled = false;
                        textShopCritChance.enabled = false;
                        textShopGemPower.enabled = false;
                        break;
                    case 3: //w
                        textShopArmor.enabled = false;
                        textShopWeight.enabled = false;

                        shopWeightObj.SetActive(false);
                        shopArmorObj.SetActive(false);
                        shopDamageObj.SetActive(true);
                        shopCritChanceObj.SetActive(true);
                        shopGemPowerObj.SetActive(true);

                        textShopDamage.enabled = true;
                        textShopCritChance.enabled = true;
                        textShopGemPower.enabled = true;
                        break;
                }
            }
            else
            {
                shopNameObj.SetActive(false);
                shopWeightObj.SetActive(false);
                shopArmorObj.SetActive(false);
                shopDamageObj.SetActive(false);
                shopCritChanceObj.SetActive(false);
                shopGemPowerObj.SetActive(false);

                for (int i = 0; i < cuirassElements.Length; i++)
                    cuirassElements[i].HighlightingControl(false);

                for (int i = 0; i < helmetElements.Length; i++)
                    helmetElements[i].HighlightingControl(false);

                for (int i = 0; i < shieldElements.Length; i++)
                    shieldElements[i].HighlightingControl(false);

                for (int i = 0; i < weaponElements.Length; i++)
                    weaponElements[i].HighlightingControl(false);
            }
        }

        /// <summary>
        /// Задать цвет изображения камня в магазине 
        /// в зависимости от типа камня оружия
        /// </summary>
        private void SetColorOfShopImageGem()
        {           
            switch (weaponList[weaponItemNumber].DamageTypeGem)
            {
                case GemType.None:
                    imageShopGem.color = Color.white;
                    textShopGemPower.text = "0";
                    break;
                case GemType.Frozen:
                    imageShopGem.color = colorFrozen;
                    break;
                case GemType.Fire:
                    imageShopGem.color = colorFire;
                    break;
                case GemType.Electric:
                    imageShopGem.color = colorElectric;
                    break;
                case GemType.Powerful:
                    imageShopGem.color = colorPowerful;
                    break;
            }
        }

        /// <summary>
        /// Купить выбранную кирасу
        /// </summary>
        public bool BuyCuirass()
		{
            string str = PlayerPrefs.GetString("cuirassArray");
            int[] array = LibraryObjectsWorker.StringSplitter(str, '_');

			for (int i = 0; i < array.Length; i++)
				if (array[i] == cuirassItemNumber) return false;

			PlayerPrefs.SetString("cuirassArray", str + cuirassItemNumber + "_" );
			PlayerPrefs.Save();
            userResources.Gems -= cuirassList[cuirassItemNumber].GemsCost;
            userResources.Money -= cuirassList[cuirassItemNumber].MoneyCost;
            return true;
		}

		/// <summary>
		/// Купить выбранный шлем
		/// </summary>
		public bool BuyHelmet()
		{
            string str = PlayerPrefs.GetString("helmetArray");
            int[] array = LibraryObjectsWorker.StringSplitter(str, '_');

			for (int i = 0; i < array.Length; i++)
				if (array[i] == helmetItemNumber) return false;

			PlayerPrefs.SetString("helmetArray", str + helmetItemNumber + "_");
			PlayerPrefs.Save();
            userResources.Gems -= helmetList[helmetItemNumber].GemsCost;
            userResources.Money -= helmetList[helmetItemNumber].MoneyCost;
            return true;
		}

		/// <summary>
		/// Купить щит
		/// </summary>
		public bool BuyShield()
		{
            string str = PlayerPrefs.GetString("shieldArray");
            int[] array = LibraryObjectsWorker.StringSplitter(str, '_');

            for (int i = 0; i < array.Length; i++)
                if (array[i] == shieldItemNumber) return false;

            PlayerPrefs.SetString("shieldArray", str + shieldItemNumber + "_");
			PlayerPrefs.Save();
            userResources.Gems -= shieldList[shieldItemNumber].GemsCost;
            userResources.Money -= shieldList[shieldItemNumber].MoneyCost;
            return true;
        }

		/// <summary>
		/// Купить оружие
		/// </summary>
		public bool BuyWeapon()
		{
            string str = PlayerPrefs.GetString("weaponArray");
            int[] array = LibraryObjectsWorker.StringSplitter(str, '_');

            for (int i = 0; i < array.Length; i++)
                if (array[i] == weaponItemNumber) return false;

            PlayerPrefs.SetString("weaponArray", str + weaponItemNumber + "_");
			PlayerPrefs.Save();
            userResources.Gems -= weaponList[weaponItemNumber].GemsCost;
            userResources.Money -= weaponList[weaponItemNumber].MoneyCost;
            return true;
		}

        /// <summary>
        /// Проверить, достаточно ли ресурсов для покупки предмета
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool IsEnoughUserResources(int index)
        {
            switch (index)
            {
                case 0:
                    return userResources.IsEnoughGemNMoney(cuirassList[cuirassItemNumber].MoneyCost,
                        cuirassList[cuirassItemNumber].GemsCost);
                case 1:
                    return userResources.IsEnoughGemNMoney(helmetList[helmetItemNumber].MoneyCost,
                        helmetList[helmetItemNumber].GemsCost);
                case 2:
                    return userResources.IsEnoughGemNMoney(shieldList[shieldItemNumber].MoneyCost,
                        shieldList[shieldItemNumber].GemsCost);
                case 3:
                    return userResources.IsEnoughGemNMoney(weaponList[weaponItemNumber].MoneyCost,
                        weaponList[weaponItemNumber].GemsCost);
                default:
                    return false;
            }
        }
        #endregion

        #region Корутины
        /// <summary>
        /// Создает и настраивает кнопу в ленте - ШИТ
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> ShieldCorutine()
		{
            object[] tempObjects = Resources.LoadAll(shieldPrefix);
            shieldElements = new IRepositoryObject[tempObjects.Length];

            for (int i = 0; i < tempObjects.Length; i++)
			{
				GameObject gemGgamObj = (GameObject)tempObjects[i];
				GameObject item = Instantiate(itemArmor);
				ArmorButton button = item.GetComponent<ArmorButton>();

                shieldList.Add(gemGgamObj.GetComponent<PartArmoryInformation>());
                shieldElements[i] = button;

                button.SetArmorCraft(armorCraft);
                button.SetShop(shopComponent);
                button.SetNumber(i);
				button.ArmoryClassShop = shieldList[i].ArmoryType;
				button.Weight = shieldList[i].WeightArmory.ToString();

                button.SetName(shieldList[i].ArmoryName);
                button.SetMoneyCost(shieldList[i].MoneyCost);
                button.SetGemsCost(shieldList[i].GemsCost);
                button.SetLogo(shieldList[i].ImageArm);

                item.transform.SetParent(shieldRepository.transform, false);
			}
			scrollRectShieldRepository =
				shieldRepository.transform.parent.GetComponent<ScrollRect>();
			yield return 0;
		}

		/// <summary>
		/// Создает и настраивает кнопу в ленте - КИРАСА
		/// </summary>
		/// <returns></returns>
		private IEnumerator<float> CuirassCorutine()
		{
            object[] tempObjects = Resources.LoadAll(cuirassPrefix);
            cuirassElements = new IRepositoryObject[tempObjects.Length];

            for (int i = 0; i < tempObjects.Length; i++)
			{
				GameObject gemGgamObj = (GameObject)tempObjects[i];
				GameObject item = Instantiate(itemArmor);
				ArmorButton button = item.GetComponent<ArmorButton>();

                cuirassList.Add(gemGgamObj.GetComponent<PartArmoryInformation>());
                cuirassElements[i] = button;

                button.SetArmorCraft(armorCraft);
                button.SetShop(shopComponent);
				button.SetNumber(i);
				button.ArmoryClassShop = cuirassList[i].ArmoryType;
				button.Weight = cuirassList[i].WeightArmory.ToString();

                button.SetName(cuirassList[i].ArmoryName);
                button.SetMoneyCost(cuirassList[i].MoneyCost);
                button.SetGemsCost(cuirassList[i].GemsCost);
                button.SetLogo(cuirassList[i].ImageArm);

                item.transform.SetParent(cuirassRepository.transform, false);
			}
			scrollRectCuirasseRepository =
				cuirassRepository.transform.parent.GetComponent<ScrollRect>();
			yield return 0;
		}

		/// <summary>
		/// Создает и настраивает кнопу в ленте - ШЛЕМ
		/// </summary>
		/// <returns></returns>
		private IEnumerator<float> HelmetCorutine()
		{
            object[] tempObjects = Resources.LoadAll(helmetPrefix);
            helmetElements = new IRepositoryObject[tempObjects.Length];

            for (int i = 0; i < tempObjects.Length; i++)
			{
				GameObject gemGgamObj = (GameObject)tempObjects[i];
				GameObject item = Instantiate(itemArmor);
				ArmorButton button = item.GetComponent<ArmorButton>();

                helmetList.Add(gemGgamObj.GetComponent<PartArmoryInformation>());
                helmetElements[i] = button;

                button.SetArmorCraft(armorCraft);
                button.SetShop(shopComponent);
				button.SetNumber(i);
				button.ArmoryClassShop = helmetList[i].ArmoryType;
				button.Weight = helmetList[i].WeightArmory.ToString();

                button.SetName(helmetList[i].ArmoryName);
                button.SetMoneyCost(helmetList[i].MoneyCost);
                button.SetGemsCost(helmetList[i].GemsCost);
                button.SetLogo(helmetList[i].ImageArm);

                item.transform.SetParent(helmetRepository.transform, false);
			}
			scrollRectHelmetRepository =
				helmetRepository.transform.parent.GetComponent<ScrollRect>();

			yield return 0;
		}

		/// <summary>
		/// Запускать для отображения элементов оружия в ленте
		/// </summary>
		/// <returns></returns>
		private IEnumerator<float> WeaponCorutine()
		{
            object[] tempObjects = Resources.LoadAll(weaponPrefix);
            weaponElements = new IRepositoryObject[tempObjects.Length];

            for (int i = 0; i < tempObjects.Length; i++)
			{
				GameObject weaponGamObj = (GameObject)tempObjects[i];
				GameObject item = Instantiate(itemWeapon);
				WeaponButton button = item.GetComponent<WeaponButton>();

                weaponList.Add(weaponGamObj.GetComponent<Weapon>());
                weaponElements[i] = button;

                button.SetWeaponCraft(weaponCraft);
                button.SetShop(shopComponent);
				button.SetNumber(i);

				button.SetName(weaponList[i].HeadName);
				button.SetLogo(weaponList[i].ItemImage);
                button.SetMoneyCost(weaponList[i].MoneyCost);
                button.SetGemsCost(weaponList[i].GemsCost);

                item.transform.SetParent(weaponRepository.transform, false);
			}

			scrollRectWeaponRepository =
				weaponRepository.transform.parent.GetComponent<ScrollRect>();
			yield return 0;
		}
        #endregion
    }
}
