using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Online_Shop_Salon.Models;

namespace Online_Shop_Salon
{
    public class Role :RoleProvider
    {
        public override string ApplicationName { get; set; }
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            using (Online_shopEntities db = new Online_shopEntities())
            {
                var user = db.tbl_Account.Include("tbl_Role").Where(a => a.Email == username).FirstOrDefault().tbl_Role.Role_Name;
                string[] role = { user };
                return role;
            }
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}
