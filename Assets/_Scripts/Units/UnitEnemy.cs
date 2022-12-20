using UnityEngine;
using DG.Tweening;

public class UnitEnemy : Unit
{
    [SerializeField] private Transform _targetPlayer;
    [SerializeField] private Animator _animator;

        public void MoveToAttack()
    {
        _animator.SetTrigger("Move");
        transform.DOMove(_targetPlayer.position, 1f).SetLoops(2, LoopType.Yoyo);
    }
}
