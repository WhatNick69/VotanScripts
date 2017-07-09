using MovementEffects;
using Playerbehaviour;
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
        private static string tagStairs = "Stairs";
        private bool onTheFloor = true;
        private float angle;

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
            bool flag1;
            bool flag2;

            while (playerComponentControl.
                PlayerConditions.IsAlive)
            {
                flag1 = false;
                flag2 = false;
                if (CheckForRayForward())
                {
                    playerComponentControl.PlayerController
                        .SetStopPositionFromCollision();
                    flag1 = true;
                }

                if (CheckForRayUp())
                {
                    playerComponentControl.PlayerConditions.RotateConditionBar
                           (true, angle);
                    flag2 = true;
                }
                if (!flag1 && !flag2)
                    playerComponentControl.PlayerConditions.RotateConditionBar();

               yield return Timing.WaitForSeconds(frequencyUpdate);
            }
        }

        /// <summary>
        /// Пустить луч вперед и проверить, есть ли поблизости препятствие.
        /// Пускаем луч вниз и проверяем, лестница ли под нами.
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
                    Debug.Log(true);
                    onTheFloor = true;
                    return true;
                }
                else if (rayCastHit.collider.tag.Equals(tagStairs))
                {
                    Debug.Log(false);
                    angle = rayCastHit.collider.GetComponent<Transform>().localEulerAngles.z;
                    onTheFloor = false;
                }
            }
            else
            {
                onTheFloor = true;
            }
            return false;
        }

        /// <summary>
        /// Пустить луч вниз
        /// </summary>
        /// <returns></returns>
        private bool CheckForRayUp()
        {
            ray = new Ray(playerModel.position
               , -playerModel.up);
            if (Physics.Raycast(ray, out rayCastHit, 1f))
            {
                if (rayCastHit.collider.tag.Equals(tagStairs))
                {
                    angle = rayCastHit.collider.GetComponent<Transform>().localEulerAngles.z;
                    return true; // если внизу нас - лестница
                }
                else if (!onTheFloor)
                {
                    Debug.Log("ЛЕстница внизу");
                    //angle = rayCastHit.collider.GetComponent<Transform>().localEulerAngles.z;
                    Debug.Log(angle);
                    return true; ;
                }
            }
            return false;
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
