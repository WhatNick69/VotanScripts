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
    /// наносимый урон персонажем
    /// </summary>
    public class ItemPower
        : MonoBehaviour, IItem
    {
        #region Переменные
        [SerializeField, Tooltip("Тип предмета")]
        private ItemType itemType;
        [SerializeField, Tooltip("Качество предмета")]
        private ItemQuality itemQuality;
        [SerializeField, Tooltip("Изображение предмета")]
        private Image itemImage;
        private Image parentImage;
        [SerializeField, Tooltip("Величина бонуса к мощности игрока"), Range(10, 1000)]
        private float powerValue;
        [SerializeField, Tooltip("Время между приемами предмета"), Range(1, 120)]
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

        public float ItemStrenght
        {
            get
            {
                return powerValue;
            }

            set
            {
                powerValue = value;
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
        /// Инициализация предмета
        /// </summary>
        /// <param name="itemImage">Изображение предмета</param>
        /// <param name="powerValue">Бонус к атаке</param>
        /// <param name="itemCount">Количество предметов данного класса</param>
        /// <param name="secondsForTimer">Время перезарядки предмета</param>
        public void InitialisationItem
            (Image itemImage, float powerValue, int itemCount, int secondsForTimer)
        {
            this.itemImage = itemImage;
            this.powerValue = powerValue;
            this.itemCount = itemCount;
            this.secondsForTimer = secondsForTimer;
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
                parentImage.color = fonNonActiveColor;

                playerComponentsControlInstance.PlayerHUDAudioStorage.PlaySoundItemClick();
                ItemCount--;
                playerComponentsControlInstance.PlayerHUDManager.TellItemIndicator(itemNumberPosition,false);
                playerComponentsControlInstance.PlayerVisualEffects.PlayPowerEffectWeapon();
                Timing.RunCoroutine(CoroutineForPowerItem());
                Timing.RunCoroutine(CoroutineTimer());
            }
            else
            {
                playerComponentsControlInstance.PlayerHUDAudioStorage.PlaySoundImpossibleClick();
            }
        }

        /// <summary>
        /// Таймер на перезарядку
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

        /// <summary>
        /// Корутина для повышения мощности игрока
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForPowerItem()
        {
            isEffecting = true;
            playerComponentsControlInstance.PlayerWeapon.Damage += powerValue;

            if (ItemCount <= 0)
            {
                transform.parent = null;
                playerComponentsControlInstance.
                    PlayerHUDManager.DeleteItemInterfaceReference(itemNumberPosition);
                playerComponentsControlInstance.PlayerHUDManager.RefreshInventory();
            }

            yield return Timing.WaitForSeconds(10);

            playerComponentsControlInstance.PlayerWeapon.Damage -= powerValue;
            playerComponentsControlInstance.PlayerVisualEffects.StopPowerEffectWeapon();
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
            if (!isEffecting && isContainsItem)
            {
                itemImage.fillAmount = 1;
                parentImage.color = Color.white;
                playerComponentsControlInstance.PlayerHUDManager.
                    TellItemIndicator(itemNumberPosition, true);
            }
        }
    }
}
