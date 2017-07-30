﻿using MovementEffects;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VotanInterfaces;
using System;

namespace AbstractBehaviour
{
    /// <summary>
    /// Абстрактный метод, позволяющий 
    /// реализовать бар здоровья/брони
    /// </summary>
    public abstract class AbstractObjectConditions
        : MonoBehaviour,IVotanObjectConditions
    {
        #region Переменные
        [SerializeField, Tooltip("Здоровье объекта")]
        protected float healthValue;
        [SerializeField, Tooltip("Диаграмма здоровья")]
        protected Image circleHealthUI;
        protected float initialisatedHealthValue;
        protected float colorChannelRed;
        protected float colorChannelGreen;
        [SerializeField]
        private bool isAlive = true; // жив ли объект
        [SerializeField]
        private RectTransform mainBarCanvas;
        #endregion

        #region Свойства
        /// <summary>
        /// Свойство для здоровья персонажа
        /// </summary>
        public virtual float HealthValue
        {
            get
            {
                return healthValue;
            }

            set
            {
                healthValue = value;
                if (healthValue > 0)
                {
                    RefreshHealthCircle();
                }
                else if (healthValue <= 0 && isAlive)
                {
                    isAlive = false;
                    Timing.RunCoroutine(DieState());
                    healthValue = 0;
                    RefreshHealthCircle();
                }
            }
        }

        public bool IsAlive
        {
            get
            {
                return isAlive;
            }

            set
            {
                isAlive = value;
            }
        }

        public RectTransform MainBarCanvas
        {
            get
            {
                return mainBarCanvas;
            }

            set
            {
                mainBarCanvas = value;
            }
        }
        #endregion

        /// <summary>
        /// Состояние смерти
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerator<float> DieState();

        /// <summary>
        /// Обновить RingbarUIImage (заполненность и цвет)
        /// </summary>
        public virtual void RefreshHealthCircle()
        {
            circleHealthUI.fillAmount = healthValue / initialisatedHealthValue;
            if (circleHealthUI.fillAmount >= 0.5f)
            {
                colorChannelRed = (1 - circleHealthUI.fillAmount) * 2;
            }
            else
            {
                colorChannelGreen = circleHealthUI.fillAmount * 2;
            }
            circleHealthUI.color = new Color(colorChannelRed, colorChannelGreen, 0);
        }

        public void RestartFiller()
        {
            circleHealthUI.fillAmount = 1;
            circleHealthUI.color = new Color(0, 1, 0);
        }

        /// <summary>
        /// Установить оригинальное значение жизней
        /// </summary>
        public void SetHealthParameter(float health)
        {
            healthValue = (int)health;
            initialisatedHealthValue = healthValue;
        }
    }
}
