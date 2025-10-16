//using UnityEditor.U2D.Animation;
using UnityEngine;

public class PlayerStats
{
    public float maxHP;
    public float currentHP;
    public float attack;
    public float defense;
    public float moveSpeed;

    public void Initialize(CharacterData data)
    {
        maxHP = data.baseHP;
        currentHP = maxHP;
        attack = data.baseAttack;
        defense = data.baseDefense;
        moveSpeed = data.baseMoveSpeed;
    }
}
