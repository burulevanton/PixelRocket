using System;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class GameUI : MonoBehaviour
    {
        public GameObject CoinsPanel;
        public GameObject DistancePanel;
        public GameObject HealthPanel;

        public Text CoinsText;
        public Text DistanceText;
        public Text HealthText;

        private void Update()
        {
            if (GameController.Instance.InMainMenu)
            {
                CoinsPanel.SetActive(true);
                DistancePanel.SetActive(true);
                HealthPanel.SetActive(false);
                CoinsText.text = GameData.Instance.Coins.ToString();
                DistanceText.text = GameData.Instance.Record.ToString();
            }
            else
            {
                CoinsPanel.SetActive(true);
                DistancePanel.SetActive(true);
                HealthPanel.SetActive(true);
                CoinsText.text = GameController.Instance.Player.Coins.ToString();
                DistanceText.text = GameController.Instance.Player.DistanceCovered.ToString();
                HealthText.text = GameController.Instance.Player.Lives.ToString();
            }
        }
    }
}