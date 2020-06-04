using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DHTMLX.Scheduler;
using DHTMLX.Common;
using DHTMLX.Scheduler.Data;
using Personal_Management.Models;

namespace Personal_Management.Controllers
{
    public class BasicChedulerController : Controller
    {
        private PersonalContext db;
        public ActionResult Index()
        {
            var details = db.Events.ToList();
           
            return View(details);
        }

        public ContentResult Data()
        {
            try
            {
                var details = db.Events.ToList();
                return new SchedulerAjaxData(details);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ContentResult Save(int? id, FormCollection actionValues)

        {

            var action = new DataAction(actionValues);

            try
            {
                var changedEvent = (Events)DHXEventsHelper.Bind(typeof(Events), actionValues);

                switch (action.Type)
                {
                    case DataActionTypes.Insert:
                        Events EV = new Events();
                        EV.Id = changedEvent.Id;
                        EV.Start_date = changedEvent.Start_date;
                        EV.End_date = changedEvent.End_date;
                        EV.Text = changedEvent.Text;
                        db.Events.Add(EV);
                        db.SaveChanges();


                        break;
                    case DataActionTypes.Delete:
                        var details = db.Events.Where(x => x.Id == id).FirstOrDefault();
                        db.Events.Remove(details);
                        db.SaveChanges();

                        break;
                    default:// "update"    
                        var data = db.Events.Where(x => x.Id == id).FirstOrDefault();
                        data.Start_date = changedEvent.Start_date;
                        data.End_date = changedEvent.End_date;
                        data.Text = changedEvent.Text;
                        db.SaveChanges();

                        break;
                }
            }
            catch
            {
                action.Type = DataActionTypes.Error;
            }
            return (ContentResult)new AjaxSaveResponse(action);
        }

        
        // GET: api/scheduler
        
    }
}