using DocumentFormat.OpenXml.Drawing.Charts;

using Library.Assess.Models.ReportPDF;

using Microsoft.Extensions.Logging;

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Library.Assess.Services.ReportPDF;

public interface IPdfReportService
{
    /// <summary>
    /// Generates a Talent Report PDF based on the provided request and returns a response object.
    /// </summary>
    /// <param name="request">The request containing all data for the Talent Report.</param>
    /// <returns>A <see cref="TalentReportResponse"/> containing the PDF data and metadata.</returns>
    Task<TalentReportResponse> GenerateReportAsync(TalentReportRequest request);

    /// <summary>
    /// Generates a Talent Report PDF and returns it as a byte array.
    /// </summary>
    /// <param name="request">The request containing all data for the Talent Report.</param>
    /// <returns>A byte array representing the generated PDF file.</returns>
    Task<byte[]> GeneratePdfBytesAsync(TalentReportRequest request);
}

public class PdfReportService : IPdfReportService
{
    public PdfReportService()
    {        // Set QuestPDF license (use Community for free version)
        QuestPDF.Settings.License = LicenseType.Community;
    }

    /// <summary>
    /// Generates a Talent Report PDF based on the provided request and returns a response object.
    /// </summary>
    /// <param name="request">The request containing all data for the Talent Report.</param>
    /// <returns>A <see cref="TalentReportResponse"/> containing the PDF data and metadata.</returns>
    public async Task<TalentReportResponse> GenerateReportAsync(TalentReportRequest request)
    {
        try
        {
            var pdfBytes = await GeneratePdfBytesAsync(request);
            var fileName = $"TalentReport_{request.EmployeeName.Replace(" ", "_")}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

            return new TalentReportResponse
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
            return new TalentReportResponse
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
    public async Task<byte[]> GeneratePdfBytesAsync(TalentReportRequest request)
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
    /// <param name="request">The <see cref="TalentReportRequest"/> containing all data for the report.</param>
    private void ComposePage1(PageDescriptor page, TalentReportRequest request)
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

            column.Item().Element(c => ComposeBottomDecoration(c, request));
        });
    }
    #endregion

    #region Page 2 Methods
    /// <summary>
    /// Composes the content for Page 2 of the Talent Report PDF.
    /// </summary>
    /// <param name="page">The <see cref="PageDescriptor"/> representing the PDF page to be composed.</param>
    /// <param name="request">The <see cref="TalentReportRequest"/> containing all data for the report.</param>
    private void ComposePage2(PageDescriptor page, TalentReportRequest request)
    {
        page.Size(PageSizes.A4);
        page.Margin(0);
        page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Arial"));

        // Prevent headers/footers on overflow pages
        page.Header().ShowOnce().Element(c => ComposePageHeader(c, request));
        page.Footer().ShowOnce().Element(c => ComposePageFooter(c, request,2));

        page.Content().PaddingHorizontal(20).PaddingVertical(15)
            .Element(c => ComposePage2Content(c, request));
    }
    #endregion

    #region Page 3 Methods
    /// <summary>
    /// Composes the content for Page 3 of the Talent Report PDF.
    /// </summary>
    /// <param name="page">The <see cref="PageDescriptor"/> representing the PDF page to be composed.</param>
    /// <param name="request">The <see cref="TalentReportRequest"/> containing all data for the report.</param>
    private void ComposePage3(PageDescriptor page, TalentReportRequest request)
    {
        page.Size(PageSizes.A4);
        page.Margin(0);
        page.DefaultTextStyle(x => x.FontSize(8).FontFamily("Arial"));

        // Prevent headers/footers on overflow pages
        page.Header().ShowOnce().Element(c => ComposePageHeader(c, request));
        page.Footer().ShowOnce().Element(c => ComposePageFooter(c, request,3));

        page.Content().PaddingHorizontal(20).PaddingVertical(15)
            .Element(c => ComposePage3Content(c, request));
    }
    #endregion

    #region Page 4 Methods

    /// <summary>
    /// Composes the content for Page 4 of the Talent Report PDF.
    /// </summary>
    /// <param name="page">The <see cref="PageDescriptor"/> representing the PDF page to be composed.</param>
    /// <param name="request">The <see cref="TalentReportRequest"/> containing all data for the report.</param>
    private void ComposePage4(PageDescriptor page, TalentReportRequest request)
    {
        page.Size(PageSizes.A4);
        page.Margin(0);
        page.DefaultTextStyle(x => x.FontSize(8).FontFamily("Arial"));

        // Prevent headers/footers on overflow pages
        page.Header().ShowOnce().Element(c => ComposePageHeader(c, request));
        page.Footer().ShowOnce().Element(c => ComposePageFooter(c, request,4));

        page.Content().PaddingHorizontal(20).PaddingVertical(15)
            .Element(c => ComposePage4Content(c, request));
    }
    #endregion

    #region Page 5 Methods

    /// <summary>
    /// Composes the content for Page 5 of the Talent Report PDF.
    /// </summary>
    /// <param name="page">The <see cref="PageDescriptor"/> representing the PDF page to be composed.</param>
    /// <param name="request">The <see cref="TalentReportRequest"/> containing all data for the report.</param>
    private void ComposePage5(PageDescriptor page, TalentReportRequest request)
    {
        page.Size(PageSizes.A4);
        page.Margin(0);
        page.DefaultTextStyle(x => x.FontSize(8).FontFamily("Arial"));

        // Prevent headers/footers on overflow pages
        page.Header().ShowOnce().Element(c => ComposePageHeader(c, request));
        page.Footer().ShowOnce().Element(c => ComposePageFooter(c, request,5));

        page.Content().PaddingHorizontal(20).PaddingVertical(15)
            .Element(c => ComposePage5Content(c, request));
    }
    #endregion

    #region Page 6 Methods

    /// <summary>
    /// Composes the content for Page 6 of the Talent Report PDF.
    /// </summary>
    /// <param name="page">The <see cref="PageDescriptor"/> representing the PDF page to be composed.</param>
    /// <param name="request">The <see cref="TalentReportRequest"/> containing all data for the report.</param>
    private void ComposePage6(PageDescriptor page, TalentReportRequest request)
    {
        page.Size(PageSizes.A4);
        page.Margin(0);
        page.DefaultTextStyle(x => x.FontSize(8).FontFamily("Arial"));

        // Prevent headers/footers on overflow pages
        page.Header().ShowOnce().Element(c => ComposePageHeader(c, request));
        page.Footer().ShowOnce().Element(c => ComposePageFooter(c, request,6));

        page.Content().PaddingHorizontal(20).PaddingVertical(15)
            .Element(c => ComposePage6Content(c, request));
    }
    #endregion

    #region Page 7 Methods

    /// <summary>
    /// Composes the content for Page 7 of the Talent Report PDF.
    /// </summary>
    /// <param name="page">The <see cref="PageDescriptor"/> representing the PDF page to be composed.</param>
    /// <param name="request">The <see cref="TalentReportRequest"/> containing all data for the report.</param>
    private void ComposePage7(PageDescriptor page, TalentReportRequest request)
    {
        page.Size(PageSizes.A4);
        page.Margin(0);
        page.DefaultTextStyle(x => x.FontSize(8).FontFamily("Arial"));

        // Prevent headers/footers on overflow pages
        page.Header().ShowOnce().Element(c => ComposePageHeader(c, request));
        page.Footer().ShowOnce().Element(c => ComposePageFooter(c, request,7));

        page.Content().PaddingHorizontal(20).PaddingVertical(15)
            .Element(c => ComposePage7Content(c, request));
    }
    #endregion

    #endregion

    #region Section Composers

    #region Generic Header/Footer Methods from page 2-7
    /// <summary>
    /// Composes the header section for a PDF page, including employee information, company details, position, and company logo.
    /// </summary>
    /// <param name="container">The <see cref="IContainer"/> used to compose the header layout.</param>
    /// <param name="request">The <see cref="TalentReportRequest"/> containing all data required for the header.</param>
    private void ComposePageHeader(IContainer container, TalentReportRequest request)
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

               // Right side: Logo + Wamly text - aligned
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
    /// <param name="request">The <see cref="TalentReportRequest"/> containing all data required for the footer.</param>
    /// <param name="pageNumber">The current page number to display in the footer.</param>
    private void ComposePageFooter(IContainer container, TalentReportRequest request, int pageNumber)
    {
        container
            .Background(Colors.White)
            .BorderTop(1)
            .BorderColor(Colors.Grey.Lighten3)
            .PaddingHorizontal(40)
            .PaddingVertical(15)
            .Row(row =>
            {
                // Left section (FooterText + Logo + | + URL)
                row.RelativeItem().AlignLeft().Row(leftRow =>
                {
                    // Footer text
                    if (!string.IsNullOrEmpty(request.FooterText))
                    {
                        leftRow.AutoItem().AlignMiddle().Text(request.FooterText)
                            .FontSize(10).FontColor(Colors.Grey.Darken1).WrapAnywhere();

                        // Small spacing after footer text
                        leftRow.ConstantItem(5);
                    }

                    // Company logo
                    leftRow.AutoItem().AlignMiddle().Container().Height(14).Width(42).Element(element =>
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
                    leftRow.ConstantItem(0);
                    // Bold separator "|"
                    leftRow.AutoItem().AlignMiddle().Text("|")
                        .FontSize(14).FontColor(Colors.Grey.Darken1).Bold();

                    // Reduced spacing after separator
                    leftRow.ConstantItem(2);

                    // Footer URL
                    if (!string.IsNullOrEmpty(request.Page2.FooterUrl))
                        leftRow.AutoItem().AlignMiddle().Text(request.Page2.FooterUrl)
                            .FontSize(9).FontColor(Colors.Grey.Darken1).WrapAnywhere();
                });

                // Page number pinned right
                row.ConstantItem(50)
                    .AlignRight()
                    .AlignBottom()
                    .Text(pageNumber)
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
    /// <param name="request">The <see cref="TalentReportRequest"/> containing company logo and name.</param>
    private void ComposeHeader(IContainer container, TalentReportRequest request)
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
    /// <param name="request">The <see cref="TalentReportRequest"/> containing the title and subtitle.</param>
    private void ComposeTitle(IContainer container, TalentReportRequest request)
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
    /// <param name="request">The <see cref="TalentReportRequest"/> containing employee and company details.</param>
    private void ComposeEmployeeInfo(IContainer container, TalentReportRequest request)
    {
        container.Padding(15).Column(infoColumn =>
        {
            infoColumn.Item().Text($"{request.CompanyName} - Developer")
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
    /// <param name="request">The <see cref="TalentReportRequest"/> containing the report description text.</param>
    private void ComposeDescription(IContainer container, TalentReportRequest request)
    {
        container.Column(descColumn =>
        {
            descColumn.Item().Text(request.Page1.ReportDescription)
                .FontSize(11).LineHeight(1.4f).FontColor(Colors.Black).Justify().WrapAnywhere();
        });
    }

    /// <summary>
    /// Composes the development summary section on Page 1.
    /// </summary>
    /// <param name="container">The <see cref="IContainer"/> used to compose the development summary layout.</param>
    /// <param name="request">The <see cref="TalentReportRequest"/> containing the development summary text.</param>
    private void ComposeDevelopmentSummary(IContainer container, TalentReportRequest request)
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
    /// <param name="request">The <see cref="TalentReportRequest"/> containing the usage instructions text.</param>
    private void ComposeUsageInstructions(IContainer container, TalentReportRequest request)
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
    /// <param name="request">The <see cref="TalentReportRequest"/> containing the validity period text.</param>
    private void ComposeValidityInfo(IContainer container, TalentReportRequest request)
    {
        container.Text(request.Page1.ValidityPeriod)
            .FontSize(11).LineHeight(1.4f).FontColor(Colors.Black).Justify();
    }

    /// <summary>
    /// Composes the footer information section on Page 1, e.g., confidentiality notice.
    /// </summary>
    /// <param name="container">The <see cref="IContainer"/> used to compose the footer layout.</param>
    /// <param name="request">The <see cref="TalentReportRequest"/> containing footer details.</param>
    private void ComposeFooterInfo(IContainer container, TalentReportRequest request)
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
    /// <param name="request">The <see cref="TalentReportRequest"/> containing branding details.</param>
    private void ComposeBottomDecoration(IContainer container, TalentReportRequest request)
    {
        container
            .AlignBottom()
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
    /// <param name="request">The <see cref="TalentReportRequest"/> containing Page 2 data.</param>
    private void ComposePage2Content(IContainer container, TalentReportRequest request)
    {
        container.Column(column =>
        {
            // Title
            column.Item().PaddingBottom(10).Text(request.Page2.PageTitle)
                .FontSize(16).Bold().FontColor(Colors.Black).WrapAnywhere();

            // Introduction paragraphs
            column.Item().PaddingBottom(10).Column(textCol =>
            {
                if (!string.IsNullOrEmpty(request.Page2.IntroductionParagraph1))
                    textCol.Item().PaddingBottom(6).Text(request.Page2.IntroductionParagraph1)
                        .FontSize(11).LineHeight(1.3f).WrapAnywhere();

                if (!string.IsNullOrEmpty(request.Page2.IntroductionParagraph2))
                    textCol.Item().PaddingBottom(6).Text(request.Page2.IntroductionParagraph2)
                        .FontSize(11).LineHeight(1.3f).WrapAnywhere();

                if (!string.IsNullOrEmpty(request.Page2.IntroductionParagraph3))
                    textCol.Item().PaddingBottom(6).Text(request.Page2.IntroductionParagraph3)
                        .FontSize(11).LineHeight(1.3f).WrapAnywhere();

                if (!string.IsNullOrEmpty(request.Page2.IntroductionParagraph4))
                    textCol.Item().Text(request.Page2.IntroductionParagraph4)
                        .FontSize(11).LineHeight(1.3f).WrapAnywhere();
            });

            // Summary Profile title
            column.Item().PaddingBottom(8).Text(request.Page2.SummaryProfileTitle)
                .FontSize(14).Bold().FontColor(Colors.Black).WrapAnywhere();

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
    #endregion

    #region Page 3 Methods    

    /// <summary>
    /// Composes the main content for Page 3 of the Talent Report PDF, including detailed profile sections for behaviours and skills.
    /// </summary>
    /// <param name="container">The <see cref="IContainer"/> used to compose the content layout.</param>
    /// <param name="request">The <see cref="TalentReportRequest"/> containing Page 3 data.</param>
    private void ComposePage3Content(IContainer container, TalentReportRequest request)
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
            column.Item().PaddingBottom(4).Row(topLabelRow => // Further reduced padding
            {
                topLabelRow.ConstantItem(240); // Slightly reduced

                topLabelRow.RelativeItem().Row(labelRow =>
                {
                    labelRow.RelativeItem().AlignLeft()
                        .Text("Low").FontSize(8).FontColor(Colors.Black).Bold(); // Further reduced

                    labelRow.RelativeItem().AlignCenter()
                        .Text("Moderate").FontSize(8).FontColor(Colors.Black).Bold();

                    labelRow.RelativeItem().AlignRight()
                        .Text("High").FontSize(8).FontColor(Colors.Black).Bold();
                });
            });

            // Essential behaviours section
            if (request.Page3.EssentialBehaviours?.Any() == true)
            {
                column.Item().PaddingBottom(3).Element(c => ComposeProfileSection(c, "Essential behaviours", // Further reduced padding
                    request.Page3.EssentialBehaviours, "#6B7280"));
            }

            // Important behaviours section  
            if (request.Page3.ImportantBehaviours?.Any() == true)
            {
                column.Item().PaddingBottom(3).Element(c => ComposeProfileSection(c, "Important behaviours", // Further reduced padding
                    request.Page3.ImportantBehaviours, "#6B7280"));
            }

            // Essential skills and aptitudes section
            if (request.Page3.EssentialSkills?.Any() == true)
            {
                column.Item().Element(c => ComposeProfileSection(c, "Essential skills and aptitudes",
                    request.Page3.EssentialSkills, "#6B7280"));
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
                    // Data row                   
                    tableColumn.Item().Row(dataRow =>
                    {
                        // Item name                       
                        dataRow.ConstantItem(240)
                            .Background(Colors.White)
                            .PaddingHorizontal(10).PaddingVertical(4)
                            .AlignMiddle()
                            .Text(item.Name)
                            .FontSize(8).FontColor(Colors.Black);

                        // Score scale area                       
                        dataRow.RelativeItem().Background(Colors.White).Row(scoreRow =>
                        {
                            for (int i = 1; i <= 10; i++)
                            {
                                // Define which columns should have grey background
                                var greyColumns = new int[] { 1, 2, 5, 6, 9, 10 };
                                var cellBackgroundColor = greyColumns.Contains(i) ? "#E5E5E5" : "#FFFFFF";

                                scoreRow.RelativeItem().Height(24)
                                    .Background(cellBackgroundColor) // Apply grey or white background
                                    .AlignCenter().AlignMiddle()
                                    .Element(scoreCell =>
                                    {
                                        if (i == item.Score)
                                        {
                                            scoreCell.Width(12).Height(12)
                                                .Background(GetScoreColor(item.Score))
                                                .Border(1)
                                                .BorderColor("#BDBEC0")
                                                .CornerRadius(6);
                                        }
                                    });
                            }
                        });
                    });

                    // Horizontal separator line (instead of BorderBottom)                   
                    tableColumn.Item().PaddingBottom(0).Element(lineContainer =>
                    {
                        lineContainer.LineHorizontal(1.5f)
                            .LineColor("#C8C8CA");
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
    /// <summary>
    /// Composes the main content for Page 4 of the Talent Report PDF, including full behavioural styles sections.
    /// </summary>
    /// <param name="container">The <see cref="IContainer"/> used to compose the content layout.</param>
    /// <param name="request">The <see cref="TalentReportRequest"/> containing Page 4 data.</param>
    private void ComposePage4Content(IContainer container, TalentReportRequest request)
    {
        container.Column(column =>
        {
            // Title
            column.Item().PaddingBottom(8).Text("Full Behavioural Styles Profile")
                .FontSize(16).Bold().FontColor(Colors.Black);

            // Description paragraph
            column.Item().PaddingBottom(10).Text(text =>
            {
                text.Span("This profile gives a detailed view of your greater and lesser preferences to execute different work-related behaviours that will influence your potential to be successful in your role. These preferences are based on your desires or motives to execute these behaviours, as well as behaviours you perceive as personal strengths.")
                    .FontSize(9).LineHeight(1.3f).FontColor(Colors.Black);
            });

            // Show Low/Moderate/High labels above all sections
            column.Item().PaddingBottom(4).Row(topLabelRow =>
            {
                topLabelRow.ConstantItem(240);
                topLabelRow.RelativeItem().Row(labelRow =>
                {
                    labelRow.RelativeItem().AlignLeft()
                        .Text("Low").FontSize(8).FontColor(Colors.Black).Bold();
                    labelRow.RelativeItem().AlignCenter()
                        .Text("Moderate").FontSize(8).FontColor(Colors.Black).Bold();
                    labelRow.RelativeItem().AlignRight()
                        .Text("High").FontSize(8).FontColor(Colors.Black).Bold();
                });
            });

            // Problem Solving section
            if (request.Page4.ProblemSolving?.Any() == true)
            {
                column.Item().PaddingBottom(4).Element(c => ComposeBehaviouralSection(c, "Problem Solving",
                    request.Page4.ProblemSolving, "#6B7280"));
            }

            // Influencing People section  
            if (request.Page4.InfluencingPeople?.Any() == true)
            {
                column.Item().PaddingBottom(4).Element(c => ComposeBehaviouralSection(c, "Influencing People",
                    request.Page4.InfluencingPeople, "#6B7280"));
            }

            // Adapting Approaches section
            if (request.Page4.AdaptingApproaches?.Any() == true)
            {
                column.Item().PaddingBottom(4).Element(c => ComposeBehaviouralSection(c, "Adapting Approaches",
                    request.Page4.AdaptingApproaches, "#6B7280"));
            }

            // Delivering Success section
            if (request.Page4.DeliveringSuccess?.Any() == true)
            {
                column.Item().Element(c => ComposeBehaviouralSection(c, "Delivering Success",
                    request.Page4.DeliveringSuccess, "#6B7280"));
            }
        });
    }

    /// <summary>
    /// Composes a behavioural section table with a title, a list of <see cref="BehaviouralItem"/>s, and a header color.
    /// </summary>
    /// <param name="container">The <see cref="IContainer"/> used to compose the section layout.</param>
    /// <param name="sectionTitle">The title of the behavioural section.</param>
    /// <param name="items">The list of <see cref="BehaviouralItem"/>s to display in the section.</param>
    /// <param name="sectionColor">The background color for the section header.</param>
    private void ComposeBehaviouralSection(IContainer container, string sectionTitle, List<BehaviouralItem> items, string sectionColor)
    {
        container.Column(column =>
        {
            // Section with border
            column.Item().Column(sectionColumn =>
            {
                // Section header spanning full width
                sectionColumn.Item().Row(headerRow =>
                {
                    // Section title - takes partial width
                    headerRow.ConstantItem(240).Background(sectionColor).CornerRadius(4)
                        .PaddingHorizontal(12).PaddingVertical(4)
                        .AlignMiddle()
                        .Text(sectionTitle)
                        .FontSize(12).Bold().FontColor(Colors.White);

                    // Scale numbers 1-10 - fills remaining width
                    headerRow.RelativeItem().BorderColor(Colors.Black).Row(scaleRow =>
                    {
                        for (int i = 1; i <= 10; i++)
                        {
                            var cellColor = GetScaleBackgroundColor(i);
                            scaleRow.RelativeItem().PaddingHorizontal(1).Height(20)
                                .Background(cellColor)
                                 .CornerRadius(3)
                                .AlignCenter().AlignMiddle()
                                .Text(i.ToString())
                                .FontSize(9).FontColor(Colors.White).Bold();
                        }
                    });
                });

                // Behavior items
                foreach (var item in items)
                {
                    sectionColumn.Item().BorderTop(1).BorderColor(Colors.Grey.Lighten2).Row(itemRow =>
                    {
                        // Behavior name and sub-behaviors - same width as section title
                        itemRow.ConstantItem(240).Background(Colors.White)
                            .PaddingHorizontal(2).PaddingVertical(3)
                            .Column(nameColumn =>
                            {
                                // Main behavior name
                                nameColumn.Item().Text(item.Name)
                                    .FontSize(10).Bold().FontColor(Colors.Black);

                                // Sub-behaviors if any
                                if (item.SubBehaviors?.Any() == true)
                                {
                                    nameColumn.Item().Text(text =>
                                    {
                                        text.Span(string.Join(", ", item.SubBehaviors.Select(sb => $"{sb.Name} ({sb.Score})")))
                                            .FontSize(8).FontColor(Colors.Grey.Darken1);
                                    });
                                }
                            });

                        // Score visualization - matches scale header width exactly
                        itemRow.RelativeItem().Background(Colors.Grey.Lighten3).Row(scoreRow =>
                        {
                            for (int i = 1; i <= 10; i++)
                            {
                                // Define which columns should have grey background
                                var greyColumns = new int[] { 1, 2, 5, 6, 9, 10 };
                                var cellBackgroundColor = greyColumns.Contains(i) ? "#E5E5E5" : "#FFFFFF";

                                scoreRow.RelativeItem().Height(35) 
                                    .Background(cellBackgroundColor)
                                    .AlignCenter().AlignMiddle()
                                    .Element(scoreCell =>
                                    {
                                        if (i == item.Score)
                                        {
                                            // Show filled circle at score position
                                            scoreCell.Width(14).Height(14)
                                                .Background(GetBehaviorScoreColor(item.Score))
                                                .Border(2).BorderColor("#BDBEC0")
                                                .CornerRadius(7);
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
    /// Returns a color corresponding to a scale position from 1 to 10 for behavioural section headers.
    /// </summary>
    /// <param name="position">The position on the scale (1–10).</param>
    /// <returns>A hex color string for the scale cell background.</returns>
    private string GetScaleBackgroundColor(int position)
    {
        // Color gradient for scale background (1-10)
        var colors = new[]
        {
           "#E31D25", "#EB1C48", "#F15929", "#F59726", "#F3D520", "#F3D520", "#DAE241", "#D7D820", "#8CC63E", "#39B54A"
    };
        return colors[Math.Max(0, Math.Min(9, position - 1))];
    }

    /// <summary>
    /// Returns a color corresponding to a score for the behaviour score indicator circle.
    /// </summary>
    /// <param name="score">The score value (1–10).</param>
    /// <returns>A hex color string representing the score indicator color.</returns>
    private string GetBehaviorScoreColor(int score)
    {
        // Color for the score indicator circle
        var colors = new[]
        {
        "#E31D25", "#EB1C48", "#F15929", "#F59726", "#F3D520", "#F3D520", "#DAE241", "#D7D820", "#8CC63E", "#39B54A"
    };
        return colors[Math.Max(0, Math.Min(9, score - 1))];
    }

    #endregion

    #region Page 5 Methods
    /// <summary>
    /// Composes the main content for Page 5 of the Talent Report PDF, which includes development tips and dynamic sections.
    /// </summary>
    /// <param name="container">The <see cref="IContainer"/> used to compose the layout and content of the page.</param>
    /// <param name="request">The <see cref="TalentReportRequest"/> containing Page 5 data, including development and dynamic sections.</param>
    private void ComposePage5Content(IContainer container, TalentReportRequest request)
    {
        container.Column(column =>
        {
            // Page Title
            column.Item().PaddingBottom(10).Text("DEVELOPMENT TIPS")
                .FontSize(18).Bold().FontColor(Colors.Black);

            // Introduction paragraph
            column.Item().PaddingBottom(8).Text(text =>
            {
                text.Span("Based on the results shown on your profile you can now identify the behaviours you might like to develop further.")
                    .FontSize(10).FontFamily("Arial");
            });

            // Main content paragraph
            column.Item().PaddingBottom(8).Text(text =>
            {
                text.Span("This section of the report outlines what actions could be considered to improve your behavioural competence. The appropriateness of each piece of advice will differ for each individual and will to some extent depend on your job and the opportunities and resources available.")
                    .FontSize(10).FontFamily("Arial");
            });

            // Development strategy paragraph
            column.Item().PaddingBottom(8).Text(text =>
            {
                text.Span("It is recommended that you initially look at both your personal strengths and development areas as part of your personal development strategy. In some cases there might be no strengths or developmental behaviours identified. Then you could identify the behaviours that would be to the greatest benefit to your development in your current work context. Read through the relevant development tips and decide on one action or development tip that would be practical in your work context for each of the behavioural dimensions you would like to focus on for your development.")
                    .FontSize(10).FontFamily("Arial");
            });

            // Dynamic Development Sections
            if (request.Page5?.DevelopmentSections != null && request.Page5.DevelopmentSections.Any())
            {
                foreach (var section in request.Page5.DevelopmentSections)
                {
                    column.Item().PaddingBottom(5).AlignCenter().Column(sectionColumn =>
                    {
                        // Section Title with grey background
                        sectionColumn.Item().PaddingBottom(8)
                            .Background("#8C919D").CornerRadius(4)
                            .Padding(4)
                            .AlignCenter()
                            .Text(section.Title)
                            .FontSize(10).Bold().FontColor(Colors.White);

                        // Section Description (italic)
                        if (!string.IsNullOrEmpty(section.Description))
                        {
                            sectionColumn.Item().PaddingBottom(8).AlignLeft().Text(text =>
                            {
                                text.Span(section.Description)
                                    .FontSize(10).FontFamily("Arial");
                            });
                        }

                        // Section Tips
                        if (section.Tips != null && section.Tips.Any())
                        {
                            sectionColumn.Item().PaddingBottom(4).Column(bulletColumn =>
                            {
                                foreach (var tip in section.Tips)
                                {
                                    bulletColumn.Item().PaddingBottom(4).Row(row =>
                                    {
                                        row.ConstantItem(15).AlignTop()
                                            .PaddingTop(2)
                                            .Text("•")
                                            .FontSize(16);

                                        row.RelativeItem().AlignLeft()
                                            .Text(tip)
                                            .FontSize(10)
                                            .FontFamily("Arial")
                                            .LineHeight(1.2f);
                                    });
                                }
                            });
                        }
                    });
                }
            }


            // Dynamic Sections (same styling)
            if (request.Page5?.DynamicSections != null)
            {
                foreach (var section in request.Page5.DynamicSections)
                {
                    column.Item().PaddingBottom(5).AlignCenter().Column(sectionColumn =>
                    {
                        // Section Title with grey background
                        sectionColumn.Item().PaddingBottom(8)
                             .Background("#8C919D").CornerRadius(4)
                            .Padding(4)
                            .AlignCenter()
                            .Text(section.Title)
                            .FontSize(11).Bold().FontColor(Colors.White);

                        // Section Description (italic)
                        if (!string.IsNullOrEmpty(section.Description))
                        {
                            sectionColumn.Item().PaddingBottom(8).AlignLeft().Text(text =>
                            {
                                text.Span(section.Description)
                                    .FontSize(10).FontFamily("Arial");
                            });
                        }

                        // Section Tips
                        if (section.Tips != null && section.Tips.Any())
                        {
                            sectionColumn.Item().PaddingBottom(4).Column(bulletColumn =>
                            {
                                foreach (var tip in section.Tips)
                                {
                                    bulletColumn.Item().PaddingBottom(4).Row(row =>
                                    {
                                        row.ConstantItem(15).AlignTop()
                                            .PaddingTop(2)
                                            .Text("•")
                                            .FontSize(16);

                                        row.RelativeItem().AlignLeft()
                                            .Text(tip)
                                            .FontSize(10)
                                            .FontFamily("Arial")
                                            .LineHeight(1.2f);
                                    });
                                }
                            });
                        }
                    });
                }
            }

        });
    }
    #endregion

    #region Page 6 Methods
    /// <summary>
    /// Composes the main content for Page 6 of the Talent Report PDF, which includes the "About" section,
    /// details on the success profile, assessment methods, scores, color-coded performance boxes,
    /// percentile information, and guidance on interpreting the report.
    /// </summary>
    /// <param name="container">The <see cref="IContainer"/> used to structure and render the page content.</param>
    /// <param name="request">The <see cref="TalentReportRequest"/> containing Page 6 data and related content.</param>
    private void ComposePage6Content(IContainer container, TalentReportRequest request)
    {
        container.Column(column =>
        {
            // Page Title
            column.Item().PaddingBottom(10).Text("ABOUT")
                .FontSize(18).Bold().FontColor(Colors.Black);

            // Using this summary and additional reports section
            column.Item().PaddingBottom(3).Text("Using this summary and additional reports")
                .FontSize(10).Bold().FontFamily("Arial");

            column.Item().PaddingBottom(8).Text(text =>
            {
                text.Span("This Talent Match Report provides an overview of the key results attained from the assessment and compares this with critical behavioural requirements for a specific role. This report should be read with the detailed reports from each of the assessment methods. This summary report is not intended to be comprehensive and should not be used as the single source of information in the making of any final talent decisions.")
                    .FontSize(8).FontFamily("Arial").LineHeight(1.2f);
            });

            // About the success profile section
            column.Item().PaddingBottom(3).Text("About the success profile")
                .FontSize(10).Bold().FontFamily("Arial");

            column.Item().PaddingBottom(8).Text(text =>
            {
                text.Span("The success profile for this role was developed using a well-researched framework of behaviour. The most critical behaviours for this role were selected from this framework by subject matter experts. The success profiles are presented in two sections as described below.")
                    .FontSize(8).FontFamily("Arial").LineHeight(1.2f);
            });

            // About the assessment methods section
            column.Item().PaddingBottom(3).Text("About the assessment methods")
                .FontSize(10).Bold().FontFamily("Arial");

            column.Item().PaddingBottom(8).Text(text =>
            {
                text.Span("The use of the assessment methods contained in this report is limited to those people who have received specialist training in its use and interpretation. Questionnaires were completed online and without supervision. Due consideration must be given to the subjective nature of questions based ratings in the interpretation of the data.")
                    .FontSize(8).FontFamily("Arial").LineHeight(1.2f);
            });

            // About the scores section
            column.Item().PaddingBottom(3).Text("About the scores")
                .FontSize(10).Bold().FontFamily("Arial");

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
                        .Padding(8)
                        .AlignCenter()
                        .Text("Potential limitations for this role")
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
                text.Span("Your individual scores from the assessments have been compared with other individuals who have previously completed the assessment (more about this in the technical information section at the back of the report). Scores are based on a 1 to 10 scale as shown below.")
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
                    colorRow.RelativeItem(1).Height(20).Background(colors[i])
                        .AlignCenter().AlignMiddle()
                        .Text(numbers[i])
                        .FontSize(8).Bold().FontFamily("Arial").FontColor(Colors.White);
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

    #region Page 7 Methods

    /// <summary>
    /// Composes the main content for Page 7 of the Talent Report PDF, which includes technical information,
    /// job/role data, assessment methods, and input data sections.
    /// </summary>
    /// <param name="container">The <see cref="IContainer"/> used to structure and render the page content.</param>
    /// <param name="request">The <see cref="TalentReportRequest"/> containing Page 7 data and related content.</param>
    private void ComposePage7Content(IContainer container, TalentReportRequest request)
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
                        columns.ConstantColumn(15);  // First dotted line
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
                    table.Cell().Text(request.Page7?.JobAnalysis ?? "-").FontSize(9);
                    table.Cell().AlignCenter().LineVertical(1).LineDashPattern([2f, 2f]);
                    table.Cell().AlignRight().Text(request.Page7?.JobAnalysisDate ?? "-").FontSize(9);
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
                        columns.ConstantColumn(120);  // TEST column
                        columns.ConstantColumn(15);   // Separator line
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

                    if (request.Page7?.Assessments != null && request.Page7.Assessments.Any())
                    {
                        foreach (var test in request.Page7.Assessments)
                        {
                            // Test name
                            table.Cell().Text(test.TestName ?? "-").FontSize(9);
                            // Separator line
                            table.Cell().AlignCenter().LineVertical(1).LineDashPattern([2f, 2f]);
                            // Details
                            table.Cell().Text(text =>
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
                    inner.Item().Text(request.Page7?.InputData ?? "-")
                        .FontSize(9).FontFamily("Arial");

                    if (!string.IsNullOrEmpty(request.Page7?.TemplateVersion))
                    {
                        inner.Item().PaddingTop(5).Text(request.Page7.TemplateVersion)
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

    #endregion
}