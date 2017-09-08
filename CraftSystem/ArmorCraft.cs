using MovementEffects;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VotanLibraries;
using ShopSystem;

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
		[SerializeField]
		GameObject upadatePanel;
		[SerializeField]
		GameObject updateButton;

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

		[SerializeField]
		private int cuirassItemNumber;
		[SerializeField]
		private int helmetItemNumber;
		[SerializeField]
		private int shieldItemNumber;

		ArmorPrefabs AP;
		PlayerStats PStats;
        WeaponCraft WC;
        [SerializeField]
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
        private void Awake()
        {
            PStats = GetComponent<PlayerStats>();
            WC = GetComponent<WeaponCraft>();

            cuirassList = new List<PartArmoryInformation>();
            helmetList = new List<PartArmoryInformation>();
            shieldList = new List<PartArmoryInformation>();

            Timing.RunCoroutine(ShieldCorutine());
            Timing.RunCoroutine(HelmetCorutine());
            Timing.RunCoroutine(CuirassCorutine());
        }

        /// <summary>
        /// Проверка
        /// </summary>
        private void FixedUpdate()
        {
            ChekScroll();
        }

        /// <summary>
        /// Запускать при старте игры, обязательно!
        /// </summary>
        public void PlayArena()
        {
            if (AP == null)
                AP = GameObject.Find("GetPrefabs").GetComponent<ArmorPrefabs>();

            AP.Cuirass = cuirassList[cuirassItemNumber].gameObject;
            AP.Helmet = helmetList[helmetItemNumber].gameObject;
            AP.Shield = shieldList[shieldItemNumber].gameObject;
        }

        /// <summary>
        /// Метод проверяет: 
        /// 1. Двинул ли игрок ленту с элементаи брони
        /// 2. Сменился элемент на следующий или нет? Если да, то меняет номер элемента
        /// кторый отправится в таблицу с характеристиками
        /// 
        /// - После этого, в таблицу отправляются значения элементов, которые находятся
        /// по центру окна прокрутки (определяется предыдущими проверками)
        /// - В самом кенце сохраняется позиция ленты, для проверки в следющем кадре 1го условия
        /// </summary>
        private void ChekScroll()
        {
            if (normPosCuirass != scrollRectCuirasseRepository.horizontalNormalizedPosition ||
                normPosHelmet != scrollRectHelmetRepository.horizontalNormalizedPosition ||
                normPosShield != scrollRectShieldRepository.horizontalNormalizedPosition)
            {
                if (cuirassItemNumber !=
                    Mathf.Round(scrollRectCuirasseRepository.horizontalNormalizedPosition * (cuirassList.Count - 1)) ||
                    helmetItemNumber != Mathf.Round(scrollRectHelmetRepository.horizontalNormalizedPosition * (helmetList.Count - 1)) ||
                    shieldItemNumber != Mathf.Round(scrollRectShieldRepository.horizontalNormalizedPosition * (shieldList.Count - 1)))
                {
                    intemNumbCuirass = Mathf.Round(scrollRectCuirasseRepository.horizontalNormalizedPosition * (cuirassList.Count - 1));
                    intemNumbHelmet = Mathf.Round(scrollRectHelmetRepository.horizontalNormalizedPosition * (helmetList.Count - 1));
                    intemNumbShield = Mathf.Round(scrollRectShieldRepository.horizontalNormalizedPosition * (shieldList.Count - 1));
                }
                else
                {
                    intemNumbCuirass = cuirassItemNumber;
                    intemNumbHelmet = HelmetItemNumber;
                    intemNumbShield = shieldItemNumber;
                }

                PStats.NewCuirassArmor = cuirassList[(int)intemNumbCuirass].ArmoryValue;
                PStats.NewCuirassWeight = cuirassList[(int)intemNumbCuirass].WeightArmory;
                PStats.NewHelmetArmor = helmetList[(int)intemNumbHelmet].ArmoryValue;
                PStats.NewHelmetWeight = helmetList[(int)intemNumbHelmet].WeightArmory;
                PStats.NewShieldArmor = shieldList[(int)intemNumbShield].ArmoryValue;
                PStats.NewShieldWeight = shieldList[(int)intemNumbShield].WeightArmory;
            }
            normPosCuirass = scrollRectCuirasseRepository.horizontalNormalizedPosition;
            normPosHelmet = scrollRectHelmetRepository.horizontalNormalizedPosition;
            normPosShield = scrollRectShieldRepository.horizontalNormalizedPosition;
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

            WC.CloseAllWindows();

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

            WC.CloseAllWindows();

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

            WC.CloseAllWindows();

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
                upadatePanel.SetActive(true);
                cuirassUpadateButton.SetActive(true);
                helmetUpadateButton.SetActive(false);
                shieldUpadateButton.SetActive(false);
            }
            else if (helmetWindow.activeSelf)
            {
                upadatePanel.SetActive(true);
                helmetUpadateButton.SetActive(true);
                shieldUpadateButton.SetActive(false);
                cuirassUpadateButton.SetActive(false);
            }
            else
            {
                upadatePanel.SetActive(true);
                shieldUpadateButton.SetActive(true);
                cuirassUpadateButton.SetActive(false);
                helmetUpadateButton.SetActive(false);
            }
        }

        /// <summary>
        /// Закрыть окно обновления окон
        /// </summary>
        public void CloseUpdateWindow()
        {
            upadatePanel.SetActive(false);
        }

        /// <summary>
        /// Обновляет окно шлемов. Вызывать при покупке
        /// </summary>
        public void RestartHelmetWindow()
        {
            int k = LibraryObjectsWorker.StringSplitter
                            (PlayerPrefs.GetString("helmetArray"), '_').Length - 1;
            for (int i = 0; i < k; i++)
            {

                GameObject d = helmetRepository.transform.GetChild(0).gameObject;
                helmetRepository.transform.GetChild(0).SetParent(null);
                Destroy(d);
            }
            Timing.RunCoroutine(HelmetCorutine());
        }

        /// <summary>
        /// Обновляет окно кирас. Вызывать при покупке
        /// </summary>
        public void RestartCuirassWindow()
        {
            int k = LibraryObjectsWorker.StringSplitter
                            (PlayerPrefs.GetString("cuirassArray"), '_').Length - 1;
            Debug.Log(k);
            for (int i = 0; i < k; i++)
            {
                Debug.Log(i);
                GameObject d = cuirassRepository.transform.GetChild(0).gameObject;
                cuirassRepository.transform.GetChild(0).SetParent(null);
                Destroy(d);
                cuirassList.RemoveAt(0);
            }
            Timing.RunCoroutine(CuirassCorutine());
        }

        /// <summary>
        /// Обновляет окно щитов. Вызывать при покупке
        /// </summary>
        public void RestartShieldWindow()
        {
            int k = LibraryObjectsWorker.StringSplitter
                            (PlayerPrefs.GetString("shieldArray"), '_').Length - 1;

            for (int i = 0; i < k; i++)
            {
                GameObject d = shieldRepository.transform.GetChild(0).gameObject;
                shieldRepository.transform.GetChild(0).SetParent(null);
                Destroy(d);
                shieldList.RemoveAt(0);
            }
            Timing.RunCoroutine(ShieldCorutine());
        }

        /// <summary>
        /// Улучшает выбранную кирасу
        /// </summary>
        public void UpdateCuirass()
        {
            int level = PlayerPrefs.GetInt("cuirass_" + cuirassItemNumber);
            if (level < 3)
            {
                PlayerPrefs.SetInt(("cuirass_" + cuirassItemNumber), level + 1);
                Debug.Log(level + 1);
                PlayerPrefs.Save();
            }
            upadatePanel.SetActive(false);
        }

        /// <summary>
        /// Улучшает выбранный щит
        /// </summary>
        public void UpdateShield()
        {
            int level = PlayerPrefs.GetInt("shield_" + shieldItemNumber);
            if (level < 3)
            {
                PlayerPrefs.SetInt(("shield_" + shieldItemNumber), level + 1);
                Debug.Log(level + 1);
                PlayerPrefs.Save();
            }
            upadatePanel.SetActive(false);
        }

        /// <summary>
        /// Улучшает выбранный шлем
        /// </summary>
        public void UpdateHelmet()
        {
            int level = PlayerPrefs.GetInt("helmet_" + helmetItemNumber);
            if (level < 3)
            {
                PlayerPrefs.SetInt(("helmet_" + helmetItemNumber), level + 1);
                Debug.Log(level + 1);
                PlayerPrefs.Save();
            }
            upadatePanel.SetActive(false);
        }
        #endregion

        #region Корутины
        /// <summary>
        /// Создает и настраивает кнопу в ленте - ШИТ
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> ShieldCorutine()
		{
            string str = PlayerPrefs.GetString("shieldArray");
            int[] elements = LibraryObjectsWorker.StringSplitter(str, '_');
            object[] tempObjects = new object[elements.Length];

            for (int i = 0; i < elements.Length; i++)
                tempObjects[i] = Resources.Load(shieldPrefix + elements[i] + shieldPostfix);

            for (int i = 0; i < tempObjects.Length; i++)
			{
				GameObject gemGgamObj = (GameObject)tempObjects[i];

                shieldList.Add(gemGgamObj.GetComponent<PartArmoryInformation>());
				GameObject item = Instantiate(itemArmor);
				ArmorButton button = item.GetComponent<ArmorButton>();
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
            yield return 0;
		}

		/// <summary>
		/// Создает и настраивает кнопу в ленте - КИРАСА
		/// </summary>
		/// <returns></returns>
		private IEnumerator<float> CuirassCorutine()
		{
            string str = PlayerPrefs.GetString("cuirassArray");
            int[] elements = LibraryObjectsWorker.StringSplitter(str, '_');
            object[] tempObjects = new object[elements.Length];

            for (int i = 0; i < elements.Length; i++)
                tempObjects[i] = Resources.Load(cuirassPrefix + elements[i] + cuirassPostfix);

            for (int i = 0; i < tempObjects.Length; i++)
			{
				GameObject gemGgamObj = (GameObject)tempObjects[i];

				cuirassList.Add(gemGgamObj.GetComponent<PartArmoryInformation>());
				GameObject item = Instantiate(itemArmor);
				ArmorButton button = item.GetComponent<ArmorButton>();
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
            yield return 0;
		}

		/// <summary>
		/// Создает и настраивает кнопу в ленте - ШЛЕМ
		/// </summary>
		/// <returns></returns>
		private IEnumerator<float> HelmetCorutine()
		{
            string str = PlayerPrefs.GetString("helmetArray");
            int[] elements = LibraryObjectsWorker.StringSplitter(str, '_');
            object[] tempObjects = new object[elements.Length];

			for (int i = 0; i < elements.Length; i++)
                tempObjects[i] = Resources.Load(helmetPrefix + elements[i] + helmetPostfix);

			for (int i = 0; i < tempObjects.Length; i++)
			{
				GameObject gemGgamObj = (GameObject)tempObjects[i];

				helmetList.Add(gemGgamObj.GetComponent<PartArmoryInformation>());
				GameObject item = Instantiate(itemArmor);
				ArmorButton button = item.GetComponent<ArmorButton>();
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
            yield return 0;
		}
        #endregion
    }
}
