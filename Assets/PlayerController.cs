using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public float moveSpeed = 100f;

    private float xInput;
    private float zInput;

    // Called when game is loading
    void Awake()
    {
        rb = GetComponent<Rigidbody>();        
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
    }

    private void FixedUpdate() {
        // Movement
        Move();
    }

    private void ProcessInputs() {
        xInput = Input.GetAxis("Horizontal");
        zInput = Input.GetAxis("Vertical");
    }

    private void Move() {
        rb.AddForce(new Vector3(xInput, 0f, zInput) * moveSpeed);
    }

}
