using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour 
{
	public float worldSpeed = 0.05f;
	public float minEnergy = 0.5f;

	public int dimension = 128;

	private Controls controls;
	private Water water;

	void Start ()
	{
		controls = GetComponent<Controls>();	
		water = GetComponent<Water>();
	}
	
	void Update () 
	{
		if (Arduino.Manager.detected) 
		{
			Arduino.Manager.Update();
		}

		water.UpdateRain();
	}

	public float GetEnergy () {
		return Mathf.Max(minEnergy, (Mathf.Cos(controls.GetSunAngle() - Mathf.PI / 2f) + 1f) * 0.5f);
	}
}
