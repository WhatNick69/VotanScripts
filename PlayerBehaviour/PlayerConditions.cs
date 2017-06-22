using MovementEffects;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VotanObjectBehaviour;

namespace PlayerBehaviour
{
    /// <summary>
    /// Описывает рингбар здоровья, ману, броню, ярость
    /// </summary>
    public class PlayerConditions
    : AbstractObjectConditions
    {
        #region Переменные
        [SerializeField, Tooltip("Мана персонажа")]
        private float manaValuePlayer;
        [SerializeField, Tooltip("Ярость персонажа")]
        private float rageValuePlayer;
        [SerializeField, Tooltip("Кольцо ярости")]
        private Image ringRageUI;
        private Animation ringRageUIAnimation;
        [SerializeField,Tooltip("Размер регена ярости")]
        private float rageRegenSize;
        [SerializeField, Tooltip("Модель игрока")]
        private GameObject playerBody;

        private bool isRageRegen; // можно ли регенерить ярость
        private float initialisatedRageValuePlayer; // начальное значение ярости
        #endregion

        #region Свойства

        /// <summary>
        /// Свойство для маны персонажа
        /// </summary>
        public float ManaValuePlayer
        {
            get
            {
                return manaValuePlayer;
            }

            set
            {
                if (manaValuePlayer - value < 0) manaValuePlayer = 0;
                else manaValuePlayer = value;
            }
        }

        /// <summary>
        /// Свойство для ярости персонажа
        /// </summary>
        public float RageValuePlayer
        {
            get
            {
                return rageValuePlayer;
            }

            set
            {
                if (rageValuePlayer > 0)
                {
                    rageValuePlayer = value;
                    RefreshRingRage();
                }
                else if (rageValuePlayer >= initialisatedRageValuePlayer)
                {
                    Debug.Log("Максимальная ярость!");
                    rageValuePlayer = initialisatedRageValuePlayer;
                    RefreshRingRage();
                }
            }
        }
        #endregion

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Start()
        {
            isAlive = true;
            isRageRegen = true;
            ringRageUIAnimation = ringRageUI.GetComponent<Animation>();
            initialisatedHealthValue = healthValue;
            initialisatedRageValuePlayer = rageValuePlayer;
            rageValuePlayer = 1;

            colorChannelRed = 0;
            colorChannelGreen = 1;
            StartRingbarAnimation();
            StartRageCoroutineRegen();
        }

        /// <summary>
        /// Покадровое обновление
        /// </summary>
        private void Update()
        {
            //HealthValue -= 0.25f;
        }

        /// <summary>
        /// Обновить состояние кольца ярости
        /// </summary>
        private void RefreshRingRage()
        {
            ringRageUI.fillAmount = rageValuePlayer / initialisatedRageValuePlayer;
        }

        /// <summary>
        /// Запустить корутин регена ярости
        /// </summary>
        private void StartRageCoroutineRegen()
        {
            Timing.RunCoroutine(CoroutineForRage());
        }

        /// <summary>
        /// Старт анимации
        /// </summary>
        private void StartRingbarAnimation()
        {
            ringRageUIAnimation["PlayerRingbarRotationAnimation"].speed = 0.2f;
            ringRageUIAnimation.Play();
        }

        /// <summary>
        /// Корутин на реген ярости
        /// </summary>
        /// <returns></returns>
        IEnumerator<float> CoroutineForRage()
        {
            while (isRageRegen)
            {
                if (rageValuePlayer <= initialisatedRageValuePlayer)
                {
                    RageValuePlayer += rageRegenSize;
                    yield return Timing.WaitForSeconds(0.25f);
                }
                else
                {
                    yield return Timing.WaitForSeconds(1);
                }
                if (!isAlive) isRageRegen = false;              
            }
        }

        /// <summary>
        /// Получаем урон
        /// </summary>
        /// <param name="damageValue"></param>
        public void GetDamage(float damageValue)
        {
            healthValue -= damageValue;
        }

        /// <summary>
        /// Получаем урон.
        /// Задаем время, в течении которого следует получать урон
        /// Задаем промежуток времени, через который мы получаем урон
        /// </summary>
        /// <param name="damageValue"></param>
        /// <param name="time"></param>
        /// <param name="dmgPerSecond"></param>
        public void GetDamage(float damageValue, float time, float dmgPerSecond = 0.5f)
        {
            healthValue -= damageValue;
            Timing.RunCoroutine(SetDamageForTime(damageValue, time, dmgPerSecond));
        }

        /// <summary>
        /// Корутин для продолжительного получения урона
        /// </summary>
        /// <param name="damageValue"></param>
        /// <param name="time"></param>
        /// <param name="dmgPerSecond"></param>
        /// <returns></returns>
        IEnumerator<float> SetDamageForTime(float damageValue,float time, float dmgPerSecond)
        {
            int i = 0;
            while (time/dmgPerSecond > i)
            {
                healthValue -= damageValue;
                yield return Timing.WaitForSeconds(dmgPerSecond);
                i++;
            }
        }

        /// <summary>
        /// Состояние смерти
        /// </summary>
        public override void DieState()
        {
            base.DieState();
            playerBody.SetActive(false);
        }
    }
}
