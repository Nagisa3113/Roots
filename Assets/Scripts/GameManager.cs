using System.Collections.Generic;

public class GameManager : Singleton<GameManager>
{
    public int currentLevel;
    public bool isGameOver;
    public bool isGameStart;
    public List<RootManager> rootManagers;

    protected override void Awake()
    {
        base.Awake();
    }

    public void GameStart(int index)
    {
        currentLevel = index;
        isGameStart = true;
        isGameOver = false;
        rootManagers[index].SetupActive();
        CameraController.Instance.StartCoroutine(CameraController.Instance.MoveToLevel(index));
    }

    public void GameOver()
    {
        isGameOver = true;
        rootManagers[currentLevel].SetupInactive();
        CameraController.Instance.CameraShake();
    }

    public void GameWin()
    {
        //todo: congs
        //todo: camera move 
    }
}