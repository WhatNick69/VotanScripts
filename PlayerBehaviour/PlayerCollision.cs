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

        /// <summary>
        /// Отключить либо включить просчет физики персонажа
        /// </summary>
        public void RigidbodyState(bool flag)
        {
            playerRGB.detectCollisions = flag;
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
                if (CheckForRay())
                    playerComponentControl.PlayerController
                        .SetStopPositionFromCollision();

                yield return Timing.WaitForSeconds(frequencyUpdate);
            }
        }

        /// <summary>
        /// Пустить луч вперед и проверить, есть ли поблизости препятствие
        /// </summary>
        /// <returns></returns>
        public bool CheckForRay()
        {
           ray = new Ray(playerModel.position
               , playerModel.forward);

            if (Physics.Raycast(ray, out rayCastHit, 1))
            {
                if (rayCastHit.collider.tag.Equals(tagNameObstacle)
                    || rayCastHit.collider.tag.Equals(tagNameEnemy))
                    return true;
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
