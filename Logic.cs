using StudentsCRUDWithImageMVC.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace StudentsCRUDWithImageMVC
{
    public class Logic
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["dbStd"].ToString());

        #region === Display All Details ===
        public List<ListsModel> GetAllStudentsDetails()
        {
            List<ListsModel> LMD = new List<ListsModel>();
            try
            {
                SqlCommand cmd = new SqlCommand("[dbo].[GetAllStudentsDetails_SP]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter SDA = new SqlDataAdapter(cmd);
                DataTable DT = new DataTable();
                SDA.Fill(DT);

                foreach (DataRow row in DT.Rows)
                {
                    ListsModel std = new ListsModel
                    {
                        RollNo = Convert.ToInt32(row["RollNo"]),
                        Name = row["Name"].ToString(),
                        City = row["City"].ToString(),
                        State = row["State"].ToString(),
                        Fees = Convert.ToInt32(row["Fees"]),
                        JoiningDate = Convert.ToDateTime(row["JoiningDate"]),
                        Image = row["Image"] == DBNull.Value ? null : (byte[])row["Image"]
                    };
                    LMD.Add(std);
                }
            }
            catch (Exception ex)
            {
                // Log or handle error
                Console.WriteLine("Error in row : " + ex.Message);
            }
            return LMD;
        }
        #endregion

        #region === insert Student Logic ===
        public void insertDetails(InsertModel insertStdValues)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("[dbo].[InsertValuesInStudents_SP]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Name", insertStdValues.Name);
                cmd.Parameters.AddWithValue("@City", insertStdValues.City);
                cmd.Parameters.AddWithValue("@State", insertStdValues.State);
                cmd.Parameters.AddWithValue("@Fees", insertStdValues.Fees);
                cmd.Parameters.AddWithValue("@JoiningDate", insertStdValues.JoiningDate);
                cmd.Parameters.AddWithValue("@Image", insertStdValues.Image ?? (object)DBNull.Value);


                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region === Edit Student Details ===
        public void EditDetails(EditModel EditStdValues)
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("[dbo].[UpadateStudentValues_SP]", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@RollNo", EditStdValues.RollNo);
                cmd.Parameters.AddWithValue("@Name", EditStdValues.Name);
                cmd.Parameters.AddWithValue("@City", EditStdValues.City);
                cmd.Parameters.AddWithValue("@State", EditStdValues.State);
                cmd.Parameters.AddWithValue("@Fees", EditStdValues.Fees);
                cmd.Parameters.AddWithValue("@JoiningDate", EditStdValues.JoiningDate);
                cmd.Parameters.AddWithValue("@Image", EditStdValues.Image ?? (object)DBNull.Value);


                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        #endregion

        #region === Delete Student Details ===
        public void DeleteDetails(int id)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("[dbo].[DeleteStudentValues_SP]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RollNo", id);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        #endregion
    }

}