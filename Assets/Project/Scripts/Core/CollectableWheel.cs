using DG.Tweening;
//using MoreMountains.NiceVibrations;
using UnityEngine;

namespace Project.Scripts.Core
{
    public class CollectableWheel : MonoBehaviour
    {
        public int Level => level;
        [SerializeField] private int level;
        [SerializeField] private new Collider collider;

        private int carLevel;
        private void OnValidate()
        {
            collider = GetComponent<Collider>();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Head"))
            {
                OnPickCar();
                GameManager.Instance.player.PickAnimation();
            }
        }

        public void OnHitPlayer()
        {
            collider.enabled = false;
            transform.DOScale(0, 0.1f);
        }
        public void OnPickCar()
        {
            if (GameManager.Instance.player.level < level)
            {
                GameManager.Instance.player.KnockBack();
                return;
            }
            OnHitPlayer();
            GameManager.Instance.player.eggParticle.Play();
            Instantiate(GameManager.Instance.player.collectablePrefab, GameManager.Instance.player.headTransform.position+new Vector3(0,1,0), Quaternion.identity, 
                GameManager.Instance.player.transform).Init(level);
         //   MMVibrationManager.Haptic(HapticTypes.MediumImpact);
            GameManager.Instance.player.level += level;
            GameManager.Instance.player.CheckVisual();

            GameManager.Instance.player.levelText.UpdateText(GameManager.Instance.player.level, $"level");
        }
    }
}