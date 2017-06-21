using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using MovementEffects;
using System.Collections.Generic;

namespace PlayerBehaviour
{
    /// <summary>
    /// Используется для управления персонажем.
    /// Задает движение и поворот.
    /// 
    /// Оптимизировано.
    /// </summary>
    public class PlayerController
        : MonoBehaviour
    {
        #region Переменные
        private Transform playerTransform;
        [SerializeField,Tooltip("Модель персонажа")]
        private Transform bodyTransform;
        [SerializeField,Tooltip("Камера персонажа")]
        private Camera playerCamera;

        [SerializeField, Tooltip("Скорость движения"), Range(1, 25)]
        private float moveSpeed;
        [SerializeField, Tooltip("Плавность движения"), Range(1, 10)]
        private float rotateSpeed;
        [SerializeField, Tooltip("Частота обновления"), Range(0.001f, 0.05f)]
        private float updateFrequency;

        private float currentMagnitude; // текущее значение магнитуды векторов
        //меньше которого используется сглаженное время
        private float angle; // угол для поворота

        private Vector3 moveVector3;
        private Vector3 tempVectorTransform;
        private bool isUpdating; // надо ли обновлять позицию?
        private float magnitudeTemp;

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

        /// <summary>
        /// Read-Write скорости игрока
        /// </summary>
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
        #endregion

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Start()
        {
            continueCalculateInCoroutine = true;
            isUpdating = true;
            playerTransform = GetComponent<Transform>();
            InitialisationOfCoroutines();
        }

        /// <summary>
        /// Выполняется только у локального игрока
        /// </summary>
        private void FixedUpdate()
        {
            // dosomething... maybe
        }

        /// <summary>
        /// Запускаем корутины
        /// </summary>
        private void InitialisationOfCoroutines()
        {
            Timing.RunCoroutine(CoroutineForFixedUpdatePositionAndRotation());
        }

        /// <summary>
        /// Корутин на обновление позиции и поворота
        /// </summary>
        /// <returns></returns>
        IEnumerator<float> CoroutineForFixedUpdatePositionAndRotation()
        {
            while (true)
            {
                if (continueCalculateInCoroutine)
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
            UpdateNewTransformPositionAndRotation();
        }

        /// <summary>
        /// Интерполируем новые координаты, в том числе и поворот,
        /// которые были получены из Обновляющего Корутина()
        /// </summary>
        private void UpdateNewTransformPositionAndRotation()
        {
            currentMagnitude = (playerTransform.position - tempVectorTransform).magnitude;

            if (currentMagnitude > 1f)
                // Если двигаем стик, то плавно разгоняемся
                // иначе плавно замедляемся
                if (isUpdating)
                    playerTransform.position =
                        Vector3.MoveTowards(playerTransform.position, tempVectorTransform,
                            magnitudeTemp * Time.deltaTime);
                else
                    playerTransform.position = Vector3.Lerp(playerTransform.position, tempVectorTransform,
                        moveSpeed*Time.deltaTime);

            bodyTransform.rotation = Quaternion.Lerp(bodyTransform.rotation
                , Quaternion.Euler(0, angle, 0), rotateSpeed * Time.deltaTime);
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
                tempVectorTransform = (moveVector3 + playerTransform.position);
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
            if (isUpdating)
                angle = Mathf.Atan2(moveVector3.x, moveVector3.z) * Mathf.Rad2Deg;
        }                 
    }
}
