using System;
using System.Collections.Generic;
using MovementEffects;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

namespace PlayerBehaviour
{
    /// <summary>
    /// Внутриигровой интерфейс игрока
    /// </summary>
    public class PlayerUI 
        : MonoBehaviour
    {
        [SerializeField, Tooltip("Интерфейс экрана игрока")]
        private GameObject playerInterface;
        [SerializeField, Tooltip("Gameover окно")]
        private GameObject gameoverWindow;
        private Animation gameoverWindowAnimation;
        private MobileControlRig mobileControlRig;
        private string animName = "GameOverUIAnimation";

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Start()
        {
            mobileControlRig =
                playerInterface.transform.parent.
                GetComponent<MobileControlRig>();
            gameoverWindowAnimation =
                gameoverWindow.GetComponent<Animation>();
            gameoverWindow.SetActive(false);
        }

        /// <summary>
        /// Задать состояние видимости интерфейса экрана игрока
        /// </summary>
        /// <param name="active"></param>
        public void SetActiveOfPlayerInterface(bool active)
        {
            mobileControlRig.enabled = false;
            playerInterface.SetActive(active);
        }

        public void EventGameOver()
        {
            SetActiveOfPlayerInterface(false);
            Timing.RunCoroutine(CoroutineForVisibleGameOverWindow());
        }

        private IEnumerator<float> CoroutineForVisibleGameOverWindow()
        {
            yield return Timing.WaitForSeconds(1);
            gameoverWindow.SetActive(true);
            CallGameOverWindow(false);
        }

        private void CallGameOverWindow(bool isReverse)
        {
            if (isReverse)
            {
                gameoverWindowAnimation[animName].speed = -1;
                gameoverWindowAnimation[animName].time =
                    gameoverWindowAnimation[animName].length;
            }
            else
            {
                gameoverWindowAnimation[animName].speed = 1;
                gameoverWindowAnimation[animName].time = 0;
            }
            gameoverWindowAnimation.Play();
        }

        /// <summary>
        /// Выйти в меню
        /// </summary>
        public void ExitToMenu()
        {
            Timing.KillCoroutines();
            SceneManager.LoadScene(0);
        }
    }
}
