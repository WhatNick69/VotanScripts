using System.Collections.Generic;
using MovementEffects;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VotanGameplay;
using VotanLibraries;
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
        [SerializeField, Tooltip("Очки-объект")]
        private GameObject scoresObject;
        [SerializeField, Tooltip("Железо-объект")]
        private GameObject steelObject;
        [SerializeField, Tooltip("Дерево-объект")]
        private GameObject woodObject;
        [SerializeField, Tooltip("Гемы-объект")]
        private GameObject gemObject;
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
        /// Установить количество железа, дерева и гемов 
        /// исходя из количества очков после окончания игры
        /// </summary>
        private bool SetTotalResourcesAfterGame(bool isWin)
        {
            if (!isWin) playerComponentsControl.PlayerResources.ScoreValue /= 4;

            LibraryStaticFunctions.ConvertScoreToResources
                (playerComponentsControl.PlayerResources);

            Timing.RunCoroutine(CoroutineForSlowmotionAddingScore
                (scoresObject.transform.GetComponentInChildren<Text>(),
                "Score: ", 1, playerComponentsControl.PlayerResources.ScoreValue, 50));
            Timing.RunCoroutine(CoroutineForSlowmotionAddingScore
                (woodObject.transform.GetComponentInChildren<Text>(),
                "Wood: ",1, playerComponentsControl.PlayerResources.WoodResource, 1));
            Timing.RunCoroutine(CoroutineForSlowmotionAddingScore
                (steelObject.transform.GetComponentInChildren<Text>(),
                "Steel: ", 1, playerComponentsControl.PlayerResources.SteelResource, 1));

            if (playerComponentsControl.PlayerResources.Gems > 0)
            {
                if (isWin)
                {
                    gemObject.transform.GetComponentInChildren<Text>().text =
                        "Gems: " + playerComponentsControl.PlayerResources.Gems;
                    return true;
                }
                else
                {
                    playerComponentsControl.PlayerResources.Gems = 0;
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Корутина для плавного достижение 
        /// определенного числового значения
        /// </summary>
        /// <param name="addScoreValue"></param>
        /// <returns></returns>
        private IEnumerator<float> CoroutineForSlowmotionAddingScore
            (Text textElement, string prefix, float latency, long destinationValue, int incrementValue)
        {
            yield return Timing.WaitForSeconds(latency);

            long tempValue = destinationValue;
            int value = 0;
            while (tempValue > 0)
            {
                if (tempValue / incrementValue >= 1)
                {
                    tempValue -= incrementValue;
                    value += incrementValue;
                }
                else
                {
                    tempValue--;
                    value++;
                }
                textElement.text = prefix + value;
                yield return Timing.WaitForOneFrame;
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
            scoresObject.SetActive(true);
            woodObject.SetActive(true);
            steelObject.SetActive(true);
            gemObject.SetActive(SetTotalResourcesAfterGame(true));

            Joystick.IsGameOver = true;

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
            scoresObject.SetActive(true);
            woodObject.SetActive(true);
            steelObject.SetActive(true);
            gemObject.SetActive(SetTotalResourcesAfterGame(false));

            pauseButton.SetActive(false);

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
