using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasScaler))]
public class CanvasReferenceScaler : MonoBehaviour
{
    private CanvasScaler canvasScaler;

    [Header("Canvas Reference Setup")] [SerializeField]
    private Vector2 mainReferenceResolution = new Vector2(1536f, 2048f);

    [SerializeField] [Range(0.0f, 1f)] private float matchWidthOrHeight;
    [SerializeField] private CanvasScaler.ScaleMode uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

    private void Awake()
    {
        this.canvasScaler = this.GetComponent<CanvasScaler>();
        this.AdjustReferenceResolution();
    }

    private void AdjustReferenceResolution()
    {
        if (!(bool) (Object) this.canvasScaler)
            return;
        this.canvasScaler.uiScaleMode = this.uiScaleMode;
        this.canvasScaler.matchWidthOrHeight = this.matchWidthOrHeight;
        this.canvasScaler.referenceResolution =
            new Vector2(
                (float) ((double) this.mainReferenceResolution.x *
                    ((double) Screen.width * 1.0 / (double) Screen.height) / 0.4618000090122223),
                this.mainReferenceResolution.y);
    }
}