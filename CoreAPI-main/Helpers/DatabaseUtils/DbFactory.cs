using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using WebApi.Helpers.Common;

namespace WebApi.Helpers.DatabaseUtils
{
    public class DbFactory
    {
        public DbFactory() { }

        ///<summary>
        /// Lấy đối tượng kết nối đến cơ sở dữ liệu
        ///</summary>
        public static DbConnection GetDbConnection()
        {
            if (GlobalVariable.DbInUse == "MSSQL")
            {
                var conn = new SqlConnection(GlobalVariable.DbConnectionString);
                return conn;
            }
            //else if(GlobalVariable.DbInUse == "PostgreSQL")
            //{
            //    var conn = NpgsqlConnection(GlobalVariable.DbConnectionString);
            //    return conn;
            //}
            else
            {
                return null;
            }
        }

        ///<summary>
        /// Thực hiện câu truy vấn trả về 1 bảng dữ liệu dạng JArray bằng cách gọi SP
        ///</summary>
        public static string GetDataTableFromStoredProcedure(string spName, IDictionary<string, object> spParams = null)
        {
            using var conn = GetDbConnection();
            if (conn == null) throw new ArgumentNullException("Get connection fail");
            var command = conn.CreateCommand();
            command.CommandText = spName;
            command.CommandType = CommandType.StoredProcedure;
            if (spParams != null && spParams.Count > 0)
            {
                foreach (KeyValuePair<string, object> kvp in spParams)
                {
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = kvp.Key;
                    parameter.Value = kvp.Value;
                    command.Parameters.Add(parameter);
                }
            }
            conn.Open();
            using var result = command.ExecuteReader();
            if (result.HasRows)
            {
                var dataTable = new DataTable();
                dataTable.Load(result);
                string serialize = JsonConvert.SerializeObject(dataTable);
                if (conn.State != ConnectionState.Closed) conn.Close();
                return serialize;
            }
            else
            {
                if (conn.State != ConnectionState.Closed) conn.Close();
                return "[]";
            }
        }

        ///<summary>
        /// Thực hiện câu truy vấn trả về 1 bảng dữ liệu dạng JArray bằng câu lệnh SQL
        ///</summary>
        public static string GetDataTableFromSQL(string sql)
        {
            using var conn = GetDbConnection();
            if (conn == null) throw new ArgumentNullException("Get connection fail");
            var command = conn.CreateCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.Text;
            conn.Open();
            using var result = command.ExecuteReader();
            if (result.HasRows)
            {
                var dataTable = new DataTable();
                dataTable.Load(result);
                string serialize = JsonConvert.SerializeObject(dataTable);
                if (conn.State != ConnectionState.Closed) conn.Close();
                return serialize;
            }
            else
            {
                if (conn.State != ConnectionState.Closed) conn.Close();
                return "[]";
            }
        }

        ///<summary>
        /// Thực hiện câu truy vấn trả về 1 biến bằng câu lệnh SQL
        ///</summary>
        public static string GetVariableFromSQL(string sql)
        {
            using var conn = GetDbConnection();
            if (conn == null) throw new ArgumentNullException("Get connection fail");
            var command = conn.CreateCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.Text;
            conn.Open();
            using var result = command.ExecuteReader();
            if (result.HasRows)
            {
                result.Read();
                string variable = result[0].ToString();
                if (conn.State != ConnectionState.Closed) conn.Close();
                return variable;
            }
            else
            {
                if (conn.State != ConnectionState.Closed) conn.Close();
                return "";
            }
        }

        ///<summary>
        /// Thực hiện câu truy vấn trả về 1 biến dữ liệu bằng cách gọi SP
        ///</summary>
        public static string GetVariableFromStoredProcedure(string spName, IDictionary<string, object> spParams = null)
        {
            using var conn = GetDbConnection();
            if (conn == null) throw new ArgumentNullException("Get connection fail");
            var command = conn.CreateCommand();
            command.CommandText = spName;
            command.CommandType = CommandType.StoredProcedure;
            if (spParams != null && spParams.Count > 0)
            {
                foreach (KeyValuePair<string, object> kvp in spParams)
                {
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = kvp.Key;
                    parameter.Value = kvp.Value;
                    command.Parameters.Add(parameter);
                }
            }
            conn.Open();
            using var result = command.ExecuteReader();
            if (result.HasRows)
            {
                result.Read();
                string variable = result[0].ToString();
                if (conn.State != ConnectionState.Closed) conn.Close();
                return variable;
            }
            else
            {
                if (conn.State != ConnectionState.Closed) conn.Close();
                return "";
            }
        }
    }

}
