using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateLaserController : LaserController {
	
	//public Sprite laserOnSprite;
	//public Sprite laserOffSprite;
	public float toggleInterval = 0.5f;
	public float rotationSpeed = 0.0f;
	//private bool isLaserOn = true;
	private float timeUntilNextToggle;
	//private Collider2D laserCollider;
	//private SpriteRenderer laserRenderer;


	// Use this for initialization
	void Start () {
		if (GameData.Instance.Difficulty == 2)
		{
			rotationSpeed *= 2.0f;
		}
		timeUntilNextToggle = toggleInterval;
		laserCollider = gameObject.GetComponent<Collider2D>();
		laserRenderer = gameObject.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!laserHit)
		{
			timeUntilNextToggle -= Time.deltaTime;
			if (timeUntilNextToggle <= 0)
			{
				isLaserOn = !isLaserOn;
				laserCollider.enabled = isLaserOn;
				if (isLaserOn)
				{
					laserRenderer.sprite = laserOnSprite;
				}
				else
				{
					laserRenderer.sprite = laserOffSprite;
				}

				timeUntilNextToggle = toggleInterval;
			}
		}
		transform.RotateAround(transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);
	}
}
