using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float maxHeadUpAngle;
    [SerializeField] private float minHeadDownAngle;

    private Vector3 velocity;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        velocity = Vector3.zero;

        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        float rotationX = verticalInput * speed * Time.deltaTime * -1;
        float rotationY = horizontalInput * speed * Time.deltaTime;

        transform.Rotate(new Vector3(rotationX, 0f, 0f), Space.Self);
        transform.Rotate(new Vector3(0f, rotationY, 0f), Space.World);

        Vector3 rotation = transform.localEulerAngles;

        if (rotation.x < 180)
            rotation.x = Mathf.Min(rotation.x, maxHeadUpAngle);
        else
            rotation.x = Mathf.Max(rotation.x, minHeadDownAngle);

        transform.eulerAngles = rotation;

        rb.velocity = velocity;
    }
}