using Microsoft.Data.Sqlite;

namespace StudentManagent.Data;

public static class Database
{
    private static readonly string connectionString = "Data Source=StudentDB.db";

    public static void Initialize()
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        string tableSql = @"
            CREATE TABLE IF NOT EXISTS Students (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                NIM TEXT NOT NULL,
                Nama TEXT NOT NULL,
                Jurusan TEXT NOT NULL,
                Gender TEXT NOT NULL,
                Email TEXT
            );";

        using var command = new SqliteCommand(tableSql, connection);
        command.ExecuteNonQuery();
    }
}