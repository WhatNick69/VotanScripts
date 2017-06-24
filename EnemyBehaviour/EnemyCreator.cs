using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCreator : MonoBehaviour {

    public GameObject Enemy;
    private Ataka Player;
    List<Enemy> ListEn;
    int i = 0;
    int k = 0;
    int j = 0;

    IEnumerator IY()
    {
		while (k < 500)
		{
			yield return new WaitForSeconds(3);

			Player.AddEnemyToList(Instantiate(Enemy).GetComponent<Enemy>());
				
			//Tex.text = k.ToString();
			ListEn = Player.ReturnList();
			ListEn[k].transform.position = new Vector3(Random.Range(-7, 7), 1.5f, Random.Range(-7, 7));
			k++;
		}	
	}
	void Start ()
    {
        ListEn = new List<Enemy>();
        Player = GameObject.FindWithTag("Player").GetComponent<Ataka>();
        StartCoroutine(IY());
	}
}
