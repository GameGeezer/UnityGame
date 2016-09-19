using UnityEngine;

class LookAtCamera : MonoBehaviour
{
    public Transform lookingAt;

    private Vector3 previousMouseLocation = new Vector3();
    private Transform target;
    private float speed = 1.1f;


    public void Start()
    {
        target = GetComponent<Transform>();

        previousMouseLocation.x = Input.mousePosition.x;
        previousMouseLocation.y = Input.mousePosition.y;
    }

    public void LateUpdate()
    {
        if (Input.GetMouseButton(2))
        {
            float xDifference = previousMouseLocation.x - Input.mousePosition.x;
            float yDifference = previousMouseLocation.y - Input.mousePosition.y;
            transform.Rotate(yDifference, -xDifference, 0);
        }

        float y = Input.GetAxis("Vertical") * speed;
        float x = Input.GetAxis("Horizontal") * speed;

        transform.Translate(y * Vector3.forward);
        transform.Translate(x * Vector3.right);

        previousMouseLocation.x = Input.mousePosition.x;
        previousMouseLocation.y = Input.mousePosition.y;

        transform.LookAt(lookingAt);
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360.0f)
            angle += 360.0f;

        if (angle > 360.0f)
            angle -= 360.0f;

        return Mathf.Clamp(angle, min, max);
    }
}