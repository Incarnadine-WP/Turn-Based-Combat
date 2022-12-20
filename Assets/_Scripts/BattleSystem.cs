using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject[] _enemyPrefab;
    [SerializeField] private SpriteRenderer _playerSprite;

    [SerializeField] private Transform _playerPlace;
    [SerializeField] private Transform _enemyPlace;

    [SerializeField] private BattleHUD _playerHUD;
    [SerializeField] private BattleHUD _enemyHUD;

    [Header("Text")]
    [SerializeField] private Text _dialogueText;
    [SerializeField] private TextMeshProUGUI[] _damageText;
    [SerializeField] private PopDamage[] _popDamage;

    private UnitPlayer _playerUnit;
    private UnitEnemy _enemyUnit;
    private BattleState _state;
    private float _delayText = 2f;
    private float _delayTextFast = 1f;
    private float _delayAction = 0;

    private int _nextLvl = 0;
    private int _healPlayer = 30;


    private void Start()
    {
        _state = BattleState.START;
        StartCoroutine(SetupBattle(_enemyPrefab[0]));

        PlayerStart();
    }

    private void Update()
    {
        _delayAction += Time.deltaTime;
    }

    private void PlayerStart()
    {
        GameObject player = Instantiate(_playerPrefab, _playerPlace);
        _playerUnit = player.GetComponent<UnitPlayer>();
        _playerHUD.SetHUDPlayer(_playerUnit);
    }

    private IEnumerator SetupBattle(GameObject enemy)
    {
        enemy = Instantiate(enemy, _enemyPlace);
        _enemyUnit = enemy.GetComponent<UnitEnemy>();

        _dialogueText.text = "Prepare to fight with - " + _enemyUnit.unitName;

        _enemyHUD.SetHUDEnemy(_enemyUnit);

        yield return new WaitForSeconds(_delayText);

        _state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    private IEnumerator PlayerAttack()
    {
        yield return new WaitForSeconds(_delayTextFast);

        bool isDead = _enemyUnit.TakeDamage(_playerUnit.damage);

        StartCoroutine(_popDamage[1].PopTweenMove());
        _dialogueText.text = "You deal to " + _enemyUnit.unitName + " " + _playerUnit.damage + " damage!";
        _damageText[0].text = _playerUnit.damage.ToString();

        _enemyHUD.SetHP(_enemyUnit.currentHP);

        yield return new WaitForSeconds(_delayText);

        if (isDead && _nextLvl == 0)
        {
            _state = BattleState.PLAYERTURN;
            StartCoroutine(SetupBattle(_enemyPrefab[1]));
            _nextLvl++;
        }
        else if (isDead && _nextLvl == 1)
        {
            _state = BattleState.PLAYERTURN;
            StartCoroutine(SetupBattle(_enemyPrefab[2]));
            _nextLvl++;
        }
        else if (isDead && _nextLvl == 2)
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
        SpriteRenderer player = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();

        _playerUnit.Heal(_healPlayer);
        _playerHUD.SetHP(_playerUnit.currentHP);

        _dialogueText.text = "You healed " + _healPlayer + " points";

        player.DOColor(Color.green,1f).SetLoops(2, LoopType.Yoyo);

        yield return new WaitForSeconds(_delayText);

        _state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    private IEnumerator EnemyTurn()
    {
        _enemyUnit.MoveToAttack();
        yield return new WaitForSeconds(_delayTextFast);

        bool isDead = _playerUnit.TakeDamage(_enemyUnit.damage);

        StartCoroutine(_popDamage[2].PopTweenMove());
        _dialogueText.text = _enemyUnit.unitName + " attack you on " + _enemyUnit.damage + " damage.";
        _damageText[1].text = _enemyUnit.damage.ToString();

        _playerHUD.SetHP(_playerUnit.currentHP);

        yield return new WaitForSeconds(_delayText);

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
        _dialogueText.text = "Choose an action:";
    }

    public void EndBattle()
    {
        if (_state == BattleState.WON)
        {
            _dialogueText.text = "Victory!";
        }
        else if (_state == BattleState.LOST)
        {
            _dialogueText.text = "Defeat...";
            _gameOverPanel.SetActive(true);
        }
    }

    public void OnAttackButton()
    {
        if (_state != BattleState.PLAYERTURN)
            return;

        if (_delayAction >= 3)
        {
            _delayAction = 0f;

            StartCoroutine(_popDamage[0].PopTweenMove());

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

