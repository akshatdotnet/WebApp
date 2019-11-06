using MVCTutorial.Business.Interface;
using MVCTutorial.Domain;
using MVCTutorial.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MVCTutorial.Controllers
{

    public class TestController : Controller
    {
        IEmployeeBusiness empBusiness;
        IDepartmentBusiness departmentBusiness;
        
        public TestController(IEmployeeBusiness _empBusiness, IDepartmentBusiness _departmentBusiness)
        {
            empBusiness = _empBusiness;
            departmentBusiness = _departmentBusiness;
        }

        public ActionResult Index()
        {
            //Used to bind department dropdown
            List<DepartmentDomainModel> list = departmentBusiness.GetAllDepartment();
            ViewBag.DepartmentList = new SelectList(list, "DepartmentId", "DepartmentName");
            
            return View();
        }


        public ActionResult AddContactAddOrEdit()
        {
            ViewBag.Message = "Hello AddContact.";  
            return View();
        }

        public ActionResult Admin()
        {
            ViewBag.Message = "Hello Admin.";           

            return View();

        }

        public ActionResult SuperAdmin()
        {
            ViewBag.Message = "Hello SuperAdmin.";

            return View();

        }

        public JsonResult ImageUpload(ProductViewModel model)
        {
            MVCTutorialEntities db = new MVCTutorialEntities();
            int imageId = 0;

            var file = model.ImageFile;

            byte[] imagebyte = null;

            if (file != null)
            {

                file.SaveAs(Server.MapPath("/UploadedImage/" + file.FileName));

                BinaryReader reader = new BinaryReader(file.InputStream);

                imagebyte = reader.ReadBytes(file.ContentLength);

                ImageStore img = new ImageStore();

                img.ImageName = file.FileName;
                img.ImageByte = imagebyte;
                img.ImagePath = "/UploadedImage/" + file.FileName;
                img.IsDeleted = false;
                db.ImageStores.Add(img);
                db.SaveChanges();

                imageId = img.ImageId;

            }

            return Json(imageId, JsonRequestBehavior.AllowGet);
            //var file = model.ImageFile;

            //if (file != null)
            //{

            //    var fileName = Path.GetFileName(file.FileName);
            //    var extention = Path.GetExtension(file.FileName);
            //    var filenamewithoutextension = Path.GetFileNameWithoutExtension(file.FileName);

            //    file.SaveAs(Server.MapPath("/UploadedImage/" + file.FileName));


            //}
            //return Json(file.FileName, JsonRequestBehavior.AllowGet);

        }


        public ActionResult ImageRetrieve(int imgID)
        {
            MVCTutorialEntities db = new MVCTutorialEntities();

            var img = db.ImageStores.SingleOrDefault(x => x.ImageId == imgID);

            return File(img.ImageByte, "image/jpg");


        }

        public ActionResult SideMenu1()
        {
            List<MenuItem> list = new List<MenuItem>();

            list.Add(new MenuItem { Link = "/Repo/Index", LinkName = "Home" });         
            list.Add(new MenuItem { Link = "/Test/AddContactAddOrEdit", LinkName = "AddContact" });
            list.Add(new MenuItem { Link = "/Test/AddCompanyAddOrEdit", LinkName = "AddCompany" });
            list.Add(new MenuItem { Link = "/Test/AddCallAddOrEdit", LinkName = "AddCall" });
            list.Add(new MenuItem { Link = "/Test/Login", LinkName = "Login" });
            list.Add(new MenuItem { Link = "/Test/Registration", LinkName = "Register" });


            return PartialView("SideMenu1", list);
        }


        /// <summary>
        /// Get employee records, it includes search functionality
        /// </summary>
        /// <param name="param"></param>
        /// <param name="EName"></param>
        /// <returns></returns>
        public JsonResult GetEmployeeRecord(DataTablesParam param, string EName)
        {

            List<EmployeeDomainModel> List = new List<EmployeeDomainModel>();

            int pageNo = 1;

            if (param.iDisplayStart >= param.iDisplayLength)
            {
                pageNo = (param.iDisplayStart / param.iDisplayLength) + 1;

            }

            int totalCount = 0; //total records in table 

            //Search by department,employeeName,and Country
            if (param.sSearch != null)
            {
                totalCount = empBusiness.TotalEmployeeCount(param.sSearch);

                List = empBusiness.SearchEmployee(param.sSearch, pageNo, param.iDisplayLength);
            }

            //Datatable Exrernal Search (by employeeName)
            else if (EName != "")
            {
                totalCount = empBusiness.TotalEmployeeCountByEmployeeName(EName);

                List = empBusiness.SearchEmployeeByEmployeeName(EName, pageNo, param.iDisplayLength);

            }
            //Get All Employee without any search params
            else
            {
                totalCount = empBusiness.TotalEmployeeRecord();

                List = empBusiness.GetEmployeeRecords(pageNo, param.iDisplayLength);
            }

            List<EmployeeViewModel> empVMList = new List<EmployeeViewModel>();

            AutoMapper.Mapper.Map(List, empVMList);

            return Json(new
            {
                aaData = empVMList,
                sEcho = param.sEcho,
                iTotalDisplayRecords = totalCount,
                iTotalRecords = totalCount

            }
                , JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// Add Update employee based on employeeId
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(EmployeeViewModel model)
        {
            try
            {
                List<DepartmentDomainModel> list = departmentBusiness.GetAllDepartment();
                ViewBag.DepartmentList = new SelectList(list, "DepartmentId", "DepartmentName");

                EmployeeDomainModel empDomain = new EmployeeDomainModel();
                AutoMapper.Mapper.Map(model, empDomain);

                empDomain = empBusiness.AddEditEmployee(empDomain);

                AutoMapper.Mapper.Map(empDomain, model);

                return View(model);

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// Called when we click on edit button, it renders employee record by Id using Partial view
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public ActionResult AddEditEmployee(int EmployeeId)
        {
            List<DepartmentDomainModel> list = departmentBusiness.GetAllDepartment();
            ViewBag.DepartmentList = new SelectList(list, "DepartmentId", "DepartmentName");

            EmployeeViewModel model = new EmployeeViewModel();
            EmployeeDomainModel empDomainModel = new EmployeeDomainModel();

            if (EmployeeId > 0)
            {
                empDomainModel = empBusiness.GetEmployeeById(EmployeeId);
                AutoMapper.Mapper.Map(empDomainModel, model);
            }
            return PartialView("Partial2", model);
        }

        public ActionResult AddEditContact(int EmployeeId)
        {
            List<DepartmentDomainModel> list = departmentBusiness.GetAllDepartment();
            ViewBag.DepartmentList = new SelectList(list, "DepartmentId", "DepartmentName");

            EmployeeViewModel model = new EmployeeViewModel();
            EmployeeDomainModel empDomainModel = new EmployeeDomainModel();

            if (EmployeeId > 0)
            {
                empDomainModel = empBusiness.GetEmployeeById(EmployeeId);
                AutoMapper.Mapper.Map(empDomainModel, model);
            }
            return PartialView("Partial2", model);
        }

        /// <summary>
        /// Delete employee by Id (set IsDelete==false)
        /// </summary>
        /// <param name="EmployeeId"></param>
        /// <returns></returns>
        public ActionResult DeleteEmployee(int EmployeeId)
        {
            bool result = empBusiness.DeleteEmployee(EmployeeId);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Returns a piece of html using Partial view. This can be view of the left side of page
        /// </summary>
        /// <returns></returns>
        public ActionResult SideMenu()
        {
            return PartialView("SideMenu");
        }


        #region 
        //Send OTP 

        public JsonResult SendOTP()
        {
            int otpValue = new Random().Next(100000, 999999);
            var status = "";
            try
            {
                string recipient = ConfigurationManager.AppSettings["RecipientNumber"].ToString();
                string APIKey = ConfigurationManager.AppSettings["APIKey"].ToString();

                string message = "Your OTP Number is " + otpValue + " ( Sent By : VaishanavTech )";
                String encodedMessage = HttpUtility.UrlEncode(message);

                using (var webClient = new WebClient())
                {
                    byte[] response = webClient.UploadValues("https://api.textlocal.in/send/", new NameValueCollection(){
                                        
                                         {"apikey" , APIKey},
                                         {"numbers" , recipient},
                                         {"message" , encodedMessage},
                                         {"sender" , "TXTLCL"}});

                    string result = System.Text.Encoding.UTF8.GetString(response);//"success";

                    var jsonObject = JObject.Parse(result);

                    status = jsonObject["status"].ToString();

                    Session["CurrentOTP"] = otpValue;
                }


                return Json(status, JsonRequestBehavior.AllowGet);


            }
            catch (Exception e)
            {

                throw (e);

            }

        }
        public ActionResult EnterOTP()
        {
            return View();
        }
        [HttpPost]
        public JsonResult VerifyOTP(string OtpTxt)//(string otp)
        {
            bool result = false;
            //Session["OTP"] = "7777";
            //string sessionOTP = Session["CurrentOTP"].ToString();
            string sessionOTP = Session["OTP"].ToString();

            if (OtpTxt == sessionOTP)
            {
                result = true;

            }
            else
            {
                result = false;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion




        #region 
        //Login

        public ActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public JsonResult LoginUser(RegistrationViewModel model)
        {
            string result = "";
            string BrowserSinatureId = model.SignatureId; //GetUser_IP(model.UserName);//192.168.43.26

            result = VerifySignature(BrowserSinatureId, model.UserName);
            //if (model.OtpTxt != null)
            //{
            //    result = "";
            //}
            if (result == "signaturefail")
            {
                Session["OTP"] = "7777";
                return Json(result, JsonRequestBehavior.AllowGet);
            }


            MVCTutorialEntities db = new MVCTutorialEntities();

            SiteUser user = db.SiteUsers.SingleOrDefault(x => x.EmailId == model.EmailId && x.Password == model.Password);
    
            //string result = "fail";
            if (user != null)
            {

                Session["UserId"] = user.UserId;
                Session["UserName"] = user.UserName;
                if (user.RoleId == 3)
                {
                    result = "GeneralUser";

                }
                else if (user.RoleId == 1)
                {
                    result = "Admin";

                }
                else if (user.RoleId == 2) 
                {
                    result = "SuperAdmin";
                }

            }


            return Json(result, JsonRequestBehavior.AllowGet);
       
        }

        //

        public string VerifySignature(string ip, string userid)
        {
           return "signaturefail";

        }


        //Login SMS Integration

        public static string GetUser_IP(string LoginId)
        {
            string VisitorsIPAddr = string.Empty;
            bool GetLan = false;
            try
            {
                if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                {
                    VisitorsIPAddr = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                    //MyFileIO.WriteToFile("GetUserIP.txt", "1" + VisitorsIPAddr + "|", false);
                }
                else if (System.Web.HttpContext.Current.Request.UserHostAddress.Length != 0)
                {
                    VisitorsIPAddr = System.Web.HttpContext.Current.Request.UserHostAddress;
                    //MyFileIO.WriteToFile("GetUserIP.txt", "2" + VisitorsIPAddr + "|", false);
                }
                if (string.IsNullOrEmpty(VisitorsIPAddr) || VisitorsIPAddr.Trim() == "::1")
                {
                    GetLan = true;
                    VisitorsIPAddr = string.Empty;
                }
                if (GetLan && string.IsNullOrEmpty(VisitorsIPAddr))
                {
                    //This is for Local(LAN) Connected ID Address
                    string stringHostName = Dns.GetHostName();
                    //Get Ip Host Entry
                    IPHostEntry ipHostEntries = Dns.GetHostEntry(stringHostName);
                    //Get Ip Address From The Ip Host Entry Address List
                    IPAddress[] arrIpAddress = ipHostEntries.AddressList;

                    try
                    {
                        VisitorsIPAddr = arrIpAddress[arrIpAddress.Length - 1].ToString();                        
                    }
                    catch
                    {
                        try
                        {
                            VisitorsIPAddr = arrIpAddress[0].ToString();                            
                        }
                        catch
                        {
                            try
                            {
                                arrIpAddress = Dns.GetHostAddresses(stringHostName);
                                VisitorsIPAddr = arrIpAddress[0].ToString();                                
                            }
                            catch
                            {
                                VisitorsIPAddr = "127.0.0.1";
                            }
                        }
                    }
                }
                
                return VisitorsIPAddr;
            }
            catch (Exception ex)
            {
               return VisitorsIPAddr;
            }
        }

        //===================

        public ActionResult Logout()
        {

            Session.Clear();
            Session.Abandon();

            return RedirectToAction("Login");

        }



        public ActionResult Registration()
        {

            return View();

        }

        [HttpPost]

        public JsonResult RegisterUser(RegistrationViewModel model)
        {

            MVCTutorialEntities db = new MVCTutorialEntities();


            SiteUser siteUser = new SiteUser();

            siteUser.UserName = model.UserName;

            siteUser.EmailId = model.EmailId;

            siteUser.Password = model.Password;

            siteUser.Address = model.Address;

            siteUser.RoleId = 2;

            db.SiteUsers.Add(siteUser);

            db.SaveChanges();


            return Json("Success", JsonRequestBehavior.AllowGet);

        }


        #endregion
    }
}
