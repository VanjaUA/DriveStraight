using UnityEngine;
using UnityEngine.SceneManagement;

public struct MinMax<T>
{
   public T min;
   public T max;
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    public CarSpawner carSpawner;
    [SerializeField]
    public RoadManager roadManager;
    [SerializeField]
    public Player player;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    public void GameOver() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
