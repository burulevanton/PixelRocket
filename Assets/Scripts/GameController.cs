using System;
using UnityEngine;

public class GameController : Singleton<GameController>
{
        public bool IsPause { get; set; } = false;

        public bool IsEnd { get; set; } = false;

        public bool InMainMenu { get; private set; } = true;

        public GameObject PauseMenu;

        public PlayerController Player;

        public GameObject EndGameMenu;

        public GameObject MainMenu;

        public BlackPanel blackPanel;

        public AudioListener audioListener;

        public void ChangeMusicMute(bool value)
        {
                audioListener.enabled = value;
        }


        public void PauseGame()
        {
                if (PauseMenu.activeSelf)
                {
                        UnPauseGame();
                        return;
                }
                Time.timeScale = 0.0f;
                PauseMenu.SetActive(true);
                IsPause = true;
        }

        public void UnPauseGame()
        {
                Time.timeScale = 1.0f;
                PauseMenu.SetActive(false);
                IsPause = false;
        }

        public void RestartGame()
        {
                UnPauseGame();
                blackPanel.gameObject.SetActive(true);
                blackPanel.FadeScene((() =>
                {
                        IsEnd = true;
                        RoomGenerator.Instance.ClearScene();
                        Player.RestartGame();
                        IsEnd = false;
                        RoomGenerator.Instance.StartGenerate();
                        PauseMenu.SetActive(false);
                        EndGameMenu.SetActive(false);
                        InMainMenu = false;
                        AfterFade();
                }));
        }

        private void AfterFade()
        {
                blackPanel.AppearanceScene(() =>
                {
                        blackPanel.gameObject.SetActive(false);   
                });
        }

        public void EndGame()
        {
                EndGameMenu.SetActive(true);
        }

        public void Respawn()
        {
                EndGameMenu.SetActive(false);
                Player.Respawn();
                GameData.Instance.AddCoins(-1000);
        }

        public void StartGame()
        {
                MainMenu.SetActive(false);
                RestartGame();
        }

        private void Start()
        {
                RoomGenerator.Instance.StartGenerate();
                ChangeMusicMute(GameData.Instance.Music);
        }

        public void Exit()
        {
                Application.Quit();
        }
}