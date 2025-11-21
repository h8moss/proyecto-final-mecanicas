using UnityEngine;

public class MainPlayerMovement : MonoBehaviour
{
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (controller == null)
            controller = gameObject.AddComponent<CharacterController>();

        // Ajustes recomendados
        controller.height = 2f;
        controller.radius = 0.4f;
        controller.center = new Vector3(0, 1f, 0);
        controller.skinWidth = 0.05f;
    }

    void Update()
    {
        // Empuja ligeramente hacia abajo para hacer contacto con el piso
        controller.Move(Vector3.down * 0.1f);
    }
}