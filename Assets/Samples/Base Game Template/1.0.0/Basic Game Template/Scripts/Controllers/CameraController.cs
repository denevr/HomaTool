using System.Collections;
using UnityEngine;

namespace HomaGames.Internal.BaseTemplate.Examples.Basic
{
    public class CameraController : MonoBehaviour
    {
        private new Camera camera;

        private void Awake()
        {
            camera = GetComponent<Camera>();
        }

        public void ZoomIn()
        {
            StopAllCoroutines();
            StartCoroutine(ChangeFoVRoutine(30, 1));
        }

        public void ZoomOut()
        {
            StopAllCoroutines();
            StartCoroutine(ChangeFoVRoutine(60, 1));
        }

        private IEnumerator ChangeFoVRoutine(float fov, float time)
        {
            float percent = 0;
            float elapsed = 0;
            float startFoV = camera.fieldOfView;
            while (percent < 1)
            {
                percent = elapsed / time;
                camera.fieldOfView = Mathf.Lerp(startFoV, fov, percent);
                yield return null;
                elapsed += Time.deltaTime;
            }
        }
    }
}