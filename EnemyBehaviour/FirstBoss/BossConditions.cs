using PlayerBehaviour;
using VotanLibraries;
using MovementEffects;
using VotanInterfaces;
using System.Collections.Generic;
using VotanGameplay;
using UnityEngine;

namespace EnemyBehaviour
{
    /// <summary>
    /// Здоровье и броня босса
    /// </summary>
    public class BossConditions 
        : EnemyConditions, IBossConditions
    {
        #region Переменные
        private EnemyArmory enemyArmory;
        private bool isBossAlive = true;
        #endregion

        #region Свойства
        public bool IsBossAlive
        {
            get
            {
                return isBossAlive;
            }
        }
        #endregion

        /// <summary>
        /// Инициализация
        /// </summary>
        public override void Start()
        {
            base.Start();
            enemyArmory = GetComponent<EnemyArmory>();
        }

        /// <summary>
        /// Состояние смерти босса
        /// </summary>
        /// <returns></returns>
        public override IEnumerator<float> DieState()
        {
            IsAlive = false;
            isBossAlive = false;
            
            enemyAbstract.AbstractObjectSounder.PlayDeadAudio();
            //enemyAbstract.EnemyAnimationsController.DisableAllStates();
            enemyAbstract.EnemyAnimationsController.SetSpeedAnimationByRunSpeed(0.5f);
            enemyAbstract.EnemyAnimationsController.SetState(3, true);
            enemyAbstract.EnemyAnimationsController.PlayDeadNormalizeCoroutine();
            MainBarCanvas.gameObject.SetActive(false);
            enemyAbstract.EnemyMove.Agent.enabled = false;
            GetComponent<BoxCollider>().enabled = false;

            if (isFrozen)
                yield return Timing.WaitForSeconds(5 + enemyAbstract.IceEffect.TimeToDisable);
            else
                yield return Timing.WaitForSeconds(5);

            // Выключаем врага. Возвращаем в стек врагов. Почти, как если бы
            // мы его уничтожали.
            //enemyAbstract.EnemyAnimationsController.AnimatorOfObject.enabled = false;
            EnemyCreator.SendToPlayersCallOfWin();
            EnemyCreator.ReturnEnemyToStack(enemyAbstract.EnemyNumber);
        }

        /// <summary>
        /// Получить урон.
        /// По броне, либо по телу.
        /// </summary>
        /// <param name="dmg"></param>
        /// <param name="gemPower"></param>
        /// <param name="weapon"></param>
        /// <param name="isSuperAttack"></param>
        /// <returns></returns>
        public override bool GetDamage(float dmg, float gemPower, IWeapon weapon, bool isSuperAttack)
        {
            if (isMayGetDamage)
            {
                if (enemyArmory.IsAlive)
                {
                    if (isSuperAttack)
                    {
                        enemyAbstract.AbstractObjectSounder.PlayGetDamageAudio(true);

                        enemyAbstract.Physicffect.EventEffectRageAttack(weapon);

                        dmg = GetDamageWithResistanceWithoutEffect(dmg, weapon);
                        Timing.RunCoroutine(CoroutineForGetDamage(false, dmg));

                        enemyArmory.DecreaseArmoryLevel(-
                            LibraryStaticFunctions.GetRangeValue(dmg, 0.1f));

                        weapon.WhileTime();
                        return true;
                    }
                    else if (weapon.SpinSpeed / weapon.OriginalSpinSpeed >= 0.1f)
                    {
                        enemyAbstract.AbstractObjectSounder.PlayGetDamageAudio(true);

                        /* Если это электрический удар в рукопашную - отодвигаем противника.
                         Молния не должна иметь право отодвигать врага. */
                        if (weapon.AttackType == DamageType.Electric)
                            enemyAbstract.Physicffect.EventEffectWithoutDefenceBonus(weapon);

                        dmg = GetDamageWithResistance(dmg, gemPower, weapon);
                        Timing.RunCoroutine(CoroutineForGetDamage(false, dmg));

                        enemyArmory.DecreaseArmoryLevel(-
                            LibraryStaticFunctions.GetRangeValue(dmg, 0.1f));

                        weapon.WhileTime();
                        return true;
                    }
                }
                else
                {
                    if (isSuperAttack)
                    {
                        enemyAbstract.AbstractObjectSounder.PlayGetDamageAudio();

                        enemyAbstract.Physicffect.EventEffectRageAttack(weapon);

                        dmg = GetDamageWithResistanceWithoutEffect(dmg, weapon);
                        Timing.RunCoroutine(CoroutineForGetDamage(false, dmg));

                        if (HealthValue <= 0) return false;
                        HealthValue -=
                            LibraryStaticFunctions.GetRangeValue(dmg, 0.1f);
                        if (HealthValue <= 0)
                            enemyAbstract.ScoreAddingEffect.EventEffect(weapon);

                        weapon.WhileTime();
                        return true;
                    }
                    else if (weapon.SpinSpeed / weapon.OriginalSpinSpeed >= 0.1f)
                    {
                        enemyAbstract.AbstractObjectSounder.PlayGetDamageAudio();

                        /* Если это электрический удар в рукопашную - отодвигаем противника.
                         Молния не должна иметь право отодвигать врага. */
                        if (weapon.AttackType == DamageType.Electric)
                            enemyAbstract.Physicffect.EventEffectWithoutDefenceBonus(weapon);

                        dmg = GetDamageWithResistance(dmg, gemPower, weapon);
                        Timing.RunCoroutine(CoroutineForGetDamage(false, dmg));

                        if (HealthValue <= 0) return false;
                        HealthValue -=
                            LibraryStaticFunctions.GetRangeValue(dmg, 0.1f);
                        if (HealthValue <= 0)
                            enemyAbstract.ScoreAddingEffect.EventEffect(weapon);

                        weapon.WhileTime();
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Корутина на получения урона и воспроизведения анимации.
        /// Если получаемый урон больше, либо равен четверти здоровья врага
        /// то мы воспроизводим анимацию
        /// </summary>
        /// <param name="isLongAttack"></param>
        /// <param name="dmg"></param>
        /// <returns></returns>
        public override IEnumerator<float> CoroutineForGetDamage(bool isLongAttack = false,float dmg=0)
        {
            if (!isLongAttack)
                isMayGetDamage = false;

            bool flag;
            if (enemyArmory.IsAlive)
            {
                flag = LibraryStaticFunctions.BossMayPlayGetDamageAnimation
                    (enemyArmory.InitialisatedHealthValue, dmg);
            }
            else
            {
                flag = LibraryStaticFunctions.BossMayPlayGetDamageAnimation
                    (initialisatedHealthValue, dmg);
            }

            if (flag)
                enemyAbstract.EnemyAnimationsController.SetState(2, true);
            yield return Timing.WaitForSeconds(frequencyOfGetDamage);
            if (flag)
                enemyAbstract.EnemyAnimationsController.SetState(2, false);

            if (!isLongAttack)
                isMayGetDamage = true;
        }
    }
}
