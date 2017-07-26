using UnityEngine;
using UnityEngine.UI;

namespace VotanUI
{
    /// <summary>
    /// Выбрать арену
    /// </summary>
    public class ArenaButton 
        : MonoBehaviour
    {

        [SerializeField]
        Text arenaName;
        int numberButton;
        SelectArena SA;

        public string ArenaName
        {
            set
            {
                arenaName.text = value;
            }
        }

        public int SetNumber
        {
            set
            {
                numberButton = value;
            }
        }

        public void SetWeaponCraft(SelectArena sa)
        {
            SA = sa;
        }

        public void GetNumber()
        {
            SA.ArenaNumber = numberButton;
        }
    }
}
