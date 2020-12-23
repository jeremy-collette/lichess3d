using UnityEngine;

public class TargetCamera : MonoBehaviour
{
    public GameObject target;

    private float distanceToTarget = 15.0f;

    private Vector3 previousPosition;

    void Update()
    {
        distanceToTarget -= Input.mouseScrollDelta.y;
        Camera.main.transform.position = target.transform.position;

        // When we initially right click
        if (Input.GetMouseButtonDown(1))
        {
            previousPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }
        // Holding down right click
        else if (Input.GetMouseButton(1))
        {
            var newPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            var direction = previousPosition - newPosition;

            var yAxisRotation = -direction.x * 180.0f;
            Camera.main.transform.Rotate(new Vector3(0, 1, 0), yAxisRotation, Space.World);
            var xAxisRotation = direction.y * 180.0f;
            Camera.main.transform.Rotate(new Vector3(1, 0, 0), xAxisRotation);

            previousPosition = newPosition;
        }

        Camera.main.transform.Translate(new Vector3(0, 0, -distanceToTarget));
    }
}