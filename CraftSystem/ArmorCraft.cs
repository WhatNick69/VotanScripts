using MovementEffects;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using VotanLibraries;

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

		string cuirassPostfix = "_C";
		string cuirassPrefix = "Prefabs/Armor/Cuirass/";
		string helmetPostfix = "_H";
		string helmetPrefix = "Prefabs/Armor/Helmet/";
		string shieldPostfix = "_S";
		string shieldPrefix = "Prefabs/Armor/Shield/";

		private List<PartArmoryInformation> cuirassList;
		private List<PartArmoryInformation> helmetList;
		private List<PartArmoryInformation> shieldList;

		private GameObject[] shieldArray;
		private GameObject[] cuirassArray;
		private GameObject[] helmetArray;

		[SerializeField]
		private int cuirassItemNumber;
		[SerializeField]
		private int helmetItemNumber;
		[SerializeField]
		private int shieldItemNumber;

		ArmorPrefabs AP;
		ArmorCraft AC;
		PlayerStats PStats;

        ScrollRect scrollRectHelmetRepository;
        ScrollRect scrollRectShieldRepository;
        ScrollRect scrollRectCuirasseRepository;

		float intemNumbCuirass;
		float intemNumbHelmet;
		float intemNumbShield;
		float normPosCuirass;
		float normPosHelmet;
		float normPosShield;

		List<int> arrayBoughtCuirass;
		List<int> arrayBoughtHelmet;
		List<int> arrayBoughtShield;
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

        /// <summary>
        /// Вызывает окно с лнтой шлемов
        /// </summary>
        public void HelmetWindow()
		{
			shieldWindow.SetActive(false);
			cuirassWindow.SetActive(false);
			helmetWindow.SetActive(true);

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

			PStats.ArmorPage();
			scrollRectShieldRepository.horizontalNormalizedPosition = 0;
        }

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

		public void CloseUpdateWindow()
		{
			upadatePanel.SetActive(false);
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
				helmetArray[i] = null;
				helmetList.RemoveAt(0);
				arrayBoughtHelmet.RemoveAt(0);
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

			for (int i = 0; i < k; i++)
			{
				GameObject d = cuirassRepository.transform.GetChild(0).gameObject;
				cuirassRepository.transform.GetChild(0).SetParent(null);
				Destroy(d);
				cuirassArray[i] = null;
				cuirassList.RemoveAt(0);
				arrayBoughtCuirass.RemoveAt(0);
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
				shieldArray[i] = null;
				shieldList.RemoveAt(0);
				arrayBoughtShield.RemoveAt(0);
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
				Debug.Log(level+1);
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
				if (cuirassItemNumber != Mathf.Round(scrollRectCuirasseRepository.horizontalNormalizedPosition * (cuirassList.Count - 1)) ||
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
		#endregion

		/// <summary>
		/// Запускать при старте игры, обязательно!
		/// </summary>
		public void PlayArena()
		{
            if (AP == null)
                AP = GameObject.Find("GetPrefabs").GetComponent<ArmorPrefabs>();
	        
            AP.Cuirass = cuirassArray[cuirassItemNumber];
			AP.Helmet = helmetArray[helmetItemNumber];
			AP.Shield = shieldArray[shieldItemNumber];
		}

		private void Awake() // ____________start__________
		{
			PStats = GetComponent<PlayerStats>();
			AC = GetComponent<ArmorCraft>();
			cuirassList = new List<PartArmoryInformation>();
			helmetList = new List<PartArmoryInformation>();
			shieldList = new List<PartArmoryInformation>();
			shieldArray = new GameObject[Resources.LoadAll("Prefabs/Armor/Shield").Length];
			cuirassArray = new GameObject[Resources.LoadAll("Prefabs/Armor/Cuirass").Length];
			helmetArray = new GameObject[Resources.LoadAll("Prefabs/Armor/Helmet").Length];

			arrayBoughtCuirass = new List<int>();
			arrayBoughtHelmet = new List<int>();
			arrayBoughtShield = new List<int>();

			Timing.RunCoroutine(ShieldCorutine());
			Timing.RunCoroutine(HelmetCorutine());
			Timing.RunCoroutine(CuirassCorutine());
		}

		private void FixedUpdate()
		{
			ChekScroll();
		}

		/// <summary>
		/// Создает и настраивает кнопу в ленте - ШИТ
		/// </summary>
		/// <returns></returns>
		private IEnumerator<float> ShieldCorutine()
		{
			int k = LibraryObjectsWorker.StringSplitter
							(PlayerPrefs.GetString("shieldArray"), '_').Length;
			for (int i = 0; i < k; i++)
			{
				arrayBoughtShield.Add(LibraryObjectsWorker.StringSplitter
							(PlayerPrefs.GetString("shieldArray"), '_')[i]);
			}

			for (int i = 0; i < k; i++)
			{
				if (Resources.Load(shieldPrefix + i + shieldPostfix))
				{
					GameObject gemGgamObj = (GameObject)Resources.Load(shieldPrefix + arrayBoughtShield[i] + shieldPostfix);
					shieldArray[i] = gemGgamObj.GetComponent<LevelManager>().GetItemLevel(PlayerPrefs.
						GetInt("shield_" + arrayBoughtShield[i]));
					shieldList.Add(shieldArray[i].GetComponent<PartArmoryInformation>());
					GameObject item = Instantiate(itemArmor);
					ArmorButton button = item.GetComponent<ArmorButton>();
					button.SetArmorCraft(AC);
					button.SetNumber(i);
                    button.ArmoryClass = shieldList[i].ArmoryType;
					button.Weight = shieldList[i].WeightArmory.ToString();
					button.SetName(shieldList[i].ArmoryName);
					button.SetArmor(shieldList[i].ArmoryValue.ToString());
					button.SetLogo (shieldList[i].ImageArm);
					item.transform.SetParent(shieldRepository.transform, false);
				}
			}
            scrollRectShieldRepository = 
                shieldRepository.transform.parent.GetComponent<ScrollRect>();
			yield return 0.02f;
		}

		/// <summary>
		/// Создает и настраивает кнопу в ленте - КИРАСА
		/// </summary>
		/// <returns></returns>
		private IEnumerator<float> CuirassCorutine()
		{
			int k = LibraryObjectsWorker.StringSplitter
							(PlayerPrefs.GetString("cuirassArray"), '_').Length;
			for (int i = 0; i < k; i++)
			{
				arrayBoughtCuirass.Add(LibraryObjectsWorker.StringSplitter
							(PlayerPrefs.GetString("cuirassArray"), '_')[i]);
			}

			for (int i = 0; i < k; i++)
			{
				if (Resources.Load(cuirassPrefix + i + cuirassPostfix))
				{
					GameObject gemGgamObj = (GameObject)Resources.Load(cuirassPrefix + arrayBoughtCuirass[i] + cuirassPostfix);
					cuirassArray[i] = gemGgamObj.GetComponent<LevelManager>().GetItemLevel(PlayerPrefs.
						GetInt("cuirass_" + arrayBoughtCuirass[i]));
					cuirassList.Add(cuirassArray[i].GetComponent<PartArmoryInformation>());
					GameObject item = Instantiate(itemArmor);
					ArmorButton button = item.GetComponent<ArmorButton>();
					button.SetArmorCraft(AC);
					button.SetNumber(i);
                    button.ArmoryClass = cuirassList[i].ArmoryType;
					button.Weight = cuirassList[i].WeightArmory.ToString();
					button.SetName(cuirassList[i].ArmoryName);
					button.SetArmor(cuirassList[i].ArmoryValue.ToString());
					button.SetLogo (cuirassList[i].ImageArm);
					item.transform.SetParent(cuirassRepository.transform, false);
				}
			}
            scrollRectCuirasseRepository =
                cuirassRepository.transform.parent.GetComponent<ScrollRect>();
			yield return 0.02f;
		}

		/// <summary>
		/// Создает и настраивает кнопу в ленте - ШЛЕМ
		/// </summary>
		/// <returns></returns>
		private IEnumerator<float> HelmetCorutine()
		{
			int k = LibraryObjectsWorker.StringSplitter
							(PlayerPrefs.GetString("helmetArray"), '_').Length;
			for (int i = 0; i < k; i++)
			{
				arrayBoughtHelmet.Add(LibraryObjectsWorker.StringSplitter
							(PlayerPrefs.GetString("helmetArray"), '_')[i]);
			}

			for (int i = 0; i < k; i++)
			{
				if (Resources.Load(helmetPrefix + i + helmetPostfix))
				{
					GameObject gemGgamObj = (GameObject)Resources.Load(helmetPrefix + arrayBoughtHelmet[i] + helmetPostfix);
					helmetArray[i] = gemGgamObj.GetComponent<LevelManager>().GetItemLevel(PlayerPrefs.
						GetInt("helmet_" + arrayBoughtHelmet[i]));
					helmetList.Add(helmetArray[i].GetComponent<PartArmoryInformation>());
					GameObject item = Instantiate(itemArmor);
					ArmorButton button = item.GetComponent<ArmorButton>();
					button.SetArmorCraft(AC);
					button.SetNumber(i);
                    button.ArmoryClass = helmetList[i].ArmoryType;
					button.Weight = helmetList[i].WeightArmory.ToString();
                    button.SetName(helmetList[i].ArmoryName);
					button.SetArmor(helmetList[i].ArmoryValue.ToString());
					button.SetLogo (helmetList[i].ImageArm);
					item.transform.SetParent(helmetRepository.transform, false);
				}
			}
            scrollRectHelmetRepository =
                helmetRepository.transform.parent.GetComponent<ScrollRect>();

            yield return 0.02f;
		}
	}
}
