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
        Bush = 2,
        Hut = 3,
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
        Build = 8,
        Fight = 9,
        Drink = 10,
        Shout = 11,
        SeeLand = 12,
        SeeStaticObject = 13,
        SeeItem = 14,
        SeeEntity = 15,
        Listen = 16,
        GetHandType = 17,
        GetHandCount = 18,
        GetDayPart  = 19,
        Turn = 20,
        Goto = 21,
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
        Mate = 8,
        Build = 9,
        Fight = 10,
        Drink = 11,
        Shout = 12,
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
        Skin = 4,
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