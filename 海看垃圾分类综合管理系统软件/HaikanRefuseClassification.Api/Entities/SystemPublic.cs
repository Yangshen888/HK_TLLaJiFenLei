using HaikanRefuseClassification.Api.Extensions.AuthContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanRefuseClassification.Api.Entities
{
    public static class SystemPublic
    {
        public static bool CreateSystemLog(string LoginName, string OperationContent, string Type, string UserId, string UserType)
        {
            using (var _dbContext = new RefuseClassificationContext())
            {
                var entity = new SystemLog();
                entity.AddPeople = LoginName;
                entity.AddTime = DateTime.Now;
                entity.OperationTime = DateTime.Now;
                //entity.Ipaddress = model.Ipaddress;
                entity.IsDelete = 1;
                entity.OperationContent = OperationContent;
                entity.Type = Type;
                entity.UserId = UserId;
                entity.UserName = LoginName;
                if (UserType == "SuperAdministrator")
                {
                    entity.UserIdtype = 0;
                }
                else if (UserType == "Admin")
                { entity.UserIdtype = 1; }
                else if (UserType == "GeneralUser")
                { entity.UserIdtype = 2; }
                else if (UserType == "Parent")
                { entity.UserIdtype = 3; }
                else
                {
                    entity.UserIdtype = 4;
                }
                _dbContext.SystemLog.Add(entity);
                _dbContext.SaveChanges();

                return true;
            }
        }
        public static bool CreateSystemLog(string OperationContent, string Type)
        {
            using (var _dbContext = new RefuseClassificationContext())
            {
                var entity = new SystemLog();
                entity.AddPeople = AuthContextService.CurrentUser.DisplayName;
                entity.AddTime = DateTime.Now;
                entity.OperationTime = DateTime.Now;
                //entity.Ipaddress = model.Ipaddress;
                entity.IsDelete = 1;
                entity.OperationContent = OperationContent;
                entity.Type = Type;
                entity.UserId = AuthContextService.CurrentUser.Guid.ToString();
                entity.UserName = AuthContextService.CurrentUser.DisplayName;
                string UserType = AuthContextService.CurrentUser.UserType.ToString();
                if (UserType == "SuperAdministrator")
                {
                    entity.UserIdtype = 0;
                }
                else if (UserType == "Admin")
                { entity.UserIdtype = 1; }
                else if (UserType == "GeneralUser")
                { entity.UserIdtype = 2; }
                else if (UserType == "Parent")
                { entity.UserIdtype = 3; }
                else
                {
                    entity.UserIdtype = 4;
                }
                _dbContext.SystemLog.Add(entity);
                _dbContext.SaveChanges();

                return true;
            }
        }
    }
}
