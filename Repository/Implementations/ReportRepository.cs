using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Sales_Inventory.DAL;
using Sales_Inventory.Helper;
using Sales_Inventory.Models;
using Sales_Inventory.Models.DTO;
using Sales_Inventory.Repository.Interfaces;
using System.Data;

namespace Sales_Inventory.Repository.Implementations
{
    public class ReportRepository : IReportRepository
    {
        private readonly salesinventory_dbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        private readonly ReportDAL _reportDAL;

        public ReportRepository(salesinventory_dbContext context, IWebHostEnvironment webHostEnvironment, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _reportDAL = new ReportDAL(_configuration);
        }

        public async Task<string> DailyCashSalesReport(DateTime dateFrom, DateTime dateTo)
        {
            var current_session = JsonConvert.DeserializeObject<Session>(_httpContextAccessor.HttpContext!.Session.GetString("UserSession")!);

            string webRootPath = _webHostEnvironment.WebRootPath;
            string path = System.IO.Path.Combine(webRootPath, "reports");
            string filePath = $"{path}/DailySalesReport.pdf";

            string[] table_header = { "Order No", "Customer Name", "Item Count", "Net", "Gross", "Discount", "Remarks", "Payment Type" };
            var table_data = _reportDAL.ListDailyCashSalesReport(dateFrom, dateTo);//ListDailyCashSalesReport();
            string total_sales = table_data.Sum(s => s.net).ToString();

            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(filePath));
            Document doc = new Document(pdfDoc, PageSize.A4.Rotate());
            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);

            doc.SetMargins(10, 10, 10, 10);

            doc.Add(new Paragraph("D' Home Hardware").SetMultipliedLeading(0.5f).SetFontSize(22).SetFont(font).SetTextAlignment(TextAlignment.CENTER));
            doc.Add(new Paragraph("Koronadal City, South Cotabato").SetMultipliedLeading(0.5f).SetFontSize(10).SetTextAlignment(TextAlignment.CENTER));
            doc.Add(new Paragraph("Daily Cash Sales Report").SetMultipliedLeading(0.5f).SetFontSize(10).SetTextAlignment(TextAlignment.CENTER));
            doc.Add(new Paragraph(DateTime.Now.ToString("MM/dd/yyyy")).SetMultipliedLeading(0.5f).SetFontSize(10).SetTextAlignment(TextAlignment.CENTER));

            doc.Add(new Paragraph(".").SetFont(font).SetFontSize(14));

            Table table = new Table(UnitValue.CreatePercentArray(new float[] { 10, 20, 10, 10, 10, 10, 10, 10 })).UseAllAvailableWidth();

            foreach (var header in table_header)
            {
                Cell cell = new Cell();
                cell.SetBackgroundColor(Color.ConvertRgbToCmyk(new DeviceRgb(192, 192, 192)));
                cell.SetBorder(Border.NO_BORDER);
                cell.SetMinHeight(24);
                cell.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                cell.Add(new Paragraph(header).SetFontSize(12).SetFont(font).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(cell);
            }
            foreach (var data in table_data)
            {
                Cell order_no = new Cell();
                order_no.SetMinHeight(18);
                order_no.SetBorder(Border.NO_BORDER);
                order_no.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                order_no.Add(new Paragraph(data.order_no).SetFontSize(9).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(order_no);

                Cell customer_name = new Cell();
                customer_name.SetMinHeight(18);
                customer_name.SetBorder(Border.NO_BORDER);
                customer_name.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                customer_name.Add(new Paragraph(data.customer_name).SetFontSize(9).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(customer_name);

                Cell item_count = new Cell();
                item_count.SetMinHeight(18);
                item_count.SetBorder(Border.NO_BORDER);
                item_count.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                item_count.Add(new Paragraph(data.item_count.ToString()).SetFontSize(9).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(item_count);

                Cell net = new Cell();
                net.SetMinHeight(18);
                net.SetBorder(Border.NO_BORDER);
                net.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                net.Add(new Paragraph(data.net.ToString()).SetFontSize(9).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(net);

                Cell amount = new Cell();
                amount.SetMinHeight(18);
                amount.SetBorder(Border.NO_BORDER);
                amount.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                amount.Add(new Paragraph(data.amount.ToString()).SetFontSize(9).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(amount);

                Cell discount = new Cell();
                discount.SetMinHeight(18);
                discount.SetBorder(Border.NO_BORDER);
                discount.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                discount.Add(new Paragraph(data.discount.ToString()).SetFontSize(9).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(discount);

                Cell remarks = new Cell();
                remarks.SetMinHeight(18);
                remarks.SetBorder(Border.NO_BORDER);
                remarks.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                remarks.Add(new Paragraph(data.remarks).SetFontSize(9).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(remarks);

                Cell payment_type = new Cell();
                payment_type.SetMinHeight(18);
                payment_type.SetBorder(Border.NO_BORDER);
                payment_type.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                payment_type.Add(new Paragraph(data.payment_type).SetFontSize(9).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(payment_type);

            }

            doc.Add(table);

            doc.Add(new Paragraph($"Total Cash Sales:  {total_sales}").SetFontSize(10));

            doc.Close();

            return "/reports/DailySalesReport.pdf";
        }
        public async Task<string> DailyCreditSalesReport(DateTime dateFrom, DateTime dateTo)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string path = System.IO.Path.Combine(webRootPath, "reports");
            string filePath = $"{path}/DailyCreditSalesReport.pdf";

            string[] table_header = { "Order No", "Customer Name", "Item Count", "Net", "Gross", "Discount", "Remarks", "Payment Type" };
            var table_data = _reportDAL.ListDailyCreditSalesReport(dateFrom, dateTo);
            string total_sales = table_data.Sum(s => s.net).ToString();

            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(filePath));
            Document doc = new Document(pdfDoc, PageSize.A4.Rotate());
            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);

            doc.SetMargins(10, 10, 10, 10);

            doc.Add(new Paragraph("D' Home Hardware").SetMultipliedLeading(0.5f).SetFontSize(22).SetFont(font).SetTextAlignment(TextAlignment.CENTER));
            doc.Add(new Paragraph("Koronadal City, South Cotabato").SetMultipliedLeading(0.5f).SetFontSize(10).SetTextAlignment(TextAlignment.CENTER));
            doc.Add(new Paragraph("Daily Credit Sales Report").SetMultipliedLeading(0.5f).SetFontSize(10).SetTextAlignment(TextAlignment.CENTER));
            doc.Add(new Paragraph(DateTime.Now.ToString("MM/dd/yyyy")).SetMultipliedLeading(0.5f).SetFontSize(10).SetTextAlignment(TextAlignment.CENTER));

            doc.Add(new Paragraph(".").SetMultipliedLeading(1.5f).SetFont(font).SetFontSize(14));

            Table table = new Table(UnitValue.CreatePercentArray(new float[] { 10, 20, 10, 10, 10, 10, 10, 10 })).UseAllAvailableWidth();

            foreach (var header in table_header)
            {
                Cell cell = new Cell();
                cell.SetBackgroundColor(Color.ConvertRgbToCmyk(new DeviceRgb(192, 192, 192)));
                cell.SetBorder(Border.NO_BORDER);
                cell.SetMinHeight(24);
                cell.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                cell.Add(new Paragraph(header).SetFontSize(12).SetFont(font).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(cell);
            }
            foreach (var data in table_data)
            {
                Cell order_no = new Cell();
                order_no.SetMinHeight(18);
                order_no.SetBorder(Border.NO_BORDER);
                order_no.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                order_no.Add(new Paragraph(data.order_no).SetFontSize(9).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(order_no);

                Cell customer_name = new Cell();
                customer_name.SetMinHeight(18);
                customer_name.SetBorder(Border.NO_BORDER);
                customer_name.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                customer_name.Add(new Paragraph(data.customer_name).SetFontSize(9).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(customer_name);

                Cell item_count = new Cell();
                item_count.SetMinHeight(18);
                item_count.SetBorder(Border.NO_BORDER);
                item_count.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                item_count.Add(new Paragraph(data.item_count.ToString()).SetFontSize(9).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(item_count);

                Cell net = new Cell();
                net.SetMinHeight(18);
                net.SetBorder(Border.NO_BORDER);
                net.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                net.Add(new Paragraph(data.net.ToString()).SetFontSize(9).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(net);

                Cell amount = new Cell();
                amount.SetMinHeight(18);
                amount.SetBorder(Border.NO_BORDER);
                amount.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                amount.Add(new Paragraph(data.amount.ToString()).SetFontSize(9).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(amount);

                Cell discount = new Cell();
                discount.SetMinHeight(18);
                discount.SetBorder(Border.NO_BORDER);
                discount.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                discount.Add(new Paragraph(data.discount.ToString()).SetFontSize(9).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(discount);

                Cell remarks = new Cell();
                remarks.SetMinHeight(18);
                remarks.SetBorder(Border.NO_BORDER);
                remarks.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                remarks.Add(new Paragraph(data.remarks).SetFontSize(9).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(remarks);

                Cell payment_type = new Cell();
                payment_type.SetMinHeight(18);
                payment_type.SetBorder(Border.NO_BORDER);
                payment_type.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                payment_type.Add(new Paragraph(data.payment_type).SetFontSize(9).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(payment_type);
            }

            doc.Add(table);
            doc.Add(new Paragraph($"Total Credit Sales:  {total_sales}").SetFontSize(10));

            doc.Close();

            return "/reports/DailyCreditSalesReport.pdf";
        }
        public async Task<string> DailyExpensesReport(DateTime dateFrom, DateTime dateTo)
        {

            string webRootPath = _webHostEnvironment.WebRootPath;
            string path = System.IO.Path.Combine(webRootPath, "reports");
            string filePath = $"{path}/DailyExpensesReport.pdf";

            string[] table_header = { "Expense Type", "Receiver", "Amount", "Date", "Added By" };
            var table_data = _reportDAL.ListDailyExpensesReport(dateFrom, dateTo);
            string total_expense = table_data.Sum(s => s.amount).ToString();

            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(filePath));
            Document doc = new Document(pdfDoc, PageSize.A4.Rotate());
            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);

            doc.SetMargins(10, 10, 10, 10);

            doc.Add(new Paragraph("D' Home Hardware").SetMultipliedLeading(0.5f).SetFontSize(22).SetFont(font).SetTextAlignment(TextAlignment.CENTER));
            doc.Add(new Paragraph("Koronadal City, South Cotabato").SetMultipliedLeading(0.5f).SetFontSize(10).SetTextAlignment(TextAlignment.CENTER));
            doc.Add(new Paragraph("Daily Expenses Report").SetMultipliedLeading(0.5f).SetFontSize(10).SetTextAlignment(TextAlignment.CENTER));
            doc.Add(new Paragraph(DateTime.Now.ToString("MM/dd/yyyy")).SetMultipliedLeading(0.5f).SetFontSize(10).SetTextAlignment(TextAlignment.CENTER));

            doc.Add(new Paragraph(".").SetFont(font).SetFontSize(14));

            Table table = new Table(UnitValue.CreatePercentArray(new float[] { 20, 20, 10, 10, 10 })).UseAllAvailableWidth();

            foreach (var header in table_header)
            {
                Cell cell = new Cell();
                cell.SetBackgroundColor(Color.ConvertRgbToCmyk(new DeviceRgb(192, 192, 192)));
                cell.SetBorder(Border.NO_BORDER);
                cell.SetMinHeight(24);
                cell.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                cell.Add(new Paragraph(header).SetFontSize(12).SetFont(font).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(cell);
            }
            foreach (var data in table_data)
            {
                Cell type = new Cell();
                type.SetMinHeight(18);
                type.SetBorder(Border.NO_BORDER);
                type.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                type.Add(new Paragraph(data.type).SetFontSize(9).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(type);

                Cell receiver = new Cell();
                receiver.SetMinHeight(18);
                receiver.SetBorder(Border.NO_BORDER);
                receiver.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                receiver.Add(new Paragraph(data.receiver).SetFontSize(9).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(receiver);

                Cell amount = new Cell();
                amount.SetMinHeight(18);
                amount.SetBorder(Border.NO_BORDER);
                amount.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                amount.Add(new Paragraph(data.amount.ToString()).SetFontSize(9).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(amount);

                Cell date = new Cell();
                date.SetMinHeight(18);
                date.SetBorder(Border.NO_BORDER);
                date.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                date.Add(new Paragraph(data.expense_date.ToString("MM/dd/yyyy")).SetFontSize(9).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(date);

                Cell user = new Cell();
                user.SetMinHeight(18);
                user.SetBorder(Border.NO_BORDER);
                user.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                user.Add(new Paragraph(data.user).SetFontSize(9).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(user);

            }

            doc.Add(table);

            doc.Add(new Paragraph($"Total Sales:  {total_expense}").SetFontSize(10));

            doc.Close();

            return "/reports/DailyExpensesReport.pdf";
        }

        public async Task<string> InventoryHistoryReport(DateTime dateFrom, DateTime dateTo)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string path = System.IO.Path.Combine(webRootPath, "reports");
            string filePath = $"{path}/InventoryHistory.pdf";

            string[] table_header = { "Name", "Code", "Barcode", "Old Qty", "Added Qty", "New Qty", "Date Added", "Added By" };
            var table_data = _reportDAL.InventoryHistoryReport(dateFrom, dateTo);

            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(filePath));
            Document doc = new Document(pdfDoc, PageSize.A4.Rotate());
            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);

            doc.SetMargins(10, 10, 10, 10);

            doc.Add(new Paragraph("D' Home Hardware").SetMultipliedLeading(0.5f).SetFontSize(22).SetFont(font).SetTextAlignment(TextAlignment.CENTER));
            doc.Add(new Paragraph("Koronadal City, South Cotabato").SetMultipliedLeading(0.5f).SetFontSize(10).SetTextAlignment(TextAlignment.CENTER));
            doc.Add(new Paragraph("Daily Expenses Report").SetMultipliedLeading(0.5f).SetFontSize(10).SetTextAlignment(TextAlignment.CENTER));
            doc.Add(new Paragraph(DateTime.Now.ToString("MM/dd/yyyy")).SetMultipliedLeading(0.5f).SetFontSize(10).SetTextAlignment(TextAlignment.CENTER));

            doc.Add(new Paragraph(".").SetFont(font).SetFontSize(14));

            Table table = new Table(UnitValue.CreatePercentArray(new float[] { 10, 10, 10, 10, 10, 10, 10,10 })).UseAllAvailableWidth();

            foreach (var header in table_header)
            {
                Cell cell = new Cell();
                cell.SetBackgroundColor(Color.ConvertRgbToCmyk(new DeviceRgb(192, 192, 192)));
                cell.SetBorder(Border.NO_BORDER);
                cell.SetMinHeight(24);
                cell.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                cell.Add(new Paragraph(header).SetFontSize(12).SetFont(font).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(cell);
            }
            foreach (var data in table_data)
            {
                Cell product_name = new Cell();
                product_name.SetMinHeight(18);
                product_name.SetBorder(Border.NO_BORDER);
                product_name.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                product_name.Add(new Paragraph(data.product_name).SetFontSize(10).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(product_name);

                Cell product_code = new Cell();
                product_code.SetMinHeight(18);
                product_code.SetBorder(Border.NO_BORDER);
                product_code.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                product_code.Add(new Paragraph(data.product_code).SetFontSize(10).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(product_code);

                Cell barcode = new Cell();
                barcode.SetMinHeight(18);
                barcode.SetBorder(Border.NO_BORDER);
                barcode.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                barcode.Add(new Paragraph(data.barcode).SetFontSize(10).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(barcode);

                Cell old_qty = new Cell();
                old_qty.SetMinHeight(18);
                old_qty.SetBorder(Border.NO_BORDER);
                old_qty.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                old_qty.Add(new Paragraph(data.old_qty.ToString()).SetFontSize(10).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(old_qty);

                Cell added_qty = new Cell();
                added_qty.SetMinHeight(18);
                added_qty.SetBorder(Border.NO_BORDER);
                added_qty.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                added_qty.Add(new Paragraph(data.added_qty.ToString()).SetFontSize(10).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(added_qty);

                Cell new_qty = new Cell();
                new_qty.SetMinHeight(18);
                new_qty.SetBorder(Border.NO_BORDER);
                new_qty.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                new_qty.Add(new Paragraph(data.new_qty.ToString()).SetFontSize(10).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(new_qty);

                Cell date_added = new Cell();
                date_added.SetMinHeight(18);
                date_added.SetBorder(Border.NO_BORDER);
                date_added.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                date_added.Add(new Paragraph(data.date_added.ToString("MM/dd/yyyy")).SetFontSize(10).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(date_added);

                Cell added_by = new Cell();
                added_by.SetMinHeight(18);
                added_by.SetBorder(Border.NO_BORDER);
                added_by.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                added_by.Add(new Paragraph(data.added_by).SetFontSize(10).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(added_by);
            }

            doc.Add(table);
            doc.Close();

            return "/reports/InventoryHistory.pdf";
        }
    }
}
