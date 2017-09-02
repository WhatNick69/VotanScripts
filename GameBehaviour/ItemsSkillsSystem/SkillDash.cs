﻿using System.Collections.Generic;
using PlayerBehaviour;
using UnityEngine;
using UnityEngine.UI;
using VotanInterfaces;
using MovementEffects;
using System;

namespace GameBehaviour
{
    /// <summary>
    /// Атакующий рывок
    /// </summary>
    public class SkillDash
        : MonoBehaviour, ISkill
    {
        #region Переменные и ссылки
        [SerializeField, Tooltip("Изображение предмета")]
        private Image skillImage;
        private Image parentImage;
        [SerializeField, Tooltip("Время между приемами предмета"), Range(1, 120)]
        private int secondsForTimer;
		[SerializeField, Tooltip("Цена скила в опыте"), Range(1, 100000)]
		private int priceExp;
		[SerializeField, Tooltip("Хранитель компонентов игрока")]
        private PlayerComponentsControl playerComponentsControlInstance;
        [SerializeField, Tooltip("Низвание умения")]
        private string skillName;
        [SerializeField, Tooltip("Описание умения")]
        private string skillTutorial;

        private Color fonNonActiveColor;
        private int itemNumberPosition;
        private bool isMayToFire;
        #endregion

        #region Свойства
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

        public Image SkillImage
        {
            get
            {
                return skillImage; 
            }
        }

        public bool IsMayToFire
        {
            get
            {
                return isMayToFire;
            }

            set
            {
                isMayToFire = value;
            }
        }

        public string SkillName
        {
            get
            {
                return skillName;
            }
        }

        public string SkillTutorial
        {
            get
            {
                return skillTutorial;
            }
        }

		public int PriceExp
		{
			get
			{
				return priceExp;
			}
		}
		#endregion

		/// <summary>
		/// Инициализация
		/// </summary>
		public void Starter(int number)
        {
            parentImage = transform.GetComponentInParent<Image>();
            fonNonActiveColor = new Color(1, 1, 1, 0.2f);
            isMayToFire = true;

            itemNumberPosition = number;
            playerComponentsControlInstance.PlayerHUDManager.
                TellSkillIndicator(itemNumberPosition, true);
        }

        /// <summary>
        /// Зажечь умение
        /// </summary>
        public void FireEventSkill()
        {
            if (isMayToFire)
            {
                parentImage.color = fonNonActiveColor;

                playerComponentsControlInstance.PlayerHUDAudioStorage.PlaySoundSkillClick();
                playerComponentsControlInstance.PlayerHUDManager.
                    TellSkillIndicator(itemNumberPosition, false);
                playerComponentsControlInstance.PlayerFight.SkillLongAttack();
                Timing.RunCoroutine(CoroutineTimer());
            }
            else
            {
                playerComponentsControlInstance.PlayerHUDAudioStorage.PlaySoundImpossibleClick();
            }
        }

        /// <summary>
        /// Инициализация умения
        /// </summary>
        /// <param name="skillImage"></param>
        /// <param name="secondsForTimer"></param>
        public void InitialisationSkill(Image skillImage, int secondsForTimer)
        {
            this.skillImage = skillImage;
            this.secondsForTimer = secondsForTimer;
        }

        /// <summary>
        /// Нажать на предмет
        /// </summary>
        public void OnClickFireSkill()
        {
            playerComponentsControlInstance.PlayerHUDManager.FireSkill(this);
        }

        /// <summary>
        /// Перезарядка умения
        /// </summary>
        /// <returns></returns>
        public IEnumerator<float> CoroutineTimer()
        {
            isMayToFire = false;
            while (skillImage.fillAmount != 0)
            {
                skillImage.fillAmount -= 0.1f;
                yield return Timing.WaitForOneFrame;
            }

            skillImage.fillAmount = 0;
            int iterations = (int)(secondsForTimer / 0.05f);
            float incrementPart = 1f / iterations;

            for (int i = 0; i < iterations; i++)
            {
                skillImage.fillAmount += incrementPart;
                yield return Timing.WaitForSeconds(0.05f);
            }
            skillImage.fillAmount = 1;
            parentImage.color = Color.white;

            playerComponentsControlInstance.PlayerHUDManager.
                TellSkillIndicator(itemNumberPosition, true);

            isMayToFire = true;
        }

    }
}
