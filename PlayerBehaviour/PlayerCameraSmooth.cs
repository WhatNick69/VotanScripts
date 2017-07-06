using MovementEffects;
using Playerbehaviour;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerBehaviour
{
    /// <summary>
    /// Интерполяционное движение камеры следом за персонажем
    /// </summary>
    public class PlayerCameraSmooth
        : MonoBehaviour
    {

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
            Timing.RunCoroutine(CoroutineGetPositionOfPlayer());
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
                isNormalized = false;
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
                    , targetRotation, followRotateSpeed* Time.deltaTime);

                cameraTransform.position =
                    Vector3.Lerp(cameraTransform.position,
                    targetPosition, followMoveSpeed * Time.deltaTime);
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
