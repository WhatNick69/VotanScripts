﻿using GameBehaviour;
using MovementEffects;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerBehaviour
{
    /// <summary>
    /// Столкновения с мобами, со стенами, с препятствиями.
    /// Подъем по лестницам, падения с лестниц.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerCollision
        : MonoBehaviour
    {
        #region Переменные
        [SerializeField, Tooltip("Частота обновления"), Range(0.01f, 0.9f)]
        private float frequencyUpdate;
        [SerializeField,Tooltip("Хранитель компонентов")]
        private PlayerComponentsControl playerComponentControl;
        [SerializeField, Tooltip("Количество лучей для просчета")]
        private int raysCount;
        [SerializeField, Tooltip("Радиус просчета")]
        private float searchingRadius;
        [SerializeField, Tooltip("Ротатор лучей")]
        private Transform rotaterRaycaster;
        private bool[] boolsList;
        private float angle;

        private Rigidbody playerRGB;
        private Ray ray;
        private RaycastHit rayCastHit;
        private Ray rayGround;
        private RaycastHit rayCastHitGround;

        private static string tagNameObstacle = "Obstacle";
        private static string tagNameEnemy = "Enemy";

        private bool isRunning;
        private bool isGrounded;
        private bool isOutside;
        #endregion

        #region Свойства
        public Rigidbody PlayerRGB
        {
            get
            {
                return playerRGB;
            }

            set
            {
                playerRGB = value;
            }
        }

        public bool IsOutside
        {
            get
            {
                return isOutside;
            }

            set
            {
                isOutside = value;
            }
        }

        public bool IsGrounded
        {
            get
            {
                return isGrounded;
            }

            set
            {
                isGrounded = value;
            }
        }
        #endregion

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Start()
        {
            boolsList = new bool[raysCount]; // инициализируем размер массива булей
            PlayerRGB = GetComponent<Rigidbody>();
            angle = 360 / raysCount;
            isGrounded = true;

            Timing.RunCoroutine(CoroutineRaycastSearching());
            Timing.RunCoroutine(CoroutineForCheckPlayerGrounded());
            Timing.RunCoroutine(CoroutineForErrorControlling());
        }

        private IEnumerator<float> CoroutineForCheckPlayerGrounded()
        {
            while (isRunning)
            {
                if (Physics.Raycast(transform.position, -transform.up, out rayCastHitGround, 0.5f))
                {
                    if (!isGrounded)
                    {
                        isGrounded = true;
                    }
                }
                else
                {
                    // Метод, который пускает по диагонали в третьем измерении 4 луча и проверяет на столкновения.
                    isGrounded = false;
                }
                yield return Timing.WaitForSeconds(0.1f);
            }
        }

        public void AddDamageForceToPlayer(Vector3 position)
        {
            PlayerRGB.AddForce(position*15000);
        }

        /// <summary>
        /// Отключить либо включить просчет физики персонажа
        /// </summary>
        public void RigidbodyState(bool flag)
        {
            PlayerRGB.detectCollisions = flag;         
        }

        /// <summary>
        /// Отключаем ригидбади
        /// </summary>
        public void RigidbodyDead()
        {
            PlayerRGB.detectCollisions = false;
            PlayerRGB.useGravity = false;
        }

        /// <summary>
        /// Поиск столкновений
        /// </summary>
        private void RaycastSearchIteration()
        {
            rotaterRaycaster.rotation = Quaternion.Euler(0, 0, 0);
            for (int i = 0; i < raysCount; i++)
            {
                ray = new Ray(rotaterRaycaster.position
                    , rotaterRaycaster.forward);
                if (Physics.Raycast(ray, out rayCastHit, searchingRadius))
                {
                    if (rayCastHit.collider.CompareTag(tagNameObstacle)
                    || rayCastHit.collider.CompareTag(tagNameEnemy))
                    {
                        //Debug.DrawRay(rotaterRaycaster.position, rotaterRaycaster.forward, Color.red, 0.05f);
                        boolsList[i] = false;
                    }                 
                }
                else
                {
                    //Debug.DrawRay(rotaterRaycaster.position, rotaterRaycaster.forward, Color.green, 0.05f);
                    boolsList[i] = true;
                }
                rotaterRaycaster.rotation = Quaternion.Euler(0, rotaterRaycaster.localEulerAngles.y+angle, 0);
            }
        }

        /// <summary>
        /// Проверять направление
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public bool CheckDirection(float angle)
        {
            for (int i = 0;i< raysCount; i++)
                if (this.angle*(i+1) >= angle)
                    if (i == raysCount - 1)
                        if (boolsList[0] && boolsList[i])
                            return true;
                        else
                            return false;
                    else
                        if (boolsList[i] && boolsList[i+1])
                            return true;
                        else
                            return false;
            return false;
        }

        /// <summary>
        /// Пускать лучи в корутине и искать коллизии
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineRaycastSearching()
        {
            isRunning = true;
            while (playerComponentControl.
                PlayerConditions.IsAlive)
            {
                RaycastSearchIteration();

                yield return Timing.WaitForSeconds(frequencyUpdate);
            }
            isRunning = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForErrorControlling()
        {
            while (transform.position.y >= -3)
            {
                if (!isGrounded)
                    isOutside = true;
                else
                    isOutside = false;
                   
                yield return Timing.WaitForSeconds(0.5f);
            }
            playerComponentControl.PlayerConditions.RunDieState();
        }
    }
}
