using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{

    [SerializeField] private float speed;
    private Vector3 velocity;

    private Rigidbody rb;
    [SerializeField] private float maxHeadUpAngle;
    [SerializeField] private float minHeadDownAngle;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        velocity = Vector3.zero;

        //velocity.y = Input.GetAxis("Vertical") * speed;
        //velocity.x = Input.GetAxis("Horizontal") * speed;

        transform.Rotate(new Vector3(Input.GetAxis("Vertical") * -1, 0f, 0f), Space.Self);
        transform.Rotate(new Vector3(0f, Input.GetAxis("Horizontal"), 0f), Space.World);


        Vector3 rotation = transform.localEulerAngles;


        if (rotation.x < 180)
            rotation.x = Mathf.Min(rotation.x, maxHeadUpAngle);
        else
            rotation.x = Mathf.Max(rotation.x, minHeadDownAngle);

        transform.eulerAngles = rotation;

        rb.velocity = velocity;
    }
}
