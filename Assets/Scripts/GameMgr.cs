using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour
{
    [Header("Lists")]
    [Tooltip("�P�ժ��ƻs�M��")]
    [SerializeField] public List<CardScriptableObjects> BulletList;
    [Tooltip("��P���Ϥ��M��")]
    [SerializeField] public List<Image> BulletImages;
    [Tooltip("�P�դ��o��A��P���M��")]
    [SerializeField] public List<CardScriptableObjects> GunBullet;
    [Header("Times")]
    [Tooltip("�ɶ��t�v�A1�����`�y�t")]
    [SerializeField] private float timeSlowDownFactor = 0.05f;
    [Tooltip("�ɶ���_(�Ϊ���Ȱ�)���ɪ�")]
    [SerializeField] private float timeSlowDownLength = 2f;
    [Header("Player")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] public int healthPoint;
    [Tooltip("�]�O���A���F�|�^�R��P")]
    [SerializeField] private Slider MagicSlider;
    [SerializeField] private float MagicPoint;
    [SerializeField] private float maxMagicPoint;
    [Tooltip("�R�ߪ��C������")]
    [SerializeField] private GameObject[] healthPointObj;
    //public Queue<List<Image>> im = new Queue<List<Image>>();
    private bool isDirty;
    // Start is called before the first frame update
    void Start()
    {
        //�]�w�̰��]�O��
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
        //0�H�U���έp��
        if (Time.timeScale > 0f)
            Time.timeScale -= (1f / timeSlowDownLength) * Time.unscaledDeltaTime;
        //����ƭ�
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);

        if (Input.GetButtonDown("Fire1"))
            NullTheFirstCard();

        MagicValue();
        WalkTime();

        //��s
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
                    // �ϥΤU�@�� Image �� sprite ��s��e Image
                    BulletImages[i].sprite = BulletImages[i + 1].sprite;
                }
            }

            // �̫�@�� Image �ϥίS�w�޿�ιw�]�Ȩӧ�s
            if (BulletImages[BulletImages.Count - 1] != null)
            {
                BulletImages[BulletImages.Count - 1].sprite = null;
            }
            isDirty = false;
        }
    }
    #region �ɶ�����
    /// <summary>
    /// �C�ʧ@
    /// </summary>
    public void DoSlowMotion()
    {
        Time.timeScale = timeSlowDownFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f;
    }
    /// <summary>
    /// ���ʫh�ɶ��^�_�y�t�A�����ʫh�v�����w�ܼȰ�
    /// </summary>
    private void WalkTime()
    {
        //Ū����J
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        if (x != 0 || z != 0)
        {
            //���ప��1
            Time.timeScale = Mathf.Clamp(Mathf.Sqrt(x * x + z * z), 0f, 1f);
            Time.fixedDeltaTime = Time.timeScale * .02f;
        }
    }
    #endregion
    #region �]�O����
    /// <summary>
    /// �]�O���A�Y�O���F�N��d
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
    #region �P��/��P����
    /// <summary>
    /// ���ŲĤ@�o�l�u�A�ñN�l�u������첾�A�P�ɰ�UI��s
    /// </summary>
    public void NullTheFirstCard()
    {
        BulletImages[0].sprite = null;
        //BulletImages.Add(im.Dequeue()[0]);
        //BulletImages.RemoveAt(0);
        isDirty = true;
    }
    /// <summary>
    /// �ɺ��A����P�A�ç�d�P�P�B��UI�W
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
    #region �ͩR�Ȭ���
    /// <summary>
    /// ��s�ͩR�ȹϤ�
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
