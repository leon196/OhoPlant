using UnityEngine;
using System.Collections;

public class Shaders : MonoBehaviour {

	public float worldSpeed = 0.05f;
	public float minLight = 0.5f;

	private Game game;
	private Controls controls;

	// Use this for initialization
	void Start () {
		game = GetComponent<Game>();	
		controls = GetComponent<Controls>();
	}
	
	// Update is called once per frame
	void Update () {
		Shader.SetGlobalFloat("WorldTime", Time.time);
		Shader.SetGlobalFloat("WorldSpeed", worldSpeed);
		Shader.SetGlobalVector("MoonDirection", controls.GetMoonDirectionVec4());
		Shader.SetGlobalFloat("WorldLight", Mathf.Max(minLight, game.GetGlobalLight()));
	}
}
