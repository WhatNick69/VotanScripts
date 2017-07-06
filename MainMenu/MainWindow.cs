using UnityEngine;
using UnityEngine.SceneManagement;

namespace VotanUI
{
    public class MainWindow 
        : MonoBehaviour
    {

        public void InventotyPageLoad()
        {

        }

        public void PlayArena()
        {
            SceneManager.LoadScene(1);
        }
    }
}
