using UnityEngine;
using System.Collections.Generic;
using MovementEffects;
using VotanUI;

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
		private GameObject gripRepository;
		[SerializeField]
		private GameObject headRepository;
		[SerializeField]
		private GameObject gemRepository;

 		[SerializeField]
		private GameObject itemGrip;
		[SerializeField]
		private GameObject itemHead;
		[SerializeField]
		private GameObject itemGem;

		[SerializeField]
		GameObject gripWindow;
		[SerializeField]
		GameObject headWindow;
		[SerializeField]
		GameObject gemWindow;

		private GameObject grip;
        private GameObject head;
        private GameObject gem;

		string headPostfix = "_H";
		string headPrefix = "Prefabs/Weapon/Head/";
		string gripPostfix = "_G";
		string gripPrefix = "Prefabs/Weapon/Grip/";
		string gemPostfix = "_Gm";
		string gemPrefix = "Prefabs/Weapon/Gem/";

		private List<Grip> gripList;
		private List<Head> headList;
		private List<Gem> gemList;
		[SerializeField]
		private int gripItemNumber;
		[SerializeField]
		private int headItemNumber;
		[SerializeField]
		private int gemItemNumber;
		[SerializeField]
		WeaponPrefabs WP;
		WeaponCraft WC;

		#endregion

		#region Свойства
		public void GripWindow()
		{
			headWindow.SetActive(false);
			gemWindow.SetActive(false);
			gripWindow.SetActive(true);
		}

		public void HeadWindow()
		{
			gemWindow.SetActive(false);
			gripWindow.SetActive(false);
			headWindow.SetActive(true);
		}

		public void GemWindow()
		{
			headWindow.SetActive(false);
			gripWindow.SetActive(false);
			gemWindow.SetActive(true);
		}

		public void SetGemItemNumber(int x)
		{
            MenuSoundManager.PlaySoundStatic(1);
			gemItemNumber = x;
		}
		public void SetGripItemNumber(int x)
		{
            MenuSoundManager.PlaySoundStatic(1);
            gripItemNumber = x;
		}

		public void SetHeadItemNumber(int x)
		{
            MenuSoundManager.PlaySoundStatic(1);
            headItemNumber = x;
		}

		public GameObject GetGripPrafab()
        {
            return grip;
        }

        public GameObject GetHeadPrafab()
        {
            return head;
        }

        public GameObject GetGemPrafab()
        {
            return gem;
        }
        #endregion

        public void PlayArena()
        {
            if (WP == null)
                WP = GameObject.Find("GetWeaponPrefabs").GetComponent<WeaponPrefabs>();

            WP.Grip = (GameObject)Resources.Load(gripPrefix + gripItemNumber + gripPostfix);
			WP.Head = (GameObject)Resources.Load(headPrefix + headItemNumber + headPostfix);
			WP.Gem = (GameObject)Resources.Load(gemPrefix + gemItemNumber + gemPostfix);
        }

		public void LoadPrefab()
		{

		}

		private void Awake() // ____________start__________
		{
			WC = GetComponent<WeaponCraft>();
			gripList = new List<Grip>();
			headList = new List<Head>();
			gemList = new List<Gem>();
			Timing.RunCoroutine(GripCorutine());
			Timing.RunCoroutine(HeadCorutine());
			Timing.RunCoroutine(GemCorutine());
			
		}

		private IEnumerator<float> GemCorutine()
		{
			int count = Resources.LoadAll("Prefabs/Weapon/Gem").Length;
			for (int i = 0; i < count; i++)
			{
				if (Resources.Load(gemPrefix + i.ToString() + gemPostfix))
				{
					GameObject gemGgamObj = (GameObject)Resources.Load(gemPrefix + i.ToString() + gemPostfix);
					gemList.Add(gemGgamObj.GetComponent<Gem>());
					GameObject item = Instantiate(itemGem);
					GemButton button = item.GetComponent<GemButton>();
					button.SetWeaponCraft(WC);
					button.SetNumber(i);
					button.SetName(gemList[i].GemName);
					button.SetGemPower(gemList[i].GemPower.ToString());
					button.SetGemType(gemList[i].DamageTypeGem);
					button.SetLogo (gemList[i].ItemImage);
					item.transform.SetParent(gemRepository.transform, false);
				}
			}
			yield return 0;
		}

		private IEnumerator<float> HeadCorutine()
		{
			int count = Resources.LoadAll("Prefabs/Weapon/Head").Length;
			for (int i = 0; i < count; i++)
			{
				if (Resources.Load(headPrefix + i.ToString() + headPostfix))
				{
					GameObject headGamObj = (GameObject)Resources.Load(headPrefix + i.ToString() + headPostfix);
					headList.Add(headGamObj.GetComponent<Head>());
					GameObject item = Instantiate(itemHead);
					HeadButton button = item.GetComponent<HeadButton>();
					button.SetWeaponCraft(WC);
					button.SetNumber(i);
					button.SetName(headList[i].HeadName);
					button.SetDamage(headList[i].DamageBase.ToString());
					button.SetWeight(headList[i].HeadWeight.ToString());
					button.SetSpinBous(headList[i].BonusSpinSpeedFromHead.ToString());
					button.SetLogo (headList[i].ItemImage);
					item.transform.SetParent(headRepository.transform, false);

				}
			}
			yield return 0;
		}

		private IEnumerator<float> GripCorutine()
		{
			int count = Resources.LoadAll("Prefabs/Weapon/Grip").Length;

			for (int i = 0; i < count; i++) // Колличество видов оружия. Пока не трогать
			{
				if (Resources.Load(gripPrefix + i.ToString() + gripPostfix))
				{
					GameObject gripGamObj = (GameObject)Resources.Load(gripPrefix + i.ToString() + gripPostfix); // загрузка префаба оружия
					gripList.Add(gripGamObj.GetComponent<Grip>()); // отправка компонента в лист
					GameObject item = Instantiate(itemGrip); // создание кнопки
					GripButton button = item.GetComponent<GripButton>(); // Снимаем компонент свойств кномпи с объекта
					button.SetWeaponCraft(WC);  // ниже задаем параметры, которые увидит игрок
					button.SetNumber(i);
					button.SetName(gripList[i].GripName);
					button.SetWeight(gripList[i].GripWeight.ToString());
					button.SetSpinBous(gripList[i].BonusSpinSpeedFromGrip.ToString());
					button.SetDefence(gripList[i].GripDefence.ToString());
					button.SetLogo (gripList[i].ItemImage);
					item.transform.SetParent(gripRepository.transform, false); // удочерям кнопку "листу" кнопок

				}
			}
			yield return 0;
		}
	}
}
