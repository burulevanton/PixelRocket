using System;
using UnityEngine;
using UnityEngine.UI;

public class EndGameMenu : MonoBehaviour
{
        public Text distCovered;
        public Text coinsCollected;
        public Text newRecord;

        public GameObject ButtonRespawn;

        private void OnEnable()
        {
                distCovered.text = GameController.Instance.Player.DistanceCovered.ToString();
                if (GameController.Instance.Player.DistanceCovered > GameData.Instance.Record)
                {
                        newRecord.gameObject.SetActive(true);
                        GameData.Instance.SetNewRecord(GameController.Instance.Player.DistanceCovered);
                }
                GameData.Instance.AddCoins((int)GameController.Instance.Player.Coins);
                coinsCollected.text = GameData.Instance.Coins.ToString();
                if(GameData.Instance.Coins > 1000)
                        ButtonRespawn.SetActive(true);
                        
        }

        private void OnDisable()
        {
                newRecord.gameObject.SetActive(false);
                ButtonRespawn.SetActive(false);
        }
}