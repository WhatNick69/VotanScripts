using AbstractBehaviour;
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

        [SerializeField,Tooltip("Защита от холода"),Range(0,1f)]
        private float frostResistance;
        [SerializeField, Tooltip("Защита от огня"), Range(0, 1f)]
        private float fireResistance;
        [SerializeField, Tooltip("Защита от электричества"), Range(0, 1f)]
        private float electricResistance;
        [SerializeField, Tooltip("Защита от ударов"), Range(0, 1f)]
        private float physicResistance;

        private void Start()
        {
            initialisatedHealthValue = healthValue;
            colorChannelRed = 0;
            colorChannelGreen = 1;
        }

        public float GetDamageWithResistance(float dmg,DamageType typeOfDamage)
        {
            switch (typeOfDamage)
            {
                case DamageType.Electric:
                    return dmg * (1 - electricResistance);
                case DamageType.Fire:
                    return dmg * (1 - fireResistance);
                case DamageType.Frozen:
                    return dmg * (1 - frostResistance);
                case DamageType.Powerful:
                    return dmg * (1 - physicResistance);
            }
            return dmg;
        }

        // 3 секунды +- 0.5 секунды - для заморозки
        // для огня 4 секунды +-1 секунда - для огня
        private void RunCoroutineForGetFireDamage()
        {

        }

        public override void DieState()
        {
            base.DieState();
            DestroyObject();
        }

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
