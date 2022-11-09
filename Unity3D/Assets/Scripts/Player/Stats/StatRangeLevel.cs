using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatRangeLevel
{
    public string Name { get; set; } = "";
    public enum Range { Low, Middle, High}
    protected float BaseStat;
    [Range(0, 50)][SerializeField] protected float LowerDivision = 33f;   
    [Range(51, 100)][SerializeField] protected float UpperDivision = 66f;

    public StatRangeLevel() { }
    public StatRangeLevel(float baseStat, float lowerDivision, float upperDivision, string name)
        : this(baseStat, lowerDivision, upperDivision)
    {
        Name = name;
    }
    public StatRangeLevel(float baseStat, float lowerDivision, float upperDivision)
    {
        BaseStat = baseStat;
        LowerDivision = lowerDivision;
        UpperDivision = upperDivision;
    }
    
    public Range GetRangeLevel(float baseStat)
    {
        BaseStat = baseStat;
        return GetRangeLevel();
    }
    public Range GetRangeLevel()
    {
        if (BaseStat < LowerDivision)
            return Range.Low;
        else if (BaseStat < UpperDivision)
            return Range.Middle;
        else return Range.High;
    }
}
