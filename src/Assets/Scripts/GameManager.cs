using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    //�G�̃^�C�v
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

    //���݂̓G�̃^�C�v�̕ێ�
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

        //spawnInterval(Inspector�Őݒ�)�b���ƂɓG�𐶐�
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
                        //Score��1000���z������A�G�̐������~�߂�
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

            //�G����_���[�W���󂯂���A���h��Ɛԓ_��
            if (!isClear)
            {
                RedOut();
            }
        }
    }

    //SetUp�p�֐�
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

    //�ʏ�̓G���o��������֐�
    void SpawnEnemy()
    {
        if (!isGameOver)
        {
            // ���݂̃C���f�b�N�X�ʒu�ɓG�𐶐�
            Instantiate(enemyPrefab, spawnPoints[currentSpawnIndex].position, Quaternion.identity);

            // �C���f�b�N�X�����ɐi�߂�
            currentSpawnIndex += direction;

            // �X�|�[���|�C���g�̒[�ɓ��B�����ꍇ�A�i�s�����𔽓]
            if (currentSpawnIndex >= spawnPoints.Length)
            {
                currentSpawnIndex = spawnPoints.Length - 1;
                direction = -1;  // �������ɐi�s
            }
            else if (currentSpawnIndex < 0)
            {
                currentSpawnIndex = 0;
                direction = 1;  // �E�����ɐi�s
            }
        }
    }

    //�~�`�ɒe�����G���o��������֐�
    void SpawnCircleEnemy()
    {
        if (!isGameOver)
        {
            circleEnemy = Instantiate(CircleEnemyPrefab, spawnCircleEnemyPoint.position, Quaternion.identity);
        }
    }

    //�{�X�G�̃Q�[�W������
    void CircleEnemyGaugeSetUp()
    {
        sliderCircleEnemyHP.maxValue = circleEnemyMaxHP;
        sliderCircleEnemyHP.value = circleEnemyMaxHP;
        sliderCircleEnemyHP.gameObject.SetActive(true);
    }

    //���݂̓G�̃^�C�v��Ԃ�
    public EnemyType EnemyTypeReference()
    {
        return enemyType;
    }

    //�������ꂽ�~�e���̓G�i�{�X�j���A����̈ʒu�i���ړ����n�߂�ʒu�j�ɕt�������ǂ���
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

    //GameOver���ɌĂяo�����
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

    //ReStart�{�^���������ꂽ�Ƃ��ɌĂяo�����
    public void ReStartGame()
    {
        Time.timeScale = 1;
        SetUp();
        player.gameObject.SetActive(true);
        
        InvokeRepeating("SpawnEnemy", 1.0f, 0.5f);   
    }

    //�N���A���ɌĂяo�����
    void Clear()
    {
        inGameUI.SetActive(false);
        sliderCircleEnemyHP.gameObject.SetActive(false);
        gameClearUI.SetActive(true);
    }

    //�v���C���[��HP�����炷
    public void TakeDamage(int damage)
    {
        HP -= damage;
        sliderHP.value = HP;
        flag = 1;
    }

    //�~�`�e���̓G��HP�����炷
    public void TakeDamageEnemy(int damage)
    {
        circleEnemyHP -= damage;
        sliderCircleEnemyHP.value = circleEnemyHP;
    }

    //�X�R�AUI���X�V����
    public void UpdateScoreText(int score)
    {
        Score += 100;
        scoreText.SetText("Score:{0}", Score);
    }

    //�v���C���[���_���[�W���󂯂��Ƃ��̉�ʌ���
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
