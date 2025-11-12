using UnityEngine;

public class MecanicaAbsorber : MonoBehaviour
{
    public int[] slots = new int[3];
    

    Alma alma;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Alma"))
        {
            alma = other.GetComponent<Alma>();
            alma.absorbieron += Absorber;
        }
    }

    public void OnDisable()
    {
        alma.absorbieron -= Absorber;
    }

    private void Absorber(Alma alma)
    {
        HabilidadLlenar(alma.tipoDeAlma);

        switch(alma.tipoDeAlma)
        {
            case 1:
                break;
        }
    }

    private void HabilidadLlenar(int tipo)
    {
        Debug.Log("Soy fuego");
    }
}
