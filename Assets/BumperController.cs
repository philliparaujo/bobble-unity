using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumperController : MonoBehaviour
{
    public float forceMagnitude = 1f;
    public float xDirection = 1f;
    public float zDirection = 1f;
    private Vector3 launchDirection;

    public AudioSource audioSource;
    public AudioClip boingSound;

    void Start() {
        launchDirection = new Vector3(xDirection, 0, zDirection).normalized;
    }

    private void OnCollisionEnter(Collision collision) {
        Rigidbody rb = collision.rigidbody;
        if (rb == null) return;

        rb.AddForce(launchDirection * forceMagnitude * rb.mass, ForceMode.Impulse);

        if (audioSource != null && boingSound != null) {
            audioSource.PlayOneShot(boingSound);
        }
    }
}
