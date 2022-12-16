using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class PopDamage : MonoBehaviour
{
    [SerializeField] private Transform _target;

    public void AttackTweenPlayer()
    {
        transform.DOMove(_target.position, 1f);
    }

    public IEnumerator PopTextDMG()
    {
        Vector3 startPosition = transform.position;

        gameObject.SetActive(true);
        transform.DOMove(_target.position, 1f);

        yield return new WaitForSeconds(1.2f);
        gameObject.SetActive(false);
        transform.DOMove(startPosition,1);
    }

}
