//�l�u�A�o�g�[�t��
using UnityEngine;

public class CardBullet : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform GunPoint;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 100f, ForceMode.Impulse);
    }
}
