﻿using System;
using PlayerBehaviour;
using UnityEngine;
using VotanInterfaces;
using AbstractBehaviour;
using System.Collections.Generic;
using MovementEffects;
using VotanLibraries;

namespace GameBehaviour
{
    /// <summary>
    /// Реализует эффект земляной атаки.
    /// Земляной эффект отталкивает врага, а также повышает 
    /// защиту персонажа на короткий промежуток времени.
    /// </summary>
    public class PhysicEffectManager
        : MonoBehaviour, IPhysicEffect
    {
        [SerializeField, Tooltip("Враг")]
        private AbstractEnemy enemy;
        [SerializeField, Tooltip("Трэил земляного эффекта")]
        private TrailRenderer trailRenderer;
        [SerializeField, Tooltip("Частота обновления")]
        private float updateFrequency;
        private IWeapon weapon;
        private Transform playerObjectTransform;
        private bool isStillTrailMoving;

        /// <summary>
        /// Зажечь земляной эффект
        /// </summary>
        /// <param name="weapon"></param>
        public void EventEffect(IWeapon weapon)
        {
            if (weapon.IsMayToGetPhysicDefence &&
                !isStillTrailMoving)
            {
                weapon.GetPlayer.PlayerCameraSmooth.
                    DoNoize((weapon.SpinSpeed / weapon.OriginalSpinSpeed)+0.5f);
                isStillTrailMoving = true;
                weapon.IsMayToGetPhysicDefence = true;
                this.weapon = weapon;
                playerObjectTransform = weapon.GetPlayer.PlayerWeapon.transform;
                if (enemy.EnemyConditions.IsAlive)
                {
                    Timing.RunCoroutine(CoroutineForNockback());
                }
                Timing.RunCoroutine(CoroutineForTrailMoving());
            }
        }

        /// <summary>
        /// Корутина для отталкивания
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForNockback()
        {
            Vector3 vec = enemy.transform.position - enemy.EnemyMove.
                PlayerObjectTransformForFollow.transform.position;
            Vector3 nockBackPosition = enemy.transform.position + 
                vec*LibraryStaticFunctions.StrenghtOfNockback(weapon);

            int i = 0;
            while (Vector3.Distance(enemy.transform.position,nockBackPosition) > 0.2f)
            {
                nockBackPosition.y = enemy.transform.position.y;
                enemy.transform.position = Vector3.Lerp(enemy.transform.position, 
                    nockBackPosition, Time.deltaTime);
                i++;
                if (i > 30 || !enemy.EnemyConditions.IsAlive) yield break;
                yield return Timing.WaitForOneFrame;
            }
        }

        /// <summary>
        /// Корутина для движения частицы
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForTrailMoving()
        {
            trailRenderer.transform.localPosition = Vector3.zero;
            trailRenderer.gameObject.SetActive(true);
            trailRenderer.startColor =
                new Color((float)LibraryStaticFunctions.rnd.NextDouble(),
                (float)LibraryStaticFunctions.rnd.NextDouble(),
                (float)LibraryStaticFunctions.rnd.NextDouble(), 1);

            while (Vector3.Distance(trailRenderer.transform.position, playerObjectTransform.position) >= 0.5f)
            {
                trailRenderer.transform.LookAt(playerObjectTransform);
                trailRenderer.transform.Translate(trailRenderer.transform.forward/2, Space.World);
                yield return Timing.WaitForSeconds(updateFrequency);
            }
            weapon.AddTempPhysicDefence(weapon.GemPower / 10);
            yield return Timing.WaitForSeconds(0.25f);
            trailRenderer.gameObject.SetActive(false);
            isStillTrailMoving = false;
        }
    }
}
