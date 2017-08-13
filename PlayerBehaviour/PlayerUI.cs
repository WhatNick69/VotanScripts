using System.Collections.Generic;
using MovementEffects;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        #region Переменные и ссылки
        // ОБЩИЕ ССЫЛКИ
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
        [SerializeField, Tooltip("Стики")]
        private Transform sticks;
        [SerializeField, Tooltip("Инвентарь")]
        private Transform inventory;

        private string animName = "GameOverUIAnimation";

        private Animation gameoverWindowAnimation;
        #endregion

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
        public void SetActiveOfPlayerInterface(Transform sticks, bool active)
        {
            for (int i = 0;i<sticks.childCount;i++)
            {
                if (sticks.GetChild(i).GetComponent<Image>())
                {
                    sticks.GetChild(i).GetComponent<Image>().enabled = active;
                }
                if (sticks.GetChild(i).transform.childCount>0)
                {
                    SetActiveOfPlayerInterface(sticks.GetChild(i), active);
                }
            }
        }

        /// <summary>
        /// Событие победы
        /// </summary>
        public void EventWin()
        {
            SetActiveOfPlayerInterface(sticks,false);
            SetActiveOfPlayerInterface(inventory, false);

            pauseButton.SetActive(false);
            scoreInterface.SetActive(false);
            scores.SetActive(true);

            long score = playerComponentsControl.PlayerScore.ScoreValue;
            scores.transform.GetComponentInChildren<Text>().text = "Scores: "
                + score;
            Timing.RunCoroutine(CoroutineForVisibleGameOverWindow(0.3f));
        }

        /// <summary>
        /// Событие на проигрышь
        /// </summary>
        public void EventGameOver()
        {
            SetActiveOfPlayerInterface(sticks,false);
            SetActiveOfPlayerInterface(inventory, false);

            scoreInterface.SetActive(false);
            scores.SetActive(true);

            pauseButton.SetActive(false);
            long score = playerComponentsControl.PlayerScore.ScoreValue/4;
            scores.transform.GetComponentInChildren<Text>().text = "Scores: "
                + score;
            Timing.RunCoroutine(CoroutineForVisibleGameOverWindow(1));
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
            SetActiveOfPlayerInterface(sticks,true);
            SetActiveOfPlayerInterface(inventory, true);

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
            SetActiveOfPlayerInterface(sticks, false);
            SetActiveOfPlayerInterface(inventory, false);

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

            SetActiveOfPlayerInterface(sticks, true);
            SetActiveOfPlayerInterface(inventory, true);

            gameoverWindow.SetActive(false);
            continuePlayButton.SetActive(false);
            pauseButton.SetActive(true);

            GameManager.DisableMusic(false);
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

    }
}
