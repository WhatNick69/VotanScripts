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
        [SerializeField, Tooltip("Враг является боссом?")]
        private bool isBoss;
        [SerializeField, Tooltip("Босья сила сокращения эффекта"),Range(0,1f)]
        private float bossEffectDecreaseMultiplier;
        [SerializeField, Tooltip("Трэил земляного эффекта")]
        private TrailRenderer trailRenderer;
        [SerializeField, Tooltip("Частота обновления")]
        private float updateFrequency;
        private IWeapon weapon;
        private Transform playerObjectTransform;
        private bool isStillTrailMoving;
        private bool isSuperAttack;
        private Vector3 nockBackPosition;
        private Vector3 vectorTemp;

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
        /// Оттолкнуть врага, если скорость вращения выше 0.95
        /// </summary>
        /// <param name="weapon"></param>
        public void EventEffectWithoutDefenceBonus(IWeapon weapon)
        {
            if (LibraryStaticFunctions.MayToNockbackEnemy(weapon))
            {
                isSuperAttack = false;

                weapon.GetPlayer.PlayerCameraSmooth.
                       DoNoize((weapon.SpinSpeed / weapon.OriginalSpinSpeed) + 0.5f);
                this.weapon = weapon;
                playerObjectTransform = weapon.GetPlayer.PlayerWeapon.transform;
                if (enemy.EnemyConditions.IsAlive)
                {
                    Timing.RunCoroutine(CoroutineForNockback());
                }
            }
        }

        /// <summary>
        /// Оттолкнуть врага атакующим рывком
        /// </summary>
        /// <param name="weapon"></param>
        public void EventEffectRageAttack(IWeapon weapon)
        {
            isSuperAttack = true;
            weapon.GetPlayer.PlayerCameraSmooth.
                      DoNoize(1);
            this.weapon = weapon;
            playerObjectTransform = weapon.GetPlayer.PlayerWeapon.transform;
            if (enemy.EnemyConditions.IsAlive)
            {
                Timing.RunCoroutine(CoroutineForNockback());
            }
        }

        /// <summary>
        /// Установить позицию для отталкивания
        /// 
        /// Если это босс - то в 4 раза меньше
        /// </summary>
        private void SetNockbackPosition()
        {
            vectorTemp = enemy.transform.position - enemy.EnemyMove.
                PlayerObjectTransformForFollow.transform.position;
            if (isBoss)
            {
                nockBackPosition = enemy.transform.position +
                    (vectorTemp * LibraryStaticFunctions.
                    StrenghtOfNockback(weapon, isSuperAttack)
                    *(1-bossEffectDecreaseMultiplier));
            }
            else
            {
                nockBackPosition = enemy.transform.position +
                    vectorTemp * LibraryStaticFunctions.
                    StrenghtOfNockback(weapon, isSuperAttack);
            }
        }

        /// <summary>
        /// Корутина для отталкивания
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForNockback()
        {
            SetNockbackPosition();

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
                new Color(Random.Range(0,1f),
                Random.Range(0, 1f),
                Random.Range(0, 1f), 1);

            while (Vector3.Distance(trailRenderer.transform.position, playerObjectTransform.position) >= 0.5f)
            {
                trailRenderer.transform.LookAt(playerObjectTransform);
                trailRenderer.transform.Translate(trailRenderer.transform.forward/2, Space.World);
                yield return Timing.WaitForSeconds(updateFrequency);
            }
            weapon.AddTempPhysicDefence(weapon.GemPower / 10);
            weapon.GetPlayer.PlayerSounder.PlayPhysicDefenceBonus();

            yield return Timing.WaitForSeconds(0.25f);
            trailRenderer.gameObject.SetActive(false);
            isStillTrailMoving = false;
        }
    }
}
