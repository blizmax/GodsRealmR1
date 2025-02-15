using MyBox;
using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ChestStats", menuName = "Objects/Chest stats")]
public class ChestStats : ScriptableObject
{
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (InputString != string.Empty)
        {
            int[] a = InputString.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(n => Convert.ToInt32(n)).ToArray();

            Items = new RangedInt(a[0], a[1]);
            Gold = new RangedInt(a[2], a[3]);
            Potions = new RangedInt(a[4], a[5]);
        }
    }

    public string InputString;
#endif

    [Header("Info")]
    public Rarity ChestType;

    //�������� ���������� ���������
    [Header("Items")]
    public RangedInt Items;

    [Header("Gold")]
    public RangedInt Gold;

    [Header("Potions")]
    public RangedInt Potions;
}