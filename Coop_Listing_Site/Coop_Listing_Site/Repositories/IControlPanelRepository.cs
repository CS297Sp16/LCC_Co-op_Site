using System.Collections.Generic;
using Coop_Listing_Site.Models;
using Coop_Listing_Site.Models.ViewModels;

namespace Coop_Listing_Site.Repositories
{
    public interface IControlPanelRepository
    {
        Department AddDepartment(Department dept);
        Department DeleteDepartmentById(int? id);
        void DeleteDepartmentConfirmed(int id);
        EmailInfo EmailSentConfirmation();
        IEnumerable<Department> GetAllDepartments();
        IEnumerable<User> GetAllUsers();
        User GetCurrentUser(User currentUser);
        Department GetDepartmentById(int? id);
        EmailInfo GetEmailInfo();
        User GetUserById(int? id);
        void PostEmailInfo(string SMTPAddress, string SMTPUser, string SMTPPassword, string InviteEmail, string Domain);
        void UpdateStudentInfo(User currentUser, StudentUpdateModel studentUpdateModel);
    }
}