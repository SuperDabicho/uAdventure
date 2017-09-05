using UnityEngine;
using System.Collections;

public class TodoPoderoso : MonoBehaviour {

	private GameObject fondo;

	// Use this for initialization
	void Start () {
		//fondo = this.gameObject;
	}

	void OnMouseDown(){
		Cursor.visible = false;
		transform.GetComponent<TextMesh> ().fontStyle = FontStyle.Bold;
	}
	void OnMouseUp(){
		Cursor.visible = true;
		transform.GetComponent<TextMesh> ().fontStyle = FontStyle.Normal;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDrag(){
		Vector3 mouseposition = new Vector3 (Input.mousePosition.x, Input.mousePosition.y);
		Vector3 objPosition = Camera.main.ScreenToWorldPoint (mouseposition);
		objPosition.z = 0;

		transform.position = objPosition;
//		if (transform.GetComponent<TextMesh>().color.Equals(new Color(0, 255, 0))) {
//			float x = transform.position.x-20;
//			float y = transform.position.y;
//			transform.position = new Vector3 (x, y, 0);
//		}
	}
	//fondo.GetComponent<Renderer>().material.color = new Color(255, 0, 0);

//	void OnTriggerEnter2D(Collider2D other){
//		if (comprobar (other)) {
//			transform.GetComponent<TextMesh>().color = Color.green;
//		} else {
//			transform.GetComponent<TextMesh>().color = Color.red;
//		}
//	}

	void OnTriggerStay2D(Collider2D other){ 
		if (other.GetComponent<Solucion> ().isPregunta()){
			if (Cursor.visible) {	//Input.GetMouseButtonUp(0) 0-left 1-right 2-middle
				if (comprobar (other)) {
					other.transform.GetComponent<TextMesh> ().color = Color.green;
					Vector3 newPosition = new Vector3 (10, other.transform.position.y - 4, 0);
					transform.position = newPosition;	
					Destroy (transform.GetComponent<BoxCollider2D> ());//Evita que reaccione la pregunta con otra respuesta si ya esta solucionada.
					Destroy (other.transform.GetComponent<BoxCollider2D> ());
				} else {
					other.transform.GetComponent<TextMesh> ().color = Color.red;
				}
			}
		}
	}

	void OnTriggerExit2D(Collider2D other){
		transform.GetComponent<TextMesh> ().color = Color.white;
		other.transform.GetComponent<TextMesh> ().color = Color.white;
	}

	bool comprobar(Collider2D other){
		int correcta= other.GetComponent<Solucion>().GetSolucion();
		int prueba = transform.GetComponent<Solucion>().GetSolucion();
		Debug.Log (correcta + " " + prueba);
		return (correcta.Equals(prueba));
	}
}
