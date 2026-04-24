using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;

namespace Library.Database.DAL
{
    public interface ISqlDataAccess
    {
        Task<DynamicParameters?> ExecuteWithOutputsAsync(string storedProcedure, DynamicParameters parameters, string ConnectionId = "ConnectionDefault");
        Task<IEnumerable<T>?> ExecuteWithOutputsAsync<T>(string storedProcedure, DynamicParameters parameters, string ConnectionId = "ConnectionDefault");
        DataTable GetDataStoredProcedure(string methodName, string storedProcedureName, ref List<SqlParameter> parameters, string ConnectionId = "ConnectionDefault");
        IEnumerable<T> LoadData<T, U>(string storedProcedure, U parameters, string ConnectionId = "ConnectionDefault");
        Task<IEnumerable<T>> LoadDataAsync<T, U>(string storedProcedure, U parameters, string ConnectionId = "ConnectionDefault");
        Task<IEnumerable<T>> LoadDataAsync<T, U, O>(string storedProcedure, U LogonInformation, O parameters, string ConnectionId = "ConnectionDefault");
        Task<List<T>?> LoadDataJsonAsync<T, U>(string storedProcedure, U parameters, string ConnectionId = "ConnectionDefault");
        Task<(IEnumerable<T> Results, DynamicParameters? OutputParams)> LoadDataWithOutputsAsync<T>(string storedProcedure, DynamicParameters parameters, string ConnectionId = "ConnectionDefault");
        Task<T> LoadFirstDataAsync<T, U>(string storedProcedure, U parameters, string ConnectionId = "ConnectionDefault");
        Task<T> LoadSingularAsync<T, U>(string storedProcedure, U parameters, string ConnectionId = "ConnectionDefault");
        Task<IEnumerable<T>> QueryDataAsync<T, U>(string query, U parameters, string ConnectionId = "ConnectionDefault");
        Task SaveData<T>(string storedProcedure, T parameters, string ConnectionId = "ConnectionDefault");
    }

    public class SqlDataAccess : ISqlDataAccess
    {
        private readonly IConfiguration _config;

        public SqlDataAccess(IConfiguration config)
        {
            _config = config;
        }

        public DataTable GetDataStoredProcedure(string methodName, string storedProcedureName, ref List<SqlParameter> parameters, string ConnectionId = "ConnectionDefault")
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(_config.GetConnectionString(ConnectionId));

            try
            {
                if (connection.State == ConnectionState.Closed) connection.Open();
                SqlCommand command = new SqlCommand(storedProcedureName, connection) { CommandType = CommandType.StoredProcedure };
                SqlParameter[] parms = parameters.ToArray();
                if (parameters != null) command.Parameters.AddRange(parms);
                command.CommandTimeout = 1200;
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);
                parameters = parms.ToList();
            }
            catch (Exception ex)
            { }
            finally
            {
                if (connection.State == ConnectionState.Open) connection.Close();
            }

            return dt;
        }


        //private readonly System.Configuration.ConfigurationManager. _config;




        public async Task<IEnumerable<T>> LoadDataAsync<T, U>(
            string storedProcedure,
            U parameters,
            string ConnectionId = "ConnectionDefault")
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(_config.GetConnectionString(ConnectionId)))
                {
                    return await connection.QueryAsync<T>(storedProcedure
                                                        , parameters
                                                        , commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<IEnumerable<T>> LoadDataAsync<T, U, O>(
            string storedProcedure,
            U LogonInformation,
            O parameters,
            string ConnectionId = "ConnectionDefault")
        {
            var expando = new System.Dynamic.ExpandoObject() as IDictionary<string, object?>;

            if (LogonInformation != null)
            {
                foreach (var prop in typeof(U).GetProperties())
                {
                    expando[prop.Name] = prop.GetValue(LogonInformation);
                }
            }

            if (parameters != null)
            {
                foreach (var prop in typeof(O).GetProperties())
                {
                    expando[prop.Name] = prop.GetValue(parameters);
                }
            }

            try
            {
                using (IDbConnection connection = new SqlConnection(_config.GetConnectionString(ConnectionId)))
                {
                    return await connection.QueryAsync<T>(
                        storedProcedure,
                        (object)expando,
                        commandType: CommandType.StoredProcedure
                    );
                }
            }
            catch (Exception)
            {
                return null;
            }


        }


        private class TempJson
        {
            public string? Json { get; set; }
        }

        public async Task<List<T>?> LoadDataJsonAsync<T, U>(
         string storedProcedure,
         U parameters,
         string ConnectionId = "ConnectionDefault") 
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(_config.GetConnectionString(ConnectionId)))
                {
                    var data = await connection.QuerySingleOrDefaultAsync<string>(storedProcedure
                                                        , parameters
                                                        , commandType: CommandType.StoredProcedure);
                    //if (data == null) return default(T);
                    //var data = await LoadSingularAsync<string, U>(storedProcedure, parameters, ConnectionId);
                    if(data == null || string.IsNullOrWhiteSpace(data)) return null;

                    var data2 = Newtonsoft.Json.JsonConvert.DeserializeObject<Wrapper<T>>(data);
                    return data2?.Data??null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private class Wrapper<T>
        {
            public List<T>? Data { get; set; }
        }


        public async Task<T> LoadFirstDataAsync<T, U>(
           string storedProcedure,
           U parameters,
           string ConnectionId = "ConnectionDefault")
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(_config.GetConnectionString(ConnectionId)))
                {
                    var Data = await connection.QueryAsync<T>(storedProcedure
                                                        , parameters
                                                        , commandType: CommandType.StoredProcedure);
                    if (Data == null || Data.Count() == 0) return default(T);
                    return Data.FirstOrDefault();
                }
            }
            catch (Exception)
            {
                return default(T);
            }

        }

        public async Task<T> LoadSingularAsync<T, U>(
           string storedProcedure,
           U parameters,
           string ConnectionId = "ConnectionDefault")
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(_config.GetConnectionString(ConnectionId)))
                {
                    var Data = await connection.ExecuteScalarAsync<T>(storedProcedure
                                                        , parameters
                                                        , commandType: CommandType.StoredProcedure);
                    if (Data == null) return default(T);
                    return Data;
                }
            }
            catch (Exception)
            {
                return default(T);
            }

        }


        public IEnumerable<T> LoadData<T, U>(string storedProcedure,
                                                 U parameters,
                                                 string ConnectionId = "ConnectionDefault")
        {

            try
            {
                using (IDbConnection connection = new SqlConnection(_config.GetConnectionString(ConnectionId)))
                {
                    return connection.Query<T>(storedProcedure,
                                               parameters,
                                               commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                return null;

            }
        }

        public async Task<IEnumerable<T>> QueryDataAsync<T, U>(
           string query,
           U parameters,
           string ConnectionId = "ConnectionDefault")
        {
            using (IDbConnection connection = new SqlConnection(_config.GetConnectionString(ConnectionId)))
            {
                return await connection.QueryAsync<T>(query
                                                    , parameters
                                                    , commandType: CommandType.Text);
            }
        }

        public async Task SaveData<T>(
            string storedProcedure,
            T parameters,
            string ConnectionId = "ConnectionDefault")
        {
            using (IDbConnection connection = new SqlConnection(_config.GetConnectionString(ConnectionId)))
            {
                await connection.ExecuteAsync(storedProcedure
                                            , parameters
                                            , commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// Returns data from a stored procedure and also captures output parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="storedProcedure"></param>
        /// <param name="parameters">parameters.Add("@OutputParam", dbType: DbType.Int32, direction: ParameterDirection.Output)</param>
        /// <param name="ConnectionId"></param>
        /// <returns></returns>
        public async Task<(IEnumerable<T> Results, DynamicParameters? OutputParams)> LoadDataWithOutputsAsync<T>(
            string storedProcedure,
            DynamicParameters parameters,
            string ConnectionId = "ConnectionDefault")
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(_config.GetConnectionString(ConnectionId)))
                {
                    var results = await connection.QueryAsync<T>(storedProcedure,
                                                        parameters,
                                                        commandType: CommandType.StoredProcedure,
                                                        commandTimeout: 300);

                    // Output parameters are now available in 'parameters'
                    return (results, parameters);
                }
            }
            catch (Exception)
            {
                return default;
            }
        }

        public async Task<DynamicParameters?> ExecuteWithOutputsAsync(
            string storedProcedure,
            DynamicParameters parameters,
            string ConnectionId = "ConnectionDefault")
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(_config.GetConnectionString(ConnectionId)))
                {
                    await connection.ExecuteAsync(storedProcedure,
                                                  parameters,
                                                  commandType: CommandType.StoredProcedure,
                                                  commandTimeout: 300);

                    // Output parameters are now available in 'parameters'
                    return parameters;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IEnumerable<T>?> ExecuteWithOutputsAsync<T>(
           string storedProcedure,
           DynamicParameters parameters,
           string ConnectionId = "ConnectionDefault")
        {
            try
            {
                //using dapper need to return both a result and output parameters
                using (IDbConnection connection = new SqlConnection(_config.GetConnectionString(ConnectionId)))
                {
                    return await connection.QueryAsync<T>(storedProcedure,
                                                        parameters,
                                                        commandType: CommandType.StoredProcedure);
                  
                }
            }
            catch (Exception)
            {
                return default;
            }
        }

    }
}
