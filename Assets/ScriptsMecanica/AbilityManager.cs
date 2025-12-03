using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public static Vector3 ClickPoint;

    [SerializeField] private AbilityBase[] abilities;
    private int selectedIndex = 0;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        DetectMouseClick();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            abilities[0].Activate();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            abilities[1].Activate();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            abilities[2].Activate();
        }
    }

    void DetectMouseClick()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 200f, LayerMask.GetMask("Ground")))
        {
            ClickPoint = hit.point;
        }
    }
}
