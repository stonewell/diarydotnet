using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Diary.Net.DB;
using System.Data.Common;

namespace Diary.Net
{
    static class Program
    {
        private static DiaryNetDS dairyNetDS_ = new DiaryNetDS();

        private static readonly DbProviderFactory dbProvideFactory_ = 
#if __MonoCS__
            DbProviderFactories.GetFactory("Mono.Data.SQLite");
#else
            DbProviderFactories.GetFactory("System.Data.SQLite");
#endif
        private static readonly DbConnection dbConnection_ = 
            dbProvideFactory_.CreateConnection();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.ApplicationExit += new EventHandler(Application_ApplicationExit);

            Application.Run(new MainFrm());
        }

        static void Application_ApplicationExit(object sender, EventArgs e)
        {
System.Console.WriteLine("Exit");
            if (dbConnection_.State != System.Data.ConnectionState.Closed)
            {
System.Console.WriteLine("VACUUM");
                DBManager.ExecuteNonQuery(dbConnection_, "VACUUM");
                dbConnection_.Close();
            }
System.Console.WriteLine("Exit Done");
        }

        public static DiaryNetDS DiaryNetDS
        {
            get
            {
                return dairyNetDS_;
            }
        }

        public static DbProviderFactory DbProvideFactory
        {
            get
            {
                return dbProvideFactory_;
            }
        }

        public static DbConnection DbConnection
        {
            get
            {
                if (dbConnection_.State == System.Data.ConnectionState.Closed ||
                    dbConnection_.State == System.Data.ConnectionState.Broken ||
                    dbConnection_.State == System.Data.ConnectionState.Connecting)
                {
                    throw new Exception("DBConnection's state is invalid:" +
                        dbConnection_.State);
                }

                return dbConnection_;
            }
        }

        public static void InitializeDB(string dbName, string username, string password)
        {
            if (dbConnection_.State != System.Data.ConnectionState.Closed)
                dbConnection_.Close();

            DbConnectionStringBuilder builder =
                dbProvideFactory_.CreateConnectionStringBuilder();

            builder["password"] = username + 
                "-Diary@Angel&Stone$2008-" + 
                password;
            builder["data source"] = dbName;
            dbConnection_.ConnectionString = builder.ConnectionString;

            dbConnection_.Open();

            DBManager.ExecuteNonQuery(dbConnection_, "PRAGMA auto_vacuum =1");

            DBManager.VerifyAndPatchDatabase(dbConnection_);

            dairyNetDS_ = new DiaryNetDS();
        }
    }
}
