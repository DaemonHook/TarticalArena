using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions.Must;

//
// public enum EventType
// {
//     NULL,
// }
//
// /// <summary>
// /// 事件数据（不可变）
// /// </summary>
// public record EventMessage(EventType Type, object Body)
// {
//     public EventType Type { get; } = Type;
//     public object Body { get; } = Body;
// }
//
// /// <summary>
// /// 对 c# event 的封装，添加了更多信息
// /// </summary>
// public class GameEventItem
// {
//     public delegate void Handler(EventMessage eventMessage);
//
//     /// <summary>
//     /// 监听信息，包括了监听者名称，处理程序和最大触发次数
//     /// </summary>
//     private record HandlerInfo(string ListenerName, Handler Handler, int TimeLimit)
//     {
//         public string ListenerName { get; } = ListenerName;
//         public Handler Handler { get; } = Handler;
//
//         /// <summary>
//         /// 触发次数限制
//         /// </summary>
//         public int TimeLimit { get; set; } = TimeLimit;
//
//         public virtual bool Equals(HandlerInfo other)
//         {
//             return other != null
//                    && string.Equals(ListenerName, other.ListenerName, StringComparison.OrdinalIgnoreCase)
//                    && Handler == other.Handler;
//         }
//
//         public override int GetHashCode()
//         {
//             return HashCode.Combine(ListenerName, Handler);
//         }
//     }
//
//     private string eventKey;
//     private readonly HashSet<HandlerInfo> handlerInfos = new();
//     private event Handler GameEvent;
//
//     public int ListenerCount => handlerInfos.Count;
//
//     /// <summary>
//     /// 调用构造函数时就添加第一个处理程序
//     /// </summary>
//     public GameEventItem(string key, string listener, Handler handler, int time)
//     {
//         eventKey = key;
//         handlerInfos.Add(new HandlerInfo(listener, handler, time));
//         GameEvent += handler;
//     }
//
//     /// <summary>
//     /// 注册事件的监听者
//     /// </summary>
//     /// <returns>是否成功（若失败说明重复添加了）</returns>
//     public bool AddEventHandler(string listenerName, Handler handler, int time)
//     {
//         var suc = handlerInfos.Add(new HandlerInfo(listenerName, handler, time));
//         if (!suc)
//         {
//             Debug.LogError("重复的 listener 和 handler");
//             return false;
//         }
//
//         GameEvent += handler;
//         return true;
//     }
//
//     
//     public void Trigger(EventMessage eventMessage)
//     {
//         if (GameEvent != null) GameEvent(eventMessage);
//         else
//         {
//             Debug.LogError("GameEvent is null!");
//         }
//
//         List<HandlerInfo> toDelete = new List<HandlerInfo>();
//         foreach (var info in handlerInfos)
//         {
//             if (info.TimeLimit == Util.Infinity)
//                 // 如果为 Infinity 则不会减少调用次数 
//                 continue;
//             info.TimeLimit -= 1;
//             if (info.TimeLimit <= 0)
//             {
//                 GameEvent -= info.Handler;
//                 toDelete.Add(info);
//                 Debug.Log($"次数耗尽, {info} 被移除");
//             }
//         }
//         foreach (var handlerInfo in toDelete)
//         {
//             handlerInfos.Remove(handlerInfo);
//         }
//     }
//
//     public override string ToString()
//     {
//         var stringBuilder = new StringBuilder();
//         int index = 0;
//         foreach (var info in handlerInfos)
//         {
//             stringBuilder.AppendFormat("{0}, ListenerName: {1}, TimeRemaining: {2}", index++, info.ListenerName,
//                 info.TimeLimit);
//         }
//
//         return stringBuilder.ToString();
//     }
// }

/// <summary>
/// 事件的枚举
/// </summary>
public enum EventType
{
    Null,
}

/// <summary>
/// 全局事件管理器的简要实现
/// TODO: 重写以增加功能
/// </summary>
public class EventManager : BaseManager<EventManager>
{
    private Dictionary<EventType, Action<object>> registered = new();
    // private Dictionary<string, string> eventInfos = new();

    public override void InitManager()
    {
        base.InitManager();
        registered.Clear();
        
    }

    // public void AddEvent(EventType type, string info = null)
    // {
    //     if (registered.ContainsKey(type))
    //     {
    //         Debug.LogError("重复的事件：" + type);
    //     }
    //     registered.Add(type, o => { });
    // }

    /// <summary>
    /// 触发事件
    /// </summary>
    public void Trigger(EventType type, object param = null)
    {
        if (!registered.ContainsKey(type))
        {
            Debug.LogError("非法的事件：" + type);
        }

        registered[type].Invoke(param);
    }

    public void RegisterHandler(EventType type, Action<object> handler)
    {
        if (!registered.ContainsKey(type))
        {
            registered.Add(type, handler);
        }
        else
        {
            registered[type] += handler;
        }
    }

    public void UnregisterHandler(EventType type, Action<object> handler)
    {
        registered[type] -= handler;
    }
}