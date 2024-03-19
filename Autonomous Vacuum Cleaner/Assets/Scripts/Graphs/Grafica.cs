using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grafica{

    public List<Vertice> grafica = new List<Vertice>();
	public List<Vertice> camino = new List<Vertice>();

	//Agrega un v�rtice a la lista de v�rtices de la gr�fica.
    public void AgregarVertice(Vertice nuevoVertice) {
        //Completar
		if(!grafica.Contains(nuevoVertice)) grafica.Add(nuevoVertice);
    }

	//Aplica el Algoritmo de A*
	public bool AStar(Vertice inicio, Vertice final) {
		//Completar

		if (inicio == null || final == null){
			return false;
		}

		List<Vertice> abierto = new List<Vertice>();
		List<Vertice> cerrado = new List<Vertice>();

		inicio.g = 0;
		inicio.h = distancia(inicio,final);
		inicio.f = inicio.h;

		abierto.Add(inicio);

		while (abierto.Count > 0) {
			int i = menorF(abierto);
			Vertice actual = abierto[i];
			 if(actual.id == final.id) {
				reconstruirCamino(inicio,final);
				return true;
			 }
			 abierto.RemoveAt(i);
			 cerrado.Add(actual);

			 foreach(Vertice v in actual.vecinos){
				if(-1 < closed.IndexOf(v)) continue;
				
				if(open.IndexOf(v) == -1){
					v.camino = actual;
					v.g = actual.g + 1;
					v.h = distancia(actual,final);
					v.f = v.g + v.h;
				}
			 }
		}

		return true;
    }

	//Auxiliar que reconstruye el camino de A*
	public void reconstruirCamino(Vertice inicio, Vertice final) {

		string aux = "";
		camino.Clear();
		camino.Add(final);

		var rc = final.camino;
		while (rc.id != inicio.id) {
			camino.Insert(0, rc);
			rc = rc.camino;
		}
		camino.Insert(0, inicio);

		foreach(Vertice v in camino) {
			aux += v.id.ToString() + "";
		}


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
		float menorf = 0;
		int indice = 0;
		int count = 0;

		menorf = l[0].f;
		for(int i = 0; i < l.Count; i++){
			if(l[i].f <= menorf){
				menorf = l[i].f;
				indice = count;
		    }
		count++;
		}
		return indice;
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
