using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskUtility;

namespace DAL
{
    public class TaskDAL
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
        public DataSet GetSelectedEmployeeDetails(string UserId, int EmpId)
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

                parameters[9] = new SqlParameter("@Role", SqlDbType.VarChar);
                parameters[9].Value = dr["Role"];

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

        public int CreateSysUser(int EmpId, string UserId, string Email, string PasswordHash, string SecurityStamp, string UserName)
        {
            int Errorcode = 0;
            try
            {
                SqlParameter[] objParam = new SqlParameter[8];

                objParam[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                objParam[0].Value = EmpId;

                objParam[1] = new SqlParameter("@UserId", SqlDbType.NVarChar);
                objParam[1].Value = UserId;

                objParam[2] = new SqlParameter("@Email", SqlDbType.NVarChar);
                objParam[2].Value = Email;

                objParam[3] = new SqlParameter("@PasswordHash", SqlDbType.NVarChar);
                objParam[3].Value = PasswordHash;

                objParam[4] = new SqlParameter("@SecurityStamp", SqlDbType.NVarChar);
                objParam[4].Value = SecurityStamp;

                objParam[5] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                objParam[5].Value = 0;
                objParam[5].Direction = ParameterDirection.InputOutput;

                objParam[6] = new SqlParameter("@UserName", SqlDbType.NVarChar);
                objParam[6].Value = UserName;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_CreateNewUser", objParam);
                Errorcode = Convert.ToInt32(objParam[5].Value.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Errorcode;
        }

        public int CheckEmaillAlreadyExits(int EmpId, string Email)
        {
            int Errorcode = 0;
            try
            {
                SqlParameter[] objParam = new SqlParameter[3];

                objParam[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                objParam[0].Value = EmpId;

                objParam[1] = new SqlParameter("@Email", SqlDbType.VarChar);
                objParam[1].Value = Email;

                objParam[2] = new SqlParameter("@Errorcode", SqlDbType.Int);
                objParam[2].Value = Errorcode;
                objParam[2].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_CheckEmailAlreadyExits", objParam);
                Errorcode = Convert.ToInt32(objParam[2].Value.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Errorcode;
        }
        public int CheckMobilAlreadyExits(int EmpId,  string Mobile)
            {
                int Errorcode = 0;
                try
                {
                    SqlParameter[] objParam = new SqlParameter[3];

                    objParam[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                    objParam[0].Value = EmpId;

                    objParam[1] = new SqlParameter("@Mobile", SqlDbType.VarChar);
                    objParam[1].Value = Mobile;

                    objParam[2] = new SqlParameter("@Errorcode", SqlDbType.Int);
                    objParam[2].Value = Errorcode;
                    objParam[2].Direction = ParameterDirection.InputOutput;

                    SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_CheckMobileAlreadyExists", objParam);
                    Errorcode = Convert.ToInt32(objParam[2].Value.ToString());

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return Errorcode;
            }

        public int VerifyOTP(int EmpId, string Email, string OTP)
        {
            int Errorcode = 0;
            try
            {
                SqlParameter[] objParam = new SqlParameter[4];

                objParam[0] = new SqlParameter("@EmpId", SqlDbType.Int);
                objParam[0].Value = EmpId;

                objParam[1] = new SqlParameter("@Email", SqlDbType.VarChar);
                objParam[1].Value = Email;

                objParam[2] = new SqlParameter("@OTP", SqlDbType.VarChar);
                objParam[2].Value = OTP;

                objParam[3] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                objParam[3].Value = 0;
                objParam[3].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_VerifyOTP", objParam);
                Errorcode = Convert.ToInt32(objParam[3].Value.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Errorcode;
        }

        #endregion Employee Master

        #region Send LogIn OTP 

        public int SendOTP( string EmailOrPhone)
        {
            int Errorcode = 0;
            try
            {
                SqlParameter[] objParam = new SqlParameter[2];

                objParam[0] = new SqlParameter("@UEmailOrPhone", SqlDbType.VarChar);
                objParam[0].Value = EmailOrPhone;

                objParam[1] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                objParam[1].Value = 0;
                objParam[1].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SendOTPForVerificationDetails", objParam);
                Errorcode = Convert.ToInt32(objParam[1].Value.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Errorcode;
        }

        #endregion Send LogIn OTP 

        #region Task Master
        public DataSet GetTaskList(string UserId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@UserId", SqlDbType.NVarChar);
                parameters[0].Value = UserId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetTaskList", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }
        public DataSet GetSelectedTaskDetails(int TaskId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];

                parameters[0] = new SqlParameter("@TaskId", SqlDbType.Int);
                parameters[0].Value = TaskId;

                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetSelectedTaskDetails", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }
        public int SaveTaskDetails(DataRow dr, string UserId, out int TaskId)
        {
            int errCode = 0;
            TaskId = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[15];

                parameters[0] = new SqlParameter("@TaskId", SqlDbType.Int);
                parameters[0].Value = dr["TaskId"];
                parameters[0].Direction = ParameterDirection.InputOutput;

                parameters[1] = new SqlParameter("@TaskName", SqlDbType.VarChar);
                parameters[1].Value = dr["TaskName"];

                parameters[2] = new SqlParameter("@TaskDescription", SqlDbType.VarChar);
                parameters[2].Value = dr["TaskDescription"];

                parameters[3] = new SqlParameter("@TaskDate", SqlDbType.DateTime);
                parameters[3].Value = dr["TaskDate"];

                parameters[4] = new SqlParameter("@ccMail", SqlDbType.VarChar);
                parameters[4].Value = dr["ccMail"];

                parameters[5] = new SqlParameter("@isActive", SqlDbType.Bit);
                parameters[5].Value = dr["isActive"];

                parameters[6] = new SqlParameter("@StatusId", SqlDbType.VarChar);
                parameters[6].Value = dr["StatusId"];

                parameters[7] = new SqlParameter("@UserId", SqlDbType.NVarChar);
                parameters[7].Value = UserId;

                parameters[8] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[8].Value = 0;
                parameters[8].Direction = ParameterDirection.InputOutput;

                parameters[9] = new SqlParameter("@FrequencyType", SqlDbType.Char);
                parameters[9].Value = dr["FrequencyType"];

                parameters[10] = new SqlParameter("@WeekDays", SqlDbType.VarChar);
                parameters[10].Value = dr["WeekDays"];

                parameters[11] = new SqlParameter("@WeekNo", SqlDbType.Int);
                parameters[11].Value = dr["WeekNo"];

                parameters[12] = new SqlParameter("@MonthNo", SqlDbType.Int);
                parameters[12].Value = dr["MonthNo"];

                parameters[13] = new SqlParameter("@NthDay", SqlDbType.Int);
                parameters[13].Value = dr["NthDay"];

                parameters[14] = new SqlParameter("@AssignedToEmpId", SqlDbType.Int);
                parameters[14].Value = dr["AssignedToEmpId"];

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_SaveTaskDetails", parameters);
                if (parameters[8].Value != null)
                    errCode = Convert.ToInt32(parameters[8].Value.ToString());
                if (parameters[0].Value != null)
                    TaskId = Convert.ToInt32(parameters[0].Value.ToString());
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errCode;
        }
        public DataSet GetPendingApprovalTaskList(string UserId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@UserId", SqlDbType.NVarChar);
                parameters[0].Value = UserId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetPendingApprovalTaskList", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }
        public int UpdateTaskStatusDetails(string UserId, int TaskId,int StatusId,string Remark)
        {
            int errCode = 0;
            try
            {
                SqlParameter[] parameters = new SqlParameter[5];

                parameters[0] = new SqlParameter("@TaskId", SqlDbType.Int);
                parameters[0].Value = TaskId;

                parameters[1] = new SqlParameter("@StatusId", SqlDbType.VarChar);
                parameters[1].Value = StatusId;

                parameters[2] = new SqlParameter("@Remark", SqlDbType.VarChar);
                parameters[2].Value = Remark;

                parameters[3] = new SqlParameter("@UserId", SqlDbType.NVarChar);
                parameters[3].Value = UserId;

                parameters[4] = new SqlParameter("@ErrorCode", SqlDbType.Int);
                parameters[4].Value = 0;
                parameters[4].Direction = ParameterDirection.InputOutput;

                SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_UpdateTaskDetails", parameters);
                if (parameters[4].Value != null)
                    errCode = Convert.ToInt32(parameters[4].Value.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errCode;
        }

        public DataSet GetEmpDrpDwnList(int TaskId)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@TaskId", SqlDbType.Int);
                parameters[0].Value = TaskId;
                ds = SqlHelper.ExecuteDataset(Common.SqlConnectionString, CommandType.StoredProcedure, "sp_GetEmpDrpDwnList", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }
        #endregion Task Master
    }
}
