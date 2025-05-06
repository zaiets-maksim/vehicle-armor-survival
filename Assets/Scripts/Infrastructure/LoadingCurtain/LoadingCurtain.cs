﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Connect4.Scripts.Infrastructure
{
    public class LoadingCurtain : MonoBehaviour, ILoadingCurtain
    {
        [SerializeField] private float _delay = 0.5f;
        public event Action OnComplete;
        
        public Image Image;
        public float MoveUpSpeed = 20f;
        public float TimeStep = 0.03f;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        public void SetDelay(float delay)
        {
            _delay = delay;
        }

        public void Show()
        {
            Image.rectTransform.anchoredPosition = Vector2.zero;
            gameObject.SetActive(true);
        }

        public void Hide() =>
            StartCoroutine(GoUp());

        private IEnumerator GoUp()
        {
            yield return new WaitForSeconds(_delay);
            
            while (Image.rectTransform.anchoredPosition.y < Image.rectTransform.rect.height)
            {
                MoveImageUp();
                yield return new WaitForSeconds(TimeStep);
            }

            OnComplete?.Invoke();
            gameObject.SetActive(false);
        }

        private void MoveImageUp()
        {
            RectTransform imageTransform = Image.rectTransform;
            Vector2 anchoredPosition = imageTransform.anchoredPosition;

            anchoredPosition = new Vector2(anchoredPosition.x, anchoredPosition.y + MoveUpSpeed);

            imageTransform.anchoredPosition = anchoredPosition;
        }
    }
}