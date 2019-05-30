using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateScenarios : MonoBehaviour {
	public GameObject pointChecker;
	public GameObject ScenarioGenerator;
	public GameObject Cenario8;

	[Range(3f,6f)]
	public float CreateDistance;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (ScenarioGenerator.transform.position.y < pointChecker.transform.position.y) {

			Instantiate (Cenario8, ScenarioGenerator.transform.position, Quaternion.identity);

			Vector3 Generatorpos = ScenarioGenerator.transform.position;
			Generatorpos.y += CreateDistance;
			ScenarioGenerator.transform.position = Generatorpos;
		}
		
	}
}
