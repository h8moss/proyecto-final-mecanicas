using UnityEngine;

public class RandomRotation : MonoBehaviour
{

    void Update()
    {
        transform.rotation = Quaternion.Euler(
            transform.rotation.eulerAngles.x,
            transform.rotation.eulerAngles.y + Random.Range(-10,10),
            transform.rotation.eulerAngles.z
        );
    }
}
