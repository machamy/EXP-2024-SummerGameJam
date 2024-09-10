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
            // 출처 : https://devsquare.tistory.com/11
            Rect viewportRect = camera.rect;

            // 원하는 가로 세로 비율을 계산하는 코드
            float screenAspectRatio = (float)Screen.width / Screen.height;
            float targetAspectRatio = (float)targetWidth/ targetHeight; // 원하는 고정 비율 설정 (예: 16:9)

            // 화면 가로 세로 비율에 따라 뷰포트 영역을 조정하는 코드
            if (screenAspectRatio < targetAspectRatio)
            {
                // 화면이 더 '높다'면 (세로가 더 길다면) 세로를 조절하는 코드
                viewportRect.height = screenAspectRatio / targetAspectRatio;
                viewportRect.y = (1f - viewportRect.height) / 2f;
            }
            else
            {
                // 화면이 더 '넓다'면 (가로가 더 길다면) 가로를 조절하는 코드.
                viewportRect.width = targetAspectRatio / screenAspectRatio;
                viewportRect.x = (1f - viewportRect.width) / 2f;
            }

            // 조정된 뷰포트 영역을 카메라에 설정하는 코드
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