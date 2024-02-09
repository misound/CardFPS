using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Card : MonoBehaviour
{
    [Header("Card Properties")]
    [SerializeField] public CardScriptableObjects cardScripts;
    [SerializeField] private Material material;
    [SerializeField] private int Atk;
    [SerializeField] private string CardName;
    [SerializeField] private AudioClip clip;
    [Header("Explosion Card Effect")]
    [SerializeField] private ParticleSystem boom;
    [SerializeField] private float boomRange;
    [Tooltip("通常是敵人")]
    [SerializeField] private LayerMask target;
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;

    private TrailRenderer trilRenderer;
    private void Awake()
    {
        trilRenderer = GetComponent<TrailRenderer>();
        audioSource = GetComponent<AudioSource>();
    }
    void Start()
    {
        carPro(cardScripts);
        GetComponent<MeshRenderer>().material = material;
        //省資源
        Destroy(gameObject,2f);
    }
    /// <summary>
    /// 載入分發到的資源
    /// </summary>
    /// <param name="card">經由GameMgr分發到的牌</param>
    void carPro(CardScriptableObjects card)
    {
        material = card.Artwork;
        Atk = card._cardAtk;
        CardName = card.Cardname;
        clip = card._audioClip;
    }
    /// <summary>
    /// 處理進敵人身體的邏輯
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Target>() != null)
        {
            other.gameObject.GetComponent<Target>().TakeDmg(Atk);
            audioSource.PlayOneShot(clip);

            if (CardName.ToLower().Contains("icy"))
            {
                other.gameObject.GetComponent<Target>().moveSpeed -= 8f;
            }
            if (CardName.ToLower().Contains("bomb"))
            {
                boom.Play();
                boom.gameObject.transform.parent = null;
                foreach (RaycastHit Hit in Physics.SphereCastAll(transform.position, boomRange, Vector3.back, 0f, target))
                {
                    Hit.collider.gameObject.GetComponent<Target>().TakeDmg(Atk);
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, boomRange);
    }
}
