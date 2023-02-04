using UnityEngine;
using UnityEngine.U2D;

public class SpriteShapeCollider : MonoBehaviour
{
    private SpriteShapeController ssc;

    private void Awake()
    {
        ssc = GetComponentInParent<SpriteShapeController>();
    }

    private void Update()
    {
        transform.localPosition = ssc.spline.GetPosition(ssc.spline.GetPointCount() - 1);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.name);
        GameManager.Instance.GameOver();
    }
}