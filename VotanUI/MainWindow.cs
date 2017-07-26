﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;
using MovementEffects;
using System;

namespace VotanUI
{
    public class MainWindow 
        : MonoBehaviour
    {
		SelectArena sceneSc;

		[SerializeField]
		List<GameObject> allMenu;
		
		[SerializeField]
		GameObject inventoryMenuPage;
		[SerializeField]
		GameObject mMenuPage;
		[SerializeField]
		GameObject weaponMenuPage;
		[SerializeField]
		GameObject armorMenuPage;
		[SerializeField]
		GameObject selectArenaPage;
		[SerializeField]
		GameObject Settings;

        [SerializeField]
        private Image imageFromLoadingFone;

		public void InventotyPageLoad()
        {
			onMenu(inventoryMenuPage);
        }


		public void WeaponMenuLoad()
		{
			onMenu(weaponMenuPage);
		}

		public void ArmorMenuLoad()
		{
			onMenu(armorMenuPage);
		}

		public void MainMenuLoad()
		{
			onMenu(mMenuPage);
		}

        public void InventoryMenuLoad()
        {
            onMenu(inventoryMenuPage);
        }

        public void SelectMapLoad()
		{
			onMenu(selectArenaPage);
		}

		public void SettingsLoad()
		{
			onMenu(Settings);
		}

        public void PlayArena()
        {
            Timing.RunCoroutine(CoroutineForLoadingScene());
        }

        private IEnumerator<float> CoroutineForLoadingScene()
        {
            onMenu();
            MenuSoundManager.PlaySoundStatic(2);
            while (true)
            {
                imageFromLoadingFone.color = 
                    Color.Lerp(imageFromLoadingFone.color, Color.white, Time.deltaTime);
                if (imageFromLoadingFone.color.a >= 0.95f) break;
                yield return Timing.WaitForOneFrame;
            }
            SceneManager.LoadScene("Arena_" + 1);
        }

        /// <summary>
        /// Включить какое то меню
        /// </summary>
        /// <param name="page"></param>
        private void onMenu(GameObject page=null)
		{
			for (int i = 0; i < allMenu.Count; i++)
			{
				allMenu[i].SetActive(false);
			}
            MenuSoundManager.PlaySoundStatic();
            if (page) page.SetActive(true);
		}
		private void Start()
		{
			//sceneSc = new SelectArena();
		}

	}
}
