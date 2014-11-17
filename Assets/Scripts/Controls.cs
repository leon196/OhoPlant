using UnityEngine;
using System.Collections;

public class Controls : MonoBehaviour {

	private GameObject sun;
	private GameObject moon;
	private GameObject cloud;

	// Sun & Moon
	private float inputSunMoon;
	private float angleSunMoon;
	private float radiusSunMoon;

	// Cloud
	private float inputCloud;
	private float angleCloud;
	private float radiusCloud;

	// Use this for initialization
	void Start () {
		sun = Manager.Instance.Sun;
		moon = Manager.Instance.Moon;
		cloud = Manager.Instance.Cloud;
	}
	
	// Update is called once per frame
	void Update () {
		inputSunMoon = 1f - Input.mousePosition.x / Screen.width;
		angleSunMoon = inputSunMoon * Mathf.PI * 2f;
		radiusSunMoon = 5f;

		sun.transform.localPosition = GetSunDirection() * radiusSunMoon;
		moon.transform.localPosition = GetMoonDirection() * radiusSunMoon;

		inputCloud = 1f - Input.mousePosition.y / Screen.height;
		angleCloud = inputCloud * Mathf.PI * 2f;
		radiusCloud = 3f;

		cloud.transform.localPosition = GetCloudDirection() * radiusCloud;
	}

	public float GetInput1Ratio () {
		return inputSunMoon;
	}

	public float GetSunAngle () {
		return angleSunMoon;
	}

	public Vector3 GetSunDirection () {
		return new Vector3(Mathf.Cos(angleSunMoon), Mathf.Sin(angleSunMoon), 0f);
	}

	public Vector3 GetMoonDirection () {
		return new Vector3(Mathf.Cos(angleSunMoon + Mathf.PI), Mathf.Sin(angleSunMoon + Mathf.PI), 0f);
	}

	// Shaders
	public Vector4 GetMoonDirectionVec4 () {
		return new Vector4(Mathf.Cos(angleSunMoon + Mathf.PI), Mathf.Sin(angleSunMoon + Mathf.PI), 0f, 0f);
	}

	// CLOUD
	public Vector3 GetCloudDirection () {
		return new Vector3(Mathf.Cos(angleCloud), Mathf.Sin(angleCloud), 1f);
	}
	public Vector3 GetCloudPosition () {
		return new Vector3(Mathf.Cos(angleCloud), Mathf.Sin(angleCloud), 1f) * radiusCloud;
	}
	public float GetCloudRadius () {
		return cloud.renderer.material.GetFloat("_Radius") * cloud.transform.localScale.x;
	}
}
