using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlayerBehaviour;
using VotanLibraries;
using MovementEffects;

namespace EnemyBehaviour
{
    class BossConditions 
        : EnemyConditions
    {
        public override bool GetDamage(float dmg, float gemPower, IWeapon weapon, bool isSuperAttack)
        {
            if (isMayGetDamage)
            {
                if (isSuperAttack)
                {
                    enemyAbstract.AbstractObjectSounder.PlayGetDamageAudio();
                    Timing.RunCoroutine(CoroutineForGetDamage());

                    enemyAbstract.Physicffect.EventEffectRageAttack(weapon);

                    dmg = GetDamageWithResistanceWithoutEffect(dmg, weapon);

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
                    Timing.RunCoroutine(CoroutineForGetDamage());

                    /* Если это электрический удар в рукопашную - отодвигаем противника.
                     Молния не должна иметь право отодвигать врага. */
                    if (weapon.AttackType == DamageType.Electric)
                        enemyAbstract.Physicffect.EventEffectWithoutDefenceBonus(weapon);

                    dmg = GetDamageWithResistance(dmg, gemPower, weapon);

                    if (HealthValue <= 0) return false;
                    HealthValue -=
                        LibraryStaticFunctions.GetRangeValue(dmg, 0.1f);
                    if (HealthValue <= 0)
                        enemyAbstract.ScoreAddingEffect.EventEffect(weapon);

                    weapon.WhileTime();
                    return true;
                }
            }
            return false;
        }
    }
}
