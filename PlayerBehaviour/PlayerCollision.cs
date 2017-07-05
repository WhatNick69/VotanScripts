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
        private static List<GameObject> collisionList;
        [SerializeField, Tooltip("Частота обновления листа столкновенй"), Range(0.01f, 0.9f)]
        private float frequencyUpdate;
        [SerializeField]
        private PlayerComponentsControl playerComponentControl;
        [SerializeField]
        private Transform playerObject;
        private Transform playerModel;
        private Rigidbody playerRGB;

        private Ray ray;
        private RaycastHit rayCastHit;

        /// <summary>
        /// Добавить в лист коллизий
        /// </summary>
        /// <param name="GamObj"></param>
        public static void AddToListColl(GameObject GamObj)
        {
            collisionList.Add(GamObj);
        }

        /// <summary>
        /// Вернуть лист с коллизиями
        /// </summary>
        /// <returns></returns>
        public static List<GameObject> ReturnListColl()
        {
            return collisionList;
        }

        /// <summary>
        /// Включать физику, если есть объект
        /// </summary>
        /// <returns></returns>
        public bool IsPhysicsOn()
        {
            return playerRGB.detectCollisions;
        }

        /// <summary>
        /// Отключить просчет физики
        /// </summary>
        public void DisableRigidbody()
        {
            playerRGB.detectCollisions = false;
        }

        /// <summary>
        /// Удалить из листа с коллизиями
        /// </summary>
        /// <param name="GamOb"></param>
        public static void RemoveFromListCollision(GameObject GamOb)
        {
            collisionList.Remove(GamOb);
        }

        /// <summary>
        /// Проверять коллизии
        /// </summary>
        private void CollisionListUpdate()
        {
            for (int i = 0; i < collisionList.Count; i++)
            {
                if (!collisionList[i])
                {
                    collisionList.Remove(collisionList[i]);
                    continue;
                }

                if (Vector3.Distance(collisionList[i].transform.position, playerObject.position) < 5f)
                {
                    playerRGB.detectCollisions = true;
                    return;
                }
            }
            playerRGB.detectCollisions = false;
        }

        /// <summary>
        /// Проверять лист в корутине
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> UpList()
        {
            while (playerComponentControl.
                PlayerConditions.IsAlive)
            {
                if (CheckForRay())
                {
                    playerComponentControl.PlayerController
                        .SetStopPositionFromCollision() ;
                }
                else
                {
                    CollisionListUpdate();
                }
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
            collisionList = new List<GameObject>();
            collisionList.AddRange(GameObject.FindGameObjectsWithTag("Obstacle"));
            playerRGB = playerObject.GetComponent<Rigidbody>();
            playerModel = playerComponentControl.PlayerModel;
            Timing.RunCoroutine(UpList());
        }
    }
}
