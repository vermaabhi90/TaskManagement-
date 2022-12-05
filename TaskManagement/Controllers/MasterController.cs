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
            try
            {
                Model = BLObj.GetSelectedTaskDetails(TaskId);
                if (Model.TaskId == 0)
                {
                    Model.isActive = true;
                    Model.TaskDate = DateTime.Now;
                }
                   
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
    }
}