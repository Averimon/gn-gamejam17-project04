using UnityEngine;

public class Chair : MonoBehaviour
{
    private enum ChairColor
    {
        Red,
        Blue,
        Green,
        Yellow
    }

    [SerializeField] private ChairColor chairColor;

    private SpriteRenderer spriteRenderer;
    private SpriteRenderer shadowRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        shadowRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        // SetChairColor();
    }
    
    private void SetChairColor()
    {
        switch (chairColor)
        {
            case ChairColor.Blue:
                spriteRenderer.color = new Color32(97, 117, 212, 255);
                shadowRenderer.color = new Color32(61, 73, 133, 255);
                break;
            case ChairColor.Red:
                spriteRenderer.color = new Color32(212, 100, 97, 255);
                shadowRenderer.color = new Color32(132, 61, 62, 255);
                break;
            case ChairColor.Green:
                spriteRenderer.color = new Color32(97, 212, 104, 255);
                shadowRenderer.color = new Color32(62, 132, 61, 255);
                break;
            case ChairColor.Yellow:
                spriteRenderer.color = new Color32(212, 212, 97, 255);
                shadowRenderer.color = new Color32(132, 132, 61, 255);
                break;
        }
    }
}
