using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneScript : MonoBehaviour {

	[SerializeField]
	int arenaNumber;
	[SerializeField]
	string arenaName;

	public string ArenaName
	{
		get
		{
			return arenaName;
		}
	}

	public int Arena
	{
		get
		{
			return arenaNumber;
		}
	}
}
