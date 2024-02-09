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
        //�줸��
        player = FindObjectOfType<PlayerController>();
        gameMgr = FindObjectOfType<GameMgr>();
        //��r��l��
        fullHp = MaxHealth.ToString();
        hpValue = health.ToString();
        //�̤j�ͩR��
        SetMaxHealth(MaxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        //���ʳt���H�ɶ��v���[�֦̰ܳ��t��
        moveSpeed += Time.deltaTime;
        moveSpeed = Mathf.Clamp(moveSpeed,0, maxMoveSpeed);
        //�w�즨�u�|�����a�����Ǫ�
        transform.LookAt(player.gameObject.transform);
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        //����@�߹�H��v��
        healthBar.transform.LookAt(transform.position + player.transform.forward);
        hpText.transform.LookAt(transform.position + player.transform.forward);
        //��q��r�H�ɧ�s
        hpText.text = HealthText(fullHp, hpValue);
        //rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, moveSpeed * Time.deltaTime);
    }
    #region �ͩR�Ȭ���
    /// <summary>
    /// �ͩR�����ˮ`�ӷ��A�P�ɳB�z���᪺func
    /// </summary>
    /// <param name="Amount">�ˮ`�ӷ�</param>
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
    /// ���`�ɼ���ɤl�A�����ɤl���d�b��a
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
    /// �]�w�̰��ͩR��
    /// </summary>
    /// <param name="health">�̰��ͩR��</param>
    public void SetMaxHealth(int health)
    {
        healthBar.maxValue = health;
        healthBar.value = health;

        fill.color = BarGradient.Evaluate(1f);
    }
    /// <summary>
    /// ��s��q��
    /// </summary>
    /// <param name="health">�ͩR��</param>
    public void SetHealth(int health)
    {
        healthBar.value = health;
        //�H��q���C�ӽվ��C��
        fill.color = BarGradient.Evaluate(healthBar.value);
    }
    /// <summary>
    /// ��s��q���W����r
    /// </summary>
    /// <param name="Max">�̰���q</param>
    /// <param name="value">��e��q</param>
    /// <returns></returns>
    private string HealthText(string Max, string value)
    {
        string result = $"{value} / {Max}";

        return result;
    }
    #endregion
}
