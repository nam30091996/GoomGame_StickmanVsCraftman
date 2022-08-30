using System;
using UnityEngine;

public class UIController : MonoBehaviour
{
    private PlayerController playerController;

    private void Awake()
    {
        playerController = PlayerController.Instance;
    }

    public void OnJumpClick()
    {
        playerController.DoJump();
    }

    public void OnSpeedClick()
    {
        playerController.DoSpeed();
    }

    public void OnAttackClick()
    {
        playerController.DoAttack();
    }

    public void OnChangeWearponClick(string weapon)
    {
        Weapon newWeapon;
        Weapon.TryParse(weapon, false, out newWeapon);
        playerController.GetComponent<SkinCombined>().weapon = newWeapon;
    }
}
