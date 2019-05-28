using System;
using UnityEngine;
using UnityEngine.UI;

public class BlackPanel : MonoBehaviour
{
        public Action action;
        private Animator _animator;
        private Image image;

        private void Awake()
        {
                _animator = GetComponent<Animator>();
                image = GetComponent<Image>();
        }

        public void FadeScene(Action action)
        {
                _animator.Play("Fade");
                this.action = action;
        }

        public void OnFade()
        {
                action();
        }

        public void AppearanceScene(Action action)
        {
                _animator.Play("Appearance");
                this.action = action;
        }

        public void OnAppearance()
        {
                action();
        }
}