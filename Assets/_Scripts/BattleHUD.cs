using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    [SerializeField] private Text _nametext;
    [SerializeField] private Text _levelText;
    [SerializeField] private Slider _hpSlider;

    public void SetHUDEnemy(UnitEnemy unit)
    {
        _nametext.text = unit.unitName;
        _levelText.text = "Level: " +unit.unitLvl;
        _hpSlider.maxValue = unit.maxHP;
        _hpSlider.value = unit.currentHP;
    }

    public void SetHUDPlayer(UnitPlayer unit)
    {
        _nametext.text = unit.unitName;
        _levelText.text = "Level: " + unit.unitLvl;
        _hpSlider.maxValue = unit.maxHP;
        _hpSlider.value = unit.currentHP;
    }

    public void SetHP(int hp)
    {
        _hpSlider.value = hp;
    }
} 
