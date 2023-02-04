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
    private bool isShaking;
    private bool startShake;

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
        if (Vector3.Distance(targetPosInfo.position, transform.position) < 0.1f)
            inZoomProgress = false;
        else if (inZoomProgress) MoveToTarget();

        if (startShake && isShaking == false)
        {
            isShaking = true;
            startShake = false;
            StartCoroutine(IECameraShake());
        }
    }

    private void LateUpdate()
    {
        if (mode == CameraMoveMode.FollowMode)
            if (GameManager.Instance.isGameStart && !GameManager.Instance.isGameOver)
            {
                ssc = GameManager.Instance.rootManagers[GameManager.Instance.currentLevel].roots[0].ssc;
                lastRootPos = ssc.spline.GetPosition(ssc.spline.GetPointCount() - 1);
                var newPos = new Vector3(transform.position.x, lastRootPos.y, transform.position.z);
                transform.position = newPos;
            }
    }

    public IEnumerator MoveToLevel(int index)
    {
        ssc = GameManager.Instance.rootManagers[GameManager.Instance.currentLevel].roots[0].ssc;
        lastRootPos = ssc.spline.GetPosition(ssc.spline.GetPointCount() - 1);
        MoveToTarget(treeStartPosInfos[index]);
        while (inZoomProgress) yield return null;

        lastRootPos = ssc.spline.GetPosition(ssc.spline.GetPointCount() - 1);
        var newPos = new Vector3(transform.position.x, lastRootPos.y, transform.position.z);
        var n = levelStartPosInfos[index];
        n.position = newPos;
        MoveToTarget(n);
        while (inZoomProgress) yield return null;
        mode = CameraMoveMode.FollowMode;

        AudioController.Instance.GameStart();
    }

    public void ReturnToMenu()
    {
        MoveToTarget(initPosInfo);
    }

    public void CameraShake()
    {
        startShake = true;
        isShaking = false;
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
                originShake += new Vector2(1, 1) * shakeSpeed * Time.unscaledDeltaTime;
                CameraPosition = origin + shakePower * noise();
            }

            CameraPosition = origin;
        }
        isShaking = false;
        mode = CameraMoveMode.ZoomMode;
        yield return new WaitForSeconds(2f);
        ReturnToMenu();
        AudioController.Instance.BackToTitle();
    }

    public void MoveToTarget(CameraPosInfo des)
    {
        targetPosInfo = des;
        inZoomProgress = true;
    }
    
}