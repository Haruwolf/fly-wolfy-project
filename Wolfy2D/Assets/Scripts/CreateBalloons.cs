using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBalloons : MonoBehaviour
{

	//Instanciar um balão. FEITO
	//Instanciar o balão em uma posição aleatoria. FEITO
	//Instanciar vários balões em posições diferentes FEITO
	//Instanciar balões somente dentro da posição da camera FEITO
	//Instanciar os balões conforme a bola vai passando levando a camera consigo. FEITO
	//Impedir que os balões saiam da região da camera FEITO
	//Instanciar outro balão FEITO
	//Instanciar os balões aleatóriamente. FEITO

	public GameObject Inimigo;
	public GameObject[] StarPos = new GameObject[3];
	//Armazenar os objetos que vão criar a estrela.
	Vector3[] ObjectsPos = new Vector3[3];
	//Armazenar a posição que vai criar os objetos.
	public int altura;
	public GameObject Bomb;
	public GameObject PointChecker;
	public GameObject BalloonGenerator;
	public int GeneratorSeed;
	//Quantos balões são gerados de uma vez
	public float GeneratorDistance;
	//Distancia pra onde o gerador vai.
	int Seed;
	int EnemyPos;


	// Use this for initialization
	void Start ()
	{

		altura = 9;
		/*for (int i = 5; i >= 0; i--) {
			

			//Instanciar em um lugar aleatorios
			print (i);

		}*/





		
	}
	
	// Update is called once per frame
	void Update ()
	{

		//Arrumar posição do generator
		if (BalloonGenerator.transform.position.y < PointChecker.transform.position.y) {
			//Array qye armazena a posição que vai ser criada.
			//Variavel que armazena a altura q será sempre incrementada.
			ObjectsPos [0] = new Vector3 (-3, altura, 0);
			ObjectsPos [1] = new Vector3 (0, altura, 0);
			ObjectsPos [2] = new Vector3 (3, altura, 0);

			//Número aleatório para definir que estrela será criada.
			Seed = Random.Range (0, 255);
			EnemyPos = Random.Range (0, 3);

			//GenPos chama o método que é responsável por ir aumentando a distancia do balloon generator que é o que cria as estrelas.
			if (Seed < 75) { //Caso o número seja menor que 75, cria estrela1 e aumenta a altura q será criada a próxima
				Instantiate (StarPos [0], ObjectsPos [0], Quaternion.identity);
				altura += 3;
				GenPos ();
			} else if (Seed >= 75 && Seed < 150) { 
				Instantiate (StarPos [1], ObjectsPos [1], Quaternion.identity);
				altura += 3;
				GenPos (); 
			} else if (Seed >= 150 && Seed < 225) {
				Instantiate (StarPos [2], ObjectsPos [2], Quaternion.identity);
				altura += 3;
				GenPos ();
			} else { //Caso o número seja maior ou igual a 255 irá criar um inimigo, como a variável aumenta logo de cara, não tem chance de criar 
				//no mesmo lugar.
				Instantiate (Inimigo, ObjectsPos [EnemyPos], Quaternion.identity);
				altura += 3;
				GenPos ();
			}

			#region Obsoleto
			/*for (int i = GeneratorSeed; i >= 0; i--)
			{
				int BallonNumber;
				float randomposY;
				float randomposX;
				BallonNumber = Random.Range (0, 3);
				randomposY = Random.Range (-1f, 1f);
				randomposX = Random.Range (-2f, 2f);
				Instantiate (Balloes[BallonNumber], new Vector3 (BalloonGenerator.transform.position.x + 
					randomposX, BalloonGenerator.transform.position.y + randomposY, 0), Quaternion.identity);
				
				//Vai gerar os balões + uma posição aleatoria acrescentando no gerador.
			}*/
			//A posição do gerador vai ser a distancia nova do gerador, acrescida.
			#endregion


		}




		
	}

	void GenPos ()
	{
		Vector3 Balloonpos = BalloonGenerator.transform.position;
		Balloonpos.y += GeneratorDistance;
		BalloonGenerator.transform.position = Balloonpos;
	}
}
