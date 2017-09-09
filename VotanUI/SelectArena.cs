using CraftSystem;
using MovementEffects;
using System.Collections.Generic;
using UnityEngine;
using VotanLibraries;

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
        private MainWindow mainWindow;
        private ArmorCraft armorCraft;
        private WeaponCraft weaponCraft;
        private ItemsSkillsCraft itemSkillsCraft;
        private GameObject[] scenesArray;

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Start()
        {
            armorCraft = GetComponent<ArmorCraft>();
            weaponCraft = GetComponent<WeaponCraft>();
            itemSkillsCraft = GetComponent<ItemsSkillsCraft>();

            StartCoroutine(arenaLoad());
        }

        /// <summary>
        /// Загрузка арен
        /// </summary>
        /// <returns></returns>
        private IEnumerator<float> arenaLoad()
        {
            ArenaButton arenaButton;
            object[] scenes = Resources.LoadAll("Prefabs/Arena");
            string str = PlayerPrefs.GetString("arenaArray");
            int[] elements = LibraryObjectsWorker.StringSplitter(str, '_');
            scenesArray = new GameObject[scenes.Length];

            if (elements.Length == 0 || elements == null)
            {
                string saveArenaString = "";
                for (int i = 0; i < scenes.Length; i++)
                {
                    saveArenaString += "0_";
                }
                PlayerPrefs.SetString("arenaArray", saveArenaString);
            }

            for (int i = 0; i < scenes.Length; i++)
            {
                scenesArray[i] = Instantiate((GameObject)scenes[i], arenaRepository);
                arenaButton = scenesArray[i].GetComponent<ArenaButton>();
                if (i <= elements.Length-1)
                {
                    if (elements[i] == 0)
                        arenaButton.IsCompleted = false;
                    else
                        arenaButton.IsCompleted = true;
                }
                arenaButton.SceneNumber = i+1;
                arenaButton.Initialisation(mainWindow,armorCraft,weaponCraft, itemSkillsCraft);
            }

            yield return Timing.WaitForOneFrame;
        }
    }
}
