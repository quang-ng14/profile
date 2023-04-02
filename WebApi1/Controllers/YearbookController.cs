using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi1.Controllers
{
    public class YearbookStatus
    {
        public bool IsSuccess { get; set; }

        private string link = "null";
        public string Link
        {   get { return link; }
            set { link = value; } 
        }
    }
    public class upYearbookStatus
    {
        public bool IsSuccess { get; set; }
    }
    public class FormStatus
    {
        public bool IsSuccess { get; set; }
    }

    [Route("user")]

    public class YearbookController : ControllerBase
    {
        private readonly MyContext _context;     // Lay database
        public YearbookController(MyContext context)
        {
            _context = context;
        }

        [HttpPost("yearbook")]
        //public IActionResult YearbookUser([FromBody] string name)
        public IActionResult YearbookUser([FromBody] YearbookLog name)
        {
            YearbookStatus yearbookStatus = new YearbookStatus();
            name.Name = name.Name.ToUpper();
            var check = _context.YearbookLogs.FirstOrDefault(i => i.Name == name.Name);
            if (check != null)
            {
                yearbookStatus.IsSuccess = true;          
                yearbookStatus.Link = check.Link;
                return Ok(yearbookStatus);
            }
            else
            {
                yearbookStatus.IsSuccess = false;
                return BadRequest(yearbookStatus);
            }
        }

        [HttpPost("upload_yearbook")]
        public IActionResult yearbookForm([FromBody] YearbookLog info)
        {
            upYearbookStatus yearbookStatus = new upYearbookStatus();
            yearbookStatus.IsSuccess = _context.YearbookLogs.Any(i => i.Name == info.Name);
            if (!yearbookStatus.IsSuccess)
            {
                yearbookStatus.IsSuccess = true;
                info.Name = info.Name.ToUpper();
                _context.YearbookLogs.Add(info);
                _context.SaveChanges();
            }
            else
                yearbookStatus.IsSuccess = false;
            return Ok(yearbookStatus);  
        }

        [HttpPost("yearbook_form")]
        public IActionResult yearbookForm([FromBody] Form info)
        {
            FormStatus formStatus = new FormStatus();
            formStatus.IsSuccess = true;
            _context.FormLogs.Add(info);
            _context.SaveChanges();
            return Ok(formStatus);
        }

        /*
        [HttpDelete("del_yearbook")]
        public async Task<IActionResult> Delete(string name)
        {
            if (string.IsNullOrEmpty(name)) return NotFound();

            YearbookLog yearbookStatus = new YearbookLog();
        }
        */

        [HttpGet("infors")]
        public async Task<IActionResult> Get()
        {
            List<YearbookLog> logs = new List<YearbookLog>();
            var log = _context.YearbookLogs.ToList();
            if (log == null) { return Ok(log); }
            //logs.Add(log);
            return Ok(log);
        }
        
    }
}
