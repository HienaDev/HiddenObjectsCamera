using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{

    [SerializeField] private float speed;
    private Vector3 velocity;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        velocity = Vector3.zero;

        velocity.y = Input.GetAxis("Vertical") * speed;
        //velocity.x = Input.GetAxis("Horizontal") * speed;

        transform.Rotate(new Vector3(0f, Input.GetAxis("Horizontal"), 0f));

        rb.velocity = velocity;
    }
}
