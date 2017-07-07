using MovementEffects;
using Playerbehaviour;
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
        [SerializeField, Tooltip("Хранитель компонентов")]
        private PlayerComponentsControl playerComponentsControl;

        private Vector3 fightVector;
        private float boostValue = 1;
        private Vector3 rotateBodyOfPlayerVector;
        private float sumAngle;

        private bool isRotating;
        private bool isFighting;
        private bool isDefensing;
        private bool isSpining;
        private bool isDamaged;

        private float tempSpinSpeed;
        private float spiningSpeedInCoroutine;
        private bool isMayToLongAttack = true;
        #endregion

        #region Свойства
        public bool IsFighting
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

        public bool IsRotating
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

        public bool IsDefensing
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

        public bool IsSpining
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

        public bool IsDamaged
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
            spiningSpeedInCoroutine = playerComponentsControl.PlayerWeapon.SpinSpeed;

            while (spiningSpeedInCoroutine > 0)
            {
                playerComponentsControl.PlayerController.Angle 
                    += spiningSpeedInCoroutine * tempSpinSpeed;
				yield return Timing.WaitForSeconds(0.1f);
                spiningSpeedInCoroutine -= 10;
                playerComponentsControl.PlayerAnimationsController.
                       SetSpeedAnimationByRunSpeed
                       (SpeedWhileSpiningForAnimator());
            }
            isRotating = false;
        }

        /// <summary>
        /// Установить скорость анимации при вращении оружием
        /// </summary>
        /// <param name="isStick"></param>
        /// <returns></returns>
        private float SpeedWhileSpiningForAnimator(bool isStick)
        {
            if (isStick)
                return Math.Abs(tempSpinSpeed/2) 
                    + playerComponentsControl.PlayerWeapon.SpinSpeed * 0.01f/2;
            else
                return playerComponentsControl.PlayerWeapon.SpinSpeed * 0.01f / 2;
        }

        /// <summary>
        /// Перегрузка метода для установления 
        /// скорости анимации вращения оружием
        /// </summary>
        /// <returns></returns>
        private float SpeedWhileSpiningForAnimator()
		{ 
            return Math.Abs(tempSpinSpeed / 2)
                    + spiningSpeedInCoroutine * 0.01f / 2;
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
                    playerComponentsControl.PlayerController.Angle 
                        += playerComponentsControl.PlayerWeapon
                        .SpinSpeed * fightVector.x;
                    tempSpinSpeed = fightVector.x;
                    playerComponentsControl.PlayerAnimationsController.
                        SetSpeedAnimationByRunSpeed
                        (SpeedWhileSpiningForAnimator(true));
                }
                else if (isRotating)
                {
                    playerComponentsControl.PlayerController.Angle 
                        += playerComponentsControl.PlayerWeapon.SpinSpeed * 0.1f;
                    playerComponentsControl.PlayerAnimationsController.
                        SetSpeedAnimationByRunSpeed(SpeedWhileSpiningForAnimator(false));
                }
                // Рывок или защита
                else
                {
                    // Рывок
                    if (fightVector.z > 0.25f)
                    {
                        if (!isFighting && isMayToLongAttack)
                        {
                            Timing.RunCoroutine(CoroutineForStraightAttack());
                        }
                    }
                    // Защита
                    else if (fightVector.z < -0.25f)
                    {
                        // включаем защиту
                        if (!isDefensing)
                            playerComponentsControl.PlayerAnimationsController.
                                HighSpeedAnimation();

                        playerComponentsControl.PlayerAnimationsController
                            .SetState(2, true);
                        isRotating = false;
                        isDefensing = true;
                        isFighting = true;
                        playerComponentsControl.PlayerCameraSmooth.CameraZoom();
                    }
                }
            }
            else if (isSpining)
            {
                playerComponentsControl.PlayerAnimationsController
                    .HighSpeedAnimation();
                isSpining = false;
                Timing.RunCoroutine(CoroutineSlowMotionFighting(fightVector.x));
            }
            else
            {
                if (isDefensing)
                {
                    playerComponentsControl.PlayerAnimationsController.
                        HighSpeedAnimation();
                    playerComponentsControl.PlayerAnimationsController
                        .SetState(2, false);
                    isDefensing = false;
                    isFighting = false;
                }
                isMayToLongAttack = true;
                playerComponentsControl.
                    PlayerCameraSmooth.CheckVectorForCamera();
            }
        }

        /// <summary>
        /// Корутина для атакующего рывка
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForStraightAttack()
        {
            isMayToLongAttack = false;
            spiningSpeedInCoroutine = 0;
            IsRotating = false;
            isFighting = true;
            playerComponentsControl.PlayerController.StraightMoving();
            yield return Timing.WaitForSeconds(1);
            playerComponentsControl.PlayerController.StopLongAttack();
            isFighting = false;
        }
    }
}
