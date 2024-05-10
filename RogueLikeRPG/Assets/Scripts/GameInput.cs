using System;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    private PlayerInputActions playerInputActions;

    public event EventHandler OnPlayerAttack;

    private bool pause = false;

    private void Awake()
    {
        Instance = this;
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();

        playerInputActions.Combat.Attack.started += PlayerAttack_started;
        playerInputActions.PauseGame.Pause.started += Pause_started;
    }

    private void Pause_started(InputAction.CallbackContext obj)
    {
        if (!pause)
        {
            Time.timeScale = 0;
            Player.Instance.playerVisual.GetComponent<PlayerVisual>().enabled = false;
            pause = true;
        }
        else
        {
            Time.timeScale = 1;
            Player.Instance.playerVisual.GetComponent<PlayerVisual>().enabled = true;
            pause = false;
        }
    }

    private void PlayerAttack_started(InputAction.CallbackContext obj)
    {
        if (!pause)
        {
            OnPlayerAttack?.Invoke(this, EventArgs.Empty);
        }
    }

    public Vector2 GetMovementVector()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        return inputVector;
    }

    public bool ProtectStatus()
    {
        return playerInputActions.Combat.Protect.IsPressed();
    }

    public Vector3 GetMousePosition()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        return mousePos;
    }
}
