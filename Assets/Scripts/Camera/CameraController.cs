using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoblinGames
{
    public class CameraController : MonoBehaviour
    {
        enum MouseDirection { Center, Left, Right };
        Vector3 mousePos;
        private const float posX = -1.35f;
        private const float posY = 11.4f;
        private const float leftPosZ = -16.75f;
        private const float leftAngleY = -86f; // -86f
        private const float centerPosZ = -14.5f;
        private const float centerAngleY = -90f; // -90f
        private const float rightPosZ = -11.25f;
        private const float rightAngleY = -94f; // -94f

        private float currentAngleY;
        //private float posSpeed = 4.875f;
        private float posSpeed = 0.5f;
        private float rotateSpeed = 6f;
        private MouseDirection mouseDirection;

        private Vector3 vel;
        // Start is called before the first frame update
        void Awake()
        {
            currentAngleY = -90f;
            mouseDirection = MouseDirection.Center;
        }

        // Update is called once per frame
        void Update()
        {
            CameraMove();
        }

        private void CameraMove()
        {
            MouseDetection();

            if (mouseDirection == MouseDirection.Left)
            {
                if (transform.position.z > leftPosZ)
                {
                    //transform.position += new Vector3(0f, 0f, -posSpeed * Time.deltaTime);
                    transform.position = Vector3.SmoothDamp(transform.position, new Vector3(posX, posY, leftPosZ), ref vel, posSpeed);
                }
                if (currentAngleY < leftAngleY)
                {
                    currentAngleY += rotateSpeed * Time.deltaTime;
                    transform.rotation = Quaternion.Euler(new Vector3(65f, currentAngleY, 0f));
                }

            }
            else if (mouseDirection == MouseDirection.Right)
            {
                if (transform.position.z < rightPosZ)
                {
                    transform.position = Vector3.SmoothDamp(transform.position, new Vector3(posX, posY, rightPosZ), ref vel, posSpeed);
                }
                if (currentAngleY > rightAngleY)
                {
                    currentAngleY -= rotateSpeed * Time.deltaTime;
                    transform.rotation = Quaternion.Euler(new Vector3(65f, currentAngleY, 0f));
                }
            }
            else if (mouseDirection == MouseDirection.Center)
            {
                transform.position = Vector3.SmoothDamp(transform.position, new Vector3(posX, posY, centerPosZ), ref vel, posSpeed);
                if (Mathf.Abs(currentAngleY - centerAngleY) > 0.1f)
                {
                    if (currentAngleY < centerAngleY)
                    {
                        currentAngleY += rotateSpeed * Time.deltaTime;
                    }
                    else if (currentAngleY > centerAngleY)
                    {
                        currentAngleY -= rotateSpeed * Time.deltaTime;
                    }
                    transform.rotation = Quaternion.Euler(new Vector3(65f, currentAngleY, 0f));

                }
            }
        }

        private void MouseDetection()
        {
            mousePos = Input.mousePosition;

            if (mouseDirection == MouseDirection.Left)
            {
                if (mousePos.x > GameManager.screenWidth * 0.35f)
                {
                    mouseDirection = MouseDirection.Center;
                }
            }
            else if (mouseDirection == MouseDirection.Right)
            {
                if (mousePos.x < GameManager.screenWidth * 0.65f)
                {
                    mouseDirection = MouseDirection.Center;
                }
            }
            else if (mouseDirection == MouseDirection.Center)
            {
                if (mousePos.x < GameManager.screenWidth * 0.14f)
                {
                    mouseDirection = MouseDirection.Left;
                }
                else if (mousePos.x > GameManager.screenWidth * 0.86f)
                {
                    mouseDirection = MouseDirection.Right;
                }
            }
        }
    }
}
