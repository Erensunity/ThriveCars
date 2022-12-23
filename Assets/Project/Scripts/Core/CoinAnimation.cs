using DG.Tweening;
using UnityEngine;

namespace Project.Scripts.Core
{
    public class CoinAnimation : MonoBehaviour
    {
        public RectTransform rectTransform;
        public float duration;
        public Ease ease;
        public float pickSize;
    
        public void SendMoney(Transform nextParent, Vector3 position)
        {
            var point = Camera.main.WorldToViewportPoint(position);

            rectTransform.anchorMin = point;
            rectTransform.anchorMax = point;
            rectTransform.anchoredPosition = Vector2.zero;

            rectTransform.SetParent(nextParent);
            rectTransform.anchorMin = Vector2.one * 0.5f;
            rectTransform.anchorMax = Vector2.one * 0.5f;
            rectTransform.DOScale(pickSize, duration * 0.1f).SetEase(ease).SetLoops(2, LoopType.Yoyo);
            rectTransform.DOAnchorPos(Vector2.zero, duration).SetEase(ease)
                .OnComplete(() => rectTransform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack)
                    .OnComplete(() => Destroy(gameObject)));
        }
    }
}