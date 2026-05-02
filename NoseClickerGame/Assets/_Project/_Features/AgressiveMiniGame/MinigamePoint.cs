using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MinigamePoint : MonoBehaviour, IClickable
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

    public void Interact() => Completed();

    public void Completed()
    {
        MinigameBar.Instance.GetPointImpact(true);
        Debug.Log($"{this} выебан");
        PointIsClicked = true;
        _renderer.color = Color.green;
    }
    public void Fail()
    {
        MinigameBar.Instance.GetPointImpact(false);
        Debug.Log($"{this} проебан");
        if (!PointIsClicked)
            _renderer.color = Color.red;
    }

    private void MovePoint()
    {
        transform.position = new Vector2(transform.position.x - 6f * Time.deltaTime, transform.position.y);
    }
}
