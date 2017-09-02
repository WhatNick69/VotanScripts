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
        private List<GameObject> sceneList;

		private List<SceneScript> sceneArray;
        int arenaNumber;

		[SerializeField]
		MainWindow MW;
        SelectArena SA;

        public int ArenaNumber
        {
            get;
            set;
        }

        void Start()
        {
            SA = GetComponent<SelectArena>();
			sceneArray = new List<SceneScript>();
            StartCoroutine(arenaLoad());
        }

        private IEnumerator arenaLoad()
        {
            int count = Resources.LoadAll("Scenes/Arena").Length;
            for (int i = 0; i < count; i++)
            {
				GameObject ar = sceneList[i];
                GameObject arena = Instantiate(arenaButton);
                sceneArray.Add(ar.GetComponent<SceneScript>());


                arena.transform.SetParent(arenaRepository, false);
            }
            yield return 0;
        }
    }
}
