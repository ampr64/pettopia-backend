﻿namespace Domain.Common
{
    public interface IHasDomainEvents
    {
        IReadOnlyList<DomainEvent> DomainEvents { get; }

        void ClearDomainEvents();
    }
}