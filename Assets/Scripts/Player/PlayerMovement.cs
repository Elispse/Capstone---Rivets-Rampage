using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private InputActionReference pointerPos;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;
    private Vector2 mousePos;
    private WeaponParent weaponParent;
    [SerializeField]
    private PlayerActions playerActions;
    private bool movementChange = false;
    private bool change = false;


    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        weaponParent = GetComponentInChildren<WeaponParent>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = moveInput * moveSpeed * Time.deltaTime;
        mousePos = GetPointerInput();
        weaponParent.pointerPosition = mousePos;
        if (playerActions.held)
        {
            movementChange = true;
        }
        else if (!playerActions.held && !change && movementChange)
        {
            StartCoroutine(movementChangeCR(3));
            change = true;
        }
        if (movementChange)
        {
            animator.SetFloat("CurX", -(this.transform.position.x - mousePos.x));
            animator.SetFloat("CurY", -(this.transform.position.y - mousePos.y));
            animator.SetFloat("LastX", -(this.transform.position.x - mousePos.x));
            animator.SetFloat("LastY", -(this.transform.position.y - mousePos.y));
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        animator.SetBool("isWalking", true);

        if (context.canceled)
        {
            animator.SetBool("isWalking", false);
            if (!movementChange)
            {
                animator.SetFloat("LastX", moveInput.x);
                animator.SetFloat("LastY", moveInput.y);
            }
        }

        moveInput = context.ReadValue<Vector2>();
        if (!movementChange)
        {
            animator.SetFloat("CurX", moveInput.x);
            animator.SetFloat("CurY", moveInput.y);
        }
    }

    private Vector2 GetPointerInput()
    {
        Vector3 mousePos = pointerPos.action.ReadValue<Vector2>();
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    IEnumerator movementChangeCR(float time)
    {
        yield return new WaitForSeconds(time);
        movementChange = false;
        change = false;
    }
}
