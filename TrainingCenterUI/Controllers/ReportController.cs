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

        public async Task<ActionResult> AvailableCoursesReport()
        {
            // Fetch the data for the report
            var reportData = await _Db.AvailableCouresViews.ToListAsync();

            var dataTable = new DataTable();
            dataTable.Columns.Add("AvailableCourseID", typeof(int));
            dataTable.Columns.Add("CourseID", typeof(int));
            dataTable.Columns.Add("CourseName", typeof(string));
            dataTable.Columns.Add("Duration", typeof(int));
            dataTable.Columns.Add("Requirement_CourseID", typeof(int));
            dataTable.Columns.Add("IsProgramming", typeof(bool));
            dataTable.Columns.Add("IsGroupDays1", typeof(bool));
            dataTable.Columns.Add("StartTime", typeof(TimeSpan));
            dataTable.Columns.Add("EndTime", typeof(TimeSpan));
            dataTable.Columns.Add("InstructorID", typeof(int));
            dataTable.Columns.Add("FirstName", typeof(string));
            //dataTable.Columns.Add("SecondName", typeof(string));
            //dataTable.Columns.Add("LastName", typeof(string));

            foreach (var item in reportData)
            {
                dataTable.Rows.Add(
                    item.AvailableCourseID,
                    item.CourseID,
                    item.CourseName,
                    item.Duration,
                    item.Requirement_CourseID,
                    item.IsProgramming,
                    item.IsGroupDays1,
                    item.StartTime,
                    item.EndTime,
                    item.InstructorID,
                    item.FirstName
                    //item.SecondName,
                    //item.LastName
                );
            }

            // Set up the ReportViewer
            var reportViewer = new ReportViewer
            {
                ProcessingMode = ProcessingMode.Local
            };

            reportViewer.LocalReport.ReportPath = Server.MapPath("~/Reports/CoursesReport.rdlc");
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ViewsDataSet", dataTable));

            var reportBytes = reportViewer.LocalReport.Render("PDF");

            Response.AppendHeader("Content-Disposition", "inline; filename=AvailableCoursesReport.pdf");

            // Return the report as a PDF file
            return File(reportBytes, "application/pdf", "AvailableCoursesReport.pdf");
        }

        public async Task<ActionResult> WaitingListReport()
        {
            // Fetch the data for the report
            var reportData = await _Db.WaitingListViews.ToListAsync();

            var dataTable = new DataTable();
            dataTable.Columns.Add("WaitingListID", typeof(int));
            dataTable.Columns.Add("StudentName", typeof(string));
            dataTable.Columns.Add("Email", typeof(string));
            dataTable.Columns.Add("Phone", typeof(string));
            dataTable.Columns.Add("CourseName", typeof(string));
            dataTable.Columns.Add("Duration", typeof(int));
            dataTable.Columns.Add("IsProgramming", typeof(bool));
            dataTable.Columns.Add("InstructorName", typeof(string));
            dataTable.Columns.Add("GroupName", typeof(string));
            dataTable.Columns.Add("InstructorEmail", typeof(string));
            dataTable.Columns.Add("InstructorPhone", typeof(string));
            dataTable.Columns.Add("StartTime", typeof(TimeSpan));
            dataTable.Columns.Add("EndTime", typeof(TimeSpan));
            dataTable.Columns.Add("IsGroupDays1", typeof(bool));

            foreach (var item in reportData)
            {
                dataTable.Rows.Add(
                    item.WaitingListID,
                    item.StudentName,
                    item.Email,
                    item.Phone,
                    item.CourseName,
                    item.Duration,
                    item.IsProgramming,
                    item.InstructorName,
                    item.GroupName,
                    item.InstructorEmail,
                    item.InstructorPhone,
                    item.StartTime,
                    item.EndTime,
                    item.IsGroupDays1
                );
            }

            // Set up the ReportViewer
            var reportViewer = new ReportViewer
            {
                ProcessingMode = ProcessingMode.Local
            };

            reportViewer.LocalReport.ReportPath = Server.MapPath("~/Reports/WaitingListReport.rdlc");
            reportViewer.LocalReport.DataSources.Add(new ReportDataSource("ViewsDataSet", dataTable));

            var reportBytes = reportViewer.LocalReport.Render("PDF");

            Response.AppendHeader("Content-Disposition", "inline; filename=WaitingListReport.pdf");

            // Return the report as a PDF file
            return File(reportBytes, "application/pdf", "WaitingListReport.pdf");
        }
    }
}