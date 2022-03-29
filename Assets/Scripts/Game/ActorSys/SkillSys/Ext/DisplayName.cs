using System;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Field, AllowMultiple = false)]
public class DisplayName : Attribute
{
    private string m_name;
    private string m_toolTip;

    public string displayName
    {
        get
        {
            return this.m_name;
        }
    }

    public string toolTip
    {
        get
        {
            return this.m_toolTip;
        }
    }

    public DisplayName(string name)
    {
        this.m_name = name;
    }

    public DisplayName(string name, string toolTip)
    {
        this.m_name = name;
        this.m_toolTip = toolTip;
    }
}


[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public class ArrayLenLimit : Attribute
{
    public int m_minLen;
    public int m_maxLen;

    public ArrayLenLimit(int minLen, int maxLen)
    {
        this.m_minLen = minLen;
        this.m_maxLen = maxLen;
    }
}