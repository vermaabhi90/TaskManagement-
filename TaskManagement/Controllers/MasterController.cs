using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BL;
using static BL.TaskModel;
using TaskManagement.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using TaskUtility;

namespace TaskManagement.Controllers
{
    public class MasterController : Controller
    {
        // GET: Master


        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        TaskBL BLObj = new TaskBL();
        
        #region Employee Master
        [Authorize]
        public ActionResult GetEmployeeList()
        {            
            List<EmployeeModel> lstEmp = new List<EmployeeModel>();
            string UserId = User.Identity.GetUserId();
            try
            {
               
                lstEmp = BLObj.GetEmployeeList(UserId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(lstEmp);
        }
        public ActionResult CreateEmployee(int EmpId)
        {
            EmployeeModel Model = new EmployeeModel();
            try
            {
                Model = BLObj.GetSelectedEmployeeDetails("", EmpId);
                if (EmpId == 0)
                    Model.isActive = true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return View(Model);
        }
        [HttpPost]
        public ActionResult CreateEmployee(EmployeeModel obj)
        {
            EmployeeModel Model = new EmployeeModel();
            int  EmpId = 0;
            int Errorcode = 0;
            string UserId = User.Identity.GetUserId();
            try
            {
                Model.EmpId = obj.EmpId;
                Model.FirstName =obj.FirstName;
                Model.LastName = obj.LastName;
                Model.gender = obj.gender;
                Model.EmailId = obj.EmailId;
                Model.PhoneNo = obj.PhoneNo;
                Model.isActive = obj.isActive;
                Model.Role = obj.Role;
                Errorcode = BLObj.SaveEmployeeDetails(Model, out EmpId, UserId);
                Model.EmpId = EmpId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("CreateEmployee", new { EmpId = Model.EmpId });
        }      
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public ActionResult CreateNewUser(int EmpId, string Email,  string UserName)
        {    
            int Errorcode = 0;
            string Password = "Asd@123";
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser() { UserName = "DoNotSave", Email = "DoNotSave" };  // passing username and email as "DoNotSave" to make that transaction fail so user will not be save in database but will generate details to save user by procedure. (To avoid user id creation in random database.)
                try
                {
                    var i = UserManager.CreateAsync(user, Password);
                    List<KeyValuePair<string, string>> kvpList = new List<KeyValuePair<string, string>>()
                     {
                        new KeyValuePair<string, string>("User_Id", user.Id),
                        new KeyValuePair<string, string>("Email", user.Email),
                        new KeyValuePair<string, string>("PasswordHash", user.PasswordHash),
                        new KeyValuePair<string, string>("SecurityStamp", user.SecurityStamp),
                        new KeyValuePair<string, string>("UserName", user.UserName),
                    };
                    Errorcode = BLObj.CreateSysUser( EmpId, user.Id, Email, user.PasswordHash, user.SecurityStamp,UserName);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return Json(Errorcode, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckEmaillAlreadyExits(int EmpId, string Email)
        {
            int Errorcode = 0;
            try
            {
                Errorcode = BLObj.CheckEmaillAlreadyExits(EmpId,Email);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(Errorcode, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckMobileAlreadyExits(int EmpId, string Mobile)
        {
            int Errorcode = 0;
            try
            {
                Errorcode = BLObj.CheckMobilAlreadyExits(EmpId, Mobile);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(Errorcode, JsonRequestBehavior.AllowGet);
        }
        #endregion Employee Master

        #region Send LogIn OTP 
        public ActionResult SendOTP( string EmailOrPhone)
        {
            int Errorcode = 0;           
            try
            {                   
                Errorcode = BLObj.SendOTP(EmailOrPhone);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(Errorcode, JsonRequestBehavior.AllowGet);
        }
        public ActionResult VerifyOTP(string OTP, int EmpId, string Email)
        {
            TaskBL ObjBL = new TaskBL();
            int errorcode = ObjBL.VerifyOTP(EmpId, Email, OTP);
            return Json(errorcode, JsonRequestBehavior.AllowGet);
        }
        #endregion Send LogIn OTP 

        #region Task Master
        [Authorize]
        public ActionResult GetTaskList()
        {
            List<TaskModel> lstTask = new List<TaskModel>();
            string UserId = User.Identity.GetUserId();
            try
            {
                lstTask = BLObj.GetTaskList(UserId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(lstTask);
        }

        public ActionResult CreateTask(int TaskId)
        {
            TaskModel Model = new TaskModel();
            Model.CommentList = new List<AssigneeTaskLogModel>();
            try
            {
                Model = BLObj.GetSelectedTaskDetails(TaskId);
                if (Model.TaskId == 0)
                {
                    Model.isActive = true;
                    Model.TaskDate = DateTime.Now;
                }
                List<EmployeeDrpDwnModel> EmpList = new List<EmployeeDrpDwnModel>();
                EmpList = BLObj.GetEmpDrpDwnList(TaskId);
                ViewBag.EmpList = new SelectList(EmpList, "EmpId", "Employee");

                Model.CommentList = BLObj.GetAssigneeTaskLogList(TaskId);

            }   
            catch (Exception ex)
            {

                throw ex;
            }
            return View(Model);
        }
        [HttpPost]
        public ActionResult CreateTask(TaskModel obj)
        {
            TaskModel Model = new TaskModel();
            int TaskId = 0;
            int Errorcode = 0;
            string UserId = User.Identity.GetUserId();
            try
            {
                Model.TaskId = obj.TaskId;
                Model.TaskName = obj.TaskName;
                Model.TaskDescription = obj.TaskDescription;
                Model.TaskDate = obj.TaskDate;
                Model.ccMail = obj.ccMail;
                Model.StatusId = obj.StatusId;
                Model.isActive = obj.isActive;
                Model.AssignedToEmpId = obj.AssignedToEmpId;
                Model.FrequencyType = obj.FrequencyType;
                switch (Model.FrequencyType)
                {
                    case "Daily":
                        {                           
                            Model.WeekDays = "0000000";
                            break;
                        }

                    case "Weekly":
                        {
                            System.Text.StringBuilder strFrequency = new System.Text.StringBuilder("");
                            Model.WeekDays = strFrequency.ToString();
                            if (obj.Monday) { strFrequency.Append("1"); } else { strFrequency.Append("0"); }
                            if (obj.Tuesday) { strFrequency.Append("1"); } else { strFrequency.Append("0"); }
                            if (obj.Wednesday) { strFrequency.Append("1"); } else { strFrequency.Append("0"); }
                            if (obj.Thursday) { strFrequency.Append("1"); } else { strFrequency.Append("0"); }
                            if (obj.Friday) { strFrequency.Append("1"); } else { strFrequency.Append("0"); }
                            if (obj.Saturday) { strFrequency.Append("1"); } else { strFrequency.Append("0"); }
                            if (obj.Sunday) { strFrequency.Append("1"); } else { strFrequency.Append("0"); }
                            if (strFrequency.ToString() == null)
                            {
                                Model.WeekDays = "0000000";
                            }
                            else
                            {
                                Model.WeekDays = strFrequency.ToString();
                            }

                            break;
                        }
                    case "Monthly":
                        {
                           
                            Model.NthDay = obj.NthDay;
                            switch (Model.WeekDay)
                            {
                                case 1:
                                    {
                                        Model.WeekDays = "1000000";
                                        break;
                                    }
                                case 2:
                                    {
                                        Model.WeekDays = "0100000";
                                        break;
                                    }
                                case 3:
                                    {
                                        Model.WeekDays = "0010000";
                                        break;
                                    }
                                case 4:
                                    {
                                        Model.WeekDays = "0001000";
                                        break;
                                    }
                                case 5:
                                    {
                                        Model.WeekDays = "0000100";
                                        break;
                                    }
                                case 6:
                                    {
                                        Model.WeekDays = "0000010";
                                        break;
                                    }
                                case 7:
                                    {
                                        Model.WeekDays = "0000001";
                                        break;
                                    }
                                case 8:
                                    {
                                        Model.WeekDays = "0000000";
                                        break;
                                    }
                                default:
                                    {
                                        Model.WeekDays = "0000000";
                                        break;
                                    }
                            }
                            break;
                        }
                    case "Yearly":
                        {
                           
                            Model.NthDay = obj.NthDay;
                            switch (Model.WeekDay)
                            {
                                case 1:
                                    {
                                        Model.WeekDays = "1000000";
                                        break;
                                    }
                                case 2:
                                    {
                                        Model.WeekDays = "0100000";
                                        break;
                                    }
                                case 3:
                                    {
                                        Model.WeekDays = "0010000";
                                        break;
                                    }
                                case 4:
                                    {
                                        Model.WeekDays = "0001000";
                                        break;
                                    }
                                case 5:
                                    {
                                        Model.WeekDays = "0000100";
                                        break;
                                    }
                                case 6:
                                    {
                                        Model.WeekDays = "0000010";
                                        break;
                                    }
                                case 7:
                                    {
                                        Model.WeekDays = "0000001";
                                        break;
                                    }
                                case 8:
                                    {
                                        Model.WeekDays = "0000000";
                                        break;
                                    }
                                default:
                                    {
                                        Model.WeekDays = "0000000";
                                        break;
                                    }
                            }
                            Model.MonthNo =obj.MonthNo;
                            break;
                        }

                }
                Errorcode = BLObj.SaveTaskDetails(Model, out TaskId, UserId);
                Model.TaskId = TaskId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("CreateTask", new { TaskId = Model.TaskId });
        }
        [Authorize]
        public ActionResult GetPendingApprovalTaskList()
        {
            List<TaskModel> lstTask = new List<TaskModel>();
            string UserId = User.Identity.GetUserId();
            try
            {
                lstTask = BLObj.GetPendingApprovalTaskList(UserId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(lstTask);
        }

        public ActionResult CreatePendingAprovalTask(int TaskId)
        {
            TaskModel Model = new TaskModel();
            try
            {
                Model = BLObj.GetSelectedTaskDetails(TaskId);
                List<EmployeeDrpDwnModel> EmpList = new List<EmployeeDrpDwnModel>();
                EmpList = BLObj.GetEmpDrpDwnList(TaskId);
                ViewBag.EmpList = new SelectList(EmpList, "EmpId", "Employee");

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return View(Model);
        }
        [HttpPost]
        public ActionResult CreatePendingAprovalTask(TaskModel obj)
        {
            TaskModel Model = new TaskModel();
            int TaskId = 0;
            int Errorcode = 0;
            string UserId = User.Identity.GetUserId();
            try
            {
                Model.TaskId = obj.TaskId;
                Model.Remark = obj.Remark;
                Model.StatusId = obj.StatusId;
                Errorcode = BLObj.UpdateTaskStatusDetails(UserId, Model.TaskId, Model.StatusId, Model.Remark);
                Model.TaskId = TaskId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("GetPendingApprovalTaskList");
        }
        #endregion Task Master

        #region Assignee User Task Process

        [Authorize]
        public ActionResult GetAssigneeTaskList()
        {
            List<TaskModel> lstTask = new List<TaskModel>();
            string UserId = User.Identity.GetUserId();
            try
            {
                lstTask = BLObj.GetAssigneeTaskList(UserId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(lstTask);
        }

        public ActionResult CreateAssigneeTaskUpdated(int TaskId)
        {
            TaskModel Model = new TaskModel();
            Model.CommentList = new List<AssigneeTaskLogModel>();
            try
            {
                Model = BLObj.GetSelectedTaskDetails(TaskId);
                if (Model.TaskId == 0)
                {
                    Model.isActive = true;
                    Model.TaskDate = DateTime.Now;
                }
                List<EmployeeDrpDwnModel> EmpList = new List<EmployeeDrpDwnModel>();
                EmpList = BLObj.GetEmpDrpDwnList(TaskId);
                ViewBag.EmpList = new SelectList(EmpList, "EmpId", "Employee");

                Model.CommentList = BLObj.GetAssigneeTaskLogList(TaskId);

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return View(Model);
        }

        public JsonResult SaveAssigneeComments()
        {
            AssigneeTaskLogModel Model = new AssigneeTaskLogModel();
            string UserId = User.Identity.GetUserId();
            int ErrCd = 5000;
            try
            {
                var filename = "";
                string[] a = Request.Form.GetValues(0);
                string[] b = Request.Form.GetValues(1);
                string[] c = Request.Form.GetValues(2);
                string[] d = Request.Form.GetValues(3);
                Model.TaskId = Convert.ToInt32(a[0]);
                Model.FromStatusId = Convert.ToInt32(b[0]);
                Model.ToStatusId = Convert.ToInt32(c[0]);
                Model.Remark = d[0];
                if (Request.Files.Count > 0)
                {

                    foreach (string files in Request.Files)
                    {
                        string ftpServer = Common.GetConfigValue("FTP");
                        var file = Request.Files[files];
                        filename = System.IO.Path.GetFileName(file.FileName);
                        string[] str = filename.Split('.');
                        string FName = "";
                        if (str[0].Length > 30)
                        {
                            FName = str[0].ToString().Substring(0, 30);
                        }
                        else
                        {
                            FName = str[0].ToString();
                        }
                        string FileEx = str[1].ToString();
                        String guid = Guid.NewGuid().ToString();
                        string date = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString();
                        string time = DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                        string ClientCodeFile = "/TaskManagementDocs/";                      

                        filename = Convert.ToString(ClientCodeFile + FName + "_" + date + "_" + time + "." + FileEx);
                        string localPath = System.IO.Path.Combine(Server.MapPath("~//App_Data/uploads"), filename);
                        file.SaveAs(localPath);
                      
                        //MasterBLObj.UploadScannedFnfDocFileName(ResignationId, ExitInterviewFileName, User_Id, Type, ConnString);
                        int ErrCd1 = Common.UploadFileInFTP(filename, localPath, "TaskManagementDocs");
                        file.InputStream.Dispose();
                    }
                }
                  ErrCd = BLObj.UpdateAssigneeTaskStatusDetails(UserId, Model.TaskId, Model.FromStatusId, Model.ToStatusId, Model.Remark, filename);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(ErrCd, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DocDownload(int TaskId, string DocumentPath,string CalledFrom)
        {
            try
            {
                byte[] OPData = Common.DownloadFileFromFTP(DocumentPath);
                Response.AddHeader("content-disposition", "attachment; filename=" + DocumentPath);
                Response.ContentType = "application/zip";
                Response.BinaryWrite(OPData);
                Response.End();
                if(CalledFrom == "Assignee")
                {
                    return RedirectToAction("CreateAssigneeTaskUpdated", new { TaskId = TaskId });
                }
                else
                {
                    return RedirectToAction("CreateTask", new { TaskId = TaskId });
                }
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //return RedirectToAction("CreateAssigneeTaskUpdated", new { TaskId = TaskId });
        }

        #endregion Assignee User Task Process
    }
}