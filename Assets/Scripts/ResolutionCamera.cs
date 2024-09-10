using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class ResolutionCamera : MonoBehaviour
    {
        public int targetWidth = 1080;
        public int targetHeight = 2340;

        public Camera camera;
        
        public void Awake()
        {
            camera = GetComponent<Camera>();
        }

        public void Start()
        {
            // ��ó : https://devsquare.tistory.com/11
            Rect viewportRect = camera.rect;

            // ���ϴ� ���� ���� ������ ����ϴ� �ڵ�
            float screenAspectRatio = (float)Screen.width / Screen.height;
            float targetAspectRatio = (float)targetWidth/ targetHeight; // ���ϴ� ���� ���� ���� (��: 16:9)

            // ȭ�� ���� ���� ������ ���� ����Ʈ ������ �����ϴ� �ڵ�
            if (screenAspectRatio < targetAspectRatio)
            {
                // ȭ���� �� '����'�� (���ΰ� �� ��ٸ�) ���θ� �����ϴ� �ڵ�
                viewportRect.height = screenAspectRatio / targetAspectRatio;
                viewportRect.y = (1f - viewportRect.height) / 2f;
            }
            else
            {
                // ȭ���� �� '�д�'�� (���ΰ� �� ��ٸ�) ���θ� �����ϴ� �ڵ�.
                viewportRect.width = targetAspectRatio / screenAspectRatio;
                viewportRect.x = (1f - viewportRect.width) / 2f;
            }

            // ������ ����Ʈ ������ ī�޶� �����ϴ� �ڵ�
            camera.rect = viewportRect;
        }


        private void OnPreCull()
        {
            Rect rect = camera.rect;
            Rect newRect = new Rect(0, 0, 1, 1);
            camera.rect = newRect;
            GL.Clear(true, true, Color.black);
            camera.rect = rect;
        }
    }
}