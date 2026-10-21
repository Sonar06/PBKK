using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using StudentManagent.Models;

namespace StudentManagent.Data;

public class StudentRepository
{
    private readonly string connectionString = "Data Source=StudentDB.db";

    public List<Student> GetAll()
    {
        var students = new List<Student>();
        using var connection = new SqliteConnection(connectionString);
        string sql = "SELECT Id, NIM, Nama, Jurusan, Gender, Email FROM Students ORDER BY Id DESC";

        using var command = new SqliteCommand(sql, connection);
        connection.Open();
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            students.Add(new Student
            {
                Id = Convert.ToInt32(reader["Id"]),
                NIM = reader["NIM"].ToString()!,
                Nama = reader["Nama"].ToString()!,
                Jurusan = reader["Jurusan"].ToString()!,
                Gender = reader["Gender"].ToString()!,
                Email = reader["Email"].ToString()!
            });
        }
        return students;
    }

    public void Insert(Student student)
    {
        using var connection = new SqliteConnection(connectionString);
        string sql = @"INSERT INTO Students (NIM, Nama, Jurusan, Gender, Email)
                       VALUES (@NIM, @Nama, @Jurusan, @Gender, @Email)";

        using var command = new SqliteCommand(sql, connection);
        command.Parameters.AddWithValue("@NIM", student.NIM);
        command.Parameters.AddWithValue("@Nama", student.Nama);
        command.Parameters.AddWithValue("@Jurusan", student.Jurusan);
        command.Parameters.AddWithValue("@Gender", student.Gender);
        command.Parameters.AddWithValue("@Email", (object?)student.Email ?? DBNull.Value);

        connection.Open();
        command.ExecuteNonQuery();
    }

    public void Update(Student student)
    {
        using var connection = new SqliteConnection(connectionString);
        string sql = @"UPDATE Students 
                       SET NIM = @NIM, Nama = @Nama, Jurusan = @Jurusan, Gender = @Gender, Email = @Email 
                       WHERE Id = @Id";

        using var command = new SqliteCommand(sql, connection);
        command.Parameters.AddWithValue("@Id", student.Id);
        command.Parameters.AddWithValue("@NIM", student.NIM);
        command.Parameters.AddWithValue("@Nama", student.Nama);
        command.Parameters.AddWithValue("@Jurusan", student.Jurusan);
        command.Parameters.AddWithValue("@Gender", student.Gender);
        command.Parameters.AddWithValue("@Email", (object?)student.Email ?? DBNull.Value);

        connection.Open();
        command.ExecuteNonQuery();
    }

    public void Delete(int id)
    {
        using var connection = new SqliteConnection(connectionString);
        string sql = "DELETE FROM Students WHERE Id = @Id";

        using var command = new SqliteCommand(sql, connection);
        command.Parameters.AddWithValue("@Id", id);

        connection.Open();
        command.ExecuteNonQuery();
    }

    public List<Student> Search(string keyword)
    {
        var students = new List<Student>();
        using var connection = new SqliteConnection(connectionString);
        string sql = @"SELECT Id, NIM, Nama, Jurusan, Gender, Email 
                       FROM Students 
                       WHERE NIM LIKE @Keyword OR Nama LIKE @Keyword OR Jurusan LIKE @Keyword 
                       ORDER BY Id DESC";

        using var command = new SqliteCommand(sql, connection);
        command.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");

        connection.Open();
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            students.Add(new Student
            {
                Id = Convert.ToInt32(reader["Id"]),
                NIM = reader["NIM"].ToString()!,
                Nama = reader["Nama"].ToString()!,
                Jurusan = reader["Jurusan"].ToString()!,
                Gender = reader["Gender"].ToString()!,
                Email = reader["Email"].ToString()!
            });
        }
        return students;
    }
}