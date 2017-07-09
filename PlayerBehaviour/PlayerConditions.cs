using AbstractBehaviour;
using MovementEffects;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VotanLibraries;
using VotanInterfaces;
using Playerbehaviour;
using GameBehaviour;

namespace PlayerBehaviour
{
    /// <summary>
    /// Описывает рингбар здоровья, ману, броню, ярость
    /// </summary>
    public class PlayerConditions
        : AbstractObjectConditions,IPlayerConditions
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
        [SerializeField,Tooltip("Хранитель компонентов")]
        private PlayerComponentsControl playerComponentsControl;
        [SerializeField]
        private RectTransform mainBarCanvas;

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
        public void Start()
        {
            IsAlive = true;
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
        /// Обновить состояние кольца ярости
        /// </summary>
        public void RefreshRingRage()
        {
            ringRageUI.fillAmount = rageValuePlayer / initialisatedRageValuePlayer;
        }

        /// <summary>
        /// Запустить корутин регена ярости
        /// </summary>
        public void StartRageCoroutineRegen()
        {
            Timing.RunCoroutine(CoroutineForRage());
        }

        /// <summary>
        /// Старт анимации
        /// </summary>
        public void StartRingbarAnimation()
        {
            ringRageUIAnimation["PlayerRingbarRotationAnimation"].speed = 0.2f;
            ringRageUIAnimation.Play();
        }

        /// <summary>
        /// Корутин на реген ярости
        /// </summary>
        /// <returns></returns>
        public IEnumerator<float> CoroutineForRage()
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
                if (!playerComponentsControl.PlayerArmory.IsAlive)
                {
                    if (playerComponentsControl.PlayerFight.IsDefensing)
                    {
                        HealthValue -= LibraryStaticFunctions.GetPlusMinusVal(damageValue, 0.1f) * LibraryStaticFunctions.GetPlusMinusVal
                            (playerComponentsControl.PlayerWeapon
                            .DefenceValue, 0.1f);
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
                    if (playerComponentsControl.PlayerFight.IsDefensing)
                    {
                        playerComponentsControl.PlayerArmory
                            .DecreaseArmoryLevel(-
                        LibraryStaticFunctions.GetPlusMinusVal(damageValue, 0.1f) *
                        LibraryStaticFunctions.GetPlusMinusVal
                            (playerComponentsControl.PlayerWeapon
                            .DefenceValue, 0.1f));
                        CoroutineForIsMayGetDamage();
                    }
                    else
                    {
                        playerComponentsControl.PlayerArmory
                            .DecreaseArmoryLevel(-
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

        public void RotateConditionBar(bool flag=false,float angle=0)
        {
            if (flag)
                mainBarCanvas.localRotation = 
                    Quaternion.Euler(angle, -90, -90);        
            else
                mainBarCanvas.localRotation = 
                    Quaternion.Euler(90, -90, -90);
        }

        /// <summary>
        /// Корутин для продолжительного получения урона
        /// </summary>
        /// <param name="damageValue"></param>
        /// <param name="time"></param>
        /// <param name="dmgPerSecond"></param>
        /// <returns></returns>
        public IEnumerator<float> MayToGetDamage()
        {
            playerComponentsControl.PlayerFight.IsDamaged = true;
            playerComponentsControl.PlayerAnimationsController
                .HighSpeedAnimation();
            playerComponentsControl.PlayerAnimationsController
                .SetState(4, true);
            mayToGetDamage = false;
            yield return Timing.WaitForSeconds(0.5f);
            mayToGetDamage = true;
            playerComponentsControl.PlayerFight.IsDamaged = false;
            playerComponentsControl.PlayerAnimationsController
                .SetState(4, false);
        }

        /// <summary>
        /// Состояние смерти
        /// </summary>
        public override IEnumerator<float> DieState()
        {
            Debug.Log(gameObject.name +  " is dead!");
            IsAlive = false;
            AllPlayerManager.CheckList();
           //playerComponentsControl.PlayerAnimationsController
           //    .LowSpeedAnimation();
            GetComponent<PlayerController>().IsAliveFromConditions = false;
            playerComponentsControl.PlayerAnimationsController
                .DisableAllStates();
            playerComponentsControl.PlayerCollision.RigidbodyState(false);
            playerComponentsControl.PlayerCollision.RigidbodyDead();
            playerComponentsControl.PlayerAnimationsController
                .PlayDeadNormalizeCoroutine();
            mainBarCanvas.gameObject.SetActive(false);
            yield return Timing.WaitForSeconds(1);
        }
    }
}
