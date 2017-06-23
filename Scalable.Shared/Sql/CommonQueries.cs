namespace Scalable.Shared.Sql
{
    public static class CommonQueries
    {
        #region Common

        public const string ReadGeneratedIdentityValue =
                                        "SELECT @@identity";

        #endregion
    }
}
