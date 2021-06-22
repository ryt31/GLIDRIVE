using UnityEngine;

public class Pointer : MonoBehaviour
{
    public float defaultLength = 100.0f;

    [SerializeField] private GameObject dot;

    [SerializeField] private VRInputModule inputModule;

    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        UpdateLine();
    }

    private void UpdateLine()
    {
        var data = inputModule.GetData();
        var targetLength = data.pointerCurrentRaycast.distance == 0
            ? defaultLength
            : data.pointerCurrentRaycast.distance;

        var hit = createRayCast(targetLength);

        var endPosition = transform.position + transform.forward * targetLength;

        if (hit.collider != null)
            endPosition = hit.point;

        dot.transform.position = endPosition;

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endPosition);
    }

    private RaycastHit createRayCast(float length)
    {
        RaycastHit hit;
        var ray = new Ray(transform.position, transform.forward);
        Physics.Raycast(ray, out hit, defaultLength);
        return hit;
    }
}