﻿namespace SupportChat.Core.Events;

public interface IEventHandler<in T> where T : IEvent
{
    void Handle(T @event);
}