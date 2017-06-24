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
        [SerializeField, Tooltip("Скорость вращения оружием"),Range(40,75)]
        private float weaponSpeed;
        [SerializeField, Tooltip("Модель игрока")]
        private Transform playerBody;
        [SerializeField, Tooltip("Контроллер игрока")]
        private PlayerController playerController;

        private Vector3 fightVector;
        private float boostValue = 1;
        private Vector3 rotateBodyOfPlayerVector;
        private float sumAngle;
        private static bool isRotating;
        private static bool isFighting;
        private bool isControlRightStick;
        private float weaponDamage;
        private DamageType attackType;

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
        #endregion

        /// <summary>
        /// Инициализация
        /// </summary>
        void Start()
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
              ,0, CrossPlatformInputManager.GetAxis("FightState")) * boostValue;
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
                        Debug.Log("Перед проверкой");
                        if (!isFighting)
                        {
                            Debug.Log("ЗАпускаем корутин");
                            Timing.RunCoroutine(CoroutineForStraightAttack());
                        }
                    }
                    // Защита
                    else
                    {

                    }
                }
            }
            else
            {
                isRotating = false;
                isFighting = false;
            }
        }

        private IEnumerator<float> CoroutineForStraightAttack()
        {
            isFighting = true;
            playerController.StraightMoving(fightVector*10);
            yield return Timing.WaitForSeconds(2);
            isFighting = false;
        }

        /// <summary>
        /// Принять параметры оружия
        /// 
        /// Саня, этот метод для тебя 
        /// кек
        /// </summary>
        /// <param name="weaponDamage"></param>
        /// <param name="attackType"></param>
        /// <param name="weaponSpeed"></param>
        public void GetWeaponParameters(float weaponDamage,DamageType attackType,float weaponSpeed)
        {
            this.weaponDamage = weaponDamage;
            this.attackType = attackType;
            this.weaponSpeed = weaponSpeed;
        }
    }
}
