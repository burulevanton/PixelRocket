using System;
using System.Linq;
using UnityEngine;

public class CoinStack:MonoBehaviour
{
    public GameObject[] coins;

    private void OnDisable()
    {
        foreach (var coin in coins)
        {
            coin.SetActive(true);
        }
    }
}