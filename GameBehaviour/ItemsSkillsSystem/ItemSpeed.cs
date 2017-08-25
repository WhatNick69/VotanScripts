using MovementEffects;
using PlayerBehaviour;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VotanInterfaces;

namespace GameBehaviour
{
    /// <summary>
    /// Класс, описывающий предмет, который увеличивает
    /// скорости движения и поворота персонажа, а также 
    /// повышает скорость вращения оружием
    /// </summary>
    public class ItemSpeed
        : MonoBehaviour, IItem
    {
		#region Переменные
		[SerializeField]
		private string itemName;
		[SerializeField]
		private string itemTutorial;
		[SerializeField, Tooltip("Стоимость итема в золоте"), Range(1, 100000)]
		public int priceGold;
		[SerializeField, Tooltip("Качество предмета")]
        private ItemQuality itemQuality;
        [SerializeField, Tooltip("Тип предмета")]
        private ItemType itemType;
        [SerializeField, Tooltip("Изображение предмета")]
        private Image itemImage;
        private Image parentImage;
        private float speedBonus;
        [SerializeField, Tooltip("Время перезарядки"), Range(1, 15)]
        private int secondsForTimer;
        [SerializeField, Tooltip("Количество предметов данного класса"), Range(0, 10)]
        private int itemCount;
        private bool isContainsItem;
        [SerializeField, Tooltip("Хранитель компонентов игрока")]
        private PlayerComponentsControl playerComponentsControlInstance;

        private Color fonNonActiveColor;
        private int itemNumberPosition;

        private bool isEffecting;
        #endregion

        #region Свойства
        public bool IsContainsItem
        {
            get
            {
                return isContainsItem;
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

        public Image ItemImage
        {
            get
            {
                return itemImage;
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

        public int SecondsForTimer
        {
            get
            {
                return secondsForTimer;
            }

            set
            {
                secondsForTimer = value;
            }
        }

        public ItemType ItemType
        {
            get
            {
                return itemType;
            }
        }

        public ItemQuality ItemQuality
        {
            get
            {
                return itemQuality;
            }
        }

		public int PriceGold
		{
			get
			{
				return priceGold;
			}
		}

		public string ItemName
		{
			get
			{
				return itemName;
			}

			set
			{
				itemName = value;
			}
		}

		public string ItemTutorial
		{
			get
			{
				return itemTutorial;
			}

			set
			{
				itemTutorial = value;
			}
		}
		#endregion

		/// <summary>
		/// Инициализация
		/// </summary>
		public void Starter(int number)
        {
            isContainsItem = true;
            parentImage = transform.GetComponentInParent<Image>();
            fonNonActiveColor = new Color(1, 1, 1, 0.2f);

            itemNumberPosition = number;
            playerComponentsControlInstance.PlayerHUDManager.
                SetPositionToLeftIndicator(itemNumberPosition);
            playerComponentsControlInstance.PlayerHUDManager.
                TellItemIndicator(itemNumberPosition, true);
        }

        /// <summary>
        /// Установить величину прироста к скорости
        /// бега персонажа в зависимости от качества предмета
        /// </summary>
        public void SetItemStrenghtDependenceItemQuality()
        {
            switch (itemQuality)
            {
                case ItemQuality.Lite:
                    speedBonus = 0.15f;
                    break;
                case ItemQuality.Medium:
                    speedBonus = 0.4f;
                    break;
                case ItemQuality.Strong:
                    speedBonus = 0.7f;
                    break;
            }
        }

        /// <summary>
        /// Запустить предмет 
        /// </summary>
        public void FireEventItem()
        {
            if (ItemCount > 0
                && isContainsItem
                    && !isEffecting)
            {
                SetItemStrenghtDependenceItemQuality();

                parentImage.color = fonNonActiveColor;

                playerComponentsControlInstance.PlayerHUDAudioStorage.PlaySoundItemClick();
                ItemCount--;
                playerComponentsControlInstance.PlayerHUDManager.
                    TellItemIndicator(itemNumberPosition, false);
                playerComponentsControlInstance.PlayerVisualEffects.PlaySpeedEffectBoots();
                Timing.RunCoroutine(CoroutineForSpeedItem());
                Timing.RunCoroutine(CoroutineTimer());
            }
            else
            {
                playerComponentsControlInstance.PlayerHUDAudioStorage.PlaySoundImpossibleClick();
            }
        }

        /// <summary>
        /// Нажать на предмет
        /// </summary>
        public void OnClickFireItem()
        {
            playerComponentsControlInstance.PlayerHUDManager.FireItem(this);
        }

        /// <summary>
        /// Включить предмет
        /// </summary>
        public void EnableItem()
        {
            if (!isEffecting && isContainsItem)
            {
                itemImage.fillAmount = 1;
                parentImage.color = Color.white;
                playerComponentsControlInstance.PlayerHUDManager.
                    TellItemIndicator(itemNumberPosition, true);
            }
        }

        /// <summary>
        /// Корутина на предмет
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForSpeedItem()
        {
            isEffecting = true;

            playerComponentsControlInstance.PlayerController.SpeedItemBonus = speedBonus;
            playerComponentsControlInstance.PlayerController.
                OverridePlayerControllerParametersDependenceArmoryWeight();

            if (ItemCount <= 0)
            {
                transform.parent = null;
                playerComponentsControlInstance.
                    PlayerHUDManager.DeleteItemInterfaceReference(this);
            }

            yield return Timing.WaitForSeconds(10);

            float partSpeed = speedBonus / 20;
            for (int i = 0; i < 20; i++)
            {
                playerComponentsControlInstance.PlayerController.SpeedItemBonus -= partSpeed;
                if (playerComponentsControlInstance.PlayerController.SpeedItemBonus < 0)
                    playerComponentsControlInstance.PlayerController.SpeedItemBonus = 0;

                playerComponentsControlInstance.PlayerController.
                    OverridePlayerControllerParametersDependenceArmoryWeight();
                yield return Timing.WaitForSeconds(0.05f);
            }
            playerComponentsControlInstance.PlayerVisualEffects.StopSpeedEffectBoots();

            if (ItemCount <= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                isEffecting = false;
                EnableItem();
            }
        }

        /// <summary>
        /// Корутина для перезарядки
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

                isContainsItem = true;
                EnableItem();
            }
        }
    }
}
