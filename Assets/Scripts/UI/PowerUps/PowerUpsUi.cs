using UnityEngine;
using UnityEngine.UI;
public class PowerUpsUi : MonoBehaviour
{
    public AyudaConLaUIParaEdu referenciaAbilities;
    public Image[] imagenesSlots;

    void Update()
    {
        if (referenciaAbilities == null || referenciaAbilities.nombresAbilities == null)
            return;

        string[] nombres = referenciaAbilities.nombresAbilities;

 
        int max = Mathf.Min(imagenesSlots.Length, nombres.Length);

        for (int i = 0; i < max; i++)
        {
            if (nombres[i] != "vacío")
            {
                imagenesSlots[i].gameObject.SetActive(true);
            }
            else
            {
                imagenesSlots[i].gameObject.SetActive(false);
            }
        }
    }
}
