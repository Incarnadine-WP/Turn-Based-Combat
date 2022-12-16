using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _fireBall;

    [SerializeField] private Transform _playerPlace;
    [SerializeField] private Transform _enemyPlace;

    [SerializeField] private BattleHUD _playerHUD;
    [SerializeField] private BattleHUD _enemyHUD;

    [SerializeField] private Text _dialogueText;
    [SerializeField] private PopDamage _popText;

    private PopDamage _popDamage;
    private Unit _playerUnit;
    private Unit _enemyUnit;
    private BattleState _state;
    private float _delayText = 2f;
    private float _delayTextFast = 1f;
    private float _delayAction = 0;


    private void Start()
    {
        _state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    private void Update()
    {
        _delayAction += Time.deltaTime;
    }

    private IEnumerator SetupBattle()
    {
        GameObject player = Instantiate(_playerPrefab, _playerPlace);
        _playerUnit = player.GetComponent<Unit>();

        GameObject enemy = Instantiate(_enemyPrefab, _enemyPlace);
        _enemyUnit = enemy.GetComponent<Unit>();

        _dialogueText.text = "Prepare to fight with - " + _enemyUnit.unitName;

        _playerHUD.SetHUD(_playerUnit);
        _enemyHUD.SetHUD(_enemyUnit);

        yield return new WaitForSeconds(_delayText);

        _state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    private IEnumerator PlayerAttack()
    {
        yield return new WaitForSeconds(_delayTextFast);

        bool isDead = _enemyUnit.TakeDamage(_playerUnit.damage);
        StartCoroutine(_popText.PopTextDMG());

        _enemyHUD.SetHP(_enemyUnit.currentHP);
        _dialogueText.text = "You deal to " + _enemyUnit.unitName + " " + _playerUnit.damage + " damage!";

        yield return new WaitForSeconds(_delayText);

        if (isDead)
        {
            _state = BattleState.WON;
            EndBattle();
        }
        else
        {
            _state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    private IEnumerator PlayerHeal()
    {
        _playerUnit.Heal(5);

        _playerHUD.SetHP(_playerUnit.currentHP);
        _dialogueText.text = "You healed 5 points";

        yield return new WaitForSeconds(_delayText);

        _state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    private IEnumerator EnemyTurn()
    {
        _dialogueText.text = _enemyUnit.name + " attack you on " + _enemyUnit.damage + " damage.";

        yield return new WaitForSeconds(_delayTextFast);

        bool isDead = _playerUnit.TakeDamage(_enemyUnit.damage);

        _playerHUD.SetHP(_playerUnit.currentHP);

        yield return new WaitForSeconds(_delayTextFast);

        if (isDead)
        {
            _state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            _state = BattleState.PLAYERTURN;
            PlayerTurn();
        }

    }

    private void PlayerTurn()
    {
        _dialogueText.text = "Choose an action";
    }

    private void EndBattle()
    {
        if (_state == BattleState.WON)
        {
            _dialogueText.text = "Victory!";
        }
        else if (_state == BattleState.LOST)
        {
            _dialogueText.text = "Defeat...";
        }
    }

    public void OnAttackButton()
    {
        if (_state != BattleState.PLAYERTURN)
            return;

        if (_delayAction >= 3)
        {
            _delayAction = 0f;

            GameObject fireBall = Instantiate(_fireBall, _playerPlace);
            _popDamage = fireBall.GetComponent<PopDamage>();
            Destroy(fireBall, 1.1f);

            _popDamage.AttackTweenPlayer();

            StartCoroutine(PlayerAttack());
        }
    }

    public void OnHealButton()
    {
        if (_state != BattleState.PLAYERTURN)
            return;

        if (_delayAction >= 3f)
        {
            _delayAction = 0f;
            StartCoroutine(PlayerHeal());
        }
    }
}
