using Microsoft.Data.Sqlite;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Transactions;
using System.Windows.Input;

namespace MstscManager.Utils {
    internal class DbSqlHelper {
        public static string ConnectionString { get; set; }
        public static SqliteConnection common_conn = null;
        public static SqliteTransaction common_transaction = null;
        private static void PrepareCommand(SqliteCommand cmd, SqliteConnection conn, string cmdText, params object[] p) {
            if (conn.State != ConnectionState.Open) conn.Open();
            if(common_transaction!=null) cmd.Transaction = common_transaction;
            cmd.Parameters.Clear();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 30;
            if (p != null) {
                int index = 0;
                while (index < p.Length) {
                    cmd.CommandText = cmd.CommandText.ReplaceOne("?","$t"+index.ToString());
                    index++;
                }
                index = 0;
                foreach (object parm in p) {
                    cmd.Parameters.AddWithValue("$t" + index.ToString(), parm);
                    index++;
                }
            }
            cmd.Prepare();
        }
        private static void PrepareCommand2(SqliteCommand cmd, SqliteConnection conn, string cmdText, params object[] p) {
            if (conn.State != ConnectionState.Open) conn.Open();
            cmd.Parameters.Clear();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 30;
            if (p != null) {
                int index = 0;
                while (index < p.Length) {
                    cmd.CommandText = cmd.CommandText.ReplaceOne("?", "$t" + index.ToString());
                    index++;
                }
                index = 0;
                foreach (object parm in p) {
                    cmd.Parameters.AddWithValue("$t" + index.ToString(), parm);
                    index++;
                }
            }
            cmd.Prepare();
        }
        public static int ExecuteNonQuery(string cmdText, params object[] p) {
            if (common_conn == null) common_conn = new SqliteConnection(ConnectionString);
            //using (SqliteConnection conn = new SqliteConnection(ConnectionString)) {
            using (SqliteCommand command = new SqliteCommand()) {
                PrepareCommand(command, common_conn, cmdText, p);
                return command.ExecuteNonQuery();
            }
            //}
        }
        public static int ExecuteNonQuery2(string cmdText, params object[] p) {
            //if (common_conn == null) common_conn = new SqliteConnection(ConnectionString);
            using (SqliteConnection conn = new SqliteConnection(ConnectionString)) {
                using (SqliteCommand command = new SqliteCommand()) {
                PrepareCommand2(command, conn, cmdText, p);
                return command.ExecuteNonQuery();
            }
        }
    }
        public static SqliteDataReader ExecuteReader(string cmdText, params object[] p) {
            if(common_conn==null) common_conn = new SqliteConnection(ConnectionString);
            SqliteCommand command = new SqliteCommand();
            PrepareCommand(command, common_conn, cmdText, p);
            return command.ExecuteReader(CommandBehavior.CloseConnection);
            
        }
        public static SqliteDataReader ExecuteReader2(string cmdText, params object[] p) {
            //if (common_conn == null) common_conn = new SqliteConnection(ConnectionString);
            SqliteConnection conn = new SqliteConnection(ConnectionString);
            SqliteCommand command = new SqliteCommand();
            PrepareCommand2(command, conn, cmdText, p);
            return command.ExecuteReader(CommandBehavior.CloseConnection);

        }
        //public static string ExecuteScalar(string cmdText, params object[] p) {
        //    using (SqliteConnection conn = new SqliteConnection(ConnectionString)) {
        //        using (SqliteCommand command = new SqliteCommand()) {
        //            PrepareCommand(command, conn, cmdText, p);
        //            return (string)command.ExecuteScalar();
        //        }
        //    }
        //}
        public static int UpdatePassword( string newPassword) {
            using (var connection = new SqliteConnection(ConnectionString)) {
                connection.Open();
                using (var command = connection.CreateCommand()) {
                    command.CommandText = "SELECT quote($newPassword);";
                    command.Parameters.AddWithValue("$newPassword", newPassword);
                    var quotedNewPassword = command.ExecuteScalar() as string;
                    command.CommandText = "PRAGMA rekey = " + quotedNewPassword;
                    command.Parameters.Clear();
                    int x = command.ExecuteNonQuery();
                    return x;
                }
            }
        }
    }
    public static class StringReplace {
        public static string ReplaceOne(this string str, string oldStr, string newStr) {
            StringBuilder sb = new StringBuilder(str);
            int index = str.IndexOf(oldStr);
            if (index > -1)
                return str.Substring(0, index) + newStr + str.Substring(index + oldStr.Length);
            return str;
        }
    }
}
