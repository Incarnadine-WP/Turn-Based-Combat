using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class PopDamage : MonoBehaviour
{
    [SerializeField] private Transform _target;

    public IEnumerator PopTweenMove()
    {
        Vector3 startPosition = transform.position;

        gameObject.SetActive(true);
        transform.DOMove(_target.position, 1f);

        yield return new WaitForSeconds(0.9f);
        gameObject.SetActive(false);
        transform.DOMove(startPosition,1f);
    }

    public IEnumerator FadeObj(SpriteRenderer sprite)
    {
        sprite.DOFade(0, 2f);
        yield return new WaitForSeconds(2f);
        sprite.gameObject.SetActive(false);
    }

}
