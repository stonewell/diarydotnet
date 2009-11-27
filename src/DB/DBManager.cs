using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Collections;
using System.Data;

namespace Diary.Net.DB
{
    public static class DBManager
    {
        private static readonly Hashtable tables_ =
            new Hashtable();

        static DBManager()
        {
            tables_.Add("DiaryNotes", "CREATE TABLE DiaryNotes(ID INTEGER PRIMARY KEY, Text_ID INTEGER, Binary_ID INTEGER, Note_Date DateTime, Modify_Date DateTime)");
            tables_.Add("Attachments", "CREATE TABLE Attachments(ID INTEGER PRIMARY KEY, Ref_ID INTEGER, Is_Notes CHAR(1), Binary_ID INTEGER, FileName VarChar(255))");
            tables_.Add("Documents", "CREATE TABLE Documents(ID INTEGER PRIMARY KEY, Text_ID INTEGER, Binary_ID INTEGER, Title VARCHAR(255), Create_Date DateTime, Modify_Date DateTime)");
            tables_.Add("Content_Text", "CREATE TABLE Content_Text(ID INTEGER PRIMARY KEY, Content TEXT)");
            tables_.Add("Content_Binary", "CREATE TABLE Content_Binary(ID INTEGER PRIMARY KEY, Content IMAGE)");
            tables_.Add("Content_FullText", "CREATE VIRTUAL TABLE Content_FullText USING FTS3(ID, IS_Documents, Title, Content)");
        }

        public static void VerifyAndPatchDatabase(DbConnection conn)
        {
            if (TableExists(conn, "Drop table Content_FullText"))
            {
                ExecuteNonQuery(conn, "Drop table Content_FullText");
            }

            VerifyDatabaseTables(conn);

            ExecutePatches(conn);

            UpdateFullTextEngine(conn);
        }

        private static void UpdateFullTextEngine(DbConnection conn)
        {
            string script =
                "INSERT INTO Content_FullText SELECT T1.ID,0 AS IS_Documents,'' AS TITLE,T2.Content " +
                "FROM DiaryNotes AS T1 LEFT JOIN Content_Text AS T2 ON T1.Text_ID=T2.ID;" +
                "INSERT INTO Content_FullText SELECT T1.ID,1 AS IS_Documents,T1.Title,T2.Content " +
                "FROM Documents AS T1 LEFT JOIN Content_Text AS T2 ON T1.Text_ID=T2.ID;" +
                "INSERT INTO Content_FullText SELECT T1.Ref_ID AS ID,1 AS IS_Documents,T1.FileName as Title,'' AS Content " +
                "FROM Attachments AS T1 WHERE T1.Is_Notes = 0;" +
                "INSERT INTO Content_FullText SELECT T1.Ref_ID AS ID,0 AS IS_Documents,T1.FileName as Title,'' AS Content " +
                "FROM Attachments AS T1 WHERE T1.Is_Notes = 1";

            ExecuteNonQuery(conn, script);
        }

        private static void VerifyDatabaseTables(DbConnection conn)
        {
            foreach (string tablename in tables_.Keys)
            {
                if (!TableExists(conn, tablename))
                {
                    ExecuteNonQuery(conn, tables_[tablename] as string);
                }
            }
        }

        public static void ExecuteNonQuery(DbConnection conn, string script)
        {
            using (DbCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = script;
                cmd.ExecuteNonQuery();
            }
        }

        private static bool TableExists(DbConnection conn, string tablename)
        {
            try
            {
                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT 1 FROM " +
                        tablename +
                        " WHERE 0=1";
                    cmd.ExecuteNonQuery();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private static void ExecutePatches(DbConnection conn)
        {
        }

        public static int SaveText(DbConnection dbConnection, int id, 
            int ref_ID, bool isDocuments, string title, string text)
        {
            int result = id;

            //Save To Content_Text
            using (DbCommand cmd = dbConnection.CreateCommand())
            {
                cmd.CommandText =
                    "SELECT COUNT(*) FROM Content_Text WHERE ID=" + id;

                int count = Convert.ToInt32(cmd.ExecuteScalar());

                if (count > 0)
                {
                    cmd.CommandText =
                        "UPDATE Content_Text Set Content=@Content WHERE ID=" + id;
                }
                else
                {
                    cmd.CommandText =
                        "INSERT INTO Content_Text(Content) VALUES(@Content);SELECT last_insert_rowid() AS [ID]";
                }

                DbParameter content = cmd.CreateParameter();

                content.ParameterName = "@Content";
                content.Value = text;

                cmd.Parameters.Add(content);

                if (count > 0)
                {
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    result = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            //Update the full text search engine
            SaveFullText(dbConnection, ref_ID, isDocuments, title, text);

            return result;
        }

        public static void SaveFullText(DbConnection dbConnection,
            int id, bool isDocuments, string title, string text)
        {
            //Save To Content_Text
            using (DbCommand cmd = dbConnection.CreateCommand())
            {
                cmd.CommandText =
                    "SELECT COUNT(*) FROM Content_FullText WHERE ID=" +
                    id +
                    " AND IS_Documents=" +
                    (isDocuments ? 1 : 0);

                int count = Convert.ToInt32(cmd.ExecuteScalar());

                if (count > 0)
                {
                    cmd.CommandText =
                        "UPDATE Content_FullText Set Title=@Title, Content=@Content WHERE " +
                        " ID=@ID AND IS_Documents=@IS_Documents";
                }
                else
                {
                    cmd.CommandText =
                        "INSERT INTO Content_FullText(ID, IS_Documents, Title, Content)" +
                        " VALUES(@ID, @IS_Documents, @Title, @Content)";
                }

                //1.
                DbParameter param = cmd.CreateParameter();

                param.ParameterName = "@ID";
                param.Value = id;
                cmd.Parameters.Add(param);

                //2.
                param = cmd.CreateParameter();

                param.ParameterName = "@IS_Documents";
                param.Value = isDocuments ? 1 : 0;
                cmd.Parameters.Add(param);

                //3.
                param = cmd.CreateParameter();

                param.ParameterName = "@Title";
                param.Value = title;
                cmd.Parameters.Add(param);

                //4.
                param = cmd.CreateParameter();

                param.ParameterName = "@Content";
                param.Value = text;

                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();
            }
        }

        public static int SaveBinary(DbConnection dbConnection,
            int id, string binary)
        {
            using (DbCommand cmd = dbConnection.CreateCommand())
            {
                cmd.CommandText =
                    "SELECT COUNT(*) FROM Content_Binary WHERE ID=" + id;

                object o = cmd.ExecuteScalar();
                int count = (int)Convert.ToInt32(o);

                if (count > 0)
                {
                    cmd.CommandText =
                        "UPDATE Content_Binary Set Content=@Content WHERE ID=" + id;
                }
                else
                {
                    cmd.CommandText =
                        "INSERT INTO Content_Binary(Content) VALUES(@Content);SELECT last_insert_rowid() AS [ID]";
                }

                DbParameter content = cmd.CreateParameter();

                content.ParameterName = "@Content";
                content.Value = Encoding.Default.GetBytes(binary);

                cmd.Parameters.Add(content);

                if (count > 0)
                {
                    cmd.ExecuteNonQuery();

                    return id;
                }
                else
                {
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public static string GetBinary(DbConnection dbConnection, int id)
        {
            using (DbCommand cmd = dbConnection.CreateCommand())
            {
                cmd.CommandText = "SELECT Content FROM Content_Binary WHERE ID=" + id;

                object o = cmd.ExecuteScalar();

                if (o is byte[])
                {
                    return Encoding.Default.GetString(o as byte[]);
                }

                return "";
            }
        }

        public static DbDataAdapter CreateDiaryDataAdapter(DbProviderFactory dbProviderFactory, DbConnection dbConnection)
        {
            return CreateDataAdapter(dbProviderFactory, dbConnection, "DiaryNotes");
        }

        public static DbDataAdapter CreateAttachmentsDataAdapter(DbProviderFactory dbProviderFactory, DbConnection dbConnection)
        {
            return CreateDataAdapter(dbProviderFactory, dbConnection, "Attachments");
        }

        public static DbDataAdapter CreateDataAdapter(DbProviderFactory dbProviderFactory, DbConnection dbConnection, string tableName)
        {
            DbDataAdapter adp = dbProviderFactory.CreateDataAdapter();

            DbCommandBuilder builder = dbProviderFactory.CreateCommandBuilder();
            adp.SelectCommand = dbConnection.CreateCommand();
            adp.SelectCommand.CommandText = "SELECT * FROM " + tableName;
            builder.DataAdapter = adp;
            builder.ConflictOption = ConflictOption.OverwriteChanges;

            adp.UpdateCommand = ((ICloneable)builder.GetUpdateCommand()).Clone() as DbCommand;
            adp.InsertCommand = ((ICloneable)builder.GetInsertCommand()).Clone() as DbCommand;
            adp.DeleteCommand = ((ICloneable)builder.GetDeleteCommand()).Clone() as DbCommand;

            return adp;
        }

        public static void DeleteBinary(DbConnection dbConnection, int id)
        {
            using (DbCommand cmd = dbConnection.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM Content_Binary WHERE ID=" + id;
                cmd.ExecuteNonQuery();
            }
        }

        public static void DeleteText(DbConnection dbConnection, int id, int text_id, bool isDocument)
        {
            using (DbCommand cmd = dbConnection.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM Content_Text WHERE ID=" + text_id;
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DELETE FROM Content_FullText WHERE ID=" +
                    id +
                    " AND Is_Documents=" +
                    (isDocument ? 1 : 0);
                cmd.ExecuteNonQuery();
            }
        }

        public static DbDataAdapter CreateDocumentsDataAdapter(DbProviderFactory dbProviderFactory, DbConnection dbConnection)
        {
            return CreateDataAdapter(dbProviderFactory, dbConnection, "Documents");
        }

        public static void FullTextSearch(DbConnection dbConnection, string findWhat, ArrayList notesIds, ArrayList documentsIds)
        {
            using (DbCommand cmd = dbConnection.CreateCommand())
            {
                cmd.CommandText = "select count(*) FROM content_fullText";

                int count = Convert.ToInt32(cmd.ExecuteScalar());

                cmd.CommandText = "SELECT ID, IS_Documents FROM Content_FullText" +
                    " WHERE Content Like @FindWhat OR Title Like @FindWhat" +
                    " ORDER BY ID";

                DbParameter findWhatParam = cmd.CreateParameter();

                findWhatParam.ParameterName = "@FindWhat";
                findWhatParam.Value = "%" + findWhat + "%";

                cmd.Parameters.Add(findWhatParam);

                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = Convert.ToInt32(reader[0]);
                        int documents = Convert.ToInt32(reader[1]);

                        if (documents == 0)
                        {
                            notesIds.Add(id);
                        }
                        else
                        {
                            documentsIds.Add(id);
                        }
                    }
                }
            }
        }
    }
}
