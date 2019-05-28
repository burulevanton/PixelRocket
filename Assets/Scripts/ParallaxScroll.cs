using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScroll : MonoBehaviour {
	
	
	public Renderer background;
	public Renderer foreground1;
	public Renderer foreground2;
	public Renderer loon;
	public float backgroundSpeed = 0.02f;
	public float foregroundSpeed1 = 0.06f;
	public float foregroundSpeed2 = 0.06f;
	public float loonSpeed = 0.01f;
	public float offset = 0.0f;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float backgroundOffset = offset * backgroundSpeed;
		float foregroundOffset1 = offset * foregroundSpeed1;
		float foregroundOffset2 = offset * foregroundSpeed2;
		float loonOffset = offset * loonSpeed;
		

		background.material.mainTextureOffset = new Vector2(backgroundOffset, 0);
		foreground1.material.mainTextureOffset = new Vector2(foregroundOffset1, 0);
		foreground2.material.mainTextureOffset = new Vector2(foregroundOffset2, 0);
		loon.material.mainTextureOffset = new Vector2(loonOffset, 0);

	}
}
