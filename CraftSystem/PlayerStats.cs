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
		Text damageText;
		[SerializeField]
		Text blockText;
		[SerializeField]
		Text gemText;
		[SerializeField]
		Text gemDamageText;
		[SerializeField]
		Text critChaceText;

		[SerializeField]
		Text newArmor;
		[SerializeField]
		Text newDamage;
		[SerializeField]
		Text newBlock;
		[SerializeField]
		Text newGem;
		[SerializeField]
		Text newGemDamage;
		[SerializeField]
		Text newCritChace;

		[SerializeField]
		GameObject cuirass;
		[SerializeField]
		GameObject shield;
		[SerializeField]
		GameObject helmet;

		[SerializeField]
		GameObject grip;
		[SerializeField]
		GameObject head;
		[SerializeField]
		GameObject gem;

		[SerializeField]
		GameObject damage;
		[SerializeField]
		GameObject armor;
		[SerializeField]
		GameObject block;
		[SerializeField]
		GameObject gemTape;
		[SerializeField]
		GameObject gemPuwer;
		[SerializeField]
		GameObject crit;

		// Для рукоятей
		float gripWeight;
		float gripDefence;
		float newGripWeight;
		float newGripDefence;

		// Для наконечников
		float headWeight;
		float headDamage;
		float newHeadWeight;
		float newHeadDamage;

		// Для камней
		string gemType;
		float gemPower;
		string newGemType;
		float newGemPower;

		// Для всей брони
		float cuirassArmor;
		float helmetArmor;
		float shieldArmor;
		float newCuirassArmor;
		float newHelmetArmor;
		float newShieldArmor;

		Vector4 red = new Vector4(255, 0, 0, 255);
		Vector4 green = new Vector4(0, 10, 0, 255);

		#region
		/// <summary>
		/// Создана для удобства, просто отключает все окна с итемами
		/// </summary>
		private void PredPageLoad()
		{
			damage.SetActive(false);
			armor.SetActive(false);
			block.SetActive(false);
			gemTape.SetActive(false);
			gemPuwer.SetActive(false);
			crit.SetActive(false);
		}

		/// <summary>
		/// Вызывать при открытие листа с рукоятями
		/// </summary>
		public void GripPage()
		{
			PredPageLoad();
			block.SetActive(true);
		}

		/// <summary>
		/// Вызывать при открытие листа с наконечниками
		/// </summary>
		public void HeadPage()
		{
			PredPageLoad();
			damage.SetActive(true);
		}

		/// <summary>
		/// Вызывать при открытие листа с камнями
		/// </summary>
		public void GemPage()
		{
			PredPageLoad();
			gemTape.SetActive(true);
			gemPuwer.SetActive(true);
		}

		/// <summary>
		/// Вызывать при открытие листа с кирасами, щитами, шлемами
		/// </summary>
		public void ArmorPage()
		{
			PredPageLoad();
			armor.SetActive(true);
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

		public float GripWeight
		{
			get
			{
				return gripWeight;
			}

			set
			{
				gripWeight = value;
			}
		}

		public float GripDefence
		{
			get
			{
				return gripDefence;
			}

			set
			{
				gripDefence = value;
			}
		}

		public float NewGripWeight
		{
			get
			{
				return newGripWeight;
			}

			set
			{
				newGripWeight = value;
			}
		}

		public float NewGripDefence
		{
			get
			{
				return newGripDefence;
			}

			set
			{
				newGripDefence = value;
			}
		}

		public float HeadWeight
		{
			get
			{
				return headWeight;
			}

			set
			{
				headWeight = value;
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

		public float NewHeadWeight
		{
			get
			{
				return newHeadWeight;
			}

			set
			{
				newHeadWeight = value;
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

		public string GemType
		{
			get
			{
				return gemType;
			}

			set
			{
				gemType = value;
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

		public string NewGemType
		{
			get
			{
				return newGemType;
			}

			set
			{
				newGemType = value;
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
		#endregion

		/// <summary>
		/// Обновляет значения в таблице характеристик 
		/// оружия и брони
		/// </summary>
		public void NewStats()
		{
			leveTextl.text = "level: " + PlayerPrefs.GetInt("level");
			hpText.text = "HP: " + ((PlayerPrefs.GetInt("level") + 10) * 2);
			armorText.text = "Armor: " + (cuirassArmor + shieldArmor + helmetArmor);
			damageText.text = "Damage: " + LibraryStaticFunctions.TotalDamage(headDamage, headWeight + gripWeight);
			blockText.text = "Block: " + gripDefence;
			gemText.text = "Gem Type: " + GemType;
			gemDamageText.text = "Gem Power:" + GemPower.ToString();
			critChaceText.text = "Crit Chace: ";

			// Gem Power
			if (newGemPower >= GemPower && gem.activeInHierarchy)
			{
				newGemDamage.text = "+(" + Mathf.Abs(newGemPower - GemPower).ToString() + ")";
				newGemDamage.color = green;
			}
			else if (gem.activeInHierarchy)
			{
				newGemDamage.text = "-(" + Mathf.Abs(newGemPower - GemPower).ToString() + ")";
				newGemDamage.color = red;
			}
			
			// Head Weight
			if (newHeadDamage >= headDamage && head.activeInHierarchy)
			{
                newDamage.text = "+(" + Mathf.Abs(newHeadDamage - headDamage).ToString() + ")";
                newDamage.color = green;
			}
			else if (head.activeInHierarchy)
			{
                newDamage.text = "-(" + Mathf.Abs(newHeadDamage - headDamage).ToString() + ")";
                newDamage.color = red;
			}
			
			// Grip Defence
			if (newGripDefence >= gripDefence && grip.activeInHierarchy)
			{
				newBlock.text = "+(" + Mathf.Abs(newGripDefence - gripDefence) + ")";
				newBlock.color = green;
			}
			else if (grip.activeInHierarchy)
			{
				newBlock.text = "-(" + Mathf.Abs(newGripDefence - gripDefence) + ")";
				newBlock.color = red;
			}
			
			// Cuirass Armor
			if (newCuirassArmor >= cuirassArmor && cuirass.activeInHierarchy)
			{
				newArmor.text = "+(" + Mathf.Abs(newCuirassArmor - cuirassArmor) + ")";
				newArmor.color = green;
			}
			else if (cuirass.activeInHierarchy)
			{
				newArmor.text = "-(" + Mathf.Abs(newCuirassArmor - cuirassArmor) + ")";
				newArmor.color = red;
			}
			
			// Helmet Armor
			if (newHelmetArmor >= helmetArmor && helmet.activeInHierarchy)
			{
				newArmor.text = "+(" + Mathf.Abs(newHelmetArmor - helmetArmor) + ")";
				newArmor.color = green;
			}
			else if (helmet.activeInHierarchy)
			{
				newArmor.text = "-(" + Mathf.Abs(newHelmetArmor - helmetArmor) + ")";
				newArmor.color = red;
			}
			
			// Shield Armor
			if (newShieldArmor >= shieldArmor && shield.activeInHierarchy)
			{
				newArmor.text = "+(" + Mathf.Abs(newShieldArmor - shieldArmor) + ")";
				newArmor.color = green;
			}
			else if (shield.activeInHierarchy)
			{
				newArmor.text = "-(" + Mathf.Abs(newShieldArmor - shieldArmor) + ")";
				newArmor.color = red;
			}
		}
	}
}