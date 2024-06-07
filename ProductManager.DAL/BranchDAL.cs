using Microsoft.Data.SqlClient;
using ProductManager.Core.Interfaces;
using ProductManager.Core.Models;

namespace ProductManager.DAL
{
    public class BranchDAL : IBranchDAL
    {
        private readonly string _conn;
        private SqlTransaction _transaction;

        public BranchDAL(string conn)
        {
            _conn = conn;
        }

        public List<Branch> GetBranches()
        {
            List<Branch> branches = new();

            using (SqlConnection conn = new SqlConnection(_conn))
            {
                SqlCommand cmd = new SqlCommand("SELECT ID, Name, Address, PostalCode, City FROM Branch", conn);

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        branches.Add(new Branch()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            Name = reader["Name"].ToString()!,
                            Address = reader["Address"].ToString()!,
                            PostalCode = reader["PostalCode"].ToString()!,
                            City = reader["City"].ToString()!
                        });
                    }
                }
            }

            return branches;
        }

        public Branch GetBranch(int id)
        {
            Branch branch = new();

            using (SqlConnection conn = new SqlConnection(_conn))
            {
                SqlCommand cmd = new SqlCommand("SELECT ID, Name, Address, PostalCode, City FROM Branch WHERE ID = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Connection = conn;

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        branch.ID = Convert.ToInt32(reader["ID"]);
                        branch.Name = reader["Name"].ToString()!;
                        branch.Address = reader["Address"].ToString()!;
                        branch.PostalCode = reader["PostalCode"].ToString()!;
                        branch.City = reader["City"].ToString()!;
                    }
                }
            }

            return branch;
        }        

        public string CreateBranch(string name, string address, string postalcode, string city)
        {
            string succesMessage = string.Empty;

            using (SqlConnection conn = new SqlConnection(_conn))
            {
                conn.Open();
                _transaction = conn.BeginTransaction();

                SqlCommand cmd = new SqlCommand("INSERT INTO Branch (Name, Address, PostalCode, City) VALUES (@name, @address, @postalcode, @city)");
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@address", address);
                cmd.Parameters.AddWithValue("@postalcode", postalcode);
                cmd.Parameters.AddWithValue("@city", city);
                cmd.Connection = conn;
                cmd.Transaction = _transaction;

                try
                {
                    cmd.ExecuteNonQuery();
                    _transaction.Commit();
                    succesMessage = "Branch succesfully created!";
                }
                catch (SqlException sqlEx)
                {
                    // Number 2628 = Data Exceeds Field Max Length
                    if (sqlEx.Number == 2628) succesMessage = "Branch could not be created because the inserted data would exceed the max length!";

                    else
                    {
                        succesMessage = "Yet Unknown SQL Error!";
                        throw new Exception(sqlEx.Message);
                    }
                }
                catch (Exception ex)
                {
                    _transaction.Rollback();
                    succesMessage = "Unknown Error!";
                    throw new Exception(ex.Message);
                }
            }

            return succesMessage;
        }

        public string UpdateBranch(int id, string name, string address, string postalcode, string city)
        {
            string succesMessage = string.Empty;

            using (SqlConnection conn = new SqlConnection(_conn))
            {
                conn.Open();
                _transaction = conn.BeginTransaction();

                SqlCommand cmd = new SqlCommand("UPDATE Branch SET Name = @name, Address = @address, PostalCode = @postalcode, City = @city WHERE ID = @ID");
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@address", address);
                cmd.Parameters.AddWithValue("@postalcode", postalcode);
                cmd.Parameters.AddWithValue("@city", city);
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Connection = conn;
                cmd.Transaction = _transaction;

                try
                {
                    cmd.ExecuteNonQuery();
                    _transaction.Commit();
                    succesMessage = "Branch succesfully updated!";
                }
                catch (SqlException sqlEx)
                {
                    // Number 2628 = Data Exceeds Field Max Length
                    if (sqlEx.Number == 2628) succesMessage = "Branch could not be updated because the inserted data would exceed the max length!";

                    else
                    {
                        succesMessage = "Yet Unknown SQL Error!";
                        throw new Exception(sqlEx.Message);
                    }
                }
                catch (Exception ex)
                {
                    _transaction.Rollback();
                    succesMessage = "Unknown Error!";
                    throw new Exception(ex.Message);
                }
            }

            return succesMessage;
        }

        public string DeleteBranch(int id)
        {
            string succesMessage = string.Empty;

            using (SqlConnection conn = new SqlConnection(_conn))
            {
                conn.Open();
                _transaction = conn.BeginTransaction();

                SqlCommand cmd = new SqlCommand("DELETE FROM Branch WHERE ID = @ID");
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Connection = conn;
                cmd.Transaction = _transaction;

                try
                {
                    cmd.ExecuteNonQuery();
                    _transaction.Commit();
                    succesMessage = "Branch succesfully deleted!";
                }
                catch (SqlException sqlEx)
                {
                    // Number 547 = The DELETE statement conflicted with the REFERENCE constraint
                    if (sqlEx.Number == 547) succesMessage = "Branch could not be deleted because it is linked to an assortment";

                    else
                    {
                        succesMessage = "Yet Unknown SQL Error!";
                        throw new Exception(sqlEx.Message);
                    }
                }
                catch (Exception ex)
                {
                    _transaction.Rollback();
                    succesMessage = "Unknown Error!";
                    throw new Exception(ex.Message);
                }
            }

            return succesMessage;
        }

        public string GetBranchName(int id)
        {
            string name = string.Empty;

            using (SqlConnection conn = new(_conn))
            {
                SqlCommand cmd = new SqlCommand("SELECT Name FROM Branch WHERE ID = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Connection = conn;

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        name = reader["Name"].ToString()!;
                    }
                }
            }

            return name;
        }
    }
}
