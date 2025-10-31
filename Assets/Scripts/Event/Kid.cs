using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kid : MonoBehaviour
{
    public SpriteRenderer sr;
    public Animator animator;
    private bool isMoving = false;
    private string flagName = "KidRun";

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isMoving)
        {
            transform.Translate(Vector3.right * 2f * Time.deltaTime);
            Destroy(gameObject, 3f);
        }
    }

    private void OnEnable()
    {
        FlagManager.Instance.FlagChanged += OnFlagChanged;
    }

    private void OnDisable()
    {
        FlagManager.Instance.FlagChanged -= OnFlagChanged;
    }

    private void OnFlagChanged(string key, bool value)
    {
        if (key == flagName && value)
        {
            Run();
        }
    } // OnFlagChanged ed

    private void Run()
    {
        sr.flipX = true;
        animator.SetBool("canMove", true);
        isMoving = true;
    }
}
