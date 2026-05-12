using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerClick : MonoBehaviour
{
    [Header("Links")]
    public static PlayerClick Instance { get; private set; }
    private PlayerInput _inputSys;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;

        _inputSys = new PlayerInput();
    }

    void Update()
    {
        if (IsClick())
            ChekedClickedObject();
    }

    public bool IsClick() => _inputSys.Player.Click.triggered;
    public bool IsHoldClick() => _inputSys.Player.Click.IsPressed();

    private void ChekedClickedObject()
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

        if (hit.collider != null)
        {
            IClickable target = hit.collider.GetComponentInParent<IClickable>();
            target?.Interact();
        }
    }


    private void OnEnable() => _inputSys.Enable();
    private void OnDisable() => _inputSys.Disable();
}
