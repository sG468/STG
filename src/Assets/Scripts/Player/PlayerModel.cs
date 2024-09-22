using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel
{
    public int HP { get; set; } //HP
    public int Score { get; set; } //得点
    public Vector2Int Position { get; set; } //プレイヤーの位置（整数値で管理）

    public PlayerModel() //初期化
    {
        HP = 100;
        Score = 0;
        Position = new Vector2Int(0, 0); // 初期位置
    }
}
