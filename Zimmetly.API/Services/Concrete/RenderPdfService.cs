using Microsoft.IdentityModel.Tokens;
using QuestPDF.Companion;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Zimmetly.API.Models;
using Zimmetly.API.Services.Abstract;

namespace Zimmetly.API.Services.Concrete
{
    public class RenderPdfService : IRenderPdfService
    {
        public RenderPdfService()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }
        public byte[] GeneratePdfInvoice(List<Product> products, string user, string date, string location, string title)
        {
            var document = Document.Create(container =>
            {
                #region PageOne
                container.Page(page =>
                {
                    byte[] imageData = File.ReadAllBytes("wwwroot/uploads/logo.png");

                    page.Size(PageSizes.A4);
                    page.Margin(15);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    #region Header
                    page.Header().Height(80).Border(1).BorderColor(Colors.Grey.Medium).Background(Colors.White).Column(col =>
                    {
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(50);
                                columns.RelativeColumn(50);
                                columns.RelativeColumn(50);
                            });


                            table.Cell().Height(80).Border(1).AlignCenter().AlignMiddle().Padding(10).Image(imageData);
                            table.Cell().Height(80).Border(1).AlignCenter().AlignMiddle().Text("PERSONEL ZİMMET FORMU").FontSize(16).Bold();
                            table.Cell().Height(80).Border(1).AlignLeft().AlignMiddle().Table(t =>
                            {
                                t.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(100);
                                });
                                t.Cell().Height(20).Border(1).AlignLeft().AlignMiddle().Text(text =>
                                { // Padding eklendi
                                    text.Span(" Doküman No").Bold();
                                    text.Span(": END_HSE_FRM_0053");
                                });
                                t.Cell().Height(20).Border(1).AlignLeft().AlignMiddle().Text(text =>
                                { // Padding eklendi
                                    text.Span(" Yayım Tarihi").Bold();
                                    text.Span(": 23.07.2019");
                                });
                                t.Cell().Height(20).Border(1).AlignLeft().AlignMiddle().Text(text =>
                                { // Padding eklendi
                                    text.Span(" Revizyon No").Bold();
                                    text.Span(": 00");
                                });
                                t.Cell().Height(20).Border(1).AlignLeft().AlignMiddle().Text(text =>
                                { // Padding eklendi
                                    text.Span(" Son Yayım").Bold();
                                    text.Span(": -");
                                });

                            });
                        });
                    });
                    #endregion

                    #region Content
                    page.Content().PaddingVertical(20).Border(1).Column(col =>
                    {
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(50); // Sütunun genişliği tablonun %50'si kadar olacak
                                columns.RelativeColumn(50); // Sütunun genişliği tablonun %50'si kadar olacak
                            });


                            table.Cell().Height(30).Border(1).AlignLeft().AlignMiddle().Text($" Personelin Adı Soyadı: {user}").Bold(); ;
                            table.Cell().Height(30).Border(1).AlignLeft().AlignMiddle().Text($" Zimmet Tarihi: {date}").Bold();
                            table.Cell().Height(30).Border(1).AlignLeft().AlignMiddle().Text($" Bölümü: {location}").Bold();
                            table.Cell().Height(30).Border(1).AlignLeft().AlignMiddle().Text($" Unvanı: {title}").Bold(); ;

                        });

                        col.Spacing(15); // Boşluk

                        // Zimmetlenen Malzemeler Başlığı
                        col.Item().AlignCenter().AlignMiddle().Text("ZİMMETLENEN MALZEMELER").FontSize(12).Bold();

                        // Tablo
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(40); // No (Önceki: 30)
                                columns.ConstantColumn(100); // Seri No (Önceki: 80)
                                columns.RelativeColumn(3); // Malzeme Adı (Önceki: 2)
                                columns.RelativeColumn(3); // Açıklama (Önceki: 2)
                            });

                            table.Header(header =>
                            {
                                header.Cell().Height(30).Border(1).AlignCenter().AlignMiddle().Text("No").Bold();
                                header.Cell().Height(30).Border(1).AlignCenter().AlignMiddle().Text("Seri No").Bold();
                                header.Cell().Height(30).Border(1).AlignCenter().AlignMiddle().Text("Malzeme Adı").Bold();
                                header.Cell().Height(30).Border(1).AlignCenter().AlignMiddle().Text("Açıklama").Bold();
                            });
                            int i = 0;
                            foreach (var item in products)
                            {
                                i++;
                                table.Cell().Border(1).AlignCenter().AlignMiddle().Padding(5).Text(i.ToString());
                                table.Cell().Border(1).AlignCenter().AlignMiddle().Padding(5).Text(item.Serial); // Yükseklik otomatik
                                table.Cell().Border(1).AlignCenter().AlignMiddle().Padding(5).Text(item.Name); // Yükseklik otomatik
                                table.Cell().Border(1).AlignCenter().AlignMiddle().Padding(5).Text(item.Description); // Yükseklik otomatik
                            }

                        });
                        // Tablo
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(50);
                                columns.RelativeColumn(50);
                            });


                            table.Cell().Height(120).Border(1).AlignLeft().AlignMiddle().Table(t =>
                            {
                                t.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(100);
                                });
                                t.Cell().Height(30).AlignCenter().AlignMiddle().Text(text =>
                                { // Padding eklendi
                                    text.Span(" Teslim EDEN Personel").Bold().Underline();
                                    text.Span(": ");
                                });
                                t.Cell().Height(30).AlignLeft().AlignMiddle().Text(text =>
                                { // Padding eklendi
                                    text.Span(" Adı Soyadı").Bold().Underline();
                                });
                                t.Cell().Height(60).AlignLeft().AlignMiddle().Text(text =>
                                { // Padding eklendi
                                    text.Span(" İmza").Bold().Underline();
                                });

                            });
                            table.Cell().Height(120).Border(1).AlignLeft().AlignMiddle().Table(t =>
                            {
                                t.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(100);
                                });
                                t.Cell().Height(30).AlignCenter().AlignMiddle().Text(text =>
                                { // Padding eklendi
                                    text.Span(" Teslim ALAN Personel").Bold().Underline();
                                    text.Span(": ");
                                });
                                t.Cell().Height(30).AlignLeft().AlignMiddle().Text(text =>
                                { // Padding eklendi
                                    text.Span(" Adı Soyadı").Bold().Underline();
                                });
                                t.Cell().Height(60).AlignLeft().AlignMiddle().Text(text =>
                                { // Padding eklendi
                                    text.Span(" İmza").Bold().Underline();
                                });

                            });
                        });


                    });
                    #endregion


                });
                #endregion

                #region PageTwo

                container.Page(page =>
                {

                    page.Size(PageSizes.A4);
                    page.Margin(15);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Content().Column(col =>
                    {
                        foreach (var item in products)
                        {
                            if (!item.Attachments.IsNullOrEmpty())
                            {
                                foreach (var att in item.Attachments)
                                {
                                    col.Item().Image(File.ReadAllBytes($"wwwroot/uploads/{att.Name}")).WithRasterDpi(100).FitWidth();
                                }
                            }
                        }
                    });

                });
                #endregion

            });

            //document.ShowInCompanion();

            return document.GeneratePdf();
        }
    }
}
