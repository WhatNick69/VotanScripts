﻿using AbstractBehaviour;
using MovementEffects;
using System.Collections.Generic;
using UnityEngine;

namespace GameBehaviour
{
    /// <summary>
    /// Класс, который вешается на элементы брони
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class PartArmorySoundController
        : MonoBehaviour
    {
        private AudioSource auSo;
        private bool isMayToCollision;

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Start()
        {
            isMayToCollision = true;
            auSo = GetComponent<AudioSource>();
        }

        /// <summary>
        /// Просчет коллизий
        /// </summary>
        private void OnCollisionEnter()
        {
            if (isMayToCollision)
            {
                AbstractSoundStorage.WorkWithMetalThing(auSo);
                Timing.RunCoroutine(CoroutineForMayToCollision());
            }
        }

        /// <summary>
        /// Корутина на возможность просчета коллизий
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForMayToCollision()
        {
            isMayToCollision = false;
            yield return Timing.WaitForSeconds(0.25f);
            isMayToCollision = true;
        }
    }
}
