using UnityEngine;

public class MecanicaAbsorber : MonoBehaviour
{
    public string[] slots = new string[3];
    public int slotDispo = 4;
    public bool almaExists = false;

    private Alma alma;

    public void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            for(int i = 0; i < 3; i++)
            {
                Debug.Log(slots[i]);
            }
            
        }
        if (almaExists)
        {
            if (Input.GetKeyDown("e"))
            {
                HabilidadLlenar(alma.tipoDeAlma);
            }
            else if (Input.GetKeyDown("h"))
            {
                Debug.Log("curando");
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Alma"))
        {
            alma = other.GetComponent<Alma>();
            almaExists = true;
            alma.absorbieron += Absorber;
        }
    }

    private void Absorber(Alma alma)
    {
        almaExists = true;
    }

    private void HabilidadLlenar(int tipo)
    {
        switch (tipo)
            {
            case 1:
                slotDispo = SlotDisponible();
                if(slotDispo != 4)
                {
                    slots[slotDispo] = "rayo";
                    almaExists = false;
                }
                break;
            case 2:
                slotDispo = SlotDisponible();
                if (slotDispo != 4)
                {
                    slots[slotDispo] = "fuego";
                    almaExists = false;
                }
                break;
            case 3:
                slotDispo = SlotDisponible();
                if (slotDispo != 4)
                {
                    slots[slotDispo] = "tierra";
                    almaExists = false;
                }
                break;
            case 4:
                slotDispo = SlotDisponible();
                if (slotDispo != 4)
                {
                    slots[slotDispo] = "aire";
                    almaExists = false;
                }
                break;
            case 5:
                slotDispo = SlotDisponible();
                if (slotDispo != 4)
                {
                    slots[slotDispo] = "hielo";
                    almaExists = false;
                }
                break;
        }
    }

    private int SlotDisponible()
    {
        for (int i = 0; i < 3; i++)
        {
            if (slots[i] == "")
            {
                return i;
            }
        }
        return 4;
    }
}
