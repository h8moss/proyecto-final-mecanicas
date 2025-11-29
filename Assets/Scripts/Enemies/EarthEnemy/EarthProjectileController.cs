using UnityEngine;

[RequireComponent(typeof(PooledObjectHelper))]
public class EarthProjectileController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float maxHeight;

    private PooledObjectHelper pooledObject;

    private Vector3 startPos;
    private Vector3 targetPos;
    private float progress = 0f;
    private float distance;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pooledObject = GetComponent<PooledObjectHelper>();
        pooledObject.onReset += Reset;
        Reset();
    }

    void Update()
    {
        progress += (speed / distance) * Time.deltaTime;
        if (progress >= 1f) {
            gameObject.SetActive(false);
            return;
        }

        Vector3 currentPos = Vector3.Lerp(startPos, targetPos, progress);
        float heightOffset = Mathf.Sin(progress * Mathf.PI) * maxHeight;

        currentPos.y += heightOffset;
        transform.position = currentPos;

        if (progress > 0.01f)
        {
            Vector3 nextPos = Vector3.Lerp(startPos, targetPos, progress + 0.01f);
            nextPos.y += Mathf.Sin((progress + 0.01f) * Mathf.PI) * maxHeight;
            transform.rotation = Quaternion.LookRotation(nextPos - currentPos);
        }
    }

    void Reset() {
        progress = 0f;
    }

    public void SetTarget(Vector3 position) {
        startPos = transform.position;
        targetPos = position;
        distance = Vector3.Distance(targetPos, startPos);
    }
}
