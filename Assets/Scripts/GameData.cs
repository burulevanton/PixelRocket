using System;
using System.Collections.Generic;
using UnityEngine;

public class GameData : Singleton<GameData>
{
    public bool Music { get; private set; }
    
    public int Record { get; private set; }
    public int Coins { get; private set; }
    
    public int Difficulty { get; private set; }

    private void SetPlayerPrefs()
    {
        PlayerPrefs.SetInt("Music", 1);
        PlayerPrefs.SetInt("Record", 0);
        PlayerPrefs.SetInt("Coins", 0);
        PlayerPrefs.SetInt("Difficulty", 1);
        PlayerPrefs.Save();
    }

    private void Awake()
    {
        if(!PlayerPrefs.HasKey("Music"))
            SetPlayerPrefs();
        Music = PlayerPrefs.GetInt("Music") == 1;
        Record = PlayerPrefs.GetInt("Record");
        Coins = PlayerPrefs.GetInt("Coins");
        Difficulty = PlayerPrefs.GetInt("Difficulty");
//        AddCoins(1000);
    }

    public void ChangeMusic(bool value)
    {
        var val = value ? 1 : 0;
        PlayerPrefs.SetInt("Music", val);
        Music = value;
        GameController.Instance.ChangeMusicMute(value);
        PlayerPrefs.Save();
    }

    public void SetNewRecord(int value)
    {
        if (Record < value)
        {
            PlayerPrefs.SetInt("Record", value);
            Record = value;
            PlayerPrefs.Save();
        }
    }

    public void AddCoins(int value)
    {
        Coins += value;
        PlayerPrefs.SetInt("Coins", Coins);
        PlayerPrefs.Save();
    }

    public void ChangeDifficulty(int value)
    {
        Difficulty = value;
        PlayerPrefs.SetInt("Difficulty", value);
        PlayerPrefs.Save();
    }
    
}