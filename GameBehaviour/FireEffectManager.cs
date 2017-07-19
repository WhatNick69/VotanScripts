﻿using System.Collections.Generic;
using UnityEngine;
using VotanInterfaces;
using MovementEffects;
using AbstractBehaviour;
using VotanLibraries;
using PlayerBehaviour;
using System;

namespace GameBehaviour
{
    /// <summary>
    /// Менеджер огненного эффекта
    /// </summary>
    public class FireEffectManager 
        : MonoBehaviour, IFireEffect
    {
        #region Переменные
        [SerializeField]
        private AbstractEnemy abstractEnemy;
        private ParticleSystem particleSystem;
        private float time;
        private float damagePerTime;

        private bool isBurning;
        private IWeapon weapon;
        #endregion

        /// <summary>
        /// Инициализация
        /// </summary>
        void Start()
        {
            particleSystem = transform.GetChild(0).GetComponent<ParticleSystem>();
            damagePerTime = 0;   
        }

        /// <summary>
        /// Зажечь огненный эффект
        /// </summary>
        /// <param name="time"></param>
        /// <param name="position"></param>
        public void EventEffect(float damage,IWeapon weapon)
        {
            damagePerTime += LibraryStaticFunctions.FireDamagePerPeriod(damage,weapon);
            InitialisationParticleSystem();

            if (!isBurning)
            {
                this.weapon = weapon;
                time = LibraryStaticFunctions.TimeToBurning(weapon);
                Timing.RunCoroutine(CoroutineForFireActivity());
            }
        }

        /// <summary>
        /// Инициализация партикл-системы на основе силы горения
        /// </summary>
        private void InitialisationParticleSystem()
        {
            if (damagePerTime > 150)
            {
                damagePerTime = 150;
            }
            Debug.Log(damagePerTime);
            particleSystem.emissionRate = 
                LibraryStaticFunctions.GetCountOfParticleSystemElements(damagePerTime);
        }

        /// <summary>
        /// Корутина для случайного движения к точке назначения 
        /// и повороту огненного объекта
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForFireActivity()
        {
            int i = 0;
            int maxI = Convert.ToInt32(time / 0.25f);
            isBurning = true;

            particleSystem.Play();

            while (i < maxI)
            {
                abstractEnemy.EnemyConditions.HealthValue -= 
                    LibraryStaticFunctions.GetRangeValue(damagePerTime, 0.05f);
                yield return Timing.WaitForSeconds(0.25f);
                i++;
            }
            particleSystem.Stop();

            damagePerTime = 0;
            isBurning = false;
        }
    }
}
