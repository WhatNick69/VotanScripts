﻿using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using MovementEffects;
using System.Collections.Generic;
using VotanLibraries;
using System;

namespace PlayerBehaviour
{
    /// <summary>
    /// Используется для управления персонажем.
    /// Задает движение и поворот.
    /// Используется левый стик.
    /// 
    /// Оптимизировано.
    /// </summary>
    public class PlayerController
        : MonoBehaviour
    {
        #region Переменные
        [SerializeField, Tooltip("Позиция для атаки")]
        private Transform attackTransform;
        [SerializeField, Tooltip("Скорость движения"), Range(1, 25)]
        private float moveSpeed;
        [SerializeField, Tooltip("Плавность движения"), Range(1, 10)]
        private float rotateSpeed;
        [SerializeField, Tooltip("Величина задержки движения"), Range(1, 3)]
        private float iddleSize;
        [SerializeField, Tooltip("Частота обновления"), Range(0.001f, 0.1f)]
        private float updateFrequency;
        [SerializeField, Tooltip("Хранитель компонентов")]
        private PlayerComponentsControl playerComponentsControl;

        private PlayerAnimationsController playerAnimationsController;
        private PlayerWeapon playerWeapon;
        private PlayerFight playerFight;

        private Transform playerObjectTransform;
        private Transform playerModel;

        private float currentMagnitude; // текущее значение магнитуды векторов
        //меньше которого используется сглаженное время
        private float tempAngleForSound;

        private Vector3 moveVector3;
        private Vector3 tempVectorTransform;
        private bool isUpdating; // надо ли обновлять позицию?
        private float magnitudeTemp;
        private float magnitudeForSpeed;
        private bool isMovingStraight;
        private bool isAliveFromConditions;

        private Vector3 normalYVector;
        private const string horizontalAxis = "Horizontal";
        private const string verticalAxis = "Vertical";
        private float angle;
        private float angle2;
        private float maxSpinSpeed;
        private bool isNormal;
        private float originalSpinSpeed;
        #endregion

        #region Свойства
        public float MoveSpeed
        {
            get
            {
                return moveSpeed;
            }

            set
            {
                moveSpeed = value;
            }
        }


        public bool IsAliveFromConditions
        {
            get
            {
                return isAliveFromConditions;
            }

            set
            {
                isAliveFromConditions = value;
            }
        }

        public bool IsUpdating
        {
            get
            {
                return isUpdating;
            }

            set
            {
                isUpdating = value;
            }
        }


        public float RotateSpeed
        {
            get
            {
                return rotateSpeed;
            }

            set
            {
                rotateSpeed = value;
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
            }
        }

        public float Angle
        {
            get
            {
                return angle;
            }

            set
            {
                angle = value;
                if (angle > maxSpinSpeed) angle = maxSpinSpeed;
                else if (angle < -maxSpinSpeed) angle = -maxSpinSpeed;
                playerWeapon.SpinSpeed = Math.Abs(angle);
            }
        }

        public float MaxSpinSpeed
        {
            get
            {
                return maxSpinSpeed;
            }

            set
            {
                maxSpinSpeed = value;
                if (maxSpinSpeed > originalSpinSpeed) maxSpinSpeed = originalSpinSpeed;
                else if (maxSpinSpeed < 0) maxSpinSpeed = 0;
            }
        }

        public void SetAngle(float angleTemp)
        {
            angle = angleTemp;
            if (angle > maxSpinSpeed) angle = maxSpinSpeed;
            else if (angle < -maxSpinSpeed) angle = -maxSpinSpeed;
        }
        #endregion

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Start()
        {
            isAliveFromConditions = true;
            isUpdating = true;
            playerObjectTransform = playerComponentsControl.PlayerObject;
            playerModel = playerComponentsControl.PlayerModel;
            normalYVector.y = 0.55f;
            MoveSpeed = LibraryStaticFunctions.DependenceMoveSpeedAndWeaponWeight
                (playerComponentsControl.PlayerWeapon.Weight);
            RotateSpeed = LibraryStaticFunctions.DependenceRotateSpeedAndWeaponWeight
                (playerComponentsControl.PlayerWeapon.Weight);
            originalSpinSpeed = playerComponentsControl.PlayerWeapon.OriginalSpinSpeed; ;
            maxSpinSpeed = playerComponentsControl.PlayerWeapon.OriginalSpinSpeed;

            LibraryStaticFunctions.SetAttackTransformPosition
                (playerComponentsControl.PlayerWeapon, attackTransform);
            playerWeapon = playerComponentsControl.PlayerWeapon;
            playerAnimationsController = playerComponentsControl.PlayerAnimationsController;
            playerFight = playerComponentsControl.PlayerFight;

            InitialisationOfCoroutines();
        }

        /// <summary>
        /// Остановить атакующий рывок во время блока
        /// </summary>
        public void StopAttackWhenDefensing()
        {
            tempVectorTransform = playerObjectTransform.position;
        }

        /// <summary>
        /// Запускаем корутины
        /// </summary>
        private void InitialisationOfCoroutines()
        {
            Timing.RunCoroutine(CoroutineForFixedUpdatePositionAndRotation());
        }

        /// <summary>
        /// Воспроизведение анимации во время атакующего рывка
        /// </summary>
        public void LongAttackAnimation()
        {
            // Включаю рывок
            playerAnimationsController.SetState(3, true);
            playerAnimationsController.HighSpeedAnimation();
        }

        /// <summary>
        /// Атакующий рывок
        /// </summary>
        public void StraightMoving()
        {          
            attackTransform.position =
                new Vector3(attackTransform.position.x,
                playerObjectTransform.position.y, attackTransform.position.z);
            tempVectorTransform = attackTransform.position;
            if (!playerComponentsControl.PlayerCollision.
                CheckDirection(LibraryStaticFunctions.
                CalculateAngleBetweenPointAndVector(tempVectorTransform,
                playerObjectTransform.position)))
            {
                tempVectorTransform = playerObjectTransform.position;
            }
        }

        /// <summary>
        /// Корутин на обновление позиции и поворота
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForFixedUpdatePositionAndRotation()
        {
            yield return Timing.WaitForSeconds(1);
            while (isAliveFromConditions)
            {
                if (!playerFight.IsFighting)
                {
                    MovePlayerGetNetPosition();
                    RotatePlayerGetNewRotation();   
                }
                if (!playerComponentsControl.PlayerCollision.
                    CheckDirection(LibraryStaticFunctions.
                    CalculateAngleBetweenPointAndVector(tempVectorTransform,
                    playerObjectTransform.position)))
                {
                    tempVectorTransform = playerObjectTransform.position;
                }
                UpdateYCoordinate();
                yield return Timing.WaitForSeconds(updateFrequency);
            }
            playerAnimationsController.SetState(5, true);
        }

        /// <summary>
        /// Обновляем
        /// </summary>
        private void Update()
        {
            if (isAliveFromConditions)
            {
                UpdateNewTransformPositionAndRotation();
            }
        }
        
        /// <summary>
        /// Проверить величину магнитуды движения
        /// </summary>
        /// <returns></returns>
        public bool IsMagnitudeMoreThanValue()
        {
            return currentMagnitude > iddleSize ? true : false;
        }

        /// <summary>
        /// Интерполируем новые координаты, в том числе и поворот,
        /// которые были получены из Обновляющего Корутина()
        /// </summary>
        private void UpdateNewTransformPositionAndRotation()
        {
            currentMagnitude = (playerObjectTransform
                .position - tempVectorTransform).magnitude;

            if (!playerFight.IsFighting)
            {
                if (IsMagnitudeMoreThanValue())
                {
                    // Если двигаем стик, то плавно разгоняемся и меняем анимацию
                    // на бег. Иначе - плавно замедляемся
                    if (isUpdating)
                    {
                        playerAnimationsController.SetSpeedAnimationByRunSpeed(magnitudeForSpeed);
                        playerAnimationsController.SetState(0, true);

                        playerObjectTransform.position =
                            Vector3.MoveTowards(playerObjectTransform
                            .position, tempVectorTransform,
                                magnitudeTemp * Time.deltaTime);
                    }
                    else
                    {
                        playerAnimationsController.SetState(0, false);
                        tempVectorTransform.y = playerObjectTransform.position.y;
                        playerObjectTransform
                            .position = Vector3.Lerp(playerObjectTransform.position,
                            tempVectorTransform, moveSpeed * Time.deltaTime);
                    }
                }
                else
                {
                    playerAnimationsController.SetState(0, false);
                }

                if (playerFight.IsRotating)
                {
                        playerComponentsControl.PlayerModel.Rotate(new Vector3(0, angle/4, 0));

                    // Включаю атаку
                    playerAnimationsController.SetState(1,true);
                    // Выключаю бег
                    playerAnimationsController.SetState(0, false);
                }
                else
                {

                    playerComponentsControl.PlayerModel.localRotation 
                        = Quaternion.Slerp(playerComponentsControl.PlayerModel
                        .localRotation, Quaternion.Euler(0, angle2, 0)
                        , rotateSpeed * Time.deltaTime);

                    // выключаем атаку
                    playerAnimationsController.SetState(1, false);
                }
            }
            else if (!playerFight.IsDefensing)
            {
                playerObjectTransform.position 
                    = Vector3.Lerp(playerObjectTransform.position,
                    tempVectorTransform, moveSpeed * Time.deltaTime);
            }
        }

        /// <summary>
        /// Остановить длинную атаку
        /// </summary>
        public void StopLongAttack()
        {
            // выключить рывок
            playerAnimationsController.SetState(3, false);
            playerAnimationsController.SetState(1, false);
            playerFight.IsFighting = false;
            playerFight.IsMayToLongAttack = true;
        }

        /// <summary>
        /// Двигаем персонажа вслед за джойстиком
        /// </summary>
        private void MovePlayerGetNetPosition()
        {
            moveVector3 = new Vector3(CrossPlatformInputManager.GetAxis(horizontalAxis)
               , 0, CrossPlatformInputManager.GetAxis(verticalAxis))* moveSpeed;

            if (moveVector3.magnitude >= 0.1f)
            {
                isUpdating = true;
                magnitudeForSpeed = moveVector3.magnitude * 0.5f;
                magnitudeTemp = moveVector3.magnitude*moveSpeed;

                tempVectorTransform = (moveVector3 + playerObjectTransform.position);
            }
            else
            {
                isUpdating = false;
            }
        }

        /// <summary>
        /// Обновляем позицию по "Y" при заходе на лестницу
        /// </summary>
        private void UpdateYCoordinate()
        {
            if (playerObjectTransform.position.y < 0.55f)
            {
                normalYVector.x = playerObjectTransform.position.x;
                normalYVector.z = playerObjectTransform.position.z;
                playerObjectTransform.position = normalYVector;
            }
            if (!playerFight.IsFighting &&
                (playerModel.localPosition.y < 0 
                || playerModel.localPosition.y > 0))
            {
                playerModel.localPosition = Vector3.zero;
            }
        }

        /// <summary>
        /// Угол после вращения
        /// </summary>
        public void AngleBeforeSpining()
        {
            angle2 = playerComponentsControl.PlayerModel.localEulerAngles.y;
        }

        /// <summary>
        /// Поворачиваем персонажа вслед за джойстиком
        /// </summary>
        private void RotatePlayerGetNewRotation()
        {
            if (isUpdating && !playerFight.IsRotating)
            {
                angle2 = Mathf.Atan2(moveVector3.x, moveVector3.z) * Mathf.Rad2Deg;
            }
        }                 
    }
}
