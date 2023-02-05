using UnityEngine;
using UnityEngine.U2D;

public class SpriteShapeCollider : MonoBehaviour
{
    private SpriteShapeController ssc;
    private RootController rc;

    private void Awake()
    {
        ssc = GetComponentInParent<SpriteShapeController>();
        rc = GetComponentInParent<RootController>();
    }

    private void Update()
    {
        transform.localPosition = ssc.spline.GetPosition(ssc.spline.GetPointCount() - 1);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.name);
        if (col.CompareTag("Obstacle") == true)
        {
            AudioController.Instance.PlayImpact();
            //todo delay
            GameManager.Instance.GameOver();
        }

        if (col.CompareTag("Goal") == true)
        {
            rc.hasReachedGoal = true;
            rc.SetupInactive();
            // GameManager.Instance.GameWin();
        }
    }
}