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


	private float astresHeight = 2f;
	private float angleOffset = Mathf.PI / -10f;

	//
	private Game game;
	//private Shaders shaders;
	private GUIText text;
	private Planter planter;

	void Start () 
	{
		game = GetComponent<Game>();
		//shaders = GetComponent<Shaders>();
		text = GetComponent<GUIText>();
		planter = GetComponent<Planter>();
	}

	void Update () 
	{
		if (Arduino.Manager.Enable) 
		{
			inputSun = 1f - Arduino.Manager.Spiner(3);
			inputMoon = 1f - Arduino.Manager.Spiner(1);
			inputCloud = 1f - Arduino.Manager.Spiner(2);

			// World Speed with Slider 1
			game.worldSpeed = 0.5f + Arduino.Manager.Slider(1) * 4f;

			// Lens Scope with Slider 2
			renderer.material.SetFloat("_DetailsDirection", Arduino.Manager.Slider(2));

			// Toggle Infos
			if (Arduino.Manager.SwitchSwitched(1))
			{
				text.enabled = Arduino.Manager.Switch(1);
			}

			// Toggle Auto Restart
			if (Arduino.Manager.SwitchSwitched(2))
			{
				planter.autoRestart = Arduino.Manager.Switch(2);
			}

		} else {
			float ratio = Input.mousePosition.x / Screen.width;
			inputSun = 1f - ratio;
			inputMoon = 1f - ratio;
			inputCloud = 1f - ratio;
		}

		angleSun = inputSun * Mathf.PI;
		angleMoon = inputMoon * Mathf.PI;
		angleCloud = inputCloud * Mathf.PI;
	}

	public float GetSunRatio () {
		return inputSun;
	}

	public float GetSunAngle () {
		return angleSun;
	}

	// Directions

	public Vector3 GetSunDirection () {
		return new Vector3(Mathf.Cos(angleSun), Mathf.Sin(angleSun) / astresHeight, 0f);
	}

	public Vector3 GetMoonDirection () {
		return new Vector3(Mathf.Cos(angleMoon), Mathf.Sin(angleMoon) / astresHeight, 0f);
	}

	public Vector3 GetCloudDirection () {
		return new Vector3(Mathf.Cos(angleCloud), Mathf.Sin(angleCloud) / astresHeight, 1f);
	}

	// Shaders

	public Vector4 GetSunDirectionVec4 () {
		return new Vector4(Mathf.Cos(angleSun), Mathf.Sin(angleSun) / astresHeight, 0f, 0f);
	}

	public Vector4 GetMoonDirectionVec4 () {
		return new Vector4(Mathf.Cos(angleMoon), Mathf.Sin(angleMoon) / astresHeight, 0f, 0f);
	}

	public Vector4 GetCloudDirectionVec4() {
		return new Vector4(Mathf.Cos(angleCloud), Mathf.Sin(angleCloud) / astresHeight, 0f, 0f);
	}
}
