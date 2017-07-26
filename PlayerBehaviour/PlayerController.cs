using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using MovementEffects;
using System.Collections.Generic;
using VotanLibraries;

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
        private Transform playerObjectTransform;
        private Transform playerModel;

        private float currentMagnitude; // текущее значение магнитуды векторов
        //меньше которого используется сглаженное время
        private float angle; // угол для поворота
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

        public float Angle
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
            InitialisationOfCoroutines();
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
                if (!playerComponentsControl.PlayerFight.IsFighting)
                {
                    MovePlayerGetNetPosition();
                    RotatePlayeGetNewRotation();   
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
            playerComponentsControl.PlayerAnimationsController.SetState(5, true);
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

                        playerObjectTransform.position =
                            Vector3.MoveTowards(playerObjectTransform
                            .position, tempVectorTransform,
                                magnitudeTemp * Time.deltaTime);
                    }
                    else
                    {
                        playerComponentsControl.PlayerAnimationsController
                            .SetState(0, false);
                        tempVectorTransform.y = playerObjectTransform.position.y;
                        playerObjectTransform
                            .position = Vector3.Lerp(playerObjectTransform.position,
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
                    playerComponentsControl.PlayerModel.localRotation 
                        = Quaternion.Slerp(playerComponentsControl.PlayerModel
                        .rotation, Quaternion.Euler(0, angle, 0)
                        , rotateSpeed * 2f * Time.deltaTime);

                    // Включаю атаку
                    playerComponentsControl.PlayerAnimationsController
                        .SetState(1,true);
                    // Выключаю бег
                    playerComponentsControl.PlayerAnimationsController
                        .SetState(0, false);
                }
                else
                {
                    playerComponentsControl.PlayerModel.localRotation 
                        = Quaternion.Slerp(playerComponentsControl.PlayerModel
                        .rotation, Quaternion.Euler(0, angle, 0)
                        , rotateSpeed * Time.deltaTime);

                    // выключаем атаку
                    playerComponentsControl.PlayerAnimationsController
                        .SetState(1, false);
                }
            }
            else if (!playerComponentsControl.PlayerFight.IsDefensing)
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
            playerComponentsControl.PlayerAnimationsController.SetState(3, false);
            playerComponentsControl.PlayerAnimationsController.SetState(1, false);
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
            if (!playerComponentsControl.PlayerFight.IsFighting &&
                (playerModel.localPosition.y < 0 
                || playerModel.localPosition.y > 0))
            {
                playerModel.localPosition = Vector3.zero;
            }
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
