using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using Random = UnityEngine.Random;

[Serializable]
public struct CameraPosInfo
{
    public Vector3 position;
    public float size;
}

public enum CameraMoveMode
{
    FollowMode,
    ZoomMode
}

public class CameraController : Singleton<CameraController>
{
    public float shakeSpeed = 5f;
    public float shakePower = 2.0f;
    public float shakeTime = 1.0f;

    private SpriteShapeController ssc;

    public CameraPosInfo initPosInfo;
    public CameraPosInfo targetPosInfo;
    public List<CameraPosInfo> treeStartPosInfos;
    public List<CameraPosInfo> levelStartPosInfos;

    public Vector3 vel = Vector3.zero;
    private float zoomVel = 0f;
    public float smoothTime = 0.3f;
    public Vector3 lastRootPos;

    public CameraMoveMode mode = CameraMoveMode.ZoomMode;

    private Camera _camera;

    private bool inZoomProgress;

    public Vector2 CameraPosition
    {
        get => transform.position;
        set => transform.position = new Vector3(value.x, value.y, transform.position.z);
    }

    protected override void Awake()
    {
        base.Awake();
        _camera = GetComponent<Camera>();
        targetPosInfo = initPosInfo;
        inZoomProgress = true;
    }

    private void Update()
    {
        switch (mode)
        {
            case CameraMoveMode.FollowMode:
                if (GameManager.Instance.isGameStart && !GameManager.Instance.isGameOver)
                {
                    ssc = GameManager.Instance.rootManagers[GameManager.Instance.currentLevel].roots[0].ssc;
                    lastRootPos = ssc.spline.GetPosition(ssc.spline.GetPointCount() - 1);
                    var newPos = new Vector3(transform.position.x, lastRootPos.y, transform.position.z);
                    transform.position =
                        Vector3.SmoothDamp(transform.position, newPos, ref vel, smoothTime);
                }

                break;
            case CameraMoveMode.ZoomMode:
                if (Vector3.Distance(targetPosInfo.position, transform.position) < 0.1f)
                    inZoomProgress = false;
                else if (inZoomProgress) MoveToTarget();

                break;
        }
    }

    public void CameraShakeAfterGameOver()
    {
        StartCoroutine(IECameraShake());
    }


    public IEnumerator MoveToLevel(int index)
    {
        AudioController.Instance.swoosh.Play();

        MoveToTarget(treeStartPosInfos[index]);
        while (inZoomProgress) yield return null;

        AudioController.Instance.GameStart();

        ssc = GameManager.Instance.rootManagers[GameManager.Instance.currentLevel].roots[0].ssc;
        lastRootPos = ssc.spline.GetPosition(ssc.spline.GetPointCount() - 1);
        var newPos = new Vector3(transform.position.x, lastRootPos.y, transform.position.z);
        var n = levelStartPosInfos[index];
        n.position = newPos;
        MoveToTarget(n);
        while (inZoomProgress) yield return null;
        mode = CameraMoveMode.FollowMode;
    }

    public IEnumerator MoveToTitleAfterWin()
    {
        mode = CameraMoveMode.ZoomMode;
        MoveToTarget(treeStartPosInfos[GameManager.Instance.currentLevel]);
        while (inZoomProgress) yield return null;
        yield return new WaitForSeconds(2f);
        //todo congs
        MoveToTarget(initPosInfo);
        while (inZoomProgress) yield return null;
        AudioController.Instance.BackToTitle();
    }

    public void GameWin()
    {
        StartCoroutine(MoveToTitleAfterWin());
    }

    public void ReturnToMenu()
    {
        MoveToTarget(initPosInfo);
    }

    private void MoveToTarget()
    {
        _camera.orthographicSize =
            Mathf.SmoothDamp(_camera.orthographicSize, targetPosInfo.size, ref zoomVel, smoothTime);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosInfo.position, ref vel, smoothTime);
    }

    private IEnumerator IECameraShake()
    {
        Func<float> getRandom = () => Random.value * shakeSpeed;
        var originShake = new Vector2(getRandom(), getRandom());
        {
            float timeCount = 0;
            var origin = CameraPosition;
            Func<Vector2> noise = () =>
                new Vector2(Mathf.PerlinNoise(originShake.x, 0), Mathf.PerlinNoise(0, originShake.y)) * 2 -
                new Vector2(1, 1);
            CameraPosition = origin + shakePower * noise();
            while (timeCount < shakeTime)
            {
                yield return null;
                timeCount += Time.unscaledDeltaTime;
                originShake += new Vector2(1, 1) * (shakeSpeed * Time.unscaledDeltaTime);
                CameraPosition = origin + shakePower * noise();
            }

            CameraPosition = origin;
        }
        mode = CameraMoveMode.ZoomMode;
        yield return new WaitForSeconds(0.5f);
        UIManager.Instance.FadeOut();
        yield return new WaitForSeconds(2f);
        UIManager.Instance.FadeIn();
        ReturnToMenu();
        AudioController.Instance.BackToTitle();
    }

    public void MoveToTarget(CameraPosInfo des)
    {
        targetPosInfo = des;
        inZoomProgress = true;
    }
}