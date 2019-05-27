using System;
using UnityEngine;

public class GameController : Singleton<GameController>
{
        public bool IsPause { get; set; } = false;

        public bool IsEnd { get; set; } = false;

        public GameObject PauseMenu;

        public MouseController Mouse;
        
        public void PauseGame()
        {
                PauseMenu.SetActive(true);
                Mouse.gameObject.GetComponent<Rigidbody2D>().Sleep();
                Mouse.gameObject.GetComponent<Animator>().enabled = false;
                IsPause = true;
        }

        public void UnPauseGame()
        {
                Mouse.gameObject.GetComponent<Rigidbody2D>().WakeUp();
                Mouse.gameObject.GetComponent<Animator>().enabled = true;
                PauseMenu.SetActive(false);
                IsPause = false;
        }

        public void RestartGame()
        {
                IsEnd = true;
                RoomGenerator.Instance.ClearScene();
                UnPauseGame();
                Mouse.RestartGame();
                RoomGenerator.Instance.StartGenerate();
                IsEnd = false;
                PauseMenu.SetActive(false);
        }

        private void Start()
        {
                RoomGenerator.Instance.StartGenerate();
        }
}