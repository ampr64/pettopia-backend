﻿namespace Application.Common.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(string recipientEmail, string subject, string body);
    }
}
