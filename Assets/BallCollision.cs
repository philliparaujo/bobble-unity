using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCollision : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip collisionSound;

    void OnCollisionEnter(Collision collision) {
        Rigidbody rb = GetComponent<Rigidbody>();
        Rigidbody otherRb = collision.collider.GetComponent<Rigidbody>();
        if (rb == null || otherRb == null) return;
        if (audioSource == null || collisionSound == null) return;

        if (rb.velocity.magnitude > 5.0f || otherRb.velocity.magnitude > 5.0f) {
            audioSource.PlayOneShot(collisionSound);
        }
    }
}
