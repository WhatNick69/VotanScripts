using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

	[SerializeField]
	GameObject weapon;
	[SerializeField]
	GameObject armor;

	public void WeaponWindow()
	{
		armor.SetActive(false);
		weapon.SetActive(true);
	}

	public void ArmorWindow()
	{
		weapon.SetActive(false);
		armor.SetActive(true);
	}
}
