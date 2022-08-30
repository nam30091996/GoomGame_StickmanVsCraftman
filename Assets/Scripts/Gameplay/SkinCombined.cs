using Spine;
using Spine.Unity;
using UnityEngine;

public class SkinCombined : MonoBehaviour
{
    public Weapon weapon;
    private Weapon lastWeapon;
    private void Start()
    {
        lastWeapon = weapon;
        UpdateSkin();
    }

    private void Update()
    {
        if (weapon != lastWeapon)
        {
            UpdateSkin();
            lastWeapon = weapon;
        }
    }

    public void UpdateSkin()
    {
        var skeleton = GetComponent<SkeletonMecanim>().Skeleton;
        var skeletonData = skeleton.Data;
        var mixAndMatchSkin = new Skin("new skin");
        mixAndMatchSkin.AddSkin(skeletonData.FindSkin("nv_1"));
        switch (weapon)
        {
            case Weapon.NONE:
                break;
            case Weapon.SWORD:
                mixAndMatchSkin.AddSkin(skeletonData.FindSkin("kiem"));
                break;
            case Weapon.BOW:
                mixAndMatchSkin.AddSkin(skeletonData.FindSkin("cung"));
                break;
        }

        skeleton.SetSkin(mixAndMatchSkin);
        skeleton.SetSlotsToSetupPose();
    }
}

public enum Weapon
{
    NONE,
    SWORD,
    BOW
}
