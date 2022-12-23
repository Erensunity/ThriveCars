using UnityEngine;

namespace Project.Scripts.Core
{
    using System;
    using UnityEngine;
    using DG.Tweening;

    public class CanvasFade : MonoBehaviour
    {
        [SerializeField] private CanvasGroup mCanvas;
        private Tween fadeTween;

        [SerializeField] private float fadeDuration;

        private void OnValidate()
        {
            mCanvas = GetComponent<CanvasGroup>();
        }

        private void OnEnable()
        {
            mCanvas = GetComponent<CanvasGroup>();
            FadeIn(fadeDuration);
        }

        public void FadeIn()
        {
            mCanvas.alpha = 0.0f;
            gameObject.SetActive(true);

            FadeIn(fadeDuration);
        }

        public void FadeIn(float duration)
        {
            Fade(1f, duration, () =>
            {
                mCanvas.interactable = true;
                mCanvas.blocksRaycasts = true;
            });
        }

        public void FadeOut()
        {
            FadeOut(fadeDuration, () => { gameObject.SetActive(false); });
        }

        public void FadeOut(float duration, Action onComplete = null)
        {
            Fade(0f, duration, () =>
            {
                onComplete?.Invoke();
                mCanvas.interactable = false;
                mCanvas.blocksRaycasts = false;
            });
        }

        private void Fade(float endvalue, float duration, TweenCallback onEnd)
        {
            if (fadeTween != null)
            {
                fadeTween.Kill(false);
            }

            fadeTween = mCanvas.DOFade(endvalue, duration);
            fadeTween.onComplete += onEnd;
        }
    }
}