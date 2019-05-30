using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingButtonScript : MonoBehaviour {

	//Capturar o script
	public static WingButtonScript inst;
	//Animator da asa
	public Animator Wing;
	//Rigidbody da estrela, pra fazer ela girar.
	public Rigidbody2D Starbutton;
	//Animator do botão em volta.
	public Animator CircleButton;

	//Velocidade que a estrela irá girar.
	[Range(-10f,10f)]
	public float Torque;

	void Awake()
	{
		if (inst == null)
			inst = this;
		else
			Destroy (this);
	}
	// Use this for initialization
	void Start () {
		
	}

	//Método irá ser chamado na região "Noar" do script do lobinho.
	public void Animation()
	{
		Starbutton.AddTorque (Torque * Time.deltaTime, ForceMode2D.Impulse);
			Wing.SetTrigger ("WingFlap");
	}
	
	
		
	}

