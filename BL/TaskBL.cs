using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BL.TaskModel;

namespace BL
{
    public class TaskBL
    {

        private TaskDAL DALObj = new TaskDAL();

        #region Employee Master
        public List<EmployeeModel> GetEmployeeList(string UserId)
        {
            List<EmployeeModel> EmpLst = new List<EmployeeModel>();
            DataSet ds = DALObj.GetEmployeeList(UserId);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        EmployeeModel Model = new EmployeeModel();
                        Model.EmpId = Convert.ToInt32(dr["EmpId"].ToString());
                        Model.FirstName = dr["FirstName"].ToString();
                        Model.LastName = dr["LastName"].ToString();
                        Model.EmailId = dr["EmailId"].ToString();
                        Model.PhoneNo = dr["PhoneNo"].ToString();
                        Model.isActive = Convert.ToBoolean(dr["isActive"].ToString());
                        Model.gender = dr["gender"].ToString();
                        Model.CreatedBy = dr["CreatedBy"].ToString();
                        Model.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                        Model.ModifiedBy = dr["ModifiedBy"].ToString();
                        Model.ModifiedDate = Convert.ToDateTime(dr["ModifiedDate"].ToString());
                        EmpLst.Add(Model);
                    }
                }
            }
            return EmpLst;
        }
        public EmployeeModel GetSelectedEmployeeDetails(string UserId, int EmpId)
        {
            EmployeeModel Model = new EmployeeModel();
            DataSet ds = DALObj.GetSelectedEmployeeDetails(UserId, EmpId);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Model.EmpId = Convert.ToInt32(ds.Tables[0].Rows[0]["EmpId"].ToString());
                        Model.FirstName = ds.Tables[0].Rows[0]["FirstName"].ToString();
                        Model.LastName = ds.Tables[0].Rows[0]["LastName"].ToString();
                        Model.EmailId = ds.Tables[0].Rows[0]["EmailId"].ToString();
                        Model.PhoneNo = ds.Tables[0].Rows[0]["PhoneNo"].ToString();
                        Model.isActive = Convert.ToBoolean(ds.Tables[0].Rows[0]["isActive"].ToString());
                        Model.gender = ds.Tables[0].Rows[0]["gender"].ToString();
                        Model.User_Id = ds.Tables[0].Rows[0]["UserId"].ToString();
                        Model.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                        Model.CreatedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CreatedDate"].ToString());
                        Model.ModifiedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                        Model.ModifiedDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["ModifiedDate"].ToString());
                    }

                }
            }
            return Model;
        }
        public int SaveEmployeeDetails(EmployeeModel Model, out int EmpId, string UserId)
        {
            DataSet ds = new DataSet();
            EmpId = 0;
            int errorcode = 0;
            try
            {
                ds = DALObj.GetSelectedEmployeeDetails(UserId, 0);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].Rows.Clear();
                        DataRow dr = ds.Tables[0].NewRow();
                        dr["EmpId"] = Model.EmpId;
                        dr["FirstName"] = Model.FirstName;
                        dr["LastName"] = Model.LastName;
                        dr["EmailId"] = Model.EmailId;
                        dr["PhoneNo"] = Model.PhoneNo;
                        dr["isActive"] = Model.isActive;
                        dr["gender"] = Model.gender;
                        return DALObj.SaveEmployeeDetails(dr, UserId, out EmpId);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errorcode;
        }
        public int CreateSysUser( int EmpId, string User_Id, string Email, string PasswordHash, string SecurityStamp, string UserName)
        {
            int Errorcode = 0;
            try
            {
                Errorcode = DALObj.CreateSysUser(EmpId, User_Id, Email, PasswordHash, SecurityStamp,UserName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Errorcode;
        }
        public int CheckEmaillAlreadyExits(int EmpId,  string Email)
        {
            int Errorcode = 0;
            try
            {
                Errorcode = DALObj.CheckEmaillAlreadyExits(EmpId,  Email);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Errorcode;
        }
        public int CheckMobilAlreadyExits(int EmpId, string Mobile)
        {
            int Errorcode = 0;
            try
            {
                Errorcode = DALObj.CheckMobilAlreadyExits(EmpId, Mobile);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Errorcode;
        }

        public int VerifyOTP(int EmpId,  string Email,string OTP)
        {
            int Errorcode = 0;
            try
            {
                Errorcode = DALObj.VerifyOTP(EmpId, Email,OTP);
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
                Errorcode = DALObj.SendOTP(EmailOrPhone);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Errorcode;
        }
        #endregion Send LogIn OTP 

        #region Task Master
        public List<TaskModel> GetTaskList(string UserId)
        {
            List<TaskModel> TaskLst = new List<TaskModel>();
            DataSet ds = DALObj.GetTaskList(UserId);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        TaskModel Model = new TaskModel();
                        Model.TaskId = Convert.ToInt32(dr["TaskId"].ToString());
                        Model.TaskName = dr["TaskName"].ToString();
                        Model.ActiveStr = dr["ActiveStr"].ToString();
                        Model.Status = dr["Status"].ToString();
                        Model.ModifiedBy = dr["ModifiedBy"].ToString();
                        Model.ModifiedDate = dr["ModifiedDate"].ToString();
                        Model.TaskDate = Convert.ToDateTime(dr["TaskDate"].ToString());
                        TaskLst.Add(Model);
                    }
                }
            }
            return TaskLst;
        }
        public TaskModel GetSelectedTaskDetails(int TaskId)
        {
            TaskModel Model = new TaskModel();
            DataSet ds = DALObj.GetSelectedTaskDetails(TaskId);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Model.TaskId = Convert.ToInt32(ds.Tables[0].Rows[0]["TaskId"].ToString());
                        Model.TaskName = ds.Tables[0].Rows[0]["TaskName"].ToString();
                        Model.TaskDescription = ds.Tables[0].Rows[0]["TaskDescription"].ToString();
                        Model.ccMail = ds.Tables[0].Rows[0]["ccMail"].ToString();
                        Model.Status = ds.Tables[0].Rows[0]["Status"].ToString();
                        Model.isActive = Convert.ToBoolean(ds.Tables[0].Rows[0]["isActive"].ToString());
                        Model.TaskDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["TaskDate"].ToString());
                        Model.CreatedBy = ds.Tables[0].Rows[0]["CreatedBy"].ToString();
                        Model.CreatedDate = ds.Tables[0].Rows[0]["CreatedDate"].ToString();
                        Model.ModifiedBy = ds.Tables[0].Rows[0]["ModifiedBy"].ToString();
                        Model.ModifiedDate = ds.Tables[0].Rows[0]["ModifiedDate"].ToString();
                        Model.StatusId = Convert.ToInt32(ds.Tables[0].Rows[0]["StatusId"].ToString());
                        Model.ApprovedBy = ds.Tables[0].Rows[0]["ApprovedBy"].ToString();
                        Model.ApprovedDate = ds.Tables[0].Rows[0]["ApprovedDate"].ToString();
                        Model.Remark = ds.Tables[0].Rows[0]["ApproverRemark"].ToString();

                    }

                }
            }
            return Model;
        }
        public int SaveTaskDetails(TaskModel Model, out int TaskId, string UserId)
        {
            DataSet ds = new DataSet();
            TaskId = 0;
            int errorcode = 0;
            try
            {
                ds = DALObj.GetSelectedTaskDetails(0);
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].Rows.Clear();
                        DataRow dr = ds.Tables[0].NewRow();
                        dr["TaskId"] = Model.TaskId;
                        dr["TaskName"] = Model.TaskName;
                        dr["TaskDescription"] = Model.TaskDescription;
                        dr["TaskDate"] = Model.TaskDate;
                        dr["StatusId"] = Model.StatusId;
                        dr["isActive"] = Model.isActive;
                        dr["ccMail"] = Model.ccMail;
                        return DALObj.SaveTaskDetails(dr, UserId, out TaskId);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return errorcode;
        }

        public List<TaskModel> GetPendingApprovalTaskList(string UserId)
        {
            List<TaskModel> TaskLst = new List<TaskModel>();
            DataSet ds = DALObj.GetPendingApprovalTaskList(UserId);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        TaskModel Model = new TaskModel();
                        Model.TaskId = Convert.ToInt32(dr["TaskId"].ToString());
                        Model.TaskName = dr["TaskName"].ToString();
                        Model.ActiveStr = dr["ActiveStr"].ToString();
                        Model.Status = dr["Status"].ToString();
                        Model.ModifiedBy = dr["ModifiedBy"].ToString();
                        Model.ModifiedDate = dr["ModifiedDate"].ToString();
                        Model.TaskDate = Convert.ToDateTime(dr["TaskDate"].ToString());
                        TaskLst.Add(Model);
                    }
                }
            }
            return TaskLst;
        }
        public int UpdateTaskStatusDetails( string UserId, int TaskId,int StatusId, string Remark)
        {
            int Errorcode = 0;
            try
            {
                Errorcode = DALObj.UpdateTaskStatusDetails( UserId, TaskId,StatusId,Remark);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Errorcode;
        }
        #endregion Task Master
    }
}
