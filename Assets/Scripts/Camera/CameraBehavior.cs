using UnityEngine;
using System.Collections;

public class CameraBehavior : MonoBehaviour
{
    public CameraPositionSettings positionSettings = new CameraPositionSettings();
    public CameraOrbitSettings orbitSettings = new CameraOrbitSettings();

    private Vector3 previousMouseLocation = new Vector3();
    private Transform target;
    private float speed = 1.1f;


    public void Start()
    {
        target = GetComponent<Transform>();

        previousMouseLocation.x = Input.mousePosition.x;
        previousMouseLocation.y = Input.mousePosition.y;

     //   transform.localPosition = positionSettings.offsetFromTarget;
    }

    public void Update()
    {
        //transform.position = target.position;
        transform.localPosition = Quaternion.Euler(orbitSettings.rotation.x, orbitSettings.rotation.y, 0) * Vector3.Scale(Vector3.forward, positionSettings.offsetFromTarget);
        transform.rotation = Quaternion.LookRotation(target.position - (transform.position + transform.localPosition));
        if (Input.GetMouseButton(1))
        {
            orbitSettings.rotation.y += previousMouseLocation.x - Input.mousePosition.x;
            orbitSettings.rotation.x += previousMouseLocation.y - Input.mousePosition.y;
          //  transform.Rotate(yDifference, -xDifference, 0);
        }

        previousMouseLocation.x = Input.mousePosition.x;
        previousMouseLocation.y = Input.mousePosition.y;
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