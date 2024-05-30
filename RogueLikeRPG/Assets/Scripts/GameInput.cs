using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    private PlayerInputActions playerInputActions;

    public event EventHandler OnPlayerAttack;

    public bool pause = false;
    public bool help = false;
    public bool otherMenu = false;

    public GameObject PauseMenu;
    public GameObject HelpMenu;

    public GameObject textAndBars;

    private void Awake()
    {
        Instance = this;
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();

        playerInputActions.Combat.Attack.started += PlayerAttack_started;
        playerInputActions.PauseGame.Pause.started += Pause_started;
    }

    private void OnDestroy()
    {
        playerInputActions.Combat.Attack.started -= PlayerAttack_started;
        playerInputActions.PauseGame.Pause.started -= Pause_started;
    }

    private void Update()
    {
        PauseMenu.SetActive(pause);
        HelpMenu.SetActive(help);
    }

    private void Pause_started(InputAction.CallbackContext obj)
    {
        if (!otherMenu) {
            if (!pause)
            {
                pause = true;
                PauseGame();
            }
            else
            {
                UnpauseGame();
            }
        }
    }

    public void PauseGame()
    {
        playerInputActions.Combat.Attack.started -= PlayerAttack_started;
        Time.timeScale = 0;
        Player.Instance.playerVisual.GetComponent<PlayerVisual>().enabled = false;
    }

    public void Help()
    {
        otherMenu = true;
        pause = false;
        help = true;
    }

    public void UnpauseGame()
    {
        playerInputActions.Combat.Attack.started += PlayerAttack_started;
        Player.Instance.playerVisual.GetComponent<PlayerVisual>().enabled = true;
        Time.timeScale = 1;
        pause = false;
        help = false;
        otherMenu = false;
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

