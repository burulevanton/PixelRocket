using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using ObjectPool;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{

	public float jetpackForce = 30.0f;
	private float playerMovementSpeed;
	public float startMovementSpeed = 3.0f;
	
	public Transform groundCheckTransform;
	private bool isGrounded;
	public LayerMask groundCheckLayerMask;
	private Animator playerAnimator;
	
	
	private Rigidbody2D _playerRigidbody2D;
	
	private bool isDead = false;
	
	private uint coins = 0;

	public uint Coins => coins;

	//public Text coinsCollectedLabel;
	
	
	public ParallaxScroll parallax;

	private bool isInvulnerability;

	public Text timeInvulnerability;
	public GameObject InvulnerabilityPanel;

	public float invulnerabilityTime = 5.0f;

	private float currentInvulnerabilityTime = 0.0f;

	private SpriteRenderer _spriteRenderer;
	private uint renderOpacity = 10;

	private uint lives = 2;
	public uint Lives => lives;
	
	//public Text livesText;

	private bool _jetpackActive = false;

	//public Text distanceText;
	private int distanceCovered;

	public int DistanceCovered => distanceCovered;

	private float startPoint;
	public bool speedUp;
	
	public AudioClip coinCollectSound;
	

	void Start()
	{
		_playerRigidbody2D = GetComponent<Rigidbody2D>();
		playerAnimator = GetComponent<Animator>();
		_spriteRenderer = GetComponent<SpriteRenderer>();
		startPoint = transform.position.x;
		playerMovementSpeed = startMovementSpeed;
		GameController.Instance.Player = this;
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
		InvulnerabilityPanel.gameObject.SetActive(isInvulnerability);
		//livesText.text = lives.ToString();
		distanceCovered = (int)Math.Ceiling(transform.position.x - startPoint);
		//distanceText.text = distanceCovered.ToString();
		if (distanceCovered % 50 == 0 && speedUp)
		{
			playerMovementSpeed *= 1.1f;
			speedUp = false;
		}
		if (distanceCovered % 50 != 0 && !speedUp)
			speedUp = true;
	}
	private void FixedUpdate()
	{
		if(GameController.Instance.IsPause || GameController.Instance.IsEnd)
			return;
		if (Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Stationary) && !GameController.Instance.InMainMenu)
			_jetpackActive = true;
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended  && !GameController.Instance.InMainMenu) 
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
			newVelocity.x = playerMovementSpeed;
			_playerRigidbody2D.velocity = newVelocity;
		}

		UpdateGroundedStatus();
		parallax.offset = transform.position.x;
	}
	
	void UpdateGroundedStatus()
	{
		isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, 0.1f, groundCheckLayerMask);
		playerAnimator.SetBool("isGrounded", isGrounded);
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
		laserCollider.gameObject.GetComponent<LaserController>().Hit();
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
			playerAnimator.SetBool("isDead", true);
		}
//		laserCollider.enabled = false;
	}
	
	void CollectCoin(Collider2D coinCollider)
	{
		coins++;
		//coinsCollectedLabel.text = coins.ToString();
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
		lives = (uint) (GameData.Instance.Difficulty == 1 ? 2 : 1);
		playerMovementSpeed = startMovementSpeed;
		coins = 0;
		transform.position = new Vector3(startPoint, -3.835001f, transform.position.z);
		playerAnimator.enabled = true;
		playerAnimator.SetBool("isDead", false);
		playerAnimator.SetTrigger("dieOnce");
		isDead = false;
		playerAnimator.Play("run");
		//coinsCollectedLabel.text = coins.ToString();
	}

	public void Respawn()
	{
		lives = 1;
		playerAnimator.enabled = true;
		playerAnimator.SetBool("isDead", false);
		playerAnimator.SetTrigger("dieOnce");
		isDead = false;
		playerAnimator.Play("run");
	}

	public void OnPlayerDeath()
	{
		playerAnimator.enabled = false;
		GameController.Instance.EndGame();
	}
}
