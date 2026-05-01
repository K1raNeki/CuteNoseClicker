using UnityEngine;
using UnityEngine.UI;

public class AgressiveBar : MonoBehaviour
{
    [Header("LinksMain")]
    [SerializeField] private Canvas _agressiveCanvas;
    [SerializeField] private Image _progressBar;

    void Awake()
    {
        _agressiveCanvas.gameObject.SetActive(false);

        Animal.AnimalAgressiveStart += StartMiniGame;
    }

    private void StartMiniGame(bool isStart)
    {
        _agressiveCanvas.gameObject.SetActive(isStart);

    }

    private void UpdateUIBarAgressive()
    {
    }


    void OnDestroy() => Animal.AnimalAgressiveStart -= StartMiniGame;
}
