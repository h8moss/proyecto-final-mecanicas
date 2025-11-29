using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField] private GameObject pooled;
    [SerializeField] private int maxObjects;
    [SerializeField] private Transform objectParent;

    private GameObject[] instances;

    void Start()
    {
        instances = new GameObject[maxObjects];
        for (int i=0; i<maxObjects; i++)
        {
            instances[i] = Instantiate(pooled, objectParent);
            instances[i].SetActive(false);

        }
    }

    public GameObject GetObject()
    {
        for (int i=0; i<maxObjects; i++)
        {
            if (!instances[i].activeInHierarchy)
            {
                instances[i].SetActive(true);
                if (TryGetComponent(out PooledObjectHelper poh)) {
                    poh.Reset();
                }

                return instances[i];
            }
        }

        return null;
    }
}
