using Library.Assess.Models.TalentMatchSelectionReportPDF;
using Library.Assess.Utilities;

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Library.Assess.Services.TalentMatchSelectionReportPDF;

public interface ITalentMatchSelectionReportPDFService
{
    /// <summary>
    /// Generates a Talent Report PDF based on the provided request and returns a response object.
    /// </summary>
    /// <param name="request">The request containing all data for the Talent Report.</param>
    /// <returns>A <see cref="TalentMatchReportResponse"/> containing the PDF data and metadata.</returns>
    Task<TalentMatchSelectionReportResponse> GenerateReportAsync(TalentMatchSelectionReportRequest request);

    /// <summary>
    /// Generates a Talent Report PDF and returns it as a byte array.
    /// </summary>
    /// <param name="request">The request containing all data for the Talent Report.</param>
    /// <returns>A byte array representing the generated PDF file.</returns>
    Task<byte[]> GeneratePdfBytesAsync(TalentMatchSelectionReportRequest request);
}

public class TalentMatchSelectionReportPDFService : ITalentMatchSelectionReportPDFService
{
    public TalentMatchSelectionReportPDFService()
    {
        // Set QuestPDF license (use Community for free version)
        QuestPDF.Settings.License = LicenseType.Community;
    }

    /// <summary>
    /// Generates a Talent Report PDF based on the provided request and returns a response object.
    /// </summary>
    /// <param name="request">The request containing all data for the Talent Report.</param>
    /// <returns>A <see cref="TalentReportResponse"/> containing the PDF data and metadata.</returns>
    public async Task<TalentMatchSelectionReportResponse> GenerateReportAsync(TalentMatchSelectionReportRequest request)
    {
        try
        {
            var pdfBytes = await GeneratePdfBytesAsync(request);
            var fileName = $"TalentReport_{request.EmployeeName.Replace(" ", "_")}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

            return new TalentMatchSelectionReportResponse
            {
                Success = true,
                PdfData = pdfBytes,
                FileName = fileName,
                GeneratedAt = DateTime.Now,
                Message = "Report generated successfully"
            };
        }
        catch (Exception ex)
        {
            return new TalentMatchSelectionReportResponse
            {
                Success = false,
                Message = $"Error generating report: {ex.Message}",
                GeneratedAt = DateTime.Now
            };
        }
    }

    /// <summary>
    /// Generates a Talent Report PDF and returns it as a byte array.
    /// </summary>
    /// <param name="request">The request containing all data for the Talent Report.</param>
    /// <returns>A byte array representing the generated PDF file.</returns>
    public async Task<byte[]> GeneratePdfBytesAsync(TalentMatchSelectionReportRequest request)
    {
        return await Task.Run(() =>
        {
            try
            {
                // If both work individually, try together
                var fullDoc = Document.Create(container =>
                {
                    container.Page(page => ComposePage1(page, request));
                    container.Page(page => ComposePage2(page, request));
                    container.Page(page => ComposePage3(page, request));
                    container.Page(page => ComposePage4(page, request));
                    container.Page(page => ComposePage5(page, request));
                    container.Page(page => ComposePage6(page, request));
                    container.Page(page => ComposePage7(page, request));
                    container.Page(page => ComposePage8(page, request));
                    container.Page(page => ComposePage9(page, request));
                    container.Page(page => ComposePage10(page, request));
                    container.Page(page => ComposePage11(page, request));
                    container.Page(page => ComposePage12(page, request));
                    container.Page(page => ComposePage13(page, request));
                    container.Page(page => ComposePage14(page, request));
                    container.Page(page => ComposePage15(page, request));
                    container.Page(page => ComposePage16(page, request));
                });

                return fullDoc.GeneratePdf();
            }
            catch (Exception ex)
            {
                throw;
            }
        });
    }

    #region Page Composers

    #region Page 1 Methods
    /// <summary>
    /// Composes the content for Page 1 of the Talent Report PDF.
    /// </summary>
    /// <param name="page">The page descriptor representing the PDF page to be composed.</param>
    /// <param name="request">The <see cref="TalentMatchReportRequest"/> containing all data for the report.</param>
    private void ComposePage1(PageDescriptor page, TalentMatchSelectionReportRequest request)
    {
        page.Size(PageSizes.A4);
        page.Margin(0);
        page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Arial"));

        page.Content().Column(column =>
        {
            column.Item().Element(c => ComposeHeader(c, request));

            column.Item().Padding(40).Column(contentColumn =>
            {
                contentColumn.Item().Element(c => ComposeTitle(c, request));
                contentColumn.Item().PaddingTop(40).Element(c => ComposeEmployeeInfo(c, request));
                contentColumn.Item().PaddingTop(25).Element(c => ComposeDescription(c, request));
                contentColumn.Item().PaddingTop(15).Element(c => ComposeDevelopmentSummary(c, request));
                contentColumn.Item().PaddingTop(15).Element(c => ComposeUsageInstructions(c, request));
                contentColumn.Item().PaddingTop(15).Element(c => ComposeValidityInfo(c, request));
                contentColumn.Item().PaddingTop(25).Element(c => ComposeFooterInfo(c, request));
            });

        });
        // Fixed footer at page bottom
        ComposeBottomDecoration(page, request);
    }
    #endregion

    #region Page 2 Methods
    /// <summary>
    /// Composes the content for Page 2 of the Talent Report PDF.
    /// </summary>
    /// <param name="page">The <see cref="PageDescriptor"/> representing the PDF page to be composed.</param>
    /// <param name="request">The <see cref="TalentMatchSelectionReportRequest"/> containing all data for the report.</param>
    private void ComposePage2(PageDescriptor page, TalentMatchSelectionReportRequest request)
    {
        page.Size(PageSizes.A4);
        page.Margin(0);
        page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Arial"));

        // Prevent headers/footers on overflow pages
        page.Header().ShowOnce().Element(c => ComposePageHeader(c, request));
        page.Footer().ShowOnce().Element(c => ComposePageFooter(c, request, 2));

        page.Content().PaddingHorizontal(20).PaddingVertical(15)
            .Element(c => ComposePage2Content(c, request));
    }
    #endregion

    #region Page 3 Methods
    /// <summary>
    /// Composes the content for Page 3 of the Talent Report PDF.
    /// </summary>
    /// <param name="page">The <see cref="PageDescriptor"/> representing the PDF page to be composed.</param>
    /// <param name="request">The <see cref="TalentMatchSelectionReportRequest"/> containing all data for the report.</param>
    private void ComposePage3(PageDescriptor page, TalentMatchSelectionReportRequest request)
    {
        page.Size(PageSizes.A4);
        page.Margin(0);
        page.DefaultTextStyle(x => x.FontSize(8).FontFamily("Arial"));

        // Prevent headers/footers on overflow pages
        page.Header().ShowOnce().Element(c => ComposePageHeader(c, request));
        page.Footer().ShowOnce().Element(c => ComposePageFooter(c, request, 3));

        page.Content().PaddingHorizontal(20).PaddingVertical(15)
            .Element(c => ComposePage3Content(c, request));
    }
    #endregion

    #region Page 4 Methods

    /// <summary>
    /// Composes the content for Page 4 of the Talent Report PDF.
    /// </summary>
    /// <param name="page">The <see cref="PageDescriptor"/> representing the PDF page to be composed.</param>
    /// <param name="request">The <see cref="TalentMatchSelectionReportRequest"/> containing all data for the report.</param>
    private void ComposePage4(PageDescriptor page, TalentMatchSelectionReportRequest request)
    {
        page.Size(PageSizes.A4);
        page.Margin(0);
        page.DefaultTextStyle(x => x.FontSize(8).FontFamily("Arial"));

        // Prevent headers/footers on overflow pages
        page.Header().ShowOnce().Element(c => ComposePageHeader(c, request));
        page.Footer().ShowOnce().Element(c => ComposePageFooter(c, request, 4));

        page.Content().PaddingHorizontal(20).PaddingVertical(15)
            .Element(c => ComposePage4Content(c, request));
    }
    #endregion

    #region Page 5 Methods

    /// <summary>
    /// Composes the content for Page 5 of the Talent Report PDF.
    /// </summary>
    /// <param name="page">The <see cref="PageDescriptor"/> representing the PDF page to be composed.</param>
    /// <param name="request">The <see cref="TalentMatchReportRequest"/> containing all data for the report.</param>
    private void ComposePage5(PageDescriptor page, TalentMatchSelectionReportRequest request)
    {
        page.Size(PageSizes.A4);
        page.Margin(0);
        page.DefaultTextStyle(x => x.FontSize(8).FontFamily("Arial"));

        // Prevent headers/footers on overflow pages
        page.Header().ShowOnce().Element(c => ComposePageHeader(c, request));
        page.Footer().ShowOnce().Element(c => ComposePageFooter(c, request, 5));

        page.Content().PaddingHorizontal(20).PaddingVertical(15)
            .Element(c => ComposePage5Content(c, request));
    }
    #endregion

    #region Page 6 Methods

    /// <summary>
    /// Composes the content for Page 6 of the Talent Report PDF.
    /// </summary>
    /// <param name="page">The <see cref="PageDescriptor"/> representing the PDF page to be composed.</param>
    /// <param name="request">The <see cref="TalentMatchReportRequest"/> containing all data for the report.</param>
    private void ComposePage6(PageDescriptor page, TalentMatchSelectionReportRequest request)
    {
        page.Size(PageSizes.A4);
        page.Margin(0);
        page.DefaultTextStyle(x => x.FontSize(8).FontFamily("Arial"));

        // Prevent headers/footers on overflow pages
        page.Header().ShowOnce().Element(c => ComposePageHeader(c, request));
        page.Footer().ShowOnce().Element(c => ComposePageFooter(c, request, 6));

        page.Content().PaddingHorizontal(20).PaddingVertical(15)
            .Element(c => ComposePage6Content(c, request));
    }
    #endregion

    #region Page 7 Methods

    /// <summary>
    /// Composes the content for Page 7 of the Talent Report PDF.
    /// </summary>
    /// <param name="page">The <see cref="PageDescriptor"/> representing the PDF page to be composed.</param>
    /// <param name="request">The <see cref="TalentMatchReportRequest"/> containing all data for the report.</param>
    private void ComposePage7(PageDescriptor page, TalentMatchSelectionReportRequest request)
    {
        page.Size(PageSizes.A4);
        page.Margin(0);
        page.DefaultTextStyle(x => x.FontSize(8).FontFamily("Arial"));

        // Prevent headers/footers on overflow pages
        page.Header().ShowOnce().Element(c => ComposePageHeader(c, request));
        page.Footer().ShowOnce().Element(c => ComposePageFooter(c, request, 7));

        page.Content().PaddingHorizontal(20).PaddingVertical(15)
            .Element(c => ComposePage7Content(c, request));
    }
    #endregion

    #region Page 8 Methods

    /// <summary>
    /// Composes the content for Page 8 of the Talent Report PDF.
    /// </summary>
    /// <param name="page">The <see cref="PageDescriptor"/> representing the PDF page to be composed.</param>
    /// <param name="request">The <see cref="TalentMatchReportRequest"/> containing all data for the report.</param>
    private void ComposePage8(PageDescriptor page, TalentMatchSelectionReportRequest request)
    {
        page.Size(PageSizes.A4);
        page.Margin(0);
        page.DefaultTextStyle(x => x.FontSize(8).FontFamily("Arial"));

        // Prevent headers/footers on overflow pages
        page.Header().ShowOnce().Element(c => ComposePageHeader(c, request));
        page.Footer().ShowOnce().Element(c => ComposePageFooter(c, request, 8));

        page.Content().PaddingHorizontal(20).PaddingVertical(15)
            .Element(c => ComposePage8Content(c, request));
    }
    #endregion

    #region Page 9 Methods

    /// <summary>
    /// Composes the content for Page 9 of the Talent Report PDF.
    /// </summary>
    /// <param name="page">The <see cref="PageDescriptor"/> representing the PDF page to be composed.</param>
    /// <param name="request">The <see cref="TalentMatchReportRequest"/> containing all data for the report.</param>
    private void ComposePage9(PageDescriptor page, TalentMatchSelectionReportRequest request)
    {
        page.Size(PageSizes.A4);
        page.Margin(0);
        page.DefaultTextStyle(x => x.FontSize(8).FontFamily("Arial"));

        // Prevent headers/footers on overflow pages
        page.Header().ShowOnce().Element(c => ComposePageHeader(c, request));
        page.Footer().ShowOnce().Element(c => ComposePageFooter(c, request, 9));

        page.Content().PaddingHorizontal(20).PaddingVertical(15)
            .Element(c => ComposePage9Content(c, request));
    }
    #endregion

    #region Page 10 Methods

    /// <summary>
    /// Composes the content for Page 10 of the Talent Report PDF.
    /// </summary>
    /// <param name="page">The <see cref="PageDescriptor"/> representing the PDF page to be composed.</param>
    /// <param name="request">The <see cref="TalentMatchReportRequest"/> containing all data for the report.</param>
    private void ComposePage10(PageDescriptor page, TalentMatchSelectionReportRequest request)
    {
        page.Size(PageSizes.A4);
        page.Margin(0);
        page.DefaultTextStyle(x => x.FontSize(8).FontFamily("Arial"));

        // Prevent headers/footers on overflow pages
        page.Header().ShowOnce().Element(c => ComposePageHeader(c, request));
        page.Footer().ShowOnce().Element(c => ComposePageFooter(c, request, 10));

        page.Content().PaddingHorizontal(20).PaddingVertical(15)
            .Element(c => ComposePage10Content(c, request));
    }
    #endregion

    #region Page 11 Methods

    /// <summary>
    /// Composes the content for Page 6 of the Talent Report PDF.
    /// </summary>
    /// <param name="page">The <see cref="PageDescriptor"/> representing the PDF page to be composed.</param>
    /// <param name="request">The <see cref="TalentMatchReportRequest"/> containing all data for the report.</param>
    private void ComposePage11(PageDescriptor page, TalentMatchSelectionReportRequest request)
    {
        page.Size(PageSizes.A4);
        page.Margin(0);
        page.DefaultTextStyle(x => x.FontSize(8).FontFamily("Arial"));

        // Prevent headers/footers on overflow pages
        page.Header().ShowOnce().Element(c => ComposePageHeader(c, request));
        page.Footer().ShowOnce().Element(c => ComposePageFooter(c, request,11));

        page.Content().PaddingHorizontal(20).PaddingVertical(15)
            .Element(c => ComposePage11Content(c, request));
    }
    #endregion

    #region Page 12 Methods

    /// <summary>
    /// Composes the content for Page 6 of the Talent Report PDF.
    /// </summary>
    /// <param name="page">The <see cref="PageDescriptor"/> representing the PDF page to be composed.</param>
    /// <param name="request">The <see cref="TalentMatchReportRequest"/> containing all data for the report.</param>
    private void ComposePage12(PageDescriptor page, TalentMatchSelectionReportRequest request)
    {
        page.Size(PageSizes.A4);
        page.Margin(0);
        page.DefaultTextStyle(x => x.FontSize(8).FontFamily("Arial"));

        // Prevent headers/footers on overflow pages
        page.Header().ShowOnce().Element(c => ComposePageHeader(c, request));
        page.Footer().ShowOnce().Element(c => ComposePageFooter(c, request, 12));

        page.Content().PaddingHorizontal(20).PaddingVertical(15)
            .Element(c => ComposePage12Content(c, request));
    }
    #endregion

    #region Page 13 Methods

    /// <summary>
    /// Composes the content for Page 6 of the Talent Report PDF.
    /// </summary>
    /// <param name="page">The <see cref="PageDescriptor"/> representing the PDF page to be composed.</param>
    /// <param name="request">The <see cref="TalentMatchReportRequest"/> containing all data for the report.</param>
    private void ComposePage13(PageDescriptor page, TalentMatchSelectionReportRequest request)
    {
        page.Size(PageSizes.A4);
        page.Margin(0);
        page.DefaultTextStyle(x => x.FontSize(8).FontFamily("Arial"));

        // Prevent headers/footers on overflow pages
        page.Header().ShowOnce().Element(c => ComposePageHeader(c, request));
        page.Footer().ShowOnce().Element(c => ComposePageFooter(c, request, 13));

        page.Content().PaddingHorizontal(20).PaddingVertical(15)
            .Element(c => ComposePage13Content(c, request));
    }
    #endregion

    #region Page 14 Methods

    /// <summary>
    /// Composes the content for Page 14 of the Talent Report PDF.
    /// </summary>
    /// <param name="page">The <see cref="PageDescriptor"/> representing the PDF page to be composed.</param>
    /// <param name="request">The <see cref="TalentMatchReportRequest"/> containing all data for the report.</param>
    private void ComposePage14(PageDescriptor page, TalentMatchSelectionReportRequest request)
    {
        page.Size(PageSizes.A4);
        page.Margin(0);
        page.DefaultTextStyle(x => x.FontSize(8).FontFamily("Arial"));

        // Prevent headers/footers on overflow pages
        page.Header().ShowOnce().Element(c => ComposePageHeader(c, request));
        page.Footer().ShowOnce().Element(c => ComposePageFooter(c, request, 14));

        page.Content().PaddingHorizontal(20).PaddingVertical(15)
            .Element(c => ComposePage14Content(c, request));
    }
    #endregion

    #region Page 15 Methods

    /// <summary>
    /// Composes the content for Page 15 of the Talent Report PDF.
    /// </summary>
    /// <param name="page">The <see cref="PageDescriptor"/> representing the PDF page to be composed.</param>
    /// <param name="request">The <see cref="TalentMatchReportRequest"/> containing all data for the report.</param>
    private void ComposePage15(PageDescriptor page, TalentMatchSelectionReportRequest request)
    {
        page.Size(PageSizes.A4);
        page.Margin(0);
        page.DefaultTextStyle(x => x.FontSize(8).FontFamily("Arial"));

        // Prevent headers/footers on overflow pages
        page.Header().ShowOnce().Element(c => ComposePageHeader(c, request));
        page.Footer().ShowOnce().Element(c => ComposePageFooter(c, request, 15));

        page.Content().PaddingHorizontal(20).PaddingVertical(15)
            .Element(c => ComposePage15Content(c, request));
    }
    #endregion

    #region Page 16 Methods

    /// <summary>
    /// Composes the content for Page 16 of the Talent Report PDF.
    /// </summary>
    /// <param name="page">The <see cref="PageDescriptor"/> representing the PDF page to be composed.</param>
    /// <param name="request">The <see cref="TalentMatchReportRequest"/> containing all data for the report.</param>
    private void ComposePage16(PageDescriptor page, TalentMatchSelectionReportRequest request)
    {
        page.Size(PageSizes.A4);
        page.Margin(0);
        page.DefaultTextStyle(x => x.FontSize(8).FontFamily("Arial"));

        // Prevent headers/footers on overflow pages
        page.Header().ShowOnce().Element(c => ComposePageHeader(c, request));
        page.Footer().ShowOnce().Element(c => ComposePageFooter(c, request, 16));

        page.Content().PaddingHorizontal(20).PaddingVertical(15)
            .Element(c => ComposePage16Content(c, request));
    }
    #endregion

    #endregion

    #region Section Composers

    #region Generic Header/Footer Methods from page 2-6
    /// <summary>
    /// Composes the header section for a PDF page, including employee information, company details, position, and company logo.
    /// </summary>
    /// <param name="container">The <see cref="IContainer"/> used to compose the header layout.</param>
    /// <param name="request">The <see cref="TalentMatchSelectionReportRequest"/> containing all data required for the header.</param>
    private void ComposePageHeader(IContainer container, TalentMatchSelectionReportRequest request)
    {
        container.Height(40) // Further reduced
           .Background(Colors.White)
           .BorderBottom(1)
           .BorderColor(Colors.Grey.Lighten2)
           .Row(row =>
           {
               // Left side: Employee info + Company + Position
               row.RelativeItem()
                       .PaddingLeft(30)
                       .PaddingVertical(8) // Further reduced
                       .Column(col =>
                       {
                           col.Item().Text(text =>
                           {
                               text.DefaultTextStyle(x => x.FontSize(8).FontColor(Colors.Black)); // Further reduced

                               text.Span(request.EmployeeName).Bold();
                               text.Span(" | ");
                               text.Span($"{request.Page2.ReportDate:dd MMM yyyy}");
                               text.Span(" | ");
                               text.Span(request.ReportType).Bold();

                           });

                           col.Item().Text($"{request.CompanyName} - {request.Position}")
                               .FontSize(8) // Further reduced
                               .FontColor(Colors.Black)
                               .WrapAnywhere();
                       });

               // Right side: Logo 
               row.ConstantItem(180) // Further reduced
                   .PaddingRight(30)
                   .PaddingVertical(8) // Further reduced
                   .AlignRight()
                   .AlignMiddle()
                   .Row(rightRow =>
                   {
                       // Define a consistent height for both
                       const float logoHeight = 20; // Further reduced

                       // Logo container
                       rightRow.AutoItem().Height(logoHeight).AlignMiddle().Container().Element(logo =>
                       {
                           if (!string.IsNullOrEmpty(request.CompanyLogo) && IsValidBase64(request.CompanyLogo))
                           {
                               try
                               {
                                   var cleanBase64 = ExtractBase64FromDataUrl(request.CompanyLogo);
                                   var imageBytes = Convert.FromBase64String(cleanBase64);
                                   logo.Image(imageBytes, ImageScaling.FitHeight);
                               }
                               catch
                               {
                                   logo.Background(Colors.Red.Medium)
                                       .AlignCenter()
                                       .AlignMiddle()
                                       .Text("Logo").FontSize(6).FontColor(Colors.White);
                               }
                           }
                           else
                           {
                               logo.Background(Colors.Red.Medium)
                                   .AlignCenter()
                                   .AlignMiddle()
                                   .Text("Logo").FontSize(6).FontColor(Colors.White);
                           }
                       });
                   });
           });
    }

    /// <summary>
    /// Composes the footer section for a PDF page, including footer text and page number.
    /// </summary>
    /// <param name="container">The <see cref="IContainer"/> used to compose the footer layout.</param>
    /// <param name="request">The <see cref="TalentMatchSelectionReportRequest"/> containing all data required for the footer.</param>
    /// <param name="pageNumber">The current page number to display in the footer.</param>
    private void ComposePageFooter(IContainer container, TalentMatchSelectionReportRequest request, int pageNumber)
    {
        container
            .Background(Colors.White)
            .BorderTop(1)
            .BorderColor(Colors.Grey.Lighten3)
            .PaddingHorizontal(40)
            .PaddingVertical(15)
            .Row(row =>
            {
                // LEFT: Footer Text + Logo + URL
                row.RelativeItem().AlignMiddle().Row(leftRow =>
                {
                    // Footer text
                    if (!string.IsNullOrEmpty(request.FooterText))
                    {
                        leftRow.AutoItem().Text(request.FooterText)
                            .FontSize(10)
                            .FontColor(Colors.Grey.Darken1)
                            .WrapAnywhere();
                        leftRow.ConstantItem(5); // spacing
                    }

                    // Company logo
                    leftRow.AutoItem().Container().Height(16).Width(40).AlignMiddle().Element(element =>
                    {
                        if (!string.IsNullOrWhiteSpace(request?.PoweredByLogo) && IsValidBase64(request.PoweredByLogo))
                        {
                            try
                            {
                                var cleanBase64 = ExtractBase64FromDataUrl(request.PoweredByLogo);
                                var imageBytes = Convert.FromBase64String(cleanBase64);
                                element.Image(imageBytes, ImageScaling.FitArea);
                            }
                            catch
                            {
                                element.AlignCenter().AlignMiddle().Text("Logo")
                                    .FontSize(10).FontColor(Colors.Grey.Darken1);
                            }
                        }
                        else
                        {
                            element.AlignCenter().AlignMiddle().Text("Logo")
                                .FontSize(10).FontColor(Colors.Grey.Darken1);
                        }
                    });

                    leftRow.ConstantItem(5); // spacing

                    // Separator
                    leftRow.AutoItem().Text("|")
                        .FontSize(14).FontColor(Colors.Grey.Darken1).Bold();
                    leftRow.ConstantItem(5); // spacing

                    // Footer URL
                    if (!string.IsNullOrEmpty(request.Page2.FooterUrl))
                        leftRow.AutoItem().Text(request.Page2.FooterUrl)
                            .FontSize(10)
                            .FontColor(Colors.Grey.Darken1)
                            .WrapAnywhere();
                });

                // RIGHT: Page number
                row.AutoItem().AlignRight().AlignMiddle().Text($"Page {pageNumber}")
                    .FontSize(10)
                    .FontColor(Colors.Grey.Darken1);
            });
    }

    #endregion

    #region Page 1 Methods

    /// <summary>
    /// Composes the top header section of the report page with company logo and name.
    /// </summary>
    /// <param name="container">The <see cref="IContainer"/> used to compose the header layout.</param>
    /// <param name="request">The <see cref="TalentMatchReportRequest"/> containing company logo and name.</param>
    private void ComposeHeader(IContainer container, TalentMatchSelectionReportRequest request)
    {
        container.PaddingTop(20).PaddingHorizontal(40).Row(row =>
        {
            row.RelativeItem().AlignLeft()
                .Padding(12).Row(logoRow =>
                {
                    if (!string.IsNullOrEmpty(request.CompanyLogo) && IsValidBase64(request.CompanyLogo))
                    {
                        try
                        {
                            var cleanBase64 = ExtractBase64FromDataUrl(request.CompanyLogo);
                            var imageBytes = Convert.FromBase64String(cleanBase64);

                            logoRow.AutoItem().PaddingRight(12).AlignCenter()
                                .Width(200).Height(80).Image(imageBytes);
                        }
                        catch
                        {
                            logoRow.AutoItem().PaddingRight(12).AlignCenter()
                                .Width(80).Height(80).Background(Colors.Red.Medium);
                        }
                    }
                    else
                    {
                        logoRow.AutoItem().PaddingRight(12).AlignCenter()
                            .Width(80).Height(80).Background(Colors.Red.Medium);
                    }
                });
        });
    }


    /// <summary>
    /// Composes the main report title and subtitle on Page 1.
    /// </summary>
    /// <param name="container">The <see cref="IContainer"/> used to compose the title layout.</param>
    /// <param name="request">The <see cref="TalentMatchReportRequest"/> containing the title and subtitle.</param>
    private void ComposeTitle(IContainer container, TalentMatchSelectionReportRequest request)
    {
        container.Column(titleColumn =>
        {
            titleColumn.Item().Text(request.Page1.ReportTitle)
                .FontSize(30).FontColor(Colors.Black);

            titleColumn.Item().PaddingTop(5).Text(request.Page1.ReportSubtitle)
                .FontSize(28).SemiBold().FontColor(Colors.Black);
        });
    }

    /// <summary>
    /// Composes the employee information section including name, company, and report date.
    /// </summary>
    /// <param name="container">The <see cref="IContainer"/> used to compose the employee info layout.</param>
    /// <param name="request">The <see cref="TalentMatchReportRequest"/> containing employee and company details.</param>
    private void ComposeEmployeeInfo(IContainer container, TalentMatchSelectionReportRequest request)
    {
        container.Padding(15).Column(infoColumn =>
        {
            infoColumn.Item().Text($"{request.CompanyName} - {request.Position}")
                .FontSize(14).SemiBold().FontColor(Colors.Black);

            infoColumn.Item().PaddingTop(5).Text(request.ReportDate.ToString("dd MMM yyyy"))
                .FontSize(12).FontColor(Colors.Grey.Darken1);

            infoColumn.Item().PaddingTop(15).Text(request.EmployeeName)
                .FontSize(24).SemiBold().FontColor(Colors.Black);
        });
    }

    /// <summary>
    /// Composes the report description section on Page 1.
    /// </summary>
    /// <param name="container">The <see cref="IContainer"/> used to compose the description layout.</param>
    /// <param name="request">The <see cref="TalentMatchReportRequest"/> containing the report description text.</param>
    private void ComposeDescription(IContainer container, TalentMatchSelectionReportRequest request)
    {
        container.Column(descColumn =>
        {
            // Static text with interpolation
            descColumn.Item().Text(
                $"This report provides a summary of the competency potential for this candidate when compared to the role of {request.CompanyName} - {request.Position}. " +
                "The competency potential scores are based on the candidate’s responses to the potential assessments described in this report."
            )
            .FontSize(11).LineHeight(1.4f).FontColor(Colors.Black).Justify();
        });
    }



    /// <summary>
    /// Composes the development summary section on Page 1.
    /// </summary>
    /// <param name="container">The <see cref="IContainer"/> used to compose the development summary layout.</param>
    /// <param name="request">The <see cref="TalentMatchReportRequest"/> containing the development summary text.</param>
    private void ComposeDevelopmentSummary(IContainer container, TalentMatchSelectionReportRequest request)
    {
        container.Column(summaryColumn =>
        {
            summaryColumn.Item().Text(request.Page1.DevelopmentSummary)
                .FontSize(11).LineHeight(1.4f).FontColor(Colors.Black).Justify();
        });
    }

    /// <summary>
    /// Composes the usage instructions section on Page 1 of the report.
    /// </summary>
    /// <param name="container">The <see cref="IContainer"/> used to compose the instructions layout.</param>
    /// <param name="request">The <see cref="TalentMatchReportRequest"/> containing the usage instructions text.</param>
    private void ComposeUsageInstructions(IContainer container, TalentMatchSelectionReportRequest request)
    {
        container.Column(instructionsColumn =>
        {
            instructionsColumn.Item().Text(request.Page1.UsageInstructions)
                .FontSize(11).LineHeight(1.4f).FontColor(Colors.Black).Justify();
        });
    }


    /// <summary>
    /// Composes the validity period section on Page 1 of the report.
    /// </summary>
    /// <param name="container">The <see cref="IContainer"/> used to compose the validity info layout.</param>
    /// <param name="request">The <see cref="TalentMatchReportRequest"/> containing the validity period text.</param>
    private void ComposeValidityInfo(IContainer container, TalentMatchSelectionReportRequest request)
    {
        container.Text(request.Page1.ValidityPeriod)
            .FontSize(11).LineHeight(1.4f).FontColor(Colors.Black).Justify();
    }

    /// <summary>
    /// Composes the footer information section on Page 1, e.g., confidentiality notice.
    /// </summary>
    /// <param name="container">The <see cref="IContainer"/> used to compose the footer layout.</param>
    /// <param name="request">The <see cref="TalentMatchReportRequest"/> containing footer details.</param>
    private void ComposeFooterInfo(IContainer container, TalentMatchSelectionReportRequest request)
    {
        container.Column(footerColumn =>
        {
            footerColumn.Item().Text("Confidential Information")
                .FontSize(14).SemiBold().FontColor(Colors.Black);
        });
    }

    /// <summary>
    /// Composes the bottom decoration for Page 1, including "Powered By" branding.
    /// </summary>
    /// <param name="container">The <see cref="IContainer"/> used to compose the bottom decoration.</param>
    /// <param name="request">The <see cref="TalentMatchReportRequest"/> containing branding details.</param>
    private void ComposeBottomDecoration(PageDescriptor page, TalentMatchSelectionReportRequest request)
    {
        page.Footer()
            .AlignRight()
            .PaddingRight(20)
            .PaddingBottom(10)
            .Container()
            .Width(300)
            .Height(60)
            .Background(Colors.Grey.Lighten1)
            .Padding(10)
            .AlignCenter()
            .AlignMiddle()
            .Column(col =>
            {
                col.Item().AlignCenter().AlignMiddle().Inlined(stack =>
                {
                    // "POWERED BY" text
                    stack.Item().Text(text =>
                    {
                        text.Span("© POWERED BY ")
                            .FontSize(8).FontColor(Colors.Grey.Darken1);
                    });

                    // Logo image
                    stack.Item().Height(35).Image(Convert.FromBase64String(request.PoweredByLogo));
                });
            });
    }


    #endregion

    #region Page 2 Methods

    /// <summary>
    /// Composes the main content for Page 2 of the Talent Report PDF, including introduction paragraphs and summary quadrants.
    /// </summary>
    /// <param name="container">The <see cref="IContainer"/> used to compose the content layout.</param>
    /// <param name="request">The <see cref="TalentMatchSelectionReportRequest"/> containing Page 2 data.</param>
    private void ComposePage2Content(IContainer container, TalentMatchSelectionReportRequest request)
    {
        container.Column(column =>
        {
            // Title
            column.Item().PaddingBottom(10).Text(request.Page2.PageTitle)
                .FontSize(16).Bold().FontColor(Colors.Black).WrapAnywhere();

            // Introduction paragraphs
            column.Item().PaddingBottom(5).Column(textCol =>
            {
                textCol.Item().PaddingBottom(5).Text(
                     "Effective performance in most roles is dependent on the extent to which an individual’s likely behaviour is aligned with the behavioural requirements that lead to success in a particular role. "
                       )
                     .FontSize(11).LineHeight(1.3f).WrapAnywhere();

                if (!string.IsNullOrEmpty(request.Page2.IntroductionParagraph2))
                    textCol.Item().PaddingBottom(5).Text(request.Page2.IntroductionParagraph2)
                        .FontSize(11).LineHeight(1.3f).WrapAnywhere();
                textCol.Item().PaddingBottom(5).Text(
                    "There are several factors that determine if an individual will be successful in a role. Some are backward looking such as qualification and experience, while others relate to the individual’s current environment including their relationship with a manager and their team. "
                )
                .FontSize(11).LineHeight(1.3f).WrapAnywhere();

                textCol.Item().Text("This report provides a forward looking perspective. ").FontSize(11).LineHeight(1.3f).WrapAnywhere();
            });

            // Summary Profile title
            column.Item().PaddingBottom(2).Text(request.Page2.SummaryProfileTitle)
                .FontSize(14).Bold().FontColor(Colors.Black).WrapAnywhere();

            // ***** NEW TALENT MATCH SECTION *****
            column.Item().PaddingBottom(5).Element(c => ComposeTalentMatchSection(c, request));

            // Divider line with reduced padding
            column.Item().PaddingVertical(1).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

            // --------- FIRST ROW OF QUADRANTS ---------
            column.Item().Row(row =>
            {
                row.RelativeItem(1).Element(c => ComposeQuadrantBox(c,
                    request.Page2.PotentialLimitationsTitle,
                    request.Page2.PotentialLimitationsContent,
                    request.Page2.PotentialLimitationsColor));

                // Reduced spacing between boxes
                row.AutoItem()
                   .PaddingHorizontal(5)
                   .LineVertical(1)
                   .LineDashPattern(new float[] { 4f, 4f })
                   .LineColor(Colors.Grey.Lighten2);

                row.RelativeItem(1).Element(c => ComposeQuadrantBox(c,
                    request.Page2.KeyStrengthsTitle,
                    request.Page2.KeyStrengthsContent,
                    request.Page2.KeyStrengthsColor));
            });

            // Divider line with reduced padding
            column.Item().PaddingVertical(4).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

            // --------- SECOND ROW OF QUADRANTS ---------
            column.Item().Row(row =>
            {
                row.RelativeItem(1).Element(c => ComposeQuadrantBox(c,
                   request.Page2.DevelopmentOpportunitiesTitle,
                   request.Page2.DevelopmentOpportunitiesContent,
                   request.Page2.DevelopmentOpportunitiesColor));

                // Reduced spacing between boxes
                row.AutoItem()
                   .PaddingHorizontal(5)
                   .LineVertical(1)
                   .LineDashPattern(new float[] { 4f, 4f })
                   .LineColor(Colors.Grey.Lighten2);

                row.RelativeItem(1).Element(c => ComposeQuadrantBox(c,
                    request.Page2.GoodPotentialTitle,
                    request.Page2.GoodPotentialContent,
                    request.Page2.GoodPotentialColor));
            });
        });
    }

    /// <summary>
    /// Composes a single quadrant box with a title, list of items, and a background color.
    /// </summary>
    /// <param name="container">The <see cref="IContainer"/> used to compose the quadrant layout.</param>
    /// <param name="title">The title of the quadrant.</param>
    /// <param name="items">The list of items to display inside the quadrant.</param>
    /// <param name="color">The background color for the quadrant header.</param>
    private void ComposeQuadrantBox(IContainer container, string title, List<string> items, string color)
    {
        container.Column(column =>
        {
            // Header with title - centered and consistent height
            column.Item()
                .Height(30) // Fixed height for alignment
                .Background(color)
                .Padding(6)
                .AlignCenter()
                .AlignMiddle()
                .Text(title ?? "")
                .FontSize(12)
                .Bold()
                .FontColor(Colors.White)
                .WrapAnywhere();

            // Content with fixed minimum height for alignment
            column.Item()
                .MinHeight(50) // Ensures all boxes have same minimum height
                .Background(Colors.White)
                .Padding(8)
                .Column(contentCol =>
                {
                    if (items != null && items.Any())
                    {
                        foreach (var item in items.Take(10))
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                contentCol.Item()
                                    .PaddingBottom(2)
                                    .Text(item)
                                    .FontSize(9)
                                    .LineHeight(1.2f)
                                    .WrapAnywhere();
                            }
                        }
                        // Add "..." if more items exist
                        if (items.Count > 10)
                        {
                            contentCol.Item()
                                .Text("...")
                                .FontSize(9)
                                .FontColor(Colors.Grey.Darken1);
                        }
                    }
                    else
                    {
                        // Empty state - centered
                        contentCol.Item()
                            .AlignCenter()
                            .AlignMiddle()
                            .Text("")
                            .FontSize(9)
                            .FontColor(Colors.Grey.Medium);
                    }
                });
        });
    }
    private void ComposeTalentMatchSection(IContainer container, TalentMatchSelectionReportRequest request)
    {
        container
            .Padding(12)
            .Column(column =>
            {
                // ===== GREY HEADER BAR =====
                column.Item()
                    .Background(Colors.Grey.Darken2) // Dark grey for header
                    .PaddingVertical(6)
                    .AlignCenter()
                    .Text("TALENT MATCH")
                    .FontSize(13)
                    .Bold()
                    .FontColor(Colors.White);

                // ===== SCALE + FIT SCORE =====
                column.Item().PaddingVertical(8).Row(row =>
                {
                    // Left section: Labels + Scale
                    row.RelativeItem(7).Column(labelScaleCol =>
                    {
                        // Labels row
                        labelScaleCol.Item().Row(labelRow =>
                        {
                            // Left label
                            labelRow.RelativeItem(2).AlignMiddle().Column(col =>
                            {
                                col.Item().Text("Unlikely").FontSize(9).SemiBold();
                                col.Item().Text("to be successful").FontSize(9);
                            });

                            // Center label
                            labelRow.RelativeItem(2).AlignMiddle().Column(col =>
                            {
                                col.Item().AlignCenter().Text("Some limitations").FontSize(9).SemiBold();
                            });

                            // Right label
                            labelRow.RelativeItem(2).AlignMiddle().Column(col =>
                            {
                                col.Item().AlignRight().Text("Highly likely").FontSize(9).SemiBold();
                                col.Item().AlignRight().Text("to be successful").FontSize(9);
                            });
                        });

                        // SCALE BAR (1–9)
                        labelScaleCol.Item().PaddingTop(4).Row(scaleRow =>
                        {
                            for (int i = 1; i <= 9; i++)
                            {
                                var color = GetScaleColor(i);
                                var fontColor = (i <= 4 || i == 9) ? Colors.White : Colors.Black;

                                scaleRow.RelativeItem()
                                    .MinWidth(22)
                                    .Height(22)
                                    .Background(color)
                                    .Border(0.5f)
                                    .BorderColor(Colors.White).CornerRadius(4)
                                    .AlignMiddle()
                                    .AlignCenter()
                                    .Text(i.ToString())
                                    .FontSize(10)
                                    .FontColor(fontColor)
                                    .Bold();
                            }
                        });
                    });

                    // Dotted vertical line separator
                    row.AutoItem()
                        .PaddingLeft(19) // shift more to the right
                        .PaddingRight(0) //  remove right padding
                        .LineVertical(3)
                        .LineDashPattern(new float[] { 4f, 4f })
                        .LineColor(Colors.Grey.Lighten1);



                    // Right section: FIT SCORE CARD
                    row.RelativeItem(2).AlignMiddle().Column(scoreCol =>
                    {
                        scoreCol.Item().AlignCenter().Text("Fit for this role").FontSize(9);
                        scoreCol.Spacing(8);
                        scoreCol.Item()
                                .AlignCenter()
                                .AlignMiddle()
                                .Background("#dcddde")
                                .Border(0.5f)
                                .BorderColor("#dcddde")
                                .CornerRadius(8)           // rounded corners
                                .MinWidth(50)              // wider box (adjust as needed)
                                .PaddingVertical(8)        // slightly more vertical padding
                                .PaddingHorizontal(16)     // wider horizontal padding
                                .Text(request.Page2.FitForThisRole.ToString("0.0"))
                                .FontSize(13)
                                .Bold();

                    });
                });


                // ===== ROLE DESCRIPTION (BOLD NAME + EMPHASIS) =====
                column.Item().PaddingBottom(10).Text(text =>
                {
                    text.Span(request.EmployeeName).Bold().FontSize(10).FontColor(Colors.Grey.Darken1);
                    text.Span(" is ").FontSize(10).FontColor(Colors.Grey.Darken1);
                    text.Span("more likely").Bold().FontSize(10).FontColor(Colors.Grey.Darken1);
                    text.Span(" to be successful in the following role/job: ").FontSize(10).FontColor(Colors.Grey.Darken1);
                    text.Span($"{request.CompanyName}").Bold().FontSize(10).FontColor(Colors.Grey.Darken1);
                    text.Span($" - {request.Position}.").Bold().FontSize(10).FontColor(Colors.Grey.Darken1);
                });


                // ===== ASSESSMENT TABLE =====
                column.Item().PaddingTop(8).Column(assessCol =>
                {
                    ComposeAssessmentRow(assessCol, "Behavioural Styles", request.Page2.BehaviouralStylesScore, request.Page2.BehaviouralStyles);
                    ComposeAssessmentRow(assessCol, "Abilities and Skills", request.Page2.AbilitiesandSkillsScore, request.Page2.AbilitiesandSkills);
                });
            });
    }

    // HELPER ROW
    private void ComposeAssessmentRow(ColumnDescriptor assessCol, string label, int score, string resultLabel)
    {
        assessCol.Item().PaddingBottom(6).Column(col =>
        {
            // Label row
            col.Item().Row(row =>
            {
                row.RelativeItem(2)
                    .AlignLeft()
                    .PaddingTop(12)    // shift label slightly downward
                    .Text(label)
                    .FontSize(10)
                    .Bold();

                // Wording row above the scale
                row.RelativeItem(6).Column(wordingCol =>
                {
                    wordingCol.Item().Row(wordingRow =>
                    {
                        wordingRow.RelativeItem(3).AlignLeft().Text("Low").FontSize(9).SemiBold();
                        wordingRow.RelativeItem(3).AlignCenter().Text("Moderate").FontSize(9).SemiBold();
                        wordingRow.RelativeItem(3).AlignRight().Text("High").FontSize(9).SemiBold();
                    });

                    // Scale row (1-9)
                    wordingCol.Item().Row(scoreRow =>
                    {
                        for (int i = 1; i <= 9; i++)
                        {
                            var isSelected = i == score;
                            var boxColor = isSelected ? GetScaleColor(i) : "#dcddde";
                            var textColor = isSelected ? Colors.Black : Colors.Transparent;

                            scoreRow.RelativeItem()
                                .MinWidth(20)
                                .Height(20)
                                .Background(boxColor)
                                .Border(0.5f)
                                .BorderColor(Colors.White)
                                .AlignMiddle()
                                .AlignCenter()
                                .Text(isSelected ? i.ToString() : string.Empty)
                                .FontSize(9)
                                .Bold()
                                .FontColor(textColor);
                        }
                    });
                });

                // Dotted vertical line separator
                row.AutoItem()
                   .PaddingHorizontal(15)
                   .LineVertical(3)
                   .LineDashPattern(new float[] { 4f, 4f })
                   .LineColor(Colors.Grey.Lighten1);

                // Result box (right side)
                row.RelativeItem(2)
                    .AlignMiddle()
                    .AlignCenter()
                    .Background(resultLabel.Contains("High") ? Colors.Yellow.Lighten2 : Colors.Yellow.Lighten3)
                    .PaddingVertical(4)
                    .PaddingHorizontal(8)
                    .Text(resultLabel)
                    .FontSize(9)
                    .Bold();
            });
        });
    }



    // Helper method for scale colors
    private string GetScaleColor(int value)
    {
        return value switch
        {
            1 => "#e21f27",
            2 => "#ea1f4a",
            3 => "#f15a2b",
            4 => "#f29725",
            5 => "#f4d523",
            6 => "#f4d523",
            7 => "#dbe141",
            8 => "#d7d624",
            9 => "#d7b224",
            _ => Colors.White
        };
    }


    #endregion

    #region Page 3 Methods    

    /// <summary>
    /// Composes the main content for Page 3 of the Talent Report PDF, including detailed profile sections for behaviours and skills.
    /// </summary>
    /// <param name="container">The <see cref="IContainer"/> used to compose the content layout.</param>
    /// <param name="request">The <see cref="TalentMatchSelectionReportRequest"/> containing Page 3 data.</param>
    private void ComposePage3Content(IContainer container, TalentMatchSelectionReportRequest request)
    {
        container.Column(column =>
        {
            // Title
            column.Item().PaddingBottom(6).Text("Detailed Profile") // Further reduced padding
                .FontSize(14).Bold().FontColor(Colors.Black); // Further reduced font size

            // Description paragraph
            column.Item().PaddingBottom(8).Text(text => // Further reduced padding
            {
                text.Span("This profile provides a summary of your preferences and capabilities compared against the essential and important work-related behaviours and abilities required for success in the role of ")
                    .FontSize(8).LineHeight(1.2f).FontColor(Colors.Black); // Further reduced font size and line height

                text.Span($"{request.CompanyName}").Bold().FontColor(Colors.Black).FontSize(8);
                text.Span(" - ").FontSize(8).FontColor(Colors.Black);
                text.Span($"{request.Position}").Bold().FontColor(Colors.Black).FontSize(8);
                text.Span(".").FontSize(8).FontColor(Colors.Black);
            });

            // Show Low/Moderate/High labels above tables
            column.Item().PaddingBottom(6).Row(topLabelRow => // Further reduced padding
            {
                topLabelRow.ConstantItem(240); // Slightly reduced

                topLabelRow.RelativeItem().Row(labelRow =>
                {
                    labelRow.RelativeItem().AlignLeft()
                        .Text("Low").FontSize(7).FontColor(Colors.Black).Bold(); // Further reduced

                    labelRow.RelativeItem().AlignCenter()
                        .Text("Moderate").FontSize(7).FontColor(Colors.Black).Bold();

                    labelRow.RelativeItem().AlignRight()
                        .Text("High").FontSize(7).FontColor(Colors.Black).Bold();
                });
            });

            // Essential behaviours section
            if (request.Page3.EssentialBehaviours?.Any() == true)
            {
                column.Item().PaddingBottom(3).Element(c => ComposeProfileSection(c, "Essential behaviours", // Further reduced padding
                    request.Page3.EssentialBehaviours, "#8C919D"));
            }

            // Important behaviours section  
            if (request.Page3.ImportantBehaviours?.Any() == true)
            {
                column.Item().PaddingBottom(3).Element(c => ComposeProfileSection(c, "Important behaviours", // Further reduced padding
                    request.Page3.ImportantBehaviours, "#8C919D"));
            }

            // Essential skills and aptitudes section
            if (request.Page3.EssentialSkills?.Any() == true)
            {
                column.Item().Element(c => ComposeProfileSection(c, "Essential skills and aptitudes",
                    request.Page3.EssentialSkills, "#8C919D"));
            }
        });
    }


    /// <summary>
    /// Composes a profile section table with a title, a list of <see cref="ProfileItem"/>s, and a header color.
    /// </summary>
    /// <param name="container">The <see cref="IContainer"/> used to compose the section layout.</param>
    /// <param name="sectionTitle">The title of the profile section.</param>
    /// <param name="items">The list of <see cref="ProfileItem"/>s to display in the section.</param>
    /// <param name="headerColor">The background color of the section header.</param>
    /// <param name="showScale">Whether to show the numeric scale (default: false).</param>
    private void ComposeProfileSection(IContainer container, string sectionTitle, List<ProfileItem> items, string headerColor, bool showScale = false)
    {
        container.Column(column =>
        {
            // Complete table with horizontal borders only
            column.Item().Column(tableColumn =>
            {
                // Section header with color scale - no vertical borders
                tableColumn.Item().Row(headerRow =>
                {
                    // Section title
                    headerRow.ConstantItem(240).Background(headerColor).CornerRadius(4) // Slightly reduced                    
                        .PaddingHorizontal(10).PaddingVertical(4) // Reduced padding
                        .Text(sectionTitle)
                        .FontSize(12).Bold().FontColor(Colors.White); // Further reduced font size

                    // Color scale legend with numbers - no vertical borders between cells
                    headerRow.RelativeItem().BorderColor(Colors.Black).Row(scaleRow =>
                    {
                        var colors = new[] { "#E31D25", "#EB1C48", "#F15929", "#F59726", "#F3D520", "#F3D520", "#DAE241", "#D7D820", "#8CC63E", "#39B54A" };
                        for (int i = 1; i <= 10; i++)
                        {
                            scaleRow.RelativeItem().PaddingHorizontal(1).Height(20) // Reduced height
                                .Background(colors[i - 1])
                                 .CornerRadius(3)
                                .AlignCenter().AlignMiddle()
                                .Text(i.ToString())
                                .FontSize(9).FontColor(Colors.White).Bold(); // Further reduced font size
                        }
                    });
                });

                // Data rows with horizontal borders only
                foreach (var item in items)
                {
                    tableColumn.Item().BorderBottom(1.2f).BorderColor("#000000").Row(dataRow => // Only bottom border
                    {
                        // Item name - no right border
                        dataRow.ConstantItem(240) // Slightly reduced
                            .Background(Colors.White)
                            .PaddingHorizontal(10).PaddingVertical(4) // Reduced padding
                            .AlignMiddle()
                            .Text(item.Name)
                            .FontSize(8).FontColor(Colors.Black); // Further reduced font size

                        // Score scale area - no vertical borders between cells
                        dataRow.RelativeItem().Background(Colors.White).Row(scoreRow =>
                        {
                            for (int i = 1; i <= 10; i++)
                            {
                                // Define which columns should have grey background
                                var greyColumns = new int[] { 1, 2, 5, 6, 9, 10 };
                                var cellBackgroundColor = greyColumns.Contains(i) ? "#E5E5E5" : "#FFFFFF";

                                scoreRow.RelativeItem().Height(24) // Reduced height
                                    .Background(cellBackgroundColor)
                                    .AlignCenter().AlignMiddle()
                                    .Element(scoreCell =>
                                    {
                                        if (i == item.Score)
                                        {
                                            // Show colored circle at score position with border
                                            scoreCell.Width(12).Height(12) // Reduced size
                                                .Background(GetScoreColor(item.Score))
                                                .Border(1)                      // <-- add border
                                                .BorderColor("#BDBEC0")         // <-- border color
                                                .CornerRadius(6);               // keep it circular
                                        }
                                    });
                            }
                        });

                    });
                }
            });
        });
    }

    /// <summary>
    /// Returns a color corresponding to a score from 1 to 10.
    /// </summary>
    /// <param name="score">The score value (1–10).</param>
    /// <returns>A hex color string representing the score.</returns>
    private string GetScoreColor(int score)
    {
        var colors = new[]
        {
       "#E31D25", "#EB1C48", "#F15929", "#F59726", "#F3D520", "#F3D520", "#DAE241", "#D7D820", "#8CC63E", "#39B54A"
    };
        return colors[Math.Max(0, Math.Min(9, score - 1))];
    }

    #endregion

    #region Page 4 Methods

    private void ComposePage4Content(IContainer container, TalentMatchSelectionReportRequest request)
    {
        container.Padding(15).Column(column =>
        {
            column.Spacing(3);

            // Header Section
            column.Item().Row(row =>
            {
                row.RelativeItem(2).Column(col =>
                {
                    col.Item().Text("How to use this").FontSize(16).FontColor("#999999");
                    col.Item().Text("INTERVIEW           GUIDE").FontSize(16).Bold().FontColor("#666666");
                    col.Item().PaddingTop(2).Text(text =>
                    {
                        text.Span("This interview guide contains competency based questions to guide the interview process. Below is an illustration of the process to follow when using this guide.")
                            .FontSize(10f).FontColor("#666666").LineHeight(1.1f);
                    });
                });

                row.RelativeItem(4).PaddingLeft(8).Column(col =>
                {
                    col.Spacing(2);

                    // Step 1
                    col.Item().Row(r =>
                    {
                        // Circle with icon
                        r.AutoItem().Width(56).Height(56).Border(5).BorderColor("#CCCCCC")
                            .CornerRadius(28).Background("#FFFFFF")
                            .AlignCenter().AlignMiddle()
                            .Container().Width(30).Height(30).AlignCenter().AlignMiddle().Svg(SvgIcons.Briefcase.Replace("{COLOR}", "#E74C3C"));

                        // Number and text in the same row
                        r.RelativeItem().PaddingLeft(5).AlignMiddle().Row(innerRow =>
                        {
                            innerRow.AutoItem().Text("1").FontSize(40).Bold().FontColor("#E74C3C");
                            // Make this RelativeItem so text can wrap
                            innerRow.RelativeItem()
                                .PaddingLeft(10)
                                .AlignMiddle()
                                .Text("PREPARE FOR THE INTERVIEW")
                                .FontSize(15f) // increased size
                                .FontColor("#999999");
                        });
                    });

                    // Arrow shifted to the right, below the icon
                    col.Item().PaddingLeft(20).PaddingTop(-5) // move arrow to right
                        .Container().Width(28).Height(28).AlignCenter().AlignMiddle()
                        .Svg(SvgIcons.Down);


                    // Step 2
                    col.Item().Row(r =>
                    {
                        r.AutoItem().Width(56).Height(56).Border(5).BorderColor("#CCCCCC")
                            .CornerRadius(28).Background("#FFFFFF")
                            .AlignCenter().AlignMiddle()
                            .Container().Width(30).Height(30).AlignCenter().AlignMiddle().Svg(SvgIcons.QuestionMark.Replace("{COLOR}", "#E74C3C"));

                        // Number and text in the same row
                        r.RelativeItem().PaddingLeft(5).AlignMiddle().Row(innerRow =>
                        {
                            innerRow.AutoItem().Text("2").FontSize(40).Bold().FontColor("#E74C3C");
                            innerRow.RelativeItem().AlignMiddle().PaddingLeft(12).Text("SELECT QUESTIONS FROM THOSE\nPRESENTED IN EACH CATEGORY").FontSize(14f).FontColor("#999999");
                        });
                    });

                    // Arrow shifted to the right, below the icon
                    col.Item().PaddingLeft(20).PaddingTop(-5) // move arrow to right
                        .Container().Width(28).Height(28).AlignCenter().AlignMiddle()
                        .Svg(SvgIcons.Down);
                });
            });

            // Main Content
            column.Item().Row(row =>
            {
                // Left Column
                row.RelativeItem(2).Column(leftCol =>
                {
                    // How to Ask Good Questions
                    leftCol.Item().PaddingTop(-40).Column(col =>
                    {
                        // Header text
                        col.Item().Text(t =>
                        {
                            t.Span("HOW TO ASK ").FontSize(14f).FontColor("#999999");
                            t.Span(" "); // small spacing
                            t.Span("GOOD INTERVIEWING QUESTIONS").FontSize(14f).Bold().FontColor("#666666");
                        });

                        // Bullet points
                        col.Item().PaddingTop(2).Column(bulletCol =>
                        {
                            bulletCol.Spacing(1.5f);
                            AddBulletPoint(bulletCol, "Ask open-ended questions (how?, why?, describe, tell me about).");
                            AddBulletPoint(bulletCol, "Ask for practical, real-life examples.");
                            AddBulletPoint(bulletCol, "Always probe to discover more information and do not let the interview structure limit your probing.");
                            AddBulletPoint(bulletCol, "Avoid asking improper questions by focusing on the inherent job requirements.");
                            AddBulletPoint(bulletCol, "If the candidate strays off the subject, redirect as quickly as possible.");
                            AddBulletPoint(bulletCol, "Paraphrase the candidate's answer to show that you have listened.");
                        });
                    });


                    // Points to Remember
                    leftCol.Item().PaddingTop(15).Text(text =>
                    {
                        text.Span("POINTS TO ").FontSize(14f).FontColor("#999999");
                        text.Span("REMEMBER").FontSize(14f).Bold().FontColor("#666666");
                    });

                    leftCol.Item().PaddingTop(2).Column(bulletCol =>
                    {
                        bulletCol.Spacing(1.5f);
                        AddBulletPoint(bulletCol, "Do not rush to a decision or judgement.");
                        AddBulletPoint(bulletCol, "Do not make binding contractual statements during the interview.");
                        AddBulletPoint(bulletCol, "Remain professional but open and welcoming.");
                        AddBulletPoint(bulletCol, "\"Hire for fit, train for skills\"");
                    });

                    // Interview Score Legend
                    leftCol.Item().PaddingTop(50).Column(scoreSection =>
                    {
                        // Row for number + dots
                        scoreSection.Item().Row(row =>
                        {
                            // Number "6" in yellowish color
                            row.AutoItem().Text("6")
                                .FontSize(35f)
                                .Bold()
                                .FontColor("#dae146");  // yellow-orange shade

                            // Grey dots
                            row.AutoItem().Text(" ● ● ● ● ● ● ● ● ● ● ● ● ●")
                                .FontSize(12f)
                                .FontColor("#999999"); // grey shade
                        });

                        // Subtitle
                        scoreSection.Item()
                                    .PaddingTop(-20)   // or TranslateY(-10)
                                    .PaddingLeft(-15)
                                    .AlignCenter()
                                    .Text("INTERVIEW SCORE")
                                    .FontSize(12f)
                                    .Bold()
                                    .FontColor("#666666");

                        // Legend
                        scoreSection.Item().PaddingTop(5).Column(scoreCol =>
                        {
                            scoreCol.Spacing(2f);
                            AddScoreLegend(scoreCol, "#E74C3C", "Applicant did not meet expectations", 1);
                            AddScoreLegend(scoreCol, "#F39C12", "Applicant requires additional competency", 2);
                            AddScoreLegend(scoreCol, "#F4D03F", "Applicant met expectations", 3);
                            AddScoreLegend(scoreCol, "#52BE80", "Applicant exceeded expectations", 4);
                            AddScoreLegend(scoreCol, "#27AE60", "Applicant is an excellent candidate for acceptance", 5);
                        });

                    });

                });

                // Right Column (Steps 3–8)
                row.RelativeItem(4).PaddingLeft(10).Column(rightCol =>
                {
                    rightCol.Spacing(3);

                    // Step 3
                    rightCol.Item().Row(r =>
                    {
                        // Circle with icon
                        r.AutoItem()
                            .Width(56)
                            .Height(56)
                            .Border(5)
                            .BorderColor("#CCCCCC")
                            .CornerRadius(28)
                            .Background("#FFFFFF")
                            .AlignCenter()
                            .AlignMiddle()
                            .Container()
                                .Width(30)
                                .Height(30)
                                .AlignCenter()
                                .AlignMiddle()
                              .Svg(SvgIcons.Users.Replace("{COLOR}", "#f1592a"));// inject dynamic color


                        // Number and text in the same row
                        r.RelativeItem().PaddingLeft(5).AlignMiddle().Row(innerRow =>
                        {
                            innerRow.AutoItem().Text("3").FontSize(40).Bold().FontColor("#f1592a");
                            innerRow.RelativeItem().AlignMiddle().PaddingLeft(12).Text("CONDUCT THE INTERVIEW").FontSize(14f).FontColor("#999999");
                        });
                    });

                    // Arrow shifted to the right, below the icon
                    rightCol.Item().PaddingLeft(20).PaddingTop(-5) // move arrow to right
                        .Container().Width(28).Height(28).AlignCenter().AlignMiddle()
                        .Svg(SvgIcons.Down);

                    // Step 4
                    rightCol.Item().Row(r =>
                    {
                        // Circle with icon
                        r.AutoItem().Width(56).Height(56).Border(5).BorderColor("#CCCCCC")
                            .CornerRadius(28).Background("#FFFFFF")
                            .AlignCenter().AlignMiddle()
                            .Container().Width(30).Height(30).AlignCenter().AlignMiddle().Svg(SvgIcons.ThinkIcon.Replace("{COLOR}", "#f59726"));

                        // Number and text in the same row
                        r.RelativeItem().PaddingLeft(5).AlignMiddle().Row(innerRow =>
                        {
                            innerRow.AutoItem().Text("4").FontSize(40).Bold().FontColor("#f59726");
                            innerRow.RelativeItem().AlignMiddle().PaddingLeft(12).Text("RECORD THE RESULTS").FontSize(14f).FontColor("#999999");
                        });
                    });

                    // Arrow shifted to the right, below the icon
                    rightCol.Item().PaddingLeft(20).PaddingTop(-5) // move arrow to right
                        .Container().Width(28).Height(28).AlignCenter().AlignMiddle()
                        .Svg(SvgIcons.Down);

                    // Step 5
                    rightCol.Item().Row(r =>
                    {
                        // Circle with icon
                        r.AutoItem().Width(56).Height(56).Border(5).BorderColor("#CCCCCC")
                            .CornerRadius(28).Background("#FFFFFF")
                            .AlignCenter().AlignMiddle()
                            .Container().Width(30).Height(30).AlignCenter().AlignMiddle().Svg(SvgIcons.Search.Replace("{COLOR}", "#edd623"));

                        // Number + text title
                        r.RelativeItem().PaddingLeft(6).AlignMiddle().Row(innerRow =>
                        {
                            innerRow.AutoItem().Text("5").FontSize(40).Bold().FontColor("#edd623");
                            innerRow.RelativeItem().AlignMiddle().PaddingLeft(12).Text("SCORE AND EVALUATE THE DATA")
                                .FontSize(14f).FontColor("#999999");
                        });
                    });

                    // Bullets below with spacing
                    rightCol.Item().PaddingLeft(90).PaddingTop(-8).Column(c =>
                    {
                        c.Spacing(3f);
                        AddOrangeBullet(c, "For each consistency you can rate the candidate on a 5-point scale by using the interview rating scores presented in the next step.");
                        AddOrangeBullet(c, "Ensure a consistent interpretation of the rating score.");
                        AddOrangeBullet(c, "Openly debate with the panel to obtain a rating decision.");
                        AddOrangeBullet(c, "Always compare candidates to the critical job requirements and not to other applicants to limit any subjectivity and bias.");
                    });


                    // Arrow shifted to the right, below the icon
                    rightCol.Item().PaddingLeft(20).PaddingTop(-115) // move arrow to right
                        .Container().Width(28).Height(28).AlignCenter().AlignMiddle()
                        .Svg(SvgIcons.Down);

                    // Step 6
                    rightCol.Item().Row(r =>
                    {
                        // Circle with icon
                        r.AutoItem().Width(56).Height(56).Border(5).BorderColor("#CCCCCC")
                            .CornerRadius(28).Background("#FFFFFF")
                            .AlignCenter().AlignMiddle()
                            .Container().Width(30).Height(30).AlignCenter().AlignMiddle().Svg(SvgIcons.Percentage.Replace("{COLOR}", "#dae146"));
                    });

                    // Arrow shifted to the right, below the icon
                    rightCol.Item().PaddingLeft(20).PaddingTop(-5) // move arrow to right
                        .Container().Width(28).Height(28).AlignCenter().AlignMiddle()
                        .Svg(SvgIcons.Down);

                    // Step 7
                    rightCol.Item().Row(r =>
                    {
                        // Circle with icon
                        r.AutoItem().Width(56).Height(56).Border(5).BorderColor("#CCCCCC")
                            .CornerRadius(28).Background("#FFFFFF")
                            .AlignCenter().AlignMiddle()
                            .Container().Width(30).Height(30).AlignCenter().AlignMiddle().Svg(SvgIcons.Lines.Replace("{COLOR}", "#d0d727"));

                        // Number + text title
                        r.RelativeItem().PaddingLeft(6).AlignMiddle().Row(innerRow =>
                        {
                            innerRow.AutoItem().Text("7").FontSize(40).Bold().FontColor("#d0d727");
                            innerRow.RelativeItem().AlignMiddle().PaddingLeft(12).Text("RECORD THE RESULTS").FontSize(14f).FontColor("#999999");
                        });
                    });

                    // Arrow shifted to the right, below the icon
                    rightCol.Item().PaddingLeft(20).PaddingTop(-5) // move arrow to right
                        .Container().Width(28).Height(28).AlignCenter().AlignMiddle()
                        .Svg(SvgIcons.Down);

                    // Step 8
                    rightCol.Item().Row(r =>
                    {
                        // Circle with icon
                        r.AutoItem().Width(56).Height(56).Border(5).BorderColor("#CCCCCC")
                            .CornerRadius(28).Background("#FFFFFF")
                            .AlignCenter().AlignMiddle()
                            .Container().Width(30).Height(30).AlignCenter().AlignMiddle().Svg(SvgIcons.Star.Replace("{COLOR}", "#8bc73c"));
                        // Number + text title
                        r.RelativeItem().PaddingLeft(6).AlignMiddle().Row(innerRow =>
                        {
                            innerRow.AutoItem().Text("8").FontSize(40).Bold().FontColor("#8bc73c");
                            innerRow.RelativeItem().AlignMiddle().PaddingLeft(12).Text("MAKE RECOMMENDATION").FontSize(14f).FontColor("#999999");
                        });
                    });
                });
            });
        });
    }

    private void AddBulletPoint(ColumnDescriptor column, string text)
    {
        column.Item().Row(row =>
        {
            row.ConstantItem(8).PaddingTop(3).AlignLeft().Width(6).Height(6).Background("#CCCCCC");
            row.RelativeItem().PaddingLeft(4).Text(text).FontSize(8f).FontColor("#666666").LineHeight(1.2f);
        });
    }

    private void AddScoreLegend(ColumnDescriptor column, string color, string text, int number)
    {
        column.Item().Row(row =>
        {
            // Rounded circle with number inside
            row.ConstantItem(16).AlignMiddle().Width(12).Height(12)
                .CornerRadius(6) // fully rounded
                .Background(color)
                .AlignCenter()
                .AlignMiddle()
                .Text(number.ToString())
                .FontSize(8f)
                .Bold()
                .FontColor("#FFFFFF"); // white number inside colored circle

            // Text
            row.RelativeItem().PaddingLeft(5).AlignMiddle()
                .Text(text)
                .FontSize(9f)
                .FontColor("#666666");
        });
    }


    private void AddOrangeBullet(ColumnDescriptor column, string text)
    {
        column.Item().Row(row =>
        {
            row.ConstantItem(8).PaddingTop(2).AlignLeft().Width(7).Height(7).Background("#F39C12");
            row.RelativeItem().PaddingLeft(4).Text(text).FontSize(10f).FontColor("#666666").LineHeight(1.2f);
        });
    }

    /// <summary>
    /// Composes the main content for Page 4 of the Talent Report PDF, including full behavioural styles sections.
    /// </summary>
    /// <param name="container">The <see cref="IContainer"/> used to compose the content layout.</param>
    /// <param name="request">The <see cref="TalentMatchReportRequest"/> containing Page 4 data.</param>
    //private void ComposePage4Content(IContainer container, TalentMatchReportRequest request)
    //{
    //    container.Column(column =>
    //    {
    //        // Title
    //        column.Item().PaddingBottom(8).Text("Full Behavioural Styles Profile")
    //            .FontSize(16).Bold().FontColor(Colors.Black);

    //        // Description paragraph
    //        column.Item().PaddingBottom(10).Text(text =>
    //        {
    //            text.Span("This profile gives a detailed view of your greater and lesser preferences to execute different work-related behaviours that will influence your potential to be successful in your role. These preferences are based on your desires or motives to execute these behaviours, as well as behaviours you perceive as personal strengths.")
    //                .FontSize(9).LineHeight(1.3f).FontColor(Colors.Black);
    //        });

    //        // Show Low/Moderate/High labels above all sections
    //        column.Item().PaddingBottom(8).Row(topLabelRow =>
    //        {
    //            topLabelRow.ConstantItem(280);
    //            topLabelRow.RelativeItem().Row(labelRow =>
    //            {
    //                labelRow.RelativeItem().AlignLeft()
    //                    .Text("Low").FontSize(8).FontColor(Colors.Black).Bold();
    //                labelRow.RelativeItem().AlignCenter()
    //                    .Text("Moderate").FontSize(8).FontColor(Colors.Black).Bold();
    //                labelRow.RelativeItem().AlignRight()
    //                    .Text("High").FontSize(8).FontColor(Colors.Black).Bold();
    //            });
    //        });

    //        // Problem Solving section
    //        if (request.Page4.ProblemSolving?.Any() == true)
    //        {
    //            column.Item().PaddingBottom(4).Element(c => ComposeBehaviouralSection(c, "Problem Solving",
    //                request.Page4.ProblemSolving, "#8C919D"));
    //        }

    //        // Influencing People section  
    //        if (request.Page4.InfluencingPeople?.Any() == true)
    //        {
    //            column.Item().PaddingBottom(4).Element(c => ComposeBehaviouralSection(c, "Influencing People",
    //                request.Page4.InfluencingPeople, "#8C919D"));
    //        }

    //        // Adapting Approaches section
    //        if (request.Page4.AdaptingApproaches?.Any() == true)
    //        {
    //            column.Item().PaddingBottom(4).Element(c => ComposeBehaviouralSection(c, "Adapting Approaches",
    //                request.Page4.AdaptingApproaches, "#8C919D"));
    //        }

    //        // Delivering Success section
    //        if (request.Page4.DeliveringSuccess?.Any() == true)
    //        {
    //            column.Item().Element(c => ComposeBehaviouralSection(c, "Delivering Success",
    //                request.Page4.DeliveringSuccess, "#8C919D"));
    //        }
    //    });
    //}

    /// <summary>
    /// Composes a behavioural section table with a title, a list of <see cref="BehaviouralItem"/>s, and a header color.
    /// </summary>
    /// <param name="container">The <see cref="IContainer"/> used to compose the section layout.</param>
    /// <param name="sectionTitle">The title of the behavioural section.</param>
    /// <param name="items">The list of <see cref="BehaviouralItem"/>s to display in the section.</param>
    /// <param name="sectionColor">The background color for the section header.</param>
    //private void ComposeBehaviouralSection(IContainer container, string sectionTitle, List<BehaviouralItem> items, string sectionColor)
    //{
    //    container.Column(column =>
    //    {
    //        // Section with border
    //        column.Item().Column(sectionColumn =>
    //        {
    //            // Section header spanning full width
    //            sectionColumn.Item().Row(headerRow =>
    //            {
    //                // Section title - takes partial width
    //                headerRow.ConstantItem(240).Background(sectionColor).CornerRadius(4)
    //                    .PaddingHorizontal(12).PaddingVertical(3)
    //                    .AlignMiddle()
    //                    .Text(sectionTitle)
    //                    .FontSize(12).Bold().FontColor(Colors.White);

    //                // Scale numbers 1-10 - fills remaining width
    //                headerRow.RelativeItem().Background(Colors.White).Row(scaleRow =>
    //                {
    //                    for (int i = 1; i <= 10; i++)
    //                    {
    //                        var cellColor = GetScaleBackgroundColor(i);
    //                        scaleRow.RelativeItem().Height(20)
    //                            .Background(cellColor)
    //                            .BorderLeft(1).BorderColor(Colors.White).CornerRadius(4)
    //                            .AlignCenter().AlignMiddle()
    //                            .Text(i.ToString())
    //                            .FontSize(10).FontColor(Colors.White).Bold();
    //                    }
    //                });
    //            });

    //            // Behavior items
    //            foreach (var item in items)
    //            {
    //                sectionColumn.Item().BorderTop(1).BorderColor(Colors.Grey.Lighten2).Row(itemRow =>
    //                {
    //                    // Behavior name and sub-behaviors - same width as section title
    //                    itemRow.ConstantItem(240).Background(Colors.White)
    //                        .PaddingHorizontal(2).PaddingVertical(3)
    //                        .Column(nameColumn =>
    //                        {
    //                            // Main behavior name
    //                            nameColumn.Item().Text(item.Name)
    //                                .FontSize(10).Bold().FontColor(Colors.Black);

    //                            // Sub-behaviors if any
    //                            if (item.SubBehaviors?.Any() == true)
    //                            {
    //                                nameColumn.Item().Text(text =>
    //                                {
    //                                    text.Span(string.Join(", ", item.SubBehaviors.Select(sb => $"{sb.Name} ({sb.Score})")))
    //                                        .FontSize(8).FontColor(Colors.Grey.Darken1);
    //                                });
    //                            }
    //                        });

    //                    // Score visualization - matches scale header width exactly
    //                    itemRow.RelativeItem().Background(Colors.Grey.Lighten3).Row(scoreRow =>
    //                    {
    //                        for (int i = 1; i <= 10; i++)
    //                        {
    //                            scoreRow.RelativeItem().Height(22)
    //                                .Background(Colors.Grey.Lighten3)
    //                                .BorderLeft(1).BorderColor(Colors.White)
    //                                .AlignCenter().AlignMiddle()
    //                                .Element(scoreCell =>
    //                                {
    //                                    if (i == item.Score)
    //                                    {
    //                                        // Show filled circle at score position
    //                                        scoreCell.Width(14).Height(14)
    //                                            .Background(GetBehaviorScoreColor(item.Score))
    //                                            .Border(2).BorderColor("#BDBEC0")
    //                                            .CornerRadius(7);
    //                                    }
    //                                });
    //                        }
    //                    });
    //                });
    //            }
    //        });
    //    });
    //}

    /// <summary>
    /// Returns a color corresponding to a scale position from 1 to 10 for behavioural section headers.
    /// </summary>
    /// <param name="position">The position on the scale (1–10).</param>
    /// <returns>A hex color string for the scale cell background.</returns>
    //private string GetScaleBackgroundColor(int position)
    //{
    //    // Color gradient for scale background (1-10)
    //    var colors = new[]
    //    {
    //    "#DC2626", "#EA580C", "#D97706", "#CA8A04", "#EAB308",
    //    "#65A30D", "#16A34A", "#059669", "#0D9488", "#0891B2"
    //};
    //    return colors[Math.Max(0, Math.Min(9, position - 1))];
    //}

    /// <summary>
    /// Returns a color corresponding to a score for the behaviour score indicator circle.
    /// </summary>
    /// <param name="score">The score value (1–10).</param>
    /// <returns>A hex color string representing the score indicator color.</returns>
    //private string GetBehaviorScoreColor(int score)
    //{
    //    // Color for the score indicator circle
    //    var colors = new[]
    //    {
    //    "#E31D25", "#EB1C48", "#F15929", "#F59726", "#F3D520", "#F3D520", "#DAE241", "#D7D820", "#8CC63E", "#39B54A"
    //};
    //    return colors[Math.Max(0, Math.Min(9, score - 1))];
    //}

    #endregion

    #region Page 5 Methods
    /// <summary>
    /// Composes the main content for Page 5 of the Talent Report PDF, which includes the "About" section,
    /// details on the success profile, assessment methods, scores, color-coded performance boxes,
    /// percentile information, and guidance on interpreting the report.
    /// </summary>
    /// <param name="container">The <see cref="IContainer"/> used to structure and render the page content.</param>
    /// <param name="request">The <see cref="TalentMatchReportRequest"/> containing Page 5 data and related content.</param>
    private void ComposePage5Content(IContainer container, TalentMatchSelectionReportRequest request)
    {
        container.Column(column =>
        {
            column.Spacing(15);

            // Header
            column.Item().Text("Interview Questions")
                .FontSize(16)
                .Bold()
                .FontColor(Colors.Black);

            ComposeInterviewQuestionsSection(column, request.Page5.InterviewSection, "Interview Rating");
        });
    }
    #endregion

    #region Page 6 Methods

    /// <summary>
    /// Composes the main content for Page 6 of the Talent Report PDF, which includes technical information,
    /// job/role data, assessment methods, and input data sections.
    /// </summary>
    /// <param name="container">The <see cref="IContainer"/> used to structure and render the page content.</param>
    /// <param name="request">The <see cref="TalentMatchReportRequest"/> containing Page 6 data and related content.</param>
    private void ComposePage6Content(IContainer container, TalentMatchSelectionReportRequest request)
    {
        container.Column(column =>
        {
            column.Spacing(15);
            ComposeInterviewQuestionsSection(column, request.Page6.InterviewSection, "Interview Rating");
        });
    }

    #endregion

    #region Page 7 Methods

    /// <summary>
    /// Composes the main content for Page 7 of the Talent Report PDF, which includes technical information,
    /// job/role data, assessment methods, and input data sections.
    /// </summary>
    /// <param name="container">The <see cref="IContainer"/> used to structure and render the page content.</param>
    /// <param name="request">The <see cref="TalentMatchReportRequest"/> containing Page 7 data and related content.</param>
    private void ComposePage7Content(IContainer container, TalentMatchSelectionReportRequest request)
    {
        container.Column(column =>
        {
            column.Spacing(15);
            ComposeInterviewQuestionsSection(column, request.Page7.InterviewSection, "Interview Rating");
        });
    }

    #endregion

    #region Page 8 Methods

    /// <summary>
    /// Composes the main content for Page 8 of the Talent Report PDF, which includes technical information,
    /// job/role data, assessment methods, and input data sections.
    /// </summary>
    /// <param name="container">The <see cref="IContainer"/> used to structure and render the page content.</param>
    /// <param name="request">The <see cref="TalentMatchReportRequest"/> containing Page 8 data and related content.</param>
    private void ComposePage8Content(IContainer container, TalentMatchSelectionReportRequest request)
    {
        container.Column(column =>
        {
            column.Spacing(15);
            ComposeInterviewQuestionsSection(column, request.Page8.InterviewSection, "Interview Rating");
        });
    }

    #endregion

    #region Page 9 Methods

    /// <summary>
    /// Composes the main content for Page 9 of the Talent Report PDF, which includes technical information,
    /// job/role data, assessment methods, and input data sections.
    /// </summary>
    /// <param name="container">The <see cref="IContainer"/> used to structure and render the page content.</param>
    /// <param name="request">The <see cref="TalentMatchReportRequest"/> containing Page 9 data and related content.</param>
    private void ComposePage9Content(IContainer container, TalentMatchSelectionReportRequest request)
    {
        container.Column(column =>
        {
            column.Spacing(15);
            ComposeInterviewQuestionsSection(column, request.Page9.InterviewSection, "Interview Rating");
        });
    }

    #endregion

    #region Page 10 Methods

    /// <summary>
    /// Composes the main content for Page 10 of the Talent Report PDF, which includes technical information,
    /// job/role data, assessment methods, and input data sections.
    /// </summary>
    /// <param name="container">The <see cref="IContainer"/> used to structure and render the page content.</param>
    /// <param name="request">The <see cref="TalentMatchReportRequest"/> containing Page 10 data and related content.</param>
    private void ComposePage10Content(IContainer container, TalentMatchSelectionReportRequest request)
    {
        container.Column(column =>
        {
            column.Spacing(15);
            ComposeInterviewQuestionsSection(column, request.Page10.InterviewSection, "Interview Rating");
        });
    }

    #endregion

    #region Page 11 Methods

    /// <summary>
    /// Composes the main content for Page 11 of the Talent Report PDF, which includes technical information,
    /// job/role data, assessment methods, and input data sections.
    /// </summary>
    /// <param name="container">The <see cref="IContainer"/> used to structure and render the page content.</param>
    /// <param name="request">The <see cref="TalentMatchReportRequest"/> containing Page 11 data and related content.</param>
    private void ComposePage11Content(IContainer container, TalentMatchSelectionReportRequest request)
    {
        container.Column(column =>
        {
            column.Spacing(15);
            ComposeInterviewQuestionsSection(column, request.Page11.InterviewSection, "Interview Rating");
        });
    }

    #endregion

    #region Page 12 Methods

    /// <summary>
    /// Composes the main content for Page 12 of the Talent Report PDF, which includes technical information,
    /// job/role data, assessment methods, and input data sections.
    /// </summary>
    /// <param name="container">The <see cref="IContainer"/> used to structure and render the page content.</param>
    /// <param name="request">The <see cref="TalentMatchReportRequest"/> containing Page 12 data and related content.</param>
    private void ComposePage12Content(IContainer container, TalentMatchSelectionReportRequest request)
    {
        container.Column(column =>
        {
            column.Spacing(15);
            ComposeInterviewQuestionsSection(column, request.Page12.InterviewSection, "Interview Rating");
        });
    }

    #endregion

    #region Page 13 Methods

    /// <summary>
    /// Composes the main content for Page 13 of the Talent Report PDF, which includes technical information,
    /// job/role data, assessment methods, and input data sections.
    /// </summary>
    /// <param name="container">The <see cref="IContainer"/> used to structure and render the page content.</param>
    /// <param name="request">The <see cref="TalentMatchReportRequest"/> containing Page 13 data and related content.</param>
    private void ComposePage13Content(IContainer container, TalentMatchSelectionReportRequest request)
    {
        container.Column(column =>
        {
            column.Spacing(15);
            ComposeInterviewQuestionsSection(column, request.Page13.InterviewSection, "Interview Rating");
        });
    }

    #endregion

    #region Page 14 Methods

    /// <summary>
    /// Composes the main content for Page 14 of the Talent Report PDF, which includes technical information,
    /// job/role data, assessment methods, and input data sections.
    /// </summary>
    /// <param name="container">The <see cref="IContainer"/> used to structure and render the page content.</param>
    /// <param name="request">The <see cref="TalentMatchReportRequest"/> containing Page 14 data and related content.</param>
    private void ComposePage14Content(IContainer container, TalentMatchSelectionReportRequest request)
    {
        container.Column(column =>
        {
            column.Spacing(15);

            // Title
            column.Item().Text("Interview Ratings Summary")
                .FontSize(18)
                .Bold()
                .FontColor("#666666");

            // Details Section
            column.Item().Element(ComposeDetailsSection);

            // Summary of Interview Scores Section
            column.Item().Element(ComposeInterviewScoresSection);

            // Commentary Section
            column.Item().Element(ComposeCommentarySection);

            // Recommendation Section
            column.Item().Element(ComposeRecommendationSection);

            // Final Selection Recommendation
            column.Item().Element(ComposeFinalSelectionSection);
        });
    }

    private void ComposeDetailsSection(IContainer container)
    {
        container.Column(column =>
        {
            // Header
            column.Item().CornerRadius(5).Background("#A9A9A9").Padding(4).Text("Details")
                .FontSize(11)
                .Bold()
                .FontColor("#FFFFFF");

            // Details Grid
            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(1);  // Labels
                    columns.ConstantColumn(1);  // Separator line column
                    columns.RelativeColumn(2);  // Values
                });

                void AddRow(string label, string value = "")
                {
                    // Left text (label)
                    table.Cell().BorderBottom(1).BorderColor("#CCCCCC").Padding(5)
                        .Text(label).FontSize(9).FontColor("#666666");

                    // Vertical dotted separator
                    table.Cell().BorderBottom(1).BorderColor("#CCCCCC").PaddingVertical(2)
                        .Element(e => e.LineVertical(1)
                                  .LineColor("#666666")   // darker grey
                            .LineDashPattern(new float[] { 2, 2 }, Unit.Point));  // dotted style


                    // Right text (value)
                    table.Cell().BorderBottom(1).BorderColor("#CCCCCC").Padding(5)
                        .Text(value).FontSize(9);
                }

                AddRow("Interviewer Name:");
                AddRow("Interview date:");
                AddRow("Role applied for:");
                AddRow("Signed:");
            });
        });
    }

    private void ComposeInterviewScoresSection(IContainer container)
    {
        container.Column(column =>
        {
            // ========== HEADER ROW 1 (Text labels ABOVE) ==========
            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(2);     // Summary label
                    columns.RelativeColumn(0.4f);  // 1
                    columns.RelativeColumn(0.4f);  // 2
                    columns.RelativeColumn(0.4f);    // 3
                    columns.RelativeColumn(0.4f);  // 4
                    columns.RelativeColumn(0.4f);  // 5
                });

                table.Cell().Text(""); // Empty slot above summary

                // Above 1 -> Below
                table.Cell().AlignCenter().PaddingTop(8).Text("Below").FontSize(10).Bold().FontColor("#000000");

                table.Cell().Text(""); // Empty above 2

                // Above 3 -> Meet Expectations (split into 2 lines)
                table.Cell().AlignCenter().Column(col =>
                {
                    col.Item().AlignCenter().Text("Meet").FontSize(8).Bold();
                    col.Item().Text("Expectations").FontSize(8).Bold();
                });

                table.Cell().Text(""); // Empty above 4

                // Above 5 -> Exceed
                table.Cell().AlignCenter().PaddingTop(8).Text("Exceed").FontSize(10).Bold().FontColor("#000000");
            });

            // ========== HEADER ROW 2 (Colored Numbers + Summary Label) ==========
            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(2);     // Summary label
                    columns.RelativeColumn(0.4f);  // 1
                    columns.RelativeColumn(0.4f);  // 2
                    columns.RelativeColumn(0.4f);    // 3
                    columns.RelativeColumn(0.4f);  // 4
                    columns.RelativeColumn(0.4f);  // 5
                });

                table.Cell().CornerRadius(5).Background("#A9A9A9").Padding(4).AlignMiddle().Text("Summary of interview scores")
                    .FontSize(11).Bold().FontColor("#FFFFFF");

                table.Cell().CornerRadius(5).Background("#DC143C").Padding(4).AlignCenter().Text("1").FontSize(8).Bold().FontColor("#FFFFFF");
                table.Cell().CornerRadius(5).Background("#FF8C00").Padding(4).AlignCenter().Text("2").FontSize(8).Bold().FontColor("#FFFFFF");
                table.Cell().CornerRadius(5).Background("#FFD700").Padding(4).AlignCenter().Text("3").FontSize(8).Bold().FontColor("#000000");
                table.Cell().CornerRadius(5).Background("#90EE90").Padding(4).AlignCenter().Text("4").FontSize(8).Bold().FontColor("#FFFFFF");
                table.Cell().CornerRadius(5).Background("#32CD32").Padding(4).AlignCenter().Text("5").FontSize(8).Bold().FontColor("#FFFFFF");
            });

            // ========== CRITERIA ROWS ==========
            string[] criteria = new[]
            {
            "Providing Insights",
            "Interacting with People",
            "Establishing Rapport",
            "Convincing People",
            "Articulating Information",
            "Thinking Positively",
            "Taking Action",
            "Seizing Opportunities",
            "Technical Expertise"
        };

            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(2);     // Summary label
                    columns.RelativeColumn(0.4f);  // 1
                    columns.RelativeColumn(0.4f);  // 2
                    columns.RelativeColumn(0.4f);    // 3
                    columns.RelativeColumn(0.4f);  // 4
                    columns.RelativeColumn(0.4f);  // 5
                });

                foreach (var criterion in criteria)
                {
                    table.Cell().Border(1).BorderColor("#CCCCCC").Padding(5)
                        .Text(criterion).FontSize(8).Bold();

                    for (int i = 0; i < 5; i++)
                    {
                        table.Cell().Border(1).BorderColor("#CCCCCC").Padding(5).Text("");
                    }
                }
            });
        });
    }

    private void ComposeCommentarySection(IContainer container)
    {
        container.Column(column =>
        {
            // Header
            column.Item().CornerRadius(5).Background("#A9A9A9").Padding(4).Text("Commentary")
                .FontSize(11)
                .Bold()
                .FontColor("#FFFFFF");

            // Commentary Grid
            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(1);
                    columns.RelativeColumn(1);
                });

                // Key concerns header
                table.Cell().BorderBottom(1).BorderColor("#CCCCCC").Padding(5)
                    .Text("Key concerns about the candidate").FontSize(9).FontColor("#666666");

                // Key strengths header
                table.Cell().BorderBottom(1).BorderColor("#CCCCCC").Padding(5)
                    .Text("Key strengths of this candidate").FontSize(9).FontColor("#666666");

                // Empty content areas
                table.Cell().Border(1).BorderColor("#CCCCCC").Padding(5).MinHeight(80).Text("");
                table.Cell().Border(1).BorderColor("#CCCCCC").Padding(5).MinHeight(80).Text("");
            });
        });
    }

    private void ComposeRecommendationSection(IContainer container)
    {
        container.Column(column =>
        {
            // Header
            column.Item().CornerRadius(5).Background("#A9A9A9").Padding(4).Text("Recommendation")
                .FontSize(11)
                .Bold()
                .FontColor("#FFFFFF");

            // Empty content area
            column.Item().Border(1).BorderColor("#CCCCCC").Padding(5).MinHeight(60).Text("");
        });
    }

    private void ComposeFinalSelectionSection(IContainer container)
    {
        container.Column(column =>
        {
            // Top row with colored segments
            column.Item().Row(row =>
            {
                // Left segment with rounded left corner
                row.RelativeItem(5).Element(e =>
                {
                    e.Background("#A9A9A9")
                     .CornerRadius(5)
                     .Padding(3)
                     .AlignLeft().AlignMiddle()
                     .Text("Final selection recommendation")
                     .FontSize(10)
                     .Bold()
                     .FontColor("#FFFFFF");
                });

                // Spacer
                row.ConstantItem(1);

                // Red segment
                row.RelativeItem(2).Element(e =>
                {
                    e.Background("#DC143C")
                      .CornerRadius(5)
                     .Padding(3)
                     .AlignCenter().AlignMiddle()
                     .Text("Not recommended")
                     .FontSize(10)
                     .Bold()
                     .FontColor("#FFFFFF");
                });

                // Spacer
                row.ConstantItem(1);

                // Yellow segment with two lines
                row.RelativeItem(2).Element(e =>
                {
                    e.Background("#FFD700")
                      .CornerRadius(5)
                     .Padding(3)
                     .AlignCenter().AlignMiddle()
                     .Column(col =>
                     {
                         col.Item().Text("Recommended").FontSize(9).Bold().FontColor("#000000");
                         col.Item().Text("with reservation").FontSize(9).Bold().FontColor("#000000");
                     });
                });

                // Spacer
                row.ConstantItem(1);

                // Green segment with rounded right corner
                row.RelativeItem(2).Element(e =>
                {
                    e.Background("#32CD32")
                     .CornerRadius(5)
                     .Padding(3)
                     .AlignCenter().AlignMiddle()
                     .Text("Recommended")
                     .FontSize(10)
                     .Bold()
                     .FontColor("#FFFFFF");
                });
            });

            // Bottom row with dotted lines
            column.Item().Row(row =>
            {
                // Empty space under gray segment
                row.RelativeItem(5);

                // Dotted line 1
                row.ConstantItem(1).Height(30).Column(col =>
                {
                    for (int i = 0; i < 10; i++)
                    {
                        col.Item().Height(2).LineVertical(1).LineColor("#808080");
                        col.Item().Height(1); // gap
                    }
                });

                // Empty space under red segment
                row.RelativeItem(2);

                // Dotted line 2
                row.ConstantItem(1).Height(30).Column(col =>
                {
                    for (int i = 0; i < 10; i++)
                    {
                        col.Item().Height(2).LineVertical(1).LineColor("#808080");
                        col.Item().Height(1); // gap
                    }
                });

                // Empty space under yellow segment
                row.RelativeItem(2);

                // Dotted line 3
                row.ConstantItem(1).Height(30).Column(col =>
                {
                    for (int i = 0; i < 10; i++)
                    {
                        col.Item().Height(2).LineVertical(1).LineColor("#808080");
                        col.Item().Height(1); // gap
                    }
                });

                // Empty space under green segment
                row.RelativeItem(2);
            });
        });
    }
    #endregion

    #region Page 15 Methods

    /// <summary>
    /// Composes the main content for Page 15 of the Talent Report PDF, which includes technical information,
    /// job/role data, assessment methods, and input data sections.
    /// </summary>
    /// <param name="container">The <see cref="IContainer"/> used to structure and render the page content.</param>
    /// <param name="request">The <see cref="TalentMatchReportRequest"/> containing Page 15 data and related content.</param>
    private void ComposePage15Content(IContainer container, TalentMatchSelectionReportRequest request)
    {
        container.Column(column =>
        {
            // Page Title
            column.Item().PaddingBottom(10).Text("ABOUT")
                .FontSize(18).Bold().FontColor(Colors.Black);

            // Using this summary and additional reports section
            column.Item().PaddingBottom(1).Text("Using this summary and additional reports")
                .FontSize(10).Bold().FontFamily("Arial");

            column.Item().PaddingBottom(8).Text(text =>
            {
                text.Span("This Talent Match Report provides an overview of the key results attained from the assessment and compares this with critical behavioural requirements for a specific role. This report should be read with the detailed reports from each of the assessment methods. This summary report is not intended to be comprehensive and should not be used as the single source of information in the making of any final talent decisions.")
                    .FontSize(8).FontFamily("Arial").LineHeight(1.2f);
            });

            // About the success profile section
            column.Item().PaddingBottom(1).Text("About the success profile")
                .FontSize(10).Bold().FontFamily("Arial");

            column.Item().PaddingBottom(8).Text(text =>
            {
                text.Span("The success profile for this role was developed using a well-researched framework of behaviour. The most critical behaviours for this role were selected from this framework by subject matter experts. The success profiles are presented in two sections as described below.")
                    .FontSize(8).FontFamily("Arial").LineHeight(1.2f);
            });

            // About the assessment methods section
            column.Item().PaddingBottom(1).Text("About the assessment methods")
                .FontSize(10).Bold().FontFamily("Arial");

            column.Item().PaddingBottom(8).Text(text =>
            {
                text.Span("The use of the assessment methods contained in this report is limited to those people who have received specialist training in its use and interpretation. Questionnaires were completed online and without supervision. Due consideration must be given to the subjective nature of questions based ratings in the interpretation of the data.")
                    .FontSize(8).FontFamily("Arial").LineHeight(1.2f);
            });

            // About the scores section
            column.Item().PaddingBottom(1).Text("About the scores")
                .FontSize(10).Bold().FontFamily("Arial");

            column.Item().PaddingBottom(8).Text(text =>
            {
                text.Span("The overall fit score is a weighted score based on the individual’s fit against the essential and important behaviours, as well as the essential skills and capabilities for this role. The results are based on a 1 to 10 scale where 1 is unlikely to be successful and 10 is highly likely to be successful in the particular role.")
                    .FontSize(8).FontFamily("Arial").LineHeight(1.2f);
            });

            column.Item().PaddingBottom(8).Text(text =>
            {
                text.Span("The summary profile includes a summary of behaviours and capabilities that are seen as key strengths, good potential, development opportunities or potential limitations for the role and are defined below.")
                    .FontSize(8).FontFamily("Arial").LineHeight(1.2f);
            });

            // Color-coded information boxes
            column.Item().PaddingBottom(8).Row(row =>
            {
                // Potential limitations (Red section)
                row.AutoItem().Width(270).PaddingRight(5).Column(leftColumn =>
                {
                    leftColumn.Item().PaddingBottom(1)
                        .Background("#E21E26")
                        .CornerRadius(4)
                        .Padding(8)
                        .AlignCenter()
                        .Text("Possible risks for this role")
                        .FontSize(10).Bold().FontColor(Colors.White);

                    leftColumn.Item()
                        .Padding(8)
                        .Text(text =>
                        {
                            text.Span("Behaviours or capabilities in this block are essential or important for success in your role, you show extremely low or very low potential in these areas. Sustained achievement in these areas is a key likely and may limit future success.")
                                .FontSize(8).FontFamily("Arial").LineHeight(1.2f);
                        });
                });

                // Key strengths (Green section)
                row.AutoItem().Width(270).PaddingLeft(5).Column(rightColumn =>
                {
                    rightColumn.Item().PaddingBottom(1)
                        .Background("#39B54A")
                          .CornerRadius(4)
                        .Padding(8)
                        .AlignCenter()
                        .Text("Key strengths for this role")
                        .FontSize(10).Bold().FontColor(Colors.White);

                    rightColumn.Item()
                        .Padding(8)
                        .Text(text =>
                        {
                            text.Span("Behaviours or capabilities in this block are essential or important for success in your role, you show extremely high or very high potential in these areas. Sustained achievement in these areas is very likely and predicts significant strengths that should lead to future success.")
                                .FontSize(8).FontFamily("Arial").LineHeight(1.2f);
                        });
                });
            });

            // Second row of color-coded boxes
            column.Item().PaddingBottom(8).Row(row =>
            {
                // Development opportunities (Orange section)
                row.AutoItem().Width(270).PaddingRight(5).Column(leftColumn =>
                {
                    leftColumn.Item().PaddingBottom(1)
                        .Background("F39726")
                          .CornerRadius(4)
                        .Padding(8)
                        .AlignCenter()
                        .Text("Development opportunities for this role")
                        .FontSize(10).Bold().FontColor(Colors.White);

                    leftColumn.Item()
                        .Padding(8)
                        .Text(text =>
                        {
                            text.Span("Behaviours or capabilities in this block are essential or important for success in your role, you show low or fairly low potential in these areas. Sustained achievement in these areas is quite likely with focused achievement in this role.")
                                .FontSize(8).FontFamily("Arial").LineHeight(1.2f);
                        });
                });

                // Good potential strengths (Light Green section)
                row.AutoItem().Width(270).PaddingLeft(5).Column(rightColumn =>
                {
                    rightColumn.Item().PaddingBottom(1)
                        .Background("#D7D724")
                          .CornerRadius(4)
                        .Padding(8)
                        .AlignCenter()
                        .Text("Good potential strengths for this role")
                        .FontSize(10).Bold().FontColor(Colors.White);

                    rightColumn.Item()
                        .Padding(8)
                        .Text(text =>
                        {
                            text.Span("Behaviours or capabilities in this block are essential or important for success in your role, you show high or fairly high potential in these areas. Sustained achievement in these areas is likely with focused effort in this role.")
                                .FontSize(8).FontFamily("Arial").LineHeight(1.2f);
                        });
                });
            });

            // Individual scores description
            column.Item().PaddingBottom(8).Text(text =>
            {
                text.Span("The individual profile scores from the assessments have been compared with other individuals who have previously completed the assessment (more about this in the technical information section at the back of the report). Results are based on a 1 to 10 scale as shown below.")
                    .FontSize(8).FontFamily("Arial").LineHeight(1.2f);
            });

            // Score scale with labels
            column.Item().PaddingBottom(4).Row(scaleRow =>
            {
                scaleRow.RelativeItem(1).AlignCenter().Text("Extremely\nLow").FontSize(7).FontFamily("Arial");
                scaleRow.RelativeItem(1).AlignCenter().Text("Very Low").FontSize(7).FontFamily("Arial");
                scaleRow.RelativeItem(1).AlignCenter().Text("Low").FontSize(7).FontFamily("Arial");
                scaleRow.RelativeItem(1).AlignCenter().Text("Fairly Low").FontSize(7).FontFamily("Arial");
                scaleRow.RelativeItem(2).AlignCenter().Text("Like most\nothers").FontSize(7).FontFamily("Arial");
                scaleRow.RelativeItem(1).AlignCenter().Text("Fairly High").FontSize(7).FontFamily("Arial");
                scaleRow.RelativeItem(1).AlignCenter().Text("High").FontSize(7).FontFamily("Arial");
                scaleRow.RelativeItem(1).AlignCenter().Text("Very High").FontSize(7).FontFamily("Arial");
                scaleRow.RelativeItem(1).AlignCenter().Text("Extremely\nHigh").FontSize(7).FontFamily("Arial");
            });

            // Color bar with numbers inside boxes
            column.Item().PaddingBottom(2).Row(colorRow =>
            {
                string[] numbers = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
                string[] colors = {
                      "#E31D25", "#EB1C48", "#F15929", "#F59726", "#F3D520", "#F3D520", "#DAE241", "#D7D820", "#8CC63E", "#39B54A"
                };

                for (int i = 0; i < 10; i++)
                {
                    colorRow.RelativeItem(1).Height(16).Background(colors[i]).CornerRadius(4)
                        .AlignCenter().AlignMiddle()
                        .Text(numbers[i])
                        .FontSize(10).Bold().FontFamily("Arial").FontColor(Colors.White);
                }
            });

            // Percentile row (1%, 5%, … 99%)
            string[] percentiles = { "1%", "5%", "10%", "25%", "40%", "60%", "75%", "90%", "95%", "99%" };
            column.Item().PaddingBottom(8).Row(percentileRow =>
            {
                foreach (var p in percentiles)
                {
                    percentileRow.RelativeItem(1).AlignCenter()
                        .Text(p).FontSize(7).FontFamily("Arial");
                }
            });

            // Red underline
            column.Item().PaddingBottom(8).Row(underlineRow =>
            {
                underlineRow.RelativeItem(1).Height(2).Background("#E31D25");
                underlineRow.RelativeItem(1).Height(2).Background("#EB1C48");
                underlineRow.RelativeItem(1).Height(2).Background("#F15929");
                underlineRow.RelativeItem(1).Height(2).Background("#F59726");
                underlineRow.RelativeItem(2).Height(2).Background("#F3D520");
                underlineRow.RelativeItem(1).Height(2).Background("#DAE241");
                underlineRow.RelativeItem(1).Height(2).Background("#D7D820");
                underlineRow.RelativeItem(1).Height(2).Background("#8CC63E");
                underlineRow.RelativeItem(1).Height(2).Background("#39B54A");
            });

            // Bottom development categories
            column.Item().PaddingBottom(8).Row(bottomRow =>
            {
                bottomRow.RelativeItem(2).AlignCenter().Text("Significant Development\nwould be needed")
                    .FontSize(6).Bold().FontFamily("Arial");
                bottomRow.RelativeItem(2).AlignCenter().Text("Development\nwould be needed")
                    .FontSize(6).Bold().FontFamily("Arial");
                bottomRow.RelativeItem(2).AlignCenter().Text("Effective")
                    .FontSize(6).Bold().FontFamily("Arial");
                bottomRow.RelativeItem(2).AlignCenter().Text("Strength")
                    .FontSize(6).Bold().FontFamily("Arial");
                bottomRow.RelativeItem(2).AlignCenter().Text("Significant\nstrength")
                    .FontSize(6).Bold().FontFamily("Arial");
            });

            column.Item().PaddingBottom(8).Text("*Percentage better than comparison group")
              .FontSize(7).Italic().FontFamily("Arial");

            // About this report section
            column.Item().PaddingBottom(8).Text("About this report")
                .FontSize(10).Bold().FontFamily("Arial");

            column.Item().PaddingBottom(8).Text(text =>
            {
                text.Span("This report is based on assessments that explore an individual's motives, preferences, needs and talents in critical work areas. This report may also explore an individual’s leadership challenges and/or strategic capability.")
                    .FontSize(8).FontFamily("Arial").LineHeight(1.2f);
            });

            column.Item().PaddingBottom(8).Text(text =>
            {
                text.Span("Since some of the questionnaires used in this report are self-report measures, the results reflect the individual's self-perceptions. Nevertheless, extensive research has shown these questionnaires to be a valid measure of how people will operate in the workplace.")
                    .FontSize(8).FontFamily("Arial").LineHeight(1.2f);
            });

            column.Item().PaddingBottom(8).Text(text =>
            {
                text.Span("It should be remembered that the information contained in this report is potentially sensitive and every effort should be made to ensure that it is stored in a secure place. This report has been generated electronically. TTS-Top Talent Solutions, or its suppliers cannot guarantee that it has not been changed or edited. We accept no liability for the consequences of the use of this report, howsoever arising.")
                    .FontSize(8).FontFamily("Arial").LineHeight(1.2f);
            });

        });
    }

    #endregion

    #region Page 16 Methods

    /// <summary>
    /// Composes the main content for Page 16 of the Talent Report PDF, which includes technical information,
    /// job/role data, assessment methods, and input data sections.
    /// </summary>
    /// <param name="container">The <see cref="IContainer"/> used to structure and render the page content.</param>
    /// <param name="request">The <see cref="TalentMatchReportRequest"/> containing Page 16 data and related content.</param>
    private void ComposePage16Content(IContainer container, TalentMatchSelectionReportRequest request)
    {
        container.Column(column =>
        {
            // --- PAGE TITLE ---
            column.Item().PaddingBottom(10).Text("TECHNICAL INFORMATION")
                .FontSize(18).Bold().FontColor(Colors.Black);

            // --- JOB/ROLE DATA Section ---
            column.Item().PaddingBottom(10).Column(section =>
            {
                section.Item().Background("#8C919D").CornerRadius(4)
                    .Padding(6).Text("JOB / ROLE DATA")
                    .FontSize(10).Bold().FontColor(Colors.White);

                section.Item().Padding(8).Table(table =>
                {
                    // Define exact column widths
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(120); // Label column
                        columns.ConstantColumn(100);  // First dotted line
                        columns.RelativeColumn();    // Content column
                        columns.ConstantColumn(15);  // Second dotted line (only for row 2)
                        columns.ConstantColumn(80);  // Date column (only for row 2)
                    });

                    // Row 1: Job or role involved
                    table.Cell().Text("Job or role involved").FontSize(9);
                    table.Cell().AlignCenter().LineVertical(1).LineDashPattern([2f, 2f]);
                    table.Cell().ColumnSpan(3).Text($"{request.CompanyName ?? "-"} - {request.Position ?? "-"}").FontSize(9);

                    // Row 2: Job analysis + date
                    table.Cell().Text("Job Analysis").FontSize(9);
                    table.Cell().AlignCenter().LineVertical(1).LineDashPattern([2f, 2f]);
                    table.Cell().AlignLeft().Text(request.Page16?.JobAnalysis ?? "-").FontSize(9);
                    table.Cell().AlignCenter().LineVertical(1).LineDashPattern([2f, 2f]);
                    table.Cell().AlignLeft().Text(request.Page16?.JobAnalysisDate ?? "-").FontSize(9);
                });
            });


            // --- ASSESSMENT METHODS Section ---
            column.Item().PaddingBottom(10).Column(section =>
            {
                section.Item().Background("#8C919D").CornerRadius(4)
                    .Padding(6).Text("ASSESSMENT METHODS")
                    .FontSize(10).Bold().FontColor(Colors.White);

                section.Item().Padding(8).Table(table =>
                {
                    // Define columns with separators
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(150);  // TEST column
                        columns.ConstantColumn(45);   // Separator line
                        columns.RelativeColumn();     // DETAILS column
                        columns.ConstantColumn(15);   // Separator line
                        columns.ConstantColumn(80);   // DATE column
                    });

                    // Header row
                    table.Cell().Text("TEST").FontSize(9).Bold();
                    table.Cell(); // Empty separator cell
                    table.Cell().Text("DETAILS").FontSize(9).Bold();
                    table.Cell(); // Empty separator cell
                    table.Cell().Text("DATE").FontSize(9).Bold();

                    if (request.Page16?.Assessments != null && request.Page16.Assessments.Any())
                    {
                        foreach (var test in request.Page16.Assessments)
                        {
                            // Test name
                            table.Cell().Text(test.TestName ?? "-").FontSize(9);
                            // Separator line
                            table.Cell().AlignCenter().LineVertical(1).LineDashPattern([2f, 2f]);
                            // Details
                            table.Cell().AlignLeft().Text(text =>
                            {
                                if (!string.IsNullOrEmpty(test.Norm))
                                    text.Span($"Norm: {test.Norm}\n").FontSize(9);
                                if (!string.IsNullOrEmpty(request.EmployeeName))
                                    text.Span($"Completed by: {request.EmployeeName}").FontSize(9);
                            });
                            // Separator line
                            table.Cell().AlignCenter().LineVertical(1).LineDashPattern([2f, 2f]);
                            // Date
                            table.Cell().Text(test.Date ?? "-").FontSize(9);

                            // Add grey horizontal line row after each assessment
                            table.Cell().ColumnSpan(5).LineHorizontal(0.5f).LineColor(Colors.Grey.Medium);
                        }
                    }
                    else
                    {
                        table.Cell().ColumnSpan(5).Text("No assessments available").FontSize(9);
                    }
                });
            });

            // --- INPUT DATA Section ---
            column.Item().Column(section =>
            {
                section.Item().Background("#8C919D").CornerRadius(4)
                    .Padding(6).Text("INPUT DATA")
                    .FontSize(10).Bold().FontColor(Colors.White);

                section.Item().Padding(8).Column(inner =>
                {
                    inner.Item().Text(request.Page16?.InputData ?? "-")
                        .FontSize(9).FontFamily("Arial");

                    if (!string.IsNullOrEmpty(request.Page16?.TemplateVersion))
                    {
                        inner.Item().PaddingTop(5).Text(request.Page16.TemplateVersion)
                            .FontSize(9).FontFamily("Arial");
                    }
                });
            });
        });
    }

    #endregion

    #endregion

    #region Helper Methods

    /// <summary>
    /// Validates if the provided string is a valid base64 string
    /// </summary>
    /// <param name="base64String">The base64 string to validate</param>
    /// <returns>True if valid base64, false otherwise</returns>
    private bool IsValidBase64(string base64String)
    {
        if (string.IsNullOrEmpty(base64String))
            return false;

        try
        {
            // Remove data URL prefix if present
            if (base64String.Contains(","))
            {
                base64String = base64String.Split(',')[1];
            }

            // Check if it's valid base64
            var buffer = Convert.FromBase64String(base64String);
            return buffer.Length > 0;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Extracts clean base64 string from data URL format
    /// </summary>
    /// <param name="base64String">Base64 string that may include data URL prefix</param>
    /// <returns>Clean base64 string</returns>
    private string ExtractBase64FromDataUrl(string base64String)
    {
        if (string.IsNullOrEmpty(base64String))
            return string.Empty;

        // Remove data URL prefix if present
        if (base64String.Contains(","))
        {
            return base64String.Split(',')[1];
        }

        return base64String;
    }  

    private void ComposeInterviewQuestionsSection(ColumnDescriptor column, InterviewSection interviewSection, string ratingTitle)
    {
        // Main content box
        column.Item().Column(contentColumn =>
        {
            // Section header
            contentColumn.Item().Background(Colors.Grey.Medium).CornerRadius(5).Padding(5)
                .Text(interviewSection?.Title)
                .FontSize(12)
                .Bold()
                .FontColor(Colors.White);

            // Description
            contentColumn.Item().PaddingTop(10).PaddingBottom(10)
                .Text(interviewSection?.Description)
                .FontSize(9)
                .FontColor(Colors.Black);

            contentColumn.Item().LineHorizontal(2).LineColor(Colors.Grey.Lighten1);

            // Loop through questions
            if (interviewSection?.Questions != null)
            {
                for (int i = 0; i < interviewSection.Questions.Count; i++)
                {
                    var question = interviewSection.Questions[i];

                    // Question with blank space
                    contentColumn.Item().PaddingTop(5).Row(questionRow =>
                    {
                        // Left side - Question and bullets
                        questionRow.RelativeItem().Column(qColumn =>
                        {
                            qColumn.Item().Text(question.Question)
                                .FontSize(10)
                                .FontColor(Colors.Grey.Darken2);

                            qColumn.Item().PaddingTop(5).PaddingLeft(15).Column(bulletColumn =>
                            {
                                foreach (var subQuestion in question.SubQuestions)
                                {
                                    bulletColumn.Item()
                                        .PaddingTop(subQuestion == question.SubQuestions.First() ? 2 : 5)
                                        .Row(row =>
                                        {
                                            row.AutoItem().Text("•").FontSize(9);
                                            row.RelativeItem().PaddingLeft(5).Text(subQuestion).FontSize(9);
                                        });
                                }
                            });
                        });

                        // Vertical line
                        questionRow.ConstantItem(1).Height(150)
                            .LineVertical(1)
                            .LineColor(Colors.Grey.Medium)
                            .LineDashPattern(new float[] { 1f, 1f });

                        // Right side - Blank space for answers
                        questionRow.RelativeItem();
                    });

                    // Divider line (not after last question)
                    if (i < interviewSection.Questions.Count - 1)
                    {
                        contentColumn.Item().LineHorizontal(1).LineColor(Colors.Grey.Darken1);
                    }
                }
            }

            contentColumn.Item().LineHorizontal(2).LineColor(Colors.Grey.Lighten1);
        });

        AddRatingScaleSection(column, ratingTitle);
    }

    /// <summary>
    /// Adds a rating scale component with labels, colored boxes (1-5), and dotted lines
    /// </summary>
    /// <param name="column">The column container to add the rating scale to</param>
    /// <param name="labelText">The label text to display (e.g., "Interview Rating")</param>
    private void AddRatingScaleSection(ColumnDescriptor column, string labelText = "Interview Rating")
    {
        // Rating scale labels at top
        column.Item().Row(row =>
        {
            row.RelativeItem(4).Text(""); // Empty space for label area

            row.RelativeItem(4).Row(labelRow =>
            {
                // "Well below expectations" aligned with box 1
                labelRow.RelativeItem(1).AlignCenter().Text("Well below\nexpectations")
                    .FontSize(8)
                    .FontColor(Colors.Black);

                // Empty space for box 2
                labelRow.RelativeItem(1).Text("");

                // "Meets expectations" aligned with box 3
                labelRow.RelativeItem(1).AlignCenter().Text("Meets\nexpectations")
                    .FontSize(8)
                    .FontColor(Colors.Black);

                // Empty space for box 4
                labelRow.RelativeItem(1).Text("");

                // "Well above expectations" aligned with box 5
                labelRow.RelativeItem(1).AlignCenter().Text("Well above\nexpectations")
                    .FontSize(8)
                    .FontColor(Colors.Black);
            });
        });

        // Rating boxes section with colored boxes
        column.Item().PaddingTop(-10).Row(ratingRow =>
        {
            // Label section
            ratingRow.RelativeItem(4).CornerRadius(8)
                .Background(Colors.Grey.Medium)
                .Padding(6)
                .AlignMiddle()
                .Text(labelText)
                .FontSize(12)
                .Bold()
                .FontColor(Colors.White);

            // Rating boxes section
            ratingRow.RelativeItem(4).Row(boxRow =>
            {
                // Define rating box configurations
                var ratingConfigs = new[]
                {
                new { Rating = "1", Color = Colors.Red.Medium },
                new { Rating = "2", Color = Colors.Orange.Medium },
                new { Rating = "3", Color = Colors.Yellow.Medium },
                new { Rating = "4", Color = Colors.Green.Lighten2 },
                new { Rating = "5", Color = Colors.Green.Medium }
            };

                foreach (var config in ratingConfigs)
                {
                    boxRow.RelativeItem().CornerRadius(6)
                        .Background(config.Color)
                        .Border(1)
                        .BorderColor(Colors.White)
                        .Padding(8)
                        .Row(row =>
                        {
                            row.RelativeItem().AlignLeft().AlignMiddle()
                                .Text("-").FontSize(10).Bold().FontColor(Colors.White);
                            row.RelativeItem().AlignCenter().AlignMiddle()
                                .Text(config.Rating).FontSize(10).Bold().FontColor(Colors.White);
                            row.RelativeItem().AlignRight().AlignMiddle()
                                .Text("+").FontSize(10).Bold().FontColor(Colors.White);
                        });
                }
            });
        });

        // Dotted lines below matching box widths
        column.Item().PaddingTop(-15).Row(lineRow =>
        {
            lineRow.RelativeItem(4).Text(""); // Empty space for label area
            lineRow.RelativeItem(4).Row(dotRow =>
            {
                // Create dotted line for each of the 5 boxes
                for (int i = 0; i < 10; i++)
                {
                    dotRow.RelativeItem().Column(col =>
                    {
                        // Create dotted effect with small segments
                        for (int j = 0; j < 10; j++)
                        {
                            col.Item().Height(2).BorderLeft(1).BorderColor(Colors.Grey.Medium);
                            col.Item().Height(3); // Gap
                        }
                    });
                }
                // Add one more line at the end
                dotRow.RelativeItem(0.1f).Column(col =>
                {
                    for (int j = 0; j < 10; j++)
                    {
                        col.Item().Height(2).BorderLeft(1).BorderColor(Colors.Grey.Medium);
                        col.Item().Height(3);
                    }
                });
            });
        });
    }
    #endregion
}