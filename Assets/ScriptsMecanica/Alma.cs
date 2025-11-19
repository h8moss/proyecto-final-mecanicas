using System;
using UnityEngine;

public class Alma : MonoBehaviour
{
    public int tipoDeAlma = 1;
    public delegate void Absorbido(Alma alma);
    public Absorbido absorbieron;

    public void Absorber()
    {
        absorbieron?.Invoke(this);
        //Destroy(gameObject);
    }
}
