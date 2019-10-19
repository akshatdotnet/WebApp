using MVCTutorial.Domain;
using MVCTutorial.Models;
using System;

namespace MVCTutorial.Infrastructure
{
    public class AutomapperWebProfile : AutoMapper.Profile
    {
        public AutomapperWebProfile()
        {

            CreateMap<EmployeeDomainModel, EmployeeViewModel>()                
                .ForMember(dest=>dest.ExtraValue,opt=>opt.MapFrom(src=>src.ExtraValue.Encrypt()))
                .ForMember(dest => dest.CurrentDate, opt => opt.MapFrom(src => src.CurrentDate.ToString("MM/dd/yyy hh:mm")));
            
           
            //Reverese mapping

            CreateMap<EmployeeViewModel, EmployeeDomainModel>();     
           
        }

        public static void Run()
        {
            AutoMapper.Mapper.Initialize(a =>
            {
                a.AddProfile<AutomapperWebProfile>();


            });
        }
       

    }

    public static class ExtensionMethod
    {

        public static string Encrypt(this Int32 num)
        {

            return "Technotips:" + num;
        }
    }
}