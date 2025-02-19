namespace Univent.App.Exceptions
{
    public class NameConflictWithStatusException : Exception
    {
        private const string EnabledMessageTemplate = "The {0} named '{1}' already exists and it is currently enabled.";
        private const string DisabledMessageTemplate = "The {0} named '{1}' already exists, but it is currently disabled.";

        public NameConflictWithStatusException()
            : base() { }

        public NameConflictWithStatusException(string entityType, string entityName, bool isDisabled)
            : base(string.Format(isDisabled ? DisabledMessageTemplate : EnabledMessageTemplate, entityType, entityName)) { }

        public NameConflictWithStatusException(string entityType, string entityName, bool isDisabled, Exception innerException)
            : base(string.Format(isDisabled ? DisabledMessageTemplate : EnabledMessageTemplate, entityType, entityName), innerException) { }
    }
}
