using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VotanUI
{
    /// <summary>
    /// Выбор арены
    /// </summary>
    public class SelectArena 
        : MonoBehaviour
    {

        [SerializeField]
        private Transform arenaRepository;
        [SerializeField]
        GameObject arenaButton;
        [SerializeField, Tooltip("Арены")]
        private List<SceneScript> sceneList;
        private Scene arena;
        string arenaPrefix = "Scenes/Arena/Arena_";
        int arenaNumber;
        SelectArena SA;

        public int ArenaNumber
        {
            get;
            set;
        }

        void Start()
        {
            SA = GetComponent<SelectArena>();
            sceneList = new List<SceneScript>();
            StartCoroutine(arenaLoad());
        }

        private IEnumerator arenaLoad()
        {
            int count = Resources.LoadAll("Scenes/Arena").Length;
            for (int i = 0; i < count; i++)
            {
                GameObject ar = (GameObject)Resources.Load(arenaPrefix + (i + 1));
                GameObject arena = Instantiate(arenaButton);
                sceneList.Add(ar.GetComponent<SceneScript>());


                arena.transform.SetParent(arenaRepository, false);
            }
            yield return 0;
        }
    }
}
