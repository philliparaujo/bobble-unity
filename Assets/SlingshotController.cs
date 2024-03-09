using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingshotController : MonoBehaviour
{
    private Vector3 mousePosition;

    private Vector3 backwardMousePosition;
    // private Vector3 playerPosition;
    private Vector3 forwardMousePosition;
    private Vector3 direction;
    private bool isDragging = false;
    private bool readyToLaunch = false;
    public float forceMultiplier = 6.0f;

    private float minDistance = 0.75f;
    private float maxDistance = 5.0f;
    private float backwardMultiplier = 0.4f;

    private float playerYLevel = -4.25f;

    private Rigidbody rb;
    private LineRenderer lineRenderer;

    public AudioSource audioSource;
    public AudioClip smackSound;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();

        if (lineRenderer != null) {
            lineRenderer.enabled = false;
        }
    }

    void Update()
    {
        // We only care about when clicking, dragging, or if player unexpectedly moves
        if (!Input.GetMouseButtonDown(0) && !isDragging) {
            if (readyToLaunch) { 
                forwardMousePosition = rb.position - direction;
                backwardMousePosition = rb.position + direction;

                if (direction.magnitude > maxDistance) {
                    forwardMousePosition = rb.position - direction.normalized*maxDistance;
                    backwardMousePosition = rb.position + direction.normalized*maxDistance;
                }

                backwardMousePosition = Vector3.Lerp(rb.position, backwardMousePosition, backwardMultiplier);  // Scaling down
                DrawArrowhead();
            }
        }

        mousePosition = GetWorldPositionOnPlane(Input.mousePosition, playerYLevel);

        // When first clicking on the player
        if (Input.GetMouseButtonDown(0) && Vector3.Distance(mousePosition, rb.position) < minDistance) {
            isDragging = true;
            readyToLaunch = true;
            lineRenderer.enabled = true;
            lineRenderer.positionCount = 6;
        }

        // When dragging
        if (isDragging) {
            direction = mousePosition - rb.position;
            forwardMousePosition = rb.position - direction;
            backwardMousePosition = mousePosition;

            if (direction.magnitude > maxDistance) {
                forwardMousePosition = rb.position - direction.normalized*maxDistance;
                backwardMousePosition = rb.position + direction.normalized*maxDistance;
            }

            backwardMousePosition = Vector3.Lerp(rb.position, backwardMousePosition, backwardMultiplier);  // Scaling down
            DrawArrowhead();

            // When releasing
            if (Input.GetMouseButtonUp(0)) {
                isDragging = false;
            }
        }
    }

    void calculateArrowParameters() {
        
    }

    public void releaseArrow() {
        lineRenderer.enabled = false;
        if (!readyToLaunch) return;
        if (Vector3.Distance(mousePosition, rb.position) >= minDistance) {
            Vector3 forceDirection = forwardMousePosition - rb.position;
            rb.AddForce(forceDirection * forceMultiplier, ForceMode.Impulse);
        }
        readyToLaunch = false;
    }

    void DrawArrowhead() {
        float arrowheadLength = 0.5f;
        float arrowheadAngle = 55f;

        Vector3 arrowSide1 = Quaternion.Euler(0, -arrowheadAngle, 0) * -direction.normalized * arrowheadLength;
        Vector3 arrowSide2 = Quaternion.Euler(0, arrowheadAngle, 0) * -direction.normalized * arrowheadLength;

        lineRenderer.SetPosition(0, backwardMousePosition);
        lineRenderer.SetPosition(1, rb.position);
        lineRenderer.SetPosition(2, forwardMousePosition);
        lineRenderer.SetPosition(3, forwardMousePosition - arrowSide1);
        lineRenderer.SetPosition(4, forwardMousePosition);
        lineRenderer.SetPosition(5, forwardMousePosition - arrowSide2);
    }

    Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float yPosition) {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        Plane groundPlane = new Plane(Vector3.up, new Vector3(0, yPosition, 0));
        if (groundPlane.Raycast(ray, out float distance)) {
            Vector3 worldPosition = ray.GetPoint(distance);
            return worldPosition;
        }
        return Vector3.zero;
    }

    void OnCollisionEnter(Collision collision) {
        Rigidbody rb = GetComponent<Rigidbody>();
        
        if (collision.gameObject.CompareTag("wall") || collision.gameObject.CompareTag("sphere")) {
            if (rb.velocity.magnitude > 3.0f) {
                if (audioSource != null && smackSound != null) {
                    audioSource.PlayOneShot(smackSound);
                }
            }
        }
    }

    public bool isReadyToLaunch() {
        return readyToLaunch;
    }
}
