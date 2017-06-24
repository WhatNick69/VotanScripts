using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEnterTest : MonoBehaviour {

    public GameObject obj;
	// Use this for initialization
	void Start () {
        int i = 0;
        System.Random rnd = new System.Random();
		while (i<300)
        {
            Instantiate(obj).transform.position =
                new Vector3((float)(rnd.Next(-100, 100) * rnd.NextDouble())
                , 0, (float)(rnd.Next(-100, 100) * rnd.NextDouble()));
            i++;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnCollisionEnter(Collision col)
    {
        GameObject obj = col.contacts[0].otherCollider.gameObject;
        if (obj.tag == "Enemy")
            Destroy(obj);
    }
}
