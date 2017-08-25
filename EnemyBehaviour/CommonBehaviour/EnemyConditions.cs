using AbstractBehaviour;
using GameBehaviour;
using MovementEffects;
using PlayerBehaviour;
using System.Collections.Generic;
using UnityEngine;
using VotanGameplay;
using VotanInterfaces;
using VotanLibraries;

namespace EnemyBehaviour
{
    /// <summary>
    /// Описывает состояние врага
    /// </summary>
    [RequireComponent(typeof(IEnemyBehaviour))]
    public class EnemyConditions 
        : AbstractObjectConditions, IEnemyConditions, IObjectFitBat
    {
        #region Переменные
        [SerializeField, Tooltip("Бар для здоровья")]
        private GameObject healthBar;
        [SerializeField, Tooltip("Частота обновления поворота биллборда врага"),Range(0.01f,0.1f)]
        private float billboardRotateFrequency;

        [SerializeField,Tooltip("Защита от холода"),Range(0, 0.9f)]
        private float frostResistance;
        [SerializeField, Tooltip("Защита от огня"), Range(0, 0.9f)]
        private float fireResistance;
        [SerializeField, Tooltip("Защита от электричества"), Range(0, 0.9f)]
        private float electricResistance;
        [SerializeField, Tooltip("Защита от ударов"), Range(0, 0.9f)]
        private float physicResistance;
        [SerializeField, Tooltip("Тень")]
        private Transform blobShadow;
        [SerializeField, Tooltip("Размер для нормальной тени"), Range(1.25f, 3)]
        private float sizeForNormalShadow;
        [SerializeField, Tooltip("Размер для малой тени"), Range(0.6f, 2)]
        private float sizeForLittleShadow;

        [SerializeField, Tooltip("Электрическое управление тенью")]
        private ElectricityColorInterfaceChanger electricityColorInterfaceChanger;

        private Transform cameraTransform;

        private IAIMoving enemyMove;
        protected IEnemyBehaviour enemyAbstract;

        protected bool isMayGetDamage = true;
        protected bool isFrozen; // заморожен ли
        private bool isShocked; // шокирован электричеством ли
        private bool isBurned; // жарится ли

        private Vector3 normalShadowSize;
        private Vector3 littleShadowSize;
        private bool isDownInterfaceTransformHasBeenChanged;
        #endregion

        #region Свойства
        public bool IsFrozen
        {
            get
            {
                return isFrozen;
            }

            set
            {
                isFrozen = value;
            }
        }

        public bool IsBurned
        {
            get
            {
                return isBurned;
            }

            set
            {
                isBurned = value;
            }
        }

        public bool IsShocked
        {
            get
            {
                return isShocked;
            }

            set
            {
                isShocked = value;
            }
        }

        public bool IsDownInterfaceTransformHasBeenChanged
        {
            get
            {
                return isDownInterfaceTransformHasBeenChanged;
            }

            set
            {
                isDownInterfaceTransformHasBeenChanged = value;
            }
        }

        public bool IsMayGetDamage
        {
            get
            {
                return isMayGetDamage;
            }

            set
            {
                isMayGetDamage = value;
            }
        }

        public float FrostResistance
        {
            get
            {
                return frostResistance;
            }

            set
            {
                frostResistance = value;
            }
        }

        public float FireResistance
        {
            get
            {
                return fireResistance;
            }

            set
            {
                fireResistance = value;
            }
        }

        public float ElectricResistance
        {
            get
            {
                return electricResistance;
            }

            set
            {
                electricResistance = value;
            }
        }

        public float PhysicResistance
        {
            get
            {
                return physicResistance;
            }

            set
            {
                physicResistance = value;
            }
        }
        #endregion

        /// <summary>
        /// Инициализация
        /// </summary>
        public virtual void Start()
        {
            enemyAbstract = GetComponent<IEnemyBehaviour>();
            enemyMove = enemyAbstract.EnemyMove;
            normalShadowSize = new Vector3(sizeForNormalShadow, 
                sizeForNormalShadow, sizeForNormalShadow);
            littleShadowSize = new Vector3(sizeForLittleShadow,
                sizeForLittleShadow, sizeForLittleShadow);

            electricityColorInterfaceChanger.SpriteRendererObject = 
                blobShadow.GetComponent<SpriteRenderer>();       
        }

        /// <summary>
        /// Рестарт врага
        /// </summary>
        public virtual void RestartEnemyConditions()
        {
            IsAlive = true;
            ActiveDownInterface(true);
            RestartFiller();
            initialisatedHealthValue = healthValue;
            colorChannelRed = 0;
            colorChannelGreen = 1;
            FindCameraInScene();
            MainBarCanvas.gameObject.SetActive(true);
            GetComponent<BoxCollider>().enabled = true;
            Timing.RunCoroutine(CoroutineBarBillboard());
        }

        /// <summary>
        /// Найти камеру на сцене
        /// </summary>
        public void FindCameraInScene()
        {
            // Будет откомментированно при сетевой разработке
            // if (isClient) {
            cameraTransform = GameObject.
                FindGameObjectWithTag("MainCamera").transform;
            // }
        }

        /// <summary>
        /// Поворачивает биллборд вслед за камерой игрока
        /// </summary>
        public IEnumerator<float> CoroutineBarBillboard()
        {
            while (IsAlive)
            {
                yield return Timing.WaitForSeconds(billboardRotateFrequency);
                if (cameraTransform != null)
                    healthBar.transform.LookAt(cameraTransform);
                else
                    FindCameraInScene();
            }
        }

        /// <summary>
        /// Поворачивать тень под врагом
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="angle"></param>
        public void RotateConditionBar(bool flag, float angle)
        {
            if (flag && !IsDownInterfaceTransformHasBeenChanged)
            {
                blobShadow.localScale = littleShadowSize;
                IsDownInterfaceTransformHasBeenChanged = true;
            }
            else if (!flag && IsDownInterfaceTransformHasBeenChanged)
            {
                blobShadow.localScale = normalShadowSize;
                IsDownInterfaceTransformHasBeenChanged = false;
            }
        }

        /// <summary>
        /// Отключить тень
        /// </summary>
        public void ActiveDownInterface(bool flag)
        {
            blobShadow.gameObject.SetActive(flag);
        }

        /// <summary>
        /// Состояние  смерти
        /// </summary>
        /// <returns></returns>
        public override IEnumerator<float> DieState()
        {
            IsAlive = false;
            enemyAbstract.AbstractObjectSounder.PlayDeadAudio();
            //enemyAbstract.EnemyAnimationsController.DisableAllStates();
            enemyAbstract.EnemyAnimationsController.SetSpeedAnimationByRunSpeed(0.5f);
            enemyAbstract.EnemyAnimationsController.SetState(3, true);
            enemyAbstract.EnemyAnimationsController.PlayDeadNormalizeCoroutine();
            MainBarCanvas.gameObject.SetActive(false);
            enemyAbstract.EnemyMove.DisableAgent();
            GetComponent<BoxCollider>().enabled = false;

            while (!enemyAbstract.EnemyAnimationsController.IsDowner)
                yield return Timing.WaitForSeconds(0.5f);
            EnemyCreator.ReturnEnemyToStack(enemyAbstract.EnemyNumber);
        }

        /// <summary>
        /// Получить урон с сопротивлением
        /// </summary>
        /// <param name="dmg"></param>
        /// <param name="typeOfDamage"></param>
        /// <returns></returns>
        public float GetDamageWithResistance(float dmg, float gemPower,
            IWeapon weapon)
        {
            float damage = 0;
            switch (weapon.GemType)
            {
                // ЭЛЕКТРИЧЕСТВО
                case GemType.Electric:
                    RunCoroutineForGetElectricDamage(dmg, gemPower, weapon); 
                    damage = dmg * (1 - electricResistance);
                return damage;

                // ОГОНЬ
                case GemType.Fire:
                    damage = dmg * (1 - fireResistance);
                    RunFireDamage(damage, weapon);
                return damage;

                // ЛЁД
                case GemType.Frozen:
                    if (!isFrozen)
                    {
                        if (LibraryStaticFunctions.MayableToBeFreezy(gemPower))
                        {
                            RunCoroutineForFrozenDamage(dmg, weapon);
                        }
                        else
                        {
                            // оттолкнуть врага
                            enemyAbstract.Physicffect.
                                EventEffectWithoutDefenceBonus(weapon); 
                        }
                    }
                return dmg * (1 - frostResistance);

                // ФИЗИКА
                case GemType.Powerful:
                    RunCoroutineForPhysicDamage(weapon);
                return dmg * (1 - physicResistance);

                default:
                    return dmg * (1 - physicResistance);
            }
        }

        /// <summary>
        /// Получить урон без эффекта
        /// </summary>
        /// <param name="dmg"></param>
        /// <param name="weapon"></param>
        /// <returns></returns>
        public float GetDamageWithResistanceWithoutEffect(float dmg,IWeapon weapon)
        {
            switch (weapon.GemType)
            {
                // ЭЛЕКТРИЧЕСТВО
                case GemType.Electric:
                    return dmg * (1 - electricResistance); ;

                // ОГОНЬ
                case GemType.Fire:
                    return dmg * (1 - fireResistance); ;

                // ЛЁД
                case GemType.Frozen:
                    return dmg * (1 - frostResistance);

                // ФИЗИКА
                case GemType.Powerful:
                    return dmg * (1 - physicResistance);

                default:
                    return dmg * (1 - physicResistance);
            }
        }

        #region Методы для получения разного вида урона
        /// <summary>
        /// Получить урон в рукопашном бою
        /// </summary>
        /// <param name="dmg"></param>
        public virtual bool GetDamage(float dmg, float gemPower
            , IWeapon weapon,bool isSuperAttack)
        {
            if (isMayGetDamage)
            {
                if (isSuperAttack)
                {
                    enemyAbstract.AbstractObjectSounder.PlayGetDamageAudio();
                    Timing.RunCoroutine(CoroutineForGetDamage());

                    enemyAbstract.Physicffect.EventEffectRageAttack(weapon);

                    dmg = GetDamageWithResistanceWithoutEffect(dmg,weapon);

                    if (HealthValue <= 0) return false;
                    HealthValue -=
                        LibraryStaticFunctions.GetRangeValue(dmg, 0.1f);
                    if (HealthValue <= 0)
                        enemyAbstract.ScoreAddingEffect.EventEffect(weapon);

                    weapon.WhileTime();
                    return true;
                }
                else if (weapon.SpinSpeed / weapon.OriginalSpinSpeed >= 0.1f)
                {
                    enemyAbstract.AbstractObjectSounder.PlayGetDamageAudio();
                    Timing.RunCoroutine(CoroutineForGetDamage());

                    /* Если это электрический удар в рукопашную - отодвигаем противника.
                     Молния не должна иметь право отодвигать врага. */
                    if (weapon.GemType == GemType.Electric)
                        enemyAbstract.Physicffect.EventEffectWithoutDefenceBonus(weapon);

                    dmg = GetDamageWithResistance(dmg, gemPower, weapon);

                    if (HealthValue <= 0) return false;
                    HealthValue -=
                        LibraryStaticFunctions.GetRangeValue(dmg, 0.1f);
                    if (HealthValue <= 0)
                        enemyAbstract.ScoreAddingEffect.EventEffect(weapon);

                    weapon.WhileTime();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Получить урон от молнии
        /// </summary>
        /// <param name="dmg"></param>
        /// <param name="gemPower"></param>
        /// <param name="dmgType"></param>
        /// <param name="weapon"></param>
        public void GetDamageElectricity(float dmg, float gemPower
            , IWeapon weapon)
        {
            Timing.RunCoroutine(CoroutineForGetDamage(true));
            dmg = GetDamageWithResistance(dmg, gemPower, weapon);

            if (HealthValue <= 0) return;
            HealthValue -=
                LibraryStaticFunctions.GetRangeValue(dmg, 0.1f);
            if (HealthValue <= 0)
            {
                enemyAbstract.ScoreAddingEffect.EventEffect(weapon);
            }
        }

        /// <summary>
        /// Может ли враг получать урон?
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator<float> CoroutineForGetDamage(bool isLongAttack = false, 
            float dmg = 0)
        {
            if (!isLongAttack)
                isMayGetDamage = false;

            enemyAbstract.EnemyAnimationsController.SetState(2, true);
            yield return Timing.WaitForSeconds(frequencyOfGetDamage);
            enemyAbstract.EnemyAnimationsController.SetState(2, false);

            if (!isLongAttack)
                isMayGetDamage = true;
        }

        /// <summary>
        /// Корутина на получение электрического удара
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="gemPower"></param>
        /// <param name="weapon"></param>
        public void RunCoroutineForGetElectricDamage(float damage,
            float gemPower, IWeapon weapon)
        {
            Timing.RunCoroutine(CoroutineForElectricDamage
                (damage, gemPower, weapon));
        }

        /// <summary>
        /// Корутина, свидетельствующая о том, что враг шокирован электричеством.
        /// Шок действует ровно на 1 секунду. Пока что так. Так легче.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<float> CoroutineForElectricDamage(float damage,
            float gemPower, IWeapon weapon)
        {
            isShocked = true;
            electricityColorInterfaceChanger.ElectricityBlobShadow();
            enemyAbstract.ElectricEffect.EventEffect(damage, gemPower, weapon);
            yield return Timing.WaitForSeconds(1);
            isShocked = false;
        }

        /// <summary>
        /// Корутина на получение электрического удара
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="gemPower"></param>
        /// <param name="weapon"></param>
        public void RunCoroutineForPhysicDamage(IWeapon weapon)
        {
            Timing.RunCoroutine(CoroutineForPhysicDamage(weapon));
        }

        /// <summary>
        /// Корутина, свидетельствующая о том, что враг шокирован электричеством.
        /// Шок действует ровно на 1 секунду. Пока что так. Так легче.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<float> CoroutineForPhysicDamage(IWeapon weapon)
        {
            enemyAbstract.Physicffect.EventEffect(weapon);
            yield return Timing.WaitForSeconds(1);
        }

        /// <summary>
        /// Запустить корутины для замораживающего эффекта
        /// </summary>
        public void RunCoroutineForFrozenDamage(float damage,IWeapon weapon)
        {
            Timing.RunCoroutine(CoroutineForFrozenDamage(damage,weapon));
        }

        /// <summary>
        /// Замедление врага от холода
        /// </summary>
        /// <returns></returns>
        public IEnumerator<float> CoroutineForFrozenDamage(float damage,IWeapon weapon)
        {
            enemyAbstract.EnemyAnimationsController.SetState(2, true);
            enemyMove.SetNewSpeedOfNavMeshAgent(0, 0);
            enemyAbstract.EnemyAnimationsController.SetSpeedAnimationByRunSpeed(0);
            float time = enemyAbstract.IceEffect.EventEffect(damage, weapon);
            enemyAbstract.EnemyMove.DisableAgent();

            IsFrozen = true;
            yield return Timing.WaitForSeconds(time);
            IsFrozen = false;

            if (IsAlive)
            {
                enemyMove.SetNewSpeedOfNavMeshAgent(enemyMove.AgentSpeed,
                    enemyMove.RotationSpeed);
                enemyAbstract.EnemyAnimationsController.SetSpeedAnimationByRunSpeed(0.5f);
                enemyAbstract.EnemyMove.Agent.enabled = true;
                enemyAbstract.EnemyAnimationsController.SetState(2, false);
            }
            else
            {
                enemyAbstract.EnemyAnimationsController.DisableAllStates();
                enemyAbstract.EnemyAnimationsController.SetState(3, true);
                enemyAbstract.EnemyAnimationsController.SetSpeedAnimationByRunSpeed(0.5f);
                enemyAbstract.EnemyAnimationsController.PlayDeadNormalizeCoroutine();
            }
        }

        /// <summary>
        /// Запустить огненный эффект
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="weapon"></param>
        public void RunFireDamage(float damage, IWeapon weapon)
        {
            enemyAbstract.Physicffect.EventEffectWithoutDefenceBonus(weapon); // оттолкнуть врага

            enemyAbstract.FireEffect.EventEffect(damage, weapon);
        }
        #endregion
    }
}
