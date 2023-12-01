using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using webbapp.Controllers.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace webbapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParseController : ControllerBase
    {
        // GET api/<ParseController>/5
        [HttpGet("{number}")]
        public Answer<StringResponse> Get(string number)
        {
            bool isOk = true;
            string res = string.Empty;
            try
            {
                res = Parser.Parse(number);
            }
            catch (ArgumentOutOfRangeException) 
            {
                isOk = false;
                res = "Number is out of specified range $0,00  - $999 999 999,99.";
            }
            catch (ArgumentException e)
            {
                isOk = false;
                res = string.Format(CultureInfo.CurrentCulture, "Input value is not a valid number: {0}.", e.Message);
            }

            Answer<StringResponse> ans = new Answer<StringResponse>(
                isOk,
                new StringResponse() { Text = res });
            return ans;
        }
    }
}
