using UnityEngine;
using UnityEngine.UI;

public class MinigameBar : MonoBehaviour
{
    [Header("Links")]
    public static MinigameBar Instance { get; private set; }
    [SerializeField] private Image _agressiveBar;

    [Header("Settings")]
    public float BarScore;
    private float _animalStrange = 0.6f;

    void Awake()
    {
        Instance = this;
        _agressiveBar.fillAmount = _animalStrange;
    }

    public void GetPointImpact(bool positive, float score = 0.01f)
    {
        BarScore += positive ? -score : score;
        if(BarScore <= 0)
        {
            BarScore = 0;
            Animal.AnimalAgressiveStart(false);
        }
        _agressiveBar.fillAmount = BarScore;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<MinigamePoint>(out MinigamePoint point))
        {
            if (!point.PointIsClicked)
            {
                point.Fail();
            }
        }
    }
}
