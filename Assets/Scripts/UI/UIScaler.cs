using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoblinGames
{
    public class UIScaler : MonoBehaviour
    {
        [SerializeField] RectTransform rectTransform;
        private Canvas canvas;

        private Vector2 oldScreenSize;

        private void Start()
        {
            canvas = GetComponentInParent<Canvas>();
            oldScreenSize = new Vector2(canvas.pixelRect.width, canvas.pixelRect.height);
            rectTransform.sizeDelta = oldScreenSize;
        }

        private void Update()
        {
            if(CheckEpsilon(oldScreenSize.x, canvas.pixelRect.width) || CheckEpsilon(oldScreenSize.y, canvas.pixelRect.height))
            {
                oldScreenSize = new Vector2(canvas.pixelRect.width, canvas.pixelRect.height);
                rectTransform.sizeDelta = oldScreenSize;
            }
        }

        private void OnValidate()
        {
            if(rectTransform == null) rectTransform = GetComponent<RectTransform>();
        }

        private bool CheckEpsilon(float a, float b)
        {
            float value = Mathf.Abs(a - b);
            if(value > float.Epsilon)
            {
                return true;
            }

            return false;
        }
    }
}
