﻿using UnityEngine;
using System.Collections.Generic;
using MovementEffects;
using VotanUI;
using UnityEngine.UI;
using GameBehaviour;
using VotanInterfaces;
using VotanLibraries;
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
		private GameObject weaponRepository;
		[SerializeField]
		private GameObject itemRepository;
		[SerializeField]
		private GameObject skillsRepository;

		[SerializeField, Tooltip("кнопка оружия")]
		private GameObject itemWeapon;
		[SerializeField, Tooltip("кнопка скила")]
		private GameObject itemSkill;
		[SerializeField, Tooltip("кнопка итема")]
		private GameObject itemItm;

		[SerializeField]
		GameObject weaponWindow;
		[SerializeField]
		GameObject weaponUpadateButton; // Отображать вместе с weaponWindow
		[SerializeField]
		GameObject skillWindow;
		[SerializeField]
		GameObject itemWindow;

		private GameObject weapon;
		private GameObject weaponGamObj;
		private GameObject skillGamObj;
		private GameObject itemGamObj;


		string headPrefix = "Prefabs/Weapon/Weapon_";
		string skillPrefix = "Prefabs/Skills/Skill_";
		string itemPrefix = "Prefabs/Items/Item_";

		private List<ISkill> skillList;
		private List<IItem> itemList;
		private List<Weapon> weaponList;
		private GameObject[] weaponArray;
		private int[] weaponStats;

		[SerializeField, Tooltip("Закинуть сюда все зелья")]
		private List<GameObject> itemArray;

		private int weaponItemNumber;

		private int itemItemNumberOne;
		private int itemItemNumberTwo;
		private int itemItemNumberThree;

		private int skillItemNumberOne;
		private int skillItemNumberTwo;
		private int skillItemNumberThree;

		[SerializeField]
		WeaponPrefabs WP;
		[SerializeField]
		ItemSkillPrefabs ItemSkillPef;
		WeaponCraft WC;
		PlayerStats PStats;

        ScrollRect scrollRectWeaponRepository;
		ScrollRect scrollRectSkillRepository;
		ScrollRect scrollRectItemRepository;

		float intemNumbWeapon;
		float normPosWeapon;

		List<int> arrayBoughtWeapon;
		#endregion

		#region Свойства
		private void CloseAllWindows()
		{
			skillWindow.SetActive(false);
			itemWindow.SetActive(false);
			weaponWindow.SetActive(false);
			weaponUpadateButton.SetActive(false);

		}

		/// <summary>
		/// Вызывать для открытия окна с оружием
		/// </summary>
		public void WeaponWindow()
		{
			skillWindow.SetActive(false);
			itemWindow.SetActive(false);
			weaponWindow.SetActive(true);
			weaponUpadateButton.SetActive(true);

			PStats.WeaponPage();
			scrollRectWeaponRepository.horizontalNormalizedPosition = 0;
        }

		/// <summary>
		/// Вызывать для открытия окна со скилами
		/// </summary>
		public void SkillWindow()
		{
			skillWindow.SetActive(true);
			itemWindow.SetActive(false);
			weaponWindow.SetActive(false);
			weaponUpadateButton.SetActive(false);
			PStats.SkillPage();
			scrollRectWeaponRepository.horizontalNormalizedPosition = 0;
		}

		/// <summary>
		/// Вызывать для открытия окна с итемами
		/// </summary>
		public void ItemWindow()
		{
			skillWindow.SetActive(false);
			itemWindow.SetActive(true);
			weaponWindow.SetActive(false);
			weaponUpadateButton.SetActive(false);
			PStats.SkillPage();
			scrollRectWeaponRepository.horizontalNormalizedPosition = 0;
		}
		/// <summary>
		/// При экипировке элемента оружия, его характеристики
		/// отправляются в таблицу
		/// </summary>
		/// <param name="x"></param>
		public void SetWeaponItemNumber(int x)
		{
            MenuSoundManager.PlaySoundStatic(1);
            weaponItemNumber = x;
			PStats.HeadDamage = weaponList[x].DamageBase;
			PStats.CritChance = weaponList[x].CriticalChance;
		}

		public void SetSkillItemNumber(int numberButton)
		{
			if (skillItemNumberOne == -1)
			{
				MenuSoundManager.PlaySoundStatic(1);
				skillItemNumberOne = numberButton;
				PStats.SetSkillImg(skillList[numberButton].SkillImage.sprite, 0);
			}
			else if (skillItemNumberTwo == -1)
			{
				MenuSoundManager.PlaySoundStatic(1);
				skillItemNumberTwo = numberButton;
				PStats.SetSkillImg(skillList[numberButton].SkillImage.sprite, 1);
			}
			else if (skillItemNumberThree == -1)
			{
				MenuSoundManager.PlaySoundStatic(1);
				skillItemNumberThree = numberButton;
				PStats.SetSkillImg(skillList[numberButton].SkillImage.sprite, 2);
			}
			else
			{
				//
			}
		}

		public void SetItemNumber(int numberButton)
		{
			if (itemItemNumberOne == -1)
			{
				MenuSoundManager.PlaySoundStatic(1);
				itemItemNumberOne = numberButton;
				PStats.SetItemImg(itemList[numberButton].ItemImage.sprite, 0);
			}
			else if (itemItemNumberTwo == -1)
			{
				MenuSoundManager.PlaySoundStatic(1);
				itemItemNumberTwo = numberButton;
				PStats.SetItemImg(itemList[numberButton].ItemImage.sprite, 1);
			}
			else if (itemItemNumberThree == -1)
			{
				MenuSoundManager.PlaySoundStatic(1);
				itemItemNumberThree = numberButton;
				PStats.SetItemImg(itemList[numberButton].ItemImage.sprite, 2);
			}
			else
			{
				//
			}
		}

		/// <summary>
		/// Улучшает выбранное оружие
		/// </summary>
		public void UpdateWeapon()
		{
			weaponStats = LibraryObjectsWorker.StringSplitter(PlayerPrefs.GetString("weapon_" + weaponItemNumber), '_');
			if (weaponStats[0] < 3)
			{
				weaponStats[0] = weaponStats[0] + 1;
				PlayerPrefs.SetString("weapon_" + weaponItemNumber, 
					(weaponStats[0] + "_" + weaponStats[1] + "_" + weaponStats[2]).ToString());
				PlayerPrefs.Save();
			}
		}

		/// <summary>
		/// Вызывать для удаления умения из 1й ячейки 
		/// </summary>
		public void RemoveSkillOne()
		{
			skillItemNumberOne = -1;
			PStats.SetSkillImg(0);
		}

		/// <summary>
		/// Вызывать для удаления умения из 2й ячейки 
		/// </summary>
		public void RemoveSkillTwo()
		{
			skillItemNumberTwo = -1;
			PStats.SetSkillImg(1);
		}

		/// <summary>
		/// Вызывать для удаления умения из 3й ячейки 
		/// </summary>
		public void RemoveSkillThree()
		{
			skillItemNumberThree = -1;
			PStats.SetSkillImg(2);
		}

		/// <summary>
		///  Вызывать для удаления зелья из 1й ячейки 
		/// </summary>
		public void RemoveItemOne()
		{
			itemItemNumberOne = -1;
			PStats.SetItemImg(0);
		}

		/// <summary>
		///  Вызывать для удаления зелья из 2й ячейки 
		/// </summary>
		public void RemoveItemTwo()
		{
			itemItemNumberTwo = -1;
			PStats.SetItemImg(1);
		}

		/// <summary>
		/// Вызывать для удаления зелья из 3й ячейки 
		/// </summary>
		public void RemoveItemThree()
		{
			itemItemNumberThree = -1;
			PStats.SetItemImg(2);
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
			if (normPosWeapon != scrollRectWeaponRepository.horizontalNormalizedPosition)
			{
				if (weaponItemNumber != Mathf.Round(scrollRectWeaponRepository.horizontalNormalizedPosition * (weaponList.Count - 1)))
				{
					intemNumbWeapon = Mathf.Round(scrollRectWeaponRepository.horizontalNormalizedPosition * (weaponList.Count - 1));
				}
				else
				{
					intemNumbWeapon = weaponItemNumber;
				}

				PStats.NewHeadDamage = weaponList[(int)intemNumbWeapon].DamageBase;
				PStats.NewCritChance = weaponList[(int)intemNumbWeapon].CriticalChance;
			}
			normPosWeapon = scrollRectWeaponRepository.horizontalNormalizedPosition;
			PStats.NewStats();
		}
		#endregion

		/// <summary>
		/// Вызывать при запуске арены
		/// </summary>
		public void PlayArena()
        {
            if (WP == null)
                WP = GameObject.Find("GetPrefabs").GetComponent<WeaponPrefabs>();

			WP.Weapon = weaponArray[weaponItemNumber];
			if (skillItemNumberOne >= 0)
				ItemSkillPef.FirstSkill = (GameObject)Resources.Load(skillPrefix + skillItemNumberOne);
			if(skillItemNumberTwo >= 0)
				ItemSkillPef.SecondSkill = (GameObject)Resources.Load(skillPrefix + skillItemNumberTwo);
			if (skillItemNumberThree >= 0)
				ItemSkillPef.ThirdSkill = (GameObject)Resources.Load(skillPrefix + skillItemNumberThree);
			if (itemItemNumberOne >= 0)
				ItemSkillPef.FirstItem = itemArray[itemItemNumberOne];
			if (itemItemNumberTwo >= 0)
				ItemSkillPef.SecondItem = itemArray[itemItemNumberTwo];
			if (itemItemNumberThree >= 0)
				ItemSkillPef.ThirdItem = itemArray[itemItemNumberThree];
		}

		private void Awake() // ____________start__________
		{
			PStats = GetComponent<PlayerStats>();
			WC = GetComponent<WeaponCraft>();

			weaponList = new List<Weapon>();
			skillList = new List<ISkill>();
			itemList = new List<IItem>();
			weaponArray = new GameObject[Resources.LoadAll("Prefabs/Weapon").Length];
			weaponStats = new int[3];
			arrayBoughtWeapon = new List<int>();
			skillItemNumberOne = -1;
			skillItemNumberTwo = -1;
			skillItemNumberThree = -1;
			itemItemNumberOne = -1;
			itemItemNumberTwo = -1;
			itemItemNumberThree = -1;

			Timing.RunCoroutine(WeaponCorutine());
			Timing.RunCoroutine(SkillCorutine());
			Timing.RunCoroutine(ItemsCorutine());
		}

		private void FixedUpdate()
		{
			ChekScroll();
		}

		/// <summary>
		/// Запускать для отображения элементов оружия в ленте
		/// </summary>
		/// <returns></returns>
		private IEnumerator<float> WeaponCorutine()
		{
			int k = LibraryObjectsWorker.StringSplitter
							(PlayerPrefs.GetString("weaponArray"), '_').Length;
			for (int i = 0; i < k; i++)
			{
				arrayBoughtWeapon.Add(LibraryObjectsWorker.StringSplitter
							(PlayerPrefs.GetString("weaponArray"), '_')[i]);
			}

			for (int i = 0; i < k; i++)
			{
				if (Resources.Load(headPrefix + i.ToString()))
				{
					weaponGamObj =(GameObject) Resources.Load(headPrefix + arrayBoughtWeapon[i]);
                    weaponStats = LibraryObjectsWorker.StringSplitter
                        (PlayerPrefs.GetString("weapon_" + arrayBoughtWeapon[i]), '_');
					//  weaponStats[0] - уровень
					//  weaponStats[1] - тип камня (использовать как перечислитель)
					//  weaponStats[2] - сила камня (1 - 100)
					weaponArray[i] = weaponGamObj.GetComponent<LevelManager>().GetItemLevel(weaponStats[0]);
					weaponList.Add(weaponArray[i].GetComponent<Weapon>());
					GameObject item = Instantiate(itemWeapon);
					WeaponButton button = item.GetComponent<WeaponButton>();
					button.SetWeaponCraft(WC);
					button.SetNumber(i);
					button.SetName(weaponList[i].HeadName); 
					button.SetLogo (weaponList[i].ItemImage);
					button.SetGemType((GemType)weaponStats[1]);
					item.transform.SetParent(weaponRepository.transform, false);

				}
			}

            scrollRectWeaponRepository =
                weaponRepository.transform.parent.GetComponent<ScrollRect>();
            yield return 0;
		}

		private IEnumerator<float> SkillCorutine()
		{
			int count = Resources.LoadAll("Prefabs/Skills").Length;

			for (int i = 0; i < count; i++)
			{
				if (Resources.Load(skillPrefix + i.ToString()))
				{
                    GameObject skillGamObj = (GameObject)Resources.Load(skillPrefix + i.ToString());
					skillList.Add(skillGamObj.GetComponent<ISkill>());
					GameObject item = Instantiate(itemSkill);
					ItemOrSkillButton button = item.GetComponent<ItemOrSkillButton>();
					button.SetWeaponCraft(WC);
					button.SetNumber(i);
					button.NameSkill.text = skillList[i].SkillName;
					button.TutorialSkill.text = skillList[i].SkillTutorial;
					button.SetImage(skillList[i].SkillImage.sprite);
					item.transform.SetParent(skillsRepository.transform, false);

				}
			}
			scrollRectSkillRepository =
				skillsRepository.transform.parent.GetComponent<ScrollRect>();
			yield return 0;
		}

		private IEnumerator<float>ItemsCorutine()
		{
			int count = itemArray.Count;

			for (int i = 0; i < count; i++)
			{
                GameObject itemGamObj = itemArray[i];
				itemList.Add(itemGamObj.GetComponent<IItem>());
				GameObject item = Instantiate(itemItm);
				ItemOrSkillButton button = item.GetComponent<ItemOrSkillButton>();
				button.SetWeaponCraft(WC);
				button.SetNumber(i);
				button.NameSkill.text = itemList[i].ItemName;
				button.TutorialSkill.text = itemList[i].ItemTutorial;
				button.SetImage (itemList[i].ItemImage.sprite);
				item.transform.SetParent(itemRepository.transform, false);
			}
			scrollRectSkillRepository =
				skillsRepository.transform.parent.GetComponent<ScrollRect>();
			yield return 0;
		}
	}
}
