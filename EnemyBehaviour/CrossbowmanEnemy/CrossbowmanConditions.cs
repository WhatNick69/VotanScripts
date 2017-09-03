using System.Collections.Generic;
using PlayerBehaviour;
using MovementEffects;
using UnityEngine;
using VotanGameplay;

namespace EnemyBehaviour
{
    /// <summary>
    /// Компонент жизней для класса врагов "Арбалетчик"
    /// </summary>
    public class CrossbowmanConditions
        : EnemyConditions
    {
        /// <summary>
        /// Корутина на получение ледяного урона
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="weapon"></param>
        /// <returns></returns>
        public override IEnumerator<float> CoroutineForFrozenDamage(float damage, IWeapon weapon)
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
                enemyAbstract.EnemyAnimationsController.SetState(4, true);
                enemyAbstract.EnemyAnimationsController.SetSpeedAnimationByRunSpeed(0.5f);
                enemyAbstract.EnemyAnimationsController.PlayDeadNormalizeCoroutine();
            }
        }

        /// <summary>
        /// Смерть арбалетчика
        /// </summary>
        /// <returns></returns>
        public override IEnumerator<float> DieState()
        {
            IsAlive = false;
            enemyAbstract.AbstractObjectSounder.PlayDeadAudio();
            //enemyAbstract.EnemyAnimationsController.DisableAllStates();
            enemyAbstract.EnemyAnimationsController.SetSpeedAnimationByRunSpeed(1);
            enemyAbstract.EnemyAnimationsController.SetState(4, true);
            enemyAbstract.EnemyAnimationsController.PlayDeadNormalizeCoroutine();
            MainBarCanvas.gameObject.SetActive(false);
            enemyAbstract.EnemyMove.DisableAgent();
            GetComponent<BoxCollider>().enabled = false;

            while (!enemyAbstract.EnemyAnimationsController.IsDowner)
                yield return Timing.WaitForSeconds(0.5f);
            EnemyCreator.ReturnEnemyToStack(enemyAbstract.EnemyNumber);
        }
    }
}
