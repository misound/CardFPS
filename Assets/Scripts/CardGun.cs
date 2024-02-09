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
        //�Ͳ��l�u(�d�P)
        GameObject temp = Instantiate(Bullet);
        //�]�w�Ͳ������פΦ�m
        temp.transform.position = FirePoint.position;
        temp.transform.localRotation = FirePoint.rotation;
        //Ū���l�u�����Ĥ@�i�d�P
        temp.GetComponent<Card>().cardScripts = GameMgr.GunBullet[0];
        //�w�g�o�X�l�u�F
        GameMgr.GunBullet.Add(null);
        //�����o�X���l�u
        GameMgr.GunBullet.RemoveAt(0);
        //�ɶ��Ȯɦ^�y
        GameMgr.DoSlowMotion();
        //�H����P����
        audioSource.PlayOneShot(clip[rand.Next(0, clip.Length)]);
        /* �ϥήg�u���P�w�覡
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
