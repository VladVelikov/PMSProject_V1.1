namespace PMS.Common
{
    public static class ErrorMessages
    {
        public const string ModelNotValidMessage = "The Model Passed by HTTP Request/Response Is Not Valid!";
        public const string WrongDataMessage = "Wrong data readings encoutered!";
        public const string NotDeletedMessage = "Delete operation not completed! Possible wrong Id or someone needs that data!";
        public const string NotEditedMessage = "Edit operation not completed! Possible wrong Id or data missing!";
        public const string NotCreatedMessage = "Record Was Not Created!";
        public const string NotFoundMessage = "Requred data or object was not found!";
        public const string NotUpdatedMessage = "Due to missing or wrong data the record was not updated!";
        public const string EmptyListMessage = "There are no records in the database to display. At least one record should be created first.";
    }
}
