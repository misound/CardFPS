using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class CardScriptableObjects : ScriptableObject
{
    [Header("Data")]
    [SerializeField] public string Cardname;
    [SerializeField] public string Description;
    [SerializeField] public Material Artwork;
    [SerializeField] public Sprite ArtworkForUI;
    [SerializeField] public int _cardAtk;
    [SerializeField] public AudioClip _audioClip;
}
