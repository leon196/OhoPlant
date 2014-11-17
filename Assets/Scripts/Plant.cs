using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Plant : MonoBehaviour {

	//private LineRenderer lineRenderer;
	private Texture2D plantMap;
	private int dimension = 128;
	private Color[] clear;
	private List<Vector3> linePositions;
	private float energy;
	private float resource;
	private float angle;
	private Vector3 direction;
	private Vector3 torsade;
	private Vector3 lastPosition;
	private float lastTime;
	private GameObject leaf;

	// Use this for initialization
	void Start () {
		//lineRenderer = gameObject.GetComponent<LineRenderer>();	
		plantMap = new Texture2D(dimension, dimension, TextureFormat.ARGB32, false);
		clear = new Color[dimension*dimension];
		for (int i = 0; i < dimension*dimension; ++i) clear[i] = Color.clear;
		plantMap.SetPixels(0, 0, dimension, dimension, clear);
		plantMap.Apply();
		gameObject.renderer.material.mainTexture = plantMap;

		leaf = GameObject.Instantiate(Manager.Instance.Leaf, Vector3.zero, Quaternion.identity) as GameObject;

		angle = 0f;
		energy = 0f;
		resource = 5f;
		direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
		direction.Normalize();
		torsade = new Vector3();
		lastPosition = new Vector3(0f, 0f, -2f);
		linePositions = new List<Vector3>();
		linePositions.Add(lastPosition);
		linePositions.Add(lastPosition);
		//lineRenderer.SetVertexCount(linePositions.Count);
		lastTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (resource > 0f) {
			int index = linePositions.Count-1;
			Vector3 position = linePositions[index];

			energy = Manager.Instance.Game.GetGlobalLight();
			float angleTorsade = Mathf.PI * Mathf.Cos(Time.time) * 0.2f;
			torsade.x = Mathf.Cos(angleTorsade);
			torsade.y = Mathf.Sin(angleTorsade);

			direction = Manager.Instance.Controls.GetSunDirection();
			//angle += Time.deltaTime * Mathf.Max(-0.1f, Mathf.Min(0.1f, (Mathf.Atan2(direction.y, direction.x) - angle))) * 4f;
			//direction.x = Mathf.Cos(angle);
			//direction.y = Mathf.Sin(angle);

			//Vector3.Lerp(direction, Manager.Instance.Controls.GetSunDirection(), 0.01f);// + torsade;
			//direction.Normalize();
			float speed = 10f;
			position = position + direction * Time.deltaTime * energy * speed;
			int x = (int)Mathf.Max(0f, Mathf.Min(dimension, position.x + dimension / 2f));
			int y = (int)Mathf.Max(0f, Mathf.Min(dimension, position.y + dimension / 2f));
			plantMap.SetPixel(x, y, Color.blue);

			//if (y > dimension - Screen.height)

			leaf.transform.position = (position / dimension) * 10f - Vector3.forward;
			linePositions[index] = position;
			//lineRenderer.SetPosition(index, linePositions[index]);

			if (Vector3.Distance(lastPosition, position) > 0.2f) {// || lastTime + 0.5f < Time.time) {
				linePositions.Add(position);
				//lineRenderer.SetVertexCount(linePositions.Count);
				//lineRenderer.SetPosition(linePositions.Count-1, position);
				lastPosition = position;
				lastTime = Time.time;
			}

			plantMap.Apply();

			resource -= Time.deltaTime * energy;
		}
	}
}
