using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IK_procedural : MonoBehaviour
{
    public Transform cuerpo; // El cuerpo del personaje (su centro de gravedad)
    public float distanciaPasos = 0.5f; // Distancia m�xima entre los pasos
    public float velocidad = 3f; // Velocidad de los pasos
    public float stepHeight = 0.2f; // Altura m�xima de cada paso
    public float desfaseInicial = 0.5f; // Desfase inicial para desincronizar los pies
    public float umbralMovimiento = 0.01f; // Umbral m�nimo para considerar que el personaje se est� moviendo
    public float anticipacion = 0.5f; // Anticipaci�n para que el pie est� m�s adelante del cuerpo

    private float espacioPie; // Posici�n relativa del pie respecto al cuerpo
    private float lerp; // Interpolaci�n de los pasos
    private bool enMovimiento; // Indica si el personaje est� en movimiento
    private Vector3 previousCuerpoPosition; // Posici�n previa del cuerpo para detectar movimiento

    [SerializeField] LayerMask terrainLayer = default; // Capa de terreno para detectar el suelo

    Vector3 currentPos, newPosition, oldPosition; // Posiciones del pie

    void Start()
    {
        currentPos = newPosition = oldPosition = transform.position;
        espacioPie = transform.localPosition.x; // Distancia del pie con respecto al cuerpo
        lerp = 1; // Comienza con el pie en reposo (lerp = 1)
        enMovimiento = false; // El personaje comienza quieto
        previousCuerpoPosition = cuerpo.position; // Guardar la posici�n inicial del cuerpo
    }

    void Update()
    {
        // Detectar si el personaje se ha movido comparando la posici�n actual con la anterior
        Vector3 movimientoPersonaje = cuerpo.position - previousCuerpoPosition;
        float distanciaMovimiento = new Vector3(movimientoPersonaje.x, 0, movimientoPersonaje.z).magnitude;

        // Detectar si el personaje est� en movimiento o si se ha detenido
        if (distanciaMovimiento >= umbralMovimiento)
        {
            if (!enMovimiento)
            {
                enMovimiento = true;
            }
        }
        else
        {
            enMovimiento = false; // El personaje est� quieto, los pies se mantienen en su lugar
        }

        // Si el personaje no se est� moviendo, detener la animaci�n de los pies
        if (!enMovimiento)
        {
            return; // No mover los pies si el personaje est� quieto
        }


        // Actualiza la posici�n del pie actual
        transform.position = currentPos;

        // Movimiento lateral (izquierda/derecha)
        Vector3 lateral = cuerpo.right.normalized;

        // Proyectar la posici�n del cuerpo hacia adelante en base a la direcci�n y velocidad del movimiento
        Vector3 direccionMovimiento = movimientoPersonaje.normalized;
        Vector3 posicionProyectada = cuerpo.position + direccionMovimiento * anticipacion;

        // El rayo desde el pie para detectar el terreno (desde el cuerpo hacia el suelo)
        Ray ray = new Ray(posicionProyectada + (lateral * espacioPie), Vector3.down);
        Debug.DrawRay(posicionProyectada + (lateral * espacioPie), Vector3.down, Color.red); // Dibuja el rayo para visualizar

        if (Physics.Raycast(ray, out RaycastHit info, 10, terrainLayer))
        {
            // Calcula la distancia en el plano XZ (solo horizontal) entre la posici�n actual del pie y la nueva posici�n
            float distanciaXZ = Vector3.Distance(new Vector3(newPosition.x, 0, newPosition.z), new Vector3(info.point.x, 0, info.point.z));

            // Si el pie ha avanzado lo suficiente y no est� en medio de un paso, inicia un nuevo paso
            if (distanciaXZ > distanciaPasos && lerp >= 1)
            {
                newPosition = info.point; // Nueva posici�n en el terreno
                lerp = 0; // Reinicia la interpolaci�n para iniciar el nuevo paso
            }
        }

        // Si el pie est� en proceso de hacer un paso
        if (lerp < 1)
        {
            // Interpola la posici�n del pie entre la posici�n anterior y la nueva usando SmoothStep para una transici�n suave
            Vector3 footPos = Vector3.Lerp(oldPosition, newPosition, Mathf.SmoothStep(0, 1, lerp));

            // Aumenta la altura del pie con una curva sinusoidal para darle un movimiento de arco
            footPos.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

            // Actualiza la posici�n actual del pie
            currentPos = footPos;

            // Incrementa el valor de la interpolaci�n en funci�n del tiempo y la velocidad del personaje
            lerp += Time.deltaTime * velocidad * (distanciaMovimiento / distanciaPasos);
        }
        else
        {
            // Cuando el paso termina, actualiza la posici�n anterior con la nueva
            oldPosition = newPosition;
        }

        // Al final del frame, actualiza la posici�n previa del cuerpo
        previousCuerpoPosition = cuerpo.position;
    }

    private void OnDrawGizmos()
    {
        // Dibuja una esfera en la posici�n del pie para visualizar el punto de destino
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(newPosition, .1f);
    }
}