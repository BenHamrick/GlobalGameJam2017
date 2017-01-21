using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float gravityScale = 1f;
    public float ghostJumpTime = 1f;
    public float movementForce = 50f;
    public float maxVelocity = 10f;
    public float sizeScale = 2f;
    public float jumpForce = 1300f;

    bool isOnGround = true;
    float ghostJumpTimer = 0f;

    Rigidbody2D rigidBody2D;

	// Use this for initialization
	void Start () {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
        CheckGound();
    }

    void CheckGound()
    {
        Vector2 direction = Vector2.down;
        Vector2 left = Vector3.Cross(direction, Vector3.forward);
        left.Normalize();
        left *= .4f;
        direction.Normalize();
        float distance = .8f * transform.localScale.x;

        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, direction, distance);
        if (hit.collider != null && (hit.collider.gameObject.tag == "Floor" ||
            (hit.collider.gameObject.tag == "Ball" && hit.collider.gameObject != gameObject))) {
            Debug.DrawLine((Vector2)transform.position, (Vector2)transform.position + direction * distance, Color.green);
            if (!isOnGround) {
                PerlinShake.instance.PlayShake(0.2f, 10.0f, 0.1f);
                isOnGround = true;
                ghostJumpTimer = 0f;
            }
            return;
        }
        Debug.DrawLine((Vector2)transform.position, (Vector2)transform.position + direction * distance, Color.red);

        hit = Physics2D.Raycast((Vector2)transform.position + left, direction, distance);
        if (hit.collider != null && (hit.collider.gameObject.tag == "Floor" ||
            (hit.collider.gameObject.tag == "Ball" && hit.collider.gameObject != gameObject))) {
            Debug.DrawLine((Vector2)transform.position + left, (Vector2)transform.position + left + direction * distance, Color.green);
            if (!isOnGround) {
                PerlinShake.instance.PlayShake(0.2f, 10.0f, 0.1f);
                isOnGround = true;
                ghostJumpTimer = 0f;
            }
            return;
        }
        Debug.DrawLine((Vector2)transform.position + left, (Vector2)transform.position + left + direction * distance, Color.red);

        hit = Physics2D.Raycast((Vector2)transform.position - left, direction, distance);
        if (hit.collider != null && (hit.collider.gameObject.tag == "Floor" ||
            (hit.collider.gameObject.tag == "Ball" && hit.collider.gameObject != gameObject))) {
            Debug.DrawLine((Vector2)transform.position - left, (Vector2)transform.position - left + direction * distance, Color.green);
            if (!isOnGround) {
                PerlinShake.instance.PlayShake(0.2f, 10.0f, 0.1f);
                isOnGround = true;
                ghostJumpTimer = 0f;
            }
            return;
        }
        Debug.DrawLine((Vector2)transform.position - left, (Vector2)transform.position - left + direction * distance, Color.red);
        ghostJumpTimer += Time.deltaTime;
        if (ghostJumpTimer > ghostJumpTime) {
            isOnGround = false;
        }
    }
}
