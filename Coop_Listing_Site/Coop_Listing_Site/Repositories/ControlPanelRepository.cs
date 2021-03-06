﻿using Coop_Listing_Site.DAL;
using Coop_Listing_Site.Models;
using Coop_Listing_Site.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Coop_Listing_Site.Repositories
{
    public class ControlPanelRepository : IControlPanelRepository
    {
        CoopContext db;
        UserManager<User> userManager;

        public ControlPanelRepository()
        {
            db = db ?? new CoopContext();

            userManager = userManager ?? new UserManager<User>(
               new UserStore<User>(new CoopContext()));
        }

        public IEnumerable<User> GetAllUsers()
        {
            return db.Users.ToList();
        }

        public IEnumerable<Department> GetAllDepartments()
        {
            return db.Departments.ToList();
        }

        public User GetUserById(int? id)
        {
            return db.Users.Find(id);
        }

        public User GetCurrentUser(User currentUser)
        {
            var user = currentUser;
            return user;
        }

        public Department GetDepartmentById(int? id)
        {
            return db.Departments.Find(id);
        }

        public Department AddDepartment(Department dept)
        {
            var department = db.Departments.Add(dept);
            db.SaveChanges();

            return department;
        }

        public void DeleteDepartmentConfirmed(int id)
        {
            var dept = GetDepartmentById(id);
            db.Departments.Remove(dept);
            db.SaveChanges();
        }

        public Department DeleteDepartmentById(int? id)
        {
            var dept = GetDepartmentById(id);
            db.Departments.Remove(dept);
            db.SaveChanges();
            return dept;
        }

        public EmailInfo GetEmailInfo()
        {
            var email = db.Emails.FirstOrDefault();
            //TODO: finish this method
            return email;
        }

        public void PostEmailInfo(string SMTPAddress, string SMTPUser, string SMTPPassword, string InviteEmail, string Domain)
        {
            var email = db.Emails.FirstOrDefault();
            //TODO: finish this method

        }

        public EmailInfo EmailSentConfirmation()
        {
            var email = db.Emails.FirstOrDefault();
            //TODO: finish this method
            return email;
        }

        public void UpdateStudentInfo(User currentUser, StudentUpdateModel studentUpdateModel)
        {
            var user = db.Users.FirstOrDefault(u => u.Id == currentUser.Id);

            var studInfo = db.Students.FirstOrDefault(si => si.User == user);

            var major = db.Majors.FirstOrDefault(mj => mj.MajorID == studentUpdateModel.MajorID);

            studInfo.GPA = (double)studentUpdateModel.GPA;

            if (studInfo.Major.MajorID != studentUpdateModel.MajorID)
            {
                studInfo.Major = major;
            }

            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            db.Entry(studInfo).State = EntityState.Modified;
            db.SaveChanges();
        }





    }
}