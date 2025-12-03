using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    public static Vector3 ClickPoint;
    public static Vector3 HoverPoint;
    public MecanicaAbsorber absorbido;

    [SerializeField] private AbilityBase[] abilities;

    private Camera cam;
    private Transform player;

    private bool isPreviewing = false;
    private int previewIndex = -1;

    void Start()
    {
        absorbido = GetComponentInParent<MecanicaAbsorber>();
        cam = Camera.main;

        player = transform;
    }

    void Update()
    {
        abilities = absorbido.slots;

        DetectMouseHover();
        DetectMouseClick();

        if (!isPreviewing)
        {
            if (Input.GetKeyDown(KeyCode.Q)) StartPreview(0);
            if (Input.GetKeyDown(KeyCode.E)) StartPreview(1);
            if (Input.GetKeyDown(KeyCode.R)) StartPreview(2);
        }
        else
        {
            UpdatePreviewPosition();

            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetMouseButtonDown(0))
            {
                CastAbility();
            }
        }
    }

    void StartPreview(int index)
    {
        if (abilities[index] == null) return;

        isPreviewing = true;
        previewIndex = index;

        abilities[index].ShowPreview();
    }

    void UpdatePreviewPosition()
    {
        abilities[previewIndex].ShowPreview();
    }

    void CastAbility()
    {
        abilities[previewIndex].HidePreview();
        abilities[previewIndex].Activate();

        absorbido.slots[previewIndex] = null;

        isPreviewing = false;
        previewIndex = -1;
    }

    void DetectMouseHover()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 200f))
        {
            HoverPoint = new Vector3(hit.point.x, player.position.y, hit.point.z);
        }
    }

    void DetectMouseClick()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        ClickPoint = HoverPoint; // << ya no raycasteamos otra vez
    }
}
