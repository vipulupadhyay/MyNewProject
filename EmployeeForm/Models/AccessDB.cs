using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EmployeeForm.Models
{
    public class AccessDB
    {
        #region Declaration
        public string ConnectionString = ConnectionDB.DBConn.ToString();
        #endregion

        private SqlConnection con;
        //To Handle connection related activities
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["con"].ToString();
            con = new SqlConnection(constr);

        }


        public static class ConnectionDB
        {
            public static string DBConn = ConfigurationManager.ConnectionStrings["con"].ConnectionString;

        }
        public bool AddEmployee(Employee obj)
        {
            try
            {
                connection();
                SqlCommand com = new SqlCommand("InsertEmployeeData", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@UserId", obj.Person.UserId);
                com.Parameters.AddWithValue("@FullName", obj.Person.FullName);
                com.Parameters.AddWithValue("@Gender", obj.Person.Gender);
                com.Parameters.AddWithValue("@ProfileImage", obj.Person.ProfileImage);
                com.Parameters.AddWithValue("@Passward", Util.Encryptword(obj.Person.Passward));
                com.Parameters.AddWithValue("@Email", obj.Person.Email);
                com.Parameters.AddWithValue("@BankName", obj.Bank.BankName);
                com.Parameters.AddWithValue("@DegreeId", obj.Education.DegreeId);
                com.Parameters.AddWithValue("@SportName", obj.Sport.SportName);
                con.Open();
                int i = com.ExecuteNonQuery();
                con.Close();
                if (i >= 1)
                {
                    bool IsMailSent = false;
                    if (!string.IsNullOrEmpty(obj.Person.Email) && string.IsNullOrEmpty(obj.Person.UserId.ToString()))
                    {
                        IsMailSent = SendMail.SendMails(obj.Person.Email);
                    }
                    return true;

                }
                else
                {

                    return false;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }


        public EmployeeManagementList GetUserList(DataTableProperty DataTableProperty)
        {
            try
            {
                EmployeeManagementList lstUserModel = new EmployeeManagementList();

                using (SqlConnection Mycn = new SqlConnection(ConnectionString))
                {

                    Mycn.Open();

                    using (SqlCommand cmd = new SqlCommand("GetAllUsers", con))
                    {
                        cmd.Connection = Mycn;

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int)).Value = UserID;
                        cmd.Parameters.Add(new SqlParameter("@PageNo", SqlDbType.Int)).Value = DataTableProperty.PageNo;
                        cmd.Parameters.Add(new SqlParameter("@RecordPerPage", SqlDbType.Int)).Value = DataTableProperty.RecordPerPage;
                        cmd.Parameters.Add(new SqlParameter("@SortField", SqlDbType.VarChar, 50)).Value = DataTableProperty.SortField;
                        cmd.Parameters.Add(new SqlParameter("@SortOrder", SqlDbType.VarChar, 50)).Value = DataTableProperty.SortOrder;
                        cmd.Parameters.Add(new SqlParameter("@Filter", SqlDbType.VarChar, 50)).Value = DataTableProperty.Filter;
                        cmd.Parameters.Add(new SqlParameter("@FullName", SqlDbType.VarChar, 50)).Value = DataTableProperty.FullName;
                        cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar, 50)).Value = DataTableProperty.Email;

                        DataSet dataset = new DataSet();

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            // Fill the DataSet using default values for DataTable names, etc
                            da.Fill(dataset);

                            Mycn.Close();
                            Mycn.Dispose();
                            cmd.Dispose();

                        }

                        if (dataset.Tables.Count > 0)
                        {
                            if (dataset.Tables[2].Rows.Count > 0)
                            {
                                List<EmployeeInfo> objEmployeeInfo = new List<EmployeeInfo>();
                                for (int i = 0; i < dataset.Tables[2].Rows.Count; i++)
                                {
                                    EmployeeInfo ObjEmployeeInfo = new EmployeeInfo();

                                    ObjEmployeeInfo.UserId = Convert.ToString(dataset.Tables[2].Rows[i]["UserId"]);
                                    ObjEmployeeInfo.FullName = Convert.ToString(dataset.Tables[2].Rows[i]["FullName"]);
                                    ObjEmployeeInfo.Email = dataset.Tables[2].Rows[i]["Email"].ToString();
                                    ObjEmployeeInfo.ProfileImage = dataset.Tables[2].Rows[i]["ProfileImage"].ToString();

                                    objEmployeeInfo.Add(ObjEmployeeInfo);
                                }

                                lstUserModel.EmployeeInfoList = objEmployeeInfo;
                            }

                            lstUserModel.totalNumberofRecord = Convert.ToInt32(dataset.Tables[0].Rows[0][0]);
                            lstUserModel.filteredRecord = Convert.ToInt32(dataset.Tables[1].Rows[0][0]);
                        }


                    }
                    return lstUserModel;


                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {


            }


        }

        public EmployeeInfo GetUserByUserId(int? UserId)
        {
            try
            {
                EmployeeInfo ObjEmployeeInfo = new EmployeeInfo();


                using (SqlConnection Mycn = new SqlConnection(ConnectionString))
                {

                    Mycn.Open();

                    using (SqlCommand cmd = new SqlCommand("GetUserById", con))
                    {
                        cmd.Connection = Mycn;

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int)).Value = UserId;

                        DataSet dataset = new DataSet();

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dataset);

                            Mycn.Close();
                            Mycn.Dispose();
                            cmd.Dispose();
                        }

                        if (dataset.Tables.Count > 0)
                        {
                            if (dataset.Tables[0].Rows.Count > 0)
                            {
                                ObjEmployeeInfo.UserId = Convert.ToString(dataset.Tables[0].Rows[0]["UserId"]);
                                ObjEmployeeInfo.FullName = Convert.ToString(dataset.Tables[0].Rows[0]["FullName"]);
                                ObjEmployeeInfo.Email = dataset.Tables[0].Rows[0]["Email"].ToString();
                                ObjEmployeeInfo.ProfileImage = dataset.Tables[0].Rows[0]["ProfileImage"].ToString();
                                ObjEmployeeInfo.BankName = dataset.Tables[0].Rows[0]["BankName"].ToString();
                                ObjEmployeeInfo.Gender = dataset.Tables[0].Rows[0]["Gender"].ToString();
                                ObjEmployeeInfo.Passward =Util.Decryptword( dataset.Tables[0].Rows[0]["Passward"].ToString());
                                ObjEmployeeInfo.SportName = dataset.Tables[0].Rows[0]["SportName"].ToString();
                                ObjEmployeeInfo.EducationId = dataset.Tables[0].Rows[0]["EducationId"].ToString();
                                
                            }

                        }


                    }

                    return ObjEmployeeInfo;


                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {


            }


        }

        public bool Delete(int? UserId)
        {
            try
            {
                connection();
                SqlCommand com = new SqlCommand("deleteUserById", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@UserId", UserId);
                con.Open();
                int i = com.ExecuteNonQuery();
                con.Close();
                if (i >= 1)
                {

                    return true;

                }
                else
                {

                    return false;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }


    }
}