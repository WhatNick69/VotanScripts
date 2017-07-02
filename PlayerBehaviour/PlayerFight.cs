using AbstractBehaviour;
using MovementEffects;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace PlayerBehaviour
{
    /// <summary>
    /// Описывает поведение, реализуемое при помощи движения правого стика
    /// </summary>
    public class PlayerFight
        : MonoBehaviour
    {
        #region Переменные
        [SerializeField, Tooltip("Частота обновления"), Range(0.12f, 0.5f)]
        private float updateFrequency;
        [SerializeField, Tooltip("Контроллер игрока")]
        private PlayerController playerController;

        private Vector3 fightVector;
        private float boostValue = 1;
        private Vector3 rotateBodyOfPlayerVector;
        private float sumAngle;

        private static bool isRotating;
        private static bool isFighting;
        private static bool isDefensing;
        private static bool isSpining;
        private static bool isDamaged;

        private PlayerWeapon myWeapon;
        private PlayerCameraSmooth playerCamSmooth;

        private float tempSpinSpeed;
        private float spiningSpeedInCoroutine;
        #endregion

        #region Свойства
        public PlayerWeapon MyWeapon
        {
            get
            {
                return myWeapon;
            }

            set
            {
                myWeapon = value;
            }
        }

        public static bool IsFighting
        {
            get
            {
                return isFighting;
            }

            set
            {
                isFighting = value;
            }
        }

        public static bool IsRotating
        {
            get
            {
                return isRotating;
            }

            set
            {
                isRotating = value;
            }
        }

        public static bool IsDefensing
        {
            get
            {
                return isDefensing;
            }

            set
            {
                isDefensing = value;
            }
        }

        public static bool IsSpining
        {
            get
            {
                return isSpining;
            }

            set
            {
                isSpining = value;
            }
        }

        public static bool IsDamaged
        {
            get
            {
                return isDamaged;
            }

            set
            {
                isDamaged = value;
            }
        }
        #endregion

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Start()
        {
            playerCamSmooth = GetComponent<PlayerCameraSmooth>();
            InitialisationCoroutineForFightControl();
        }

        /// <summary>
        /// Запуск корутина, который отслеживает нажатие по правому стику
        /// </summary>
        private void InitialisationCoroutineForFightControl()
        {
            Timing.RunCoroutine(CoroutineFightCheckTaps());
        }

        /// <summary>
        /// Корутина, отслуживающая положение правого 
        /// стика с заданной частотой
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineFightCheckTaps()
        {
            while (true)
            {
                GetStickVector();
                CheckFightState();
                yield return Timing.WaitForSeconds(updateFrequency);
            }
        }

        /// <summary>
        /// Получить вектор движения правого стика
        /// </summary>
        private void GetStickVector()
        {
            fightVector = new Vector3(CrossPlatformInputManager.GetAxis("AttackSide")
              , 0, CrossPlatformInputManager.GetAxis("FightState")) * boostValue;
        }

        /// <summary>
        /// Плавное замедление вращения оружием
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private IEnumerator<float> CoroutineSlowMotionFighting(float x)
        {
            spiningSpeedInCoroutine = myWeapon.SpinSpeed;
            while (spiningSpeedInCoroutine > 0)
            {
                PlayerController.Angle += spiningSpeedInCoroutine * tempSpinSpeed;
                yield return Timing.WaitForSeconds(0.1f);
                spiningSpeedInCoroutine -= 10;
            }
            isRotating = false;
        }

        /// <summary>
        /// Совершает действие, относительно положению правого стика.
        /// Вращение влево, вправо, рывок или защита.
        /// </summary>
        private void CheckFightState()
        {
            if (fightVector.magnitude != 0)
            {
                // Вращение влево, либо вправо
                if (Math.Abs(fightVector.x) > Math.Abs(fightVector.z))
                {
                    isRotating = true;
                    isSpining = true;
                    PlayerController.Angle += myWeapon.SpinSpeed * fightVector.x;
                    tempSpinSpeed = fightVector.x;
                }
                // Рывок или защита
                else
                {
                    // Рывок
                    if (fightVector.z > 0.25f)
                    {
                        if (!isFighting)
                        {
                            Timing.RunCoroutine(CoroutineForStraightAttack());
                        }
                    }
                    // Защита
                    else if (fightVector.z < 0)
                    {
                        // включаем защиту
                        playerController.
                            PlayerAnimController.HighSpeedAnimation();
                        playerController.
                            PlayerAnimController.AnimatorOfObject.SetBool("isDefensing", true);
                        isRotating = false;
                        isDefensing = true;
                        isFighting = true;
                        playerCamSmooth.CameraZoom();
                    }
                }
            }
            else if (isSpining)
            {
                isSpining = false;
                Timing.RunCoroutine(CoroutineSlowMotionFighting(fightVector.x));
            }
            else
            {
                if (isDefensing)
                {
                    playerController.
    PlayerAnimController.AnimatorOfObject.SetBool("isDefensing", false);
                    isDefensing = false;
                    isFighting = false;
                }
                playerCamSmooth.CheckVectorForCamera();
            }
        }

        private IEnumerator<float> CoroutineForStraightAttack()
        {
            spiningSpeedInCoroutine = 0;
            IsRotating = false;
            isFighting = true;
            playerController.StraightMoving();
            yield return Timing.WaitForSeconds(1);
            playerController.StopLongAttaack();
            isFighting = false;
        }
    }
}
