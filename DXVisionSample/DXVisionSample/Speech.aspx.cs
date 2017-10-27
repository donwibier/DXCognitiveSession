using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DXVisionSample
{
	public partial class Speech : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}
        const string appId = "4af50150-a836-47ba-ace7-25eff41713cc";
        const string key = "59990945ef334679b426b6a365957443";
        protected string Analyze(string text)
        {
            var client = new HttpClient();
            string cmd = HttpUtility.UrlEncode(text);
            // Execute the REST API call.
            var url = $"https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/{appId}?subscription-key={key}&staging=true&verbose=true&timezoneOffset=0&q={cmd}";
            var response = client.GetStringAsync(url);
            Task.WaitAll(response);
            // Get the JSON response.
            return response.Result;
        }

        protected void cbxLuis_Callback(object source, DevExpress.Web.CallbackEventArgs e)
        {
            e.Result = Analyze(e.Parameter);
        }
    }
}