using UnityEngine;
using UnityEngine.UI;

public class SwitchButtonDifficulty : MonoBehaviour
{
    public Image Image1;
    public Image Image2;

    private void OnEnable()
    {
        if (GameData.Instance.Difficulty == 1)
        {
            Image1.gameObject.SetActive(true);
            Image2.gameObject.SetActive(false);
        }
        else
        {
            Image1.gameObject.SetActive(false);
            Image2.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        if (GameData.Instance.Difficulty == 1)
        {
            Image1.gameObject.SetActive(true);
            Image2.gameObject.SetActive(false);
        }
        else
        {
            Image1.gameObject.SetActive(false);
            Image2.gameObject.SetActive(true);
        }
    }

    public void ButtonClick()
    {
        var prevValue = GameData.Instance.Difficulty;
        GameData.Instance.ChangeDifficulty(prevValue == 1 ? 2 : 1);
    }
}