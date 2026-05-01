using UnityEngine;

public class MinigameBar : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D collision)
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
