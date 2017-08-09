using AbstractBehaviour;
using GameBehaviour;
using System.Collections.Generic;
using UnityEngine;
using VotanLibraries;

namespace EnemyBehaviour
{
    /// <summary>
    /// Броня вражеского объекта
    /// </summary>
    class EnemyArmory
        : AbstractObjectConditions
    {
        [SerializeField, Tooltip("Лист с элементами брони")]
        private List<ArmoryObject> armoryArray;
        private float tempArmory;
        private float partOfArmory;

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Start()
        {
            IsAlive = true;
            colorChannelGreen = 1;
            colorChannelRed = 0.5f;
            circleHealthUI.color = new Color(colorChannelRed, 0, colorChannelGreen);
            initialisatedHealthValue = healthValue;

            partOfArmory = 1f / (float)armoryArray.Count;
        }

        /// <summary>
        /// Пустая реализация смерти брони у врага
        /// </summary>
        /// <returns></returns>
        public override IEnumerator<float> DieState()
        {
            yield break;
        }

        /// <summary>
        /// Снизить уровень брони врага
        /// </summary>
        /// <param name="value"></param>
        public void DecreaseArmoryLevel(float value)
        {
            if (healthValue > 0)
            {
                healthValue += value;
                RefreshHealthCircle();
            }
            else
            {
                IsAlive = false;
                healthValue = 0;
                RefreshHealthCircle();
            }
        }

        /// <summary>
        /// Обновить броню врага
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
            if (tempArmory >= partOfArmory)
            {
                while (tempArmory >= partOfArmory)
                {
                    if (CheckIfArmoryActiveAtLeastOne())
                    {
                        int a = Random.Range(0, armoryArray.Count);
                        armoryArray[a].FireEvent();
                        armoryArray.RemoveAt(a);
                        tempArmory -= partOfArmory;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Проверить, есть ли как минимум один элемент
        /// в списке элементов брони у врага
        /// </summary>
        /// <returns></returns>
        private bool CheckIfArmoryActiveAtLeastOne()
        {
            return armoryArray.Count > 0 ? true : false;
        }
    }
}
