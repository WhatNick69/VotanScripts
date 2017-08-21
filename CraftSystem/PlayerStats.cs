using PlayerBehaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VotanLibraries;

namespace CraftSystem
{
	public class PlayerStats : MonoBehaviour {

		[SerializeField]
		Text leveTextl;
		[SerializeField]
		Text hpText;
		[SerializeField]
		Text armorText;
		[SerializeField]
		Text WeightText;
		[SerializeField]
		Text damageText;
		[SerializeField]
		Text gemText;
		[SerializeField]
		Text gemDamageText;
		[SerializeField]
		Text critChaceText;

		[SerializeField]
		Text newArmorText;
		[SerializeField]
		Text newWeightText;
		[SerializeField]
		Text newDamageText;
		[SerializeField]
		Text newGemText;
		[SerializeField]
		Text newGemDamageText;
		[SerializeField]
		Text newCritChaceText;

		[SerializeField]
		GameObject cuirass;
		[SerializeField]
		GameObject shield;
		[SerializeField]
		GameObject helmet;

		[SerializeField]
		GameObject weapon;

		[SerializeField]
		GameObject NewDamage;
		[SerializeField]
		GameObject NewArmor;
		[SerializeField]
		GameObject NewWeight;
		[SerializeField]
		GameObject NewGemType;
		[SerializeField]
		GameObject NewGemPuwer;
		[SerializeField]
		GameObject NewCrit;

		// Для наконечников
		float critChance;
		float headDamage;
		float newCritChance;
		float newHeadDamage;

		// Для камней
		GemType gemType;
		float gemPower;
		GemType newGemType;
		float newGemPower;

		// Для всей брони
		float cuirassArmor;
		float helmetArmor;
		float shieldArmor;
		float cuirassWeight;
		float helmetWeight;
		float shieldWeight;
		float newCuirassArmor;
		float newHelmetArmor;
		float newShieldArmor;
		float newCuirassWeight;
		float newHelmetWeight;
		float newShieldWeight;

		Vector4 red = new Vector4(255, 0, 0, 255);
		Vector4 green = new Vector4(0, 40, 0, 255);

		#region
		/// <summary>
		/// Создана для удобства, просто отключает все окна с итемами
		/// </summary>
		private void PredPageLoad()
		{
			NewDamage.SetActive(false);
			NewArmor.SetActive(false);
			NewGemType.SetActive(false);
			NewGemPuwer.SetActive(false);
			NewCrit.SetActive(false);
			NewWeight.SetActive(false);
		}

		/// <summary>
		/// Вызывать при открытие листа с оружия
		/// </summary>
		public void WeaponPage()
		{
			PredPageLoad();
			NewDamage.SetActive(true);
			NewCrit.SetActive(true);
			NewGemPuwer.SetActive(true);
			NewGemType.SetActive(true);
		}

		/// <summary>
		/// Вызывать при открытие листа с кирасами, щитами, шлемами
		/// </summary>
		public void ArmorPage()
		{
			PredPageLoad();
			NewArmor.SetActive(true);
			NewWeight.SetActive(true);
		}

		public float CuirassArmor
		{
			get
			{
				return cuirassArmor;
			}

			set
			{
				cuirassArmor = value;
			}
		}

		public float HelmetArmor
		{
			get
			{
				return helmetArmor;
			}

			set
			{
				helmetArmor = value;
			}
		}

		public float ShieldArmor
		{
			get
			{
				return shieldArmor;
			}

			set
			{
				shieldArmor = value;
			}
		}

		public float NewCuirassArmor
		{
			get
			{
				return newCuirassArmor;
			}

			set
			{
				newCuirassArmor = value;
			}
		}

		public float NewHelmetArmor
		{
			get
			{
				return newHelmetArmor;

			}

			set
			{
				newHelmetArmor = value;
			}
		}

		public float NewShieldArmor
		{
			get
			{
				return newShieldArmor;
			}

			set
			{
				newShieldArmor = value;
			}
		}

		public float HeadDamage
		{
			get
			{
				return headDamage;
			}

			set
			{
				headDamage = value;
			}
		}

		public float NewHeadDamage
		{
			get
			{
				return newHeadDamage;
			}

			set
			{
				newHeadDamage = value;
			}
		}

		public float GemPower
		{
			get
			{
				return gemPower;
			}

			set
			{
				gemPower = value;
			}
		}

		public float NewGemPower
		{
			get
			{
				return newGemPower;
			}

			set
			{
				newGemPower = value;
			}
		}

		public float CuirassWeight
		{
			get
			{
				return cuirassWeight;
			}

			set
			{
				cuirassWeight = value;
			}
		}

		public float HelmetWeight
		{
			get
			{
				return helmetWeight;
			}

			set
			{
				helmetWeight = value;
			}
		}

		public float ShieldWeight
		{
			get
			{
				return shieldWeight;
			}

			set
			{
				shieldWeight = value;
			}
		}

		public float NewCuirassWeight
		{
			get
			{
				return newCuirassWeight;
			}

			set
			{
				newCuirassWeight = value;
			}
		}

		public float NewHelmetWeight
		{
			get
			{
				return newHelmetWeight;
			}

			set
			{
				newHelmetWeight = value;
			}
		}

		public float NewShieldWeight
		{
			get
			{
				return newShieldWeight;
			}

			set
			{
				newShieldWeight = value;
			}
		}

		public float CritChance
		{
			get
			{
				return critChance;
			}

			set
			{
				critChance = value;
			}
		}

		public float NewCritChance
		{
			get
			{
				return newCritChance;
			}

			set
			{
				newCritChance = value;
			}
		}
		#endregion

		/// <summary>
		/// Обновляет значения в таблице характеристик 
		/// оружия и брони
		/// </summary>
		public void NewStats()
		{
			leveTextl.text = PlayerPrefs.GetInt("level").ToString();
			hpText.text = "HP: " + ((PlayerPrefs.GetInt("level") + 10) * 2);
			armorText.text = (cuirassArmor + shieldArmor + helmetArmor).ToString();
			WeightText.text = (cuirassWeight + helmetWeight + shieldWeight).ToString();
			damageText.text = headDamage.ToString();
			gemDamageText.text = GemPower.ToString().ToString();
			critChaceText.text = CritChance.ToString();

			// Crit Chance
			if (newCritChance >= critChance && NewCrit.activeInHierarchy)
			{
				newCritChaceText.text = "+(" + Mathf.Abs(newCritChance - critChance) + ")";
				newCritChaceText.color = green;
			}
			else
			{
				newCritChaceText.text = "-(" + Mathf.Abs(newCritChance - critChance) + ")";
				newCritChaceText.color = red;
			}

			// Cuiras Weight
			if (newCuirassWeight >= cuirassWeight && cuirass.activeInHierarchy)
			{
				newWeightText.text = "+(" + Mathf.Abs(newCuirassWeight - cuirassWeight) + ")";
				newWeightText.color = green;
			}
			else if (cuirass.activeInHierarchy)
			{
				newWeightText.text = "-(" + Mathf.Abs(newCuirassWeight - cuirassWeight) + ")";
				newWeightText.color = red;
			}

			// Helmet Weight
			if (newHelmetWeight >= helmetWeight && helmet.activeInHierarchy)
			{
				newWeightText.text = "+(" + Mathf.Abs(newHelmetWeight - helmetWeight) + ")";
				newWeightText.color = green;
			}
			else if (helmet.activeInHierarchy)
			{
				newWeightText.text = "-(" + Mathf.Abs(newHelmetWeight - helmetWeight) + ")";
				newWeightText.color = red;
			}

			// Shield Weight
			if (newShieldWeight >= shieldWeight && shield.activeInHierarchy)
			{
				newWeightText.text = "+(" + Mathf.Abs(newShieldWeight - shieldWeight) + ")";
				newWeightText.color = green;
			}
			else if (shield.activeInHierarchy)
			{
				newWeightText.text = "-(" + Mathf.Abs(newShieldWeight - shieldWeight) + ")";
				newWeightText.color = red;
			}

			// Head Damage
			if (newHeadDamage >= headDamage && NewDamage.activeInHierarchy)
			{
				newDamageText.text = "+(" + Mathf.Abs(newHeadDamage - headDamage).ToString() + ")";
				newDamageText.color = green;
			}
			else if (NewDamage.activeInHierarchy)
			{
				newDamageText.text = "-(" + Mathf.Abs(newHeadDamage - headDamage).ToString() + ")";
				newDamageText.color = red;
			}
			
			// Cuirass Armor
			if (newCuirassArmor >= cuirassArmor && cuirass.activeInHierarchy)
			{
				newArmorText.text = "+(" + Mathf.Abs(newCuirassArmor - cuirassArmor) + ")";
				newArmorText.color = green;
			}
			else if (cuirass.activeInHierarchy)
			{
				newArmorText.text = "-(" + Mathf.Abs(newCuirassArmor - cuirassArmor) + ")";
				newArmorText.color = red;
			}
			
			// Helmet Armor
			if (newHelmetArmor >= helmetArmor && helmet.activeInHierarchy)
			{
				newArmorText.text = "+(" + Mathf.Abs(newHelmetArmor - helmetArmor) + ")";
				newArmorText.color = green;
			}
			else if (helmet.activeInHierarchy)
			{
				newArmorText.text = "-(" + Mathf.Abs(newHelmetArmor - helmetArmor) + ")";
				newArmorText.color = red;
			}
			
			// Shield Armor
			if (newShieldArmor >= shieldArmor && shield.activeInHierarchy)
			{
				newArmorText.text = "+(" + Mathf.Abs(newShieldArmor - shieldArmor) + ")";
				newArmorText.color = green;
			}
			else if (shield.activeInHierarchy)
			{
				newArmorText.text = "-(" + Mathf.Abs(newShieldArmor - shieldArmor) + ")";
				newArmorText.color = red;
			}
		}
	}
}