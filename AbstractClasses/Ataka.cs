using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerBehaviour;

public class Ataka 
    : MonoBehaviour {
	
    List<Enemy> ListEnemy;
	List<Enemy> AttackList;
    [SerializeField]
    private Transform startGunPoint,finishGunPoint;
    
    float zz = 8;
    float a;
    float b;
    float c;
    float ta;
    float tb;

    public void AddEnemyToList(Enemy x) 
    {
        ListEnemy.Add(x);
    }

	public List<Enemy> ReturnList()
	{
		return ListEnemy;
	}

	private float AttackRange(Vector3 Player, Vector3 Enemy)
	{
		return Vector3.Distance(Player, Enemy);
	}

	private bool Bush(Vector3 x, Vector3 y, Vector3 z, Vector3 w)
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

    private Vector3 AttackPoint(Transform X, Transform Y)
    {
        float A = X.position.x + ta * (Y.position.x - X.position.x);
        float B = X.position.z + ta * (Y.position.z - X.position.z);

        return new Vector3(A, 2, B);
    }

	private Vector3 WeaponDot(int index)
	{
		return Vector3.zero;
	}

    private void EnemyAttack(int Damage)
    {
        for (int i = 0; i < AttackList.Count; i++)
        {
            Debug.Log("Зашел1");
            if (AttackList[i])
            {
                Debug.Log(AttackList.Count);
                if (Bush(startGunPoint.position, 
                    finishGunPoint.position, AttackList[i].ReturnPosition(0), 
                    AttackList[i].ReturnPosition(1))|| 
					Bush(startGunPoint.position, 
                    finishGunPoint.position, AttackList[i].ReturnPosition(2)
                    , AttackList[i].ReturnPosition(3)))
				{
                    Debug.Log("Зашел3");
					AttackList[i].GetDamade(1);
					if(AttackList[i].ReturnHeealth()<=0 || !AttackList[i])
					{
						AttackList.RemoveAt(i);
					}				
				}
            }
		}
    }

    void Start()
    {
        ListEnemy = new List<Enemy>();
		AttackList = new List<Enemy>();
    }

	private void FixedUpdate()
	{
		for (int i = 0; i < ListEnemy.Count; i++)
			if (ListEnemy[i])
			{
				if (AttackRange(transform.position, ListEnemy[i].transform.position) < 3)
				{
					if (!AttackList.Contains(ListEnemy[i])) AttackList.Add(ListEnemy[i]);
				}
			}
		EnemyAttack(1);
	}
}

