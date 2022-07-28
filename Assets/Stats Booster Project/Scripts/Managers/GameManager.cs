using UnityEngine;

namespace StatsBoosterPrototype.Managers 
{
    public class GameManager : MonoBehaviour
    {
        public GameObject PlayerStatsCanvas;
        public GameObject InventoryCanvas;
        public GameObject MainGUICanvas;

        public void ClosePlayerStatsCanvas()
        {
            PlayerStatsCanvas.SetActive(false);

            MainGUICanvas.SetActive(true);
        }

        public void CloseInventoryCanvas()
        {
            InventoryCanvas.SetActive(false);

            MainGUICanvas.SetActive(true);
        }

        public void OpenInventoryCanvas()
        {
            MainGUICanvas.SetActive(false);
            InventoryCanvas.SetActive(true);
        }

        public void OpenPlayerStatsCanvas()
        {
            MainGUICanvas.SetActive(false);
            PlayerStatsCanvas.SetActive(true);
        }
    }
}
