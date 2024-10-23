using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    //敵のタイプ
    public enum EnemyType { normal,circle}

    public static GameManager Instance = null;

    public PlayerController player;
    public EnemyBulletManager enemyBulletManager;

    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject CircleEnemyPrefab;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject gameClearUI;
    [SerializeField] private GameObject inGameUI;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Slider sliderHP;
    [SerializeField] private Slider sliderCircleEnemyHP;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform spawnCircleEnemyPoint;
    [SerializeField] private float spawnInterval = 0.1f;
    [SerializeField] private GameObject redOutWindow;

    GameObject cameraObj;

    int HP = 200;
    int maxHP = 200;
    int Score = 0;
    int border = 400;
    int flag = 0;

    int circleEnemyHP = 10;
    int circleEnemyMaxHP = 10;

    private int currentSpawnIndex = 0;
    private int direction = 1;


    public bool isArrived = false;
    private bool isGameOver = false;
    private bool isNextEnemy = false;
    private bool isClear = false;

    //現在の敵のタイプの保持
    EnemyType enemyType;

    GameObject circleEnemy;

    Vector2 goalPosition_ = new Vector2 (0, 8);

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        cameraObj = GameObject.Find("Main Camera");
        SetUp();

        //spawnInterval(Inspectorで設定)秒ごとに敵を生成
        InvokeRepeating("SpawnEnemy", 1.0f, spawnInterval);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver || isClear) 
        {
            if (enemyType == EnemyType.normal)
            {
                if (Score >= border)
                {
                    if (!isNextEnemy)
                    {
                        //Scoreが1000を越えたら、敵の生成を止める
                        CancelInvoke("SpawnEnemy");
                        Invoke("SpawnCircleEnemy", 5.0f);
                        enemyType = EnemyType.circle;
                        isNextEnemy = true;
                    }
                }
            }
            else if (enemyType == EnemyType.circle)
            {
                if (circleEnemyHP <= 0)
                {
                    Destroy(circleEnemy);
                    Clear();
                    isClear = true;
                }

                if (circleEnemy != null)
                {
                    if (IsCircleEnemyArrived(circleEnemy.transform.position) && !isArrived) 
                    {
                        isArrived = true;
                        CircleEnemyGaugeSetUp();
                    }
                }                
            }

            if (HP <= 0)
            {
                RetryGame();
            }

            //敵からダメージを受けたら、横揺れと赤点滅
            if (!isClear)
            {
                RedOut();
            }
        }
    }

    //SetUp用関数
    void SetUp()
    {
        isGameOver = false;
        gameOverUI.SetActive(false);
        gameClearUI.SetActive(false);
        inGameUI.SetActive(true); 
        redOutWindow.SetActive(false);
        sliderCircleEnemyHP.gameObject.SetActive(false);
        HP = maxHP;
        circleEnemyHP = circleEnemyMaxHP;
        Score = 0;
        enemyType = EnemyType.normal;
        sliderHP.maxValue = HP;
        sliderHP.value = HP;       
        scoreText.SetText("Score:0");
    }

    //通常の敵を出現させる関数
    void SpawnEnemy()
    {
        if (!isGameOver)
        {
            // 現在のインデックス位置に敵を生成
            Instantiate(enemyPrefab, spawnPoints[currentSpawnIndex].position, Quaternion.identity);

            // インデックスを次に進める
            currentSpawnIndex += direction;

            // スポーンポイントの端に到達した場合、進行方向を反転
            if (currentSpawnIndex >= spawnPoints.Length)
            {
                currentSpawnIndex = spawnPoints.Length - 1;
                direction = -1;  // 左方向に進行
            }
            else if (currentSpawnIndex < 0)
            {
                currentSpawnIndex = 0;
                direction = 1;  // 右方向に進行
            }
        }
    }

    //円形に弾を撃つ敵を出現させる関数
    void SpawnCircleEnemy()
    {
        if (!isGameOver)
        {
            circleEnemy = Instantiate(CircleEnemyPrefab, spawnCircleEnemyPoint.position, Quaternion.identity);
        }
    }

    //ボス敵のゲージ初期化
    void CircleEnemyGaugeSetUp()
    {
        sliderCircleEnemyHP.maxValue = circleEnemyMaxHP;
        sliderCircleEnemyHP.value = circleEnemyMaxHP;
        sliderCircleEnemyHP.gameObject.SetActive(true);
    }

    //現在の敵のタイプを返す
    public EnemyType EnemyTypeReference()
    {
        return enemyType;
    }

    //生成された円弾幕の敵（ボス）が、所定の位置（横移動を始める位置）に付いたかどうか
    bool IsCircleEnemyArrived(Vector2 circleEnemyPosition)
    {
        if (circleEnemyPosition != null)
        {
            if (goalPosition_.y >= circleEnemyPosition.y)
            {
                return true;
            }

            return false;
        }
        else
        {
            return false;
        }   
    }

    //GameOver時に呼び出される
    void RetryGame()
    {
        isGameOver = true;
        if (Score < border)
        {
            CancelInvoke("SpawnEnemy");
        }
        Time.timeScale = 3;
        Debug.Log("GameOver!");
        player.gameObject.SetActive(false);
        inGameUI.SetActive(false);
        sliderCircleEnemyHP.gameObject.SetActive(false);
        gameOverUI.SetActive(true);
    }

    //ReStartボタンが押されたときに呼び出される
    public void ReStartGame()
    {
        Time.timeScale = 1;
        SetUp();
        player.gameObject.SetActive(true);
        
        InvokeRepeating("SpawnEnemy", 1.0f, 0.5f);   
    }

    //クリア時に呼び出される
    void Clear()
    {
        inGameUI.SetActive(false);
        sliderCircleEnemyHP.gameObject.SetActive(false);
        gameClearUI.SetActive(true);
    }

    //プレイヤーのHPを減らす
    public void TakeDamage(int damage)
    {
        HP -= damage;
        sliderHP.value = HP;
        flag = 1;
    }

    //円形弾幕の敵のHPを減らす
    public void TakeDamageEnemy(int damage)
    {
        circleEnemyHP -= damage;
        sliderCircleEnemyHP.value = circleEnemyHP;
    }

    //スコアUIを更新する
    public void UpdateScoreText(int score)
    {
        Score += 100;
        scoreText.SetText("Score:{0}", Score);
    }

    //プレイヤーがダメージを受けたときの画面効果
    void RedOut()
    {
        switch (flag)
        {
            case 1:
                redOutWindow.SetActive(true);
                goto case 3;
            case 3:
                cameraObj.transform.Translate(30 * Time.deltaTime, 0, 0);
                if (cameraObj.transform.position.x >= 1.0f)
                    flag++;
                break;
            case 2:
                cameraObj.transform.Translate(-30 * Time.deltaTime, 0, 0);
                if (cameraObj.transform.position.x <= -1.0f)
                    flag++;
                break;
            case 4:
                cameraObj.transform.Translate(-30 * Time.deltaTime, 0, 0);
                if (cameraObj.transform.position.x <= 0)
                {
                    flag = 0;
                    redOutWindow.SetActive(false);
                }
                break;
        }
    }
}
