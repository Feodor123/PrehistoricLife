using System;
using System.Collections.Generic;
namespace PrehistoricLife
{
    public class Action
    {
        public static Dictionary<ActionType, int> actionCount = new Dictionary<ActionType, int>()
        {
            {ActionType.Noting,1},
            {ActionType.Go,8},
            {ActionType.Take,Enum.GetNames(typeof(Hand)).Length * (Enum.GetValues(typeof(Item)).Length - 1)},//брать ничто нельзя
            {ActionType.Put,Enum.GetNames(typeof(Hand)).Length},
            {ActionType.Throw,Enum.GetNames(typeof(Hand)).Length},
            {ActionType.Eat,Enum.GetNames(typeof(Hand)).Length},
            {ActionType.Sleep,1},
            {ActionType.UseStaticObject,1},
            {ActionType.Mate,1},
            {ActionType.Build,1},//пока только шалаш
            {ActionType.Fight,1},
            {ActionType.Drink,1},
            {ActionType.Cry,0},//хз
            {ActionType.See,0},//хзхз
            {ActionType.Listen,0},//хзхзхз
        };//TODO: do
        public static Dictionary<ActionType, int> actionBias = new Dictionary<ActionType, int>() { };//TODO: do
        static Action()
        {
            actionBias.Add(0,0);
            for (int i = 1;i < Enum.GetValues(typeof(ActionType)).Length; i++)
            {
                actionBias.Add((ActionType)i, actionBias[(ActionType)(i - 1)] + actionCount[(ActionType)(i - 1)]);
            }
        }
    }
}
