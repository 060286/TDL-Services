namespace TDL.Infrastructure.Constants
{
    public static class ExceptionConstant
    {
        public const string RestrictedResource = "Cannot access authorized resource!";

        public const string NotFound = "{0} cannot be found!";

        public const string ExpiredAuthorizationToken = "The token is expired!";

        public const string InvalidAuthorizationToken = "The token is invalid!";

        public const string ExportDataNotFound = "Data of the view name {0} cannot be found!";

        public const string NotFoundDataSet = "Dataset is not available!";

        public const string ConcurrencyConflictMessage = "Reload Page: You are required to reload the page for the status to be updated";

        public const string UpdateStatusFailedMessage = "Cannot update the status";

        public const string ValidateTotalRecord = "Total records is invalid!";
    }
}
