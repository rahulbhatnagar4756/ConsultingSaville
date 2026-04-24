using Library.Assess.Models.PerformanceReview;
using Library.Assess.Utilities.TalentReport;

using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Assess.Services.PerformanceReviewReports
{
    public interface IPerformanceReviewService
    {
        Task<PerformanceReviewResponse> GenerateReportAsync(PerformanceReviewRequest reviewRequest);
        Task<byte[]> GeneratePdfBytesAsync(PerformanceReviewRequest reviewRequest);
    }

    public class PerformanceReviewService : IPerformanceReviewService
    {
        public PerformanceReviewService()
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var fontsPath = Path.Combine(baseDirectory, "Utilities", "fonts");

            RegisterFont("Neo Sans Std Regular.otf", fontsPath);
            RegisterFont("Neo Sans Std Bold.otf", fontsPath);
        }

        private void RegisterFont(string fontFileName, string fontsPath)
        {
            var fontPath = Path.Combine(fontsPath, fontFileName);
            if (File.Exists(fontPath))
                FontManager.RegisterFont(File.OpenRead(fontPath));
        }

        public async Task<PerformanceReviewResponse> GenerateReportAsync(PerformanceReviewRequest reviewRequest)
        {
            var pdfBytes = await GeneratePdfBytesAsync(reviewRequest);

            return new PerformanceReviewResponse
            {
                FileName = $"Samancor_PerformanceReview_{reviewRequest.Name}.pdf",
                PdfData = pdfBytes,
                GeneratedAt = DateTime.Now,
                Success = true
            };
        }

        public async Task<byte[]> GeneratePdfBytesAsync(PerformanceReviewRequest reviewRequest)
        {
            var document = Document.Create(container =>
            {
                container.Page(page => ComposePage1(page, reviewRequest));
                container.Page(page => ComposePage2(page, reviewRequest));
                container.Page(page => ComposePage3(page, reviewRequest));
                container.Page(page => ComposePage4(page, reviewRequest));
            });

            return document.GeneratePdf();
        }

        // ================= COMMON HEADER =================

        private void ComposeCommonHeaderFirstPage(PageDescriptor page, PerformanceReviewRequest request)
        {
            page.Header()
                .PaddingBottom(0)
                .Column(column =>
                {
                    column.Item().Row(row =>
                    {
                        // Logo on left aligned, size approx 180x70 as in your code
                        row.ConstantItem(180)
                           .AlignLeft()
                           .Element(logoContainer =>
                           {
                               if (!string.IsNullOrEmpty(request.CompanyLogo))
                               {
                                   try
                                   {
                                       var cleanBase64 = TalentReport.ExtractBase64FromDataUrl(request.CompanyLogo);
                                       var logoBytes = Convert.FromBase64String(cleanBase64);

                                       logoContainer
                                           .Container()
                                           .Width(180)
                                           .Height(70)
                                           .Image(logoBytes, ImageScaling.FitArea);
                                   }
                                   catch
                                   {
                                       logoContainer
                                           .Background(Colors.Red.Medium)
                                           .AlignCenter()
                                           .AlignMiddle()
                                           .Text("Logo")
                                           .FontSize(8)
                                           .FontColor(Colors.White);
                                   }
                               }
                               else
                               {
                                   logoContainer
                                       .Background(Colors.Grey.Lighten2)
                                       .AlignCenter()
                                       .AlignMiddle()
                                       .Text("Logo")
                                       .FontSize(8)
                                       .FontColor(Colors.White);
                               }
                           });

                        // Fill rest of the space (empty)
                        row.RelativeItem();
                    });

                    // Thin horizontal line across full page width
                    column.Item()
                        .BorderBottom(1)       // 1 pt thin line, can adjust
                        .BorderColor("#0e1d40")
                        .PaddingBottom(10);
                });
        }


        private void ComposeCommonHeader(PageDescriptor page, PerformanceReviewRequest request)
        {
            page.Header()
                .PaddingBottom(10)
                .Column(column =>
                {
                    // Top border
                    column.Item()
                        .BorderBottom(3)
                        .BorderColor("#0e1d40")
                        .PaddingBottom(10);

                    // Header row
                    column.Item().Row(row =>
                    {
                        row.RelativeItem(); // empty left

                        // Logo container on right
                        row.ConstantItem(180)
                            .AlignRight()
                            .Element(logoContainer =>
                            {
                                if (!string.IsNullOrEmpty(request.CompanyLogo))
                                {
                                    try
                                    {
                                        // Extract base64 from data URL if needed
                                        var cleanBase64 = TalentReport.ExtractBase64FromDataUrl(request.CompanyLogo);
                                        var logoBytes = Convert.FromBase64String(cleanBase64);

                                        logoContainer
                                            .Container()
                                            .Width(150)  // adjust width
                                            .Height(50)  // adjust height
                                            .AlignRight()
                                            .AlignMiddle()
                                            .Image(logoBytes, ImageScaling.FitArea); // FitArea keeps aspect ratio
                                    }
                                    catch
                                    {
                                        // fallback if base64 is invalid
                                        logoContainer
                                            .Background(Colors.Red.Medium)
                                            .AlignCenter()
                                            .AlignMiddle()
                                            .Text("Logo")
                                            .FontSize(8)
                                            .FontColor(Colors.White);
                                    }
                                }
                                else
                                {
                                    // fallback if no logo
                                    logoContainer
                                        .Background(Colors.Grey.Lighten2)
                                        .AlignCenter()
                                        .AlignMiddle()
                                        .Text("Logo")
                                        .FontSize(8)
                                        .FontColor(Colors.White);
                                }
                            });
                    });
                });
        }


        // ================= COMMON FOOTER =================
        private void ComposeCommonFooter(PageDescriptor page, PerformanceReviewRequest request)
        {
            page.Footer()
                .PaddingTop(10)
                .DefaultTextStyle(x => x.FontSize(9))
                .Column(column =>
                {
                    column.Item()
                        .BorderTop(3)
                        .BorderColor("#0e1d40")
                        .PaddingTop(8);

                    column.Item().Row(row =>
                    {
                        row.RelativeItem().AlignLeft().Text(text =>
                        {
                            text.Span("Performance Review Report | ");
                            text.Span(request.Name + " ");
                            text.Line(request.TimePeriod);
                        });

                        row.RelativeItem().AlignCenter().Text(text =>
                        {
                            text.Span("Page ");
                            text.CurrentPageNumber();
                            text.Span(" of ");
                            text.TotalPages();
                        });

                        row.RelativeItem().AlignRight().Text(text =>
                        {
                            text.Span("Copyright 2021 | ");
                            text.Span("Powered by www.talentsolved.com");
                        });
                    });
                });
        }

        // ================= PAGE CONFIG =================
        private void ConfigurePage(PageDescriptor page)
        {
            page.Size(PageSizes.A4);
            page.Margin(50);

            page.DefaultTextStyle(x => x
                .FontFamily("Neo Sans Std")
                .FontSize(12));
        }

        // ================= PAGE 1 =================
        private void ComposePage1(PageDescriptor page, PerformanceReviewRequest request)
        {
            ConfigurePage(page);
            ComposeCommonHeaderFirstPage(page, request);

            page.Content()
                .Height(400) // Set fixed height for the entire content area
                .Column(column =>
                {
                    // Row takes most of the height (subtracting some space for bottom border)
                    column.Item()
                        .Height(390)  // 400 - 30 (approx for bottom border padding)
                        .Row(row =>
                        {
                            // LEFT CONTENT – 60%
                            row.RelativeItem(60)
                                .PaddingLeft(40)
                                .PaddingTop(120)
                                .PaddingRight(0)
                                .PaddingBottom(0)
                                .Column(col =>
                                {
                                    col.Item()
                                       .Text("Performance Review")
                                       .FontSize(18)
                                       .FontColor("#0e1d40");

                                    col.Item()
                                       .PaddingTop(10)
                                       .Text($"For {request.Name}")
                                       .FontSize(18)
                                       .FontColor("#0e1d40");
                                });

                            // RIGHT CONTENT – 40%
                            row.RelativeItem(40)
                                .Background(Colors.Grey.Lighten2)
                                .PaddingTop(190)
                                .Padding(20)
                                .PaddingBottom(0)
                                .Column(col =>
                                {
                                    // Align the "Private and Confidential" block to bottom inside the column
                                    col.Item()
                                       .AlignBottom()
                                       .PaddingBottom(20);

                                    col.Item()
                                       .Text("Private and Confidential")
                                       .FontSize(13)
                                       .SemiBold()
                                       .FontColor("#0e1d40");

                                    col.Item()
                                       .PaddingTop(10)
                                       .Text(DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt"))
                                       .FontSize(13)
                                       .FontColor("#0e1d40");
                                });
                        });

                    // Bottom horizontal line - separate item
                    column.Item()
                        .PaddingTop(-9)
                        .BorderBottom(1)
                        .BorderColor("#0e1d40")
                        .PaddingBottom(10);
                });

            ComposeCommonFooter(page, request);
        }



        // ================= PAGE 2 =================


        private void ComposePage2(PageDescriptor page, PerformanceReviewRequest request)
        {

            // Example: get all Pillar names from KpaKpis
            List<string> pillarNames = request.KpaKpis?
                .Where(k => !string.IsNullOrEmpty(k.Pillar)) // filter out null or empty
                .Select(k => k.Pillar!)
                .Distinct() // optional, to get unique pillar names
                .ToList() ?? new List<string>();


            ConfigurePage(page);
            ComposeCommonHeader(page, request);

            page.Content().Padding(10).Column(column =>
            {
                // Title
                column.Item().Text("Performance Review").FontSize(24).Bold();

                // Initials + Quarters
                column.Item().PaddingTop(5).Row(row =>
                {
                    string initials = string.Join("", request.Name.Split(' ').Select(n => n[0])).ToUpper();

                    // Initials box
                    row.RelativeItem(1)
                        .AlignMiddle()
                        .Text(initials)
                        .FontSize(40)
                        .Bold();

                    // Quarters
                    // =======================
                    // Quarters Section
                    // =======================

                    // Step 1: Prepare quarter values from request
                    var quarterValues = request.Quarters?.ToList() ?? new List<int>();

                    // Ensure there are 4 values (Q1-Q4)
                    while (quarterValues.Count < 4)
                        quarterValues.Add(0);

                    // Calculate YTD (average of Q1-Q4)
                    int ytd = quarterValues.Any() ? (int)Math.Round(quarterValues.Average()) : 0;

                    // Append YTD
                    quarterValues.Add(ytd);

                    // Step 2: Define quarter labels
                    string[] quartersLabels = { "Q1", "Q2", "Q3", "Q4", "YTD" };
                    int[] quartersValuesArray = quarterValues.ToArray();

                    // Step 3: Render the quarters row
                    row.RelativeItem(3).AlignMiddle().Row(quarters =>
                    {
                        for (int i = 0; i < quartersLabels.Length; i++)
                        {
                            int value = i < quartersValuesArray.Length ? quartersValuesArray[i] : 0;

                            quarters.RelativeItem(1)
                                .Padding(3)
                                .Border(1)
                                .BorderColor(Colors.Grey.Lighten2)
                                .Column(box =>
                                {
                                    // Quarter value
                                    box.Item()
                                        .PaddingTop(1)
                                        .Text(value.ToString())
                                        .Bold()
                                        .AlignCenter()
                                        .FontSize(10);

                                    // Quarter label
                                    box.Item()
                                        .Background(Colors.Grey.Lighten1)
                                        .Text(quartersLabels[i])
                                        .FontSize(8)
                                        .AlignCenter();
                                });
                        }
                    });


                });

                // Time Period
                column.Item().PaddingTop(5).Row(row =>
                {
                    row.ConstantItem(120).AlignMiddle().Text("Time Period").Bold();
                    row.RelativeItem().Text(request.TimePeriod ?? "1 January 2025 to 31 December 2025").FontSize(10).AlignLeft();
                });

                // Full Name
                column.Item().PaddingTop(2).Row(row =>
                {
                    row.ConstantItem(120).AlignMiddle().Text("Full Name").Bold();
                    row.RelativeItem().Text(request.Name).FontSize(10).AlignLeft();
                });

                // History Table
                // Inside your PDF generation method
                column.Item().PaddingTop(5).Table(table =>
                {
                    // Define columns (you can tweak relative widths)
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(2); // Date
                        columns.RelativeColumn(2); // Business Unit
                        columns.RelativeColumn(3); // Department
                        columns.RelativeColumn(3); // Position (wider for long text)
                        columns.RelativeColumn(1); // Level
                    });

                    // Header row
                    foreach (var headerText in new[] { "Date", "Business Unit", "Department", "Position", "Level" })
                    {
                        table.Cell().Element(cell =>
                        {
                            cell.Background(Colors.Grey.Lighten3)
                                .Border(1)
                                .BorderColor(Colors.Grey.Medium)
                                .PaddingVertical(5)
                                .PaddingHorizontal(3)
                                .AlignCenter()
                                .Text(headerText)
                                .Bold()
                                .FontSize(10);
                        });
                    }

                    if (request.History != null)
                    {
                        // Data rows
                        foreach (var r in request.History)
                        {
                            string formattedDate = "-";
                            if (DateTime.TryParse(r.CreateDate, out var dt))
                                formattedDate = dt.ToString("dd MMMM yyyy", CultureInfo.InvariantCulture);

                            var rowData = new string[]
                            {
                           formattedDate,
                           string.IsNullOrWhiteSpace(r.BusinessUnit) ? "-" : r.BusinessUnit,
                           string.IsNullOrWhiteSpace(r.Department) ? "-" : r.Department,
                           string.IsNullOrWhiteSpace(r.Position) ? "-" : r.Position,
                           string.IsNullOrWhiteSpace(r.Levels) ? "-" : r.Levels
                            };

                            foreach (var cellData in rowData)
                            {
                                table.Cell().Element(cell =>
                                {
                                    cell.Border(1)
                                        .BorderColor(Colors.Grey.Medium)
                                        .PaddingVertical(5)
                                        .PaddingHorizontal(3)
                                        .AlignCenter()
                                        .Text(cellData)
                                        .FontSize(10)
                                        .WrapAnywhere(); // allows word wrapping inside cells
                                });
                            }
                        }
                    }
                });
                // Pillars title
                column.Item().PaddingTop(10).Text("Pillars").FontSize(24).Bold();

                // Info Banner
                column.Item().PaddingTop(5).Row(row =>
                {
                    row.ConstantItem(25)
                        .AlignMiddle()
                        .Background(Colors.Orange.Lighten4)
                        .AlignCenter()
                        .Text("i")
                        .FontSize(14)
                        .Bold()
                        .FontColor(Colors.Orange.Darken2);

                    row.RelativeItem()
                        .PaddingLeft(5)
                        .AlignMiddle()
                        .Text($"Below you can see where {request.Name} is tracking in terms of the organization pillars.")
                        .FontColor(Colors.Orange.Darken2)
                        .FontSize(10);
                });

                // Chart grid
                column.Item().PaddingTop(5).Row(row =>
                {
                    // Y-axis labels
                    row.ConstantItem(35).Column(yAxis =>
                    {
                        for (int i = 5; i >= 0; i--)
                        {
                            yAxis.Item().Height(30).AlignMiddle().AlignRight()
                                .Text((i * 0.2m).ToString("0.0"))
                                .FontSize(8);
                        }
                    });



                    if (pillarNames.Count > 0)
                    {
                        row.RelativeItem().Column(chartColumn =>
                        {
                            int chartRows = 6;
                            int chartCols = pillarNames.Count + 1;

                            // Chart cells
                            chartColumn.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    for (int i = 0; i < chartCols; i++)
                                        columns.RelativeColumn(1);
                                });

                                for (int i = 0; i < chartCols * chartRows; i++)
                                {
                                    table.Cell().Height(30)
                                        .Border(0.5f)
                                        .Border(1)
                                        .BorderColor(Colors.Grey.Lighten1);
                                }
                            });

                            // Pillars labels (no line break)
                            chartColumn.Item()
                                .PaddingTop(-50) // move labels up
                                .Table(labelTable =>
                                {
                                    labelTable.ColumnsDefinition(columns =>
                                    {
                                        for (int i = 0; i < chartCols; i++)
                                            columns.RelativeColumn(3);
                                        labelTable.Cell();
                                    });

                                    // Empty cell for Y-axis spacer


                                    foreach (var label in pillarNames)
                                    {
                                        labelTable.Cell()
                                            .Height(65)
                                            .MinWidth(50)        // ensure text fits
                                            .AlignBottom()
                                            .AlignCenter()
                                            .Rotate(35)
                                            .Text(label)
                                            .FontSize(5.8f)        // small enough to fit one line
                                            .AlignCenter();
                                    }
                                });
                        });

                    }

                });


            });

            ComposeCommonFooter(page, request);
        }




        // ================= PAGE 3 =================

        private void ComposePage3(PageDescriptor page, PerformanceReviewRequest request)
        {
            ConfigurePage(page);
            ComposeCommonHeader(page, request);

            page.Content().Column(column =>
            {
                // Page title
                column.Item().Text("KPAs").FontSize(20).SemiBold();

                // Info row
                column.Item().PaddingTop(10).Row(row =>
                {
                    row.ConstantItem(18)
                       .AlignMiddle()
                       .Text("i")
                       .FontSize(14)
                       .FontColor(Colors.Orange.Darken2);

                    row.RelativeItem()
                       .AlignMiddle()
                       .Text($"Below you can see where {request.Name} is tracking in terms of their Performance Goals.")
                       .FontSize(10)
                       .FontColor(Colors.Orange.Darken2);
                });

                // KPAs TABLE OR EMPTY STATE
                if (request.KpaKpis != null && request.KpaKpis.Any())
                {
                    // Table container
                    column.Item()
                          .PaddingTop(10)
                          .BorderColor(Colors.Grey.Lighten2)
                          .Background(Colors.White)
                          .Element(container =>
                          {
                              container.Table(table =>
                              {
                                  // Define columns
                                  table.ColumnsDefinition(columns =>
                                  {
                                      columns.RelativeColumn(2); // Pillar
                                      columns.ConstantColumn(40); // KPA/KPI
                                      columns.RelativeColumn(3); // Name
                                      columns.ConstantColumn(40); // Q1
                                      columns.ConstantColumn(40); // Q2
                                      columns.ConstantColumn(40); // Q3
                                      columns.ConstantColumn(40); // Q4
                                  });

                                  // Table header
                                  table.Header(header =>
                                  {
                                      header.Cell().Element(c => c.Border(1).BorderColor(Colors.Grey.Lighten2).Padding(3)).Text("Pillar").Bold();
                                      header.Cell().Element(c => c.Border(1).BorderColor(Colors.Grey.Lighten2).Padding(3)).Text("").Bold();
                                      header.Cell().Element(c => c.Border(1).BorderColor(Colors.Grey.Lighten2).Padding(3)).Text("Name").Bold();
                                      header.Cell().Element(c => c.Border(1).BorderColor(Colors.Grey.Lighten2).Padding(3)).AlignCenter().Text("Q1").Bold();
                                      header.Cell().Element(c => c.Border(1).BorderColor(Colors.Grey.Lighten2).Padding(3)).AlignCenter().Text("Q2").Bold();
                                      header.Cell().Element(c => c.Border(1).BorderColor(Colors.Grey.Lighten2).Padding(3)).AlignCenter().Text("Q3").Bold();
                                      header.Cell().Element(c => c.Border(1).BorderColor(Colors.Grey.Lighten2).Padding(3)).AlignCenter().Text("Q4").Bold();
                                  });

                                  if (request.KpaKpis != null)
                                  {
                                      // Populate table dynamically
                                      foreach (var row in request.KpaKpis)
                                      {
                                          table.Cell().Element(c => c.Border(1).BorderColor(Colors.Grey.Lighten2).Padding(3).DefaultTextStyle(TextStyle.Default.FontSize(10))).Text(row.Pillar);
                                          table.Cell().Element(c => c.Border(1).BorderColor(Colors.Grey.Lighten2).Padding(3).DefaultTextStyle(TextStyle.Default.FontSize(10))).Text(row.KpaKpi);
                                          table.Cell().Element(c => c.Border(1).BorderColor(Colors.Grey.Lighten2).Padding(3).DefaultTextStyle(TextStyle.Default.FontSize(10))).Text(row.Name);
                                          table.Cell().Element(c => c.Border(1).BorderColor(Colors.Grey.Lighten2).Padding(3).DefaultTextStyle(TextStyle.Default.FontSize(10))).AlignCenter().Text(DisplayValue(row.Q1));
                                          table.Cell().Element(c => c.Border(1).BorderColor(Colors.Grey.Lighten2).Padding(3).DefaultTextStyle(TextStyle.Default.FontSize(10))).AlignCenter().Text(DisplayValue(row.Q2));
                                          table.Cell().Element(c => c.Border(1).BorderColor(Colors.Grey.Lighten2).Padding(3).DefaultTextStyle(TextStyle.Default.FontSize(10))).AlignCenter().Text(DisplayValue(row.Q3));
                                          table.Cell().Element(c => c.Border(1).BorderColor(Colors.Grey.Lighten2).Padding(3).DefaultTextStyle(TextStyle.Default.FontSize(10))).AlignCenter().Text(DisplayValue(row.Q4));
                                      }
                                  }

                              });
                          });

                }
                else
                {
                    // EMPTY STATE MESSAGE
                    column.Item()
                          .PaddingTop(25)
                          .AlignCenter()
                          .Text("No KPAs have been defined for this review period.")
                          .FontSize(11)
                          .Italic()
                          .FontColor(Colors.Grey.Darken1);
                }
            });

            ComposeCommonFooter(page, request);
        }

        string DisplayValue(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "0";

            if (!decimal.TryParse(value, out var number))
                return "0";

            return Math.Round(number).ToString("0");
        }



        private static IContainer HeaderCell(IContainer container)
        {
            return container
                .Padding(5)
                .Border(1)
                .BorderColor(Colors.Grey.Lighten2)
                .Background(Colors.Grey.Lighten3)
                .DefaultTextStyle(x => x.SemiBold().FontSize(10));
        }

        private static IContainer BodyCell(IContainer container)
        {
            return container
                .PaddingVertical(4)
                .PaddingHorizontal(6)
                .DefaultTextStyle(x => x.FontSize(9));
        }

        private static void AddRow(
            TableDescriptor table,
            string pillar,
            string type,
            string name,
            string q1,
            string q2,
            string q3,
            string q4)
        {
            string Safe(string val) => string.IsNullOrWhiteSpace(val) ? "-" : val;

            table.Cell().Element(BodyCell).Text(pillar);
            table.Cell().Element(BodyCell).Text(type);
            table.Cell().Element(BodyCell).Text(name);
            table.Cell().Element(BodyCell).AlignCenter().Text(Safe(q1));
            table.Cell().Element(BodyCell).AlignCenter().Text(Safe(q2));
            table.Cell().Element(BodyCell).AlignCenter().Text(Safe(q3));
            table.Cell().Element(BodyCell).AlignCenter().Text(Safe(q4));
        }


        // ================= PAGE 4 =================
        private void ComposePage4(PageDescriptor page, PerformanceReviewRequest request)
        {
            ConfigurePage(page);
            ComposeCommonHeader(page, request);

            page.Content().Padding(40).Column(column =>
            {
                // TITLE
                column.Item()
                    .Text("My Team’s Performance")
                    .FontSize(24)
                    .SemiBold();

                // INFO LINE
                column.Item().PaddingTop(8).Row(row =>
                {
                    row.ConstantItem(14)
                        .Text("i")
                        .FontSize(13)
                        .FontColor(Colors.Orange.Medium);

                    row.RelativeItem()
                        .Text($"Below is a summary of {request.Name} team's overall KPA's")
                        .FontSize(11)
                        .FontColor(Colors.Orange.Darken2);
                });

                column.Item().PaddingTop(25);

                // TEAM MEMBERS – dynamic only from request
                if (request.TeamMembers != null && request.TeamMembers.Any())
                {
                    foreach (var member in request.TeamMembers)
                    {
                        column.Item().PaddingTop(12).Element(c =>
                        {
                            byte[]? imageBytes = null;

                            if (!string.IsNullOrEmpty(member.ImageBase64))
                            {
                                try
                                {
                                    // Extract the base64 portion if it's a data URL
                                    var cleanBase64 = TalentReport.ExtractBase64FromDataUrl(member.ImageBase64);
                                    imageBytes = Convert.FromBase64String(cleanBase64);
                                }
                                catch
                                {
                                    // Optional: fallback if base64 is invalid
                                    imageBytes = null;
                                }
                            }

                            TeamPerformanceRow(
                                c,
                                member.Name,
                                member.Role,
                                imageBytes, // pass as byte[] for the row
                                GetRoundedScore(member.Score),
                                member.ScoreColor
                            );
                        });
                    }
                }
                else
                {
                    // EMPTY STATE MESSAGE
                    column.Item()
                        .PaddingTop(20)
                        .AlignCenter()
                        .Text("No team members have been added yet.")
                        .FontSize(12)
                        .Italic()
                        .FontColor(Colors.Grey.Darken1);
                }
            });

            ComposeCommonFooter(page, request);
        }

        private static string GetRoundedScore(string? score)
        {
            if (string.IsNullOrWhiteSpace(score))
                return "0";

            if (decimal.TryParse(score, out var value))
                return Math.Round(value, MidpointRounding.AwayFromZero)
                           .ToString("0");

            return "0";
        }



        // TeamPerformanceRow with safe null image handling
        private void TeamPerformanceRow(
            IContainer container,
            string name,
            string role,
            byte[]? imageBytes,
            string score,
            string scoreColor,
            float rowHeight = 80f
        )
        {
            container
                .Border(1)
                .BorderColor(Colors.Grey.Lighten1)
                .Height(rowHeight)
                .Row(row =>
                {
                    // =======================
                    // IMAGE – FULL CELL
                    // =======================
                    row.RelativeItem(30)
                        .Element(e =>
                        {
                            if (imageBytes != null && imageBytes.Length > 0)
                            {
                                e.Image(imageBytes, ImageScaling.FitHeight);
                            }
                            else
                            {
                                e.AlignCenter()
                                 .AlignMiddle()
                                 .Text("No Image")
                                 .FontSize(8)
                                 .FontColor(Colors.Grey.Darken1);
                            }
                        });

                    // =======================
                    // NAME + ROLE
                    // =======================
                    row.RelativeItem(65)
                        .PaddingVertical(12)
                        .PaddingHorizontal(10)
                        .Column(col =>
                        {
                            col.Item()
                                .Text(name)
                                .FontSize(12)
                                .SemiBold();

                            col.Item()
                                .PaddingTop(4)
                                .Text(role)
                                .FontSize(10)
                                .FontColor(Colors.Grey.Darken1);
                        });

                    // =======================
                    // SCORE
                    // =======================
                    row.RelativeItem(25)
                   .Border(1)
                   .BorderColor(Colors.Grey.Darken1)
                   .Background(GetScoreBackgroundColor(score))
                   .AlignCenter()
                   .AlignMiddle()
                   .Text(score)
                   .FontSize(22)
                   .SemiBold()
                   .FontColor(Colors.Black);
                });
        }


        private Color GetScoreBackgroundColor(string score)
        {
            if (!int.TryParse(score, out int value))
                return Colors.Grey.Lighten2;   // fallback

            if (value >= 85)
                return Colors.Green.Lighten1;

            if (value >= 70)
                return Colors.Red.Darken1;

            if (value >= 50)
                return Colors.Yellow.Lighten2;

            return Colors.White;
        }








    }
}
