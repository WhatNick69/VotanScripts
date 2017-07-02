using AbstractBehaviour;
using MovementEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using VotanLibraries;

namespace EnemyBehaviour
{
    class EnemyConditions 
        : AbstractObjectConditions
    {
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

        private EnemyMove enemyMove;

        private void Start()
        {
            initialisatedHealthValue = healthValue;
            colorChannelRed = 0;
            colorChannelGreen = 1;
            enemyMove = GetComponent<EnemyMove>();
        }

        public override float GetDamageWithResistance(float dmg,DamageType typeOfDamage)
        {
            switch (typeOfDamage)
            {
                case DamageType.Electric:
                    return dmg * (1 - electricResistance);
                case DamageType.Fire:
                    float damage = dmg * (1 - fireResistance);
                    RunCoroutineForGetFireDamage(damage);
                    return damage;
                case DamageType.Frozen:
                    RunCoroutineForFrozenDamage();
                    return dmg * (1 - frostResistance);
                case DamageType.Powerful:
                    return dmg * (1 - physicResistance);
            }
            return dmg;
        }

        // 3 секунды +- 0.5 секунды - для заморозки
        // для огня 4 секунды +-1 секунда - для огня
        private void RunCoroutineForGetFireDamage(float damage)
        {
            Timing.RunCoroutine(CoroutineForFireDamage(damage));
        }

        private void RunCoroutineForFrozenDamage()
        {
            Timing.RunCoroutine(CoroutineForFrozenDamage());
        }

        /// <summary>
        /// Временный урон от огня.
        /// </summary>
        /// <param name="damage"></param>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForFireDamage(float damage)
        {
            float timeWhileDamage = LibraryStaticFunctions.GetPlusMinusVal(3,0.25f);
            float time = 0;
            while (time <= timeWhileDamage)
            {
                HealthValue -= LibraryStaticFunctions.GetPlusMinusVal(damage/10, 0.25f);
                time += 0.25f;
                yield return Timing.WaitForSeconds(0.25f);
            }
        }

        /// <summary>
        /// Замедление врага от холода
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForFrozenDamage()
        {
            enemyMove.SetNewSpeedOfNavMeshAgent(enemyMove.AgentSpeed/3,enemyMove.RotationSpeed/3);
            yield return Timing.WaitForSeconds(LibraryStaticFunctions.GetPlusMinusVal(4, 0.25f));
            enemyMove.SetNewSpeedOfNavMeshAgent(enemyMove.AgentSpeed,enemyMove.RotationSpeed);
        }

        /// <summary>
        /// Таймовые вычисления
        /// </summary>
        private void FixedUpdate()
        {
            BarBillboard();
        }

        /// <summary>
        /// Поворачивает биллборд вслед за камерой игрока
        /// </summary>
        public void BarBillboard()
        {
            healthBar.transform.LookAt(LibraryPlayerPosition.MainCameraPlayerTransform);
        }
    }
}
