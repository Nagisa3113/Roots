public class UIManager : Singleton<UIManager>
{
    protected override void Awake()
    {
        base.Awake();
    }

    public void PlayeLevel(int index)
    {
        GameManager.Instance.GameStart(index);
    }
}