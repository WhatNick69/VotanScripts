using MovementEffects;
using System.Collections.Generic;
using UnityEngine;
using PlayerBehaviour;

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
		public void SetCuirassItemNumber(int x)
		{
			cuirassItemNumber = x;
		}
		public void SetShieldItemNumber(int x)
		{
			shieldItemNumber = x;
		}

		public void SetHelmetItemNumber(int x)
		{
			helmetItemNumber = x;
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
					button.SetArmorClass(ArmoryClass.Shield);
					button.SetName(shieldList[i].ArmoryName);
					button.SetArmor(shieldList[i].ArmoryValue.ToString());
					button.SetLogo (shieldList[i].ImageArm);
					item.transform.SetParent(shieldRepository.transform, false);
				}
			}
			yield return 0;
		}

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
					button.SetArmorClass(ArmoryClass.Cuirass);
					button.SetName(cuirassList[i].ArmoryName);
					button.SetArmor(cuirassList[i].ArmoryValue.ToString());
					button.SetLogo (cuirassList[i].ImageArm);
					item.transform.SetParent(cuirassRepository.transform, false);
				}
			}
			yield return 0;
		}

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
					button.SetArmorClass(ArmoryClass.Helmet);
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
