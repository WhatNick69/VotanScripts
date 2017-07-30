using System.Collections.Generic;
using MovementEffects;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameBehaviour;
using UnityEngine.UI;
using VotanGameplay;

namespace PlayerBehaviour
{
    /// <summary>
    /// Внутриигровой интерфейс игрока
    /// </summary>
    public class PlayerUI 
        : MonoBehaviour
    {
        private PlayerComponentsControl playerComponentsControl;
        [SerializeField, Tooltip("Gameover окно")]
        private GameObject gameoverWindow;
        [SerializeField, Tooltip("Продолжить игру")]
        private GameObject continuePlayButton;
        [SerializeField, Tooltip("Кнопка паузы")]
        private GameObject pauseButton;
        [SerializeField, Tooltip("Очки на интерфейсе")]
        private GameObject scoreInterface;
        [SerializeField, Tooltip("Очки при выигрыше")]
        private GameObject scores;
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
            playerComponentsControl =
                GetComponent<PlayerComponentsControl>();
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

        /// <summary>
        /// Событие победы
        /// </summary>
        public void EventWin()
        {
            SetActiveOfPlayerInterface(false);
            pauseButton.SetActive(false);
            scoreInterface.SetActive(false);
            scores.SetActive(true);
            scores.transform.GetComponentInChildren<Text>().text = "Scores: " 
                + playerComponentsControl.PlayerScore.ScoreValue;
            Timing.RunCoroutine(CoroutineForVisibleGameOverWindow(0.3f));
        }

        /// <summary>
        /// Событие на проигрышь
        /// </summary>
        public void EventGameOver()
        {
            SetActiveOfPlayerInterface(false);
            pauseButton.SetActive(false);
            Timing.RunCoroutine(CoroutineForVisibleGameOverWindow(1));
        }

        /// <summary>
        /// Корутина на визуализацию окна 
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForVisibleGameOverWindow(float time)
        {
            yield return Timing.WaitForSeconds(time);
            gameoverWindow.SetActive(true);
            CallGameOverWindow(false);
        }

        /// <summary>
        /// ВЫзвать окно проигрыша/выигрыша
        /// </summary>
        /// <param name="isReverse"></param>
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

        /// <summary>
        /// Удалить все элементы в сцене
        /// </summary>
        private void DeleteAll()
        {
            foreach (GameObject gameObject in FindObjectsOfType<GameObject>())
            {
                if (gameObject.CompareTag("ImportantGameobject")) continue;
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Пауза
        /// </summary>
        public void PauseGame()
        {
            SetActiveOfPlayerInterface(false);
            gameoverWindow.SetActive(true);
            continuePlayButton.SetActive(true);
            pauseButton.SetActive(false);
            GameManager.DisableMusic(true);

            Time.timeScale = 0.000001f;
        }

        /// <summary>
        /// Продолжить игру
        /// </summary>
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
