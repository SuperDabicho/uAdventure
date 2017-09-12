using UnityEngine;
using System.Collections;

public class Solucion : MonoBehaviour {
	private int solucion;
	private bool pregunta=true;
	private bool resuelto=false;

	public void SetPregunta(bool p){
		this.pregunta = p;
	}

	public bool isPregunta(){
		return this.pregunta;
	}

	public void SetSolucion(int s){
		this.solucion = s;
	}

	public int GetSolucion(){
		return this.solucion;
	}
	
	public void SetResuelto(bool resuelto){
		this.resuelto = resuelto;
	}

	public bool IsResuelto(){
		return this.resuelto;
	}
}
