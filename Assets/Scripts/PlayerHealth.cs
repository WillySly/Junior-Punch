using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : Health
{
    [SerializeField] float hitAnimationDelay = 0.3f;

    public override void gotHit(int points)
    {
        StartCoroutine(playAnimation());
        base.gotHit(points);
    }

    public IEnumerator playAnimation()
    {
        yield return new WaitForSeconds(hitAnimationDelay);
        base.animator.SetTrigger("isHurt");
    }

}
