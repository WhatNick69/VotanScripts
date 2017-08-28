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
		[SerializeField]
		private GameObject cuirassRepository;
		[SerializeField]
		private GameObject helmetRepository;
		[SerializeField]
		private GameObject shieldRepository;
		[SerializeField]
		private GameObject weaponRepository;

		[SerializeField, Tooltip("кнопка брони")]
		private GameObject itemArmor;
		[SerializeField, Tooltip("кнопка оружия")]
		private GameObject itemWeapon;

		[SerializeField]
		GameObject helmetWindow;
		[SerializeField]
		GameObject helmetBuyButton;
		[SerializeField]
		GameObject cuirassWindow;
		[SerializeField]
		GameObject cuirassBuyButton;
		[SerializeField]
		GameObject shieldWindow;
		[SerializeField]
		GameObject shieldBuyButton;
		[SerializeField]
		GameObject weaponWindow;
		[SerializeField]
		GameObject weaponBuyButton;

		private GameObject cuirass;
		private GameObject helmet;
		private GameObject shield;
		private GameObject weapon;


		string cuirassPostfix = "_C";
		string cuirassPrefix = "Prefabs/Armor/Cuirass/";
		string helmetPostfix = "_H";
		string helmetPrefix = "Prefabs/Armor/Helmet/";
		string shieldPostfix = "_S";
		string shieldPrefix = "Prefabs/Armor/Shield/";
		string headPrefix = "Prefabs/Weapon/Weapon_";

		private List<PartArmoryInformation> cuirassList;
		private List<PartArmoryInformation> helmetList;
		private List<PartArmoryInformation> shieldList;

		private GameObject[] shieldArray;
		private GameObject[] cuirassArray;
		private GameObject[] helmetArray;
		private List<Weapon> weaponList;
		private GameObject[] weaponArray;
		private int[] weaponStats;

		private int cuirassItemNumber;
		private int helmetItemNumber;
		private int shieldItemNumber;
		private int weaponItemNumber;

		ArmorPrefabs AP;
		Shop SHP;

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

		/// <summary>
		/// Вызывает окно с лнтой шлемов
		/// </summary>
		public void HelmetWindow()
		{
			shieldWindow.SetActive(false);
			cuirassWindow.SetActive(false);
			helmetWindow.SetActive(true);

			cuirassBuyButton.SetActive(false);
			helmetBuyButton.SetActive(true);
			shieldBuyButton.SetActive(false);

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

			cuirassBuyButton.SetActive(true);
			helmetBuyButton.SetActive(false);
			shieldBuyButton.SetActive(false);

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
			
			cuirassBuyButton.SetActive(false);
			helmetBuyButton.SetActive(false);
			shieldBuyButton.SetActive(true);

			scrollRectShieldRepository.horizontalNormalizedPosition = 0;
		}

		/// <summary>
		/// Открыть окно с оружием
		/// </summary>
		public void WeaponWindow()
		{
			armorWind.SetActive(false);
			weaponWind.SetActive(true);
		}

		/// <summary>
		/// Открыть окно с бронёй
		/// </summary>
		public void ArmorWindow()
		{
			weaponWind.SetActive(false);
			armorWind.SetActive(true);
		}

		/// <summary>
		/// Купить выбранную кирасу
		/// </summary>
		public void BuyCuirass()
		{
			bool b = true;
			string str = PlayerPrefs.GetString("cuirassArray");
			int k = LibraryObjectsWorker.StringSplitter
							(PlayerPrefs.GetString("cuirassArray"), '_').Length;
			List<int> arrayShopCuirass = new List<int>();
			for (int i = 0; i < k; i++)
			{
				arrayShopCuirass.Add(LibraryObjectsWorker.StringSplitter
							(PlayerPrefs.GetString("cuirassArray"), '_')[i]);
			}

			for (int i = 0; i < k; i++)
			{
				if (arrayShopCuirass[i] == cuirassItemNumber)
				{
					b = false;
					break;
				}
			}

			if (b == true)
			{
				PlayerPrefs.SetString("cuirassArray", str + cuirassItemNumber + "_" );
				PlayerPrefs.Save();
				Debug.Log("Cuirass has buy!");
			}
		}

		/// <summary>
		/// Купить выбранный шлем
		/// </summary>
		public void BuyHelmet()
		{
			bool b = true;
			string str = PlayerPrefs.GetString("helmetArray");
			int k = LibraryObjectsWorker.StringSplitter
							(PlayerPrefs.GetString("helmetArray"), '_').Length;
			List<int> arrayShopHelmet = new List<int>();
			for (int i = 0; i < k; i++)
			{
				arrayShopHelmet.Add(LibraryObjectsWorker.StringSplitter
							(PlayerPrefs.GetString("helmetArray"), '_')[i]);
			}
			
			for (int i = 0; i < k; i++)
			{
				Debug.Log(helmetItemNumber);
				if (arrayShopHelmet[i] == helmetItemNumber)
				{
					b = false;
					break;
				}
			}

			if (b == true)
			{
				PlayerPrefs.SetString("helmetArray", str + helmetItemNumber + "_");
				PlayerPrefs.Save();
				Debug.Log("Helmet has buy!");
			}
		}

		/// <summary>
		/// Купить щит
		/// </summary>
		public void BuyShield()
		{
			bool b = true;
			string str = PlayerPrefs.GetString("shieldArray");
			int k = LibraryObjectsWorker.StringSplitter
							(PlayerPrefs.GetString("shieldArray"), '_').Length;
			List<int> arrayShopShield = new List<int>();
			for (int i = 0; i < k; i++)
			{
				arrayShopShield.Add(LibraryObjectsWorker.StringSplitter
							(PlayerPrefs.GetString("shieldArray"), '_')[i]);
			}

			for (int i = 0; i < k; i++)
			{
				if (arrayShopShield[i] == shieldItemNumber)
				{
					b = false;
					break;
				}
			}

			if (b == true)
			{
				PlayerPrefs.SetString("shieldArray", str + shieldItemNumber + "_");
				PlayerPrefs.Save();
				Debug.Log("Shield has buy!");
			}
		}

		/// <summary>
		/// Купить оружие
		/// </summary>
		public void BuyWeapon()
		{
			bool b = true;
			string str = PlayerPrefs.GetString("weaponArray");
			int k = LibraryObjectsWorker.StringSplitter
							(PlayerPrefs.GetString("weaponArray"), '_').Length;
			List<int> arrayShopWeapon = new List<int>();
			for (int i = 0; i < k; i++)
			{
				arrayShopWeapon.Add(LibraryObjectsWorker.StringSplitter
							(PlayerPrefs.GetString("weaponArray"), '_')[i]);
			}

			for (int i = 0; i < k; i++)
			{
				if (arrayShopWeapon[i] == weaponItemNumber)
				{
					b = false;
					break;
				}
			}

			if (b == true)
			{
				PlayerPrefs.SetString("weaponArray", str + weaponItemNumber + "_");
				PlayerPrefs.Save();
				Debug.Log("Weapon has buy!");
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

		private void Start() // ____________start__________
		{
			SHP = GetComponent<Shop>();
			cuirassList = new List<PartArmoryInformation>();
			helmetList = new List<PartArmoryInformation>();
			shieldList = new List<PartArmoryInformation>();
			shieldArray = new GameObject[Resources.LoadAll("Prefabs/Armor/Shield").Length];
			cuirassArray = new GameObject[Resources.LoadAll("Prefabs/Armor/Cuirass").Length];
			helmetArray = new GameObject[Resources.LoadAll("Prefabs/Armor/Helmet").Length];

			weaponList = new List<Weapon>();
			weaponArray = new GameObject[Resources.LoadAll("Prefabs/Weapon").Length];
			weaponStats = new int[3];

			Timing.RunCoroutine(ShieldCorutine());
			Timing.RunCoroutine(HelmetCorutine());
			Timing.RunCoroutine(CuirassCorutine());
			Timing.RunCoroutine(WeaponCorutine());
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
					shieldArray[i] = gemGgamObj.GetComponent<LevelManager>().GetItemLevel(PlayerPrefs.GetInt("shield_" + i));
					shieldList.Add(shieldArray[i].GetComponent<PartArmoryInformation>());
					GameObject item = Instantiate(itemArmor);
					ArmorButton button = item.GetComponent<ArmorButton>();
					button.SetShop(SHP);
					button.SetNumber(i);
					button.ArmoryClassShop = shieldList[i].ArmoryType;
					button.Weight = shieldList[i].WeightArmory.ToString();
					button.SetName(shieldList[i].ArmoryName);
					button.SetArmor(shieldList[i].ArmoryValue.ToString());
					button.SetLogo(shieldList[i].ImageArm);
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
					cuirassArray[i] = gemGgamObj.GetComponent<LevelManager>().GetItemLevel(PlayerPrefs.GetInt("cuirass_" + i));
					cuirassList.Add(cuirassArray[i].GetComponent<PartArmoryInformation>());
					GameObject item = Instantiate(itemArmor);
					ArmorButton button = item.GetComponent<ArmorButton>();
					button.SetShop(SHP);
					button.SetNumber(i);
					button.ArmoryClassShop = cuirassList[i].ArmoryType;
					button.Weight = cuirassList[i].WeightArmory.ToString();
					button.SetName(cuirassList[i].ArmoryName);
					button.SetArmor(cuirassList[i].ArmoryValue.ToString());
					button.SetLogo(cuirassList[i].ImageArm);
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
				if (Resources.Load(helmetPrefix + i + helmetPostfix))
				{
					GameObject gemGgamObj = (GameObject)Resources.Load(helmetPrefix + i + helmetPostfix);
					helmetArray[i] = gemGgamObj.GetComponent<LevelManager>().GetItemLevel(PlayerPrefs.GetInt("helmet_" + i));
					helmetList.Add(helmetArray[i].GetComponent<PartArmoryInformation>());
					GameObject item = Instantiate(itemArmor);
					ArmorButton button = item.GetComponent<ArmorButton>();
					button.SetShop(SHP);
					button.SetNumber(i);
					button.ArmoryClassShop = helmetList[i].ArmoryType;
					button.Weight = helmetList[i].WeightArmory.ToString();
					button.SetName(helmetList[i].ArmoryName);
					button.SetArmor(helmetList[i].ArmoryValue.ToString());
					button.SetLogo(helmetList[i].ImageArm);
					item.transform.SetParent(helmetRepository.transform, false);
				}
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
			int k = Resources.LoadAll("Prefabs/Weapon").Length;

			for (int i = 0; i < k; i++)
			{
				if (Resources.Load(headPrefix + i.ToString()))
				{
					GameObject weaponGamObj = (GameObject)Resources.Load(headPrefix + i);
					weaponStats = LibraryObjectsWorker.StringSplitter
						(PlayerPrefs.GetString("weapon_" + i), '_');
					//  weaponStats[0] - уровень
					//  weaponStats[1] - тип камня (использовать как перечислитель)
					//  weaponStats[2] - сила камня (1 - 100)
					weaponArray[i] = weaponGamObj.GetComponent<LevelManager>().GetItemLevel(weaponStats[0]);
					weaponList.Add(weaponArray[i].GetComponent<Weapon>());
					GameObject item = Instantiate(itemWeapon);
					WeaponButton button = item.GetComponent<WeaponButton>();
					button.SetShop(SHP);
					button.SetNumber(i);
					button.SetName(weaponList[i].HeadName);
					button.SetLogo(weaponList[i].ItemImage);
					button.SetGemType((GemType)weaponStats[1]);
					item.transform.SetParent(weaponRepository.transform, false);

				}
			}

			scrollRectWeaponRepository =
				weaponRepository.transform.parent.GetComponent<ScrollRect>();
			yield return 0;
		}
	}
}
