﻿namespace Univent.App.Exceptions
{
    public class NameConflictException : Exception
    {
        private const string MessageTemplate = "A {0} with the name '{1}' already exists.";

        public NameConflictException()
            : base() { }

        public NameConflictException(string entityType, string entityName)
            : base(string.Format(MessageTemplate, entityType, entityName)) { }

        public NameConflictException(string entityType, string entityName, Exception innerException)
            : base(string.Format(MessageTemplate, entityType, entityName), innerException) { }
    }
}
