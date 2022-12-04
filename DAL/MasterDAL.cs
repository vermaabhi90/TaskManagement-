using System;
using System.Data;
using System.Data.SqlClient;
using TaskUtility;

namespace DAL
{
    public class MasterDAL
    {
        #region Employee Master

        public DataSet GetEmployeeList(string UserId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@UserId", SqlDbType.NVarChar);
                parameters[0].Value = UserId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetEmployeeList", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        public DataSet GetSelectedEmployeeDetails(string UserId,int EmpId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[2];
                parameters[0] = new SqlParameter("@UserId", SqlDbType.NVarChar);
                parameters[0].Value = UserId;

                parameters[1] = new SqlParameter("@EmpId", SqlDbType.Int);
                parameters[1].Value = EmpId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetSelectedEmployeeDetails", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        public int SaveEmployeeDetails(DataRow dr, string UserId, out int EmpId)
        {
            int errCode = 0;
            EmpId = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[10];

                parameters[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                parameters[0].Value = dr["EmpId"];
                parameters[0].Direction = ParameterDirection.InputOutput;

                parameters[1] = new SqlParameter("@FirstName", SqlDbType.VarChar);
                parameters[1].Value = dr["FirstName"];

                parameters[2] = new SqlParameter("@LastName", SqlDbType.VarChar);
                parameters[2].Value = dr["LastName"];

                parameters[3] = new SqlParameter("@EmailId", SqlDbType.VarChar);
                parameters[3].Value = dr["EmailId"];

                parameters[4] = new SqlParameter("@PhoneNo", SqlDbType.VarChar);
                parameters[4].Value = dr["PhoneNo"];

                parameters[5] = new SqlParameter("@isActive", SqlDbType.Bit);
                parameters[5].Value = dr["isActive"];

                parameters[6] = new SqlParameter("@Gender", SqlDbType.VarChar);
                parameters[6].Value = dr["Gender"];

                parameters[7] = new SqlParameter("@UserId", SqlDbType.NVarChar);
                parameters[7].Value = UserId;

                parameters[8] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[8].Value = 0;
                parameters[8].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SaveEmployeeDetails", parameters);
                if (parameters[8].Value != null)
                    errCode = Convert.ToInt32(parameters[8].Value.ToString());
                if (parameters[0].Value != null)
                    EmpId = Convert.ToInt32(parameters[0].Value.ToString());
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errCode;
        }

        #endregion Employee Master
    }
}
