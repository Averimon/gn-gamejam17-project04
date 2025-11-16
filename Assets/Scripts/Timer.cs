using System.Collections;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private GameObject mask;
    [SerializeField] private float time = 120f;
    
    void Start()
    {
        StartCoroutine(MoveTime());
    }
    
    private IEnumerator MoveTime()
    {
        float prepTime = time;
        float elapsedTime = 0f;
        
        Vector3 endPos = mask.transform.localPosition;
        float maskScaleOffset = mask.transform.localScale.x;
        Vector3 startPos = new Vector3(endPos.x - maskScaleOffset, endPos.y, endPos.z);

        mask.transform.localPosition = startPos;

        while (elapsedTime < prepTime)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / prepTime);
            mask.transform.localPosition = Vector3.Lerp(startPos, endPos, t);

            yield return null;
        }

        mask.transform.localPosition = endPos;
    }
}
