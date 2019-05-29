using System;
using UnityEngine;

public class LaserController : MonoBehaviour
{
       protected bool isLaserOn = true;
       public Sprite laserOnSprite;
       public Sprite laserOffSprite;
       protected Collider2D laserCollider;
       protected SpriteRenderer laserRenderer;
       protected bool laserHit = false;

       private void Start()
       {
              laserCollider = gameObject.GetComponent<Collider2D>();
              laserRenderer = gameObject.GetComponent<SpriteRenderer>();
       }

       private void OnEnable()
       {
              isLaserOn = true;
              laserCollider.enabled = true;
              laserHit = false;
       }

       private void Update()
       {
              if (isLaserOn)
              {
                     laserRenderer.sprite = laserOnSprite;
              }
              else
              {
                     laserRenderer.sprite = laserOffSprite;
              }
       }

       public void Hit()
       {
              isLaserOn = false;
              laserCollider.enabled = false;
              laserHit = true;
              laserRenderer.sprite = laserOffSprite;
       }
}