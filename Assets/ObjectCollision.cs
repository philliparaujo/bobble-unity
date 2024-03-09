using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision) {
        Rigidbody rb = GetComponent<Rigidbody>();
        Rigidbody otherRb = collision.collider.GetComponent<Rigidbody>();

        if (rb == null || otherRb == null) return;

        if (collision.gameObject.CompareTag("sphere")) {
            Vector3 newVelocity = CalculateElasticCollisionVelocity(rb, otherRb);
            rb.velocity = newVelocity;

            Vector3 newAngularVelocity = CalculateElasticCollisionAngularVelocity(rb, otherRb);
            rb.angularVelocity = newAngularVelocity;

            Debug.Log("velocity is " + newVelocity + ", angular is " + newAngularVelocity);
        }
    }

    private Vector3 CalculateElasticCollisionVelocity(Rigidbody rb1, Rigidbody rb2) {
        float mass1 = rb1.mass;
        float mass2 = rb2.mass;
        Vector3 velocity1 = rb1.velocity;
        Vector3 velocity2 = rb2.velocity;

        Vector3 initialMomentum = rb1.mass * rb1.velocity + rb2.mass * rb2.velocity;
        Vector3 newVelocity;

        if (rb1.mass >= rb2.mass) {
            newVelocity = Vector3.zero;
        } else {
            newVelocity = initialMomentum / rb1.mass;
        }

        return newVelocity;
    }

    private Vector3 CalculateElasticCollisionAngularVelocity(Rigidbody rb1, Rigidbody rb2) {
        float mass1 = rb1.mass;
        float mass2 = rb2.mass;
        Vector3 velocity1 = rb1.velocity;
        Vector3 velocity2 = rb2.velocity;

        float radius1 = rb1.GetComponent<SphereCollider>().radius * transform.localScale.x;
        float radius2 = rb2.GetComponent<SphereCollider>().radius * transform.localScale.x;
        float momentOfInertia1 = (2f / 5f) * rb1.mass * Mathf.Pow(radius1, 2);
        float momentOfInertia2 = (2f / 5f) * rb2.mass * Mathf.Pow(radius2, 2);

        Vector3 initialAngularMomentum = momentOfInertia1 * rb1.angularVelocity + momentOfInertia2 * rb2.angularVelocity;
        Vector3 newAngularVelocity;

        if (rb1.mass >= rb2.mass) {
            newAngularVelocity = Vector3.zero;
        } else {
            newAngularVelocity = initialAngularMomentum / rb1.mass;
        }

        return newAngularVelocity;
    }
}
