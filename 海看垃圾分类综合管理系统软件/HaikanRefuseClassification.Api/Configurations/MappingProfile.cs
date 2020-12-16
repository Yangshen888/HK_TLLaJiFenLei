using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaikanRefuseClassification.Api.ViewModels.AttendanceManagement.WorkTime;

using HaikanRefuseClassification.Api.ViewModels.Rbac.Department;
using HaikanRefuseClassification.Api.ViewModels.Rbac.SystemMenu;
using HaikanRefuseClassification.Api.ViewModels.Rbac.SystemPermission;
using HaikanRefuseClassification.Api.ViewModels.Rbac.SystemRole;
using HaikanRefuseClassification.Api.ViewModels.Rbac.SystemUser;
using HaikanRefuseClassification.Api.Entities;
using HaikanRefuseClassification.Api.ViewModels.Person;
using HaikanRefuseClassification.Api.ViewModels.Base;
using HaikanRefuseClassification.Api.ViewModels.Score;
using HaikanRefuseClassification.Api.ViewModels.SupervisorInspection;

namespace HaikanRefuseClassification.Api.Configurations
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            #region SystemUser
            CreateMap<SystemUser, UserJsonModel>();
            CreateMap<UserCreateViewModel, SystemUser>();
            CreateMap<UserEditViewModel, SystemUser>();
            CreateMap<SystemUser, UserEditViewModel>();
            #endregion
            #region SystemRole
            CreateMap<SystemRole, RoleJsonModel>();
            CreateMap<RoleCreateViewModel, SystemRole>();
            CreateMap<SystemRole, RoleCreateViewModel>();
            #endregion

            #region SystemMenu
            CreateMap<SystemMenu, MenuJsonModel>();
            CreateMap<MenuCreateViewModel, SystemMenu>();
            CreateMap<MenuEditViewModel, SystemMenu>();
            CreateMap<SystemMenu, MenuEditViewModel>();
            #endregion

            #region SystemPermission
            CreateMap<SystemPermission, PermissionJsonModel>()
                .ForMember(d => d.MenuName, s => s.MapFrom(x => x.SystemMenuUu.Name))
                .ForMember(d => d.PermissionTypeText, s => s.MapFrom(x => x.Type.ToString()));
            CreateMap<PermissionCreateViewModel, SystemPermission>();
            CreateMap<PermissionEditViewModel, SystemPermission>();
            CreateMap<SystemPermission, PermissionEditViewModel>();
            #endregion

            #region Department
            CreateMap<Department, DepartmentJsonViewModel>();
            CreateMap<Department, DepartmentCreateViewModel>();
            #endregion

            #region WorkTime
            CreateMap<WorkTime, WorkTimeJsonViewModel>();
            CreateMap<WorkTime, WorkTimeCreateViewModel>();
            #endregion

            #region person
            CreateMap<SystemUser, VolunteerCreateViewModel>();
            CreateMap<SystemUser, VolunteerEditView>();
            CreateMap<SystemUser, SupervisorCreateViewModel>();
            CreateMap<SystemUser, SupervisorEditView>();
            CreateMap<SystemUser, SupervisorShowViewModel>();
            CreateMap<SystemUser, ResponseCreateViewModel>();
            CreateMap<SystemUser, ResponseEditView>();
            CreateMap<ScoreSetting, ScoreSettingEditView>();
            #endregion
            #region base
            CreateMap<Village, CollegeCreateViewModel>();
            CreateMap<Village, CollegeShowViewModel>();
            CreateMap<Village, CollegeEditView>();
            CreateMap<Village, VillageParms>();

            CreateMap<SystemUser, HouseholdCreateViewModel>();
            CreateMap<SystemUser, HouseholdEditView>();

            CreateMap<GrabageRoom, RubbishCreateViewModel>();
            CreateMap<GrabageRoom, RubbishShowViewModel>();
            CreateMap<GrabageRoom, RubbishEditView>();

            CreateMap<Car, VehicleCreateViewModel>();
            CreateMap<Car, VehicleShowViewModel>();
            CreateMap<Car, VehicleEditView>();
            #endregion
            CreateMap<SupervisorInspection, SupervisorInspectionViewModel>();
            CreateMap<SupervisorInspectionViewModel, SupervisorInspection>();
        }
    }
}
