using Azure.Core;

using Library.Assess.Models.PersonalityPDF;
using Library.Assess.Models.SelectionStrategyReport;
using Library.Assess.Services.PersonalityWheel;
using Library.Assess.Utilities;

using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Assess.Services.PersonalityPDF;

public interface IPersonalityPdfService
{
    /// <summary>
    /// Generates a Personality PDF based on the provided request and returns a response object.
    /// </summary>
    /// <param name="request">The request containing all data for the Personality Pdf.</param>
    /// <returns>A <see cref="PersonalityPdfResponse"/> containing the PDF data and metadata.</returns>
    Task<PersonalityPdfResponse> GenerateReportAsync(PersonalityPdfRequest request);

    /// <summary>
    /// Generates a Personality PDF and returns it as a byte array.
    /// </summary>
    /// <param name="request">The request containing all data for the Personality Pdf.</param>
    /// <returns>A byte array representing the generated PDF file.</returns>
    Task<byte[]> GeneratePdfBytesAsync(PersonalityPdfRequest request);
}

public class PersonalityPdfService : IPersonalityPdfService
{
    private readonly IPersonalityWheelRepository _personalityWheelRepository;
    public PersonalityPdfService(IPersonalityWheelRepository personalityWheelRepository)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        var fontsPath = Path.Combine(baseDirectory, "Utilities", "fonts");

        // Register Neo Sans Std font variants
        FontManager.RegisterFont(File.OpenRead(Path.Combine(fontsPath, "Neo Sans Std Regular.otf")));
        FontManager.RegisterFont(File.OpenRead(Path.Combine(fontsPath, "Neo Sans Std Bold.otf")));
        FontManager.RegisterFont(File.OpenRead(Path.Combine(fontsPath, "Neo Sans Std Italic.otf")));
        FontManager.RegisterFont(File.OpenRead(Path.Combine(fontsPath, "Neo Sans Std Bold Italic.otf")));
        FontManager.RegisterFont(File.OpenRead(Path.Combine(fontsPath, "Neo Sans Std Black.otf")));
        FontManager.RegisterFont(File.OpenRead(Path.Combine(fontsPath, "Neo Sans Std Black Italic.otf")));
        FontManager.RegisterFont(File.OpenRead(Path.Combine(fontsPath, "Neo Sans Std Light.otf")));
        FontManager.RegisterFont(File.OpenRead(Path.Combine(fontsPath, "Neo Sans Std Light Italic.otf")));
        FontManager.RegisterFont(File.OpenRead(Path.Combine(fontsPath, "Neo Sans Std Medium.otf")));
        FontManager.RegisterFont(File.OpenRead(Path.Combine(fontsPath, "Neo Sans Std Medium Italic.otf")));
        FontManager.RegisterFont(File.OpenRead(Path.Combine(fontsPath, "Neo Sans Std Ultra.otf")));
        FontManager.RegisterFont(File.OpenRead(Path.Combine(fontsPath, "Neo Sans Std Ultra Italic.otf")));

        _personalityWheelRepository = personalityWheelRepository;
    }

    public async Task<PersonalityPdfResponse> GenerateReportAsync(PersonalityPdfRequest request)
    {
        try
        {
            var pdfBytes = await GeneratePdfBytesAsync(request);
            var fileName = $"PersonalityPdf_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

            return new PersonalityPdfResponse
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
            return new PersonalityPdfResponse
            {
                Success = false,
                Message = $"Error generating report: {ex.Message}",
                GeneratedAt = DateTime.Now
            };
        }
    }

    public async Task<byte[]> GeneratePdfBytesAsync(PersonalityPdfRequest request)
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

    #region Generic Header & Footer
    private void ComposeHeader(IContainer container, PersonalityPdfRequest request)
    {
        container.PaddingHorizontal(40).PaddingTop(10).Column(column =>
        {
            column.Item().PaddingVertical(10).LineHorizontal(2).LineColor(Colors.Blue.Medium);
            // Logo and Title section
            column.Item().Row(row =>
            {
                row.RelativeItem().AlignLeft().Height(60);

                row.ConstantItem(200).AlignRight().Column(logoColumn =>
                {
                    logoColumn.Item().AlignRight().Height(60).Element(logo =>
                    {
                        if (!string.IsNullOrEmpty(request.CompanyLogo) && IsValidBase64(request.CompanyLogo))
                        {
                            try
                            {
                                var cleanBase64 = ExtractBase64FromDataUrl(request.CompanyLogo);
                                var imageBytes = Convert.FromBase64String(cleanBase64);
                                // Changed to FitArea to prevent overflow
                                logo.Image(imageBytes, ImageScaling.FitArea);
                            }
                            catch
                            {
                                // Fallback if Base64 is invalid or conversion fails
                                logo.Width(100).Height(60)
                                    .Background(Colors.Red.Medium)
                                    .AlignCenter()
                                    .AlignMiddle()
                                    .Text("Logo").FontSize(6).FontColor(Colors.White);
                            }
                        }
                        else
                        {
                            // Fallback if no logo is provided
                            logo.Width(100).Height(60)
                                .Background(Colors.Red.Medium)
                                .AlignCenter()
                                .AlignMiddle()
                                .Text("Logo").FontSize(6).FontColor(Colors.White);
                        }
                    });
                });
            });
        });
    }

    private void ComposeFooter(IContainer container, PersonalityFooter footer, int pageNumber)
    {
        container.PaddingHorizontal(40).PaddingBottom(2).Column(column =>
        {
            column.Item().PaddingVertical(10).LineHorizontal(2).LineColor(Colors.Blue.Medium);
            column.Item().PaddingTop(-5).Row(row =>
            {
                row.ConstantItem(80).AlignLeft().Text(text =>
                {
                    text.Span($"{footer.CandidateName}\n").FontSize(8);
                    text.Span(footer.Date).FontSize(8);
                });

                row.RelativeItem().PaddingLeft(100).AlignCenter().Text(pageNumber.ToString()).FontSize(8);

                row.ConstantItem(250).AlignRight().Text($"Copyright 2025 | Powered by {footer.Copyright}").FontSize(8);
            });
        });
    }

    #endregion

    #region Page 1 Methods
    private void ComposePage1(PageDescriptor page, PersonalityPdfRequest request)
    {
        var wheelResponse = _personalityWheelRepository
            .GenerateWheelBytesAsync(request.Page1.Wheel)
            .GetAwaiter()
            .GetResult();
        page.Size(PageSizes.A4);
        page.Margin(0);
        page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Neo Sans Std")); // Use "Neo Sans Std"
        page.Header().Element(c => ComposeHeader(c, request));
        page.Content().Element(c => ComposePage1Content(c, wheelResponse));
        page.Footer().Element(c => ComposeFooter(c, request.Footer, 1));
    }
    private void ComposePage1Content(IContainer container, byte[] wheelImage)
    {
        container.PaddingHorizontal(40).PaddingVertical(20).Column(column =>
        {
            column.Item().PaddingTop(2).PaddingBottom(5).Text("Personality Profile")
               .FontSize(12).Bold();

            column.Item().PaddingBottom(2).LineHorizontal(1).LineColor(Colors.Blue.Medium);
            // Description text
            column.Item().PaddingBottom(2).Text(text =>
            {
                text.Span("Personality assessments measure how an individual would act in the workplace in relation to people with different personality types and job requirements. They will also highlight areas of competency potential, thus predicting how the individual will perform on the job. Personality assessments are psychometric in nature and therefore cannot be 'passed or failed'. The results should be interpreted in context and with a view to the application thereof. Please use the report and the contents thereof with caution and do not make any inferences or deductions that may impair your ability to make objective use of the information.")
                    .FontSize(9).LineHeight(1);
            });

            column.Item().PaddingBottom(1).Text(text =>
            {
                text.Span("Please contact the TalentSolved Assessment Unit should you have any questions relating to your individual personality assessment results. The circular profile below provides an overview of the main competency sections. The bars in the circular profile below should be interpreted using the scoring legend at the bottom of this page. The thicker grey line serves as a divider between the different clusters of the competency sections.")
                    .FontSize(9).LineHeight(1);
            });

            // Personality Wheel Image
            column.Item().PaddingVertical(1).PaddingTop(-10).AlignCenter().Width(515).Image(wheelImage);

            // Scoring Legend
            column.Item().PaddingTop(-10).Column(legendColumn =>
            {
                legendColumn.Item().PaddingBottom(1).Text("Scoring Legend")
                    .FontSize(11).FontColor("#00A9CE");

                legendColumn.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            columns.RelativeColumn();
                        }
                    });

                    // Header row with labels
                    table.Cell().Background("#E8E8E8").Padding(2).AlignCenter().Text("Exceptionally Low").FontSize(7);
                    table.Cell().Background("#D5D5E8").Padding(2).AlignCenter().Text("Moderately Low").FontSize(7);
                    table.Cell().Background("#C8C8DC").Padding(2).AlignCenter().Text("Low").FontSize(7);
                    table.Cell().Background("#BCBCD0").Padding(2).AlignCenter().Text("Moderately Low").FontSize(7);
                    table.Cell().Background("#AFAFC4").Padding(2).AlignCenter().Text("Average").FontSize(7);
                    table.Cell().Background("#9F9FB8").Padding(2).AlignCenter().Text("Average").FontSize(7);
                    table.Cell().Background("#8F8FAC").Padding(2).AlignCenter().Text("Moderately High").FontSize(7);
                    table.Cell().Background("#7F7FA0").Padding(2).AlignCenter().Text("High").FontSize(7);
                    table.Cell().Background("#6F6F94").Padding(2).AlignCenter().Text("Very High").FontSize(7);
                    table.Cell().Background("#5F5F88").Padding(2).AlignCenter().Text("Exceptionally High").FontSize(7);

                    // Number row
                    for (int i = 1; i <= 10; i++)
                    {
                        string bgColor = i switch
                        {
                            1 => "#E8E8E8",
                            2 => "#D5D5E8",
                            3 => "#C8C8DC",
                            4 => "#BCBCD0",
                            5 => "#AFAFC4",
                            6 => "#9F9FB8",
                            7 => "#8F8FAC",
                            8 => "#7F7FA0",
                            9 => "#6F6F94",
                            10 => "#5F5F88",
                            _ => "#FFFFFF"
                        };

                        table.Cell().Background(bgColor).Padding(2).AlignCenter()
                            .Text(i.ToString()).FontSize(16).Bold()
                            .FontColor(i >= 7 ? "#FFFFFF" : "#000000");
                    }
                });
            });
        });
    }

    #endregion

    #region Page 2 Methods
    private void ComposePage2(PageDescriptor page, PersonalityPdfRequest request)
    {
        page.Size(PageSizes.A4);
        page.Margin(0);
        page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Neo Sans Std"));

        page.Header().Element(c => ComposeHeader(c, request));
        page.Content().Element(c => ComposePage2Content(c, request));
        page.Footer().Element(c => ComposeFooter(c, request.Footer, 2));
    }
    private void ComposePage2Content(IContainer container, PersonalityPdfRequest request)
    {
        container.PaddingHorizontal(40).PaddingVertical(15).Column(column =>
        {
            column.Item().PaddingBottom(3).Text("Performance Enhancers/Inhibitors")
               .FontSize(11).Bold();
            column.Item().PaddingBottom(3).LineHorizontal(1).LineColor(Colors.Blue.Medium);

            // Description text
            column.Item().PaddingBottom(5).Text(text =>
            {
                text.Span("Culture/Environment Fit refers to the aspects such as culture, job and environment that are likely to enhance or inhibit performance:")
                    .FontSize(8).LineHeight(1);
            });

            // Performance Enhancers Section
            column.Item().PaddingTop(5).Background(Colors.Green.Darken2).Padding(4).Text("Performance Enhancers")
                .FontSize(9).Bold().FontColor(Colors.White);

            if (request.Page2.PerformanceEnhancers != null && request.Page2.PerformanceEnhancers.Any())
            {
                foreach (var enhancer in request.Page2.PerformanceEnhancers)
                {
                    column.Item().PaddingTop(4).PaddingHorizontal(10).Background(Colors.Green.Lighten4).Border(1)
                        .BorderColor(Colors.Green.Lighten2).Padding(5).Row(row =>
                        {
                            // Plus icon
                            row.AutoItem().AlignMiddle()
                                .Width(16).Height(16)
                                .Svg(SvgIcons.PerformanceEnhancer.Replace("{COLOR}", "#2E7D32"));

                            // Text content
                            row.RelativeItem().PaddingLeft(8).AlignMiddle().Text(enhancer)
                                .FontSize(10f).FontColor(Colors.Green.Darken3).LineHeight(1.2f);
                        });
                }
            }

            // Performance Inhibitors Section
            column.Item().PaddingTop(8).Background(Colors.Red.Darken1).Padding(4).Text("Performance Inhibitors")
                .FontSize(9).Bold().FontColor(Colors.White);

            if (request.Page2.PerformanceInhibitors != null && request.Page2.PerformanceInhibitors.Any())
            {
                foreach (var inhibitor in request.Page2.PerformanceInhibitors)
                {
                    column.Item().PaddingTop(4).PaddingHorizontal(10).Background(Colors.Red.Lighten4).Border(1)
                        .BorderColor(Colors.Red.Lighten2).Padding(5).Row(row =>
                        {
                            // Prohibition icon
                            row.AutoItem().AlignMiddle()
                                .Width(16).Height(16)
                                .Svg(SvgIcons.PerformanceInhibitor.Replace("{COLOR}", "#C62828"));

                            // Text content
                            row.RelativeItem().PaddingLeft(8).AlignMiddle().Text(inhibitor)
                                .FontSize(10f).FontColor(Colors.Red.Darken2).LineHeight(1.2f);
                        });
                }
            }
        });
    }

    #endregion

    #region Page 3 Methods
    private void ComposePage3(PageDescriptor page, PersonalityPdfRequest request)
    {
        page.Size(PageSizes.A4);
        page.Margin(0);
        page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Neo Sans Std"));

        page.Header().Element(c => ComposeHeader(c, request));
        page.Content().Element(c => ComposePage3Content(c, request));
        page.Footer().Element(c => ComposeFooter(c, request.Footer, 3));
    }
   
    private void ComposePage3Content(IContainer container, PersonalityPdfRequest request)
    {
        container.PaddingHorizontal(40).PaddingVertical(15).Column(column =>
        {
            // Header
            column.Item().PaddingBottom(3).Text("Team Types")
               .FontSize(11).Bold();
            column.Item().PaddingBottom(5).LineHorizontal(1).LineColor(Colors.Blue.Medium);

            // Team Type Title
            column.Item().PaddingBottom(10).AlignCenter().Text($"Your Team Type is: {request.Page3.TeamType}")
                .FontSize(11).Bold().FontColor(Colors.Blue.Medium);

            // ===== Two Boxes (Left & Right) with Axes =====
            column.Item().PaddingBottom(10).Row(row =>
            {
                row.Spacing(10); // Keep this spacing

                // ===== Left axis with gradient bars (ADAPTING APPROACHES - vertical) =====
                row.ConstantColumn(65).Column(leftAxisCol => // Reduced from 70
                {
                    leftAxisCol.Spacing(4); // Reduced spacing

                    // Left axis label (vertical, centered, rotated)
                    leftAxisCol.Item().PaddingRight(3).PaddingTop(8) // Reduced padding
                        .Width(16) // Reduced from 18
                        .Height(165)  // Reduced from 175
                        .Background("#d7c13f")
                        .AlignCenter()
                        .AlignMiddle()
                        .RotateLeft()
                        .Text("ADAPTING APPROACHES")
                            .Bold()
                            .FontSize(10) // Reduced from 11
                            .FontColor(Colors.White);

                    // Vertical tapering bars (top wide → bottom narrow)
                    leftAxisCol.Item().PaddingTop(-168).PaddingLeft(18).Column(barsCol => // Adjusted padding
                    {
                        barsCol.Spacing(6); // Reduced from 5-6
                        int[] widths = { 38, 34, 30, 26, 22, 18, 14, 10, 6, 2 }; // Reduced widths
                        foreach (var w in widths)
                            barsCol.Item().AlignLeft().Background("#d7c13f").Height(11).Width(w); // Reduced height from 12
                    });
                });

                // ===== Main content column =====
                row.RelativeColumn().PaddingLeft(-10).Column(contentCol =>
                {
                    // ===== Grid boxes row =====
                    contentCol.Item().Row(gridRow =>
                    {
                        // LEFT BOX (Warm Team)
                        gridRow.RelativeColumn().Element(left =>
                        {
                            left
                                .MinHeight(180) 
                                .MaxWidth(195)  
                                .Padding(6)
                                .Element(c => CreateLeftPlusGrid(c, request.Page3.GridPositions?.WarmTeam));
                        });
                    });

                    // ===== Bottom axis with gradient bar =====
                    contentCol.Item().PaddingTop(6).Column(bottomAxis => // Reduced padding
                    {
                        float barWidth = 10.5f; // Reduced from 12

                        bottomAxis.Item().Height(12).PaddingLeft(6).Row(gradientRow => // Reduced height
                        {
                            gradientRow.Spacing(7); // Reduced spacing
                            gradientRow.ConstantItem(barWidth).AlignBottom().Height(1).Background("#d84a3a");
                            gradientRow.ConstantItem(barWidth).AlignBottom().Height(2).Background("#d84a3a");
                            gradientRow.ConstantItem(barWidth).AlignBottom().Height(3).Background("#d84a3a");
                            gradientRow.ConstantItem(barWidth).AlignBottom().Height(4).Background("#d84a3a");
                            gradientRow.ConstantItem(barWidth).AlignBottom().Height(5).Background("#d84a3a");
                            gradientRow.ConstantItem(barWidth).AlignBottom().Height(6).Background("#d84a3a");
                            gradientRow.ConstantItem(barWidth).AlignBottom().Height(7).Background("#d84a3a");
                            gradientRow.ConstantItem(barWidth).AlignBottom().Height(8).Background("#d84a3a");
                            gradientRow.ConstantItem(barWidth).AlignBottom().Height(9).Background("#d84a3a");
                            gradientRow.ConstantItem(barWidth).AlignBottom().Height(10).Background("#d84a3a");
                        });

                        bottomAxis.Item().PaddingTop(3).PaddingLeft(6).Width(170).Height(16).Background("#d84a3a").Padding(2).Text("INFLUENCING PEOPLE").AlignCenter()
                            .Bold().FontSize(10).FontColor(Colors.White); // Reduced sizes
                    });
                });

                // ===== Right axis with gradient bars (ADAPTING APPROACHES - vertical) =====
                row.ConstantColumn(65).Column(leftAxisCol => // Reduced from 70
                {
                    leftAxisCol.Spacing(4);

                    leftAxisCol.Item().PaddingRight(3).PaddingTop(8)
                        .Width(16)
                        .Height(165)
                        .Background("#0ea4d6")
                        .AlignCenter()
                        .AlignMiddle()
                        .RotateLeft()
                        .Text("SOLVING PROBLEMS")
                            .Bold()
                            .FontSize(10)
                            .FontColor(Colors.White);

                    leftAxisCol.Item().PaddingTop(-168).PaddingLeft(18).Column(barsCol =>
                    {
                        barsCol.Spacing(6);
                        int[] widths = { 38, 34, 30, 26, 22, 18, 14, 10, 6, 2 }; // Reduced widths
                        foreach (var w in widths)
                            barsCol.Item().AlignLeft().Background("#0ea4d6").Height(11).Width(w);
                    });
                });

                // ===== Main content column =====
                row.RelativeColumn().PaddingLeft(-10).Column(contentCol =>
                {
                    // ===== Grid boxes row =====
                    contentCol.Item().Row(gridRow =>
                    {
                        // RIGHT BOX (Cool Team)
                        gridRow.RelativeColumn().Element(right =>
                        {
                            right
                                .MinHeight(180)  // Reduced from 180
                                .MaxWidth(195)   // Reduced from 200
                                .Padding(6)
                                .Element(c => CreateRightPlusGrid(c, request.Page3.GridPositions?.CoolTeam));
                        });
                    });

                    // ===== Bottom axis with gradient bar =====
                    contentCol.Item().PaddingTop(6).Column(bottomAxis =>
                    {
                        float barWidth = 10.5f;

                        bottomAxis.Item().Height(12).PaddingLeft(6).Row(gradientRow =>
                        {
                            gradientRow.Spacing(7);
                            gradientRow.ConstantItem(barWidth).AlignBottom().Height(1).Background("#21b15d");
                            gradientRow.ConstantItem(barWidth).AlignBottom().Height(2).Background("#21b15d");
                            gradientRow.ConstantItem(barWidth).AlignBottom().Height(3).Background("#21b15d");
                            gradientRow.ConstantItem(barWidth).AlignBottom().Height(4).Background("#21b15d");
                            gradientRow.ConstantItem(barWidth).AlignBottom().Height(5).Background("#21b15d");
                            gradientRow.ConstantItem(barWidth).AlignBottom().Height(6).Background("#21b15d");
                            gradientRow.ConstantItem(barWidth).AlignBottom().Height(7).Background("#21b15d");
                            gradientRow.ConstantItem(barWidth).AlignBottom().Height(8).Background("#21b15d");
                            gradientRow.ConstantItem(barWidth).AlignBottom().Height(9).Background("#21b15d");
                            gradientRow.ConstantItem(barWidth).AlignBottom().Height(10).Background("#21b15d");
                        });

                        bottomAxis.Item().PaddingTop(3).PaddingLeft(6).Width(170).Height(16).Background("#21b15d").Padding(2).Text("DELIVERING RESULTS").AlignCenter()
                            .Bold().FontSize(10).FontColor(Colors.White);
                    });
                });
            });

            // Content Sections (2x4 layout)
            column.Item().Column(contentColumn =>
            {
                // Row 1: Thoughts about myself & My frustrations
                RenderSectionRow(contentColumn,
                    "Thoughts about myself...", request.Page3.ThoughtsAboutMyself,
                    "My frustrations...", request.Page3.MyFrustrations);

                // Row 2: Other's thoughts & Who complements me
                RenderSectionRow(contentColumn,
                    "Other's thoughts about me...", request.Page3.OthersThoughtsAboutMe,
                    "Who complements me?", request.Page3.WhoComplementsMe);

                // Row 3: Teamwork & Leadership
                RenderSectionRow(contentColumn,
                    "Teamwork", request.Page3.Teamwork,
                    "Leadership", request.Page3.Leadership);

                // Row 4: How I manage & Performing at my best
                RenderSectionRow(contentColumn,
                    "How I manage", request.Page3.HowIManage,
                    "Performing at my best", request.Page3.PerformingAtMyBest);
            });
        });
    }

    #region Design Section of 2 boxes & graph
    // ===== LEFT SIDE: Plus grid with 4 boxes =====
    private static void CreateLeftPlusGrid(IContainer container, TeamGridBoxes boxes)
    {
        container.Column(col =>
        {
            col.Spacing(2);

            // Top row
            col.Item().Row(top =>
            {
                top.Spacing(2);
                top.RelativeColumn().Element(c => CreateLeftTopLeftBox(c, boxes));
                top.RelativeColumn().Element(c => CreateLeftTopRightBox(c, boxes));
            });

            // Bottom row
            col.Item().Row(bottom =>
            {
                bottom.Spacing(2);
                bottom.RelativeColumn().Element(c => CreateLeftBottomLeftBox(c, boxes));
                bottom.RelativeColumn().Element(c => CreateLeftBottomRightBox(c, boxes));
            });
        });
    }

    // ===== RIGHT SIDE: Plus grid with 4 boxes =====
    private static void CreateRightPlusGrid(IContainer container, TeamGridBoxes boxes)
    {
        container.Column(col =>
        {
            col.Spacing(2);

            // Top row
            col.Item().Row(top =>
            {
                top.Spacing(2);
                top.RelativeColumn().Element(c => CreateRightTopLeftBox(c, boxes));
                top.RelativeColumn().Element(c => CreateRightTopRightBox(c, boxes));
            });

            // Bottom row
            col.Item().Row(bottom =>
            {
                bottom.Spacing(2);
                bottom.RelativeColumn().Element(c => CreateRightBottomLeftBox(c, boxes));
                bottom.RelativeColumn().Element(c => CreateRightBottomRightBox(c, boxes));
            });
        });
    }

    // ========================================
    // LEFT SIDE - Individual Box Methods
    // ========================================

    // Left Grid - Top Left Box (mesh aligned bottom-right)
    private static void CreateLeftTopLeftBox(IContainer container, TeamGridBoxes boxes)
    {
        container
            .AspectRatio(1)  // Makes it a perfect square
            .Border(1)
            .CornerRadiusTopLeft(4)
            .BorderColor(Colors.Grey.Lighten1)
              .Background(boxes.TopLeft.Color)
            .PaddingLeft(6).PaddingTop(8)
            .Layers(layers =>
            {
                layers.Layer().Element(c => CreateMeshGrid(c));
                layers.PrimaryLayer()
                            .AlignCenter()
                            .AlignMiddle()
                            .Element(e =>
                            {
                                string text = boxes.TopLeft.Name ?? "";

                                if (!string.IsNullOrEmpty(text))
                                {
                                    e.ScaleToFit()
                                     .Row(row =>
                                     {
                                         row.Spacing(0); // No spacing between letters for natural flow

                                         foreach (char letter in text)
                                         {
                                             if (letter == ' ')
                                             {
                                                 row.AutoItem().Width(5);
                                             }
                                             else
                                             {
                                                 row.AutoItem()
                                                    .Layers(letterLayers =>
                                                    {
                                                        // Shadow/outline BEHIND each letter
                                                        letterLayers.PrimaryLayer()
                                                            .TranslateX(0f)
                                                            .TranslateY(0f)
                                                            .AlignCenter()
                                                            .AlignMiddle()
                                                            .Text(letter.ToString())
                                                            .Bold()
                                                            .FontSize(11)
                                                            .FontColor("#989898"); // Shadow color

                                                        // Main letter ON TOP
                                                        letterLayers.Layer()
                                                            .AlignCenter()
                                                            .AlignMiddle()
                                                            .Text(letter.ToString())
                                                            .Bold()
                                                            .FontSize(10)
                                                            .FontColor(Colors.White);
                                                    });

                                             }
                                         }
                                     });
                                }
                            });
            });
    }

    // Left Grid - Top Right Box (mesh aligned bottom-left)
    private static void CreateLeftTopRightBox(IContainer container, TeamGridBoxes boxes)
    {
        container
            .AspectRatio(1)
            .Border(1)
            .CornerRadiusTopRight(4)
            .BorderColor(Colors.Grey.Lighten1)
              .Background(boxes.TopRight.Color)
            .PaddingRight(6).PaddingTop(8)
            .Layers(layers =>
            {
                layers.Layer().Element(c => CreateMeshGrid(c));
                layers.PrimaryLayer()
                             .AlignCenter()
                             .AlignMiddle()
                             .Element(e =>
                             {
                                 string text = boxes.TopRight.Name ?? "";

                                 if (!string.IsNullOrEmpty(text))
                                 {
                                     e.ScaleToFit()
                                      .Row(row =>
                                      {
                                          row.Spacing(0); // No spacing between letters for natural flow

                                          foreach (char letter in text)
                                          {
                                              if (letter == ' ')
                                              {
                                                  row.AutoItem().Width(5);
                                              }
                                              else
                                              {
                                                  row.AutoItem()
                                                     .Layers(letterLayers =>
                                                     {
                                                         // Shadow/outline BEHIND each letter
                                                         letterLayers.PrimaryLayer()
                                                             .TranslateX(0f)
                                                             .TranslateY(0f)
                                                             .AlignCenter()
                                                             .AlignMiddle()
                                                             .Text(letter.ToString())
                                                             .Bold()
                                                             .FontSize(11)
                                                             .FontColor("#989898"); // Shadow color

                                                         // Main letter ON TOP
                                                         letterLayers.Layer()
                                                             .AlignCenter()
                                                             .AlignMiddle()
                                                             .Text(letter.ToString())
                                                             .Bold()
                                                             .FontSize(10)
                                                             .FontColor(Colors.White);
                                                     });

                                              }
                                          }
                                      });
                                 }
                             });
            });
    }

    // Left Grid - Bottom Left Box (mesh aligned top-right)
    private static void CreateLeftBottomLeftBox(IContainer container, TeamGridBoxes boxes)
    {
        container
            .AspectRatio(1)
            .Border(1)
            .CornerRadiusBottomLeft(4)
            .BorderColor(Colors.Grey.Lighten1)
             .Background(boxes.BottomLeft.Color)
             
             .PaddingLeft(6).PaddingBottom(6)
            .Layers(layers =>
            {
                layers.Layer().Element(c => CreateMeshGrid(c,0,4));
                layers.PrimaryLayer()
                            .AlignCenter()
                            .AlignMiddle()
                            .Element(e =>
                            {
                                string text = boxes.BottomLeft.Name ?? "";

                                if (!string.IsNullOrEmpty(text))
                                {
                                    e.ScaleToFit()
                                     .Row(row =>
                                     {
                                         row.Spacing(0); // No spacing between letters for natural flow

                                         foreach (char letter in text)
                                         {
                                             if (letter == ' ')
                                             {
                                                 row.AutoItem().Width(5);
                                             }
                                             else
                                             {
                                                 row.AutoItem()
                                                    .Layers(letterLayers =>
                                                    {
                                                        // Shadow/outline BEHIND each letter
                                                        letterLayers.PrimaryLayer()
                                                            .TranslateX(0f)
                                                            .TranslateY(0f)
                                                            .AlignCenter()
                                                            .AlignMiddle()
                                                            .Text(letter.ToString())
                                                            .Bold()
                                                            .FontSize(11)
                                                            .FontColor("#989898"); // Shadow color

                                                        // Main letter ON TOP
                                                        letterLayers.Layer()
                                                            .AlignCenter()
                                                            .AlignMiddle()
                                                            .Text(letter.ToString())
                                                            .Bold()
                                                            .FontSize(10)
                                                            .FontColor(Colors.White);
                                                    });

                                             }
                                         }
                                     });
                                }
                            });
            });
    }

    // Left Grid - Bottom Right Box (mesh aligned top-left)
    private static void CreateLeftBottomRightBox(IContainer container, TeamGridBoxes boxes)
    {
        container
            .AspectRatio(1)
            .Border(1)
            .CornerRadiusBottomRight(4)
            .BorderColor(Colors.Grey.Lighten1)
            .Background(boxes.BottomRight.Color)
             .PaddingRight(6).PaddingBottom(6)
            .Layers(layers =>
            {
                layers.Layer().Element(c => CreateMeshGrid(c));
                layers.PrimaryLayer()
                            .AlignCenter()
                            .AlignMiddle()
                            .Element(e =>
                            {
                                string text = boxes.BottomRight.Name ?? "";

                                if (!string.IsNullOrEmpty(text))
                                {
                                    e.ScaleToFit()
                                     .Row(row =>
                                     {
                                         row.Spacing(0); // No spacing between letters for natural flow

                                         foreach (char letter in text)
                                         {
                                             if (letter == ' ')
                                             {
                                                 row.AutoItem().Width(5);
                                             }
                                             else
                                             {
                                                 row.AutoItem()
                                                    .Layers(letterLayers =>
                                                    {
                                                        // Shadow/outline BEHIND each letter
                                                        letterLayers.PrimaryLayer()
                                                            .TranslateX(0f)
                                                            .TranslateY(0f)
                                                            .AlignCenter()
                                                            .AlignMiddle()
                                                            .Text(letter.ToString())
                                                            .Bold()
                                                            .FontSize(11)
                                                            .FontColor("#989898"); // Shadow color

                                                        // Main letter ON TOP
                                                        letterLayers.Layer()
                                                            .AlignCenter()
                                                            .AlignMiddle()
                                                            .Text(letter.ToString())
                                                            .Bold()
                                                            .FontSize(10)
                                                            .FontColor(Colors.White);
                                                    });

                                             }
                                         }
                                     });
                                }
                            });
            });
    }

    // ========================================
    // RIGHT SIDE - Individual Box Methods
    // ========================================

    // Right Grid - Top Left Box (mesh aligned bottom-right)
    private static void CreateRightTopLeftBox(IContainer container, TeamGridBoxes boxes)
    {
        container
            .AspectRatio(1)
            .Border(1)
            .CornerRadiusTopLeft(4)
            .BorderColor(Colors.Grey.Lighten1)
            .Background(boxes.TopLeft.Color)
              .PaddingLeft(6).PaddingTop(8)
            .Layers(layers =>
            {
                layers.Layer().Element(c => CreateMeshGrid(c));
                layers.PrimaryLayer()
                         .AlignCenter()
                         .AlignMiddle()
                         .Element(e =>
                         {
                             string text = boxes.TopLeft.Name ?? "";

                             if (!string.IsNullOrEmpty(text))
                             {
                                 e.ScaleToFit()
                                  .Row(row =>
                                  {
                                      row.Spacing(0); // No spacing between letters for natural flow

                                      foreach (char letter in text)
                                      {
                                          if (letter == ' ')
                                          {
                                              row.AutoItem().Width(5);
                                          }
                                          else
                                          {
                                              row.AutoItem()
                                                 .Layers(letterLayers =>
                                                 {
                                                     // Shadow/outline BEHIND each letter
                                                     letterLayers.PrimaryLayer()
                                                         .TranslateX(0f) 
                                                         .TranslateY(0f) 
                                                         .AlignCenter()
                                                         .AlignMiddle()
                                                         .Text(letter.ToString())
                                                         .Bold()
                                                         .FontSize(11)
                                                         .FontColor("#989898"); // Shadow color

                                                     // Main letter ON TOP
                                                     letterLayers.Layer()
                                                         .AlignCenter()
                                                         .AlignMiddle()
                                                         .Text(letter.ToString())
                                                         .Bold()
                                                         .FontSize(10)
                                                         .FontColor(Colors.White);
                                                 });

                                          }
                                      }
                                  });
                             }
                         });
            });
    }

    // Right Grid - Top Right Box (mesh aligned bottom-left)
    private static void CreateRightTopRightBox(IContainer container, TeamGridBoxes boxes)
    {
        container
            .AspectRatio(1)
            .Border(1)
              .CornerRadiusTopRight(4)
            .BorderColor(Colors.Grey.Lighten1)
            .Background(boxes.TopRight.Color)
          .PaddingRight(6).PaddingTop(8)
            .Layers(layers =>
            {
                layers.Layer().Element(c => CreateMeshGrid(c));
                layers.PrimaryLayer()
                         .AlignCenter()
                         .AlignMiddle()
                         .Element(e =>
                         {
                             string text = boxes.TopRight.Name ?? "";

                             if (!string.IsNullOrEmpty(text))
                             {
                                 e.ScaleToFit()
                                  .Row(row =>
                                  {
                                      row.Spacing(0); // No spacing between letters for natural flow

                                      foreach (char letter in text)
                                      {
                                          if (letter == ' ')
                                          {
                                              row.AutoItem().Width(5);
                                          }
                                          else
                                          {
                                              row.AutoItem()
                                                 .Layers(letterLayers =>
                                                 {
                                                     // Shadow/outline BEHIND each letter (using negative values)
                                                     letterLayers.PrimaryLayer()
                                                         .TranslateX(0f)  // Negative moves shadow to the left (behind)
                                                         .TranslateY(0f)  // Negative moves shadow up (behind)
                                                         .AlignCenter()
                                                         .AlignMiddle()
                                                         .Text(letter.ToString())
                                                         .Bold()
                                                         .FontSize(11)
                                                         .FontColor("#989898"); // Shadow color

                                                     // Main letter ON TOP
                                                     letterLayers.Layer()
                                                         .AlignCenter()
                                                         .AlignMiddle()
                                                         .Text(letter.ToString())
                                                         .Bold()
                                                         .FontSize(10)
                                                         .FontColor(Colors.White);
                                                 });

                                          }
                                      }
                                  });
                             }
                         });
            });
    }

    // Right Grid - Bottom Left Box (mesh aligned top-right)
    private static void CreateRightBottomLeftBox(IContainer container, TeamGridBoxes boxes)
    {
        container
            .AspectRatio(1)
            .Border(1)
            .CornerRadiusBottomLeft(4)
            .BorderColor(Colors.Grey.Lighten1)
            .Background(boxes.BottomLeft.Color)
              .PaddingLeft(6).PaddingBottom(6)
            .Layers(layers =>
            {
                layers.Layer().Element(c => CreateMeshGrid(c));
                layers.PrimaryLayer()
                         .AlignCenter()
                         .AlignMiddle()
                         .Element(e =>
                         {
                             string text = boxes.BottomLeft.Name ?? "";

                             if (!string.IsNullOrEmpty(text))
                             {
                                 e.ScaleToFit()
                                  .Row(row =>
                                  {
                                      row.Spacing(0); // No spacing between letters for natural flow

                                      foreach (char letter in text)
                                      {
                                          if (letter == ' ')
                                          {
                                              row.AutoItem().Width(5);
                                          }
                                          else
                                          {
                                              row.AutoItem()
                                                 .Layers(letterLayers =>
                                                 {
                                                     // Shadow/outline BEHIND each letter (using negative values)
                                                     letterLayers.PrimaryLayer()
                                                         .TranslateX(0f)  // Negative moves shadow to the left (behind)
                                                         .TranslateY(0f)  // Negative moves shadow up (behind)
                                                         .AlignCenter()
                                                         .AlignMiddle()
                                                         .Text(letter.ToString())
                                                         .Bold()
                                                         .FontSize(11)
                                                         .FontColor("#989898"); // Shadow color

                                                     // Main letter ON TOP
                                                     letterLayers.Layer()
                                                         .AlignCenter()
                                                         .AlignMiddle()
                                                         .Text(letter.ToString())
                                                         .Bold()
                                                         .FontSize(10)
                                                         .FontColor(Colors.White);
                                                 });

                                          }
                                      }
                                  });
                             }
                         });
            });
    }

    // Right Grid - Bottom Right Box (mesh aligned top-left)
    private static void CreateRightBottomRightBox(IContainer container, TeamGridBoxes boxes)
    {
        container
            .AspectRatio(1)
            .Border(1)
             .CornerRadiusBottomRight(4)
            .BorderColor(Colors.Grey.Lighten1)
            .Background(boxes.BottomRight.Color)
             .PaddingRight(6).PaddingBottom(6)
            .Layers(layers =>
            {
                layers.Layer().Element(c => CreateMeshGrid(c,1,0));
                layers.PrimaryLayer()
                       .AlignCenter()
                       .AlignMiddle()
                       .Element(e =>
                       {
                           string text = boxes.BottomRight.Name ?? "";
                       
                           if (!string.IsNullOrEmpty(text))
                           {
                               e.ScaleToFit()
                                .Row(row =>
                                {
                                    row.Spacing(0); // No spacing between letters for natural flow
                           
                                    foreach (char letter in text)
                                    {
                                        if (letter == ' ')
                                        {
                                            row.AutoItem().Width(5);
                                        }
                                        else
                                        {
                                            row.AutoItem()
                                               .Layers(letterLayers =>
                                               {
                                                   // Shadow/outline BEHIND each letter (using negative values)
                                                   letterLayers.PrimaryLayer()
                                                       .TranslateX(0f)  // Negative moves shadow to the left (behind)
                                                       .TranslateY(0f)  // Negative moves shadow up (behind)
                                                       .AlignCenter()
                                                       .AlignMiddle()
                                                       .Text(letter.ToString())
                                                       .Bold()
                                                       .FontSize(11)
                                                       .FontColor("#989898"); // Shadow color
                                               
                                                   // Main letter ON TOP
                                                   letterLayers.Layer()
                                                       .AlignCenter()
                                                       .AlignMiddle()
                                                       .Text(letter.ToString())
                                                       .Bold()
                                                       .FontSize(10)
                                                       .FontColor(Colors.White);
                                               });
                                        
                                        }
                                    }
                                });
                           }
                       });
            });
    }

    // ===== Shared: Creates a 5x5 mesh grid with optional color =====

    private static void CreateMeshGrid(IContainer container, int? highlightRow = null, int? highlightCol = null)
    {
        container.Column(col =>
        {
            col.Spacing(0);

            // Create 5 rows
            for (int i = 0; i < 5; i++)
            {
                col.Item().Row(row =>
                {
                    row.Spacing(1);

                    // Create 5 columns per row
                    for (int j = 0; j < 5; j++)
                    {
                        // Check if this is the cell to highlight
                        bool isHighlighted = (i == highlightRow && j == highlightCol);

                        row.RelativeColumn(1f).Element(cell =>
                        {
                            cell
                                .AspectRatio(1)
                                .Border(0.5f)
                                .BorderColor(Colors.White)
                                .Background(isHighlighted ? Colors.Grey.Medium : Colors.Transparent);
                        });
                    }
                });
            }
        });
    }

    #endregion

    #region Bottom Table Sections 2*2

    private void RenderSectionRow(ColumnDescriptor column,
    string leftTitle, List<string> leftItems,
    string rightTitle, List<string> rightItems)
    {
        column.Item().PaddingBottom(6).Row(row =>
        {
            // Left Section
            row.RelativeItem().Column(leftCol =>
            {
                RenderSectionContent(leftCol, leftTitle, leftItems);
            });

            row.ConstantItem(10); // Spacing between columns

            // Right Section
            row.RelativeItem().Column(rightCol =>
            {
                RenderSectionContent(rightCol, rightTitle, rightItems);
            });
        });
    }

    private void RenderSectionContent(ColumnDescriptor column, string title, List<string> items)
    {
        // Section Header
        column.Item().Background(Colors.Blue.Medium).Padding(4)
            .Text(title).FontSize(7.5f).Bold().FontColor(Colors.White);

        // Section Items
        column.Item()
            .Background(Colors.Grey.Lighten3)
            .Padding(6)
            .Column(itemsCol =>
            {
                if (items != null && items.Any())
                {
                    foreach (var item in items)
                    {
                        itemsCol.Item().PaddingBottom(2).Row(itemRow =>
                        {
                            itemRow.ConstantItem(8).Text("•").FontSize(6.5f);
                            itemRow.RelativeItem().Text(item).FontSize(6.5f).LineHeight(1.2f);
                        });
                    }
                }
                else
                {
                    // Add a small space for empty sections
                    itemsCol.Item().Height(10);
                }
            });
    }

    #endregion

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

