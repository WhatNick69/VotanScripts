using UnityEngine;
using UnityEngine.UI;

namespace AbstractBehaviour
{
    /// <summary>
    /// Абстрактный метод, позволяющий 
    /// </summary>
    public abstract class AbstractObjectConditions
        : MonoBehaviour
    {
        [SerializeField, Tooltip("Здоровье объекта")]
        protected float healthValue;
        [SerializeField, Tooltip("Диаграмма здоровья")]
        protected Image circleHealthUI;
        protected float initialisatedHealthValue;
        protected float colorChannelRed;
        protected float colorChannelGreen;
        protected bool isAlive; // жив ли игрок

        /// <summary>
        /// Свойство для здоровья персонажа
        /// </summary>
        public float HealthValue
        {
            get
            {
                return healthValue;
            }

            set
            {
                if (healthValue > 0)
                {
                    healthValue = value;
                    isAlive = true;
                    RefreshHealthCircle();
                }
                else if (healthValue <= 0 && isAlive)
                {
                    isAlive = false;
                    DieState();
                    healthValue = 0;
                    RefreshHealthCircle();
                }
            }
        }

        public virtual void DieState()
        {
            isAlive = false;
        }

        /// <summary>
        /// Обновить RingbarUIImage (заполненность и цвет)
        /// </summary>
        private void RefreshHealthCircle()
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

    }
}
