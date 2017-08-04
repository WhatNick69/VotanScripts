using MovementEffects;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

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
		GameObject cuirassWindow;
		[SerializeField]
		GameObject shieldWindow;

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
            scrollRectShieldRepository.horizontalNormalizedPosition = 0;
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
				PStats.NewHelmetArmor = helmetList[(int)intemNumbHelmet].ArmoryValue;
				PStats.NewShieldArmor = shieldList[(int)intemNumbShield].ArmoryValue;
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
	        
            AP.Cuirass = (GameObject)Resources.Load(cuirassPrefix + cuirassItemNumber + cuirassPostfix);
			AP.Helmet = (GameObject)Resources.Load(helmetPrefix + helmetItemNumber + helmetPostfix);
			AP.Shield = (GameObject)Resources.Load(shieldPrefix + shieldItemNumber + shieldPostfix);
		}

		private void Awake() // ____________start__________
		{
			PStats = GetComponent<PlayerStats>();
			AC = GetComponent<ArmorCraft>();
			cuirassList = new List<PartArmoryInformation>();
			helmetList = new List<PartArmoryInformation>();
			shieldList = new List<PartArmoryInformation>();

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
			int count = Resources.LoadAll("Prefabs/Armor/Shield").Length;
			for (int i = 0; i < count; i++)
			{
				if (Resources.Load(shieldPrefix + i + shieldPostfix))
				{
					GameObject gemGgamObj = (GameObject)Resources.Load(shieldPrefix + i + shieldPostfix);
					shieldList.Add(gemGgamObj.GetComponent<PartArmoryInformation>());
					GameObject item = Instantiate(itemArmor);
					ArmorButton button = item.GetComponent<ArmorButton>();
					button.SetArmorCraft(AC);
					button.SetNumber(i);
                    button.ArmoryClass = shieldList[i].ArmoryType;

                    button.SetName(shieldList[i].ArmoryName);
					button.SetArmor(shieldList[i].ArmoryValue.ToString());
					button.SetLogo (shieldList[i].ImageArm);
					item.transform.SetParent(shieldRepository.transform, false);
				}
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
			int count = Resources.LoadAll("Prefabs/Armor/Cuirass").Length;
			for (int i = 0; i < count; i++)
			{
				if (Resources.Load(cuirassPrefix + i + cuirassPostfix))
				{
					GameObject gemGgamObj = (GameObject)Resources.Load(cuirassPrefix + i + cuirassPostfix);
					cuirassList.Add(gemGgamObj.GetComponent<PartArmoryInformation>());
					GameObject item = Instantiate(itemArmor);
					ArmorButton button = item.GetComponent<ArmorButton>();
					button.SetArmorCraft(AC);
					button.SetNumber(i);
                    button.ArmoryClass = cuirassList[i].ArmoryType;

                    button.SetName(cuirassList[i].ArmoryName);
					button.SetArmor(cuirassList[i].ArmoryValue.ToString());
					button.SetLogo (cuirassList[i].ImageArm);
					item.transform.SetParent(cuirassRepository.transform, false);
				}
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
			int count = Resources.LoadAll("Prefabs/Armor/Helmet").Length;
			for (int i = 0; i < count; i++)
			{
				if (Resources.Load(shieldPrefix + i + shieldPostfix))
				{
					GameObject gemGgamObj = (GameObject)Resources.Load(helmetPrefix + i + helmetPostfix);
					helmetList.Add(gemGgamObj.GetComponent<PartArmoryInformation>());
					GameObject item = Instantiate(itemArmor);
					ArmorButton button = item.GetComponent<ArmorButton>();
					button.SetArmorCraft(AC);
					button.SetNumber(i);
                    button.ArmoryClass = helmetList[i].ArmoryType;

                    button.SetName(helmetList[i].ArmoryName);
					button.SetArmor(helmetList[i].ArmoryValue.ToString());
					button.SetLogo (helmetList[i].ImageArm);
					item.transform.SetParent(helmetRepository.transform, false);
				}
			}
            scrollRectHelmetRepository =
                helmetRepository.transform.parent.GetComponent<ScrollRect>();

            yield return 0;
		}
	}
}
