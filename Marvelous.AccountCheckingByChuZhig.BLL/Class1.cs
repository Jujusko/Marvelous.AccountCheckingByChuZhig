using System.Net;

namespace Marvelous.AccountCheckingByChuZhig.BLL
{
    public class AccountCheck
    {
        private readonly string _url = "https://localhost:5001/api/leads";

        public void GetAllLeads()
        {
            WebRequest myWebRequest = WebRequest.Create($"{_url}");

            try
            {
                WebResponse myWebResponse = myWebRequest.GetResponse();
                string text;
                using (var sr = new StreamReader(myWebResponse.GetResponseStream(), encoding: Encoding.UTF8))
                {
                    text = sr.ReadToEnd();
                }
                Text result = JsonSerializer.Deserialize<Text>(text);
                if (result.text is null)
                    throw new ArgumentNullException($"Text by id {id} does not exist");
                return result.text;
            }
            catch (WebException e)
            {
                return ("Random teapot, try again");
            }
        }
    }
}