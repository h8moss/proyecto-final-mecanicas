using UnityEngine;

public class MainPlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;

    Vector3 up = new Vector3(1, 0, 1).normalized;
    Vector3 down = new Vector3(-1, 0, -1).normalized;
    Vector3 left = new Vector3(-1, 0, 1).normalized;
    Vector3 right = new Vector3(1, 0, -1).normalized;

    public float speed = 4f;
    public float rotationSpeed = 7f;
    public float currentSpeed;
    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (controller == null)
            controller = gameObject.AddComponent<CharacterController>();

        animator = GetComponentInChildren<Animator>();

        if (animator == null)
            Debug.LogError("? No se encontró un Animator en los hijos del objeto PLAYER");

        controller.height = 2f;
        controller.radius = 0.4f;
        controller.center = new Vector3(0, 1f, 0);
        controller.skinWidth = 0.001f;
    }
    
    void Update()
    {
        //controller.Move(Vector3.down * 0.1f);
        controller.Move(Vector3.down * 5f * Time.deltaTime);
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 move = Vector3.zero;
        if (v > 0) move += up;
        if (v < 0) move += down;
        if (h < 0) move += left;
        if (h > 0) move += right;

        currentSpeed = move.magnitude * speed;

        if (animator != null)
            animator.SetFloat("speed", currentSpeed);

        if (move != Vector3.zero)
        {
            controller.Move(move.normalized * speed * Time.deltaTime);

            Vector3 lookDir = move.normalized;
            lookDir.y = 0;
            transform.forward = Vector3.Slerp(transform.forward, lookDir, rotationSpeed * Time.deltaTime);
        }

    }

}