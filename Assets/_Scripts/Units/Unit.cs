using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;
    public int unitLvl;
    public int damage;
    public int maxHP;
    public int currentHP;

    private PopDamage _tweenScript;
    private SpriteRenderer _spriteUnit;

    private void Start()
    {
        _spriteUnit = GetComponent<SpriteRenderer>();
        _tweenScript = FindObjectOfType<PopDamage>().GetComponent<PopDamage>();
    }

    public bool TakeDamage(int dmg)
    {

        currentHP -= dmg;

        if (currentHP <= 0)
        {
            StartCoroutine(_tweenScript.FadeObj(_spriteUnit));
            return true;
        }

        else
            return false;
    }

    public void Heal(int amount)
    {
        currentHP += amount;
        if (currentHP >= maxHP)
            currentHP = maxHP;
    }

}
