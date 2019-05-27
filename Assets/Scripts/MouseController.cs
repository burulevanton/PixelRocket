using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using ObjectPool;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MouseController : MonoBehaviour
{

	public float jetpackForce = 30.0f;
	public float mouseMovementSpeed = 3.0f;
	
	public Transform groundCheckTransform;
	private bool isGrounded;
	public LayerMask groundCheckLayerMask;
	private Animator mouseAnimator;
	
	public ParticleSystem jetpack;
	
	private Rigidbody2D _playerRigidbody2D;
	
	private bool isDead = false;
	
	private uint coins = 0;
	
	public Text coinsCollectedLabel;
	
	public Button restartButton;
	
	public ParallaxScroll parallax;

	private bool isInvulnerability;

	public Text timeInvulnerability;
	public Image InvulnerabilityImage;

	public float invulnerabilityTime = 5.0f;

	private float currentInvulnerabilityTime = 0.0f;

	private SpriteRenderer _spriteRenderer;
	private uint renderOpacity = 10;

	private uint lives = 2;
	public Text livesText;

	private bool _jetpackActive = false;

	public Text distanceText;
	private int distanceCovered;
	private float startPoint;
	public bool speedUp;
	
	public AudioClip coinCollectSound;

	void Start()
	{
		_playerRigidbody2D = GetComponent<Rigidbody2D>();
		mouseAnimator = GetComponent<Animator>();
		_spriteRenderer = GetComponent<SpriteRenderer>();
		startPoint = transform.position.x;
		GameController.Instance.Mouse = this;
	}

	void Update()
	{
		if (GameController.Instance.IsPause || GameController.Instance.IsEnd)
			return;
		if (isInvulnerability)
		{
			_spriteRenderer.color = new Color(1f, 1f, 1f, 0.1f * renderOpacity);
			renderOpacity -= 1;
			if (renderOpacity == 0)
				renderOpacity = 10;
			if (currentInvulnerabilityTime <= 0)
			{
				isInvulnerability = false;
				currentInvulnerabilityTime = 0.0f;
				_spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
			}

			timeInvulnerability.text = Math.Ceiling(currentInvulnerabilityTime).ToString(CultureInfo.InvariantCulture);
			currentInvulnerabilityTime -= Time.deltaTime;
		}
		InvulnerabilityImage.gameObject.SetActive(isInvulnerability);
		livesText.text = lives.ToString();
		distanceCovered = (int)Math.Ceiling(transform.position.x - startPoint);
		distanceText.text = distanceCovered.ToString();
		if (distanceCovered % 50 == 0 && speedUp)
		{
			mouseMovementSpeed *= 1.1f;
			speedUp = false;
		}
		if (distanceCovered % 50 != 0 && !speedUp)
			speedUp = true;
	}
	private void FixedUpdate()
	{
		if(GameController.Instance.IsPause || GameController.Instance.IsEnd)
			return;
		if (Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Stationary))
			_jetpackActive = true;
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
			_jetpackActive = false;
		_jetpackActive = _jetpackActive && !isDead && Input.touchCount > 0;
		if (_jetpackActive)
		{
			_playerRigidbody2D.AddForce(new Vector2(0, jetpackForce));
		}
		else
		{
			_playerRigidbody2D.AddForce(new Vector2(0, 0));
		}

		if (!isDead)
		{
			Vector2 newVelocity = _playerRigidbody2D.velocity;
			newVelocity.x = mouseMovementSpeed;
			_playerRigidbody2D.velocity = newVelocity;
		}

		UpdateGroundedStatus();
		AdjustJetpack(_jetpackActive);
		if (isDead && isGrounded)
		{
			restartButton.gameObject.SetActive(true);
		}
		parallax.offset = transform.position.x;
	}
	
	void UpdateGroundedStatus()
	{
		isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, 0.1f, groundCheckLayerMask);
		mouseAnimator.SetBool("isGrounded", isGrounded);
	}
	
	void AdjustJetpack(bool jetpackActive)
	{
		var jetpackEmission = jetpack.emission;
		jetpackEmission.enabled = !isGrounded;
		if (jetpackActive)
		{
			jetpackEmission.rateOverTime = 300.0f;
		}
		else
		{
			jetpackEmission.rateOverTime = 75.0f;
		}
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.CompareTag("Coins"))
			CollectCoin(collider);
		if (collider.gameObject.CompareTag("Laser"))
			HitByLaser(collider);
		if (collider.gameObject.CompareTag("Health"))
			BecomeInvulnerability(collider);
	}

	void HitByLaser(Collider2D laserCollider)
	{
		if (!isDead)
		{
			AudioSource laserZap = laserCollider.gameObject.GetComponent<AudioSource>();
			laserZap.Play();
		}
		if (isInvulnerability)
		{
			isInvulnerability = false;
			currentInvulnerabilityTime = 0.0f;
			_spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
		}
		else
		{
			lives -= 1;
			if (lives != 0) return;
			isDead = true;
			mouseAnimator.SetBool("isDead", true);
		}
	}
	
	void CollectCoin(Collider2D coinCollider)
	{
		coins++;
		coinsCollectedLabel.text = coins.ToString();
		//Destroy(coinCollider.gameObject);
		coinCollider.gameObject.SetActive(false);
		AudioSource.PlayClipAtPoint(coinCollectSound, transform.position);
	}

	void BecomeInvulnerability(Collider2D healthCollider)
	{
		isInvulnerability = true;
		currentInvulnerabilityTime += invulnerabilityTime;
		PoolManager.ReleaseObject(healthCollider.gameObject);
	}
	public void RestartGame()
	{
		isDead = true;
		transform.position = new Vector3(startPoint, 0, transform.position.z);
		mouseAnimator.Play("fly");
		isDead = false;
	}
}
