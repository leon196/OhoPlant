using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour 
{
	public float worldSpeed = 0.05f;
	public float minEnergy = 0.5f;

	private Controls controls;

	void Start ()
	{
		controls = GetComponent<Controls>();	
		
	}
	
	// Update is called once per frame
	void Update () {
	}

	public float GetGlobalLight () {
		return Mathf.Max(minEnergy, (Mathf.Cos(controls.GetSunAngle() - Mathf.PI / 2f) + 1f) * 0.5f);
	}
}
