using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Sales_Inventory.Models;
using Sales_Inventory.Models.DTO;
using Sales_Inventory.Repository.Interfaces;

namespace Sales_Inventory.Repository.Implementations
{
    public class ReportRepository : IReportRepository
    {
        private readonly salesinventory_dbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ReportRepository(salesinventory_dbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> DailyCashSalesReport()
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string path = Path.Combine(webRootPath, "reports");
            string filePath = $"{path}/DailySalesReport.pdf";

            string[] table_header = { "Order No", "Customer Name", "Item Count", "Amount", "Payment Type", "Payment Date" };
            var table_data = ListDailyCashSalesReport();
            string total_sales = table_data.Sum(s => s.amount).ToString();

            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(filePath));
            Document doc = new Document(pdfDoc);
            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);

            doc.SetMargins(10, 10, 10, 10);

            doc.Add(new Paragraph("Daily Cash Sales Report").SetFontSize(18).SetFont(font).SetTextAlignment(TextAlignment.CENTER));
            doc.Add(new Paragraph(DateTime.Now.ToString("MM/dd/yyyy HH:mm")).SetFontSize(12).SetFont(font).SetTextAlignment(TextAlignment.CENTER));

            Table table = new Table(UnitValue.CreatePercentArray(new float[] { 20, 25, 20, 20, 20, 20 })).UseAllAvailableWidth();

            foreach (var header in table_header)
            {
                Cell cell = new Cell();
                cell.SetMinHeight(25);
                cell.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                cell.Add(new Paragraph(header).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(cell);
            }
            foreach (var data in table_data)
            {
                Cell order_no = new Cell();
                order_no.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                order_no.Add(new Paragraph(data.order_no).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(order_no);

                Cell customer_name = new Cell();
                customer_name.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                customer_name.Add(new Paragraph(data.customer_name).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(customer_name);

                Cell item_count = new Cell();
                item_count.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                item_count.Add(new Paragraph(data.item_count.ToString()).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(item_count);

                Cell amount = new Cell();
                amount.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                amount.Add(new Paragraph(data.amount.ToString()).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(amount);

                Cell payment_type = new Cell();
                payment_type.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                payment_type.Add(new Paragraph(data.payment_type).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(payment_type);

                Cell payment_date = new Cell();
                payment_date.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                payment_date.Add(new Paragraph(data.payment_date.ToString("MM/dd/yyyy HH:mm")).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(payment_date);
            }

            doc.Add(table);

            doc.Add(new Paragraph($"Total Sales:  {total_sales}").SetFontSize(10).SetTextAlignment(TextAlignment.RIGHT));

            doc.Close();

            return "/reports/DailySalesReport.pdf";
        }
        public async Task<string> DailyCreditSalesReport()
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            string path = Path.Combine(webRootPath, "reports");
            string filePath = $"{path}/DailyCreditSalesReport.pdf";

            string[] table_header = { "Order No", "Customer Name", "Item Count", "Amount", "Payment Type", "Payment Date" };
            var table_data = ListDailyCreditSalesReport();
            string total_sales = table_data.Sum(s => s.amount).ToString();

            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(filePath));
            Document doc = new Document(pdfDoc);
            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);

            doc.SetMargins(10, 10, 10, 10);

            doc.Add(new Paragraph("Daily Credit Sales Report").SetFontSize(18).SetFont(font).SetTextAlignment(TextAlignment.CENTER));
            doc.Add(new Paragraph(DateTime.Now.ToString("MM/dd/yyyy HH:mm")).SetFontSize(12).SetFont(font).SetTextAlignment(TextAlignment.CENTER));

            Table table = new Table(UnitValue.CreatePercentArray(new float[] { 20, 25, 20, 20, 20, 20 })).UseAllAvailableWidth();

            foreach (var header in table_header)
            {
                Cell cell = new Cell();
                cell.SetMinHeight(25);
                cell.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                cell.Add(new Paragraph(header).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(cell);
            }

            foreach (var data in table_data)
            {
                Cell order_no = new Cell();
                order_no.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                order_no.Add(new Paragraph(data.order_no).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(order_no);

                Cell customer_name = new Cell();
                customer_name.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                customer_name.Add(new Paragraph(data.customer_name).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(customer_name);

                Cell item_count = new Cell();
                item_count.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                item_count.Add(new Paragraph(data.item_count.ToString()).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(item_count);

                Cell amount = new Cell();
                amount.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                amount.Add(new Paragraph(data.amount.ToString()).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(amount);

                Cell payment_type = new Cell();
                payment_type.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                payment_type.Add(new Paragraph(data.payment_type).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(payment_type);

                Cell payment_date = new Cell();
                payment_date.SetVerticalAlignment(VerticalAlignment.MIDDLE);
                payment_date.Add(new Paragraph(data.payment_date.ToString("MM/dd/yyyy HH:mm")).SetTextAlignment(TextAlignment.CENTER));
                table.AddCell(payment_date);
            }

            doc.Add(table);
            doc.Add(new Paragraph($"Total Sales:  {total_sales}").SetFontSize(10).SetTextAlignment(TextAlignment.RIGHT));

            doc.Close();

            return "/reports/DailyCreditSalesReport.pdf";
        }

        private List<DailySalesReportDTO> ListDailyCashSalesReport()
        {
            return _context.TblOrderHeaders.Select(s => new DailySalesReportDTO
            {
                order_no = s.OrderNo,
                customer_name = s.CustomerName,
                item_count = s.TotalItems,
                amount = s.Gross,
                payment_type = s.PaymentType,
                payment_date = s.TransactionDate
            }).ToList();

        }
        private List<DailySalesReportDTO> ListDailyCreditSalesReport()
        {
            return _context.TblCreditHeaders.Select(s => new DailySalesReportDTO
            {
                order_no = s.OrderNo,
                customer_name = s.CustomerName,
                item_count = s.TotalItems,
                amount = s.Gross,
                payment_type = s.PaymentType,
                payment_date = s.TransactionDate
            }).ToList();
        }

    }
}
