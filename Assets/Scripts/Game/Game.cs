using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour 
{
	public int dimension = 128;
	public int plantDepth = 4;
	public float worldSpeed = 0.05f;
	public float minEnergy = 0.1f;
	
	private float timeStart = 0f;
	private float worldTime = 0f;

	private Controls controls;
	private Planter planter;
	private Water water;

	void Start ()
	{
		controls = GetComponent<Controls>();
		planter = GetComponent<Planter>();
		water = GetComponent<Water>();

		timeStart = Time.time;
	}
	
	void Update () 
	{
		if (Arduino.Manager.detected) 
		{
			Arduino.Manager.Update();

			// 3rd Switch
			if (Arduino.Manager.SwitchSwitched(3))
			{
				// Reset
				planter.Reset();
			}
		}

		// World Time [0..1]
		worldTime = ((Time.time - timeStart) * worldSpeed) % 1f;

		// Spring
		if (worldTime < 0.25f)
		{

		}
		// Summer
		else if (worldTime < 0.5f)
		{

		}
		// Autumn
		else if (worldTime < 0.75f)
		{

		}
		// Winter
		else {

		}

		water.UpdateRain();
	}

	public float GetEnergy () {
		return Mathf.Max(minEnergy, (Mathf.Cos(controls.GetSunAngle() - Mathf.PI / 2f) + 1f) * 0.5f);
	}
}
