using UnityEngine;
using System.Collections;

public class Controls : MonoBehaviour 
{
	// Sun
	private float inputSun;
	private float angleSun;

	// Moon
	private float inputMoon;
	private float angleMoon;

	// Cloud
	private float inputCloud;
	private float angleCloud;

	void Start () 
	{
		inputSun = 0f;
		angleSun = 0f;

		inputCloud = 0f;
		angleCloud = 0f;
	}
	
	void Update () 
	{
		inputSun = 1f - Input.mousePosition.x / Screen.width;
		angleSun = inputSun * Mathf.PI * 2f;

		inputMoon = Input.mousePosition.x / Screen.width;
		angleMoon = inputMoon * Mathf.PI * 2f;

		inputCloud = 1f - Input.mousePosition.y / Screen.height;
		angleCloud = inputCloud * Mathf.PI * 2f;
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
