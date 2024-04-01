using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComportamientoAutomatico : MonoBehaviour {

    //Enum para los estados
    public enum State {
        DFS,
        DFS_MAP,
        MAPEO,
        Carga,
        AStarCarga,
        AStarBack
    }

    private State currentState;
    private Sensores sensor;
	private Actuadores actuador;
	private Mapa mapa;
    public Vertice verticeActual, verticeDestino, verticeCamino, Estacion;
    public Vector3 destino;
    public bool fp = true, look = false, pila = true, path = false;
    public string estado;
    public float bateria, bateriaMax;
    public int sig;

    public Queue<Vertice> bfsQueue = new Queue<Vertice>();

    void Start(){
        SetState(State.DFS_MAP);
        sensor = GetComponent<Sensores>();
		actuador = GetComponent<Actuadores>();
        mapa = GetComponent<Mapa>();
        mapa.ColocarNodo(0);
        mapa.popStack(out verticeActual);
        verticeDestino = verticeActual;
        Estacion = verticeActual; // maybe
        bateriaMax = sensor.Bateria();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Basura") {
            Destroy(other.gameObject); // When tagged, it disappears
        }
    }

    private void Update() {
        bateria = sensor.Bateria();
        if(bateria <= bateriaMax*0.35) {
            pila = false;
        }
    } 

    void FixedUpdate() {
        if(bateria <= 0) return;
        switch (currentState) {
            case State.DFS:
            estado = "DFS";
            UpdateDFS();
            break;
            case State.DFS_MAP:
            estado = "DFS_MAP";
            UpdateDFS_MAP();
            break;
            case State.MAPEO:
            estado = "MAPEO";
            UpdateMAPEO();
            break;
            case State.Carga:
            estado = "Carga";
            UpdateCarga();
            break; 
            case State.AStarCarga:
            estado = "AstarCarga";
            UpdateAStarCarga();
            break; 
            case State.AStarBack:
            estado = "AstarBack";
            UpdateAStarBack();
            break; 
        }
    }

    void UpdateMAPEO() {
        if(!sensor.FrenteLibre()) {
            actuador.Detener();
            mapa.ColocarNodo(0);
            actuador.GirarIzquierda();
        } else {
            actuador.Adelante();
        }
    }

    // Funciones de actualizacion especificas para cada estado
    /**
     * PASOS PARA EL DFS
     * 1.- Colocar un vértice (meterlo a la pila 'ColocarNodo' ya lo mete a la pila
     * 2.- Sacar de la pila, e intentar poner mas vértices
     * 3.- Hacer backtrack al siguiente vértice en la pila
     * 4.- Repetir hasta vaciar la pila
     */
    void UpdateDFS() {
        if(mapa.isEmptyStack()) {
            while(bfsQueue.Count != 0) {
                mapa.dfsStack.Push(bfsQueue.Dequeue());
            }
        }

        if (fp) {
            if(!mapa.popStack(out verticeActual)) {
                SetState(State.AStarBack);
                return;
            }
            destino = verticeActual.posicion;
            // mapa.popStack(out verticeActual);
            mapa.setPreV(verticeActual);   //Asignar a mapa el vértice nuevo al que nos vamos a mover, para crear las adyacencias necesarias.
            bfsQueue.Enqueue(verticeDestino);
            fp = false;
        }

        if(verticeActual.padre.id == verticeDestino.id) {
            if(!look) {
                transform.LookAt(verticeActual.posicion);
                look = true;
            } 
            if(Vector3.Distance(sensor.Ubicacion(), verticeActual.posicion) >= 0.04f) {
                actuador.Adelante();
            } else {
                verticeDestino = verticeActual;
                look = false;
                fp = true;
                SetState(State.DFS_MAP);
            }
        } else { // Back-tracking
            if(Vector3.Distance(sensor.Ubicacion(), verticeDestino.padre.posicion) >= 0.04f) {
                if(!look) {
                    transform.LookAt(verticeDestino.padre.posicion);
                    look = true;
                } actuador.Adelante();
            } else {
                verticeDestino = verticeDestino.padre;
                look = false;
            }
        } 
    } 

    
    void UpdateCarga() {
        if(bateria < bateriaMax) {
            if(verticeCamino == Estacion) {
                actuador.Detener();
                actuador.CargarBateria();
                return;
            } 
            SetState(State.AStarCarga);
        } else {
            pila = true;
            SetState(State.AStarBack);
        }
    } 

    void UpdateAStarCarga() {
        if(!path) {
            mapa.mapa.AStar(verticeDestino, Estacion);
            path = true;
            sig = 0;
        } else {
            if(sig < mapa.mapa.camino.Count) {
                verticeCamino = mapa.mapa.camino[sig];
                if(Vector3.Distance(sensor.Ubicacion(), verticeCamino.posicion) >= 0.04f) {
                    if(!look) {
                        transform.LookAt(verticeCamino.posicion);
                        look = true;
                    }
                    actuador.Adelante();
                } else {
                    sig++;
                    look = false;
                }
            } else {
                actuador.Detener();
                path = false;
                mapa.clearPath();
                SetState(State.Carga);
            }
        } 
    } 

    void UpdateAStarBack() {
        if(!path) {
            mapa.mapa.AStar(Estacion, verticeActual);
            path = true;
            sig = 0;
            //sig = mapa.mapa.camino.Count-1; // here
        } else {
            // if(sig >= 0) {
            if(sig < mapa.mapa.camino.Count) {
                verticeCamino = mapa.mapa.camino[sig];
                if(Vector3.Distance(sensor.Ubicacion(), verticeCamino.posicion) >= 0.04f) {
                    if(!look) {
                        transform.LookAt(verticeCamino.posicion);
                        look = true;
                    }
                    actuador.Adelante();
                } else {
                    sig++; // here
                    look = false;
                }
            } else {
                actuador.Detener();
                path = false;
                mapa.clearPath();
                SetState(State.DFS_MAP);
            }
        } 
    } 


    // Funciones de actualizacion especificas para cada estado
    void UpdateDFS_MAP() {
        if (!sensor.FrenteLibre()) {
            actuador.Detener();
        }
        if (sensor.IzquierdaLibre()) {
            mapa.ColocarNodo(1);
        }
        if (sensor.DerechaLibre()) {
            mapa.ColocarNodo(3);
        }
        if (sensor.FrenteLibre()) {
            mapa.ColocarNodo(2);
        }
        if(!pila) {
            SetState(State.AStarCarga);
        } else {
            SetState(State.DFS);
        }
    }

    // Función para cambiar de estado
    void SetState(State newState) {
        currentState = newState;
    }

}
