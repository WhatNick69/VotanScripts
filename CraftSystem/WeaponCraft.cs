using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using MovementEffects;
using System;
using PlayerBehaviour;

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
		private Text gripText;
		[SerializeField]
		private Text headText;
		[SerializeField]
		private Text gemText;

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
		public void SetGemItemNumber(int x)
		{
			gemItemNumber = x;
		}
		public void SetGripItemNumber(int x)
		{
			gripItemNumber = x;
		}

		public void SetHeadItemNumber(int x)
		{
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
            WP.Grip = (GameObject)Resources.Load(gripPrefix + gripItemNumber + gripPostfix);
			WP.Head = (GameObject)Resources.Load(headPrefix + headItemNumber + headPostfix);
			WP.Gem = (GameObject)Resources.Load(gemPrefix + gemItemNumber + gemPostfix);
        }

		public void LoadPrefab()
		{

		}


		private void Start() // ____________start__________
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
					GemButton GmM = item.GetComponent<GemButton>();
					GmM.SetWeaponCraft(WC);
					GmM.SetNumber(i);
					GmM.SetName(gemList[i].GemName);
					GmM.SetGemPower(gemList[i].GemPower.ToString());
					GmM.SetGemType(gemList[i].DamageTypeGem);
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
					HeadButton HB = item.GetComponent<HeadButton>();
					HB.SetWeaponCraft(WC);
					HB.SetNumber(i);
					HB.SetName(headList[i].HeadName);
					HB.SetDamage(headList[i].DamageBase.ToString());
					HB.SetWeight(headList[i].HeadWeight.ToString());
					HB.SetSpinBous(headList[i].BonusSpinSpeedFromHead.ToString());
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
					GripButton GB = item.GetComponent<GripButton>(); // Снимаем компонент свойств кномпи с объекта
					GB.SetWeaponCraft(WC);  // ниже задаем параметры, которые увидит игрок
					GB.SetNumber(i);
					GB.SetName(gripList[i].GripName);
					GB.SetWeight(gripList[i].GripWeight.ToString());
					GB.SetSpinBous(gripList[i].BonusSpinSpeedFromGrip.ToString());
					GB.SetDefence(gripList[i].GripDefence.ToString());
					item.transform.SetParent(gripRepository.transform, false); // удочерям кнопку "листу" кнопок

				}
			}
			yield return 0;
		}
	}
}
