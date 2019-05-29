using SAM.Taskboard.Model.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SAM.Taskboard.Web.Controllers
{
    public class TaskController : Controller
    {
        [HttpPost]
        public ActionResult CreateTask(CreateTaskViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(model);
            }

            return Json(new { success = true });
        }
    }
}