﻿using System;
using System.Collections.Generic;
using System.Linq;

public class RandomChances
{
    private List<Item> items;
    private Dictionary<Rarity, SnuffleBag<Item>> itemsByRarity;
    private Random random = new Random();

    public RandomChances(List<Item> items)
    {
        this.items = items;

        itemsByRarity = new Dictionary<Rarity, SnuffleBag<Item>>()
        {
            { Rarity.Common, new SnuffleBag<Item>(SortByRarity(Rarity.Common)) },
            { Rarity.Uncommon, new SnuffleBag<Item>(SortByRarity(Rarity.Uncommon)) },
            { Rarity.Rare, new SnuffleBag<Item>(SortByRarity(Rarity.Rare)) },
            { Rarity.Epic, new SnuffleBag<Item>(SortByRarity(Rarity.Epic)) },
            { Rarity.Legendary, new SnuffleBag<Item>(SortByRarity(Rarity.Legendary))},
        };
    }

    private List<Item> SortByRarity(Rarity rarity) => items.Where(x => x._Rarity == rarity).ToList();

    public Item GetRandomItem()
    {
        double chance = random.NextDouble() * 100;
        Rarity rarity = Rarity.Common;

        float minRange = 0;

        for (int i = 5; i > 1; i--)
        {
            float maxRange = RunManager.GetChance((Rarity)i);

            if (minRange < chance & chance <= maxRange)
            {
                rarity = (Rarity)i;
                break;
            }

            minRange = maxRange;
        }

        Item item = null;

        if (itemsByRarity[rarity].Size > 0)
        {
            item = itemsByRarity[rarity].Next();
        }

        return item;
    }

    public List<Item> GetItems(int Amount)
    {
        List<Item> items = new List<Item>(Amount);

        int i = 0;
        while (i < Amount)
        {
            Item pooledItem = GetRandomItem();
            if (pooledItem != null)
            {
                items.Add(pooledItem);
                i++;
            }
        }
        return items;
    }
}