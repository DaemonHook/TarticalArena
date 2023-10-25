using System;
using UnityEngine.SubsystemsImplementation;

/// <summary>
/// UI事件类型
/// </summary>
[Serializable]
public enum UIEventType
{
    Click,
    Select,
}

/// <summary>
/// UI事件
/// </summary>
[Serializable]
public class UIEvent
{
    public UIEventType Type;

    public object[] Params;

    public UIEvent(UIEventType type, params object[] @params)
    {
        Type = type;
        Params = @params;
    }

    public override string ToString()
    {
        return $"UIEvent: type: [{Type}] param: [{Params}]";
    }

    public Type GetParamType(int index)
    {
        switch (Type)
        {
            case UIEventType.Click:
                if (index == 0)
                    return (1, 1).GetType();
                else return null;
            default:
                return null;
        }
    }
}