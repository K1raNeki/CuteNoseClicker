using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MinigamePoint : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] SpriteRenderer _renderer;
    public bool PointIsClicked { get; private set; }

    void Awake()
    {
        if (_renderer == null) _renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        MovePoint();
    }

    public void Completed()
    {
        Debug.Log($"{this} выебан");
        PointIsClicked = true;
        _renderer.color = Color.green;
    }
    public void Fail()
    {
        Debug.Log($"{this} проебан");
        if (!PointIsClicked)
            _renderer.color = Color.red;
    }

    private void MovePoint()
    {
        transform.position = new Vector2(transform.position.x - 0.003f * Time.deltaTime, transform.position.y);
    }
}
