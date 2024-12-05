using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FMOD.Studio;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private InputActionReference pointerPos;
    [SerializeField] private PlayerActions playerActions;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;
    private Vector2 mousePos;
    private WeaponParent weaponParent;
    private bool movementChange = false;
    private bool change = false;

    private EventInstance playerFootsteps;

    private void Start()
    {
        playerFootsteps = AudioManager.instance.CreateInstance(FMODEvents.instance.playerWalk);
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        weaponParent = GetComponentInChildren<WeaponParent>();
    }


    private void Update()
    {
        if (Time.timeScale == 0)
        {
            playerFootsteps.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }

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
        UpdateSound();
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

    private void UpdateSound()
    {
        if (animator.GetBool("isWalking") == true)
        {
            PLAYBACK_STATE playbackState;
            playerFootsteps.getPlaybackState(out playbackState);
            if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
            {
                playerFootsteps.start();
            }
        }
        else
        {
            playerFootsteps.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }

    IEnumerator movementChangeCR(float time)
    {
        yield return new WaitForSeconds(time);
        movementChange = false;
        change = false;
    }
}
