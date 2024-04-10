using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public static Damage instance;
    public Player _player;
    private bool isInvincible = false;
    private Animator animator;


    private void Awake()
    {
        instance = this;
    }

    public Animator Animator { set =>  animator = value; }

    public bool IsInvincible { get { return isInvincible; } set => isInvincible = value; }

    public void DamagePlayer(int amount)
    {
        animator.SetBool("isHurted", true);
        _player._hp -= amount;
        _player.Knock();
    }

    public IEnumerator Invincibility()
    {
        yield return new WaitForSeconds(1.5f);
        isInvincible = false;
        animator.SetBool("isHurted", false);
    }
}
