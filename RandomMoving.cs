using MovementEffects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMoving 
    : MonoBehaviour {

    private Vector3 newVec;
    private System.Random rnd = new System.Random();

	// Update is called once per frame
	void Update () {
        transform.position = Vector3.Lerp(transform.position, newVec, Time.deltaTime);
	}

    void Start()
    {
        newVec = transform.position;
        Timing.RunCoroutine(RndMove());
    }

    IEnumerator<float> RndMove()
    {
        while (true)
        {
            newVec = new Vector3((float)(rnd.Next(-10, 10) * rnd.NextDouble()) 
                + transform.position.x, 1, (float)(rnd.Next(-10, 10) * rnd.NextDouble()) 
                + transform.position.z);
            yield return Timing.WaitForSeconds(1);
        }
    }
}
