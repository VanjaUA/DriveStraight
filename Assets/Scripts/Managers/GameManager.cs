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
    public const string EQUIPPED_CAR = "EquippedCar";
    public const string COINS_COUNT = "CoinsCount";

    private const int GAME_SCENE_INDEX = 1;
    private const int MAIN_MENU_INDEX = 0;

    public static GameManager instance;

    [SerializeField] public CarSpawner carSpawner;
    [SerializeField] public RoadManager roadManager;
    [SerializeField] public UIManager uiManager;

    [SerializeField] private PlayerCarSO[] playerCars;
    public Player Player { get; private set; }


    private float fastestCarMaxSpeed;

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

        int selectedCarIndex = PlayerPrefs.GetInt(EQUIPPED_CAR, 0);
        Player = Instantiate(playerCars[selectedCarIndex].carObject, Vector3.zero, Quaternion.identity).GetComponent<Player>();

        fastestCarMaxSpeed = playerCars[playerCars.Length - 1].carObject.GetComponent<Player>().GetMovementSpeedBounds().max;
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
        uiManager.UpdateTotalCoinsText(Player.CoinsTaken);

        int oldCoinsCount = PlayerPrefs.GetInt(COINS_COUNT,0);
        Debug.Log(oldCoinsCount);
        int newCoinsCount = oldCoinsCount + Player.CoinsTaken;
        Debug.Log(newCoinsCount);
        PlayerPrefs.SetInt(COINS_COUNT,newCoinsCount);

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

    public float GetFastestCarMaxSpeed() 
    {
        return fastestCarMaxSpeed;
    }
}
