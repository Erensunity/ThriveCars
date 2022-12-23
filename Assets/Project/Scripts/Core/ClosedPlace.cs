using DG.Tweening;
using UnityEngine;

namespace Project.Scripts.Core
{
    public class ClosedPlace : MonoBehaviour
    {
        private Vector3 firstScale;

        private void Awake()
        {
            firstScale = transform.localScale;
        }

        protected virtual void OnEnable()
        {
            PlayAnimation();
        }

        public void PlayAnimation()
        {
            transform.DOKill();
         //   GameManager.Instance.player.levelUpParticle.Play();
            transform.localScale = Vector3.zero;
            transform.DOScale(firstScale, 0.5f).SetEase(Ease.OutBack);
        }
    }
}