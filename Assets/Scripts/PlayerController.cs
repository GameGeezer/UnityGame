using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;

    public void Update()
    {
        float y = Input.GetAxis("Vertical") * speed;
        float x = Input.GetAxis("Horizontal") * speed;

        GameObject camera = GameObject.FindGameObjectsWithTag("Player")[0];

        Vector2 forward = new Vector2(camera.transform.forward.x, camera.transform.forward.z);

        Vector2 right = new Vector2(camera.transform.right.x, camera.transform.right.z);
        right.Normalize();
        forward.Normalize();

        CharacterController controller = GetComponent<CharacterController>();
        if (controller.isGrounded)
        {
            moveDirection = new Vector3(forward.x * y + right.x * x, 0, forward.y * y + right.y * x);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;

        }

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }
}
