using UnityEngine;
using UnityEngine.EventSystems;

public class UIController : MonoBehaviour
{
    public PlayerController playerController;

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
