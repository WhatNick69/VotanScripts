using UnityEngine;
using System.Collections.Generic;
using MovementEffects;
using VotanUI;
using UnityEngine.UI;
using GameBehaviour;
using VotanInterfaces;

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
		[SerializeField]
		private GameObject itemWeapon;
		[SerializeField]
		private GameObject itemSkill;
		[SerializeField]
		private GameObject itemItm;
		[SerializeField]
		GameObject weaponWindow;
		[SerializeField]
		GameObject skillWindow;
		[SerializeField]
		GameObject itemWindow;
		private GameObject weapon;
		GameObject weaponGamObj;
		GameObject skillGamObj;
		GameObject itemGamObj;


		string headPrefix = "Prefabs/Weapon/Weapon_";
		string skillPrefix = "Prefabs/Skills/Skill_";
		string itemPrefix = "Prefabs/Items/Item_";

		private List<ISkill> skillList;
		private List<IItem> itemList;
		private List<Weapon> weaponList;

		
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
		#endregion

		#region Свойства
		/// <summary>
		/// Вызывать для открытия окна с наконечниками
		/// </summary>
		public void WeaponWindow()
		{
			skillWindow.SetActive(false);
			itemWindow.SetActive(false);
			weaponWindow.SetActive(true);
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
			}
			else if (skillItemNumberTwo == -1)
			{
				MenuSoundManager.PlaySoundStatic(1);
				skillItemNumberTwo = numberButton;
			}
			else if (skillItemNumberThree == -1)
			{
				MenuSoundManager.PlaySoundStatic(1);
				skillItemNumberThree = numberButton;
			}
			else
			{
				//
			}
		}

		/// <summary>
		/// Вызывать для удаления умения из 1й ячейки 
		/// </summary>
		public void RemoveSkillOne()
		{
			skillItemNumberOne = -1;
		}

		/// <summary>
		/// Вызывать для удаления умения из 2й ячейки 
		/// </summary>
		public void RemoveSkillTwo()
		{
			skillItemNumberTwo = -1;
		}

		/// <summary>
		/// Вызывать для удаления умения из 3й ячейки 
		/// </summary>
		public void RemoveSkillThree()
		{
			skillItemNumberThree = -1;
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

			WP.Weapon = (GameObject)Resources.Load(headPrefix + weaponItemNumber);
			ItemSkillPef.FirstSkill = (GameObject)Resources.Load(skillPrefix + skillItemNumberOne);
			ItemSkillPef.SecondSkill = (GameObject)Resources.Load(skillPrefix + skillItemNumberTwo);
			ItemSkillPef.ThirdSkill = (GameObject)Resources.Load(skillPrefix + skillItemNumberThree);
		}

		private void Awake() // ____________start__________
		{
			PStats = GetComponent<PlayerStats>();
			WC = GetComponent<WeaponCraft>();

			weaponList = new List<Weapon>();
			skillList = new List<ISkill>();
			itemList = new List<IItem>();

			skillItemNumberOne = -1;
			skillItemNumberTwo = -1;
			skillItemNumberThree = -1;

			Timing.RunCoroutine(WeaponCorutine());
			Timing.RunCoroutine(SkillCorutine());
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
			int count = Resources.LoadAll("Prefabs/Weapon").Length;

			for (int i = 0; i < count; i++)
			{
				if (Resources.Load(headPrefix + i.ToString()))
				{
					weaponGamObj = (GameObject)Resources.Load(headPrefix + i.ToString());
					weaponList.Add(weaponGamObj.GetComponent<Weapon>());
					GameObject item = Instantiate(itemWeapon);
					WeaponButton button = item.GetComponent<WeaponButton>();
					button.SetWeaponCraft(WC);
					button.SetNumber(i);
					button.SetName(weaponList[i].HeadName);
					button.SetLogo (weaponList[i].ItemImage);
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
					
					item.transform.SetParent(skillsRepository.transform, false);

				}
			}
			scrollRectSkillRepository =
				skillsRepository.transform.parent.GetComponent<ScrollRect>();
			yield return 0;
		}

		private IEnumerator<float>ItemsCorutine()
		{
			int count = Resources.LoadAll("Prefabs/Item").Length;

			for (int i = 0; i < count; i++)
			{
				if (Resources.Load(itemPrefix + i.ToString()))
				{
					GameObject itemGamObj = (GameObject)Resources.Load(itemPrefix + i.ToString());
					itemList.Add(itemGamObj.GetComponent<IItem>());
					GameObject item = Instantiate(itemItm);
					ItemOrSkillButton button = item.GetComponent<ItemOrSkillButton>();
					button.SetWeaponCraft(WC);
					button.SetNumber(i);
					button.NameSkill.text = skillList[i].SkillName;
					button.TutorialSkill.text = skillList[i].SkillTutorial;

					item.transform.SetParent(skillsRepository.transform, false);

				}
			}
			scrollRectSkillRepository =
				skillsRepository.transform.parent.GetComponent<ScrollRect>();
			yield return 0;
		}
	}
}
