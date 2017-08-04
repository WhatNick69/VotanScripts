using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour {

	
	void Awake ()
	{
		if (PlayerPrefs.GetInt("damage") >= 0)
		{ }
		else
		{
			PlayerPrefs.SetInt("damage", 0);
		}

		if (PlayerPrefs.GetInt("lvl") >= 1)
		{ }
		else
		{
			PlayerPrefs.SetInt("lvl", 1);
		}

		if (PlayerPrefs.GetInt("hp") >= 80)
		{ }
		else
		{
			PlayerPrefs.SetInt("hp", 80);
		}

		if (PlayerPrefs.GetInt("armor") >= 0)
		{ }
		else
		{
			PlayerPrefs.SetInt("armor", 0);
		}

		if (PlayerPrefs.GetInt("experience") >= 0)
		{ }
		else
		{
			PlayerPrefs.SetInt("experience", 0);
		}

		PlayerPrefs.Save();
	}
	
}
