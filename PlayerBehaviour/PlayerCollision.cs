using MovementEffects;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerBehaviour
{
    /// <summary>
    /// Столкновения с мобами, со стенами, с препятствиями.
    /// Подъем по лестницам, падения с лестниц.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerCollision
        : MonoBehaviour
    {
        #region Переменные
        [SerializeField, Tooltip("Частота обновления"), Range(0.01f, 0.9f)]
        private float frequencyUpdate;
        [SerializeField,Tooltip("Хранитель компонентов")]
        private PlayerComponentsControl playerComponentControl;
        [SerializeField, Tooltip("Количество лучей для просчета")]
        private int raysCount;
        [SerializeField, Tooltip("Радиус просчета")]
        private float searchingRadius;
        [SerializeField, Tooltip("Ротатор лучей")]
        private Transform rotaterRaycaster;
        [SerializeField, Tooltip("Величина силы, которая достается игроку при получении удара от босса")]
        private float addForceValue;

        private bool[] boolsList;
        private float angle;

        private Rigidbody playerRGB;
        private Ray ray;
        private RaycastHit rayCastHit;
        private Ray rayGround;
        private RaycastHit rayCastHitGround;

        private static string tagNameObstacle = "Obstacle";
        private static string tagNameEnemy = "Enemy";

        private bool isRunning;
        private bool isGrounded;
        private bool isOutside;
        #endregion

        #region Свойства
        public Rigidbody PlayerRGB
        {
            get
            {
                return playerRGB;
            }

            set
            {
                playerRGB = value;
            }
        }

        public bool IsOutside
        {
            get
            {
                return isOutside;
            }

            set
            {
                isOutside = value;
            }
        }

        public bool IsGrounded
        {
            get
            {
                return isGrounded;
            }

            set
            {
                isGrounded = value;
            }
        }
        #endregion

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Start()
        {
            boolsList = new bool[raysCount]; // инициализируем размер массива булей
            PlayerRGB = GetComponent<Rigidbody>();
            angle = 360 / raysCount;
            isGrounded = true;

            Timing.RunCoroutine(CoroutineRaycastSearching());
            Timing.RunCoroutine(CoroutineForCheckPlayerGrounded());
            Timing.RunCoroutine(CoroutineForErrorControlling());
        }

        /// <summary>
        /// Корутина на проверку, находимся ли мы на земле
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForCheckPlayerGrounded()
        {
            while (isRunning)
            {
                if (Physics.Raycast(transform.position, -transform.up, out rayCastHitGround, 0.5f))
                {
                    if (!isGrounded)
                    {
                        isGrounded = true;
                    }
                }
                else
                {
                    DiagonalGroundCheckRaycaster();
                }
                yield return Timing.WaitForSeconds(frequencyUpdate);
            }
        }

        private void DiagonalGroundCheckRaycaster()
        {
            for (int i = 0;i<4;i++)
            {
                rotaterRaycaster.rotation = Quaternion.Euler(30, rotaterRaycaster.localEulerAngles.y + 90 * i, 0);
                ray = new Ray(rotaterRaycaster.position
                  , rotaterRaycaster.forward);

                if (Physics.Raycast(ray, out rayCastHitGround, 0.5f))
                {
                    if (!isGrounded)
                    {
                        isGrounded = true;
                        return;                   
                    }
                }          
            }
            isGrounded = false;
            return;
        }

        /// <summary>
        /// Добавить силу отталкивания по врагу.
        /// </summary>
        /// <param name="position"></param>
        public void AddDamageForceToPlayer(Vector3 position)
        {
            position *= addForceValue;
            position.y *= 1.05f;
            PlayerRGB.AddForce(position,ForceMode.Impulse);
        }

        /// <summary>
        /// Отключаем ригидбади
        /// </summary>
        public void RigidbodyDead(bool isFalling)
        {
            PlayerRGB.detectCollisions = false;
            PlayerRGB.useGravity = isFalling;
            if (!isFalling)
            {
                PlayerRGB.constraints = RigidbodyConstraints.FreezePosition;
            }
        }

        /// <summary>
        /// Поиск столкновений
        /// </summary>
        private void RaycastSearchIteration()
        {
            rotaterRaycaster.rotation = Quaternion.Euler(0, 0, 0);
            for (int i = 0; i < raysCount; i++)
            {
                ray = new Ray(rotaterRaycaster.position
                    , rotaterRaycaster.forward);
                if (Physics.Raycast(ray, out rayCastHit, searchingRadius))
                {
                    if (rayCastHit.collider.CompareTag(tagNameObstacle)
                    || rayCastHit.collider.CompareTag(tagNameEnemy))
                    {
                        boolsList[i] = false;
                    }                 
                }
                else
                {
                    boolsList[i] = true;
                }
                rotaterRaycaster.rotation = Quaternion.Euler(0, rotaterRaycaster.localEulerAngles.y+angle, 0);
            }
        }

        /// <summary>
        /// Проверять направление
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public bool CheckDirection(float angle)
        {
            for (int i = 0;i< raysCount; i++)
                if (this.angle*(i+1) >= angle)
                    if (i == raysCount - 1)
                        if (boolsList[0] && boolsList[i])
                            return true;
                        else
                            return false;
                    else
                        if (boolsList[i] && boolsList[i+1])
                            return true;
                        else
                            return false;
            return false;
        }

        /// <summary>
        /// Пускать лучи в корутине и искать коллизии
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineRaycastSearching()
        {
            isRunning = true;
            while (playerComponentControl.
                PlayerConditions.IsAlive)
            {
                RaycastSearchIteration();

                yield return Timing.WaitForSeconds(frequencyUpdate);
            }
            isRunning = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForErrorControlling()
        {
            while (transform.position.y >= -3)
            {
                if (!isGrounded)
                    isOutside = true;
                else
                    isOutside = false;
                   
                yield return Timing.WaitForSeconds(0.5f);
            }
            playerComponentControl.PlayerConditions.RunDieState(true);
        }
    }
}
