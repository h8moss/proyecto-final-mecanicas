using UnityEngine;

public class MainPlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 10f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 velocity;

    // Direcciones isométricas
    Vector3 up = new Vector3(1, 0, 1).normalized;
    Vector3 down = new Vector3(-1, 0, -1).normalized;
    Vector3 left = new Vector3(-1, 0, 1).normalized;
    Vector3 right = new Vector3(1, 0, -1).normalized;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (controller == null)
            controller = gameObject.AddComponent<CharacterController>();

        controller.height = 2f;
        controller.center = new Vector3(0, 1f, 0);
        controller.radius = 0.5f;
        controller.stepOffset = 0.1f;
        controller.slopeLimit = 45f;

        velocity = Vector3.zero;
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 move = Vector3.zero;
        if (v > 0) move += up;
        if (v < 0) move += down;
        if (h < 0) move += left;
        if (h > 0) move += right;

        if (move != Vector3.zero)
        {
            controller.Move(move.normalized * speed * Time.deltaTime);

            Vector3 lookDir = move.normalized;
            lookDir.y = 0;
            transform.forward = Vector3.Slerp(transform.forward, lookDir, rotationSpeed * Time.deltaTime);
        }

        // Gravedad
        if (controller.isGrounded)
        {
            velocity.y = -0.1f; 
        }
        else
        {
            velocity.y += gravity * Time.deltaTime; 
        }

        controller.Move(velocity * Time.deltaTime);
    }
}