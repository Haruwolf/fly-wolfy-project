using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBalloons : MonoBehaviour {

	public static int BalloonCount;

	void OnTriggerEnter2D(Collider2D col)
	{
		//caso encoste em algo q está com a tag balão
		if (col.gameObject.CompareTag ("Balloon")) {
			Destroy (col.gameObject);
			BalloonCount++;
			print (BalloonCount);
		}

		//caso encoste em algo que esteja com a tag bomb.
		if (col.gameObject.CompareTag ("Bomb")) {
			Wolfy.inst.EstadoAtual = Wolfy.WolfyEstados.FimJogo;
		}
	}
}
