using System;
using UnityEngine;

public class GameController : Singleton<GameController>
{
        public bool IsPause { get; set; } = false;

        public bool IsEnd { get; set; } = false;

        public GameObject PauseMenu;

        public PlayerController Player;

        public GameObject EndGameMenu;
        
        public void PauseGame()
        {
                Time.timeScale = 0.0f;
                PauseMenu.SetActive(true);
//                Mouse.gameObject.GetComponent<Rigidbody2D>().Sleep();
//                Mouse.gameObject.GetComponent<Animator>().enabled = false;
                IsPause = true;
        }

        public void UnPauseGame()
        {
                Time.timeScale = 1.0f;
//                Mouse.gameObject.GetComponent<Rigidbody2D>().WakeUp();
//                Mouse.gameObject.GetComponent<Animator>().enabled = true;
                PauseMenu.SetActive(false);
                IsPause = false;
        }

        public void RestartGame()
        {
                IsEnd = true;
                RoomGenerator.Instance.ClearScene();
                UnPauseGame();
                Player.RestartGame();
                IsEnd = false;
                RoomGenerator.Instance.StartGenerate();
                PauseMenu.SetActive(false);
                EndGameMenu.SetActive(false);
        }

        public void EndGame()
        {
                EndGameMenu.SetActive(true);
                IsEnd = true;
        }

        private void Start()
        {
                RoomGenerator.Instance.StartGenerate();
        }
}