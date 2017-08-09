using System.Collections.Generic;
using MovementEffects;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VotanGameplay;
using UnityStandardAssets.CrossPlatformInput;

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

        // ИНВЕНТАРЬ
        [SerializeField, Tooltip("Левая кнопка")]
        private RectTransform leftButton;
        [SerializeField, Tooltip("Правая кнопка")]
        private RectTransform rightButton;
        [SerializeField, Tooltip("Левый инвентарь")]
        private RectTransform leftInventory;
        [SerializeField, Tooltip("Правый инвентарь")]
        private RectTransform rightInventory;
        [SerializeField, Tooltip("Кнопка, нажатие по которой приводит к закрытию инвентарей")]
        private Button outsideButton;

        private string animName = "GameOverUIAnimation";
        private bool isRightInventoryOpen = true;
        private bool isLeftInventoryOpen = true;
        private Animation gameoverWindowAnimation;

        private Coroutine rightCoroutine;
        private Coroutine leftCoroutine;
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
            Timing.RunCoroutine(EventPlayMode());
        }

        private IEnumerator<float> EventPlayMode()
        {
            while (playerComponentsControl.PlayerConditions.IsAlive)
            {
                if (Joystick.IsPlaying)
                {
                    OnClickCloseAllWindows();
                }
                yield return Timing.WaitForSeconds(1);
            }
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
        /// Открыть правый инвентарь
        /// </summary>
        public void OnClickOpenRightInventory()
        {
            if (rightCoroutine != null)
                StopCoroutine(rightCoroutine);

            if (!isRightInventoryOpen)
            {
                playerComponentsControl.PlayerHUD.PlaySoundSwipeInventory(true);
                isRightInventoryOpen = true;
                rightCoroutine = 
                    StartCoroutine(CoroutineForMoveInventoryWindow
                    (rightInventory, rightButton, 100,-30));
            }
            else
            {
                playerComponentsControl.PlayerHUD.PlaySoundSwipeInventory(false);
                isRightInventoryOpen = false;
                rightCoroutine = 
                    StartCoroutine(CoroutineForMoveInventoryWindow
                    (rightInventory, rightButton, -75,100));
            }
        }

        /// <summary>
        /// Открыть левый инвентарь
        /// </summary>
        public void OnClickOpenLeftInventory()
        {
            if (leftCoroutine != null)
                StopCoroutine(leftCoroutine);

            if (!isLeftInventoryOpen)
            {
                playerComponentsControl.PlayerHUD.PlaySoundSwipeInventory(true);
                isLeftInventoryOpen = true;
                leftCoroutine = 
                    StartCoroutine(CoroutineForMoveInventoryWindow
                    (leftInventory, leftButton, -100,30));
            }
            else
            {
                playerComponentsControl.PlayerHUD.PlaySoundSwipeInventory(false);
                isLeftInventoryOpen = false;
                leftCoroutine = 
                    StartCoroutine(CoroutineForMoveInventoryWindow
                    (leftInventory, leftButton, 75, -100));
            }
        }

        /// <summary>
        /// Корутина для движения окна с инвентарем
        /// </summary>
        /// <param name="inventory">Компонент объекта для движения</param>
        /// <param name="destinationCor">Конечная координата движения</param>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForMoveInventoryWindow
            (RectTransform inventory, RectTransform button, float destinationCorInventory,float destinationCorButton)
        {
            int i = 0;
            float cachedTime;
            Vector2 tempVectorInventory = new Vector2(destinationCorInventory, 0);
            Vector2 tempVectorButton = new Vector2(destinationCorButton, button.anchoredPosition.y);

            while (Mathf.Abs(inventory.anchoredPosition.x - destinationCorInventory) >= 3)
            {
                i++;
                cachedTime = Time.deltaTime * i;
                inventory.anchoredPosition =
                    Vector2.Lerp(inventory.anchoredPosition, tempVectorInventory, cachedTime);
                button.anchoredPosition =
                     Vector2.Lerp(button.anchoredPosition, tempVectorButton, cachedTime);
                yield return Timing.WaitForOneFrame;
            }
        }

        /// <summary>
        /// Закрыть все окна
        /// </summary>
        public void OnClickCloseAllWindows()
        {
            bool flag = false;

            if (!isLeftInventoryOpen)
            {
                flag = true;
                if (leftInventory != null)
                    StopCoroutine(leftCoroutine);

                isLeftInventoryOpen = true;
                leftCoroutine =
                    StartCoroutine(CoroutineForMoveInventoryWindow
                    (leftInventory, leftButton, -100,30));
            }

            if (!isRightInventoryOpen)
            {
                flag = true;
                if (rightCoroutine != null)
                    StopCoroutine(rightCoroutine);

                isRightInventoryOpen = true;
                rightCoroutine =
                    StartCoroutine(CoroutineForMoveInventoryWindow
                    (rightInventory, rightButton, 100,-30));
            }

            if (flag)
                playerComponentsControl.PlayerHUD.PlaySoundSwipeInventory(true);
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
