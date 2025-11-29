using UnityEngine;

public class PooledObjectHelper : MonoBehaviour
{
    public delegate void OnReset();
    public OnReset onReset;

    public void Reset()
    {
        onReset?.Invoke();
    }
}
