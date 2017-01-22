using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public Team team;

	public float maxHealth = 100f;
	private float health;
    public float ghostJumpTime = 1f;
    public float movementForce = 50f;
    public float maxVelocity = 10f;
    public float sizeScale = 2f;
    public float jumpForce = 1300f;
    public Animator animator;
    public Transform playerModel;

    public Slider healthSlider;

	public GameObject[] deathParticles;
	public GameObject deathParticle;
    public GameObject onPlatform;
	public bool isDead;
    public bool isOnGround = true;
    float ghostJumpTimer = 0f;
    float currentMaxVelocity = 0f;

    OneWayController _oneWayController;
    Rigidbody2D _rigidbody2D;
    public InputController inputcontroller;

    bool lockFalling;

	public float Health
	{
		get{
			return health;
		}
		set{
			health = value;
			if(health <= 0){
				health = 0;
				KillPlayer();
			}
		}
	}

    void Awake()
    {
		health = maxHealth;
        inputcontroller  = GetComponent<InputController>();

		if(team == Team.blue){
			deathParticle = deathParticles[0];
		}
		else{
			deathParticle = deathParticles[1];
		}
    }

	// Use this for initialization
	void Start () {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _oneWayController = GetComponent<OneWayController>();
        if (team == Team.red) {
            playerModel.localScale = new Vector3(-playerModel.localScale.x, playerModel.localScale.y, playerModel.localScale.z);
        }
    }

	// Update is called once per frame
	void Update () {
        CheckGound();
        animator.SetBool("OnGround", isOnGround);
        healthSlider.value = health / maxHealth;
    }

	public void KillPlayer()
	{
        if (PerlinShake.instance) {
            PerlinShake.instance.PlayShake(0.4f, 10.0f, 0.15f);
        }
        isDead = true;
		GameManager.instance.StartCoroutine(GameManager.instance.Respawn(this));
		GameObject dParticle = GameObject.Instantiate(deathParticle, transform.position, deathParticle.transform.rotation);
		dParticle.AddComponent<Explosion>();
		gameObject.SetActive(false);
	}

	public void RevivePlayer()
	{
		health = 100f;
		isDead = false;
		gameObject.SetActive(true);
		Gun gun = GetComponent<Gun>();
		gun.isFiring = false;
		gun.alreadyRefilling = false;
		gun.currentAmmoCount = gun.maxAmmoCount;
	}

    void FixedUpdate()
    {
        Vector2 newVelocity = Vector2.ClampMagnitude(new Vector2(_rigidbody2D.velocity.x, 0), currentMaxVelocity);
        _rigidbody2D.velocity = new Vector2(newVelocity.x, _rigidbody2D.velocity.y);

        if (team == Team.red) {
            animator.SetFloat("Speed", -(_rigidbody2D.velocity.x / currentMaxVelocity) * .9f);
        } else {
            animator.SetFloat("Speed", (_rigidbody2D.velocity.x / currentMaxVelocity) * .9f);
        }

        animator.SetFloat("SpeedAbs", Mathf.Abs(_rigidbody2D.velocity.x));

        if (lockFalling) {
            _rigidbody2D.velocity = new Vector2(0, 0);
        }
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
        if (hit.collider != null && (hit.collider.gameObject.tag == "Floor" || hit.collider.gameObject.tag == "OneWayPlatform" || hit.collider.gameObject.tag == "Player")) {
            Debug.DrawLine((Vector2)transform.position, (Vector2)transform.position + direction * distance, Color.green);
            if (!isOnGround) {
                isOnGround = true;
                onPlatform = hit.collider.gameObject;
                ghostJumpTimer = 0f;
            }
            return;
        }
        Debug.DrawLine((Vector2)transform.position, (Vector2)transform.position + direction * distance, Color.red);

        hit = Physics2D.Raycast((Vector2)transform.position + left, direction, distance);
        if (hit.collider != null && (hit.collider.gameObject.tag == "Floor" || hit.collider.gameObject.tag == "OneWayPlatform" || hit.collider.gameObject.tag == "Player")) {
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
        if (hit.collider != null && (hit.collider.gameObject.tag == "Floor" || hit.collider.gameObject.tag == "OneWayPlatform" || hit.collider.gameObject.tag == "Player")) {
            Debug.DrawLine((Vector2)transform.position - left, (Vector2)transform.position - left + direction * distance, Color.green);
            if (!isOnGround) {
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
        animator.SetTrigger("Jump");
    }

    public void Move(Vector2 direction)
    {
        _rigidbody2D.AddForce(new Vector2(direction.x, 0) * movementForce * Time.deltaTime * 60f);
        currentMaxVelocity = Mathf.Abs(maxVelocity * direction.x);
        if (direction.y < -.8f && onPlatform != null && onPlatform.tag == "OneWayPlatform") {
            _oneWayController.Ignorecollision(onPlatform.GetComponent<Collider2D>());
        }
    }

    public void IsFiering(bool isFier)
    {
        lockFalling = isFier;
    }
}
