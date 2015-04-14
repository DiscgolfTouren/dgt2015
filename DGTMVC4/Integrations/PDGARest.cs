using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using System;


namespace PDGASaker
{
    public static class PDGARESTApi
    {
        public static PDGAPlayer GetMemberInfo(string pdgaNumber, string username, string password)
        {
            var client = new RestClient();

            // Login
            client.BaseUrl = new Uri("https://api.pdga.com/services/json/user/login");
            var request = new RestRequest(Method.POST);
            request.AddParameter("username", username);
            request.AddParameter("password", password);
            var response = client.Execute<PDGASession>(request);
            client.CookieContainer = new CookieContainer();
            var cookie = new Cookie(response.Data.session_name, response.Data.sessid);
            client.CookieContainer.Add(new System.Uri("https://api.pdga.com"), cookie);

            // GetMember Info
            client.BaseUrl = new Uri(string.Concat("https://api.pdga.com/services/json/member/", pdgaNumber));
            var requestInfo = new RestRequest(Method.GET);
            var responseInfo = client.Execute<PDGAPlayer>(requestInfo);

            // Logout
            client.BaseUrl = new Uri("https://api.pdga.com/services/json/user/logout");
            var requestLogout = new RestRequest(Method.POST);
            var responseLogout = client.Execute(requestLogout);

            return responseInfo.Data;
        }
    }

    public class PDGASession
    {
        public string session_name { get; set; }
        public string sessid { get; set; }
    }

    public class PDGAPlayer
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string full_name { get; set; }
        public string gender { get; set; }
        public string birth_year { get; set; }
        public string pdga_number { get; set; }
        public string classification { get; set; }
        public string membership_status { get; set; }
        public string membership_expiration_date { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string rating { get; set; }
        public DateTime rating_effective_date { get; set; }
        public string official_state { get; set; }
        public DateTime official_expiration_date { get; set; }

    }
}