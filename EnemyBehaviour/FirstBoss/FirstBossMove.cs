﻿using MovementEffects;
using System.Collections.Generic;
using UnityEngine;
using VotanGameplay;
using VotanInterfaces;
using System;

namespace EnemyBehaviour
{
    /// <summary>
    /// Класс, описывающий движение для босса
    /// </summary>
    public class FirstBossMove 
        : EnemyMove, IFirstBossMove
    {
        #region Переменные
        private bool isDowner;
        #endregion

        #region Свойства
        public bool IsDowner
        {
            get
            {
                return isDowner;
            }

            set
            {
                isDowner = value;
            }
        }
        #endregion

        /// <summary>
        /// Корутина, которая выкидывает босса за пределы башни.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<float> CoroutineForFlyBoss()
        {
            yield return Timing.WaitForSeconds(3.5f);
            for (int i = 0;i<100;i++)
            {
                transform.Translate((playerObjectTransformForFollow.forward-
                    transform.forward)*0.1f);
                yield return Timing.WaitForSeconds(Time.deltaTime);
            }
        }

        /// <summary>
        /// Выкинуть босса за пределы башни.
        /// </summary>
        public void GoOutEnemy()
        {
            DisableAgent();
            Timing.RunCoroutine(CoroutineForFlyBoss());
        }

        /// <summary>
        /// Установить позицию смерти
        /// </summary>
        public void SetDeadPosition()
        {

            EnableAgent();
            abstractEnemy.EnemyAnimationsController.DisableAllStates();
            abstractEnemy.EnemyAnimationsController.SetState(6, true);
            abstractEnemy.EnemyAnimationsController.SetSpeedAnimationByRunSpeed(0.5f);
            playerObjectTransformForFollow =
                GameManager.GetClosestDeadPositionForEnemy(transform.position);

            agent.stoppingDistance = 0;
            agent.speed /= 2;
            Timing.RunCoroutine(CoroutineForMoveToDeadPosition());
        }

        /// <summary>
        /// Корутина на движение к точке смерти
        /// </summary>
        /// <returns></returns>
        public IEnumerator<float> CoroutineForMoveToDeadPosition()
        {
            while (Vector3.Distance(transform.position,
                playerObjectTransformForFollow.position) > 1)
            {
                abstractEnemy.EnemyAnimationsController.SetState(6, true);
                agent.SetDestination
                    (playerObjectTransformForFollow.position);
                yield return Timing.WaitForSeconds(frequencySearching);
            }
            DeadPositionDestination();
        }

        /// <summary>
        /// Срабатывает при достижении боссом пункта смертного назначения.
        /// </summary>
        public void DeadPositionDestination()
        {
            abstractEnemy.EnemyConditions.IsAlive = true;
            abstractEnemy.EnemyAnimationsController.DisableAllStates();
            abstractEnemy.EnemyAnimationsController.SetState(7, true);
        }
    }
}
