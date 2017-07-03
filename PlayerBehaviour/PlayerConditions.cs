using AbstractBehaviour;
using MovementEffects;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VotanLibraries;
using System;

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
        private GameObject playerObject;
        private PlayerArmory playerArmory;
        private PlayerFight playerFight;
        [SerializeField]
        private PlayerAnimationsController playerAnimationsController;

        private bool isRageRegen; // можно ли регенерить ярость
        private float initialisatedRageValuePlayer; // начальное значение ярости

        private bool mayToGetDamage = true;
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
            playerArmory = GetComponent<PlayerArmory>();
            playerFight = GetComponent<PlayerFight>();

            IsAlive = true;
            isRageRegen = true;
            ringRageUIAnimation = ringRageUI.GetComponent<Animation>();
            initialisatedHealthValue = healthValue;
            initialisatedRageValuePlayer = rageValuePlayer;
            rageValuePlayer = 1;
            playerObject = LibraryPlayerPosition.PlayerObjectTransform.gameObject;

            colorChannelRed = 0;
            colorChannelGreen = 1;

            StartRingbarAnimation();
            StartRageCoroutineRegen();
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
                if (!IsAlive) isRageRegen = false;              
            }
        }

        /// <summary>
        /// Получаем урон
        /// </summary>
        /// <param name="damageValue"></param>
        public void GetDamage(float damageValue)
        {
            if (mayToGetDamage)
            {
                if (!playerArmory.IsAlive)
                {
                    if (PlayerFight.IsDefensing)
                    {
                        HealthValue -= LibraryStaticFunctions.GetPlusMinusVal(damageValue, 0.1f) * LibraryStaticFunctions.GetPlusMinusVal
                            (playerFight.MyWeapon.DefenceValue, 0.1f);
                        CoroutineForIsMayGetDamage();
                    }
                    else
                    {
                        HealthValue -= LibraryStaticFunctions.GetPlusMinusVal(damageValue, 0.1f);
                        CoroutineForIsMayGetDamage();
                    }
                }
                else
                {
                    if (PlayerFight.IsDefensing)
                    {
                        playerArmory.DecreaseArmoryLevel(-
                        LibraryStaticFunctions.GetPlusMinusVal(damageValue, 0.1f) *
                        LibraryStaticFunctions.GetPlusMinusVal
                            (playerFight.MyWeapon.DefenceValue, 0.1f));
                        CoroutineForIsMayGetDamage();
                    }
                    else
                    {
                        playerArmory.DecreaseArmoryLevel(-
                       LibraryStaticFunctions.GetPlusMinusVal(damageValue, 0.1f));
                        CoroutineForIsMayGetDamage();
                    }
                }
            }
        }

        /// <summary>
        /// Получаем урон. Запускаем корутин, в течении времени которого
        /// враг не может ударить. В этот момент воспроизводится анимация
        /// </summary>
        /// <param name="damageValue"></param>
        /// <param name="time"></param>
        /// <param name="dmgPerSecond"></param>
        public void CoroutineForIsMayGetDamage()
        {
            Timing.RunCoroutine(MayToGetDamage());
        }

        /// <summary>
        /// Корутин для продолжительного получения урона
        /// </summary>
        /// <param name="damageValue"></param>
        /// <param name="time"></param>
        /// <param name="dmgPerSecond"></param>
        /// <returns></returns>
        IEnumerator<float> MayToGetDamage()
        {
            PlayerFight.IsDamaged = true;
            playerAnimationsController.HighSpeedAnimation();
            playerAnimationsController.SetState(4, true);
            mayToGetDamage = false;
            yield return Timing.WaitForSeconds(0.5f);
            mayToGetDamage = true;
            PlayerFight.IsDamaged = false;
            playerAnimationsController.SetState(4, false);
        }

        /// <summary>
        /// Состояние смерти
        /// </summary>
        public override IEnumerator<float> DieState()
        {
            Debug.Log("Dead");
            IsAlive = false;
            playerAnimationsController.LowSpeedAnimation();
            GetComponent<PlayerController>().IsAliveFromConditions = false;
            playerAnimationsController.DisableAllStates();
            yield return Timing.WaitForSeconds(1);
        }

        public override float GetDamageWithResistance(float damage, DamageType dmgType)
        {
            // на тот случай, если будем вводить в игру 
            // повреждения по стихиям для игрока
            return 0;
        }
    }
}
