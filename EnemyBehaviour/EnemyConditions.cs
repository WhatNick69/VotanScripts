using AbstractBehaviour;
using GameBehaviour;
using MovementEffects;
using PlayerBehaviour;
using System.Collections.Generic;
using UnityEngine;
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
        private Transform cameraTransform;

        private IAIMoving enemyMove;
        private IEnemyBehaviour enemyAbstract;

        private bool isMayGetDamage = true;
        private bool isFrozen; // заморожен ли
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
        #endregion

        /// <summary>
        /// Инициализация
        /// </summary>
        public void Start()
        {
			IsAlive = true;
            initialisatedHealthValue = healthValue;
            colorChannelRed = 0;
            colorChannelGreen = 1;

            enemyAbstract = GetComponent<IEnemyBehaviour>();
            enemyMove = enemyAbstract.EnemyMove;
            normalShadowSize = new Vector3(1.25f, 1.25f, 1.25f);
            littleShadowSize = new Vector3(0.6f, 0.6f, 0.6f);

            FindCameraInScene();
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
        /// Таймовые вычисления
        /// </summary>
        public void FixedUpdate()
        {
            BarBillboard();
        }

        /// <summary>
        /// Поворачивает биллборд вслед за камерой игрока
        /// </summary>
        public void BarBillboard()
        {
            if (cameraTransform != null)
                healthBar.transform.LookAt(cameraTransform);
            else
                FindCameraInScene();
        }

        /// <summary>
        /// Вернуть здоровье
        /// </summary>
        /// <returns></returns>
        public virtual float ReturnHealth()
        {
            return HealthValue;
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
        public void DisableDownInterface()
        {
            blobShadow.gameObject.SetActive(false);
        }

        /// <summary>
        /// Состояние  смерти
        /// </summary>
        /// <returns></returns>
        public override IEnumerator<float> DieState()
        {
            IsAlive = false;
            //enemyAbstract.EnemyAnimationsController.DisableAllStates();
            enemyAbstract.EnemyAnimationsController.SetSpeedAnimationByRunSpeed(0.5f);
            enemyAbstract.EnemyAnimationsController.SetState(3, true);
            enemyAbstract.EnemyAnimationsController.PlayDeadNormalizeCoroutine();
            MainBarCanvas.gameObject.SetActive(false);
            enemyAbstract.EnemyMove.Agent.enabled = false;
            GetComponent<BoxCollider>().enabled = false;

            if (isFrozen)
                yield return Timing.WaitForSeconds(6.5f);
            else
                yield return Timing.WaitForSeconds(5);
            DestroyObject();
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
            switch (weapon.AttackType)
            {
                case DamageType.Electric:
                    RunCoroutineForGetElectricDamage(dmg, gemPower, weapon);
                    damage = dmg * (1 - electricResistance);
                    return damage;
                case DamageType.Fire:
                    damage = dmg * (1 - fireResistance);
                    RunFireDamage(damage, weapon);
                    return damage;
                case DamageType.Frozen:
                    if (!isFrozen)
                    {
                        if (LibraryStaticFunctions.MayableToBeFreezy(gemPower))
                            RunCoroutineForFrozenDamage(dmg,weapon);
                    }
                    return dmg * (1 - frostResistance);
                case DamageType.Powerful:
                    RunCoroutineForPhysicDamage(weapon);
                    return dmg * (1 - physicResistance);
            }

            return dmg;
        }

        #region Методы для получения разного вида урона
        /// <summary>
        /// Получить урон в рукопашном бою
        /// </summary>
        /// <param name="dmg"></param>
        public virtual void GetDamage(float dmg, float gemPower
            , IWeapon weapon)
        {
            if (isMayGetDamage)
            {
                weapon.WhileTime();
                Timing.RunCoroutine(CoroutineForGetDamage());
                dmg = GetDamageWithResistance(dmg, gemPower, weapon);
                //Debug.Log("Ближняя атака");
                if (HealthValue <= 0) return;
                HealthValue -=
                    LibraryStaticFunctions.GetRangeValue(dmg, 0.1f);
                if (HealthValue <= 0)
                {
                    enemyAbstract.ScoreAddingEffect.EventEffect(weapon);
                }
            }
        }

        /// <summary>
        /// Получить урон от молнии, либо от огня
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
            //Debug.Log("Дальняя атака");
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
        public IEnumerator<float> CoroutineForGetDamage(bool isLongAttack = false)
        {
            if (!isLongAttack)
                isMayGetDamage = false;

            enemyAbstract.EnemyAnimationsController.SetState(2, true);
            yield return Timing.WaitForSeconds(0.5f);
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
            IsShocked = true;
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
            float time = LibraryStaticFunctions.TimeToFreezy(weapon.GemPower);
            enemyAbstract.IceEffect.EventEffect(damage,time, weapon);
            enemyAbstract.EnemyMove.Agent.enabled = false;

            IsFrozen = true;
            yield return Timing.WaitForSeconds(LibraryStaticFunctions.GetRangeValue(time));
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
            enemyAbstract.FireEffect.EventEffect(damage, weapon);
        }
        #endregion
    }
}
