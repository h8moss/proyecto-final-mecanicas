using UnityEngine;
using UnityEngine.VFX;

public class fireBallScript : MonoBehaviour
{
    private bool GotHit = false;

    public VisualEffect vfxPrefab;
    public GameObject objectToDisable;

    public Rigidbody rb;

    public float rotationSpeed = 100f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (GotHit) return;

        GotHit = true;

        if (vfxPrefab != null)
        {
            VisualEffect vfxInstance = Instantiate(vfxPrefab, collision.contacts[0].point, Quaternion.identity);
            vfxInstance.SendEvent("OnPlay");
            Destroy(vfxInstance.gameObject, 1f);
        }

    }
}
