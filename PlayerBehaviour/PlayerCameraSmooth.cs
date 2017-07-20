using MovementEffects;
using System.Collections.Generic;
using UnityEngine;
using VotanLibraries;

namespace PlayerBehaviour
{
    /// <summary>
    /// Интерполяционное движение камеры следом за персонажем
    /// </summary>
    public class PlayerCameraSmooth
        : MonoBehaviour
    {
        #region Переменные
        [SerializeField, Tooltip("Хранитель компонентов")]
        private PlayerComponentsControl playerComponentsControl;
        [SerializeField,Tooltip("Скорость движения камеры вслед за персонажем"),
            Range(0,10)]
        private float followMoveSpeed;
        [SerializeField, Tooltip("Скорость поворота камеры вслед за персонажем"),
            Range(0, 10)]
        private float followRotateSpeed;
        [SerializeField, Tooltip("Частота обновления позиции слежения"),
            Range(0.01f, 1)]
        private float frequencyUpdate;
        [SerializeField, Tooltip("Дистанция, минимальная для обновления"),
            Range(0.1f,2)]
        private float distanceBetweenDestAndPers;

        private Vector3 standartVectorForCamera;
        private Quaternion targetRotation;
        private Vector3 targetPosition;

        private Transform cameraTransform;
        private Transform playerObjectTransform;
        private bool isUpdating;
        private bool isNormalized;

        private float noiseMoveUpdateSpeed;
        private bool isNoising;
        private float noiseRotateUpdateSpeed;

        [SerializeField]
        private float multiplierNoise;
        #endregion

        #region Свойства
        public bool IsNormalized
        {
            get
            {
                return isNormalized;
            }

            set
            {
                isNormalized = value;
            }
        }
        #endregion

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Start()
        {
            standartVectorForCamera = new Vector3(0, 9f, -8);
            playerObjectTransform =
                playerComponentsControl.PlayerObject;
            cameraTransform =
                playerComponentsControl.PlayerCamera.transform;
            followMoveSpeed *= Time.deltaTime;
            followRotateSpeed *= Time.deltaTime;

            noiseMoveUpdateSpeed = followMoveSpeed * multiplierNoise;
            noiseRotateUpdateSpeed = followRotateSpeed * multiplierNoise;
            Timing.RunCoroutine(CoroutineGetPositionOfPlayer());
        }

        public void DoNoize(float coeff)
        {
            Timing.RunCoroutine(CoroutineForNoizeCamera(coeff));
        }

        public void GetNoizeGamage(float coeff)
        {
            Timing.RunCoroutine(CoroutineForDamageNoize(coeff));
        }

        private IEnumerator<float> CoroutineForDamageNoize(float coeff)
        {
            if (!isNoising)
            {
                Debug.Log("NOIZE_DAMAGE");
                noiseMoveUpdateSpeed = followMoveSpeed * multiplierNoise;
                noiseRotateUpdateSpeed = followRotateSpeed * multiplierNoise;

                isNoising = true;
                float temp = followMoveSpeed;
                float temp2 = followRotateSpeed;
                followMoveSpeed = noiseMoveUpdateSpeed;
                followRotateSpeed = noiseRotateUpdateSpeed;
                float y = 1 + coeff;

                standartVectorForCamera
                    = new Vector3(LibraryStaticFunctions.GetPlusMinusValue(2) * y, 9 * y, -8 *y);
                yield return Timing.WaitForSeconds(0.25f);

                standartVectorForCamera = new Vector3(0, 9, -8);
                yield return Timing.WaitForSeconds(0.25f);
                followMoveSpeed = temp;
                followRotateSpeed = temp2;
                isNoising = false;
            }
        }

        private IEnumerator<float> CoroutineForNoizeCamera(float coeff)
        {
            if (!isNoising)
            {
                noiseMoveUpdateSpeed = followMoveSpeed * multiplierNoise;
                noiseRotateUpdateSpeed = followRotateSpeed * multiplierNoise;

                isNoising = true;
                float temp = followMoveSpeed;
                float temp2 = followRotateSpeed;
                followMoveSpeed = noiseMoveUpdateSpeed;
                followRotateSpeed = noiseRotateUpdateSpeed;
                float y = 1-((coeff - 0.25f)/2);

                standartVectorForCamera
                    = new Vector3(LibraryStaticFunctions.GetPlusMinusValue(2)*y, 9*(1+(y/3)),-8*(1+(y/3)));
                yield return Timing.WaitForSeconds(1);

                standartVectorForCamera = new Vector3(0, 9, -8);
                followMoveSpeed = temp;
                followRotateSpeed = temp2;
                isNoising = false;
            }
        }

        /// <summary>
        /// Зуммировать персонажа при блоке
        /// </summary>
        public void CameraZoom()
        {
            standartVectorForCamera =
                new Vector3(0, 4.5f, -4);
        }

        /// <summary>
        /// Проверить вектор на нормализацию
        /// </summary>
        public void CheckVectorForCamera()
        {
            if (standartVectorForCamera.y != 9)
            {
                Debug.Log("Нормализация");
                isNormalized = false;
            }
        }

        /// <summary>
        /// Интерполяционное следование камеры за объектоа игрока
        /// </summary>
        private void Update()
        {
            if (isUpdating)
            {
                cameraTransform.rotation =
                    Quaternion.Slerp(cameraTransform.rotation
                    , targetRotation, followRotateSpeed);

                cameraTransform.position =
                    Vector3.Lerp(cameraTransform.position,
                    targetPosition, followMoveSpeed);
            }
        }

        /// <summary>
        /// Корутин на обновление позиции
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineGetPositionOfPlayer()
        {
            yield return Timing.WaitForSeconds(frequencyUpdate);
            while (playerComponentsControl.PlayerController.
                IsAliveFromConditions)
            {
                if (!isNormalized)
                {
                    standartVectorForCamera = new Vector3(0, 9f, -8);
                    isNormalized = true;
                }

                targetRotation =
                    Quaternion.LookRotation(playerObjectTransform.position - 
                    cameraTransform.position);
                targetPosition =
                    playerObjectTransform.position + standartVectorForCamera;
                if (Quaternion.Angle(cameraTransform.rotation, targetRotation) >= 3
                    || Vector3.Distance(cameraTransform.position,targetPosition) >= distanceBetweenDestAndPers)
                    isUpdating = true;
                else
                    isUpdating = false;

                yield return Timing.WaitForSeconds(frequencyUpdate);
            }
            AfterDead();
            yield return Timing.WaitForSeconds(3);
            isUpdating = false;
        }

        /// <summary>
        /// Метод, что вызывается после смерти персонажа,
        /// для возвращения камеры в прежнее состояние
        /// </summary>
        private void AfterDead()
        {
            isUpdating = true;
            standartVectorForCamera = new Vector3(0, 9f, -8);
            targetRotation =
                Quaternion.LookRotation(playerObjectTransform.position -
                cameraTransform.position);
            targetPosition =
                playerObjectTransform.position + standartVectorForCamera;
        }
    }
}
