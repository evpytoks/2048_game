using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class Input : MonoBehaviour
{
    public event Action<Vector2Int> OnMove;
    private PlayerInput inputActions;
    private Vector2Int moveDirection;
    private Vector2 swipeStartPosition;
    private bool isSwiping = false;

    private void Awake()
    {
        inputActions = new PlayerInput();
    }

    private void OnEnable()
    {
        inputActions.Enable();

        inputActions.Player.Move.performed += ctx => Move(ctx.ReadValue<Vector2>());
        inputActions.Player.Swipe.started += ctx => OnSwipeStart(ctx);
        inputActions.Player.Swipe.canceled += ctx => OnSwipeEnd(ctx);
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Move(Vector2 input)
    {
        moveDirection = Vector2Int.zero;
        
        if (input.x > 0)  
        {
            moveDirection = Vector2Int.right;
        } else if (input.x < 0)  
        {
            moveDirection = Vector2Int.left;
        } else if (input.y > 0) 
        {
            moveDirection = Vector2Int.up;
        }
        else if (input.y < 0) 
        {
            moveDirection = Vector2Int.down;
        }

        if (moveDirection != Vector2Int.zero)
        {
            Debug.Log($"Move: {moveDirection}");
            OnMove?.Invoke(moveDirection);
        }
    }

    private void OnSwipeStart(InputAction.CallbackContext context)
    {
        if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            swipeStartPosition = Mouse.current.position.ReadValue();
        }
        else if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            swipeStartPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        }

        isSwiping = true;
    }

    private void OnSwipeEnd(InputAction.CallbackContext context)
    {
        if (!isSwiping) return;

        Vector2 swipeEndPosition = Vector2.zero;

        if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            swipeEndPosition = Mouse.current.position.ReadValue();
        }
        else if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            swipeEndPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        }

        Vector2 swipeDelta = swipeEndPosition - swipeStartPosition;

        if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
        {
            moveDirection = swipeDelta.x > 0 ? Vector2Int.right : Vector2Int.left;
        }
        else
        {
            moveDirection = swipeDelta.y > 0 ? Vector2Int.up : Vector2Int.down;
        }

        Debug.Log($"Swipe: {moveDirection}");
        OnMove?.Invoke(moveDirection); 
        isSwiping = false; 
    }
}