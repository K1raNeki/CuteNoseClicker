using System.Collections;
using UnityEngine;

public class MiniGameLineController
{
    private MiniGameMain _miniGameMain;

    public MiniGameLineController(MiniGameMain mgMain) => _miniGameMain = mgMain;

    public void CreateButtonLines(bool isStart)
    {
        switch (isStart)
        {
            case true:
                for (int i = 0; i < _miniGameMain.CurrentFactor.InvertPoints.Length; i++)
                    _miniGameMain._completedInvertPoint.Add(false);

                if (_miniGameMain.CurrentLine != null)
                    _miniGameMain.CurrentLine.SetActive(true);
                else
                    _miniGameMain.CurrentLine = Object.Instantiate(_miniGameMain.Data.LinePrefab, _miniGameMain.MinigameContainer.transform);

                var parentScale = _miniGameMain.MinigameContainer.transform.lossyScale;
                float screenWidth = Camera.main.orthographicSize * 2f * Camera.main.aspect;

                _miniGameMain.CurrentLine.transform.localScale = new Vector2(
                    screenWidth / parentScale.x,
                    _miniGameMain.Data.LineHight / parentScale.y);

                // _miniGameMain.CurrentLine.transform.localScale = new Vector2(Camera.main.orthographicSize * 2 * Camera.main.aspect, _miniGameMain.Data.LineHight);
                break;

            case false:
                _miniGameMain._completedInvertPoint.Clear();
                _miniGameMain.CurrentLine.SetActive(false);

                foreach (MiniGamePoint point in _miniGameMain._busyObjects)
                {
                    _miniGameMain._freeObjects.Add(point);
                    point.gameObject.SetActive(false);
                }
                _miniGameMain._busyObjects.Clear();
                break;
        }
    }

    public IEnumerator InvertLine(float dur, Collider2D barier)
    {
        _miniGameMain.LineInvert = !_miniGameMain.LineInvert;
        float xPos = barier.transform.position.x;
        Transform mainPos = barier.transform;

        _miniGameMain.GameIsStart = false;
        barier.transform.position = new Vector3(-xPos, mainPos.position.y, mainPos.position.z);
        foreach (MiniGamePoint point in _miniGameMain._busyObjects)
            point.StartCoroutine(point.InvertPoint(dur));

        yield return new WaitForSeconds(dur);
        _miniGameMain.GameIsStart = true;
    }

}
