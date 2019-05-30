using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public GameObject MainCamera;
	public GameObject Wolfy;
	public Rigidbody2D WolfyRB;

	LayerMask Ground;
	Vector3 originalpos;
	Vector2 Tamanhodatela;
	Transform ColisorDireito;
	Transform ColisorEsquerdo;
	Vector3 Camerapos;
	int vel = 1;
	float TamanhodoColisor = 4;

	[Range(0f, 5f)]
	public float Colisorpos = 0.5f;
	[Range(0f, 15f)]
	public float AlturaCamera;
	[Range(-10f, -1f)]
	public float CameraZoom; //Regular Zoom da camera
	// Use this for initialization
	void Start () {
		Screen.orientation = ScreenOrientation.Portrait;
		MainCamera = GameObject.FindGameObjectWithTag ("MainCamera");
		Wolfy = GameObject.FindGameObjectWithTag ("Principal");
		WolfyRB = Wolfy.GetComponent<Rigidbody2D> ();
		Ground = LayerMask.GetMask ("Chao");

		#region Colisor na tela
		//Codigo para adaptar a colisão ao aspect ratio da camera.
		//Criação de objetos e adicionando eles em suas respectivas variaveis.
		ColisorDireito = new GameObject ().transform; 
		ColisorEsquerdo = new GameObject ().transform;
		//Adicionando nomes nos objetos
		ColisorDireito.name = "ColisorDireito";
		ColisorEsquerdo.name = "ColisorEsquerdo";
		//Adicionando o componente de colisão no objeto.
		ColisorDireito.gameObject.AddComponent<BoxCollider2D> ();
		ColisorEsquerdo.gameObject.AddComponent<BoxCollider2D> ();
		//Deixando os objetos como parentes do objeto que está anexado o script.
		ColisorDireito.parent = transform;
		ColisorEsquerdo.parent = transform; 
		//Pega a posição da camera principal.
		Camerapos = Camera.main.transform.position;
		print (Camerapos);
		//Atribui o vetor de tamanho da tela
		//A distancia entre o ponto 0,0 da tela a distancia da tela em si)
		//Distancia da tela na horizontal
		Tamanhodatela.x = Vector2.Distance (Camera.main.ScreenToWorldPoint (new Vector2 (0, 0)), Camera.main.ScreenToWorldPoint (new Vector2 (Screen.width, 0)));
		//Distancia da tela na vertical
		Tamanhodatela.y = Vector2.Distance (Camera.main.ScreenToWorldPoint (new Vector2 (0, 0)), Camera.main.ScreenToWorldPoint (new Vector2 (0, Screen.height)));
		print (Tamanhodatela);
		//O tamanho do colisor vai ser o tamanho do colisor já definido + o dobro do tamanho da tela em si.
		ColisorDireito.transform.localScale = new Vector3 (TamanhodoColisor, Tamanhodatela.y * 2, TamanhodoColisor);
		//A posição do colisor vai ser a posição x da camera + o tamanho da tela menos um modificador para grudar, posição do y e zoom da camera.
		ColisorDireito.transform.position = new Vector3 (Camerapos.x + Tamanhodatela.x - Colisorpos, Camerapos.y, CameraZoom);
		ColisorEsquerdo.transform.localScale = new Vector3 (TamanhodoColisor, Tamanhodatela.y * 2, TamanhodoColisor);
		ColisorEsquerdo.transform.position = new Vector3 (Camerapos.x - Tamanhodatela.x + Colisorpos, Camerapos.y, CameraZoom);
		#endregion

		originalpos = Wolfy.transform.position;
		
	}
		
	// Update is called once per frame
	void Update () {


		//CHECAR PARA VER SE FUNCIONA
		//CRIAR UMA VARIÁVEL PARA ARMAZENAR A DISTANCIA QUE PRECISA CHEGAR AO CHÃO
		//PROVAVELMENTE CRIAR UMA PROPRIEDADE PARA CONTROLAR MELHOR VEL
		//VEL SIMPLESMENTE ALTERA O VALOR DA CAMERA DE 1 PARA -1
		//COM VALOR -1 ELE VAI DEIXAR A ALTURA DA CAMERA NEGATIVA

		if (WolfyRB.velocity.y < 0) {
			RaycastHit2D CheckNearGround = Physics2D.Raycast (Wolfy.transform.position, -Vector2.up, Mathf.Infinity, Ground);
			if (CheckNearGround.distance > 2.0)
				vel = -1;
			else
				vel = 1;
		}

		else if (WolfyRB.velocity.y >= 0)
			vel = 1;	
		

		Vector3 Wolfypos = Wolfy.transform.position;
		Wolfypos.z = CameraZoom;
		Wolfypos.y = Wolfy.transform.position.y + AlturaCamera * vel;
		Wolfypos.x = originalpos.x;
		MainCamera.transform.position = Wolfypos;

		
		//A camera vai seguir o personagem.
		
	}
}
