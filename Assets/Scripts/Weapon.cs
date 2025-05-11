public enum WeaponType
{
    pistol,
    autoRifle,
    shotgun,
    revolver,
    rifle
}

[System.Serializable]
public class Weapon
{
    public WeaponType weaponType;
    public int ammo;
    public int maxAmmo;
}
