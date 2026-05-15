using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameLineController
{
    private MiniGameMain _miniGameMain;

    public MiniGameLineController(MiniGameMain mgMain) => _miniGameMain = mgMain;

    public void CreateButtonLines(bool isStart)
    {
        switch (isStart)
        {
            case true:
                if (_miniGameMain.CurrentLine != null)
                    _miniGameMain.CurrentLine.SetActive(true);
                else
                    _miniGameMain.CurrentLine = Object.Instantiate(_miniGameMain.Data.LinePrefab, _miniGameMain.MinigameContainer.transform);

                var parentScale = _miniGameMain.MinigameContainer.transform.lossyScale;
                float screenWidth = Camera.main.orthographicSize * 2f * Camera.main.aspect;

                _miniGameMain.CurrentLine.transform.localScale = new Vector2(
                    screenWidth / parentScale.x,
                    _miniGameMain.Data.LineHight / parentScale.y);

                ArrangeInvertPoint();
                break;

            case false:
                _miniGameMain.CurrentLine.SetActive(false);

                foreach (MiniGamePoint point in _miniGameMain.BusyObjects)
                {
                    _miniGameMain.FreeObjects.Add(point);
                    point.gameObject.SetActive(false);
                }

                foreach (MiniGameMain.InvertPoint point in _miniGameMain.InvertsPoint)
                    Object.Destroy(point.ImagePoit.gameObject);

                _miniGameMain.InvertsPoint.Clear();
                _miniGameMain.BusyObjects.Clear();
                break;
        }
    }

    private void ArrangeInvertPoint()
    {
        for (int i = 0; i < _miniGameMain.CurrentFactor.InvertPoints.Length; i++)
        {
            MiniGameMain.InvertPoint newInvertPoint
                = new MiniGameMain.InvertPoint(Object.Instantiate(
                    _miniGameMain.InvertPointPrefab, _miniGameMain.AgressiveBar.transform));

            RectTransform rt = newInvertPoint.ImagePoit.rectTransform;
            rt.anchorMin = new Vector2(_miniGameMain.CurrentFactor.InvertPoints[i], 0.5f);
            rt.anchorMax = new Vector2(_miniGameMain.CurrentFactor.InvertPoints[i], 0.5f);
            rt.sizeDelta = new Vector2(80f, 80);

            _miniGameMain.InvertsPoint.Add(newInvertPoint);
        }
    }

    public IEnumerator InvertLine(float dur, Collider2D barier)
    {
        _miniGameMain.LineInvert = !_miniGameMain.LineInvert;
        float xPos = barier.transform.position.x;
        Transform mainPos = barier.transform;

        _miniGameMain.GameIsStart = false;
        barier.transform.position = new Vector3(-xPos, mainPos.position.y, mainPos.position.z);
        foreach (MiniGamePoint point in _miniGameMain.BusyObjects)
            point.StartCoroutine(point.InvertPoint(dur));

        yield return new WaitForSeconds(dur);
        _miniGameMain.GameIsStart = true;
    }

}
