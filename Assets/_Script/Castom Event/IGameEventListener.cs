using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SOEvents
{
    public interface IGameEventListener<T>
    {
        void OnEventRaised(T item);
    }
}
