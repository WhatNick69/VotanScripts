using MovementEffects;
using System.Collections.Generic;
using UnityEngine;
using VotanLibraries;
using System;
using UnityStandardAssets.CrossPlatformInput;
using VotanGameplay;

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
           Range(0.01f, 0.1f)]
        private float followMoveSpeed;
        [SerializeField, Tooltip("Скорость поворота камеры вслед за персонажем"),
           Range(0.01f, 0.1f)]
        private float followRotateSpeed;
        [SerializeField, Tooltip("Частота обновления позиции слежения"),
            Range(0.01f, 0.5f)]
        private float frequencyUpdate;
        private float tempFrequencyUpdate;
        [SerializeField, Tooltip("Дистанция, минимальная для обновления"),
            Range(0.1f,2)]
        private float distanceBetweenDestAndPers;

        private Vector3 standartVectorForCamera;
        private Quaternion targetRotation;
        private Vector3 targetPosition;
        private Vector3 defaultVector;
        private Quaternion defaultQuarternion;

        private Transform cameraTransform;
        private Transform playerObjectTransform;
        private bool isUpdating;
        private bool isNormalized;

        private float noiseMoveUpdateSpeed;
        private float noiseRotateUpdateSpeed;
        private bool isNoising;
        private PlayerConditions playerConditions;

        [SerializeField]
        private float multiplierNoise;
        private bool isFocused;
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
            defaultQuarternion = Quaternion.Euler(45, 0, 0);
            playerObjectTransform =
                playerComponentsControl.PlayerObject;
            cameraTransform =
                playerComponentsControl.PlayerCamera.transform;
            tempFrequencyUpdate = frequencyUpdate;
            playerConditions = playerComponentsControl.PlayerConditions;

            Timing.RunCoroutine(CoroutineGetPositionOfPlayer());
        }

        /// <summary>
        /// Трясти камеру
        /// </summary>
        /// <param name="coeff"></param>
        public void DoNoize(float coeff)
        {
            if (!isNoising)
            {
                //Debug.Log(coeff);
                isNoising = true;
                Timing.RunCoroutine(CoroutineForNoizeDoDamage(coeff));
            }
        }

        /// <summary>
        /// Получить урон
        /// </summary>
        /// <param name="coeff"></param>
        public void GetNoizeGamage(float coeff)
        {
            if (!isNoising)
            {
                isNoising = true;
                Timing.RunCoroutine(CoroutineForNoizeGetDamage(coeff));
            }
        }

        /// <summary>
        /// Тряска камеры при нанесении урона
        /// </summary>
        /// <param name="coeff"></param>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForNoizeDoDamage(float coeff)
        {
            frequencyUpdate = 0.05f;

            noiseMoveUpdateSpeed = followMoveSpeed;
            noiseRotateUpdateSpeed = followRotateSpeed;

            followMoveSpeed = followMoveSpeed * multiplierNoise;
            followRotateSpeed = followRotateSpeed * multiplierNoise;
            coeff -= 0.5f;
            coeff /= 2;
            if (coeff > 1) coeff = 1;

            int i = 0;
            while (i < 10)
            {
            standartVectorForCamera
                = new Vector3(LibraryStaticFunctions.GetPlusMinusValue(1 * coeff), 
                LibraryStaticFunctions.GetRangeValue(9, 0.1f* coeff),
                LibraryStaticFunctions.GetRangeValue(-8, 0.1f*coeff));
                yield return Timing.WaitForSeconds(0.05f);
                i++;
            }
            //standartVectorForCamera = new Vector3(0, 9, -8);
            followMoveSpeed = noiseMoveUpdateSpeed;
            followRotateSpeed = noiseRotateUpdateSpeed;

            frequencyUpdate = tempFrequencyUpdate;
            isNoising = false;
        }

        /// <summary>
        /// Тряска камеры при плучении урона
        /// </summary>
        /// <param name="coeff"></param>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForNoizeGetDamage(float coeff)
        {
            frequencyUpdate = 0.05f;
            noiseMoveUpdateSpeed = followMoveSpeed;
            noiseRotateUpdateSpeed = followRotateSpeed;

            followMoveSpeed = followMoveSpeed * multiplierNoise;
            followRotateSpeed = followRotateSpeed * multiplierNoise;

            coeff = LibraryStaticFunctions.GetCoefficientForGetDamageNoize(coeff);

            int i = 0;
            while (i < 5)
            {
                if (!playerComponentsControl.PlayerFight.IsDefensing)
                {
                    standartVectorForCamera
                        = new Vector3(LibraryStaticFunctions.GetPlusMinusValue(5 * coeff),
                        LibraryStaticFunctions.GetRangeValue(9, coeff),
                        LibraryStaticFunctions.GetRangeValue(-8, coeff));
                }
                else
                {
                    standartVectorForCamera
                        = new Vector3(LibraryStaticFunctions.GetPlusMinusValue(5 * coeff),
                        LibraryStaticFunctions.GetRangeValue(4.5f, coeff),
                        LibraryStaticFunctions.GetRangeValue(-4, coeff));
                }
                yield return Timing.WaitForSeconds(0.05f);
                i++;
            }

            followMoveSpeed = noiseMoveUpdateSpeed;
            followRotateSpeed = noiseRotateUpdateSpeed;


            frequencyUpdate = tempFrequencyUpdate;
            isNoising = false;
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
                //Debug.Log("Нормализация");
                isNormalized = false;
            }
        }

        /// <summary>
        /// Интерполяционное следование камеры за объектоа игрока
        /// </summary>
        private void Update()
        {
            if (playerConditions.IsAlive)
            {
                cameraTransform.rotation =
                    Quaternion.Slerp(cameraTransform.rotation
                    , targetRotation, followRotateSpeed);

                cameraTransform.position =
                    Vector3.Lerp(cameraTransform.position,
                    targetPosition, followMoveSpeed);
            }
            else
            {
                cameraTransform.rotation =
                    Quaternion.Slerp(cameraTransform.rotation
                    , defaultQuarternion, followRotateSpeed);

                if (Vector3.Distance(cameraTransform.position,
                    standartVectorForCamera) > 2)
                {
                    cameraTransform.position =
                        Vector3.MoveTowards(cameraTransform.position,
                        standartVectorForCamera, followMoveSpeed * 3);
                }
                else if (Vector3.Distance(cameraTransform.position,
                    standartVectorForCamera) > distanceBetweenDestAndPers)
                {
                    cameraTransform.position =
                        Vector3.Lerp(cameraTransform.position,
                        standartVectorForCamera, followMoveSpeed);
                }
            }
        }

        /// <summary>
        /// Фокус камеры на боссе
        /// </summary>
        /// <param name="boss"></param>
        public void FocusToBoss(Transform boss,float timeToFocus)
        {
            isFocused = true;
            Joystick.IsDragPause = true;
            GameManager.BossMusicWork(true);
            playerComponentsControl.PlayerUI.ShowHUD(false);

            Timing.RunCoroutine(CoroutineForFocusOnBoss(boss, timeToFocus));
        }

        /// <summary>
        /// Корутина для фокусировки камеры на персонаже
        /// </summary>
        /// <param name="boss"></param>
        /// <param name="timeToFocus"></param>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForFocusOnBoss(Transform boss, float timeToFocus)
        {
            Vector3 tempPosition = targetPosition;
            Quaternion tempRotation = targetRotation;
            targetRotation = Quaternion.LookRotation(boss.position - cameraTransform.position);
            targetPosition = boss.position + standartVectorForCamera;

            for (int i = 0;i<timeToFocus/0.05f;i++)
            {
                targetRotation = Quaternion.LookRotation(boss.position - cameraTransform.position);
                targetPosition = boss.position + standartVectorForCamera;
                yield return Timing.WaitForSeconds(0.05f);
            }

            targetRotation = tempRotation;
            targetPosition = tempPosition;
            playerComponentsControl.PlayerUI.ShowHUD(true);
            isFocused = false;
            Joystick.IsDragPause = false;
            GameManager.BossMusicWork(false);
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

                if (!isFocused)
                {
                    targetRotation =
                        Quaternion.LookRotation(playerObjectTransform.position -
                        cameraTransform.position);
                    targetPosition =
                        playerObjectTransform.position + standartVectorForCamera;
                }

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
