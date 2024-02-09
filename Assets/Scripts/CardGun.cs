using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGun : MonoBehaviour
{
    [SerializeField] private int Damage = 10;
    [SerializeField] private float Range = 100;
    [SerializeField] private Camera cam;
    [SerializeField] private Transform FirePoint;
    [SerializeField] private Transform camTrans;
    [SerializeField] private Transform PlayerTrans;
    [SerializeField] private GameObject Bullet;
    [SerializeField] private GameMgr GameMgr;
    [SerializeField] private AudioClip[] clip;
    [SerializeField] private AudioSource audioSource;
    System.Random rand = new();
    void Start()
    {
        GameMgr = FindObjectOfType<GameMgr>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && GameMgr.GunBullet[0] != null)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        RaycastHit hit;
        //生產子彈(卡牌)
        GameObject temp = Instantiate(Bullet);
        //設定生產的角度及位置
        temp.transform.position = FirePoint.position;
        temp.transform.localRotation = FirePoint.rotation;
        //讀取子彈輪的第一張卡牌
        temp.GetComponent<Card>().cardScripts = GameMgr.GunBullet[0];
        //已經發出子彈了
        GameMgr.GunBullet.Add(null);
        //移除發出的子彈
        GameMgr.GunBullet.RemoveAt(0);
        //時間暫時回流
        GameMgr.DoSlowMotion();
        //隨機丟牌音效
        audioSource.PlayOneShot(clip[rand.Next(0, clip.Length)]);
        /* 使用射線的判定方式
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, Range))
        {
            Debug.Log(hit.transform.name);
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                //target.TakeDmg(Damage);
            }
        }
        */

    }
}
