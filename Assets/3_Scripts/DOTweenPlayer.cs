using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOTweenPlayer : MonoBehaviour
{
    [SerializeField]
    Ease ease;
    [SerializeField]
    Vector3 toPosition;
    [SerializeField]
    float duration;
    [SerializeField]
    bool punch;
    [SerializeField]
    LoopType loopType = LoopType.Restart;

    private void Start()
    {
        
        if (!punch)
        {
            transform.DOLocalMove(toPosition, duration).SetEase(ease).SetLoops(-1, loopType);
        }
        else
        {
            transform.DOPunchPosition(toPosition, duration, 1, 1).SetEase(ease).SetLoops(-1, loopType);
        }

            
    }

}
