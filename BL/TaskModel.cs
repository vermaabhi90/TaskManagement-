using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
   

    #region Employee 
    public class EmployeeModel
    {
        public int EmpId { get; set; }
        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [Required]
        [DisplayName("Email")]
        public string EmailId { get; set; }
        [Required]
        [DisplayName("Phone Number")]
        public string PhoneNo { get; set; }
        
        [DisplayName("Active")]
        public bool isActive { get; set; }
        [Required]
        [DisplayName("Gender")]
        public string gender { get; set; }
        public string User_Id { get; set; }
        public string UserName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

    }
    #endregion Employee

    #region Task Master
    public class TaskModel
    {
        public int TaskId { get; set; }
        [Required]
        [DisplayName("Task Name")]
        public string TaskName { get; set; }

        [Required]
        [DisplayName("Active")]
        public string ActiveStr { get; set; }

        [Required]
        [DisplayName("Task Date")]
        public DateTime TaskDate { get; set; }

        [Required]
        [DisplayName("Status")]
        public string Status { get; set; }

        [Required]
        [DisplayName("Modified By")]
        public string ModifiedBy { get; set; }

        [Required]
        [DisplayName("Modified Date")]
        public string ModifiedDate { get; set; }

        [Required]
        [DisplayName("Description")]
        [DataType(DataType.MultilineText)]
        public string TaskDescription { get; set; }

       
        [DisplayName("Created By")]
        public string CreatedBy { get; set; }

      
        [DisplayName("Created Date")]
        public string CreatedDate { get; set; }

        public int StatusId { get; set; }

        [Required]
        [DisplayName("cc Mail")]
        public string ccMail { get; set; }

        [DisplayName("Active")]
        public bool isActive { get; set; }


        [DisplayName("Approve By")]
        public string ApprovedBy { get; set; }


        [DisplayName("Approve Date")]
        public string ApprovedDate { get; set; }

        [Required]
        [DisplayName("Approval Remark")]
        public string Remark { get; set; }
    }

    #endregion Task Master
}
