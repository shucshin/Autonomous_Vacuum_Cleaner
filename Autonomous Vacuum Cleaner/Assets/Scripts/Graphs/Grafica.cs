using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grafica{

    public List<Vertice> grafica = new List<Vertice>();
	public List<Vertice> camino = new List<Vertice>();

	//Agrega un v�rtice a la lista de v�rtices de la gr�fica.
    public void AgregarVertice(Vertice nuevoVertice) {
        //Completar
		if(!grafica.Contains(nuevoVertice)) {
			grafica.Add(nuevoVertice);
		}
    }

	//Aplica el Algoritmo de A*
	public bool AStar(Vertice inicio, Vertice final) {
		//Completar
		return true;
    }

	//Auxiliar que reconstruye el camino de A*
	public void reconstruirCamino(Vertice inicio, Vertice final) {
		//Completar
	}

	float distancia(Vertice a, Vertice b) {

		float dx = a.posicion.x - b.posicion.x;
		float dy = a.posicion.y - b.posicion.y;
		float dz = a.posicion.z - b.posicion.z;

		float dis = Math.sqrt((dx ** 2) + (dy ** 2) + (dz ** 2))

		return dis;
	}

	int menorF(List<Vertice> l) {
		//Coompletar
		return 0;
	}

	//M�todo que da una representaci�n escrita de la gr�fica.
	public string toString() {
		string aux = "\nG:\n";
		foreach (Vertice v in grafica) {
			aux += v.toString() + "\n";
		}
		return aux;
	}

}
