using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : MonoBehaviour
{
    private GameManager gameManager;
    private bool handlingGoal = false;

    public bool isRedGoal;

    void Start() {
        gameManager = FindObjectOfType<GameManager>();
    }

    IEnumerator WaitWhileHandlingGoal() {
        yield return new WaitForSeconds(gameManager.goalHandlingDuration);

        handlingGoal = false;
    }

    private void OnCollisionEnter(Collision collision) {
        Rigidbody rb = GetComponent<Rigidbody>();
        Rigidbody otherRb = collision.collider.GetComponent<Rigidbody>();
        if (rb == null || otherRb == null) return;

        if (collision.gameObject.CompareTag("ball") && !handlingGoal) {
            handlingGoal = true;
            bool redScored = !isRedGoal;
            gameManager.HandleGoal(redScored);

            StartCoroutine(WaitWhileHandlingGoal());
        }
    }
}
