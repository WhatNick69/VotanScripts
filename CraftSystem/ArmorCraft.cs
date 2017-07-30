using MovementEffects;
using System.Collections.Generic;
using UnityEngine;
using PlayerBehaviour;
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
		[SerializeField]
		ArmorPrefabs AP;
		ArmorCraft AC;
		#endregion

		#region Свойства
		/// <summary>
		/// Вызывает окно с лнтой шлемов
		/// </summary>
		public void HelmetWindow()
		{
			shieldWindow.SetActive(false);
			cuirassWindow.SetActive(false);
			helmetWindow.SetActive(true);
		}

		/// <summary>
		/// Вызывает окно с лнтой кирас
		/// </summary>
		public void CuirassWindow()
		{
			shieldWindow.SetActive(false);
			helmetWindow.SetActive(false);
			cuirassWindow.SetActive(true);
		}

		/// <summary>
		/// Вызывает окно с лнтой щитов
		/// </summary>
		public void ShieldWindow()
		{
			helmetWindow.SetActive(false);
			cuirassWindow.SetActive(false);
			shieldWindow.SetActive(true);
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
                AP = GameObject.Find("GetArmorPrefabs").GetComponent<ArmorPrefabs>();
	        
            AP.Cuirass = (GameObject)Resources.Load(cuirassPrefix + cuirassItemNumber + cuirassPostfix);
			AP.Helmet = (GameObject)Resources.Load(helmetPrefix + helmetItemNumber + helmetPostfix);
			AP.Shield = (GameObject)Resources.Load(shieldPrefix + shieldItemNumber + shieldPostfix);
		}

		private void Start() // ____________start__________
		{
			AC = gameObject.GetComponent<ArmorCraft>();
			cuirassList = new List<PartArmoryInformation>();
			helmetList = new List<PartArmoryInformation>();
			shieldList = new List<PartArmoryInformation>();
			Timing.RunCoroutine(ShieldCorutine());
			Timing.RunCoroutine(HelmetCorutine());
			Timing.RunCoroutine(CuirassCorutine());

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
					button.SetName(shieldList[i].ArmoryName);
					button.SetArmor(shieldList[i].ArmoryValue.ToString());
					button.SetLogo (shieldList[i].ImageArm);
					item.transform.SetParent(shieldRepository.transform, false);
				}
			}
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
					button.SetName(cuirassList[i].ArmoryName);
					button.SetArmor(cuirassList[i].ArmoryValue.ToString());
					button.SetLogo (cuirassList[i].ImageArm);
					item.transform.SetParent(cuirassRepository.transform, false);
				}
			}
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
					button.SetName(helmetList[i].ArmoryName);
					button.SetArmor(helmetList[i].ArmoryValue.ToString());
					button.SetLogo (helmetList[i].ImageArm);
					item.transform.SetParent(helmetRepository.transform, false);
				}
			}
			yield return 0;
		}
	}
}
