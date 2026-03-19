using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int maxHP = 100;
    [SerializeField] private int maxMP = 50;
    [SerializeField] private int attack = 10;
    [SerializeField] private int defense = 5;
    [SerializeField] private float speed = 3f;

    public int CurrentHP { get; private set; }
    public int CurrentMP { get; private set; }
    public int ATK => attack;
    public int DEF => defense;
    public float SPD => speed;
}