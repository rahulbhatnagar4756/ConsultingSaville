using DocumentFormat.OpenXml.ExtendedProperties;

using Library.Assess.Models.SelectionStrategyReport;

using QuestPDF.Fluent;

using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Assess.Services.SelectionStrategyReport;

public interface ISelectionStrategyReportService
{
    /// <summary>
    /// Generates a Talent Report PDF based on the provided request and returns a response object.
    /// </summary>
    /// <param name="request">The request containing all data for the Talent Report.</param>
    /// <returns>A <see cref="SelectionStrategyReportResponse"/> containing the PDF data and metadata.</returns>
    Task<SelectionStrategyReportResponse> GenerateReportAsync(SelectionStrategyReportRequest request);

    /// <summary>
    /// Generates a Talent Report PDF and returns it as a byte array.
    /// </summary>
    /// <param name="request">The request containing all data for the Talent Report.</param>
    /// <returns>A byte array representing the generated PDF file.</returns>
    Task<byte[]> GeneratePdfBytesAsync(SelectionStrategyReportRequest request);
}

public class SelectionStrategyReportService : ISelectionStrategyReportService
{
    public SelectionStrategyReportService()
    {        // Set QuestPDF license (use Community for free version)
        QuestPDF.Settings.License = LicenseType.Community;
    }

    /// <summary>
    /// Generates a Talent Report PDF based on the provided request and returns a response object.
    /// </summary>
    /// <param name="request">The request containing all data for the Talent Report.</param>
    /// <returns>A <see cref="TalentReportResponse"/> containing the PDF data and metadata.</returns>
    public async Task<SelectionStrategyReportResponse> GenerateReportAsync(SelectionStrategyReportRequest request)
    {
        try
        {
            var pdfBytes = await GeneratePdfBytesAsync(request);
            var fileName = $"SelectionStrategyReport_{request.Candidate.Replace(" ", "_")}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

            return new SelectionStrategyReportResponse
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
            return new SelectionStrategyReportResponse
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
    public async Task<byte[]> GeneratePdfBytesAsync(SelectionStrategyReportRequest request)
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
    private void ComposePage1(PageDescriptor page, SelectionStrategyReportRequest request)
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
                contentColumn.Item().PaddingTop(30).Element(c => ComposeJobAnalysisInfo(c, request));
                contentColumn.Item().PaddingTop(25).Element(c => ComposeFooterInfo(c, request));
            });

            // Push remaining space
            column.Spacing(0);

            column.Item().ExtendVertical().AlignBottom()
                .Element(c => ComposeBottomDecoration(c, request));
        });
    }
    #endregion

    #region Page 2 Methods
    /// <summary>
    /// Composes the content for Page 2 of the Talent Report PDF.
    /// </summary>
    /// <param name="page">The <see cref="PageDescriptor"/> representing the PDF page to be composed.</param>
    /// <param name="request">The <see cref="SelectionStrategyReportRequest"/> containing all data for the report.</param>
    private void ComposePage2(PageDescriptor page, SelectionStrategyReportRequest request)
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

    #endregion

    #region Section Composers

    #region Generic Header/Footer Methods from page 2
    /// <summary>
    /// Composes the header section for a PDF page, including employee information, company details, position, and company logo.
    /// </summary>
    /// <param name="container">The <see cref="IContainer"/> used to compose the header layout.</param>
    /// <param name="request">The <see cref="SelectionStrategyReportRequest"/> containing all data required for the header.</param>
    private void ComposePageHeader(IContainer container, SelectionStrategyReportRequest request)
    {
        container.Column(column =>
        {
            // Header row with text and logo
            column.Item().Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().PaddingTop(20).PaddingLeft(10).Text("Selection Strategy")
                        .FontSize(10)
                        .Bold();
                });

                row.ConstantItem(150).AlignRight().PaddingRight(15).Column(col =>
                {
                    if (!string.IsNullOrEmpty(request.CompanyLogo))
                    {
                        if (IsValidBase64(request.CompanyLogo))
                        {
                            var base64Clean = ExtractBase64FromDataUrl(request.CompanyLogo);
                            var imageBytes = Convert.FromBase64String(base64Clean);
                            col.Item().Height(30).Image(imageBytes);
                        }
                        else
                        {
                            // Fallback to file path if not base64
                            col.Item().Height(30).Image(request.CompanyLogo);
                        }
                    }
                });
            });

            // Horizontal line below header
            column.Item().PaddingTop(5).LineHorizontal(1).LineColor(Colors.Black);
        });
    }

    /// <summary>
    /// Composes the footer section for a PDF page, including footer text and page number.
    /// </summary>
    /// <param name="container">The <see cref="IContainer"/> used to compose the footer layout.</param>
    /// <param name="request">The <see cref="SelectionStrategyReportRequest"/> containing all data required for the footer.</param>
    /// <param name="pageNumber">The current page number to display in the footer.</param>
    private void ComposePageFooter(IContainer container, SelectionStrategyReportRequest request, int pageNumber)
    {
        container.BorderTop(1).BorderColor(Colors.Black).PaddingTop(5).Row(row =>
        {
            row.ConstantItem(70).AlignLeft().Text($"Selection Strategy {request.DateReport}").FontSize(8);
            row.RelativeItem().AlignCenter().PaddingLeft(85).Text($"Page {pageNumber} of 2").FontSize(8);
            row.ConstantItem(250).AlignRight().Text("Copyright 2025 | Powered by www.savilleconsulting.co.za").FontSize(8);
        });
    }
    #endregion

    #region Page 1 Methods

    /// <summary>
    /// Composes the top header section of the report page with company logo and name.
    /// </summary>
    /// <param name="container">The <see cref="IContainer"/> used to compose the header layout.</param>
    /// <param name="request">The <see cref="TalentReportRequest"/> containing company logo and name.</param>
    private void ComposeHeader(IContainer container, SelectionStrategyReportRequest request)
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
                            // Fallback to colored placeholder
                            logoRow.AutoItem().PaddingRight(12).AlignCenter()
                                .Width(200).Height(80)
                                .Background(Colors.Blue.Medium)
                                .AlignCenter().AlignMiddle()
                                .Text("COMPANY")
                                .FontSize(24).Bold().FontColor(Colors.White);
                        }
                    }
                    else
                    {
                        // Display company name as placeholder
                        logoRow.AutoItem().PaddingRight(12).AlignCenter()
                            .Width(200).Height(80)
                            .Background(Colors.Blue.Medium)
                            .AlignCenter().AlignMiddle()
                            .Text("COMPANY")
                            .FontSize(24).Bold().FontColor(Colors.White);
                    }
                });
        });
    }

    /// <summary>
    /// Composes the main report title and subtitle on Page 1.
    /// </summary>
    /// <param name="container">The <see cref="IContainer"/> used to compose the title layout.</param>
    /// <param name="request">The <see cref="TalentReportRequest"/> containing the title and subtitle.</param>
    private void ComposeTitle(IContainer container, SelectionStrategyReportRequest request)
    {
        container.Column(titleColumn =>
        {
            titleColumn.Item().Text(request.ReportTitle ?? "Selection Strategy Report")
                .FontSize(28).SemiBold().FontColor(Colors.Black);

            // Decorative line
            titleColumn.Item().PaddingTop(20)
                .Height(3)
                .Background(Colors.Blue.Medium);
        });
    }

    /// <summary>
    /// Composes the employee information section including name, company, and report date.
    /// </summary>
    /// <param name="container">The <see cref="IContainer"/> used to compose the employee info layout.</param>
    /// <param name="request">The <see cref="TalentReportRequest"/> containing employee and company details.</param>
    private void ComposeEmployeeInfo(IContainer container, SelectionStrategyReportRequest request)
    {
        container
            .Border(1).BorderColor(Colors.Grey.Lighten2)
            .Background(Colors.Grey.Lighten3)
            .Padding(15)
            .Column(infoColumn =>
            {
                infoColumn.Item().Text(request.Project ?? "Project Name")
                    .FontSize(14).SemiBold().FontColor(Colors.Black);

                infoColumn.Item().PaddingTop(8).Text(request.DateReport ?? "Date")
                    .FontSize(12).FontColor(Colors.Grey.Darken1);

                infoColumn.Item().PaddingTop(20)
                    .Height(1).Background(Colors.Grey.Lighten1);

                infoColumn.Item().PaddingTop(15).Text(request.Candidate ?? "Candidate Name")
                    .FontSize(24).SemiBold().FontColor(Colors.Black);
            });
    }

    /// <summary>
    /// Composes the footer information section on Page 1, e.g., confidentiality notice.
    /// </summary>
    /// <param name="container">The <see cref="IContainer"/> used to compose the footer layout.</param>
    /// <param name="request">The <see cref="TalentReportRequest"/> containing footer details.</param>
    private void ComposeFooterInfo(IContainer container, SelectionStrategyReportRequest request)
    {
        container.Column(footerColumn =>
        {
            footerColumn.Item()
                .Padding(12)
                .Row(row =>
                {
                    row.RelativeItem().Text("Confidential Information")
                        .FontSize(14).SemiBold().FontColor(Colors.Black);
                });
        });
    }

    /// <summary>
    /// Composes the bottom decoration for Page 1, including "Powered By" branding.
    /// </summary>
    /// <param name="container">The <see cref="IContainer"/> used to compose the bottom decoration.</param>
    /// <param name="request">The <see cref="SelectionStrategyReportRequest"/> containing branding details.</param>
    private void ComposeBottomDecoration(IContainer container, SelectionStrategyReportRequest request)
    {
        container
            .AlignRight()
            .PaddingRight(20)
            .PaddingBottom(10)
            .Width(300)
            .Height(60)
            .Background(Colors.Grey.Lighten2)
            .Padding(10)
            .AlignCenter()
            .AlignMiddle()
            .Row(row =>
            {
                row.AutoItem().AlignMiddle().Text("© POWERED BY ")
                    .FontSize(8).FontColor(Colors.Grey.Darken1);

                if (!string.IsNullOrEmpty(request.PoweredByLogo) && IsValidBase64(request.PoweredByLogo))
                {
                    try
                    {
                        var cleanBase64 = ExtractBase64FromDataUrl(request.PoweredByLogo);
                        var imageBytes = Convert.FromBase64String(cleanBase64);

                        row.AutoItem().PaddingLeft(5).Height(35).Image(imageBytes);
                    }
                    catch
                    {
                        row.AutoItem().PaddingLeft(5).AlignMiddle()
                            .Text(request.PoweredBy ?? "Company")
                            .FontSize(10).SemiBold().FontColor(Colors.Grey.Darken2);
                    }
                }
                else
                {
                    row.AutoItem().PaddingLeft(5).AlignMiddle()
                        .Text(request.PoweredBy ?? "Company")
                        .FontSize(10).SemiBold().FontColor(Colors.Grey.Darken2);
                }
            });
    }

    /// <summary>
    /// Composes the job analysis information section.
    /// </summary>
    /// <param name="container">The <see cref="IContainer"/> used to compose the job analysis layout.</param>
    /// <param name="request">The <see cref="SelectionStrategyReportRequest"/> containing job analysis details.</param>
    private void ComposeJobAnalysisInfo(IContainer container, SelectionStrategyReportRequest request)
    {
        container
            .Background(Colors.Grey.Lighten3)
            .Padding(15)
            .Column(analysisColumn =>
            {
                // Job Analysis Model Row
                analysisColumn.Item().Row(row =>
                {
                    row.RelativeItem(1).Text("Job Analysis Model:")
                        .FontSize(12).SemiBold().FontColor(Colors.Grey.Darken1);

                    row.RelativeItem(1).AlignRight().Text(request.JobAnalysis ?? "N/A")
                        .FontSize(12).FontColor(Colors.Black);
                });

                analysisColumn.Item().PaddingTop(10).Row(row =>
                {
                    row.RelativeItem(1).Text("Date of Analysis:")
                        .FontSize(12).SemiBold().FontColor(Colors.Grey.Darken1);

                    row.RelativeItem(1).AlignRight().Text(request.DateOfAnalysis ?? "N/A")
                        .FontSize(12).FontColor(Colors.Black);
                });
            });
    }

    #endregion

    #region Page 2 Methods

    private void ComposePage2Content(IContainer container, SelectionStrategyReportRequest request)
    {
        container.Column(column =>
        {
            // Main table container
            column.Item().Table(table =>
            {
                var competencies = request.SelectionStrategy.Competencies;
                var abilities = request.SelectionStrategy.Abilities;

                // Define columns with reduced widths to fit on page
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(50); // NAME (reduced from 60)
                    columns.ConstantColumn(70); // SURNAME (reduced from 80)

                    // Competency columns - reduced to 22 each
                    for (int i = 0; i < competencies.Count; i++)
                    {
                        columns.ConstantColumn(22);
                    }

                    // Ability columns - reduced to 22 each
                    for (int i = 0; i < abilities.Count; i++)
                    {
                        columns.ConstantColumn(22);
                    }

                    columns.ConstantColumn(35); // Investment (reduced from 40)
                    columns.ConstantColumn(35); // Ranking (reduced from 40)
                    columns.ConstantColumn(35); // Match (reduced from 40)
                });
                // New total: 50 + 70 + (12×22) + (3×22) + 35 + 35 + 35 = 555 points ✓

                // Header rows
                ComposeTableHeader(table, request);

                // Data rows
                ComposeTableDataRows(table, request);
            });

            // Scoring Legend
            column.Item().PaddingTop(20).Row(row =>
            {
                ComposeScoringLegend(row, request);
            });
        });
    }

    private void ComposeTableHeader(TableDescriptor table, SelectionStrategyReportRequest request)
    {
        var competencies = request.SelectionStrategy.Competencies.OrderBy(c => c.Order).ToList();
        var abilities = request.SelectionStrategy.Abilities.OrderBy(a => a.Order).ToList();

        // FIRST ROW - Main category headers with rowspan
        table.Header(header =>
        {
            // NAME - rowspan 2
            header.Cell().RowSpan(2).Border(1).BorderColor(Colors.Black)
                .AlignCenter().AlignMiddle()
                .PaddingVertical(1)
                .Text("NAME").FontSize(8).Bold();

            // SURNAME - rowspan 2
            header.Cell().RowSpan(2).Border(1).BorderColor(Colors.Black)
                .AlignCenter().AlignMiddle()
                .PaddingVertical(1)
                .Text("SURNAME").FontSize(8).Bold();

            // Competencies header - colspan for all competency columns
            header.Cell().ColumnSpan((uint)competencies.Count)
                .Border(1).BorderColor(Colors.Black)
                .Background(Colors.Grey.Lighten3)
                .AlignCenter().AlignMiddle()
                .PaddingVertical(3)
                .Text("Competencies").FontSize(11).Bold();

            // Abilities header - colspan for all ability columns
            header.Cell().ColumnSpan((uint)abilities.Count)
                .Border(1).BorderColor(Colors.Black)
                .Background(Colors.Grey.Lighten3)
                .AlignCenter().AlignMiddle()
                .PaddingVertical(3)
                .Text("Abilities").FontSize(11).Bold();

            // Investment - rowspan 2, rotated text
            header.Cell().RowSpan(2).Border(1).BorderColor(Colors.Black)
                .Background(Colors.Grey.Lighten1)
                .AlignCenter().AlignMiddle()
                .PaddingVertical(15)
                .PaddingHorizontal(3)
                .RotateLeft()
                .Text("Investment").FontSize(7).Bold();

            // Ranking - rowspan 2, rotated text
            header.Cell().RowSpan(2).Border(1).BorderColor(Colors.Black)
                .Background(Colors.Grey.Lighten1)
                .AlignCenter().AlignMiddle()
                .PaddingVertical(15)
                .PaddingHorizontal(3)
                .RotateLeft()
                .Text("Ranking").FontSize(7).Bold();

            // Match - rowspan 2, rotated text
            header.Cell().RowSpan(2).Border(1).BorderColor(Colors.Black)
                .Background(Colors.Grey.Lighten1)
                .AlignCenter().AlignMiddle()
                .PaddingVertical(15)
                .PaddingHorizontal(3)
                .RotateLeft()
                .Text("Match").FontSize(7).Bold();

            // Competency column headers (rotated)
            foreach (var competency in competencies)
            {
                header.Cell().Border(1).BorderColor(Colors.Black)
                    .Background(Colors.Grey.Lighten1)
                    .AlignCenter().AlignMiddle()
                    .PaddingVertical(20)
                    .PaddingHorizontal(4)
                    .RotateLeft()
                    .Text(competency.Name).FontSize(6).Bold();
            }

            // Ability column headers (rotated)
            foreach (var ability in abilities)
            {
                header.Cell().Border(1).BorderColor(Colors.Black)
                    .Background(Colors.Grey.Lighten1)
                    .AlignCenter().AlignMiddle()
                    .PaddingVertical(20)
                    .PaddingHorizontal(4)
                    .RotateLeft()
                    .Text(ability.Name).FontSize(6).Bold();
            }
        });
    }

    private void ComposeTableDataRows(TableDescriptor table, SelectionStrategyReportRequest request)
    {
        var candidates = request.SelectionStrategy.Candidates.OrderBy(c => c.Ranking).ToList();
        int totalRows = candidates.Count;
        int rowIndex = 0;

        foreach (var candidate in candidates)
        {
            var rowBackground = rowIndex % 2 == 0 ? Colors.White : Colors.Blue.Lighten4;
            bool isLastRow = rowIndex == totalRows - 1;
            // Competency scores
            int scoreIndex = 0;
            // NAME cell (left aligned) - hide bottom border except for last row
            table.Cell()
                .BorderLeft(1)
                .BorderTop(0)
                .BorderRight(1)
                .BorderBottom(isLastRow ? 1 : 0) // show bottom border only on last row
                .BorderColor(Colors.Black)
                .Background(rowBackground)
                .AlignLeft().AlignMiddle()
                .PaddingVertical(10)
                .PaddingHorizontal(5)
                .Text(candidate.Name).FontSize(8);

            // SURNAME cell (left aligned)
            table.Cell()
                .BorderLeft(1)
                .BorderTop(0)
                .BorderRight(1)
                .BorderBottom(isLastRow ? 1 : 0) // show bottom border only on last row
                .BorderColor(Colors.Black)
                .Background(rowBackground)
                .AlignLeft().AlignMiddle()
                .PaddingVertical(10)
                .PaddingHorizontal(5)
                .Text(candidate.Surname).FontSize(8);

            // Competency scores
            foreach (var score in candidate.CompetencyScores)
            {
                bool isFirstColumn = scoreIndex == 0;

                table.Cell()
                    .BorderLeft(isFirstColumn ? 1 : 0)  // only first column gets left border
                    .BorderTop(0)
                    .BorderRight(0)
                    .BorderBottom(isLastRow ? 1 : 0)
                    .BorderColor(Colors.Black)
                    .Background(rowBackground)
                    .PaddingHorizontal(1)
                    .PaddingTop(3)
                    .Width(20)
                    .Height(20)
                    .Background(score.ColorCode)
                    .CornerRadius(10)
                    .AlignCenter()
                    .AlignMiddle()
                    .Text(score.Rating.ToString())
                    .FontSize(9)
                    .FontColor(Colors.White)
                    .Bold();

                scoreIndex++;
            }

            // Ability scores
            foreach (var score in candidate.AbilityScores)
            {
                table.Cell().BorderLeft(0).BorderTop(0).BorderRight(0).BorderBottom(isLastRow ? 1 : 0).Background(rowBackground)
                            .PaddingHorizontal(1).PaddingTop(3)
                            .Width(20).Height(20)
                            .Background(score.ColorCode)
                            .CornerRadius(10)
                            .AlignCenter().AlignMiddle()
                            .Text(score.Rating.ToString())
                            .FontSize(9).FontColor(Colors.White).Bold();
            }

            // Investment cell as circle (H/M/L)
            var investmentColor = candidate.Investment == "H" ? "#00A859" :
                                 candidate.Investment == "M" ? "#FFA500" : "#E53935";
            table.Cell().BorderLeft(1).BorderTop(0).BorderRight(0).BorderBottom(isLastRow ? 1 : 0).Background(rowBackground)
                       .PaddingHorizontal(1).PaddingTop(3).PaddingLeft(5)
                       .Width(20).Height(20)
                       .Background(investmentColor)
                       .CornerRadius(10)
                       .AlignCenter().AlignMiddle()
                       .Text(candidate.Investment)
                       .FontSize(9).FontColor(Colors.White).Bold();

            // Ranking cell (plain text)
            table.Cell().BorderLeft(1)
                .BorderTop(0)
                .BorderRight(1)
                .BorderBottom(isLastRow ? 1 : 0) // show bottom border only on last row.BorderColor(Colors.Black)
                .Background(rowBackground)
                .AlignCenter().AlignMiddle()
                .PaddingVertical(8)
                .Text(candidate.Ranking.ToString()).FontSize(9).ExtraBold();

            // Match/Status cell (plain bold text)
            table.Cell().BorderLeft(1).BorderTop(0).BorderRight(1).BorderBottom(isLastRow ? 1 : 0).BorderColor(Colors.Black)
                .Background(rowBackground)
                .AlignCenter().AlignMiddle()
                .PaddingVertical(8)
                .Text(candidate.Status)
                .FontSize(9).Bold()
                .FontColor(candidate.Status == "GM" ? "#00A859" :
                          candidate.Status == "PM" ? "#FFA500" : Colors.Black);

            rowIndex++;
        }
    }


    private void ComposeScoringLegend(RowDescriptor row, SelectionStrategyReportRequest request)
    {
        row.RelativeItem().Column(column =>
        {
            // Title
            column.Item().PaddingBottom(5).Text("Scoring Legend").FontSize(10).Bold();

            // === Top Row: Rating label + rating blocks ===
            column.Item().Row(topRow =>
            {
                // Left "Rating" label
                topRow.ConstantItem(60)
                    .Height(20).AlignCenter()
                    .AlignMiddle()
                    .Text("Rating").FontSize(8).Bold();

                // Rating color blocks (from request)
                foreach (var rating in request.SelectionStrategy.Legend.Ratings)
                {
                    topRow.RelativeItem().Height(20)
                        .Background(rating.Color)
                        .AlignMiddle().AlignCenter()
                        .Text(rating.Rating).FontSize(7).Bold().FontColor(Colors.White);
                }
            });

            column.Item()
           .PaddingVertical(10).PaddingTop(-10)
           .LineHorizontal(2)
           .LineColor(Colors.Grey.Lighten1); // optional

            // === Second Row: Static Scale Descriptors ===
            column.Item().PaddingTop(-5).Row(descRow =>
            {
                // Left "Scale Descriptors" label
                descRow.ConstantItem(60)
                    .AlignTop().AlignCenter()
                    .Text("Scale Descriptors").FontSize(8).Bold();

                // Static text cells (not from request)
                descRow.RelativeItem().AlignTop().Text("Significant development is likely to be required.").FontSize(7).AlignCenter();
                descRow.RelativeItem().AlignTop().Text("Extensive development is likely to be required.").FontSize(7).AlignCenter();
                descRow.RelativeItem().AlignTop().Text("Some development may be required.").FontSize(7).AlignCenter();
                descRow.RelativeItem().AlignTop().Text("Competent, with little development required.").FontSize(7).AlignCenter();
                descRow.RelativeItem().AlignTop().Text("Likely strength, potential for developing and coaching others.").FontSize(7).AlignCenter();
            });

            // Gap
            column.Item().PaddingVertical(15);

            column.Item().Background("#DEE6E6").Padding(10).Row(row =>
            {
                // Title (container padding comes BEFORE Text)
                row.AutoItem()
                   .AlignMiddle()
                   .PaddingRight(12)
                   .Text("Investment")
                   .FontSize(8)
                   .Bold();

                var investments = request.SelectionStrategy.Legend.Investments;

                foreach (var inv in investments)
                {
                    // Each legend item
                    row.RelativeItem().PaddingLeft(70).Row(itemRow =>
                    {
                        // left color bar
                        itemRow.AutoItem().AlignMiddle()
                               .Height(6).Width(24)
                               .Background(inv.Color);

                        // label
                        itemRow.AutoItem().AlignMiddle()
                               .PaddingLeft(6).PaddingRight(6)
                               .Text($"{inv.Level} {inv.Description}")
                               .FontSize(6)
                               .SemiBold()
                               .FontColor(inv.Color);

                        // right color bar
                        itemRow.AutoItem().AlignMiddle()
                               .Height(6).Width(24)
                               .Background(inv.Color);
                    });
                }
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

