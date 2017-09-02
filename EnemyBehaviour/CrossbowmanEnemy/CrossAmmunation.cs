using AbstractBehaviour;
using MovementEffects;
using PlayerBehaviour;
using System.Collections.Generic;
using UnityEngine;
using VotanInterfaces;
using VotanLibraries;

namespace EnemyBehaviour
{
    /// <summary>
    /// Аммуниция класса врагов "Арбалетчик" - болт
    /// </summary>
    public class CrossAmmunation
        : MonoBehaviour, IAmmunation
    {
        #region Переменные
        [SerializeField, Tooltip("Частота просчета столкновения"), Range(0.05f, 0.5f)]
        private float checkLatency;
        [SerializeField, Tooltip("Минимальная дистанция для начала просчетов"), Range(1, 5)]
        private float minimumDistanceToPlayer;
        [SerializeField, Tooltip("Минимальная дистанция для попадания"), Range(0.1f, 3)]
        private float minimumDistanceToPenetration;

        private bool isDestinationed;
        private bool isRestarted;

        private float moveSpeed;
        private float timeToRestart;
        private float dmg;

        private Vector3 startRotation;
        private Vector3 startPosition;
        private Vector3 moveVector;
        private Vector3 ourRandomPositionInPlayerBody;

        private Transform playerModel;
        private Transform startParent;

        private PlayerBonesManager playerBonesManager;
        private IPlayerBehaviour playerComponentsControl;

        private TrailRenderer trailRender;
        private AudioSource auSource;
        #endregion

        #region Свойства
        public bool IsDestinationed
        {
            get
            {
                return isDestinationed;
            }

            set
            {
                isDestinationed = value;
            }
        }

        public Transform PlayerModel
        {
            get
            {
                return playerModel;
            }

            set
            {
                playerModel = value;
            }
        }

        public Transform StartParent
        {
            get
            {
                return startParent;
            }

            set
            {
                startParent = value;
            }
        }

        public bool IsRestarted
        {
            get
            {
                return isRestarted;
            }

            set
            {
                isRestarted = value;
            }
        }
        #endregion

        /// <summary>
        /// Инициализация
        /// </summary>
        public void InitialisationAmmunationElement()
        {
            isDestinationed = true;
            isRestarted = true;

            startParent = transform.parent;
            startPosition = transform.localPosition;
            startRotation = transform.localEulerAngles;
            moveVector = new Vector3(0, 0, 1);
            trailRender = GetComponentInChildren<TrailRenderer>(true);
            auSource = GetComponent<AudioSource>();

            gameObject.SetActive(false);
        }

        /// <summary>
        /// Запустить объект
        /// </summary>
        public void FireAmmoObject
            (IPlayerBehaviour playerComponentsControl, float dmg,float moveSpeed, float timeToRestart)
        {
            if (isDestinationed)
            {
                this.moveSpeed = moveSpeed;
                this.timeToRestart = timeToRestart;
                this.playerComponentsControl = playerComponentsControl;
                this.dmg = dmg;

                isDestinationed = false;
                isRestarted = false;
                transform.SetParent(null);
                ActiveForTrailRender(true);
                transform.gameObject.SetActive(true);

                playerBonesManager = playerComponentsControl.PlayerBonesManager;
                playerModel = playerComponentsControl.PlayerAttack.PlayerPoint;
                ourRandomPositionInPlayerBody = 
                    new Vector3(LibraryStaticFunctions.GetPlusMinusValue(3),
                    LibraryStaticFunctions.GetPlusMinusValue(3), 
                    LibraryStaticFunctions.GetPlusMinusValue(3));

                Timing.RunCoroutine(CoroutineForMovingAmmoObject());
                Timing.RunCoroutine(CoroutineForCheckInPlayerPenetration());
                Timing.RunCoroutine(CoroutineForRestartTimer());
            }
        }

        /// <summary>
        /// Рестарт единицы аммуниции
        /// </summary>
        public void RestartAmmo()
        {
            playerModel = null;
            isDestinationed = true;
            isRestarted = true;

            transform.SetParent(startParent);
            ActiveForTrailRender(false);
            transform.gameObject.SetActive(false);
            transform.localPosition = startPosition;
            transform.localEulerAngles = startRotation;
        }

        /// <summary>
        /// Корутина на проверку столкновения объекта аммуниции
        /// с телом игрока
        /// </summary>
        /// <returns></returns>
        public IEnumerator<float> CoroutineForCheckInPlayerPenetration()
        {
            Transform tempTransform;
            while (!isDestinationed)
            {
                if (Vector3.Distance(transform.position, playerModel.position) < minimumDistanceToPlayer)
                {
                    tempTransform = playerBonesManager.GetClosestBone(transform.position);
                    if (Vector3.Distance(transform.position, playerModel.position) < minimumDistanceToPenetration)
                    {
                        playerComponentsControl.PlayerConditions.GetDamage(dmg);
                        AbstractSoundStorage.PlayArrowHitAudio(auSource);
                        transform.SetParent(tempTransform);
                        transform.localPosition = ourRandomPositionInPlayerBody;
                        ActiveForTrailRender(false);
                        isDestinationed = true;
                        yield break;
                    }
                    yield return Timing.WaitForOneFrame;
                }
                else
                {
                    yield return Timing.WaitForSeconds(checkLatency);
                }
            }
        }

        /// <summary>
        /// Корутина для движения едиинцы аммуниции
        /// </summary>
        /// <returns></returns>
        public IEnumerator<float> CoroutineForMovingAmmoObject()
        {
            SetAccurate();
            while (!isDestinationed)
            {
                transform.Translate(moveVector * Time.deltaTime * moveSpeed);
                yield return Timing.WaitForOneFrame;
            }
        }

        /// <summary>
        /// Задать точность
        /// </summary>
        private void SetAccurate()
        {
            Quaternion lerpRotationQuar = Quaternion.LookRotation
                (playerModel.position - transform.position);

            lerpRotationQuar.eulerAngles =
                new Vector3(lerpRotationQuar.eulerAngles.x + LibraryStaticFunctions.GetPlusMinusValue(2),
                lerpRotationQuar.eulerAngles.y + LibraryStaticFunctions.GetPlusMinusValue(2),
                lerpRotationQuar.eulerAngles.z);
            transform.rotation = lerpRotationQuar;
        }

        /// <summary>
        /// Корутина на перезапуск снаряда
        /// </summary>
        /// <returns></returns>
        public IEnumerator<float> CoroutineForRestartTimer()
        {
            int iterations = (int)((timeToRestart+1)/0.5f);
            bool isDes = false;
            for (int i = 0;i< iterations;i++)
            {
                if (isDestinationed)
                {
                    isDes = true;
                }
                yield return Timing.WaitForSeconds(0.5f);
            }

            if (isDes)
            {
                yield return Timing.WaitForSeconds(timeToRestart);
                RestartAmmo();
            }
            else
            {
                RestartAmmo();
            }
        }

        /// <summary>
        /// Активация трэила
        /// </summary>
        /// <param name="flag"></param>
        public void ActiveForTrailRender(bool flag)
        {
            if (flag)
                trailRender.enabled = flag;
            else
                Timing.RunCoroutine(CoroutineForDisableTrailRender(false));
        }

        /// <summary>
        /// Отключение трэила
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForDisableTrailRender(bool flag)
        {
            yield return Timing.WaitForSeconds(1);
            trailRender.enabled = flag;
        }
    }
}
