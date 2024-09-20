using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "TurretLockConfig")]
public class TurretLockConfig : ScriptableObject
{
    public List<ItemLock> ATK;

    public List<ItemLock> DEFENSE;

    public List<ItemLock> RESOURCE;

    public List<ItemLock> ALLIES;
}

[System.Serializable]
public class ItemLock
{
    public List<string> attSet;

    public int unlockPrice;


}
