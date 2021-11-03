using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using MySqlConnector;

namespace WpfTetris.Utils
{
    public class MysqlManager
    {
        public const string DbInfoFile = "Db.info";

        private static MySqlConnection _MySqlConnection;

        private MysqlManager(string server, string db, string login, string pass, string tableName)
        {
            _MySqlConnection = new MySqlConnection($"Server = {server}; Database = {db}; Uid = {login}; Pwd = {pass};");
            TableName = tableName;
        }

        public string TableName { get; }
        public static MysqlManager Instance { get; private set; }

        public static MysqlManager
            CreateInstance(string server, string db, string login, string pass, string tableName)
        {
            return Instance != null ? Instance : Instance = new MysqlManager(server, db, login, pass, tableName);
        }

        public static MysqlManager CreateInstanceFromFile(string FilePath)
        {
            var data = File.ReadAllText(FilePath).Trim().Split(';');
            if (data.Length >= 5)
                return CreateInstance(data[0], data[1], data[2], data[3], data[4]);

            throw new Exception(
                "Количество параметров в файле несоотвуествует ожидаемому. ожидамемое количество:5");
        }

        public async Task<List<(string name, int score)>> GetScores()
        {
            MySqlCommand command = null;
            try
            {
                var resList = new List<(string name, int score)>();
                //Открытие соедмнения
                await OpenConnection();
                //отправка запроса
                command = new MySqlCommand($"SELECT * FROM {TableName}", _MySqlConnection);
                var reader = await command.ExecuteReaderAsync();

                //Чтение данных
                while (await reader.ReadAsync())
                {
                    (string name, int score) resTuple = (string.Empty, 0);

                    int ord = reader.GetOrdinal("name");
                    var nameValue =reader.GetValue(ord);
                    var scoreValue =  (int) reader.GetUInt32(reader.GetOrdinal("score"));

                    if (nameValue is string) resTuple.name = (string) nameValue;

                    resTuple.score = scoreValue;
                    resList.Add(resTuple);
                }

                //закрытие чтения и освобождение памяти
                reader.Close();
                await reader.DisposeAsync();

                //возврат результата
                return resList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //Закрытие соединения
                await CloseRequest(command);
            }
        }

        public async Task InsertScore(string guid, int score)
        {
            await InsertScore(score, guid, null);
        }

        public async Task InsertScore(int score, string guid, string name)
        {
            MySqlCommand command = null;
            try
            {
                //Открытие соединения
                await OpenConnection();

                //формирование строки запроса
                var insertCommand = $"INSERT INTO {TableName} (guid,name,score)";

                insertCommand += $"VALUES ('{guid}','{name}',{score})";

                //отправка комманды на запись
                command = new MySqlCommand(insertCommand, _MySqlConnection);
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //Закрытие соединения
                await CloseRequest(command);
            }
        }


        public async Task UpdateScore(int id,int score)
        {
            MySqlCommand command = null;
            try
            {
                await OpenConnection();
                command = new MySqlCommand($"UPDATE {TableName} SET score = {score} WHERE id='{id}';",
                    _MySqlConnection);
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                await CloseRequest(command);
            }
        }

        public async Task<int> GetIdFromNameAndGuid(string guid, string name)
        {
            MySqlCommand command = null;
            try
            {
                await OpenConnection();
                command = new MySqlCommand($"SELECT id FROM {TableName} WHERE guid='{guid}' AND name = '{name}'", _MySqlConnection);
                var res = await command.ExecuteScalarAsync();

                return res == null ? -1 : (int) res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                await CloseRequest(command);
            }
        }

        private async Task CloseRequest(MySqlCommand command)
        {
            if (command != null)
            {
                await command.DisposeAsync();
                if (_MySqlConnection != null) await _MySqlConnection.CloseAsync();
            }
        }

        private async Task OpenConnection()
        {
            if (_MySqlConnection.State == ConnectionState.Closed)
                await _MySqlConnection.OpenAsync();
        }
    }
}