using AbstractBehaviour;
using MovementEffects;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VotanLibraries;
using VotanInterfaces;
using GameBehaviour;

namespace PlayerBehaviour
{
    /// <summary>
    /// Описывает рингбар здоровья, ману, броню, ярость
    /// </summary>
    public class PlayerConditions
        : AbstractObjectConditions, IPlayerConditions, IObjectFitBat
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

        private bool isRageRegen; // можно ли регенерить ярость
        private float initialisatedRageValuePlayer; // начальное значение ярости

        private bool mayToGetDamage = true;
        private bool isDownInterfaceTransformHasBeenChanged;
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

        public bool IsDownInterfaceTransformHasBeenChanged
        {
            get
            {
                return isDownInterfaceTransformHasBeenChanged;
            }

            set
            {
                isDownInterfaceTransformHasBeenChanged = value;
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
                damageValue = LibraryStaticFunctions.
                    DamageFromTempPhysicDefence(damageValue,playerComponentsControl.PlayerWeapon);

                playerComponentsControl.PlayerCameraSmooth.GetNoizeGamage(damageValue / initialisatedHealthValue);
                if (!playerComponentsControl.PlayerArmory.IsAlive)
                {
                    playerComponentsControl.PlayerBloodInterfaceEffect.
                        EventBloodEyesEffect(healthValue/ initialisatedHealthValue);
                    if (playerComponentsControl.PlayerFight.IsDefensing)
                    {
                        HealthValue -= LibraryStaticFunctions.GetRangeValue(damageValue, 0.1f) *
                        (1 - LibraryStaticFunctions.GetRangeValue
                            (playerComponentsControl.PlayerWeapon
                            .DefenceValue, 0.1f) / 100);
                        CoroutineForIsMayGetDamage();
                    }
                    else
                    {
                        HealthValue -= LibraryStaticFunctions.GetRangeValue(damageValue, 0.1f);
                        CoroutineForIsMayGetDamage();
                    }
                }
                else
                {
                    if (playerComponentsControl.PlayerFight.IsDefensing)
                    {
                        playerComponentsControl.PlayerArmory
                            .DecreaseArmoryLevel(-
                        LibraryStaticFunctions.GetRangeValue(damageValue, 0.1f) *
                        (1-LibraryStaticFunctions.GetRangeValue
                            (playerComponentsControl.PlayerWeapon
                            .DefenceValue, 0.1f)/100));
                        CoroutineForIsMayGetDamage();
                    }
                    else
                    {
                        playerComponentsControl.PlayerArmory
                            .DecreaseArmoryLevel(-
                       LibraryStaticFunctions.GetRangeValue(damageValue, 0.1f));
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
        /// Поворачивать нижний интерфейс объекта
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="angle"></param>
        public void RotateConditionBar(bool flag,float angle)
        {
            if (flag && !IsDownInterfaceTransformHasBeenChanged)
            {
                MainBarCanvas.localRotation =
                    Quaternion.Euler(angle, -90, 0);
                IsDownInterfaceTransformHasBeenChanged = true;
            }
            else if (!flag && IsDownInterfaceTransformHasBeenChanged)
            {
                MainBarCanvas.localRotation =
                    Quaternion.Euler(angle, 0, 0);
                IsDownInterfaceTransformHasBeenChanged = false;
            }
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

            GetComponent<PlayerController>().IsAliveFromConditions = false;
            playerComponentsControl.PlayerAnimationsController
                .DisableAllStates();
            playerComponentsControl.PlayerCollision.RigidbodyState(false);
            playerComponentsControl.PlayerCollision.RigidbodyDead();
            playerComponentsControl.PlayerAnimationsController
                .PlayDeadNormalizeCoroutine();
            yield return Timing.WaitForSeconds(1);
        }

        /// <summary>
        /// Отключить интерфейс под игроком
        /// </summary>
        public void DisableDownInterface()
        {
            MainBarCanvas.gameObject.SetActive(false);
        }
    }
}
