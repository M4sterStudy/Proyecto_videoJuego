using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IK_procedural : MonoBehaviour
{
    public Transform cuerpo; // El cuerpo del personaje (su centro de gravedad)
    public float distanciaPasos = 0.5f; // Distancia máxima entre los pasos
    public float velocidad = 3f; // Velocidad de los pasos
    public float stepHeight = 0.2f; // Altura máxima de cada paso
    public float desfaseInicial = 0.5f; // Desfase inicial para desincronizar los pies
    public float umbralMovimiento = 0.01f; // Umbral mínimo para considerar que el personaje se está moviendo
    public float anticipacion = 0.5f; // Anticipación para que el pie esté más adelante del cuerpo

    private float espacioPie; // Posición relativa del pie respecto al cuerpo
    private float lerp; // Interpolación de los pasos
    private bool enMovimiento; // Indica si el personaje está en movimiento
    private Vector3 previousCuerpoPosition; // Posición previa del cuerpo para detectar movimiento

    [SerializeField] LayerMask terrainLayer = default; // Capa de terreno para detectar el suelo

    Vector3 currentPos, newPosition, oldPosition; // Posiciones del pie

    void Start()
    {
        currentPos = newPosition = oldPosition = transform.position;
        espacioPie = transform.localPosition.x; // Distancia del pie con respecto al cuerpo
        lerp = 1; // Comienza con el pie en reposo (lerp = 1)
        enMovimiento = false; // El personaje comienza quieto
        previousCuerpoPosition = cuerpo.position; // Guardar la posición inicial del cuerpo
    }

    void Update()
    {
        // Detectar si el personaje se ha movido comparando la posición actual con la anterior
        Vector3 movimientoPersonaje = cuerpo.position - previousCuerpoPosition;
        float distanciaMovimiento = new Vector3(movimientoPersonaje.x, 0, movimientoPersonaje.z).magnitude;

        // Detectar si el personaje está en movimiento o si se ha detenido
        if (distanciaMovimiento >= umbralMovimiento)
        {
            if (!enMovimiento)
            {
                enMovimiento = true;
            }
        }
        else
        {
            enMovimiento = false; // El personaje está quieto, los pies se mantienen en su lugar
        }

        // Si el personaje no se está moviendo, detener la animación de los pies
        if (!enMovimiento)
        {
            return; // No mover los pies si el personaje está quieto
        }


        // Actualiza la posición del pie actual
        transform.position = currentPos;

        // Movimiento lateral (izquierda/derecha)
        Vector3 lateral = cuerpo.right.normalized;

        // Proyectar la posición del cuerpo hacia adelante en base a la dirección y velocidad del movimiento
        Vector3 direccionMovimiento = movimientoPersonaje.normalized;
        Vector3 posicionProyectada = cuerpo.position + direccionMovimiento * anticipacion;

        // El rayo desde el pie para detectar el terreno (desde el cuerpo hacia el suelo)
        Ray ray = new Ray(posicionProyectada + (lateral * espacioPie), Vector3.down);
        Debug.DrawRay(posicionProyectada + (lateral * espacioPie), Vector3.down, Color.red); // Dibuja el rayo para visualizar

        if (Physics.Raycast(ray, out RaycastHit info, 10, terrainLayer))
        {
            // Calcula la distancia en el plano XZ (solo horizontal) entre la posición actual del pie y la nueva posición
            float distanciaXZ = Vector3.Distance(new Vector3(newPosition.x, 0, newPosition.z), new Vector3(info.point.x, 0, info.point.z));

            // Si el pie ha avanzado lo suficiente y no está en medio de un paso, inicia un nuevo paso
            if (distanciaXZ > distanciaPasos && lerp >= 1)
            {
                newPosition = info.point; // Nueva posición en el terreno
                lerp = 0; // Reinicia la interpolación para iniciar el nuevo paso
            }
        }

        // Si el pie está en proceso de hacer un paso
        if (lerp < 1)
        {
            // Interpola la posición del pie entre la posición anterior y la nueva usando SmoothStep para una transición suave
            Vector3 footPos = Vector3.Lerp(oldPosition, newPosition, Mathf.SmoothStep(0, 1, lerp));

            // Aumenta la altura del pie con una curva sinusoidal para darle un movimiento de arco
            footPos.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

            // Actualiza la posición actual del pie
            currentPos = footPos;

            // Incrementa el valor de la interpolación en función del tiempo y la velocidad del personaje
            lerp += Time.deltaTime * velocidad * (distanciaMovimiento / distanciaPasos);
        }
        else
        {
            // Cuando el paso termina, actualiza la posición anterior con la nueva
            oldPosition = newPosition;
        }

        // Al final del frame, actualiza la posición previa del cuerpo
        previousCuerpoPosition = cuerpo.position;
    }

    private void OnDrawGizmos()
    {
        // Dibuja una esfera en la posición del pie para visualizar el punto de destino
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(newPosition, .1f);
    }
}