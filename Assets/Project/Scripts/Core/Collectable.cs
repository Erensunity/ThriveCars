using System;
using TMPro;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI numberText = null;
    [SerializeField] private Animator textAnimator = null;
    [SerializeField] private float destroyDelay = 0.0f;
    [SerializeField] private Color positive;
    [SerializeField] private Color negative;
    

    public void Init(float value)
    {
        numberText.text = value < 0 ? "" + (int) value : "+" + (int) value;
        numberText.color = value < 0 ? negative : positive;
        textAnimator.enabled = true;
    }

    public void InitG(float value)
    {
        numberText.text = "+" + (int) value;
        numberText.color = Color.green;
        textAnimator.enabled = true;
    }

    public void InitLevel()
    {
        numberText.text = "Level +1";
        numberText.color = Color.white;
        textAnimator.enabled = true;
    }

    public void OnAnimationEnd()
    {
        Destroy(gameObject, destroyDelay);
    }

    private void Update()
    {
        Vector3 direction;
        direction = transform.position - Camera.main.transform.position;
        transform.rotation = Quaternion.LookRotation(direction);
    }
}