using System;
using PlayerBehaviour;
using UnityEngine;
using UnityEngine.UI;

namespace CraftSystem
{
    /// <summary>
    /// Менеджер всех значений, которые касаются игрока. 
    /// Позволяет обновлять значения жизней, показывает деньги, гемы,
    /// а также все параметры оружия и брони в инвентаре игрока
    /// </summary>
    public class PlayerStats 
        : MonoBehaviour
    {
        #region Переменные
        [SerializeField]
        Image inventoryImageGem;

        [SerializeField]
		Text leveTextl,hpText,armorText,weightText,damageText,
            gemDamageText,critChaceText,newArmorText,newWeightText,
            newDamageText,newGemText,newCritChaceText;

        [SerializeField]
        GameObject cuirass, shield, helmet, weapon, newDamage, newArmor,
            newWeight, NewGemPuwer, NewCrit, skillOne, skillTwo,
            skillThree, itemOne, itemTwo, itemThree;

        [SerializeField]
        Text experienceText, playerMoney1, playerMoney2, playerMoney3, playerMoney4,
            playerGems1, playerGems2, playerGems3, playerGems4;

        Image skillOneImg;
        Image skillTwoImg;
        Image skillThreeImg;
        Image itemOneImg;
        Image itemTwoImg;
        Image itemThreeImg;

        // Для наконечников
        float critChance;
		float headDamage;
		float newCritChance;
		float newHeadDamage;

		// Для камней
		GemType gemType;
		GemType newGemType;

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
        float newGemPower;
        float gemPower;

        Vector4 red = new Vector4(255, 0, 0, 255);
		Vector4 green = new Vector4(0, 40, 0, 255);
        #endregion

        #region Свойства
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

        public Image InventoryImageGem
        {
            get
            {
                return inventoryImageGem;
            }

            set
            {
                inventoryImageGem = value;
            }
        }
        #endregion

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Start()
        {
            InitialisationComponents();
        }

        /// <summary>
        /// Инициализация компонентов
        /// </summary>
        private void InitialisationComponents()
        {
            skillOneImg = skillOne.GetComponent<Image>();
            skillTwoImg = skillTwo.GetComponent<Image>();
            skillThreeImg = skillThree.GetComponent<Image>();

            itemOneImg = itemOne.transform.GetChild(0).GetComponent<Image>();
            itemTwoImg = itemTwo.transform.GetChild(0).GetComponent<Image>();
            itemThreeImg = itemThree.transform.GetChild(0).GetComponent<Image>();
        }

        /// <summary>
        /// Обновляет все значения в таблице характеристик 
        /// </summary>
        public void NewStats()
		{
			armorText.text = (cuirassArmor + shieldArmor + helmetArmor).ToString();
			weightText.text = (cuirassWeight + helmetWeight + shieldWeight).ToString();
			damageText.text = headDamage.ToString();
			gemDamageText.text = GemPower.ToString();
			critChaceText.text = CritChance.ToString();

            // Crit Chance
            if (NewCrit.activeInHierarchy)
            {
                if (newCritChance >= critChance)
                {
                    newCritChaceText.text = "+(" + Mathf.Abs(newCritChance - critChance) + ")";
                    newCritChaceText.color = green;
                }
                else
                {
                    newCritChaceText.text = "-(" + Mathf.Abs(newCritChance - critChance) + ")";
                    newCritChaceText.color = red;
                }
            }

            // Cuiras Weight
            if (cuirass.activeInHierarchy)
            {
                if (newCuirassWeight >= cuirassWeight)
                {
                    newWeightText.text = "+(" + Mathf.Abs(newCuirassWeight - cuirassWeight) + ")";
                    newWeightText.color = red;
                }
                else
                {
                    newWeightText.text = "-(" + Mathf.Abs(newCuirassWeight - cuirassWeight) + ")";
                    newWeightText.color = green;
                }
            }

            // Helmet Weight
            if (helmet.activeInHierarchy)
            {
                if (newHelmetWeight >= helmetWeight)
                {
                    newWeightText.text = "+(" + Mathf.Abs(newHelmetWeight - helmetWeight) + ")";
                    newWeightText.color = green;
                }
                else
                {
                    newWeightText.text = "-(" + Mathf.Abs(newHelmetWeight - helmetWeight) + ")";
                    newWeightText.color = red;
                }
            }

            // Shield Weight
            if (shield.activeInHierarchy)
            {
                if (newShieldWeight >= shieldWeight)
                {
                    newWeightText.text = "+(" + Mathf.Abs(newShieldWeight - shieldWeight) + ")";
                    newWeightText.color = green;
                }
                else
                {
                    newWeightText.text = "-(" + Mathf.Abs(newShieldWeight - shieldWeight) + ")";
                    newWeightText.color = red;
                }
            }

            // Head Damage
            if (newDamage.activeInHierarchy)
            {
                if (newHeadDamage >= headDamage)
                {
                    newDamageText.text = "+(" + Mathf.Abs(newHeadDamage - headDamage).ToString() + ")";
                    newDamageText.color = green;
                }
                else
                {
                    newDamageText.text = "-(" + Mathf.Abs(newHeadDamage - headDamage).ToString() + ")";
                    newDamageText.color = red;
                }
            }

            // Cuirass Armor
            if (cuirass.activeInHierarchy)
            {
                if (newCuirassArmor >= cuirassArmor)
                {
                    newArmorText.text = "+(" + Mathf.Abs(newCuirassArmor - cuirassArmor) + ")";
                    newArmorText.color = green;
                }
                else
                {
                    newArmorText.text = "-(" + Mathf.Abs(newCuirassArmor - cuirassArmor) + ")";
                    newArmorText.color = red;
                }
            }

            // Helmet Armor
            if (helmet.activeInHierarchy)
            {
                if (newHelmetArmor >= helmetArmor)
                {
                    newArmorText.text = "+(" + Mathf.Abs(newHelmetArmor - helmetArmor) + ")";
                    newArmorText.color = green;
                }
                else
                {
                    newArmorText.text = "-(" + Mathf.Abs(newHelmetArmor - helmetArmor) + ")";
                    newArmorText.color = red;
                }
            }

            // Shield Armor
            if (shield.activeInHierarchy)
            {
                if (newShieldArmor >= shieldArmor)
                {
                    newArmorText.text = "+(" + Mathf.Abs(newShieldArmor - shieldArmor) + ")";
                    newArmorText.color = green;
                }
                else
                {
                    newArmorText.text = "-(" + Mathf.Abs(newShieldArmor - shieldArmor) + ")";
                    newArmorText.color = red;
                }
            }
		}

        /// <summary>
        /// Обновляет значения оружия в таблице характеристик 
        /// </summary>
        public void NewStatsForWeapon()
        {
            damageText.text = headDamage.ToString();
            gemDamageText.text = GemPower.ToString();
            critChaceText.text = CritChance.ToString();

            // Crit Chance
            if (NewCrit.activeInHierarchy)
            {
                if (newCritChance >= critChance)
                {
                    newCritChaceText.text = "+(" + Mathf.Abs(newCritChance - critChance) + ")";
                    newCritChaceText.color = green;
                }
                else
                {
                    newCritChaceText.text = "-(" + Mathf.Abs(newCritChance - critChance) + ")";
                    newCritChaceText.color = red;
                }
            }

            // Head Damage
            if (newDamage.activeInHierarchy)
            {
                if (newHeadDamage >= headDamage)
                {
                    newDamageText.text = "+(" + Mathf.Abs(newHeadDamage - headDamage) + ")";
                    newDamageText.color = green;
                }
                else
                {
                    newDamageText.text = "-(" + Mathf.Abs(newHeadDamage - headDamage) + ")";
                    newDamageText.color = red;

                }

                if (newGemPower >= gemPower)
                {
                    newGemText.text = "+(" + Mathf.Abs(newGemPower - gemPower) + ")";
                    newGemText.color = green;
                }
                else
                {
                    newGemText.text = "-(" + Mathf.Abs(newGemPower - gemPower) + ")";
                    newGemText.color = red;
                }
            }
        }

        /// <summary>
        /// Обновляет значения шлемов в таблице характеристик 
        /// </summary>
        public void NewStatsForHelmet()
        {
            armorText.text = (cuirassArmor + shieldArmor + helmetArmor).ToString();
            weightText.text = (cuirassWeight + helmetWeight + shieldWeight).ToString();

            // Helmet Weight
            if (helmet.activeInHierarchy)
            {
                if (newHelmetWeight >= helmetWeight)
                {
                    newWeightText.text = "+(" + Mathf.Abs(newHelmetWeight - helmetWeight) + ")";
                    newWeightText.color = green;
                }
                else
                {
                    newWeightText.text = "-(" + Mathf.Abs(newHelmetWeight - helmetWeight) + ")";
                    newWeightText.color = red;
                }

                if (newHelmetArmor >= helmetArmor)
                {
                    newArmorText.text = "+(" + Mathf.Abs(newHelmetArmor - helmetArmor) + ")";
                    newArmorText.color = green;
                }
                else
                {
                    newArmorText.text = "-(" + Mathf.Abs(newHelmetArmor - helmetArmor) + ")";
                    newArmorText.color = red;
                }
            }
        }

        /// <summary>
        /// Обновляет значения кирасс в таблице характеристик 
        /// </summary>
        public void NewStatsForCuirass()
        {
            armorText.text = (cuirassArmor + shieldArmor + helmetArmor).ToString();
            weightText.text = (cuirassWeight + helmetWeight + shieldWeight).ToString();
            
            // Cuiras Weight
            if (cuirass.activeInHierarchy)
            {
                if (newCuirassWeight >= cuirassWeight)
                {
                    newWeightText.text = "+(" + Mathf.Abs(newCuirassWeight - cuirassWeight) + ")";
                    newWeightText.color = red;
                }
                else
                {
                    newWeightText.text = "-(" + Mathf.Abs(newCuirassWeight - cuirassWeight) + ")";
                    newWeightText.color = green;
                }

                if (newCuirassArmor >= cuirassArmor)
                {
                    newArmorText.text = "+(" + Mathf.Abs(newCuirassArmor - cuirassArmor) + ")";
                    newArmorText.color = green;
                }
                else
                {
                    newArmorText.text = "-(" + Mathf.Abs(newCuirassArmor - cuirassArmor) + ")";
                    newArmorText.color = red;
                }
            }
        }

        /// <summary>
        /// Обновляет значения щитов в таблице характеристик 
        /// </summary>
        public void NewStatsForShield()
        {
            armorText.text = (cuirassArmor + shieldArmor + helmetArmor).ToString();
            weightText.text = (cuirassWeight + helmetWeight + shieldWeight).ToString();
           
            // Shield Weight
            if (shield.activeInHierarchy)
            {
                if (newShieldWeight >= shieldWeight)
                {
                    newWeightText.text = "+(" + Mathf.Abs(newShieldWeight - shieldWeight) + ")";
                    newWeightText.color = green;
                }
                else
                {
                    newWeightText.text = "-(" + Mathf.Abs(newShieldWeight - shieldWeight) + ")";
                    newWeightText.color = red;
                }

                if (newShieldArmor >= shieldArmor)
                {
                    newArmorText.text = "+(" + Mathf.Abs(newShieldArmor - shieldArmor) + ")";
                    newArmorText.color = green;
                }
                else
                {
                    newArmorText.text = "-(" + Mathf.Abs(newShieldArmor - shieldArmor) + ")";
                    newArmorText.color = red;
                }
            }
        }

        /// <summary>
        /// Создана для удобства, просто отключает все окна с итемами
        /// </summary>
        private void PredPageLoad()
        {
            newDamage.SetActive(false);
            newArmor.SetActive(false);
            NewGemPuwer.SetActive(false);
            NewCrit.SetActive(false);
            newWeight.SetActive(false);
            NewStats();
        }

        /// <summary>
        /// Вызывать при открытие листа с оружия
        /// </summary>
        public void WeaponPage()
        {
            PredPageLoad();
            newDamage.SetActive(true);
            NewCrit.SetActive(true);
            NewGemPuwer.SetActive(true);
        }

        /// <summary>
        /// Вызывать при открытие листа с кирасами, щитами, шлемами
        /// </summary>
        public void ArmorPage()
        {
            PredPageLoad();
            newArmor.SetActive(true);
            newWeight.SetActive(true);
        }

        /// <summary>
        /// Вызывать при открытие листа со скилами
        /// </summary>
        public void SkillPage()
        {
            PredPageLoad();
        }

        /// <summary>
        /// Вызывать при открытие листа с итемами
        /// </summary>
        public void ItemPage()
        {
            PredPageLoad();
        }

        /// <summary>
        /// Обновить деньги пользователя
        /// </summary>
        /// <param name="money"></param>
        public void RefreshUserMoney(long money)
        {
            playerMoney1.text = money.ToString();
            playerMoney2.text = money.ToString();
            playerMoney3.text = money.ToString();
            playerMoney4.text = money.ToString();
        }

        /// <summary>
        /// Обновить гемы пользователя
        /// </summary>
        /// <param name="gems"></param>
        public void RefreshUserGems(long gems)
        {
            playerGems1.text = gems.ToString();
            playerGems2.text = gems.ToString();
            playerGems3.text = gems.ToString();
            playerGems4.text = gems.ToString();
        }

        /// <summary>
        /// Обновить опыт пользователя
        /// </summary>
        /// <param name="experience"></param>
        public int RefreshUserExpAndReturnHP(long experience)
        {
            experienceText.text = experience.ToString();
            return LevelToHP(LevelFromExperience(Convert.ToInt32(experienceText.text)));      
        }

        /// <summary>
        /// Перевести уровень в жизни
        /// </summary>
        /// <param name="level"></param>
        private int LevelToHP(int level)
        {
            leveTextl.text = level.ToString();
            int hp = (level * 5) + 100;
            hpText.text = hp.ToString();
            return hp;
        }

        /// <summary>
        /// Перевести опыт в уровень
        /// </summary>
        /// <param name="userExperience"></param>
        /// <returns></returns>
        private int LevelFromExperience(float userExperience)
        {
            float experience = 100;
            int level = 0;
            for (int i = 0;i<100;i++)
            {
                userExperience -= experience;
                if (userExperience <= 0)
                {
                    break;
                }
                else
                {
                    level++;
                }
                experience = Mathf.Ceil(experience/100*1.2f) * 100;
            }
            return level;
        }

        /// <summary>
        /// Задать изображение кнопке выбранного в бой скила
        /// </summary>
        /// <param name="spr"> Изображение скила </param>
        /// <param name="index"> </param>
        public void SetSkillImg(Sprite spr, int index)
        {
            switch (index)
            {
                case 0:
                    skillOneImg.color = Color.white;
                    skillOneImg.sprite = spr;
                    break;
                case 1:
                    skillTwoImg.color = Color.white;
                    skillTwoImg.sprite = spr;
                    break;
                case 2:
                    skillThreeImg.color = Color.white;
                    skillThreeImg.sprite = spr;
                    break;
            }
        }

        /// <summary>
        /// Перегрузка функции SetSkillImg для очистки 
        /// изображения кнопки выбранного в бой скила
        /// </summary>
        /// <param name="index"></param>
        public void SetSkillImg(int index)
        {
            switch (index)
            {
                case 0:
                    skillOneImg.color = new Color(0, 0, 0, 0); 
                    break;
                case 1:
                    skillTwoImg.color = new Color(0, 0, 0, 0); 
                    break;
                case 2:
                    skillThreeImg.color = new Color(0, 0, 0, 0); 
                    break;
            }
        }

        /// <summary>
        /// Задать изображение кнопке выбранного в бой зелья
        /// </summary>
        /// <param name="spr"> Изображения зелья </param>
        /// <param name="index"> </param>
        public void SetItemImg(Sprite spr, int index)
        {
            switch (index)
            {
                case 0:
                    itemOneImg.sprite = spr;
                    itemOneImg.color = Color.white;
                    break;
                case 1:
                    itemTwoImg.sprite = spr;
                    itemTwoImg.color = Color.white;
                    break;
                case 2:
                    itemThreeImg.sprite = spr;
                    itemThreeImg.color = Color.white;
                    break;
            }
        }

        /// <summary>
        /// Перегрузка функции SetItemImg для очистки 
        /// изображения кнопки выбранного в бой зелья
        /// </summary>
        /// <param name="index"></param>
        public void SetItemImg(int index)
        {
            switch (index)
            {
                case 0:
                    itemOneImg.color = new Color(0, 0, 0, 0);
                    break;
                case 1:
                    itemTwoImg.color = new Color(0, 0, 0, 0);
                    break;
                case 2:
                    itemThreeImg.color = new Color(0, 0, 0, 0);
                    break;
            }
        }
    }
}