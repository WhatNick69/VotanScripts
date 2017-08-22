using System.Collections.Generic;
using MovementEffects;
using UnityEngine;
using UnityEngine.UI;
using VotanLibraries;

namespace PlayerBehaviour
{
    /// <summary>
    /// Реализовывает визуализацию эффектов при использовании предметов
    /// </summary>
    public class PlayerVisualEffects
        : MonoBehaviour
    {
        #region Переменные и ссылки
        [Header("Общее")]
        [SerializeField, Tooltip("Звук-компонент для старта эффекта")]
        private AudioSource audioSourceStartEffect;
        [SerializeField, Tooltip("Звук-компонент для действия эффекта")]
        private AudioSource audioSourceActionEffect;

        [Header("Эффект молнии")]
        [SerializeField, Tooltip("Изображение для грозы")]
        private Image lightingImage;
        private Color lightColor = new Color(1, 1, 1, 0.19f);
        private Color normalColor = new Color(1, 1, 1, 0);

        [Header("Эффект кровавого экрана")]
        [SerializeField, Tooltip("Изображение крови на экране")]
        private Image bloodImage;
        private RectTransform bloodImageRect;
        private PlayerComponentsControl playerComponentsControl;
        private bool isHiting;
        private bool isMaximum;
        private Color transparencyColor;
        private Color whiteColor;

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
        private Vector3 startPowerEffectTrailPoint;
        private Vector3 finishPowerEffectTrailPoint;
        private Vector2 moveVector;
        private bool isPowerEffectEnabled;
        private float startX;
        private float finishX;
        private float startTrailPowerWidth;
        private TrailRenderer firstPowerEffectTrailRenderer;
        private TrailRenderer secondPowerEffectTrailRenderer;

        [Header("Эффект скорости на ботинках")]
        [SerializeField, Tooltip("Трэил-объект для скорости левого ботинка")]
        private Transform speedEffectTrailLeft;
        [SerializeField, Tooltip("Трэил-объект для скорости правого ботинка")]
        private Transform speedEffectTrailRight;
        [SerializeField, Tooltip("Скорость вращения для эффекта скорости")]
        private float speedSpeed;
        [SerializeField, Tooltip("Радиус вращения для эффекта скорости")]
        private float speedRadius;
        private bool isSpeedEffectEnabled;
        private TrailRenderer speedEffectTrailRendererLeft;
        private TrailRenderer speedEffectTrailRendererRight;
        private float startTrailSpeedWidth;

        [Header("Эффект восстановления здоровья")]
        [SerializeField, Tooltip("Трэил-объект для восстановления здоровья #1")]
        private Transform healthEffectTrail1;
        [SerializeField, Tooltip("Трэил-объект для восстановления здоровья #1")]
        private Transform healthEffectTrail2;
        [SerializeField, Tooltip("Трэил-объект для восстановления здоровья #1")]
        private Transform healthEffectTrail3;
        [SerializeField, Tooltip("Радиус вращения для эффекта восстановления здоровья")]
        private float healthRadius;
        [SerializeField, Tooltip("Скорость вращения для эффекта восстановления здоровья")]
        private float healthSpeed;
        private bool isHealthEffectEnabled;
        private TrailRenderer healthEffectTrailRenderer1;
        private TrailRenderer healthEffectTrailRenderer2;
        private TrailRenderer healthEffectTrailRenderer3;
        private float startTrailHealthWidth;
        #endregion

        /// <summary>
        /// Инициализация 
        /// </summary>
        private void Start() 
        {
            BloodMonitorEffectInitialisation();
            PowerWeaponEffectInitialisation();
            SpeedBootsEffectInitialisation();
            HealthEffectInitialisation();
        }


        /// <summary>
        /// Запустить, либо остановить звук.
        /// state 0: скорость,
        /// state 1: сила,
        /// state 2: здоровье.
        /// </summary>
        /// <param name="isEnable"></param>
        /// <param name="state"></param>
        private void PlayOrDisableAudio(int state = -1, bool isEnable = false)
        {
            if (isEnable)
            {
                switch (state)
                {
                    case 0:
                        playerComponentsControl.PlayerSounder.
                            PlaySpeedEffectAudio(audioSourceStartEffect,true);
                        playerComponentsControl.PlayerSounder.
                            PlaySpeedEffectAudio(audioSourceActionEffect, false);
                        break;
                    case 1:
                        playerComponentsControl.PlayerSounder.
                            PlayPowerEffectAudio(audioSourceStartEffect, true);
                        playerComponentsControl.PlayerSounder.
                            PlayPowerEffectAudio(audioSourceActionEffect, false);
                        break;
                    case 2:
                        playerComponentsControl.PlayerSounder.
                            PlayHealthStartEffectAudio(audioSourceStartEffect);
                        break;
                }
            }
            else
            {
                audioSourceActionEffect.Stop();
                audioSourceStartEffect.Stop();
            }
        }

        #region Эффект кровавого экрана
        /// <summary>
        /// Инициализация для эффекта кровавого экрана
        /// </summary>
        private void BloodMonitorEffectInitialisation()
        {
            transparencyColor = new Color(1, 1, 1, 0);
            whiteColor = new Color(1, 1, 1, 1f);
            playerComponentsControl = GetComponent<PlayerComponentsControl>();
            bloodImageRect = bloodImage.GetComponent<RectTransform>();
        }

        /// <summary>
        /// Обновление
        /// </summary>
        private void Update()
        {
            LerpBloodImageAlpha();
        }

        /// <summary>
        /// Эффект кровавого затемнение при получении урона
        /// </summary>
        public void EventBloodEyesEffect(float frequentHP)
        {
            bloodImageRect.localScale = new Vector3(
                LibraryStaticFunctions.GetValueByFrequent(1, 3, frequentHP),
                LibraryStaticFunctions.GetValueByFrequent(1, 3, frequentHP),
                LibraryStaticFunctions.GetValueByFrequent(1, 3, frequentHP));
            isHiting = true;
        }

        /// <summary>
        /// Интерполяция альфы изображения
        /// </summary>
        private void LerpBloodImageAlpha()
        {
            if (isHiting && !isMaximum)
            {
                bloodImage.color = 
                    Color.LerpUnclamped(bloodImage.color, whiteColor, Time.deltaTime * 10);
                if (bloodImage.color.a >= 0.95f)
                {
                    isMaximum = true;
                }
            }
            else if (isMaximum && isHiting)
            {
                bloodImage.color = Color.LerpUnclamped(bloodImage.color, transparencyColor, Time.deltaTime * 5);
                if (bloodImage.color.a <= 0.05f)
                {
                    bloodImage.color = transparencyColor;
                    isMaximum = false;
                    isHiting = false;
                }
            }
        }
        #endregion

        #region Эффект вспышки на экране от грозы
        /// <summary>
        /// Запуск эффекта грозы
        /// </summary>
        public void FireLightingEffect()
        {
            Timing.RunCoroutine(CoroutineForLightingImageEffect());
        }

        /// <summary>
        /// Корутина для эффекта грозы
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForLightingImageEffect()
        {
            lightingImage.gameObject.SetActive(true);
            lightingImage.color = lightColor;
            yield return Timing.WaitForSeconds(0.05f);
            lightingImage.color = normalColor;
            lightingImage.gameObject.SetActive(false);
        }
        #endregion

        #region Эффект восстановления здоровья
        /// <summary>
        /// Запустить эффект восстановления здоровья
        /// </summary>
        public void PlayHealthEffect()
        {
            if (!isHealthEffectEnabled)
            {
                isHealthEffectEnabled = true;
                PlayOrDisableAudio(2, true);
                StartHealthTrails();

                Timing.RunCoroutine(CoroutineForMoveHealthTrailsEffect());
            }
        }

        /// <summary>
        /// Отключить эффект скорости
        /// </summary>
        public void StopHealthEffect()
        {
            isHealthEffectEnabled = false;
            PlayOrDisableAudio();
            Timing.RunCoroutine(CoroutineForLerpDisableHealthEffect());
        }

        /// <summary>
        /// Корутина для эффекта восстановления здоровья
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForMoveHealthTrailsEffect()
        {
            float angle = 0;
            float y = 0;
            float z = 0;
            float x = 0;
            while (isHealthEffectEnabled)
            {
                Debug.Log("CAL");
                angle += Time.deltaTime;
                x = Mathf.Cos(angle * healthSpeed) * healthRadius;
                y = Mathf.Sin(angle * healthSpeed) * healthRadius;
                z = Mathf.Cos(angle * healthSpeed) * healthRadius;
                healthEffectTrail1.localPosition =
                    new Vector3(healthEffectTrail1.localPosition.x, y, z);
                healthEffectTrail2.localPosition =
                    new Vector3(y, x, z);
                healthEffectTrail3.localPosition =
                    new Vector3(y, -x, z);
                yield return Timing.WaitForOneFrame;
            }
        }

        /// <summary>
        /// Запустить и включить трэилы для восстановления здоровья
        /// </summary>
        private void StartHealthTrails()
        {
            healthEffectTrail1.gameObject.SetActive(true);
            healthEffectTrail2.gameObject.SetActive(true);
            healthEffectTrail3.gameObject.SetActive(true);
            healthEffectTrailRenderer1.startWidth = startTrailHealthWidth;
            healthEffectTrailRenderer2.startWidth = startTrailHealthWidth;
            healthEffectTrailRenderer2.startWidth = startTrailHealthWidth;
        }

        /// <summary>
        /// Инициализация эффекта восстановления здоровья
        /// </summary>
        private void HealthEffectInitialisation()
        {
            healthEffectTrailRenderer1 =
                healthEffectTrail1.GetComponent<TrailRenderer>();
            healthEffectTrailRenderer2 =
                healthEffectTrail2.GetComponent<TrailRenderer>();
            healthEffectTrailRenderer3 =
                healthEffectTrail3.GetComponent<TrailRenderer>();
            startTrailHealthWidth = healthEffectTrailRenderer1.startWidth;
        }

        /// <summary>
        /// Корутина на плавное отключение трэилов скорости
        /// </summary>
        /// <param name="trail"></param>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForLerpDisableHealthEffect()
        {
            int i = 0;
            while (healthEffectTrailRenderer1.startWidth >= 0.01f
                || i >= 100)
            {
                if (isHealthEffectEnabled) yield break;

                i++;
                healthEffectTrailRenderer1.startWidth =
                    Mathf.Lerp(healthEffectTrailRenderer1.startWidth, 0, Time.deltaTime * i);
                healthEffectTrailRenderer2.startWidth =
                    Mathf.Lerp(healthEffectTrailRenderer1.startWidth, 0, Time.deltaTime * i);
                healthEffectTrailRenderer3.startWidth =
                    Mathf.Lerp(healthEffectTrailRenderer1.startWidth, 0, Time.deltaTime * i);
                yield return Timing.WaitForSeconds(0.05f);
            }
            if (!isHealthEffectEnabled)
            {
                healthEffectTrailRenderer1.gameObject.SetActive(false);
                healthEffectTrailRenderer2.gameObject.SetActive(false);
                healthEffectTrailRenderer3.gameObject.SetActive(false);
            }
        }
        #endregion

        #region Эффект скорости
        /// <summary>
        /// Инициализация значения для эффекта скорости
        /// </summary>
        private void SpeedBootsEffectInitialisation()
        {
            speedEffectTrailRendererLeft = 
                speedEffectTrailLeft.GetComponent<TrailRenderer>();
            speedEffectTrailRendererRight = 
                speedEffectTrailRight.GetComponent<TrailRenderer>();
            startTrailSpeedWidth = speedEffectTrailRendererLeft.startWidth;
        }

        /// <summary>
        /// Запустить эффект скорости на ботинках персонажа
        /// </summary>
        public void PlaySpeedEffectBoots()
        {
            if (!isSpeedEffectEnabled)
            {
                isSpeedEffectEnabled = true;
                PlayOrDisableAudio(0,true);

                StartSpeedTrails();

                Timing.RunCoroutine(CoroutineForMoveTrailsSpeedEffect(speedEffectTrailLeft));
                Timing.RunCoroutine(CoroutineForMoveTrailsSpeedEffect(speedEffectTrailRight));
            }
        }

        /// <summary>
        /// Отключить эффект скорости
        /// </summary>
        public void StopSpeedEffectBoots()
        {
            isSpeedEffectEnabled = false;
            PlayOrDisableAudio();

            Timing.RunCoroutine(CoroutineForLerpDisableSpeedEffect());
        }

        /// <summary>
        /// Запуск и включение трэилов скорости
        /// </summary>
        private void StartSpeedTrails()
        {
            speedEffectTrailLeft.gameObject.SetActive(true);
            speedEffectTrailRight.gameObject.SetActive(true);
            speedEffectTrailRendererLeft.startWidth = startTrailSpeedWidth;
            speedEffectTrailRendererRight.startWidth = startTrailSpeedWidth;
        }

        /// <summary>
        /// Корутина на запуск эффекта скорости на ботинках персонажа
        /// </summary>
        /// <param name="trail"></param>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForMoveTrailsSpeedEffect(Transform trail)
        {
            float angle = 0;
            float y = 0;
            float z = 0;

            while (isSpeedEffectEnabled)
            {
                angle += Time.deltaTime;

                y = Mathf.Cos(angle * speedSpeed) * speedRadius;
                z = Mathf.Sin(angle * speedSpeed) * speedRadius;
                trail.localPosition = new Vector3(trail.localPosition.x, y, z);

                yield return Timing.WaitForOneFrame;
            }
        }

        /// <summary>
        /// Корутина на плавное отключение трэилов скорости
        /// </summary>
        /// <param name="trail"></param>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForLerpDisableSpeedEffect()
        {
            int i = 0;
            while (speedEffectTrailRendererLeft.startWidth >= 0.01f
                || i >= 100)
            {
                if (isSpeedEffectEnabled) yield break;

                i++;
                speedEffectTrailRendererLeft.startWidth =
                    Mathf.Lerp(speedEffectTrailRendererLeft.startWidth, 0, Time.deltaTime * i);
                speedEffectTrailRendererRight.startWidth =
                    Mathf.Lerp(speedEffectTrailRendererRight.startWidth, 0, Time.deltaTime * i);
                yield return Timing.WaitForSeconds(0.05f);
            }
            if (!isSpeedEffectEnabled)
            {
                speedEffectTrailRendererLeft.gameObject.SetActive(false);
                speedEffectTrailRendererRight.gameObject.SetActive(false);
            }
        }
        #endregion

        #region Эффект мощности
        /// <summary>
        /// ИНициализация переменных для эффекта мощности на оружие
        /// </summary>
        private void PowerWeaponEffectInitialisation()
        {
            startX = 0;
            finishX = -70;
            firstPowerEffectTrailRenderer = 
                powerEffectTrailFirst.GetComponent<TrailRenderer>();
            secondPowerEffectTrailRenderer =
                powerEffectTrailSecond.GetComponent<TrailRenderer>();
            startTrailPowerWidth = firstPowerEffectTrailRenderer.startWidth;
            moveVector = new Vector2(powerMoveSpeed, 0);
        }

        /// <summary>
        /// Установить цвет трэилов
        /// </summary>
        private void SetTrailsColor()
        {
            powerEffectTrailFirst.GetComponent<TrailRenderer>().startColor 
                = playerComponentsControl.PlayerWeapon.TrailRenderer.startColor;
            powerEffectTrailFirst.GetComponent<TrailRenderer>().endColor
                = playerComponentsControl.PlayerWeapon.TrailRenderer.endColor;
            powerEffectTrailSecond.GetComponent<TrailRenderer>().startColor
                = playerComponentsControl.PlayerWeapon.TrailRenderer.startColor;
            powerEffectTrailSecond.GetComponent<TrailRenderer>().endColor
                = playerComponentsControl.PlayerWeapon.TrailRenderer.endColor;
        }

        /// <summary>
        /// Остановить эффект мощности оружия
        /// </summary>
        public void StopPowerEffectWeapon()
        {
            isPowerEffectEnabled = false;
            PlayOrDisableAudio();
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
                SetTrailsColor();
                PlayOrDisableAudio(1, true);
                StartPowerTrails();

                Timing.RunCoroutine(CoroutineForMoveWeaponPowerTrail(powerEffectTrailFirst,false));
                Timing.RunCoroutine(CoroutineForMoveWeaponPowerTrail(powerEffectTrailSecond,true));
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
        private IEnumerator<float> CoroutineForMoveWeaponPowerTrail(Transform trail,bool isClocked)
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
                trail.localPosition = new Vector3(trail.localPosition.x,y, z);
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
        #endregion
    }
}
