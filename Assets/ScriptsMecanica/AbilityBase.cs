using UnityEngine;

public abstract class AbilityBase : MonoBehaviour
{
    public virtual void Activate() { }
    public virtual void Activate(Vector3 position) { }
    public virtual void ShowPreview() { }
    public virtual void HidePreview() { }
}
