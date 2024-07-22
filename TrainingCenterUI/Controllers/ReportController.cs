using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using TrainingCenterUI.Entities;

namespace TrainingCenterUI.Controllers
{
    public class ReportController : Controller
    {
        private readonly TrainingCenterUIDbContext _Db;

        public ReportController() 
        {
            _Db = new TrainingCenterUIDbContext();
        }

        // GET: Report
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> UsersReport()
        {
            // Fetch the data for the report
            var reportData = await _Db.AuditTrailView2.ToListAsync();

            var dataTable = new DataTable();
            dataTable.Columns.Add("AuditTrailID", typeof(int));
            dataTable.Columns.Add("ActionTypeName", typeof(string));
            dataTable.Columns.Add("ActionName", typeof(string));
            dataTable.Columns.Add("EntityName", typeof(string));
            dataTable.Columns.Add("UserID", typeof(int));
            dataTable.Columns.Add("IpAddress", typeof(string));
            dataTable.Columns.Add("TransactionTime", typeof(DateTime));
            dataTable.Columns.Add("EntityRecord", typeof(string));

            foreach (var item in reportData)
            {
                dataTable.Rows.Add(item.AuditTrailID, item.ActionTypeName, item.ActionName, item.EntityName, item.UserID, item.IpAddress, item.TransactionTime, item.EntityRecord);
            }

            // Set up the ReportViewer
            var reportViewer = new ReportViewer
            {
                ProcessingMode = ProcessingMode.Local
            };

            reportViewer.LocalReport.ReportPath = Server.MapPath("~/Reports/UsersHistory.rdlc");
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ViewsDataSet", dataTable));

            var reportBytes = reportViewer.LocalReport.Render("PDF");

            Response.AppendHeader("Content-Disposition", "inline; filename=AuditTrailReport.pdf");


            // Return the report as a PDF file
            return File(reportBytes, "application/pdf", "AuditTrailReport.pdf");
        }
    }
}