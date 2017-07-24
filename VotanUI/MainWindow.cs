using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace VotanUI
{
    public class MainWindow 
        : MonoBehaviour
    {
		SelectArena sceneSc;
		[SerializeField]
		GameObject inventoryMenuPage;
		[SerializeField]
		GameObject mMenuPage;
		[SerializeField]
		GameObject weaponMenuPage;
		[SerializeField]
		GameObject armorMenuPage;

		public void InventotyPageLoad()
        {
			mMenuPage.SetActive(false);
			weaponMenuPage.SetActive(false);
			armorMenuPage.SetActive(false);
			inventoryMenuPage.SetActive(true);
        }


		public void WeaponMenuLoad()
		{
			inventoryMenuPage.SetActive(false);
			weaponMenuPage.SetActive(true);
		}

		public void ArmorMenuLoad()
		{
			inventoryMenuPage.SetActive(false);
			armorMenuPage.SetActive(true);
		}

		public void MainMenuLoad()
		{
			weaponMenuPage.SetActive(false);
			armorMenuPage.SetActive(false);
			inventoryMenuPage.SetActive(false);
			mMenuPage.SetActive(true);
		}
        public void PlayArena()
        {

            SceneManager.LoadScene("Arena_" + 1);
        }

		private void Start()
		{
			sceneSc = new SelectArena();
		}

	}
}
