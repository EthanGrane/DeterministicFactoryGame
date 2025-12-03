using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float minZoom = 5f;
    public float maxZoom = 20f;
    public float zoomSpeed = 10f;
    public float zoomSmooth = 10f;

    public Camera cam;

    private float currentZoom;
    private float targetZoom;

    private void Start()
    {
        currentZoom = cam.orthographicSize;
        targetZoom = currentZoom;
    }

    void Update()
    {
        Vector3 pos = transform.position;
        pos.z = cam.transform.position.z;
        cam.transform.position = pos;

        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");

        if (Mathf.Abs(scrollDelta) > 0.01f)
        {
            targetZoom -= scrollDelta * zoomSpeed; 
            targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
        }

        currentZoom = Mathf.Lerp(currentZoom, targetZoom, Time.deltaTime * zoomSmooth);

        cam.orthographicSize = currentZoom;
    }
}