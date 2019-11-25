using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string ip = GetIP();
            string Lanip = GetLanIP2();//GetLanIP1();
            string publicip = GetIPAddress();//GetLanIP1();
            Console.WriteLine("IP Address is : " + ip);//Lan
            Console.WriteLine("Lan IP Address is : " + Lanip);
            Console.WriteLine("Pub IP Address is : " + publicip);//Wan
            Console.ReadLine();
        }



        static string GetIP()
        {
            string host = Dns.GetHostName();
            IPHostEntry ipentry = Dns.GetHostEntry(host);
            foreach (IPAddress ipadd in ipentry.AddressList)
            {
                if (ipadd.AddressFamily.ToString() == "InterNetwork")
                {
                    return ipadd.ToString();
                }
            }

            return "";

        }


        static string GetLanIP1()
        {
            string VisitorsIPAddr = string.Empty;
            bool GetLan = false;
            try
            {
                if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                {
                    VisitorsIPAddr = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                }
                else if (System.Web.HttpContext.Current.Request.UserHostAddress.Length != 0)
                {
                    VisitorsIPAddr = System.Web.HttpContext.Current.Request.UserHostAddress;
                }
                if (string.IsNullOrEmpty(VisitorsIPAddr) || VisitorsIPAddr.Trim() == "::1")
                {
                    GetLan = true;
                    VisitorsIPAddr = string.Empty;
                }
                if (GetLan && string.IsNullOrEmpty(VisitorsIPAddr))
                {
                    //This is for Local(LAN) Connected IP Address
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

        static string GetLanIP2()
        {
            string MACAddress = "";
            string IP4Address = "";
            string strHostName = System.Net.Dns.GetHostName();
            IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);
            IPAddress[] addr = ipEntry.AddressList;
            IP4Address = addr[0].ToString();
            System.Net.NetworkInformation.NetworkInterface[] nics = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
            return MACAddress = nics[0].GetPhysicalAddress().ToString();
        }

        static string GetIPAddress()
        {
            String address = "";
            WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
            using (WebResponse response = request.GetResponse())
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                address = stream.ReadToEnd();
            }

            int first = address.IndexOf("Address: ") + 9;
            int last = address.LastIndexOf("</body>");
            address = address.Substring(first, last - first);

            return address;
        }

    }

}

