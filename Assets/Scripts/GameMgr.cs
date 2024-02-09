using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour
{
    [Header("Lists")]
    [Tooltip("牌組的複製清單")]
    [SerializeField] public List<CardScriptableObjects> BulletList;
    [Tooltip("手牌的圖片清單")]
    [SerializeField] public List<Image> BulletImages;
    [Tooltip("牌組分發到你手牌的清單")]
    [SerializeField] public List<CardScriptableObjects> GunBullet;
    [Header("Times")]
    [Tooltip("時間速率，1為正常流速")]
    [SerializeField] private float timeSlowDownFactor = 0.05f;
    [Tooltip("時間恢復(或直到暫停)的時長")]
    [SerializeField] private float timeSlowDownLength = 2f;
    [Header("Player")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] public int healthPoint;
    [Tooltip("魔力條，滿了會回充手牌")]
    [SerializeField] private Slider MagicSlider;
    [SerializeField] private float MagicPoint;
    [SerializeField] private float maxMagicPoint;
    [Tooltip("愛心的遊戲物件")]
    [SerializeField] private GameObject[] healthPointObj;
    //public Queue<List<Image>> im = new Queue<List<Image>>();
    private bool isDirty;
    // Start is called before the first frame update
    void Start()
    {
        //設定最高魔力值
        MagicSlider.maxValue = maxMagicPoint;

        System.Random random = new System.Random();
        for (int i = 0; i < BulletImages.Count; i++)
        {
            GunBullet[i] = BulletList[random.Next(0, BulletList.Count)];
            BulletImages[i].sprite = GunBullet[i].ArtworkForUI;
        }
        /*
        foreach (Image image in BulletImages)
        {
            im.Enqueue(BulletImages);
            Debug.Log(image.name);
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        //0以下不用計算
        if (Time.timeScale > 0f)
            Time.timeScale -= (1f / timeSlowDownLength) * Time.unscaledDeltaTime;
        //限制數值
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);

        if (Input.GetButtonDown("Fire1"))
            NullTheFirstCard();

        MagicValue();
        WalkTime();

        //更新
        if (isDirty)
        {
            /*for (int i = 0; i < BulletImages.Count; i++)
            {
                if (BulletImages[i] != null)
                {
                    int nextIndex = (i + 1) % BulletImages.Count;
                    BulletImages[i].sprite = BulletImages[nextIndex].sprite;
                }
            }*/

            for (int i = 0; i < BulletImages.Count - 1; i++)
            {
                if (BulletImages[i + 1] != null)
                {
                    // 使用下一個 Image 的 sprite 更新當前 Image
                    BulletImages[i].sprite = BulletImages[i + 1].sprite;
                }
            }

            // 最後一個 Image 使用特定邏輯或預設值來更新
            if (BulletImages[BulletImages.Count - 1] != null)
            {
                BulletImages[BulletImages.Count - 1].sprite = null;
            }
            isDirty = false;
        }
    }
    #region 時間相關
    /// <summary>
    /// 慢動作
    /// </summary>
    public void DoSlowMotion()
    {
        Time.timeScale = timeSlowDownFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f;
    }
    /// <summary>
    /// 移動則時間回復流速，不移動則逐漸延緩至暫停
    /// </summary>
    private void WalkTime()
    {
        //讀取輸入
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        if (x != 0 || z != 0)
        {
            //不能高於1
            Time.timeScale = Mathf.Clamp(Mathf.Sqrt(x * x + z * z), 0f, 1f);
            Time.fixedDeltaTime = Time.timeScale * .02f;
        }
    }
    #endregion
    #region 魔力條等
    /// <summary>
    /// 魔力條，若是滿了就抽卡
    /// </summary>
    public void MagicValue()
    {
        MagicSlider.value = MagicPoint;
        MagicPoint += Time.deltaTime;

        if (MagicPoint >= maxMagicPoint)
        {
            //DoSlowMotion();
            DrawTheCard();
            MagicPoint = 0f;
        }
    }
    #endregion
    #region 牌組/手牌相關
    /// <summary>
    /// 做空第一發子彈，並將子彈輪輪轉位移，同時做UI更新
    /// </summary>
    public void NullTheFirstCard()
    {
        BulletImages[0].sprite = null;
        //BulletImages.Add(im.Dequeue()[0]);
        //BulletImages.RemoveAt(0);
        isDirty = true;
    }
    /// <summary>
    /// 補滿你的手牌，並把卡牌同步到UI上
    /// </summary>
    public void DrawTheCard()
    {
        System.Random random = new System.Random();
        for (int i = 0; i < BulletImages.Count; i++)
        {
            GunBullet[i] = BulletList[random.Next(0, BulletList.Count)];
            BulletImages[i].sprite = GunBullet[i].ArtworkForUI;
        }
    }
    #endregion
    #region 生命值相關
    /// <summary>
    /// 更新生命值圖片
    /// </summary>
    public void SetHP()
    {
        for (int i = 0; i < healthPointObj.Length; i++)
        {
            if (healthPoint > i)
            {
                healthPointObj[i].SetActive(true);
            }
            else
            {
                healthPointObj[i].SetActive(false);
            }

        }
    }
    #endregion
}
