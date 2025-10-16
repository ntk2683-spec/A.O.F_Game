using UnityEngine;

[CreateAssetMenu(menuName = "Data/Character")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public Sprite portrait;
    public float baseHP;
    public float baseAttack;
    public float baseDefense;
    public float baseMoveSpeed;
    public GameObject modelPrefab;
}
