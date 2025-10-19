using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gyuki : MonoBehaviour
{
    public enum BossState { Idle, Move, Atk, Die }
    public BossState currentState = BossState.Idle;

    [Header("Movement")]
    public float walkSpeed = 0.5f;
    public float detectionRange = 12f;
    public float atkRange = 4.5f;
    public float backstepDistance = 4f;

    [Header("References")]
    public Transform player;
    private bool isAtk = false;

    private void Start()
    {
        if (player == null)
        {
            GameObject go = GameObject.FindWithTag("Player");
            if (go != null) player = go.transform;
        }
        StartCoroutine(StateMachine());
    }

    private IEnumerator StateMachine()
    {
        while (true)
        {
            switch (currentState)
            {
                case BossState.Idle:
                    yield return IdleState();
                    break;
                case BossState.Move:
                    yield return MoveState();
                    break;
                case BossState.Atk:
                    yield return AtkState();
                    break;
                case BossState.Die:
                    yield return DieState();
                    break;
            }
            yield return null;
        }
    }

    private IEnumerator IdleState()
    {
        Debug.Log("보스 상태: Idle");
        yield return new WaitForSeconds(2f);
        currentState = BossState.Move;
    }
    private IEnumerator MoveState()
    {
        Debug.Log("보스 상태: Move");
        float timer = 0f;
        float moveDuration = 5f; // 최대이동시간
        while (timer < moveDuration)
        {
            timer += Time.deltaTime;
            if (player != null)
            {
                float dx = player.position.x - transform.position.x;
                float distance = Mathf.Abs(dx);
                float dir = Mathf.Sign(dx);

                if (distance <= atkRange)
                {
                    currentState = BossState.Atk;
                    yield break;
                }
                else if (distance <= detectionRange)
                {
                    // 천천히 접근
                    transform.Translate(Vector3.right * dir * walkSpeed * Time.deltaTime);
                } // if - else if ed
            } // if ed
            yield return null;
        } // while ed
                currentState = BossState.Idle;
    }  

    private IEnumerator AtkState()
    {
        if (isAtk) yield break;
        isAtk = true;
        Debug.Log("보스 상태: Atk");
        yield return null;
    }
    private IEnumerator DieState()
    {
        Debug.Log("보스 상태: Die");
        yield break;
    }

    private void OnDrawGizmosSelected()
    {
        // 선택된 오브젝트(보스) 기준으로 기즈모 그리기

        // 플레이어 감지 범위: 파란색 원
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // 공격 범위: 빨간색 원
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, atkRange);
    }
} // class ed