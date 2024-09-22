using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerModel playerModel;
    private PlayerView playerView;
    private int moveSpeed = 1;

    public float getKeyIntervalTime;
    float getKeyTime;

    void Start()
    {
        playerModel = new PlayerModel();
        playerView = GetComponent<PlayerView>();
        getKeyTime = Time.time;
    }

    void Update()
    {
        HandleInput();
        UpdateView();
    }

    void HandleInput()
    {
        if (Time.time > (getKeyTime + getKeyIntervalTime)) //とりあえず上下左右移動を、同じ処理下で（分けない）
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                MoveUp();
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                MoveDown();
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                MoveLeft();
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                MoveRight();
            }
            getKeyTime = Time.time;
        }

        //if (Time.time > (getKeyLRTime + getKeyLRIntervalTime))
        //{
            
        //    getKeyLRTime = Time.time;
        //}
        //float x = Input.GetAxisRaw("Horizontal");
        //float y = Input.GetAxisRaw("Vertical");

        //playerModel.Position += new Vector2Int((int)x, (int)y);
     
    }

    void MoveLeft()
    {
        playerModel.Position += new Vector2Int(-moveSpeed, 0);
    }

    void MoveRight()
    {
        playerModel.Position += new Vector2Int(moveSpeed, 0);
    }

    void MoveUp()
    {
        playerModel.Position += new Vector2Int(0, moveSpeed);
    }

    void MoveDown()
    {
        playerModel.Position += new Vector2Int(0, -moveSpeed);
    }

    void UpdateView()
    {
        playerView.UpdatePosition(playerModel.Position);
    }
}
