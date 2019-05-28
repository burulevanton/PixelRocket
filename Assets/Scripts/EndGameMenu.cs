using System;
using UnityEngine;
using UnityEngine.UI;

public class EndGameMenu : MonoBehaviour
{
        public Text distCovered;
        public Text coinsCollected;

        private void OnEnable()
        {
                distCovered.text = GameController.Instance.Player.DistanceCovered.ToString() + " м";
                coinsCollected.text = GameController.Instance.Player.Coins.ToString();
        }
}