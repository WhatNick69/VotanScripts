using UnityEngine;

public class PlayerData : MonoBehaviour {

	
	
	void Awake ()
	{
		for (int i = 0; i < 4; i++) // сколько щитов, до стольки и считаем
		{
			if (PlayerPrefs.GetInt("shield_" + i) >= 0)
			{ }
			else
			{
				PlayerPrefs.SetInt("shield_" + i, 0);
			}
		}

		 
		for (int i = 0; i < 3; i++)
		{
			if (PlayerPrefs.GetString("weapon_" + i) == null
                || PlayerPrefs.GetString("weapon_" + i) == "")
			{
				PlayerPrefs.SetString("weapon_" + i, "0_0_0");
				// первое число - уровень
				// второе число - тип камня (использовать как перечислитель)
				// третье число - сила камня (1 - 100)
			}
			else
			{
			}
		}
		
		PlayerPrefs.Save();
	}
	
}
