using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehavior : MonoBehaviour {

	Vector3 size;
	[Range(1,4)]
	public float Speed = 1;
		
	// Use this for initialization
	void Start () {

		size = new Vector3 (0.1f, 0.1f, 0.1f);
		
	}
	
	// Update is called once per frame
	void Update () {

		gameObject.transform.localScale -= size * Speed* Time.deltaTime;
		if (gameObject.transform.localScale.x < 0.6f)
			Destroy (gameObject);

		
	}
}
