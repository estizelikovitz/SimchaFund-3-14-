using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimchaFundLibrary
{
    public class SimchaFundDB
    {
        private string _connectionString;
        public SimchaFundDB(string connectionString)
        {
            _connectionString = connectionString;

        }

        public List<Simcha> GetSimchos()
        {
            SqlConnection connection = new(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Simcha";
            connection.Open();
            List<Simcha> simchos = new();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                simchos.Add(new Simcha
                {
                    Id = (int)reader["Id"],
                    Name = (string)reader["Name"],
                    Date = (DateTime)reader["Date"]
                });
            }

            return simchos;
        }
        public List<Contributor> GetContributors()
        {
            SqlConnection connection = new(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Contributor";
            connection.Open();
            List<Contributor> contributors = new();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                contributors.Add(new Contributor
                {
                    Id = (int)reader["Id"],
                    FirstName = (string)reader["FirstName"],
                    LastName = (string)reader["LastName"],
                    CellNumber=(string)reader["CellNumber"],
                    AlwaysIncluded = (bool)reader["AlwaysIncluded"]
                });
            }

            return contributors;
        }
        public List<Contribution> GetContributions()
        {
            SqlConnection connection = new(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Contributions";
            connection.Open();
            List<Contribution> contributions = new();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                contributions.Add(new Contribution
                {
                    SimchaId = (int)reader["SimchaId"],
                    ContributorId = (int)reader["ContributorId"],
                    Amount = (decimal)reader["Amount"]
                }) ;
            }

            return contributions;
        }
        public List<Deposit> GetDeposits()
        {
            SqlConnection connection = new(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Deposits";
            connection.Open();
            List<Deposit> deposits = new();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                deposits.Add(new Deposit
                {
                    ContributorId = (int)reader["ContributorId"],
                    Amount = (decimal)reader["Amount"],
                    Date = (DateTime)reader["Date"]

                });
            }

            return deposits;
        }

        public int AddContributor(Contributor contributor)
        {
            SqlConnection conn = new(_connectionString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Contributor (FirstName, LastName, CellNumber, AlwaysIncluded) " +
                "VALUES (@fname, @lname, @cell, @included)"+
                "Select Scope_Identity()";
            cmd.Parameters.AddWithValue("@fname", contributor.FirstName);
            cmd.Parameters.AddWithValue("@lname", contributor.LastName);
            cmd.Parameters.AddWithValue("@cell", contributor.CellNumber);
            cmd.Parameters.AddWithValue("@included", contributor.AlwaysIncluded);
            conn.Open();
            return (int)(decimal)cmd.ExecuteScalar();

        }
        public void AddDeposit(Deposit deposit)
        {
            SqlConnection conn = new(_connectionString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Deposits (ContributorId, Amount, Date) " +
                "VALUES (@contribId, @amount, @date)";
            cmd.Parameters.AddWithValue("@contribId", deposit.ContributorId);
            cmd.Parameters.AddWithValue("@amount", deposit.Amount);
            cmd.Parameters.AddWithValue("@date", deposit.Date);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public void AddSimcha(Simcha simcha)
        {
            SqlConnection conn = new(_connectionString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Simcha (Name, Date) " +
                "VALUES (@name, @date)";
            cmd.Parameters.AddWithValue("@name", simcha.Name);
            cmd.Parameters.AddWithValue("@date", simcha.Date);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public void EditContributor(Contributor contributor)
        {
            SqlConnection connection = new(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE Contributor " +
                              "SET FirstName=@fname, LastName=@lname, CellNumber=@cell, AlwaysIncluded=@included " +
                              "WHERE Id=@id";
            cmd.Parameters.AddWithValue("@fname", contributor.FirstName);
            cmd.Parameters.AddWithValue("@lname", contributor.LastName);
            cmd.Parameters.AddWithValue("@cell", contributor.CellNumber);
            cmd.Parameters.AddWithValue("@included", contributor.AlwaysIncluded);
            cmd.Parameters.AddWithValue("@Id", contributor.Id);
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }
        public void AddContribution(Contribution contribution)
        {
            SqlConnection conn = new(_connectionString);
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Contributions (ContributorId, SimchaId, Amount) " +
                "VALUES (@contribId, @simchaId, @amount)";
            cmd.Parameters.AddWithValue("@contribId", contribution.ContributorId);
            cmd.Parameters.AddWithValue("@simchaId", contribution.SimchaId);
            cmd.Parameters.AddWithValue("@amount", contribution.Amount);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public void Delete(int id)
        {
            SqlConnection connection = new(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM Contributions WHERE SimchaId=@id";
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();
            cmd.ExecuteNonQuery();
        }

    }
}
