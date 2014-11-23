using UnityEngine;
using System.Collections;

public class Shaders : MonoBehaviour 
{
	//
	public static float worldSpeed = 0.05f;
	public float minLight = 0.5f;

	//
	private float cloudRadius = 0f;
	public float CloudRadius { get { return cloudRadius; } }
	private float cloudDistance = 0f;
	public float CloudDistance { get { return cloudDistance; } }

	//
	private Game game;
	private Controls controls;

	// Use this for initialization
	void Start () {
		game = GetComponent<Game>();	
		controls = GetComponent<Controls>();

		Manager.Instance.SetSizeBounds(10f);

		cloudRadius = Manager.Instance.Environment.renderer.material.GetFloat("_CloudRadius");
		cloudDistance = Manager.Instance.Environment.renderer.material.GetFloat("_CloudDistance");
	}
	
	// Update is called once per frame
	void Update () {
		Shader.SetGlobalFloat("WorldTime", Time.time);
		Shader.SetGlobalFloat("WorldSpeed", worldSpeed);
		Shader.SetGlobalFloat("WorldLight", Mathf.Max(minLight, game.GetGlobalLight()));

		Shader.SetGlobalVector("SunDirection", controls.GetSunDirectionVec4());
		Shader.SetGlobalVector("MoonDirection", controls.GetMoonDirectionVec4());
		Shader.SetGlobalVector("CloudDirection", controls.GetCloudDirectionVec4());
	}
}
