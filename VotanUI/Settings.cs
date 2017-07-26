using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
	[SerializeField]
	Slider slider;

	private void FixedUpdate()
	{
		AudioListener.volume = slider.value;
	}


}
