using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertice{

    public List<Vertice> vecinos = new List<Vertice>();
    public int id;
    public Vertice padre;
    public Vertice camino;
    public Vector3 posicion;
    public float f, g, h;

    public Vertice(int newId, Vector3 newPos) {
        //Completar
        this.id = newId;
        this.posicion = newPos;
    }

    public void setPadre(Vertice padre) {
        //Completar
        this.padre = padre;
    }

    public void AgregarVecino(Vertice newVertice) {
        //Completar
        if(!vecinos.Contains(newVertice)) {
            vecinos.Add(newVertice);
        } 
    }

    //M�todo que convierte a cadena el v�rtice, contiene su ID, ID de vecinos y padre.
    public string toString() {
        string aux = id.ToString()+":";
        foreach(Vertice v in vecinos) {
            aux += v.id + ",";
        }
        if (padre != null) {
            aux += "P:" + padre.id;
        }
        return aux;
    }
}
