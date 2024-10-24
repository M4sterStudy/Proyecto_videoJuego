using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ComboAttack : MonoBehaviour
{
    public Animator animator;

    // Lista de secuencias de ataques
    private List<int> currentCombo = new List<int>();
    private float comboTimeWindow = 0.5f; // Tiempo entre inputs para que cuenten como combo
    private float lastInputTime = 0;

    // IDs de los combos
    private readonly List<int[]> combos = new List<int[]>
    {
        new int[] { 0 }, // LightAttack
        new int[] { 1 }, // HeavyAttack
        new int[] { 0, 0 }, // LightAttack x2
        new int[] { 1, 0, 0, 1 } // HeavyAttack, LightAttack x2, HeavyAttack
    };

    // Parámetro del Animator para controlar animaciones
    private int attackIndexHash = Animator.StringToHash("AttackIndex");

    private void Start()
    {
        // Inicializar el Input System
        var inputActions = new Player();
        inputActions.personaje.LightAttack.performed += ctx => OnLightAttack();
        inputActions.personaje.HeavyAttack.performed += ctx => OnHeavyAttack();
        inputActions.Enable();
    }

    private void OnLightAttack()
    {
        RegisterInput(0); // LightAttack es 0
    }

    private void OnHeavyAttack()
    {
        RegisterInput(1); // HeavyAttack es 1
    }

    private void RegisterInput(int attackType)
    {
        // Si el tiempo entre inputs supera el comboTimeWindow, reinicia la secuencia
        if (Time.time - lastInputTime > comboTimeWindow)
        {
            currentCombo.Clear(); // Si ha pasado demasiado tiempo, reiniciamos la secuencia
        }

        // Añadir el nuevo ataque al combo
        currentCombo.Add(attackType);
        lastInputTime = Time.time;

        // Imprimir para depurar qué ataques se están registrando
        Debug.Log("Current Combo: " + string.Join(",", currentCombo));

        // Comprobamos si el combo coincide con alguno definido
        CheckCombo();
    }

    private void CheckCombo()
    {
        // Revisar todos los combos definidos
        for (int i = 0; i < combos.Count; i++)
        {
            if (IsComboMatch(combos[i]))
            {
                Debug.Log("Combo detected: " + i); // Añadimos un log para verificar qué combo se detecta

                // Ejecutar la animación del combo detectado
                StartCoroutine(PlayComboAnimation(i));

                currentCombo.Clear(); // Limpiar la secuencia después de detectar el combo
                return; // Salir de la función para evitar que otros combos se activen en el mismo frame
            }
        }
    }

    private bool IsComboMatch(int[] combo)
    {
        if (currentCombo.Count != combo.Length)
            return false;

        for (int i = 0; i < combo.Length; i++)
        {
            if (currentCombo[i] != combo[i])
                return false;
        }

        return true;
    }

    // Coroutine para asegurarse de que las animaciones se reproducen de una en una
    private IEnumerator PlayComboAnimation(int comboIndex)
    {
        animator.SetInteger(attackIndexHash, comboIndex); // Establecemos el combo correcto

        // Esperar a que comience la animación
        yield return new WaitForEndOfFrame();
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Esperar hasta que la animación termine
        while (stateInfo.normalizedTime < 1)
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }

        animator.SetInteger(attackIndexHash, -1); // Reiniciar el estado del Animator
    }
}
