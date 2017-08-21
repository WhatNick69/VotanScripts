using System.Collections.Generic;
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
        [SerializeField,Tooltip("Враг")]
        private AbstractEnemy abstractEnemy;
        [SerializeField, Tooltip("Система частиц")]
        private ParticleSystem particleSystem;
        [SerializeField, Tooltip("Звук воспламенения")]
        private AudioSource audioSourceOfFire;
        [SerializeField, Tooltip("Звук горения")]
        private AudioSource audioSourceOfBurning;
        private float time;
        private float damagePerTime;

        private bool isBurning;
        private IWeapon weapon;
        #endregion

        private void FireAudio()
        {
            AbstractSoundStorage.PlayBurnAudio(audioSourceOfFire);
        }

        public void RestartFire()
        {
            damagePerTime = 0;
            audioSourceOfBurning.Stop();
            isBurning = false;
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
            FireAudio();

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

            particleSystem.gameObject.SetActive(true);
            particleSystem.Play();
            AbstractSoundStorage.PlayBurningAudio(audioSourceOfBurning);
            audioSourceOfBurning.Play();

            while (i < maxI)
            {
                if (abstractEnemy.EnemyConditions.HealthValue <= 0) yield break;

                abstractEnemy.EnemyConditions.HealthValue -= 
                    LibraryStaticFunctions.GetRangeValue(damagePerTime, 0.05f);
                if (abstractEnemy.EnemyConditions.HealthValue <= 0)
                {
                    abstractEnemy.ScoreAddingEffect.EventEffect(weapon);
                    break;
                }
                yield return Timing.WaitForSeconds(0.25f);
                i++;
            }
            particleSystem.Stop();
            damagePerTime = 0;

            yield return Timing.WaitForSeconds(1);
            particleSystem.gameObject.SetActive(false);

            audioSourceOfBurning.Stop();
            isBurning = false;
        }
    }
}
