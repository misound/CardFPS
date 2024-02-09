using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] public CardScriptableObjects cardScripts;
    [SerializeField] private Material material;
    [SerializeField] private int Atk;
    [SerializeField] private string CardName;
    [SerializeField] private TrailRenderer trilRenderer;
    [SerializeField] private ParticleSystem boom;
    [SerializeField] private float boomRange;
    [SerializeField] private LayerMask target;
    [SerializeField] private AudioClip clip;
    [SerializeField] private AudioSource audioSource;
    private void Awake()
    {
        trilRenderer = GetComponent<TrailRenderer>();
        audioSource = GetComponent<AudioSource>();
    }
    void Start()
    {
        carPro(cardScripts);
        GetComponent<MeshRenderer>().material = material;
        Destroy(gameObject,2f);
    }

    void carPro(CardScriptableObjects card)
    {
        material = card.Artwork;
        Atk = card._cardAtk;
        CardName = card.Cardname;
        clip = card._audioClip;
    }
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
