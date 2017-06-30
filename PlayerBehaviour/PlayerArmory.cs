using AbstractBehaviour;
using System.Collections.Generic;
using UnityEngine;
using VotanLibraries;

namespace PlayerBehaviour
{
    /// <summary>
    /// Абстрактный класс, который описывает броню объекта
    /// </summary>
    public class PlayerArmory 
        : AbstractObjectConditions
    {
        [SerializeField,Tooltip("Шлем")]
        private GameObject helmet;
        [SerializeField, Tooltip("Части кирасы. Первый элемент - главный")]
        private List<GameObject> kirasaParts;
        [SerializeField, Tooltip("Части щита. Первый элемент - главный")]
        private List<GameObject> shieldParts;

        private bool isHelmetDeactive;
        private bool isKirasaDeactive;
        private bool isShieldDeactive;
        private float kirasaPartArmory;
        private float shieldPartArmory;

        private float tempArmory;

        private void Start()
        {
            IsAlive = true;
            colorChannelGreen = 1;
            colorChannelRed = 1;
            circleHealthUI.color = new Color(0,colorChannelRed, colorChannelGreen);
            initialisatedHealthValue = healthValue;

            kirasaPartArmory = 0.25f / kirasaParts.Count;
            shieldPartArmory = 0.25f / shieldParts.Count;
        }

        public void DecreaseArmoryLevel(float value)
        {
            if (healthValue > 0)
            {
                healthValue += value;
                RefreshHealthCircle();
            }
            else if (healthValue <= 0)
            {
                IsAlive = false;
                healthValue = 0;
                RefreshHealthCircle();
            }
        }

        public override void RefreshHealthCircle()
        {
            float a = circleHealthUI.fillAmount;
            circleHealthUI.fillAmount = healthValue / initialisatedHealthValue;
            a -= circleHealthUI.fillAmount;
            tempArmory += a;
            CheckArmoryLevel();
        }

        private void CheckArmoryLevel()
        {
            if (!isHelmetDeactive && healthValue <= initialisatedHealthValue -
                (initialisatedHealthValue / 4))
            {
                isHelmetDeactive = true;
                helmet.SetActive(false);
                tempArmory -= 0.25f;
            }

            if (!isShieldDeactive && healthValue <= initialisatedHealthValue -
                (initialisatedHealthValue / 4) * 2)
            {
                while (tempArmory >= shieldPartArmory)
                {
                    int a;
                    if (shieldParts.Count > 1)
                    {
                        a = LibraryStaticFunctions.rnd.Next(1, shieldParts.Count);
                    }
                    else
                    {
                        a = 0;
                        isShieldDeactive = true;
                        break;
                    }

                    shieldParts[a].SetActive(false);
                    shieldParts.RemoveAt(a);
                    tempArmory -= shieldPartArmory;
                }
            }

            if (!isKirasaDeactive && healthValue <= initialisatedHealthValue -
                (initialisatedHealthValue / 4) * 3)
            {
                while (tempArmory >= kirasaPartArmory)
                {
                    int a;
                    if (kirasaParts.Count > 1)
                    {
                        a = LibraryStaticFunctions.rnd.Next(1, kirasaParts.Count);
                    }
                    else
                    {
                        a = 0;
                        isKirasaDeactive = true;
                        break;
                    }
                    kirasaParts[a].SetActive(false);
                    kirasaParts.RemoveAt(a);
                    tempArmory -= kirasaPartArmory;
                }
            }
        }
    }
}
