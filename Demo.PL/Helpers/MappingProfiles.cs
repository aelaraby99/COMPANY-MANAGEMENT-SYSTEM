using AutoMapper;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Demo.PL.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<EmployeeViewModel, Employee>().ReverseMap();
            /* .ForMember(EMP=>EMP.EmpName,EVM => EVM.MapFrom( EMV=>EMV.Name)*/
            CreateMap<DepartmentViewModel, Department>().ReverseMap();
            CreateMap<ApplicationUser, UserViewModel>().ReverseMap();
            CreateMap<IdentityRole, RoleViewModel>()
                .ForMember(Mapped => Mapped.RoleName, o => o.MapFrom(Src => Src.Name))
                .ReverseMap();
        }
    }
}
