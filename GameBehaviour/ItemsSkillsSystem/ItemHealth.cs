﻿using MovementEffects;
using PlayerBehaviour;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VotanInterfaces;

namespace GameBehaviour
{
    /// <summary>
    /// Класс, описывающий предмет, который восстанавливает здоровье
    /// </summary>
    public class ItemHealth
        : MonoBehaviour, IItem
    {
        #region Переменные
        [SerializeField, Tooltip("Изображение предмета")]
        private Image itemImage;
        private Image parentImage;
        [SerializeField, Tooltip("Величина восстанавливаемого здоровья"), Range(10, 1000)]
        private float healthUPValue;
        [SerializeField, Tooltip("Время между приемами предмета"), Range(1, 120)]
        private int secondsForTimer;
        [SerializeField, Tooltip("Количество предметов данного класса"), Range(0, 10)]
        private int itemCount;
        private bool isContainsItem;
        [SerializeField, Tooltip("Хранитель компонентов игрока")]
        private PlayerComponentsControl playerComponentsControlInstance;

        private Color fonNonActiveColor;
        private int itemNumberPosition;
        #endregion

        #region Свойства
        public Image ItemImage
        {
            get
            {
                return itemImage;
            }
        }

        public int ItemCount
        {
            get
            {
                return itemCount;
            }

            set
            {
                itemCount = value;
            }
        }

        public bool IsContainsItem
        {
            get
            {
                return IsContainsItem;
            }
        }

        public PlayerComponentsControl PlayerComponentsControlInstance
        {
            get
            {
                return playerComponentsControlInstance;
            }

            set
            {
                playerComponentsControlInstance = value;
            }
        }

        public int ItemNumberPosition
        {
            get
            {
                return itemNumberPosition;
    }

            set
            {
                itemNumberPosition = value;
            }
        }
        #endregion

        /// <summary>
        /// Инициализация
        /// </summary>
        public void Start()
        {
            isContainsItem = true;
            parentImage = transform.GetComponentInParent<Image>();
            fonNonActiveColor = new Color(1, 1, 1, 0.2f);

            itemNumberPosition = playerComponentsControlInstance.PlayerHUDManager
                .InitialisationThisItemToInventory(this);
            playerComponentsControlInstance.PlayerHUDManager.
                SetPositionToLeftIndicator(itemNumberPosition);
            playerComponentsControlInstance.PlayerHUDManager.
                TellItemIndicator(itemNumberPosition, false);

            Timing.RunCoroutine(CoroutineForCheckHealth());
        }
    
        /// <summary>
        /// Ручная инициализация предмета, который восстанавливает здоровье
        /// </summary>
        /// <param name="itemImage">Изображение предмета</param>
        /// <param name="healthUPValue">Величина восстанавливаемого здоровья</param>
        /// <param name="itemCount">Количество предметов</param>
        /// <param name="secondsForTimer">Время перезарядки предмета</param>
        public void InitialisationItem
            (Image itemImage, float healthUPValue, int itemCount, int secondsForTimer)
        {
            this.itemImage = itemImage;
            this.healthUPValue = healthUPValue;
            this.itemCount = itemCount;
            this.secondsForTimer = secondsForTimer;
        }

        /// <summary>
        /// Зажечь событие для кнопки здоровья
        /// </summary>
        /// <param name="playerComponentsControlInstance"></param>
        public void FireEventItem()
        {
            if (ItemCount > 0
                && isContainsItem
                    && !playerComponentsControlInstance.PlayerConditions.IsMaxHealth())
            {
                parentImage.color = fonNonActiveColor;

                playerComponentsControlInstance.PlayerHUDAudioStorage.PlaySoundItemClick();
                playerComponentsControlInstance.PlayerHUDManager.
                    TellItemIndicator(itemNumberPosition, false);
                ItemCount--;
                Timing.RunCoroutine(CoroutineHealthEffect());
                Timing.RunCoroutine(CoroutineTimer());
            }
            else
            {
                playerComponentsControlInstance.PlayerHUDAudioStorage.PlaySoundImpossibleClick();
            }
        }

        /// <summary>
        /// Корутина на плавное восстановление жизней
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineHealthEffect()
        {
            float healthBonusPart = healthUPValue / 20;
            for (int i = 0;i<20;i++)
            {
                playerComponentsControlInstance.PlayerConditions.HealthValue += healthBonusPart;
                yield return Timing.WaitForSeconds(0.05f);
            }
        }

        /// <summary>
        /// Корутина для запуска предмета
        /// </summary>
        /// <returns></returns>
        public IEnumerator<float> CoroutineTimer()
        {
            isContainsItem = false;
            while (itemImage.fillAmount != 0)
            {
                itemImage.fillAmount -= 0.1f;
                yield return Timing.WaitForOneFrame;
            }

            itemImage.fillAmount = 0;
            int iterations = (int)(secondsForTimer / 0.05f);
            float incrementPart = 1f / iterations;

            if (ItemCount > 0)
            {
                for (int i = 0; i < iterations; i++)
                {
                    itemImage.fillAmount += incrementPart;
                    yield return Timing.WaitForSeconds(0.05f);
                }

                EnableItem();
                isContainsItem = true;
            }
        }

        /// <summary>
        /// Нажать на предмет
        /// </summary>
        public void OnClickFireItem()
        {
            playerComponentsControlInstance.PlayerHUDManager.FireItem(itemNumberPosition);
        }

        /// <summary>
        /// Включить предмет
        /// </summary>
        public void EnableItem()
        {
            if (!playerComponentsControlInstance.PlayerConditions.IsMaxHealth())
            {
                itemImage.fillAmount = 1;
                parentImage.color = Color.white;
                playerComponentsControlInstance.PlayerHUDManager.
                    TellItemIndicator(itemNumberPosition, true);
            }
        }

        private IEnumerator<float> CoroutineForCheckHealth()
        {
            while (true)
            {
                if (playerComponentsControlInstance.PlayerConditions.IsMaxHealth())
                {
                    parentImage.color = fonNonActiveColor;
                    playerComponentsControlInstance.PlayerHUDManager.
                        TellItemIndicator(itemNumberPosition, false);
                }
                else if (isContainsItem)
                {
                    EnableItem();
                }
                yield return Timing.WaitForSeconds(0.5f);
            }
        }
    }
}
