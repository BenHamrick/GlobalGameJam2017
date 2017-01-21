using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float ghostJumpTime = 1f;
    public float movementForce = 50f;
    public float maxVelocity = 10f;
    public float sizeScale = 2f;
    public float jumpForce = 1300f;

    public GameObject onPlatform;
    public bool isOnGround = true;
    float ghostJumpTimer = 0f;
    float currentMaxVelocity = 0f;

    OneWayController _oneWayController;
    Rigidbody2D _rigidbody2D;

	// Use this for initialization
	void Start () {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _oneWayController = GetComponent<OneWayController>();
    }
	
	// Update is called once per frame
	void Update () {
        CheckGound();
    }

    void FixedUpdate()
    {
        Vector2 newVelocity = Vector2.ClampMagnitude(new Vector2(_rigidbody2D.velocity.x, 0), currentMaxVelocity);
        _rigidbody2D.velocity = new Vector2(newVelocity.x, _rigidbody2D.velocity.y);
    }

    void CheckGound()
    {
        Vector2 direction = Vector2.down;
        Vector2 left = Vector3.Cross(direction, Vector3.forward);
        left.Normalize();
        left *= .4f;
        direction.Normalize();
        float distance = 2f * transform.localScale.x;

        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, direction, distance);
        if (hit.collider != null && (hit.collider.gameObject.tag == "Floor" || hit.collider.gameObject.tag == "OneWayPlatform")) {
            Debug.DrawLine((Vector2)transform.position, (Vector2)transform.position + direction * distance, Color.green);
            if (!isOnGround) {
                if (PerlinShake.instance) {
                    PerlinShake.instance.PlayShake(0.2f, 10.0f, 0.1f);
                }
                isOnGround = true;
                onPlatform = hit.collider.gameObject;
                ghostJumpTimer = 0f;
            }
            return;
        }
        Debug.DrawLine((Vector2)transform.position, (Vector2)transform.position + direction * distance, Color.red);

        hit = Physics2D.Raycast((Vector2)transform.position + left, direction, distance);
        if (hit.collider != null && (hit.collider.gameObject.tag == "Floor" || hit.collider.gameObject.tag == "OneWayPlatform")) {
            Debug.DrawLine((Vector2)transform.position + left, (Vector2)transform.position + left + direction * distance, Color.green);
            if (!isOnGround) {
                if (PerlinShake.instance) {
                    PerlinShake.instance.PlayShake(0.2f, 10.0f, 0.1f);
                }
                isOnGround = true;
                onPlatform = hit.collider.gameObject;
                ghostJumpTimer = 0f;
            }
            return;
        }
        Debug.DrawLine((Vector2)transform.position + left, (Vector2)transform.position + left + direction * distance, Color.red);

        hit = Physics2D.Raycast((Vector2)transform.position - left, direction, distance);
        if (hit.collider != null && (hit.collider.gameObject.tag == "Floor" || hit.collider.gameObject.tag == "OneWayPlatform")) {
            Debug.DrawLine((Vector2)transform.position - left, (Vector2)transform.position - left + direction * distance, Color.green);
            if (!isOnGround) {
                if (PerlinShake.instance) {
                    PerlinShake.instance.PlayShake(0.2f, 10.0f, 0.1f);
                }
                isOnGround = true;
                onPlatform = hit.collider.gameObject;
                ghostJumpTimer = 0f;
            }
            return;
        }
        Debug.DrawLine((Vector2)transform.position - left, (Vector2)transform.position - left + direction * distance, Color.red);
        ghostJumpTimer += Time.deltaTime;
        if (ghostJumpTimer > ghostJumpTime) {
            isOnGround = false;
            onPlatform = null;
        }
    }

    public void Stop()
    {
        if (isOnGround)
            _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);
    }

    public void Jump()
    {
        if (!isOnGround)
            return;
        ghostJumpTimer = ghostJumpTime;
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0.0f);
        _rigidbody2D.AddForce(Vector2.up * jumpForce);
    }

    public void Move(Vector2 direction)
    {
        _rigidbody2D.AddForce(new Vector2(direction.x, 0) * movementForce * Time.deltaTime * 60f);
        currentMaxVelocity = Mathf.Abs(maxVelocity * direction.x);
        if (direction.y < -.8f && onPlatform != null && onPlatform.tag == "OneWayPlatform") {
            _oneWayController.Ignorecollision(onPlatform.GetComponent<Collider2D>());
        }
    }
}
