using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerClick : MonoBehaviour
{
    [Header("Links")]
    private PlayerInput _inputSys;

    void Awake()
    {
        _inputSys = new PlayerInput();
    }

    void Update()
    {
        if (IsClick())
            CheckedMousePos();
    }

    public bool IsClick() => _inputSys.Player.Click.triggered;
    public bool IsHoldClick() => _inputSys.Player.Click.IsPressed();

    private void CheckedMousePos()
    {
        Debug.Log(1);
        if (Mouse.current == null) return;

        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

        RaycastHit2D _hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

        if (_hit.collider != null)
        {
                    Debug.Log(3);

            if (_hit.collider.TryGetComponent<Animal>(out var animal))
            {
                        Debug.Log(4);

                animal.Interact();
                Debug.Log($"Попал в {_hit.collider.name}");
            }
        }
                Debug.Log(2);

    }

    private void OnEnable() => _inputSys.Enable();
    private void OnDisable() => _inputSys.Disable();
}
