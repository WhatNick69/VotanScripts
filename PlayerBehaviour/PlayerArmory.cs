using AbstractBehaviour;
using System.Collections.Generic;
using UnityEngine;
using VotanLibraries;
using System;
using GameBehaviour;

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

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Start()
        {
            IsAlive = true;
            colorChannelGreen = 1;
            colorChannelRed = 0.5f;
            circleHealthUI.color = new Color(colorChannelRed, 0,colorChannelGreen);
            initialisatedHealthValue = healthValue;

            kirasaPartArmory = 0.4f / kirasaParts.Count;
            shieldPartArmory = 0.4f / shieldParts.Count;
        }

        /// <summary>
        /// Снизить показатель брони
        /// </summary>
        /// <param name="value"></param>
        public void DecreaseArmoryLevel(float value)
        {
            if (healthValue > 0)
            {
                healthValue += value;
                RefreshHealthCircle();
            }

            if (healthValue <= 0)
            {
                IsAlive = false;
                healthValue = 0;
                RefreshHealthCircle();
            }
        }

        /// <summary>
        /// Обновить бар брони
        /// </summary>
        public override void RefreshHealthCircle()
        {
            float a = circleHealthUI.fillAmount;
            circleHealthUI.fillAmount = healthValue / initialisatedHealthValue;
            a -= circleHealthUI.fillAmount;
            tempArmory += a;
            CheckArmoryLevel();
        }

        /// <summary>
        /// Проверить уровень брони
        /// </summary>
        private void CheckArmoryLevel()
        {
            // если шлем целый
            if (!isHelmetDeactive && !isShieldDeactive && !isKirasaDeactive)
            {
                if (tempArmory >= 0.2f)
                {
                    isHelmetDeactive = true;
                    helmet.GetComponent<PartArmoryRigidbodyControl>().FireEvent();
                    //helmet.SetActive(false);
                    tempArmory -= 0.2f;
                }
                else
                {
                    return;
                }
            }

            // если щит целый
            if (!isShieldDeactive && isHelmetDeactive && !isKirasaDeactive)
            {
                if (tempArmory >= shieldPartArmory)
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

                        shieldParts[a].GetComponent<PartArmoryRigidbodyControl>().FireEvent();
                        shieldParts.RemoveAt(a);
                        tempArmory -= shieldPartArmory;
                    }
                }
            }

            // если броня целая
            if (!isKirasaDeactive && isHelmetDeactive && isShieldDeactive)
            {
                if (tempArmory >= shieldPartArmory)
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

                        kirasaParts[a].GetComponent<PartArmoryRigidbodyControl>().FireEvent();
                        kirasaParts.RemoveAt(a);
                        tempArmory -= kirasaPartArmory;
                    }
                }
            }
        }
    }
}
