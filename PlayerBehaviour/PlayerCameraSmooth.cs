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
        private Vector3 standartVectorForCamera;
        private Quaternion targetRotation;
        private Vector3 targetPosition;

        [SerializeField,Tooltip("Скорость движения камеры вслед за персонажем"),
            Range(0,10)]
        private float followMoveSpeed;
        [SerializeField, Tooltip("Скорость поворота камеры вслед за персонажем"),
            Range(0, 10)]
        private float followRotateSpeed;
        private PlayerController playerController;
        [SerializeField, Tooltip("Частота обновления позиции слежения"),
            Range(0.01f, 1)]
        private float frequencyUpdate;
        [SerializeField, Tooltip("Дистанция, минимальная для обновления"),
            Range(0.1f,2)]
        private float distanceBetweenDestAndPers;

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
                LibraryPlayerPosition.PlayerObjectTransform;
            cameraTransform =
                LibraryPlayerPosition.MainCameraPlayerTransform;
            Debug.Log(LibraryPlayerPosition.Player);
            playerController =
                LibraryPlayerPosition.Player.GetComponent<PlayerController>();
            Timing.RunCoroutine(CoroutineGetPositionOfPlayer());
        }

        public void CameraZoom()
        {
            standartVectorForCamera =
                new Vector3(0, 4.5f, -4);
        }

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
            while (playerController.IsAliveFromConditions)
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
        }
    }
}
