using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoblinGames
{
    public class Panel : MonoBehaviour
    {
        [SerializeField] private GameEvent<Vector2> resolutionChangedEvent;

        [SerializeField] private RectTransform rectTransform;

        private Vector2 oldResolution;

        private void Awake()
        {
            //ó���� 1920f, 1080f �������� ��ƾ� ī�彺���� ������ �� ����
            //oldResolution = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
            oldResolution = new Vector2(1920f, 1080f);
            Debug.Log($"old {rectTransform.rect}");
        }

        private void OnEnable()
        {
            resolutionChangedEvent.AddListener(OnResolutionChanged);
        }


        private void OnDisable()
        {
            resolutionChangedEvent.RemoveListener(OnResolutionChanged);
        }

        private void OnValidate()
        {
            if(rectTransform == null) rectTransform = GetComponent<RectTransform>();
        }


        private void OnResolutionChanged(Vector2 resolution)
        {
            float ratioX = resolution.x / oldResolution.x;
            float ratioY = resolution.y / oldResolution.y;

            rectTransform.localScale = new Vector2(ratioX, ratioY);
        }
    }
}
