using UnityEngine;
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
		private GameObject weaponRepository;
		[SerializeField]
		private GameObject itemWeapon;
		[SerializeField]
		GameObject weaponWindow;
        private GameObject weapon;

		string headPostfix = "_W";
		string headPrefix = "Prefabs/Weapon/";

		private List<Weapon> weaponList;
		[SerializeField]
		private int weaponItemNumber;
		[SerializeField]
		WeaponPrefabs WP;
		WeaponCraft WC;
		PlayerStats PStats;

        ScrollRect scrollRectWeaponRepository;

		float intemNumbWeapon;
		float normPosWeapon;
		#endregion

		#region Свойства
		/// <summary>
		/// Вызывать для открытия окна с наконечниками
		/// </summary>
		public void HeadWindow()
		{
			weaponWindow.SetActive(true);
            scrollRectWeaponRepository.horizontalNormalizedPosition = 0;
        }
		/// <summary>
		/// При экипировке элемента оружия, его характеристики
		/// отправляются в таблицу
		/// </summary>
		/// <param name="x"></param>
		public void SetHeadItemNumber(int x)
		{
            MenuSoundManager.PlaySoundStatic(1);
            weaponItemNumber = x;
			PStats.HeadDamage = weaponList[x].DamageBase;
			PStats.CritChance = weaponList[x].CriticalChance;
		}

        public GameObject GetWeaponPrafab()
        {
            return weapon;
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

		public void PlayArena()
        {
            if (WP == null)
                WP = GameObject.Find("GetPrefabs").GetComponent<WeaponPrefabs>();

			WP.Weapon = (GameObject)Resources.Load(headPrefix + weaponItemNumber + headPostfix);
        }

		private void Awake() // ____________start__________
		{
			PStats = GetComponent<PlayerStats>();
			WC = GetComponent<WeaponCraft>();
			weaponList = new List<Weapon>();

			Timing.RunCoroutine(HeadCorutine());
		}

		private void FixedUpdate()
		{
			ChekScroll();
		}

		/// <summary>
		/// Запускать для отображения элементов оружия в ленте
		/// </summary>
		/// <returns></returns>
		private IEnumerator<float> HeadCorutine()
		{
			int count = Resources.LoadAll("Prefabs/Weapon").Length;

			for (int i = 0; i < count; i++)
			{
				if (Resources.Load(headPrefix + i.ToString() + headPostfix))
				{
					GameObject headGamObj = (GameObject)Resources.Load(headPrefix + i.ToString() + headPostfix);
					weaponList.Add(headGamObj.GetComponent<Weapon>());
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
	}
}
