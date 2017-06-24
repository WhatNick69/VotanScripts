using System.Collections.Generic;
using UnityEngine;

namespace AbstractBehaviour
{
    /// <summary>
    /// Абстрактный компонент-атака.
    /// Является родителем для компонентов-атаки 
    /// персонажей и противников
    /// </summary>
    public abstract class AbstractAttack
        : MonoBehaviour
    {
        protected List<AbstractEnemy> listEnemy;
        protected List<AbstractEnemy> attackList;
        [SerializeField]
            protected Transform startGunPoint, finishGunPoint;

        private float a;
        private float b;
        private float c;
        private float ta;
        private float tb;

        public void AddEnemyToList(AbstractEnemy x)
        {
            listEnemy.Add(x);
        }

        public List<AbstractEnemy> ReturnList()
        {
            return listEnemy;
        }

        public float AttackRange(Vector3 Player, Vector3 Enemy)
        {
            return Vector3.Distance(Player, Enemy);
        }

        public bool Bush(Vector3 x, Vector3 y, Vector3 z, Vector3 w)
        {
            Vector3 PV1 = x;
            Vector3 PV2 = y;
            Vector3 EV3 = z;
            Vector3 EV4 = w;

            a = (PV1.x - PV2.x) * (EV4.z - EV3.z) - (PV1.z - PV2.z) * (EV4.x - EV3.x);
            b = (PV1.x - EV3.x) * (EV4.z - EV3.z) - (PV1.z - EV3.z) * (EV4.x - EV3.x);
            c = (PV1.x - PV2.x) * (PV1.z - EV3.z) - (PV1.z - PV2.z) * (PV1.x - EV3.x);

            ta = b / a;
            tb = c / a;
            //Debug.Log("a: " + a + ", b: " + b + ", c: " + c + ", ta: " + ta + ", tb: " + tb);
            return (ta >= 0 && ta <= 1 && tb >= 0 && tb <= 1);
        }

        public Vector3 AttackPoint(Transform X, Transform Y)
        {
            float A = X.position.x + ta * (Y.position.x - X.position.x);
            float B = X.position.z + ta * (Y.position.z - X.position.z);

            return new Vector3(A, 2, B);
        }

        public Vector3 WeaponDot(int index)
        {
            return Vector3.zero;
        }

        public void EnemyAttack(int Damage)
        {
            for (int i = 0; i < attackList.Count; i++)
            {
                if (attackList[i])
                {
                    if (Bush(startGunPoint.position,
                        finishGunPoint.position, attackList[i].ReturnPosition(0),
                        attackList[i].ReturnPosition(1)) ||
                        Bush(startGunPoint.position,
                        finishGunPoint.position, attackList[i].ReturnPosition(2)
                        , attackList[i].ReturnPosition(3)))
                    {
                        attackList[i].GetDamage(1);
                        if (attackList[i].ReturnHealth() <= 0 || !attackList[i])
                        {
                            attackList.RemoveAt(i);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Типы атаки
    /// </summary>
    public enum DamageType
    {
        Frozen, Fire, Powerful, Electric
    }
}
