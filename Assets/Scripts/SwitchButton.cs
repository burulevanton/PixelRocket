using System;
using UnityEngine;
using UnityEngine.UI;

public class SwitchButton : MonoBehaviour
{
        public Image ImageOn;
        public Image ImageOff;

        private void OnEnable()
        {
                if (GameData.Instance.Music)
                {
                        ImageOn.gameObject.SetActive(true);
                        ImageOff.gameObject.SetActive(false);
                }
                else
                {
                        ImageOn.gameObject.SetActive(false);
                        ImageOff.gameObject.SetActive(true);
                }
        }

        private void Update()
        {
                if (GameData.Instance.Music)
                {
                        ImageOn.gameObject.SetActive(true);
                        ImageOff.gameObject.SetActive(false);
                }
                else
                {
                        ImageOn.gameObject.SetActive(false);
                        ImageOff.gameObject.SetActive(true);
                }
        }

        public void ButtonClick()
        {
                var prevValue = GameData.Instance.Music;
                GameData.Instance.ChangeMusic(!prevValue);
        }
}