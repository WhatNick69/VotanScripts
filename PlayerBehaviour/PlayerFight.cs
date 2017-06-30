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
        [SerializeField, Tooltip("Частота обновления"), Range(0.01f, 0.1f)]
        private float updateFrequency;
        [SerializeField, Tooltip("Скорость вращения оружием"), Range(40, 70)]
        private float weaponSpeed;
        [SerializeField, Tooltip("Контроллер игрока")]
        private PlayerController playerController;

        private Vector3 fightVector;
        private float boostValue = 1;
        private Vector3 rotateBodyOfPlayerVector;
        private float sumAngle;

        private static bool isRotating;
        private static bool isFighting;
        private static bool isDefensing;

        private PlayerWeapon myWeapon;

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

        public float WeaponSpeed
        {
            get
            {
                return weaponSpeed;
            }

            set
            {
                weaponSpeed = value;
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
                    PlayerController.Angle += weaponSpeed * fightVector.x;
                }
                // Рывок или защита
                else
                {
                    // Рывок
                    if (fightVector.z > 0)
                    {
                        if (!isFighting)
                        {
                            Timing.RunCoroutine(CoroutineForStraightAttack());
                        }
                    }
                    // Защита
                    else if (fightVector.z < 0)
                    {
                        isRotating = false;
                        isDefensing = true;
                        isFighting = true;
                    }
                }
            }
            else
            {
                if (isDefensing)
                {
                    isDefensing = false;
                    isFighting = false;
                }
                isRotating = false;
            }
        }

        private IEnumerator<float> CoroutineForStraightAttack()
        {
            isFighting = true;
            playerController.StraightMoving();
            yield return Timing.WaitForSeconds(1);
            isFighting = false;
        }
    }
}
