using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateStar : MonoBehaviour {
	int RandomNumber;
	public GameObject Star;
	GameObject CloneStar;
	// Use this for initialization
	void Start () {


		//Caso o objeto não tenha uma estrela, irá instanciar somente uma vez.
		if (CloneStar == null)
				CloneStar = Instantiate (Star, transform.position, Quaternion.identity);
		
	}
	
	// Update is called once per frame
	void Update () {


		
	}
}
