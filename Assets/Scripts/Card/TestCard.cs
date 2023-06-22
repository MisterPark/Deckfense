using Codice.CM.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoblinGames
{
    public class TestCard : MonoBehaviour
    {

        private Vector3 originPosition;
        private Vector3 destination;
        private float tick;
        bool isMoving = false;

        private void Update()
        {
            ProcessMove();
        }

        private void ProcessMove()
        {
            if(isMoving)
            {
                tick += Time.deltaTime;
                Vector3 curPostion = Vector3.Lerp(originPosition, destination, tick);
                transform.localPosition = curPostion;
                if(tick >= 1f)
                {
                    isMoving = false;
                    tick = 0f;
                }
            }
          
        }

        public void SetDestination(Vector3 destination)
        {
            originPosition = transform.localPosition;
            this.destination = destination;
            isMoving = true;
            tick = 0f;
        }
    }
}
