using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    [Header("HealthBar")]
    [SerializeField] private int MaxHealth = 50;
    [SerializeField] private int health = 50;
    [SerializeField] private Slider healthBar;
    public Gradient BarGradient;
    [SerializeField] private Image fill;
    [SerializeField] private Text hpText;
    [SerializeField] private string fullHp;
    [SerializeField] private string hpValue;
    [Header("Movement")]
    [SerializeField] public float moveSpeed = 10f;
    [SerializeField] private float maxMoveSpeed = 10f;
    [Header("Components")]
    [SerializeField] private PlayerController player;
    [SerializeField] private GameMgr gameMgr;
    [SerializeField] private ParticleSystem[] systems;
    void Start()
    {
        //抓元件
        player = FindObjectOfType<PlayerController>();
        gameMgr = FindObjectOfType<GameMgr>();
        //文字初始化
        fullHp = MaxHealth.ToString();
        hpValue = health.ToString();
        //最大生命值
        SetMaxHealth(MaxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        //移動速度隨時間逐漸加快至最高速度
        moveSpeed += Time.deltaTime;
        moveSpeed = Mathf.Clamp(moveSpeed,0, maxMoveSpeed);
        //定位成只會往玩家走的怪物
        transform.LookAt(player.gameObject.transform);
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        //血條一律對象攝影機
        healthBar.transform.LookAt(transform.position + player.transform.forward);
        hpText.transform.LookAt(transform.position + player.transform.forward);
        //血量文字隨時更新
        hpText.text = HealthText(fullHp, hpValue);
        //rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, moveSpeed * Time.deltaTime);
    }
    #region 生命值相關
    /// <summary>
    /// 生命接收傷害來源，同時處理死後的func
    /// </summary>
    /// <param name="Amount">傷害來源</param>
    public void TakeDmg(int Amount)
    {
        health -= Amount;
        SetHealth(health);
        fullHp = MaxHealth.ToString();
        hpValue = health.ToString();
        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }
    /// <summary>
    /// 死亡時撥放粒子，並讓粒子停留在原地
    /// </summary>
    private void Die()
    {
        for(int i = 0; i < systems.Length; i++)
        {
            systems[i].Play();
            systems[i].transform.parent = null;
        }

        Destroy(gameObject);
    }
    /// <summary>
    /// 設定最高生命值
    /// </summary>
    /// <param name="health">最高生命值</param>
    public void SetMaxHealth(int health)
    {
        healthBar.maxValue = health;
        healthBar.value = health;

        fill.color = BarGradient.Evaluate(1f);
    }
    /// <summary>
    /// 刷新血量條
    /// </summary>
    /// <param name="health">生命值</param>
    public void SetHealth(int health)
    {
        healthBar.value = health;
        //隨血量高低而調整顏色
        fill.color = BarGradient.Evaluate(healthBar.value);
    }
    /// <summary>
    /// 刷新血量條上的文字
    /// </summary>
    /// <param name="Max">最高血量</param>
    /// <param name="value">當前血量</param>
    /// <returns></returns>
    private string HealthText(string Max, string value)
    {
        string result = $"{value} / {Max}";

        return result;
    }
    #endregion
}
