﻿namespace Application.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base("Record not found.")
        {
        }
    }
}