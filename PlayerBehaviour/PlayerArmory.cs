using AbstractBehaviour;
using System.Collections.Generic;
using UnityEngine;

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
        private List<bool> kirasaPartsDeactive;
        private List<bool> shieldPartsDeactive;


        private void Start()
        {
            IsAlive = true;
            colorChannelGreen = 1;
            colorChannelRed = 1;
            circleHealthUI.color = new Color(0,colorChannelRed, colorChannelGreen);
            initialisatedHealthValue = healthValue;
            kirasaPartsDeactive = new List<bool>();
            shieldPartsDeactive = new List<bool>();
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
            circleHealthUI.fillAmount = healthValue / initialisatedHealthValue;
            CheckArmoryLevel();
        }

        private void CheckArmoryLevel()
        {
            if (healthValue <= initialisatedHealthValue-(initialisatedHealthValue/4) && !isHelmetDeactive)
            {
                isHelmetDeactive = true;
                helmet.SetActive(false);
            }
            else if (false)
            {

            }
        }
    }
}
