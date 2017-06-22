using MovementEffects;
using System;
using System.Collections;
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
        [SerializeField, Tooltip("Скорость вращения оружием")]
        private float gunSpeed;
        [SerializeField, Tooltip("Модель игрока")]
        private Transform playerBody;
        private Transform player;

        private Vector2 fightVector;
        private float boostValue = 1;
        private Vector3 rotateBodyOfPlayerVector;
        private static bool isFighting;

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
        #endregion

        /// <summary>
        /// Инициализация
        /// </summary>
        void Start()
        {
            player = GetComponent<Transform>();
            InitialisationCoroutineForFightControl();
        }

        /// <summary>
        /// Этот метод позволяет установить скорость вращения оружия.
        /// Саня - юзай его, когда потребуется.
        /// </summary>
        /// <param name="gunSpeed"></param>
        public void SetGunSpeed(float gunSpeed)
        {
            this.gunSpeed = gunSpeed;
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
        IEnumerator<float> CoroutineFightCheckTaps()
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
            fightVector = new Vector2(CrossPlatformInputManager.GetAxis("AttackSide")
              , CrossPlatformInputManager.GetAxis("FightState")) * boostValue;
        }

        private void Update()
        {
           
        }

        /// <summary>
        /// Совершает действие, относительно положению правого стика.
        /// Вращение влево, вправо, рывок или защита.
        /// </summary>
        private void CheckFightState()
        {
            if (fightVector.magnitude != 0)
            {
                isFighting = true;
                // Вращение влево, либо вправо
                if (Math.Abs(fightVector.x) > Math.Abs(fightVector.y))
                {
                    PlayerController.Angle += gunSpeed * fightVector.x;
                }
                // Рывок или защита
                else
                {
                    // Рывок
                    if (fightVector.y > 0)
                    {

                    }
                    // Защита
                    else
                    {

                    }
                }
            }
            else
            {
                //PlayerController.Angle = playerBody.localEulerAngles.y;
                isFighting = false;
            }
        }
    }
}
