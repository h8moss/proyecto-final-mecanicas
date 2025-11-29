using UnityEngine;

public class HabilidadFuego : MonoBehaviour
{
    public int dano = 15;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            
        }
    }

    void Update()
    {
        
    }
}
