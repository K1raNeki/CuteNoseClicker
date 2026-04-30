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
            ChekedClickedObject();
    }

    public bool IsClick() => _inputSys.Player.Click.triggered;
    public bool IsHoldClick() => _inputSys.Player.Click.IsPressed();

    private void ChekedClickedObject()
    {
        if (Mouse.current == null) return;

        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

        RaycastHit2D _hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

        if (_hit.collider != null)
        {
            MonoBehaviour target = _hit.collider.GetComponentInParent<Animal>();
            Debug.Log($"Попал в {_hit.collider.name}");

            switch (target)
            {
                case Animal animal:
                    float multy = (_hit.collider == animal.NoseCollider) ? 2f : 1f;
                    animal.TakeCare(multy);
                    break;
            }
        }

    }


    private void OnEnable() => _inputSys.Enable();
    private void OnDisable() => _inputSys.Disable();
}
