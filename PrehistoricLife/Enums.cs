using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
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
    };
    public enum ActionType
    {
        Nothing = 0,
        Go = 1,
        Take = 2,
        Put = 3,
        Throw = 4,
        Eat = 5,
        Sleep = 6,
        UseStaticObject = 7,
        Fight = 8,
        Drink = 9,
        Shout = 10,
        SeeLand = 11,
        SeeStaticObject = 12,
        SeeItem = 13,
        SeeEntity = 14,
        Listen = 15,
        GetHandType = 16,
        GetHandCount = 17,
        GetDayPart  = 18,
        Turn = 19,
        Goto = 20,
    }

    public enum OperationType
    {
        Nothing = 0,
        Go = 1,
        Take = 2,
        Put = 3,
        Throw = 4,
        Eat = 5,
        Sleep = 6,
        UseStaticObject = 7,
        Fight = 8,
        Drink = 9,
        Shout = 10,
    }

    public enum DayPart
    {
        Morning = 0,
        Daytime = 1,
        Evening = 2,
        Night = 3,
    }

    public enum Item
    {
        Nothing = 0,
        Stick = 1,
        Stone = 2,
        Food = 3,
        Spear = 4,
    }

    public enum EntityType
    {
        Human = 0,
        Tiger = 1,
        Mammont = 2,
    }

    public enum Hand//maybe more)))
    {
        Right = 0,
        Left = 1,
    }
}