using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using MovementEffects;
using System.Collections.Generic;
using GameBehaviour;
using Playerbehaviour;

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

        private float currentMagnitude; // текущее значение магнитуды векторов
        //меньше которого используется сглаженное время
        private static float angle; // угол для поворота

        private Vector3 moveVector3;
        private Vector3 tempVectorTransform;
        private bool isUpdating; // надо ли обновлять позицию?
        private float magnitudeTemp;
        private float magnitudeForSpeed;
        private bool isMovingStraight;
        private bool isAliveFromConditions;

        private bool continueCalculateInCoroutine;
        private Vector3 triangleVector;
        #endregion

        #region Get-Set`s
        /// <summary>
        /// Read-Write состояния движения игрока
        /// Если true - корутин работает.
        /// Иначе - он ждет в 3 раза больший промежуток 
        /// времени и не выполняет тело метода
        /// </summary>
        public bool ContinueCalculateInCoroutine
        {
            get
            {
                return continueCalculateInCoroutine;
            }

            set
            {
                continueCalculateInCoroutine = value;
            }
        }

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

        public static float Angle
        {
            get
            {
                return angle;
            }

            set
            {
                angle = value;
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
        #endregion

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Start()
        {
            isAliveFromConditions = true;
            continueCalculateInCoroutine = true;
            isUpdating = true;
            InitialisationOfCoroutines();
            triangleVector = Vector3.zero;
        }

        /// <summary>
        /// Запускаем корутины
        /// </summary>
        private void InitialisationOfCoroutines()
        {
            Timing.RunCoroutine(CoroutineForFixedUpdatePositionAndRotation());
        }

        /// <summary>
        /// Рывок-атака
        /// </summary>
        public void StraightMoving()
        {
            // Включаю рывок
            playerComponentsControl.PlayerAnimationsController.SetState(3, true);
            playerComponentsControl.PlayerAnimationsController
                .HighSpeedAnimation();
            attackTransform.position = 
                new Vector3(attackTransform.position.x,
                playerComponentsControl.PlayerObject.position.y, attackTransform.position.z);
            tempVectorTransform = attackTransform.position;
        }

        /// <summary>
        /// Корутин на обновление позиции и поворота
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForFixedUpdatePositionAndRotation()
        {
            while (isAliveFromConditions)
            {
                UpdateYCoordinate();
                if (!playerComponentsControl.PlayerFight.IsFighting)
                {
                    MovePlayerGetNetPosition();
                    RotatePlayeGetNewRotation();

                    yield return Timing.WaitForSeconds(updateFrequency);
                }
                else
                {
                    yield return Timing.WaitForSeconds(updateFrequency*3);
                }
            }
            playerComponentsControl.PlayerAnimationsController.SetState(5, true);
        }

        /// <summary>
        /// Обновляем
        /// </summary>
        private void Update()
        {
            if (isAliveFromConditions)
                UpdateNewTransformPositionAndRotation();
        }
        
        public bool IsMagnitudeMoreThanValue()
        {
            return currentMagnitude > iddleSize ? true : false;
        }

        public void SetStopPositionFromCollision()
        {
            tempVectorTransform = playerComponentsControl.PlayerObject.position;
        }

        /// <summary>
        /// Интерполируем новые координаты, в том числе и поворот,
        /// которые были получены из Обновляющего Корутина()
        /// </summary>
        private void UpdateNewTransformPositionAndRotation()
        {
            currentMagnitude = (playerComponentsControl.PlayerObject
                .position - tempVectorTransform).magnitude;
            if (!playerComponentsControl.PlayerFight.IsFighting)
            {
                if (IsMagnitudeMoreThanValue())
                {
                    // Если двигаем стик, то плавно разгоняемся и меняем анимацию
                    // на бег. Иначе - плавно замедляемся
                    if (isUpdating)
                    {
                        playerComponentsControl.
                            PlayerAnimationsController
                            .SetSpeedAnimationByRunSpeed(magnitudeForSpeed);
                        playerComponentsControl.PlayerAnimationsController
                            .SetState(0, true);

                        playerComponentsControl.PlayerObject.position =
                            Vector3.MoveTowards(playerComponentsControl.PlayerObject
                            .position, tempVectorTransform,
                                magnitudeTemp * Time.deltaTime);
                    }
                    else
                    {
                        playerComponentsControl.PlayerAnimationsController
                            .SetState(0, false);
                        playerComponentsControl.PlayerObject
                            .position = Vector3.Lerp(playerComponentsControl.PlayerObject.position,
                            tempVectorTransform, moveSpeed * Time.deltaTime);
                    }
                }
                else
                {
                    playerComponentsControl.PlayerAnimationsController
                        .SetState(0, false);
                }

                if (playerComponentsControl.PlayerFight.IsRotating)
                {
                    playerComponentsControl.PlayerModel.localRotation = Quaternion.Slerp(playerComponentsControl.PlayerModel
                        .rotation, Quaternion.Euler(0, angle, 0), rotateSpeed * 2f * Time.deltaTime);

                    // Включаю атаку
                    playerComponentsControl.PlayerAnimationsController
                        .SetState(1,true);
                    // Выключаю бег
                    playerComponentsControl.PlayerAnimationsController
                        .SetState(0, false);
                }
                else
                {
                    playerComponentsControl.PlayerModel.localRotation = Quaternion.Slerp(playerComponentsControl.PlayerModel
                        .rotation, Quaternion.Euler(0, angle, 0), rotateSpeed * Time.deltaTime);

                    // выключаем атаку
                    playerComponentsControl.PlayerAnimationsController
                        .SetState(1, false);
                }
            }
            else if (!playerComponentsControl.PlayerFight.IsDefensing)
            {
                playerComponentsControl.PlayerObject.position = Vector3.Lerp(playerComponentsControl.PlayerObject.position,
                           tempVectorTransform, moveSpeed * Time.deltaTime);
            }
        }

        public void StopLongAttack()
        {
            // выключить рывок
            playerComponentsControl.PlayerAnimationsController.SetState(3, false);
            playerComponentsControl.PlayerAnimationsController.SetState(1, false);
        }

        /// <summary>
        /// Двигаем персонажа вслед за джойстиком
        /// </summary>
        private void MovePlayerGetNetPosition()
        {
            moveVector3 = new Vector3(CrossPlatformInputManager.GetAxis("Horizontal")
               , 0, CrossPlatformInputManager.GetAxis("Vertical"))* moveSpeed;

            if (moveVector3.magnitude >= 0.1f)
            {
                isUpdating = true;
                magnitudeForSpeed = moveVector3.magnitude * 0.5f;
                magnitudeTemp = moveVector3.magnitude*moveSpeed;
                //magnitudeTemp = moveSpeed;

                tempVectorTransform = (moveVector3 + playerComponentsControl.PlayerObject.position);
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
            triangleVector.x = playerComponentsControl.PlayerObject.position.x;
            triangleVector.y = TriaglesRender.GetHightOnY();
            triangleVector.z = playerComponentsControl.PlayerObject.position.z;
            playerComponentsControl.PlayerObject.position = triangleVector;
        }

        /// <summary>
        /// Поворачиваем персонажа вслед за джойстиком
        /// </summary>
        private void RotatePlayeGetNewRotation()
        {
            if (isUpdating && !playerComponentsControl.PlayerFight.IsRotating)
                angle = Mathf.Atan2(moveVector3.x, moveVector3.z) * Mathf.Rad2Deg;
        }                 
    }
}
