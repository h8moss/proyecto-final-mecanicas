using UnityEngine;
using System.Collections;

public class TelegraphVisual : MonoBehaviour
{
    [Header("Settings")]
    public float fillTime = 1.0f;
    public Color safeColor = new Color(1, 1, 0, 0.3f); 
    public Color dangerColor = new Color(1, 0, 0, 0.6f); 

    private SpriteRenderer sr;
    private MeshRenderer mr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        mr = GetComponent<MeshRenderer>();
    }

    public void ActivateTelegraph(float duration, Vector3 size)
    {
        transform.localScale = Vector3.zero; 
        gameObject.SetActive(true);
        StartCoroutine(AnimateTelegraph(duration, size));
    }

    private IEnumerator AnimateTelegraph(float time, Vector3 targetScale)
    {
        float timer = 0f;

        
        SetColor(safeColor);

        while (timer < time)
        {
            timer += Time.deltaTime;
            float progress = timer / time;

            
            transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, progress);
            SetColor(Color.Lerp(safeColor, dangerColor, progress));

            yield return null;
        }

        
        gameObject.SetActive(false);
    }

    private void SetColor(Color c)
    {
        if (sr != null) sr.color = c;
        if (mr != null) mr.material.color = c;
    }
}