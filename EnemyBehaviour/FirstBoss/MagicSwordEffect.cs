using MovementEffects;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyBehaviour
{
    /// <summary>
    /// Магический меч
    /// </summary>
    public class MagicSwordEffect
        : MonoBehaviour
    {
        [Header("Эффект мощности у оружия")]
        [SerializeField, Tooltip("Трэил-объект для эффекта мощности #1")]
        private Transform powerEffectTrailFirst;
        [SerializeField, Tooltip("Трэил-объект для эффекта мощности #1")]
        private Transform powerEffectTrailSecond;
        [SerializeField, Tooltip("Скорость вращения для эффекта мощности")]
        private float powerSpeed;
        [SerializeField, Tooltip("Радиус вращения для эффекта мощности")]
        private float powerRadius;
        [SerializeField, Tooltip("Скорость движения для эффекта мощости")]
        private float powerMoveSpeed;
        [SerializeField, Tooltip("Аудио оружия")]
        private AudioSource auSource;

        private Vector3 startPowerEffectTrailPoint;
        private Vector3 finishPowerEffectTrailPoint;
        private Vector2 moveVector;
        private bool isPowerEffectEnabled;
        [SerializeField]
        private float startX;
        [SerializeField]
        private float finishX;
        private float startTrailPowerWidth;
        private TrailRenderer firstPowerEffectTrailRenderer;
        private TrailRenderer secondPowerEffectTrailRenderer;

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Start()
        {
            firstPowerEffectTrailRenderer =
                powerEffectTrailFirst.GetComponent<TrailRenderer>();
            secondPowerEffectTrailRenderer =
                powerEffectTrailSecond.GetComponent<TrailRenderer>();
            startTrailPowerWidth = firstPowerEffectTrailRenderer.startWidth;
            moveVector = new Vector2(powerMoveSpeed, 0);
        }

        /// <summary>
        /// Остановить эффект мощности оружия
        /// </summary>
        public void StopPowerEffectWeapon()
        {
            isPowerEffectEnabled = false;
            auSource.Stop();
            Timing.RunCoroutine(CoroutineForLerpDisablePowerEffect());
        }

        /// <summary>
        /// Запустить эффект мощности оружия
        /// </summary>
        public void PlayPowerEffectWeapon()
        {
            if (!isPowerEffectEnabled)
            {
                isPowerEffectEnabled = true;
                auSource.Play();
                StartPowerTrails();

                Timing.RunCoroutine(CoroutineForMoveWeaponPowerTrail(powerEffectTrailFirst, false));
                Timing.RunCoroutine(CoroutineForMoveWeaponPowerTrail(powerEffectTrailSecond, true));
            }
        }

        /// <summary>
        /// Запуск и включение трэилов мощности
        /// </summary>
        private void StartPowerTrails()
        {
            powerEffectTrailFirst.gameObject.SetActive(true);
            powerEffectTrailSecond.gameObject.SetActive(true);

            firstPowerEffectTrailRenderer.startWidth = startTrailPowerWidth;
            secondPowerEffectTrailRenderer.startWidth = startTrailPowerWidth;
        }

        /// <summary>
        /// Корутина на движние трэила мощности на оружие
        /// </summary>
        /// <param name="trail"></param>
        /// <param name="isClocked"></param>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForMoveWeaponPowerTrail(Transform trail, bool isClocked)
        {
            bool isDestination = false;
            float angle = 0;
            float y = 0;
            float z = 0;
            moveVector = new Vector2(powerMoveSpeed, 0);
            while (isPowerEffectEnabled)
            {
                if (isClocked)
                    angle += Time.deltaTime;
                else
                    angle -= Time.deltaTime;

                y = Mathf.Cos(angle * powerSpeed) * powerRadius;
                z = Mathf.Sin(angle * powerSpeed) * powerRadius;
                trail.localPosition = new Vector3(trail.localPosition.x, y, z);
                if (!isDestination)
                {
                    if (trail.localPosition.x >= finishX)
                        trail.Translate(-moveVector * Time.deltaTime);
                    else
                        isDestination = true;
                }
                else
                {
                    if (trail.localPosition.x <= startX)
                        trail.Translate(moveVector * Time.deltaTime);
                    else
                        isDestination = false;
                }

                yield return Timing.WaitForOneFrame;
            }
        }

        /// <summary>
        /// Корутина на плавное отключение трэилов скорости
        /// </summary>
        /// <param name="trail"></param>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForLerpDisablePowerEffect()
        {
            int i = 0;
            while (firstPowerEffectTrailRenderer.startWidth >= 0.01f
                || i >= 100)
            {
                if (isPowerEffectEnabled) yield break;

                i++;
                firstPowerEffectTrailRenderer.startWidth =
                    Mathf.Lerp(firstPowerEffectTrailRenderer.startWidth, 0, Time.deltaTime * i);
                secondPowerEffectTrailRenderer.startWidth =
                    Mathf.Lerp(secondPowerEffectTrailRenderer.startWidth, 0, Time.deltaTime * i);
                yield return Timing.WaitForSeconds(0.05f);
            }
            if (!isPowerEffectEnabled)
            {
                firstPowerEffectTrailRenderer.gameObject.SetActive(false);
                secondPowerEffectTrailRenderer.gameObject.SetActive(false);
            }
        }
    }
}
