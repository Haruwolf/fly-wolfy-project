
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Wolfy : MonoBehaviour
{


	#region Atributos do Personagem


	/*[Range (0, 10)]
	public float AirControl;*/
	//Variável antiga, usada quando a movimentação do lobinho era por tilt.
	Rigidbody2D WolfyRB;
	//Pegar o Rigidbody do personagem.
	Transform WolfyCharacter;
	//Pegar o atributo Transform do personagem.
	BoxCollider2D WolfyCollisor;
	//Pegar o Colisor do objeto.
	LaunchWolfy WolfL;
	Controles WolfyControl;
	//Objetos da classe para serem chamados.
	public static float altura;
	//altura que o personagem alcança
	public static Wolfy inst = null;
	//instancia para usar as variaveis da classe
	public Animator WolfyAnim;
	//Animator do personagem.


	#endregion

	#region Estados do personagem

	public enum WolfyEstados
	{
		InicioJogo,
		Preparacao,
		Lancamento,
		NoAr,
		FimJogo,
		GanhouJogo}

	;

	public WolfyEstados EstadoAtual;

	//Estados do personagem e variável para armazenar eles.

	#endregion

	#region Variáveis ligadas ao tempo de lançamento.

	float Timetoplay;
	//Segundos de preparação para o jogo começar.
	float Seconds;
	//Numeros de segundos a serem dimínuidos.
	public static int Secondstoplay;
	//Segundos a serem diminuidos.
	public static float TimeLimit;
	//LimitedeTempo

	//Váriaveis estáticas não reiniciam junto com a fase, setar elas manualmente (no caso, essas já estão.


	#endregion

	#region Variaveis ligadas a força de lançamento.

	//Objeto a ser criado na tela, que é o botão para lançar o lobinho.
	public GameObject WingButton;


	//Obsoleto
	/*
	//Varíaveis para armazenar os botões.
	GameObject buttononscreen;
	GameObject otherbuttononscreen;
	GameObject thirdbuttononscreen;
	GameObject fourthbuttononscreen;
	//Variáveis para armazenar os colisores
	CircleCollider2D buttoncollider;
	CircleCollider2D otherbuttoncollider;
	CircleCollider2D thirdbuttoncollider;
	CircleCollider2D fourthbuttoncollider;
	*/
	float PoderdeForca;
	//Força que vai ser lançado.
	[Range (0, 10)]
	public float IncrementodeForca;
	//Força que é incrementado.

	#endregion

	#region Variáveis ligadas ao controle no ar
	//Direção que o lobinho irá ir.
	float direction = 0;
	//Velocidade do lobinho no ar.
	[Range(1,6)]
	public float AirSpeed;
	//Instância para armazenar os botões direcionais.
	public GameObject DirButtons;
	//Máscara da camada dos botões direcionais.
	LayerMask DirectionalButton;
	//Animator das asas.
	public Animator RightWing;
	public Animator LeftWing;
	#endregion

	#region Bools de auxilio de checagem de estado.

	bool colisaocomchao;


	#endregion

	#region Para checagem com o chão.

	//Armazenar as camadas.
	LayerMask Button;
	LayerMask Chao;
	//Posição do personagem e colisão do proprio
	Vector2 ballpos;
	float raiocolisao;
	Vector2 boxsize;

	#endregion

	#region >>Inicialização do jogo.<<

	//Para capturar o script.
	void Awake ()
	{
		if (inst == null)
			inst = this;
		else
			Destroy (this);
	}



	// Use this for initialization
	void Start ()
	{
		EstadoAtual = WolfyEstados.InicioJogo; //O jogo começa no estado "Inicio Jogo".
		#region Inicialização dos atributos
		WolfyCollisor = GetComponent <BoxCollider2D> ();
		WolfyCharacter = GetComponent<Transform> ();
		WolfyRB = GetComponent<Rigidbody2D> (); //Pegar o Rigidbody do personagem
		WolfL = new LaunchWolfy (); //declaração de objeto.
		WolfyControl = new Controles (); //declaração do objeto.
		WingButton.SetActive(false);

		#endregion
		#region Inicialização das variáveis de tempo
		Seconds = 1; //Segundos a serem descontados.
		Timetoplay = 3; //Tempo para o jogo iniciar.

		#endregion
		#region Inicialização de checagem com o chão.
		DirectionalButton = LayerMask.GetMask("DirectionalButton");
		Chao = LayerMask.GetMask ("Chao");
		Button = LayerMask.GetMask ("Button");
		//raiocolisao = WolfyCollisor.ra;
		boxsize = WolfyCollisor.size;

		#endregion

		
	}

	#endregion

	#region Mecânica do jogo

	void Update ()
	{
		



		#region Bools de controle da animação
		//bools de controle da animação, nelas estão atribuidas em qual modo o jogo está.
		bool InicioJogo = EstadoAtual == WolfyEstados.InicioJogo;
		bool Preparacao = EstadoAtual == WolfyEstados.Preparacao;
		bool Lancamento = EstadoAtual == WolfyEstados.Lancamento;
		bool Noar = EstadoAtual == WolfyEstados.NoAr;

		#endregion

		#region Animações
		//Com isso, se o personagem saiu de tal modo, automaticamente aquela bool vira false.
		WolfyAnim.SetBool ("Idle", InicioJogo);
		WolfyAnim.SetBool ("Preparing", Preparacao);
		WolfyAnim.SetBool ("Flying", Noar);

		#endregion

		#region Pegar posição do personagem e se ele está no chão
		ballpos = new Vector2 (WolfyCharacter.transform.position.x, WolfyCharacter.transform.position.y);
		//colisaocomchao = Physics2D.OverlapCircle (ballpos, raiocolisao, Chao);
		colisaocomchao = Physics2D.OverlapBox (ballpos, boxsize, 0, Chao);
		//Raycast funciona assim:
		//ELe cria um raio e armazena na variavel o que o raio atingiu.

		#endregion

		#region Controle de botões que aparecem na tela.
		WingButton.SetActive (EstadoAtual == WolfyEstados.Preparacao);
		DirButtons.SetActive (EstadoAtual == WolfyEstados.NoAr);
		#endregion

		#region >Troca de estados<		
		switch (EstadoAtual) {

		#region Inicio do Jogo
		//Estado do inicio do jogo
		case WolfyEstados.InicioJogo:

			//Existe um tempo que o jogador não pode fazer os comandos


			//Timetoplay -= 1f * Time.deltaTime; //Diminui o tempo
			Timetoplay = Mathf.Clamp (Timetoplay - 1f * Time.deltaTime, 0, 3); //Contagem para o jogo começar


			if (Timetoplay <= 0) {
				Timetoplay = 3; //reset do tempo que o jogo inicia.
				PoderdeForca = 0; //força volta a 0 após o tempo acabar.
				TimeLimit = 6f; //o limite de tempo para o próximo estado é estabelecido
				EstadoAtual = WolfyEstados.Preparacao; //entra no próximo estado

			}
			break;

			#endregion
			
		#region Preparacao
		case WolfyEstados.Preparacao: //Preparação do pulo

			//Obsoleto
			/*
			#region Para criar os botões na tela.


			#region button1
			if (buttononscreen == null) { //Caso a variável que armazena botões esteja nula.
				//A variável vai armazenar a instancia do objeto na posição do lobinho.
				float RX = Random.Range (-2, 3);
				float RY = Random.Range (-1, 3);
				buttononscreen = Instantiate (button, new Vector3 (RX, RY, gameObject.transform.position.z), Quaternion.identity);
				//E a variável do colisor vai armazenar esse novo colisor.
				buttoncollider = buttononscreen.GetComponent<CircleCollider2D> ();
			}
			#endregion

			#region button2
			if (otherbuttononscreen == null) { //Caso a variável que armazena botões esteja nula, sempre que não existir nada dentro dela, é criado.
				//A variável vai armazenar a instancia do objeto na posição do lobinho.
				float RX = Random.Range (-2, 3);
				float RY = Random.Range (-1, 3);
				otherbuttononscreen = Instantiate (button, new Vector3 (RX, RY, gameObject.transform.position.z), Quaternion.identity);
				//E a variável do colisor vai armazenar esse novo colisor.
				otherbuttoncollider = otherbuttononscreen.GetComponent<CircleCollider2D> ();
			}
			#endregion

			#region button3
			if (thirdbuttononscreen == null) { //Caso a variável que armazena botões esteja nula.
				//A variável vai armazenar a instancia do objeto na posição do lobinho.
				float RX = Random.Range (-2, 3);
				float RY = Random.Range (-1, 3);
				thirdbuttononscreen = Instantiate (button, new Vector3 (RX, RY, gameObject.transform.position.z), Quaternion.identity);
				//E a variável do colisor vai armazenar esse novo colisor.
				thirdbuttoncollider = thirdbuttononscreen.GetComponent<CircleCollider2D> ();
			}
			#endregion

			#region button4
			if (fourthbuttononscreen == null) { //Caso a variável que armazena botões esteja nula.
				//A variável vai armazenar a instancia do objeto na posição do lobinho.
				float RX = Random.Range (-2, 3);
				float RY = Random.Range (-1, 3);
				fourthbuttononscreen = Instantiate (button, new Vector3 (RX, RY, gameObject.transform.position.z), Quaternion.identity);
				//E a variável do colisor vai armazenar esse novo colisor.
				fourthbuttoncollider = fourthbuttononscreen.GetComponent<CircleCollider2D> ();
			}
			#endregion
			#endregion
			*/

			#region Evento de toque
			//Caso exista algum toque na tela.
			if (Input.touchCount > 0) {
				Touch Toque; //Armazenar o tipo de toque.
				Toque = Input.GetTouch (0); //Qual toque na tela (No caso o primeiro)


				//Envia um raio infinito quando o toque existe atráves da camera do jogo.
				RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position), -Vector2.up, Mathf.Infinity, Button);

				//Caso tenha colidido com algo.
				if (hit.collider != null) {
					
					Debug.Log (hit.collider);
					PoderdeForca += IncrementodeForca; //Aumenta sua força pra jogar. //A força aumenta.
					WingButtonScript.inst.Animation();
					//Destroy (hit.collider.gameObject);
					//Destruir o gameobject que colidiu.
				}
					

			}
			#endregion

			#region Tempo para mudança de estado.
			//Limite de tempo para pressionar os botões.
			TimeLimit = Mathf.Clamp (TimeLimit - 1f * Time.deltaTime, 0, 6);
			Secondstoplay = Mathf.RoundToInt (TimeLimit); //Converter para mostrar somente o número inteiro

			if (TimeLimit <= 0 && PoderdeForca > 0) //Caso o limite de tempo fique abaixo de 0, ele vai para o próximo estado.
				EstadoAtual = WolfyEstados.Lancamento;
			else if (PoderdeForca <= 0 && TimeLimit <= 0) {
				EstadoAtual = WolfyEstados.InicioJogo;
			}
			#endregion
				
			break;
			#endregion

		#region Lancamento
		case WolfyEstados.Lancamento: //Momento de lançamento

			WolfyAnim.SetTrigger ("Jumping"); //Chamar a animação de pulo que vai desencadear outras animações.
			WolfL.Launch (WolfyRB, PoderdeForca); //Chamar a classe pra ser lançada.
			//WolfyAnim.SetTrigger ("ToFly"); //Chamar a animação de voo.
			EstadoAtual = WolfyEstados.NoAr; //Ir para o estado em que o personagem está no ar.

			break;
			#endregion

		#region Noar
		case WolfyEstados.NoAr: //Noar
			

			//print ("Is On Air"); //Só um print pra checar mesmo.
			WolfyControl.ControlarWolfy (WolfyCharacter, WolfyRB, DirectionalButton, direction, AirSpeed, RightWing, LeftWing); //Método para controlar o wolfy no ar.
			Altura = transform.position.y; //A variavel da altura vai pegar o Y do lobinho.
			//print (Input.acceleration);
			/*if (colisaocomchao) { //Se ele está no estado do ar, e chegou a ter colisão no chão, significa que precisa voltar pra preparação.
				//Não foi colocado o reset aqui, por precaução. Para que ele não seja lançado e automaticamente perceba que já estava no chão.
				EstadoAtual = WolfyEstados.Preparacao;
			}*/

			if(WolfyRB.velocity.y < 0) //Casp p érspmage, esteja caindo.
			{
				RaycastHit2D tohit = Physics2D.Raycast(ballpos, -Vector2.up, Mathf.Infinity, Chao);
				//Variavel para armazenar o que atingiu/Enviar o raio na posição do personagem, pra baixo, eternamente, e só vai colidir com o chão)
				Debug.Log(tohit.distance);
				//Caso esteja a uma distancia de menos 1.8f e colida com o chão.
				if (tohit.distance < 1.8f && colisaocomchao){
					
					EstadoAtual = WolfyEstados.GanhouJogo;
					
					Debug.Log("Win Game!");
				}
			}


			break;
			#endregion

		#region FimJogo
		case WolfyEstados.FimJogo: //Estado para o jogador perder.
			WolfyAnim.SetTrigger ("Lose"); //Em caso de ter encostado em um inimigo
			//Ele trava no céu.
			Vector2 WolfyVel = WolfyRB.velocity;
			WolfyVel.y = 0;
			WolfyVel.x = 0;
			WolfyRB.velocity = WolfyVel; //A velocidade de seu rigidbody é retirada.

			if (Input.GetKeyDown(KeyCode.S))
			{
				Debug.Log("Reset");
				SceneManager.LoadScene("Wolfy alpha");
				altura = 0;
				CollisionBalloons.BalloonCount = 0;
			}

			break;
			#endregion

		#region GanhouJogo
			//Checar se ganhou o jogo.
		case WolfyEstados.GanhouJogo:
			WolfyAnim.SetTrigger("Win"); //Animação de vitória
			//Reiniciar o jogo
			if (Input.GetKeyDown(KeyCode.S))
			{

				SceneManager.LoadScene("Wolfy alpha");
				altura = 0;
				CollisionBalloons.BalloonCount = 0;
			}
			break;
			#endregion

		default:
			break;

		}
		#endregion


	}

	#region Checagem de altura máxima.

	public static float Altura {
		get{ return altura; }
		set {
			if (value > altura)
				altura = value;
		}
	}

	#endregion
	

}
#endregion


#region Para lançar o lobinho pro alto.
class LaunchWolfy : Wolfy
{
	public void Launch (Rigidbody2D Wolf, float forca) //Método para lançar.
	{
		Wolf.AddForce (new Vector2 (0, forca), ForceMode2D.Impulse); //Adicionar força pra lançar.
		print ("Lançado");
	}
}
#endregion

#region Controlar o lobinho no céu.
class Controles : Wolfy
{
	public void ControlarWolfy (Transform wlf, Rigidbody2D wlfrb, LayerMask dc, float direction, float ac, Animator rw, Animator lw)
	{
		

		#region Toque na tela e checagem se pressionou o botão
		if (Input.touchCount > 0) {
			Touch toquenoar;
			toquenoar = Input.GetTouch (0);

			RaycastHit2D touchhit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.GetTouch(0).position), -Vector3.forward, Mathf.Infinity, dc);
			//Debug.Log (touchhit.point);

			if (touchhit.collider != null) 
			{
				if (touchhit.collider.tag == "LeftWing") {
					lw.SetTrigger ("Pressed");
					direction = -1;
					Debug.Log ("esquerda");
				}
				if (touchhit.collider.tag == "RightWing") {
					rw.SetTrigger ("Pressed");
					direction = 1;
					Debug.Log ("direita");
				}
			}


		}
		#endregion


		#region (Obsoleto) Movimentação antiga 
		/*Vector2 movement = wlfrb.velocity; //Armazena a velocidade do rigidbody
		//int tilt = direction; //Armazena a informação do acelerometro no momento
		//movement.x = (tilt * 32) * Time.deltaTime * 4; //A velocidade da variavel vai aumentar lentamente com o acréscimo do acelerometro
		movement.x += direction * Time.deltaTime;
		wlfrb.velocity = movement; //a velocidade do rigidbody vai ser a variavel incrementada.
		//print (wlfrb.velocity.x);*/
		#endregion


		#region Responsável por alterar a movimentação do personagem
		Vector3 WolfyAirPos = wlf.transform.position;
		WolfyAirPos.x += direction * ac * Time.fixedDeltaTime;
		wlf.transform.position = WolfyAirPos;
		#endregion



	}
}
#endregion

