using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
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
            string path = Path.Combine(webRootPath, "reports");
            string filePath = $"{path}/DailySalesReport.pdf";

            string[] table_header = { "Order No", "Customer Name", "Item Count", "Amount", "Payment Type", "Payment Date" };
            var table_data = _reportDAL.ListDailyCashSalesReport(dateFrom, dateTo);//ListDailyCashSalesReport();
            string total_sales = table_data.Sum(s => s.amount).ToString();

            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(filePath));
            Document doc = new Document(pdfDoc);
            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);

            doc.SetMargins(10, 10, 10, 10);

            doc.Add(new Paragraph("D'Home Hardware").SetMultipliedLeading(0.5f).SetFontSize(22).SetFont(font).SetTextAlignment(TextAlignment.CENTER));
            doc.Add(new Paragraph("Koronadal City, South Cotabato").SetMultipliedLeading(0.5f).SetFontSize(10).SetTextAlignment(TextAlignment.CENTER));
            doc.Add(new Paragraph("Daily Cash Sales Report").SetMultipliedLeading(0.5f).SetFontSize(10).SetTextAlignment(TextAlignment.CENTER));
            doc.Add(new Paragraph(DateTime.Now.ToString("MM/dd/yyyy")).SetMultipliedLeading(0.5f).SetFontSize(10).SetTextAlignment(TextAlignment.CENTER));

            doc.Add(new Paragraph(".").SetFont(font).SetFontSize(14));

            Table table = new Table(UnitValue.CreatePercentArray(new float[] { 20, 25, 20, 20, 20, 20 })).UseAllAvailableWidth();

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

                Cell amount = new Cell();
                amount.SetMinHeight(18);
                amount.SetBorder(Border.NO_BORDER);
                amount.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                amount.Add(new Paragraph(data.amount.ToString()).SetFontSize(9).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(amount);

                Cell payment_type = new Cell();
                payment_type.SetMinHeight(18);
                payment_type.SetBorder(Border.NO_BORDER);
                payment_type.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                payment_type.Add(new Paragraph(data.payment_type).SetFontSize(9).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(payment_type);

                Cell payment_date = new Cell();
                payment_date.SetMinHeight(18);
                payment_date.SetBorder(Border.NO_BORDER);
                payment_date.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                payment_date.Add(new Paragraph(data.payment_date.ToString("MM/dd/yyyy HH:mm")).SetFontSize(9).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(payment_date);
            }

            doc.Add(table);

            doc.Add(new Paragraph($"Total Sales:  {total_sales}").SetFontSize(10));

            doc.Close();

            return "/reports/DailySalesReport.pdf";
        }
        public async Task<string> DailyCreditSalesReport(DateTime dateFrom, DateTime dateTo)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string path = Path.Combine(webRootPath, "reports");
            string filePath = $"{path}/DailyCreditSalesReport.pdf";

            string[] table_header = { "Order No", "Customer Name", "Item Count", "Amount", "Payment Type", "Payment Date" };
            var table_data = _reportDAL.ListDailyCreditSalesReport(dateFrom, dateTo);
            string total_sales = table_data.Sum(s => s.amount).ToString();

            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(filePath));
            Document doc = new Document(pdfDoc);
            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);

            doc.SetMargins(10, 10, 10, 10);

            doc.Add(new Paragraph("D'Home Hardware").SetMultipliedLeading(0.5f).SetFontSize(22).SetFont(font).SetTextAlignment(TextAlignment.CENTER));
            doc.Add(new Paragraph("Koronadal City, South Cotabato").SetMultipliedLeading(0.5f).SetFontSize(10).SetTextAlignment(TextAlignment.CENTER));
            doc.Add(new Paragraph("Daily Credit Sales Report").SetMultipliedLeading(0.5f).SetFontSize(10).SetTextAlignment(TextAlignment.CENTER));
            doc.Add(new Paragraph(DateTime.Now.ToString("MM/dd/yyyy")).SetMultipliedLeading(0.5f).SetFontSize(10).SetTextAlignment(TextAlignment.CENTER));

            doc.Add(new Paragraph(".").SetMultipliedLeading(1.5f).SetFont(font).SetFontSize(14));

            Table table = new Table(UnitValue.CreatePercentArray(new float[] { 20, 25, 20, 20, 20, 20 })).UseAllAvailableWidth();

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

                Cell amount = new Cell();
                amount.SetMinHeight(18);
                amount.SetBorder(Border.NO_BORDER);
                amount.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                amount.Add(new Paragraph(data.amount.ToString()).SetFontSize(9).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(amount);

                Cell payment_type = new Cell();
                payment_type.SetMinHeight(18);
                payment_type.SetBorder(Border.NO_BORDER);
                payment_type.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                payment_type.Add(new Paragraph(data.payment_type).SetFontSize(9).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(payment_type);

                Cell payment_date = new Cell();
                payment_date.SetMinHeight(18);
                payment_date.SetBorder(Border.NO_BORDER);
                payment_date.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                payment_date.Add(new Paragraph(data.payment_date.ToString("MM/dd/yyyy HH:mm")).SetFontSize(9).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(payment_date);
            }

            doc.Add(table);
            doc.Add(new Paragraph($"Total Sales:  {total_sales}").SetFontSize(10));

            doc.Close();

            return "/reports/DailyCreditSalesReport.pdf";
        }
    }
}
