using System.Collections.Generic;
using MovementEffects;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameBehaviour;
using UnityEngine.UI;

namespace PlayerBehaviour
{
    /// <summary>
    /// Внутриигровой интерфейс игрока
    /// </summary>
    public class PlayerUI 
        : MonoBehaviour
    {
        [SerializeField, Tooltip("Gameover окно")]
        private GameObject gameoverWindow;
        [SerializeField, Tooltip("Продолжить игру")]
        private GameObject continuePlayButton;
        [SerializeField, Tooltip("Кнопка паузы")]
        private GameObject pauseButton;
        private Animation gameoverWindowAnimation;
        [SerializeField, Tooltip("Левый стик")]
        private Image leftStick;
        [SerializeField, Tooltip("Правый стик")]
        private Image rightStick;
        private string animName = "GameOverUIAnimation";

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Start()
        {
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
            leftStick.enabled = active;
            rightStick.enabled = active;
        }

        public void EventGameOver()
        {
            SetActiveOfPlayerInterface(false);
            pauseButton.SetActive(false);
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
            Time.timeScale = 1;
            SetActiveOfPlayerInterface(true);
            Timing.KillCoroutines();
            DeleteAll();
            SceneManager.LoadScene(0);
        }

        private void DeleteAll()
        {
            foreach (GameObject gameObject in FindObjectsOfType<GameObject>())
            {
                if (gameObject.tag == "ImportantGameobject") continue;
                Destroy(gameObject);
            }
        }

        public void PauseGame()
        {
            SetActiveOfPlayerInterface(false);
            gameoverWindow.SetActive(true);
            continuePlayButton.SetActive(true);
            pauseButton.SetActive(false);
            GameManager.DisableMusic(true);

            Time.timeScale = 0.000001f;
        }

        public void ContinuePlay()
        {
            Time.timeScale = 1;

            SetActiveOfPlayerInterface(true);
            gameoverWindow.SetActive(false);
            continuePlayButton.SetActive(false);
            pauseButton.SetActive(true);
            GameManager.DisableMusic(false);
        }
    }
}
