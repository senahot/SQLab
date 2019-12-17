namespace SQLab.Model.Parser.Util
{
    public static class StatementType
    {
        //Data Definition Language
        public const string ALTER = "DDL";
        public const string CREATE = "DLL";
        public const string DROP = "DLL";
        public const string TRUNCATE = "DLL";
        public const string RENAME = "DLL";

        //Data Manipulation Language
        public const string SELECT = "DML";
        public const string INSERT = "DML";
        public const string UPDATE = "DML";
        public const string DELETE = "DML";
        public const string MERGE = "DML";
        public const string LOCK_TABLE = "DML";
        public const string CALL = "DML";
        public const string EXPLAIN_PATH = "DML";

        //Data Control Language
        public const string GRANT = "DCL";
        public const string REVOKE = "DCL";
        public const string AUDIT = "DCL";
        public const string COMMENT = "DCL";

        //Transaction Control Statement
        public const string COMMIT = "TCS";
        public const string ROLLBACK = "TCS";
        public const string SAVEPOINT = "TCS";
        public const string SET_TRANSACTION = "TCS";

        //Session Control Statement
        public const string ALTER_SESSION = "SCS";
        public const string SET_ROLE = "SCS";
    }
}
