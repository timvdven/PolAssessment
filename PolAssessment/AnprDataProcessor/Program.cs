using System.Data.SQLite;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// app.Run();

// System.Data.SQLite.SQLiteConnection.CreateFile("Data/AnprData.db");

string connectionString = "Data Source=Data/AnprData.db;Version=3;";
SQLiteConnection m_dbConnection = new SQLiteConnection(connectionString);
m_dbConnection.Open();

// varchar will likely be handled internally as TEXT
// the (20) will be ignored
// see https://www.sqlite.org/datatype3.html#affinity_name_examples
string sql = "Create Table highscores (name varchar(20), score int)";
// you could also write sql = "CREATE TABLE IF NOT EXISTS highscores ..."
SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
command.ExecuteNonQuery();

sql = "Insert into highscores (name, score) values ('Me', 9001)";
command = new SQLiteCommand(sql, m_dbConnection);
command.ExecuteNonQuery();

m_dbConnection.Close();