using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using MovementEffects;
using System.Collections.Generic;

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
        [SerializeField, Tooltip("Объект персонажа")]
        private Transform playerObjectTransform;
        [SerializeField,Tooltip("Модель персонажа")]
        private Transform playerModelTransform;
        [SerializeField, Tooltip("Позиция для атаки")]
        private Transform attackTransform;
        [SerializeField, Tooltip("Скорость движения"), Range(1, 25)]
        private float moveSpeed;
        [SerializeField, Tooltip("Плавность движения"), Range(1, 10)]
        private float rotateSpeed;
        [SerializeField, Tooltip("Величина задержки движения"), Range(1, 3)]
        private float iddleSize;
        [SerializeField, Tooltip("Частота обновления"), Range(0.001f, 0.05f)]
        private float updateFrequency;

        private float currentMagnitude; // текущее значение магнитуды векторов
        //меньше которого используется сглаженное время
        private static float angle; // угол для поворота

        private Vector3 moveVector3;
        private Vector3 tempVectorTransform;
        private bool isUpdating; // надо ли обновлять позицию?
        private float magnitudeTemp;
        private bool isMovingStraight;
        private bool isAliveFromConditions;

        private bool continueCalculateInCoroutine;
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

        public Transform PlayerModelTransform
        {
            get
            {
                return playerModelTransform;
            }

            set
            {
                playerModelTransform = value;
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

        public Transform PlayerObjectTransform
        {
            get
            {
                return playerObjectTransform;
            }

            set
            {
                playerObjectTransform = value;
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
            Debug.Log("Рывок");
            attackTransform.position = 
                new Vector3(attackTransform.position.x, 
                playerObjectTransform.position.y, attackTransform.position.z);
            tempVectorTransform = attackTransform.position;
        }

        /// <summary>
        /// Корутин на обновление позиции и поворота
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForFixedUpdatePositionAndRotation()
        {
            while (true)
            {
                if (!PlayerFight.IsFighting)
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
        }

        /// <summary>
        /// Обновляем
        /// </summary>
        private void Update()
        {
            if (isAliveFromConditions)
                UpdateNewTransformPositionAndRotation();
        }

        /// <summary>
        /// Интерполируем новые координаты, в том числе и поворот,
        /// которые были получены из Обновляющего Корутина()
        /// </summary>
        private void UpdateNewTransformPositionAndRotation()
        {
            currentMagnitude = (playerObjectTransform.position - tempVectorTransform).magnitude;

            if (!PlayerFight.IsFighting)
            {
                if (currentMagnitude > iddleSize)
                {
                    // Если двигаем стик, то плавно разгоняемся
                    // иначе плавно замедляемся
                    if (isUpdating)
                        playerObjectTransform.position =
                            Vector3.MoveTowards(playerObjectTransform.position, tempVectorTransform,
                                magnitudeTemp * Time.deltaTime);
                    else
                        playerObjectTransform.position = Vector3.Lerp(playerObjectTransform.position, 
                            tempVectorTransform,moveSpeed * Time.deltaTime);
                }
                playerModelTransform.localRotation = Quaternion.Slerp(playerModelTransform.rotation
                    , Quaternion.Euler(0, angle, 0), rotateSpeed * Time.deltaTime);
            }
            else if (!PlayerFight.IsDefensing)
            {
                playerObjectTransform.position = Vector3.Lerp(playerObjectTransform.position,
                           tempVectorTransform, moveSpeed * Time.deltaTime);
            }
        }

        /// <summary>
        /// Двигаем персонажа вслед за джойстиком
        /// </summary>
        private void MovePlayerGetNetPosition()
        {
            moveVector3 = new Vector3(CrossPlatformInputManager.GetAxis("Horizontal")
               , 0, CrossPlatformInputManager.GetAxis("Vertical")) * moveSpeed;

            if (moveVector3.magnitude >= 0.1f)
            {
                isUpdating = true;
                magnitudeTemp = moveVector3.magnitude*moveSpeed; 
                //magnitudeTemp = moveSpeed;
                tempVectorTransform = (moveVector3 + playerObjectTransform.position);
            }
            else
            {
                isUpdating = false;
            }
        }

        /// <summary>
        /// Поворачиваем персонажа вслед за джойстиком
        /// </summary>
        private void RotatePlayeGetNewRotation()
        {
            if (isUpdating && !PlayerFight.IsRotating)
                angle = Mathf.Atan2(moveVector3.x, moveVector3.z) * Mathf.Rad2Deg;
        }                 
    }
}
