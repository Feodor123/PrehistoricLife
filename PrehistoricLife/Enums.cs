using System;
using System.Collections.Generic;
namespace PrehistoricLife
{
    public enum LandType
    {
        Grass = 0,
        Water = 1,
    };
    public enum StaticObject
    {
        Nothing = 0,
        TreeObject = 1,
        Bush = 2,
        Hut = 3,
    };
    public enum ActionType
    {
        Noting = 0,
        Go = 1,
        Take = 2,
        Put = 3,
        Throw = 4,
        Eat = 5,
        Sleep = 6,
        UseStaticObject = 7,
        Mate = 8,
        Build = 9,
        Fight = 10,
        Drink = 11,
        Cry = 12,
        See = 13,
        Listen = 14,

    }
    public enum Item
    {
        Nothing = 0,
        Stick = 1,
        Stone = 2,
        Food = 3,
        Skin = 4,
    }
    public enum Hand//maybe more)))
    {
        Right = 0,
        Left = 1,
    }
}