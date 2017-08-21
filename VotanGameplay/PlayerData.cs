using UnityEngine;

public class PlayerData : MonoBehaviour {

	
	void Awake ()
	{
		for (int i = 0; i < 4; i++)
		{
			if (PlayerPrefs.GetInt("shieldLevel_" + i) >= 1)
			{ }
			else
			{
				PlayerPrefs.SetInt("shieldLevel_" + i, 0);
			}
		}

		if (PlayerPrefs.GetInt("metal") >= 0)
		{ }
		else
		{
			PlayerPrefs.SetInt("metal", 0);
		}

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
