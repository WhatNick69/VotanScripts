﻿using UnityEngine;
using System.Collections.Generic;
using MovementEffects;
using VotanUI;
using UnityEngine.UI;

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
		PlayerStats PStats;

		ScrollRect scrollRectGripRepository;
        ScrollRect scrollRectGemRepository;
        ScrollRect scrollRectHeadRepository;

		float intemNumbGrip;
		float intemNumbHead;
		float intemNumbGem;
		float normPosGrip;
		float normPosHead;
		float normPosGem;
		#endregion

		#region Свойства
		/// <summary>
		/// Вызывать для открытия окна с рукоятями
		/// </summary>
		public void GripWindow()
		{
			headWindow.SetActive(false);
			gemWindow.SetActive(false);
			gripWindow.SetActive(true);
            scrollRectGripRepository.horizontalNormalizedPosition = 0;
		}

		/// <summary>
		/// Вызывать для открытия окна с наконечниками
		/// </summary>
		public void HeadWindow()
		{
			gemWindow.SetActive(false);
			gripWindow.SetActive(false);
			headWindow.SetActive(true);
            scrollRectHeadRepository.horizontalNormalizedPosition = 0;
        }

		/// <summary>
		/// Вызывать для открытия окна с камнями
		/// </summary>
		public void GemWindow()
		{
			headWindow.SetActive(false);
			gripWindow.SetActive(false);
			gemWindow.SetActive(true);
            scrollRectGemRepository.horizontalNormalizedPosition = 0;
        }

		/// <summary>
		/// При экипировке элемента оружия, его характеристики
		/// отправляются в таблицу
		/// </summary>
		/// <param name="x"></param>
		public void SetGemItemNumber(int x)
		{
            MenuSoundManager.PlaySoundStatic(1);
			gemItemNumber = x;
			PStats.GemPower = gemList[x].GemPower;
			PStats.GemType = gemList[x].DamageTypeGem.ToString();
		}

		/// <summary>
		/// При экипировке элемента оружия, его характеристики
		/// отправляются в таблицу
		/// </summary>
		/// <param name="x"></param>
		public void SetGripItemNumber(int x)
		{
            MenuSoundManager.PlaySoundStatic(1);
            gripItemNumber = x;
		}

		/// <summary>
		/// При экипировке элемента оружия, его характеристики
		/// отправляются в таблицу
		/// </summary>
		/// <param name="x"></param>
		public void SetHeadItemNumber(int x)
		{
            MenuSoundManager.PlaySoundStatic(1);
            headItemNumber = x;
			PStats.HeadDamage = headList[x].DamageBase;
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

		/// <summary>
		/// 1. Двинул ли игрок ленту с элементаи оружия
		/// 2. Сменился элемент на следующий или нет? Если да, то меняет номер элемента
		/// кторый отправится в таблицу с характеристиками
		/// 
		/// - После этого, в таблицу отправляются значения элементов, которые находятся
		/// по центру окна прокрутки (определяется предыдущими проверками)
		/// - В самом кенце сохраняется позиция ленты, для проверки в следющем кадре 1го условия
		/// </summary>
		private void ChekScroll()
		{
			if (normPosGrip != scrollRectGripRepository.horizontalNormalizedPosition ||
				normPosHead != scrollRectHeadRepository.horizontalNormalizedPosition||
				normPosGem != scrollRectGemRepository.horizontalNormalizedPosition)
			{
				if (gripItemNumber != Mathf.Round(scrollRectGripRepository.horizontalNormalizedPosition * (gripList.Count - 1)) ||
					headItemNumber != Mathf.Round(scrollRectHeadRepository.horizontalNormalizedPosition * (headList.Count - 1)) ||
					gemItemNumber != Mathf.Round(scrollRectGemRepository.horizontalNormalizedPosition * (gemList.Count - 1)))
				{
					intemNumbHead = Mathf.Round(scrollRectHeadRepository.horizontalNormalizedPosition * (headList.Count - 1));
					intemNumbGrip = Mathf.Round(scrollRectGripRepository.horizontalNormalizedPosition * (gripList.Count - 1));
					intemNumbGem = Mathf.Round(scrollRectGemRepository.horizontalNormalizedPosition * (gemList.Count - 1));
				}
				else
				{
					intemNumbHead = headItemNumber;
					intemNumbGrip = gripItemNumber;
					intemNumbGem = gemItemNumber;
				}
				PStats.NewHeadDamage = headList[(int)intemNumbHead].DamageBase;

				PStats.NewGemPower = gemList[(int)intemNumbGem].GemPower;
				PStats.NewGemType = gemList[(int)intemNumbGem].DamageTypeGem.ToString();
			}
			normPosGrip = scrollRectGripRepository.horizontalNormalizedPosition;
			normPosHead = scrollRectHeadRepository.horizontalNormalizedPosition;
			normPosGem = scrollRectGemRepository.horizontalNormalizedPosition;
			PStats.NewStats();
		}
		#endregion

		public void PlayArena()
        {
            if (WP == null)
                WP = GameObject.Find("GetPrefabs").GetComponent<WeaponPrefabs>();

            WP.Grip = (GameObject)Resources.Load(gripPrefix + gripItemNumber + gripPostfix);
			WP.Head = (GameObject)Resources.Load(headPrefix + headItemNumber + headPostfix);
			WP.Gem = (GameObject)Resources.Load(gemPrefix + gemItemNumber + gemPostfix);
        }

		private void Awake() // ____________start__________
		{
			PStats = GetComponent<PlayerStats>();
			WC = GetComponent<WeaponCraft>();
			gripList = new List<Grip>();
			headList = new List<Head>();
			gemList = new List<Gem>();

			Timing.RunCoroutine(GripCorutine());
			Timing.RunCoroutine(HeadCorutine());
			Timing.RunCoroutine(GemCorutine());
		}

		private void FixedUpdate()
		{
			ChekScroll();
		}

		/// <summary>
		/// Запускать для отображения элементов оружия в ленте
		/// </summary>
		/// <returns></returns>
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
					button.SetNumber(i); //
					button.SetName(gripList[i].GripName); //
					button.SetLogo(gripList[i].ItemImage); //
					item.transform.SetParent(gripRepository.transform, false); // удочерям кнопку "листу" кнопок

				}
			}
			scrollRectGripRepository =
				gripRepository.transform.parent.GetComponent<ScrollRect>();
			yield return 0;
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
            scrollRectGemRepository =
                gemRepository.transform.parent.GetComponent<ScrollRect>();
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
					button.SetLogo (headList[i].ItemImage);
					item.transform.SetParent(headRepository.transform, false);

				}
			}
            scrollRectHeadRepository =
                headRepository.transform.parent.GetComponent<ScrollRect>();
            yield return 0;
		}
	}
}
