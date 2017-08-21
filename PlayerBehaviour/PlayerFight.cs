﻿using MovementEffects;
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
        [SerializeField, Tooltip("Задержка ускорения вращения"), Range(1,50)]
        private int spinLatency;
        private PlayerWeapon playerWeapon;
        private PlayerController playerController;
        private PlayerAnimationsController playerAnimationsController;
        private Vector3 fightVector;

        private bool isRotating;
        private bool isFighting;
        private bool isDefensing;
        private bool isSpining;
        private bool isDamaged;
        private bool isMayToLongAttack = true;

        private float tempSpinSpeed;
        private float spiningSpeedInCoroutine;

        private float tempAngle;
        private int decrementer;
        private float tempAngleFromPlayer;
        private bool isNormal;
        private float maxSpinSpeed;
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
                if (isDefensing)
                    playerComponentsControl.PlayerController.StopPlayerPositionInterpolate();
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

        public bool IsMayToLongAttack
        {
            get
            {
                return isMayToLongAttack;
            }

            set
            {
                isMayToLongAttack = value;
            }
        }

        public bool IsNormal
        {
            get
            {
                return isNormal;
            }

            set
            {
                isNormal = value;
                playerController.IsNormal = isNormal;
            }
        }
        #endregion

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Start()
        {
            playerWeapon = playerComponentsControl.PlayerWeapon;
            maxSpinSpeed = playerWeapon.OriginalSpinSpeed;
            playerController = playerComponentsControl.PlayerController;
            playerAnimationsController = playerComponentsControl.PlayerAnimationsController;
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
              , 0, CrossPlatformInputManager.GetAxis("FightState"));
        }

        /// <summary>
        /// Обработка величины ускорения перед отправкой игроку
        /// </summary>
        /// <param name="addingAngle"></param>
        private void BeforeSendAngleToPlayer()
        {
            if (isNormal)
            {
                playerController.Angle +=
                    (maxSpinSpeed / spinLatency);
            }
            else
            {
                playerController.Angle -=
                    (maxSpinSpeed / spinLatency);
            }
        }

        /// <summary>
        /// Плавное замедление вращения оружием
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private IEnumerator<float> CoroutineSlowMotionFighting()
        {
            bool tempFlag = playerController.Angle > 0 ? true : false;
            float minValue = maxSpinSpeed / spinLatency;
            while (Math.Abs(playerController.Angle) > minValue)
            {
                if (isSpining) yield break;

                if (tempFlag)
                {
                    playerController.Angle -=
                        (maxSpinSpeed / spinLatency);
                }
                else
                {
                    playerController.Angle +=
                        (maxSpinSpeed / spinLatency);
                }
                yield return Timing.WaitForSeconds(updateFrequency);
            }
            playerController.Angle = 0;
            playerController.AngleBeforeSpining();
            IsRotating = false;
        }

        /// <summary>
        /// Совершает действие, относительно положению правого стика.
        /// Вращение влево, вправо, рывок или защита.
        /// </summary>
        private void CheckFightState()
        {
            #region Старая реализация. Вращение, блок и рывок.
            //if (isMayToLongAttack)
            //{
            //    if (fightVector.magnitude != 0)
            //    {
            //        // Вращение влево, либо вправо
            //        if (Math.Abs(fightVector.x) > 0.5f)
            //        {
            //            // но в том случае, если мы не в блоке
            //            if (!IsDefensing)
            //            {
            //                IsRotating = true;
            //                IsSpining = true;
            //
            //                if (fightVector.x >= 0.5f)
            //                {
            //                    IsNormal = true;
            //                }
            //                else if (fightVector.x <= -0.5f)
            //                {
            //                    IsNormal = false;
            //                }
            //
            //                BeforeSendAngleToPlayer();
            //            }
            //        }
            //        // Рывок или защита
            //        else
            //        {
            //            // Рывок
            //            if (fightVector.z > 0.9f)
            //            {
            //                if (!isFighting && isMayToLongAttack && !isSpining && !isRotating)
            //                {
            //                    Timing.RunCoroutine(CoroutineForStraightAttack());
            //                }
            //            }
            //            // Защита
            //            else if (fightVector.z < -0.9f)
            //            {
            //                if (!isFighting && !isRotating)
            //                {
            //                    // включаем защиту
            //                    if (!IsDefensing)
            //                        playerAnimationsController.HighSpeedAnimation();
            //
            //                    playerAnimationsController.HighSpeedAnimation();
            //                    playerAnimationsController.SetState(2, true);
            //                    IsDefensing = true;
            //                    IsFighting = true;
            //                    playerComponentsControl.PlayerCameraSmooth.CameraZoom();
            //                }
            //            }
            //        }
            //    }
            //    else if (isSpining)
            //    {
            //        playerAnimationsController.HighSpeedAnimation();
            //        IsSpining = false;
            //        Timing.RunCoroutine(CoroutineSlowMotionFighting());
            //    }
            //    else
            //    {
            //        if (isDefensing)
            //        {
            //            playerAnimationsController.HighSpeedAnimation();
            //            playerAnimationsController.SetState(2, false);
            //            IsDefensing = false;
            //            IsFighting = false;
            //        }
            //        playerComponentsControl.
            //            PlayerCameraSmooth.CheckVectorForCamera();
            //    }
            //}
            #endregion

            #region Новая реализация. Только 
            if (isMayToLongAttack)
            {
                if (fightVector.magnitude != 0)
                {
                    // Вращение влево, либо вправо
                    if (Math.Abs(fightVector.x) > 0.5f)
                    {
                        Joystick.IsBlock = false;
                        // но в том случае, если мы не в блоке
                        if (!IsDefensing)
                        {
                            IsRotating = true;
                            IsSpining = true;

                            if (fightVector.x >= 0.5f)
                            {
                                IsNormal = true;
                            }
                            else if (fightVector.x <= -0.5f)
                            {
                                IsNormal = false;
                            }

                            BeforeSendAngleToPlayer();
                        }
                    }
                }
                else if (isSpining)
                {
                    playerAnimationsController.HighSpeedAnimation();
                    IsSpining = false;
                    Timing.RunCoroutine(CoroutineSlowMotionFighting());
                }


                if (Joystick.IsBlock)
                {
                    if (!isFighting && !isRotating)
                    {
                        // включаем защиту
                        if (!IsDefensing)
                            playerAnimationsController.HighSpeedAnimation();

                        playerAnimationsController.HighSpeedAnimation();
                        playerAnimationsController.SetState(2, true);
                        IsDefensing = true;
                        //IsFighting = true;
                        playerComponentsControl.PlayerCameraSmooth.CameraZoom();
                    }
                }
                else
                {
                    if (isDefensing)
                    {
                        playerAnimationsController.HighSpeedAnimation();
                        playerAnimationsController.SetState(2, false);
                        IsDefensing = false;
                        IsFighting = false;
                    }
                    playerComponentsControl.
                        PlayerCameraSmooth.CheckVectorForCamera();
                }
            }
            #endregion
        }

        #region Умения персонажа
        /// <summary>
        /// Атакующий рывок. 
        /// Возвращает true, если сработал.
        /// </summary>
        /// <returns></returns>
        public bool SkillLongAttack()
        {
            if (!isFighting && isMayToLongAttack && !isSpining && !isRotating)
            {
                Timing.RunCoroutine(CoroutineForStraightAttack());
                return true;
            }
            return false;
        }

        /// <summary>
        /// Корутина на атакующий рывок
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForStraightAttack()
        {
            isMayToLongAttack = false;
            IsDefensing = false;
            playerAnimationsController.SetState(2, false);
            spiningSpeedInCoroutine = 0;
            IsRotating = false;
            IsFighting = true;
            playerComponentsControl.PlayerController.LongAttackAnimation();
            yield return Timing.WaitForSeconds(0.5f);
            playerComponentsControl.PlayerController.StraightMoving();
            yield return Timing.WaitForSeconds(0.75f);
            playerComponentsControl.PlayerController.StopLongAttack();
        }
        #endregion
    }
}
