using GameBehaviour;
using MovementEffects;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerBehaviour
{
    /// <summary>
    /// Подъем по лестницам
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerCollision
        : MonoBehaviour
    {
        #region Переменные
        [SerializeField, Tooltip("Частота обновления"), Range(0.01f, 0.9f)]
        private float frequencyUpdate;
        [SerializeField]
        private PlayerComponentsControl playerComponentControl;
        [SerializeField]
        private Transform playerObject;
        private Transform playerModel;
        private Rigidbody playerRGB;

        private Ray ray;
        private RaycastHit rayCastHit;
        private static string tagNameObstacle = "Obstacle";
        private static string tagNameEnemy = "Enemy";
        private float angle;
        private bool forward;
        private bool backForward;
        private bool right;
        private bool left;
        private TriaglesRender triangleRender;
        #endregion

        /// <summary>
        /// Отключить либо включить просчет физики персонажа
        /// </summary>
        public void RigidbodyState(bool flag)
        {
            playerRGB.detectCollisions = flag;         
        }

        /// <summary>
        /// Отключаем ригидбади
        /// </summary>
        public void RigidbodyDead()
        {
            playerRGB.detectCollisions = false;
            playerRGB.useGravity = false;
        }

        /// <summary>
        /// Пускать луч в корутине
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> UpList()
        {
            while (playerComponentControl.
                PlayerConditions.IsAlive)
            {
                if (CheckForRayForward())
                {
                    playerComponentControl.PlayerController
                        .SetStopPositionFromCollision();
                }

               yield return Timing.WaitForSeconds(frequencyUpdate);
            }
        }

        /// <summary>
        /// Проверить препятствие на своем пути. Простейшая реализация
        /// </summary>
        /// <returns></returns>
        public bool CheckForRayForward()
        {
            ray = new Ray(playerModel.position
                , playerModel.forward);
            if (Physics.Raycast(ray, out rayCastHit, 1))
            {
                if (rayCastHit.collider.tag.Equals(tagNameObstacle)
                    || rayCastHit.collider.tag.Equals(tagNameEnemy))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Пускаем луч в 4 стороны и проверяем на наличие препятствий.
        /// Сложная реализация. Не работает.
        /// </summary>
        /// <returns></returns>
        public void CheckDirections()
        {
            forward = true;
            backForward = true;
            right = true;
            left = true;
           // Debug.DrawRay(playerModel.position, playerModel.forward, Color.green, 0.1f);
           // Debug.DrawRay(playerModel.position, -playerModel.forward, Color.green, 0.1f);
           // Debug.DrawRay(playerModel.position, playerModel.right, Color.green, 0.1f);
           // Debug.DrawRay(playerModel.position, -playerModel.right, Color.green, 0.1f);

            ray = new Ray(playerModel.position
                , playerModel.forward);
            if (Physics.Raycast(ray, out rayCastHit, 1))
            {
                if (rayCastHit.collider.tag.Equals(tagNameObstacle)
                    || rayCastHit.collider.tag.Equals(tagNameEnemy))
                {
                    Debug.DrawRay(playerModel.position, playerModel.forward, Color.red, 0.2f);
                    forward = false;
                }
            }

            ray = new Ray(playerModel.position
                , -playerModel.forward);
            if (Physics.Raycast(ray, out rayCastHit, 1))
            {
                if (rayCastHit.collider.tag.Equals(tagNameObstacle)
                || rayCastHit.collider.tag.Equals(tagNameEnemy))
                {
                    Debug.DrawRay(playerModel.position, -playerModel.forward, Color.red, 0.2f);
                    backForward = false;
                }
            }

            ray = new Ray(playerModel.position
                , playerModel.right);
            if (Physics.Raycast(ray, out rayCastHit, 1))
            {
                if (rayCastHit.collider.tag.Equals(tagNameObstacle)
                || rayCastHit.collider.tag.Equals(tagNameEnemy))
                {
                    Debug.DrawRay(playerModel.position, playerModel.right, Color.red, 0.2f);
                    right = false;
                }
            }

            ray = new Ray(playerModel.position
                , -playerModel.right);
            if (Physics.Raycast(ray, out rayCastHit, 1))
            {
                if (rayCastHit.collider.tag.Equals(tagNameObstacle)
                || rayCastHit.collider.tag.Equals(tagNameEnemy))
                {
                    Debug.DrawRay(playerModel.position, -playerModel.right, Color.red, 0.2f);
                    left = false;
                }
            }

            playerComponentControl.PlayerController.SetCorrectDirections
                (forward, backForward, right, left);
        }

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Start()
        {
            playerObject = transform;
            playerRGB = playerObject.GetComponent<Rigidbody>();
            playerModel = playerComponentControl.PlayerModel;
            Timing.RunCoroutine(UpList());
        }
    }
}
