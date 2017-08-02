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

		float gripWeight;
		float gripDefence;
		float newGripWeight;
		float newGripDefence;

		float headWeight;
		float headDamage;
		float newHeadWeight;
		float newHeadDamage;

		string gemType;
		float gemPower;
		string newGemType;
		float newGemPower;

		float cuirassArmor;
		float helmetArmor;
		float shieldArmor;
		float newCuirassArmor;
		float newHelmetArmor;
		float newShieldArmor;

		Vector4 red = new Vector4(255, 0, 0, 255);
		Vector4 green = new Vector4(0, 10, 0, 255);

		#region
		private void PredPageLoad()
		{
			damage.SetActive(false);
			armor.SetActive(false);
			block.SetActive(false);
			gemTape.SetActive(false);
			gemPuwer.SetActive(false);
			crit.SetActive(false);
		}

		public void GripPage()
		{
			PredPageLoad();
			block.SetActive(true);
		}

		public void HeadPage()
		{
			PredPageLoad();
			damage.SetActive(true);
		}

		public void GemPage()
		{
			PredPageLoad();
			gemTape.SetActive(true);
			gemPuwer.SetActive(true);
		}

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
				NewStats();
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
				NewStats();
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
				NewStats();
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
				NewStats();
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
				NewStats();
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
				NewStats();
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
				NewStats();
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
				NewStats();
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
				NewStats();
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
				NewStats();
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
				NewStats();
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
				NewStats();
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
				NewStats();
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
				NewStats();
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
				NewStats();
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
				NewStats();
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
				NewStats();
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
				NewStats();
			}
		}
		#endregion

		private void NewStats()
		{
			leveTextl.text = "level: " + PlayerPrefs.GetInt("level");
			hpText.text = "HP: " + ((PlayerPrefs.GetInt("level") + 10) * 2);
			armorText.text = "Armor: " + (cuirassArmor + shieldArmor + helmetArmor);
			damageText.text = "Damage: " + LibraryStaticFunctions.TotalDamage(headDamage, headWeight + gripWeight);
			blockText.text = "Block: " + gripDefence;
			gemText.text = "Gem Type: " + GemType;
			gemDamageText.text = "Gem Power:" + GemPower.ToString();
			critChaceText.text = "Crit Chace: ";

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
			
			if (NewHeadWeight >= headWeight && head.activeInHierarchy)
			{
				newDamage.text = "+(" + (LibraryStaticFunctions.TotalDamage(newHeadDamage, newHeadWeight + gripWeight) -
					LibraryStaticFunctions.TotalDamage(headDamage, headWeight + gripWeight)).ToString() + ")";
				newDamage.color = green;
			}
			else if (head.activeInHierarchy)
			{
				newDamage.text = "-(" + (LibraryStaticFunctions.TotalDamage(newHeadDamage, newHeadWeight + gripWeight) -
					LibraryStaticFunctions.TotalDamage(headDamage, headWeight + gripWeight)).ToString() + ")";
				newDamage.color = red;
			}
			

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