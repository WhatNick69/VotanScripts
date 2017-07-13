using AbstractBehaviour;
using MovementEffects;
using PlayerBehaviour;
using System.Collections.Generic;
using UnityEngine;
using VotanInterfaces;
using VotanLibraries;

namespace EnemyBehaviour
{
    public class EnemyConditions 
        : AbstractObjectConditions, IEnemyConditions
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
        private Transform cameraTransform;

        private EnemyMove enemyMove;
        private IEnemyBehaviour enemyAbstract;

        private bool isMayGetDamage = true;
        private bool isFrozen; // заморожен ли
        private bool isShocked; // шокирован электричеством ли
        private bool isBurned; // жарится ли
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
            DamageType dmgType, IWeapon weapon)
        {
            float damage = 0;
            switch (dmgType)
            {
                case DamageType.Electric:
                    RunCoroutineForGetElectricDamage(dmg, gemPower, weapon);
                    damage = dmg * (1 - electricResistance);
                    return damage;
                case DamageType.Fire:
                    damage = dmg * (1 - fireResistance);
                    RunCoroutineForGetFireDamage(damage);
                    return damage;
                case DamageType.Frozen:
                    if (!isFrozen)
                    {
                        if (LibraryStaticFunctions.MayableToBeFreezy(gemPower))
                            RunCoroutineForFrozenDamage();
                    }
                    return dmg * (1 - frostResistance);
                case DamageType.Powerful:
                    return dmg * (1 - physicResistance);
            }
            return dmg;
        }

        // 3 секунды +- 0.5 секунды - для заморозки
        // для огня 4 секунды +-1 секунда - для огня
        public void RunCoroutineForGetFireDamage(float damage)
        {
            Timing.RunCoroutine(CoroutineForFireDamage(damage));
        }

        /// <summary>
        /// Запустить корутины для замораживающего эффекта
        /// </summary>
        public void RunCoroutineForFrozenDamage()
        {
            Timing.RunCoroutine(CoroutineForFrozenDamage());
        }

        /// <summary>
        /// Временный урон от огня.
        /// </summary>
        /// <param name="damage"></param>
        /// <returns></returns>
        public IEnumerator<float> CoroutineForFireDamage(float damage)
        {
            float timeWhileDamage = LibraryStaticFunctions.GetRangeValue(3,0.25f);
            float time = 0;
            enemyAbstract.EnemyAnimationsController.SetState(2, true);
            while (time <= timeWhileDamage)
            {
                HealthValue -= LibraryStaticFunctions.GetRangeValue(damage/10, 0.25f);
                time += 0.25f;
                yield return Timing.WaitForSeconds(0.25f);
            }
            enemyAbstract.EnemyAnimationsController.SetState(2, false);
        }

        /// <summary>
        /// Замедление врага от холода
        /// </summary>
        /// <returns></returns>
        public IEnumerator<float> CoroutineForFrozenDamage()
        {
            Debug.Log("ICE");
            enemyAbstract.EnemyAnimationsController.SetState(2, true);
            enemyMove.SetNewSpeedOfNavMeshAgent(0,0);
            enemyAbstract.EnemyAnimationsController.SetSpeedAnimationByRunSpeed(0);
            float time = LibraryStaticFunctions.GetRangeValue(4, 0.25f);
            enemyAbstract.IceEffect.EventEffect(time);
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
        /// Получить урон в рукопашном бою
        /// </summary>
        /// <param name="dmg"></param>
        public virtual void GetDamage(float dmg, float gemPower
            ,DamageType dmgType, IWeapon weapon)
        {
			if (isMayGetDamage)
            {
				weapon.WhileTime();
                Timing.RunCoroutine(CoroutineForGetDamage());
                dmg = GetDamageWithResistance(dmg, gemPower,dmgType,weapon);
                Debug.Log("Ближняя атака");
                HealthValue -= 
                    LibraryStaticFunctions.GetRangeValue(dmg, 0.1f);
			}
        }

        /// <summary>
        /// Получить урон от молнии, либо от огня
        /// </summary>
        /// <param name="dmg"></param>
        /// <param name="gemPower"></param>
        /// <param name="dmgType"></param>
        /// <param name="weapon"></param>
        public void GetDamageLongDistance(float dmg, float gemPower
            , DamageType dmgType, IWeapon weapon)
        {
            Timing.RunCoroutine(CoroutineForGetDamage(true));
            dmg = GetDamageWithResistance(dmg, gemPower, dmgType, weapon);
            Debug.Log("Дальняя атака");
            HealthValue -=
                LibraryStaticFunctions.GetRangeValue(dmg, 0.1f);
        }

        /// <summary>
        /// Может ли враг получать урон?
        /// </summary>
        /// <returns></returns>
        public IEnumerator<float> CoroutineForGetDamage(bool isLongAttack=false)
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
    }
}
