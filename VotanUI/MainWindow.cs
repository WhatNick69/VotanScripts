using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;
using MovementEffects;
using ShopSystem;
using CraftSystem;

namespace VotanUI
{
    /// <summary>
    /// Главное меню
    /// </summary>
    public class MainWindow 
        : MonoBehaviour
    {
        #region Переменные и ссылки
        [SerializeField]
        private Shop shopComponent;
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
		GameObject Shop;

        [SerializeField]
        private Image imageFromLoadingFone;

        [SerializeField]
        private TouchButtonEffect[] shopTouchButtons;
        #endregion

        #region Свойства
        public void InventotyPageLoad()
        {
			onMenu(inventoryMenuPage);
        }

		public void ShopPageLoad()
		{
			onMenu(Shop);
            shopComponent.CommonStoreWindow();

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
        #endregion

        /// <summary>
        /// Запуск арены
        /// </summary>
        public void PlayArena()
        {
            Timing.RunCoroutine(CoroutineForLoadingScene());
        }

        /// <summary>
        /// Корутина на загрузку сцены
        /// </summary>
        /// <returns></returns>
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
				if (allMenu[i])
				{
					allMenu[i].SetActive(false);
				}
				else
				{
					continue;
				}
			}
            MenuSoundManager.PlaySoundStatic();
            if (page) page.SetActive(true);
		}

        /// <summary>
        /// Скрыть тачи всех кнопок
        /// </summary>
        /// <param name="tBE"></param>
        public void UnshowAllTouchesForButtons(TouchButtonEffect tBE)
        {
            for (int i = 0; i < shopTouchButtons.Length; i++)
                shopTouchButtons[i].HighlightingControl(false);
            tBE.HighlightingControl(true);
        }

        public void UnshowAllTouchesForButtons()
        {
            for (int i = 0; i < shopTouchButtons.Length; i++)
                shopTouchButtons[i].HighlightingControl(false);
        }
	}
}
