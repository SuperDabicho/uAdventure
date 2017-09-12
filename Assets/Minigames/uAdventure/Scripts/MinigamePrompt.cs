using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using uAdventure.Runner;
using System;
using uAdventure.Core;
using System.Threading;


namespace uAdventure.Minigame
{
	public class MinigamePrompt : MonoBehaviour, CustomEffectRunner
	{
		public Effect Effect {
			get { return minigameEffect; }
			set { minigameEffect = value as MinigameEffect; }
		}
		public MinigameEffect minigameEffect { get; set; }
		public string idMinigame;
		private int correctas=0;
		GameObject [] preg;
		GameObject [] resp;
		public Minigame mg;
		GameObject victoria;
		GameObject fondo;
		GameObject doc;
		private bool force_wait = true;

		void Start()
		{
			fondo=Resources.Load<GameObject>("fondo");
			GameObject.Instantiate (fondo);
			mg = Game.Instance.GameState.FindElement<Minigame> (idMinigame);
			bool[] sols = new bool[mg.Respuestas.Length];
			for (int a = 0; a < sols.Length; a++) {
				sols [a] = false;
			}
			doc = new GameObject ();
			doc.AddComponent<TextMesh> ();
			doc.GetComponent<TextMesh> ().text = mg.Documentation;
			doc.GetComponent<TextMesh> ().fontSize = 30;
			doc.GetComponent<TextMesh> ().color = Color.cyan;
			doc.transform.position = new Vector3 (10,55, 0);
			preg=new GameObject[mg.Preguntas.Length];
			resp=new GameObject[mg.Respuestas.Length];
			//spawn object
			for(int i=0;i<mg.Preguntas.Length;i++){
				preg [i] = new GameObject("Pregunta"+i.ToString());
				preg [i].AddComponent<TextMesh> ();
				preg [i].GetComponent<TextMesh> ().text = mg.Preguntas [i];
				preg [i].GetComponent<TextMesh> ().fontSize = 30;
				preg [i].AddComponent<BoxCollider2D>();
				preg [i].GetComponent<BoxCollider2D> ().isTrigger = true;
				preg [i].transform.position = new Vector3 (5,(60/(mg.Preguntas.Length+1)*i)+10, 0);
				preg [i].AddComponent<Solucion> ();
				preg [i].GetComponent<Solucion> ().SetSolucion (i);

			}


			for (int i = 0; i < mg.Respuestas.Length; i++) {
				bool respondida = true;
				int k = 0;
				while (respondida) {
					k = UnityEngine.Random.Range (0, mg.Respuestas.Length);
					respondida = sols [k];
				}
				sols [k] = true;
				resp [i] = new GameObject ("Respuesta" + i.ToString ());
				resp [i].AddComponent<TextMesh> ();
				resp [i].GetComponent<TextMesh> ().text = mg.Respuestas [k];
				resp [i].GetComponent<TextMesh> ().fontSize = 30;
				resp [i].transform.position = new Vector3 (70, (60/(mg.Preguntas.Length+1)*i)+10, 0);
				resp [i].AddComponent<Rigidbody2D> ();
				resp [i].GetComponent<Rigidbody2D> ().gravityScale = 0;
				resp [i].AddComponent<BoxCollider2D> ();
				resp [i].GetComponent<BoxCollider2D> ().isTrigger = true;
				resp [i].AddComponent<Solucion> ();
				resp [i].GetComponent<Solucion> ().SetPregunta (false);
				resp [i].GetComponent<Solucion> ().SetSolucion (k);
				resp [i].AddComponent<TodoPoderoso> ();
			}

			

			//Crea el panel de victoria
			victoria = new GameObject("PanelVictoria");
			victoria.AddComponent<TextMesh> ();
			victoria.GetComponent<TextMesh>().text = "VICTORIA";
			victoria.GetComponent<TextMesh> ().fontSize = 80;
			victoria.transform.position = new Vector3 (25,35,0);
			victoria.SetActive (false);

		}

		bool finished = false;

		void Update () {
			for(int j=0; j<mg.Respuestas.Length; j++){
				if (preg [j].GetComponent<Solucion> ().IsResuelto ()) {
					correctas += 1;
				}
			}
			if (correctas == mg.Respuestas.Length) {
				victoria.SetActive (true);

				finished=true;
			} else
				correctas = 0;
	
			if (finished){
				terminar ();
			}
		}

		private void terminar(){
			for (int i = 0; i < mg.Respuestas.Length; i++) {
				DestroyImmediate (preg [i].gameObject);
				DestroyImmediate(resp[i].gameObject);
			}
			DestroyImmediate (doc.gameObject);
			//DestroyImmediate (fondo.gameObject);
			//DestroyImmediate (victoria.gameObject);
			DestroyImmediate(this.gameObject);
		}
			
		private EffectHolder effectHolder;

		public void OnClosePrompt()
		{
			if(effectHolder == null)
			{
				force_wait = false;
				finished = true; 
			}
		}

		public bool execute()
		{
			if (effectHolder != null)
			{
				force_wait = effectHolder.execute();
				if (!force_wait)
					finished = true;
				//DestroyImmediate(this.gameObject);
			}

			return force_wait;
		}





	
	
	}
}