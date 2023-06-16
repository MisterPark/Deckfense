using UnityEngine;
using UnityEngine.EventSystems;

namespace GoblinGames
{
    public class Card : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
    {
        
        
        private bool isDragging = false;

        private Vector3 originPosition;
        private Quaternion originRotation;
        private Vector3 originScale;

        public void OnBeginDrag(PointerEventData eventData)
        {
            isDragging = true;

            originPosition = transform.localPosition;
            originRotation = transform.localRotation;
            originScale = transform.localScale;

            transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(1.2f, 1.2f);

            Debug.Log("Begin");
        }

        public void OnDrag(PointerEventData eventData)
        {
            if(isDragging)
            {
                transform.localPosition = eventData.position;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            isDragging = false;

            transform.localPosition = originPosition;
            transform.localRotation = originRotation;
            transform.localScale = originScale;

            Debug.Log("End");
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            
        }
    }
}
