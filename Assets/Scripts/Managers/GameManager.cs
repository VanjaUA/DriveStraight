using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct MinMax<T>
{
   public T min;
   public T max;
}

public enum Direction
{
    Right,
    Left,
}

public class GameManager : MonoBehaviour
{

    private const int GAME_SCENE_INDEX = 1;
    private const int MAIN_MENU_INDEX = 0;

    public static GameManager instance;

    [SerializeField] public CarSpawner carSpawner;
    [SerializeField] public RoadManager roadManager;
    [SerializeField] public UIManager uiManager;
    [SerializeField] public Player player;

    [Header("Music")]
    [SerializeField] SoundManager.Sound[] gameMusic;

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



        Time.timeScale = 1f;

    }

    private void Start()
    {
        SoundManager.instance.PlayMusic(gameMusic);
    }


    private int coinsTotal;

    public int CoinsTotal
    {
        get { return coinsTotal; }
        private set
        {
            coinsTotal = value;
        }
    }


    public void GameOver() 
    {
        uiManager.UpdateTotalCoinsText(player.CoinsTaken);

        Time.timeScale = 0f;
        uiManager.ActivateLoseScreen();
    }

    public void LoadMenu() 
    {
        SceneManager.LoadScene(MAIN_MENU_INDEX);
    }

    public void LoadGame() 
    {
        SceneManager.LoadScene(GAME_SCENE_INDEX);
    }
}
