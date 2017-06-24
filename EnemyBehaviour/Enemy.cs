using System.Collections;
using UnityEngine;

public class Enemy 
    : MonoBehaviour {

    [SerializeField]
    Transform startGunPoint, finishGunPoint, 
        rightShoulderPoint, leftShoulderPoint, 
        facePoint, backPoint;

	private float Health = 1;

    public float Health1
    {
        get
        {
            return Health;
        }

        set
        {
            Health = value;
        }
    }

    // Возвращает положение врага или его точек в сцене
    public Vector3 ReturnPosition(int Child) 
	{
		if (Child == 0) return rightShoulderPoint.position; //Right
		if (Child == 1) return leftShoulderPoint.position; //Left
		if (Child == 2) return facePoint.position; //Face
		if (Child == 3) return backPoint.position; //Back
		if (Child == 4) return transform.position; // Позиция врага
		else return Vector3.zero;
	}

	// Вызывает корутин смерти юнита
	public void Die()
    {
		StartCoroutine(DeiK());
    }

	IEnumerator DeiK()
	{
		yield return new WaitForSeconds(0.05f);
		Destroy(gameObject);

	}
	public float ReturnHeealth()
	{
		return Health;
	}

	public void GetDamade(float dmg)
	{
        Debug.Log("Отняли здоровье");
		Health -= dmg;
		if (Health <= 0) Die();
	}
}
