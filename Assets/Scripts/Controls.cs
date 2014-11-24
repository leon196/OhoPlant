using UnityEngine;
using System.Collections;

public class Controls : MonoBehaviour 
{
	// 
	private float inputSun = 0f;
	private float inputMoon = 0f;
	private float inputCloud = 0f;

	// 
	private float angleSun = 0f;
	private float angleMoon = 0f;
	private float angleCloud = 0f;

	private float angleOffset = 0f;//Mathf.PI / -2f;

	//
	private Game game;
	//private Shaders shaders;

	void Start () 
	{
		game = GetComponent<Game>();
		//shaders = GetComponent<Shaders>();
	}
	
	void Update () 
	{
		if (Arduino.Manager.Enable) 
		{
			inputSun = 1f - Arduino.Manager.Spiner(3);
			inputMoon = 1f - Arduino.Manager.Spiner(1);
			inputCloud = 1f - Arduino.Manager.Spiner(2);

			game.worldSpeed = 0.1f + Arduino.Manager.SliderWithDetails(1, 8);
			renderer.material.SetFloat("_Details", 4f + (1f - Arduino.Manager.Slider(2)) * 4f);

		} else {
			inputSun = 1f - Input.mousePosition.x / Screen.width;
			inputMoon = 1f - Input.mousePosition.x / Screen.width;
			inputCloud = 1f - Input.mousePosition.y / Screen.height;
		}

		angleSun = inputSun * Mathf.PI + angleOffset;
		angleMoon = inputMoon * Mathf.PI + angleOffset;
		angleCloud = inputCloud * Mathf.PI + angleOffset;
	}

	public float GetSunRatio () {
		return inputSun;
	}

	public float GetSunAngle () {
		return angleSun;
	}

	// Directions

	public Vector3 GetSunDirection () {
		return new Vector3(Mathf.Cos(angleSun), Mathf.Sin(angleSun), 0f);
	}

	public Vector3 GetMoonDirection () {
		return new Vector3(Mathf.Cos(angleMoon), Mathf.Sin(angleMoon), 0f);
	}

	public Vector3 GetCloudDirection () {
		return new Vector3(Mathf.Cos(angleCloud), Mathf.Sin(angleCloud), 1f);
	}

	// Shaders

	public Vector4 GetSunDirectionVec4 () {
		return new Vector4(Mathf.Cos(angleSun), Mathf.Sin(angleSun), 0f, 0f);
	}

	public Vector4 GetMoonDirectionVec4 () {
		return new Vector4(Mathf.Cos(angleMoon), Mathf.Sin(angleMoon), 0f, 0f);
	}

	public Vector4 GetCloudDirectionVec4() {
		return new Vector4(Mathf.Cos(angleCloud), Mathf.Sin(angleCloud), 0f, 0f);
	}
}
