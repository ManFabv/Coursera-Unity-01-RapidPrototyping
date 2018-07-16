using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Range(0, 60)]
    public int TargetFrameRate = 60;

    private void Awake()
    {
        Setup();
    }

    protected virtual void Setup()
    {
        Application.targetFrameRate = TargetFrameRate;
    }
}
