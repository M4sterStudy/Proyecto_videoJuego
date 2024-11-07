using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum AttackState
{
    Idle,
    LightAttack,
    HeavyAttack,
    Combo
}

public class ComboAttack : MonoBehaviour
{
    public Animator animator;
    private AttackState currentState = AttackState.Idle;
    private float comboTimeWindow = 0.5f;
    private float lastInputTime;

    private List<int> currentCombo = new List<int>();
    private readonly List<int[]> combos = new List<int[]>
    {
        new int[] { 0 }, // LightAttack
        new int[] { 1 }, // HeavyAttack
        new int[] { 0, 0 }, // LightAttack x2
        new int[] { 1, 0, 0, 1 } // HeavyAttack, LightAttack x2, HeavyAttack
    };

    private int attackIndexHash = Animator.StringToHash("AttackIndex");

    private void Start()
    {
        // Inicializar Input System
        var inputActions = new Player();
        inputActions.combate.LightAttack.performed += ctx => HandleInput(0);
        inputActions.combate.HeavyAttack.performed += ctx => HandleInput(1);
        inputActions.Enable();
    }

    private void HandleInput(int attackType)
    {
        if (Time.time - lastInputTime > comboTimeWindow)
        {
            currentCombo.Clear();
            ChangeState(AttackState.Idle); // Reinicia al estado idle si pasó el tiempo
        }

        currentCombo.Add(attackType);
        lastInputTime = Time.time;

        CheckCombo();
    }

    private void CheckCombo()
    {
        for (int i = 0; i < combos.Count; i++)
        {
            if (IsComboMatch(combos[i]))
            {
                ChangeState(AttackState.Combo);
                StartCoroutine(PlayComboAnimation(i));
                return;
            }
        }

        // Si no coincide con un combo definido, actúa según el tipo de ataque individual
        if (currentCombo.Count == 1)
        {
            ChangeState(currentCombo[0] == 0 ? AttackState.LightAttack : AttackState.HeavyAttack);
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

    private void ChangeState(AttackState newState)
    {
        if (currentState == newState) return;

        currentState = newState;

        switch (currentState)
        {
            case AttackState.LightAttack:
                animator.SetInteger(attackIndexHash, 0);
                break;
            case AttackState.HeavyAttack:
                animator.SetInteger(attackIndexHash, 1);
                break;
            case AttackState.Combo:
                // Se manejará dentro de la corrutina `PlayComboAnimation`
                break;
            case AttackState.Idle:
                animator.SetInteger(attackIndexHash, -1);
                break;
        }
    }

    private IEnumerator PlayComboAnimation(int comboIndex)
    {
        animator.SetInteger(attackIndexHash, comboIndex);

        yield return new WaitForEndOfFrame();
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        while (stateInfo.normalizedTime < 1)
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }

        currentCombo.Clear();
        ChangeState(AttackState.Idle);
    }
}