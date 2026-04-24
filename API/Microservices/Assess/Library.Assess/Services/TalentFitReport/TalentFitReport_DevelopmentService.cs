using Library.Assess.Models.PersonalityPDF;
using Library.Assess.Models.TalentFitReport;
using Library.Assess.Models.TalentMatchReportPDF;
using Library.Assess.Utilities;
using Library.Assess.Utilities.TalentReport;

using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;


namespace Library.Assess.Services.TalentFitReport
{
    public interface ITalentFitReport_DevelopmentService
    {
        /// <summary>
        /// Generates a Talent Fit Report PDF based on the provided request and returns a response object.
        /// </summary>
        /// <param name="request">The request containing all data for the Talent Report.</param>
        /// <returns>A <see cref="TalentFitReport_Response"/> containing the PDF data and metadata.</returns>
        Task<TalentFitReport_Response> GenerateReportAsync(TalentFitReport_DevelopmentRequest request);

        /// <summary>
        /// Generates a Talent Fit Report PDF and returns it as a byte array.
        /// </summary>
        /// <param name="request">The request containing all data for the Talent Report.</param>
        /// <returns>A byte array representing the generated PDF file.</returns>
        Task<byte[]> GeneratePdfBytesAsync(TalentFitReport_DevelopmentRequest request);
    }

    public class TalentFitReport_DevelopmentService : ITalentFitReport_DevelopmentService
    {
        public TalentFitReport_DevelopmentService()
        {        // Set QuestPDF license (use Community for free version)
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
        }

        /// <summary>
        /// Generates a Talent Fit Report PDF based on the provided request and returns a response object.
        /// </summary>
        /// <param name="request">The request containing all data for the Talent Report.</param>
        /// <returns>A <see cref="TalentFitReport_DevelopmentResponse"/> containing the PDF data and metadata.</returns>
        public async Task<TalentFitReport_Response> GenerateReportAsync(TalentFitReport_DevelopmentRequest request)
        {
            try
            {
                var pdfBytes = await GeneratePdfBytesAsync(request);
                var fileName = $"TalentFitReport_{request.EmployeeName.Replace(" ", "_")}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

                return new TalentFitReport_Response
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
                return new TalentFitReport_Response
                {
                    Success = false,
                    Message = $"Error generating report: {ex.Message}",
                    GeneratedAt = DateTime.Now
                };
            }
        }

        /// <summary>
        /// Generates a Talent Fit Report PDF and returns it as a byte array.
        /// </summary>
        /// <param name="request">The request containing all data for the Talent Report.</param>
        /// <returns>A byte array representing the generated PDF file.</returns>
        public async Task<byte[]> GeneratePdfBytesAsync(TalentFitReport_DevelopmentRequest request)
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
                    });

                    return fullDoc.GeneratePdf();
                }
                catch (Exception ex)
                {
                    throw;
                }
            });
        }

        #region Initate Methods

        #region Page 1 Methods
        /// <summary>
        /// Composes the content for Page 1 of the Talent Report PDF.
        /// </summary>
        /// <param name="page">The page descriptor representing the PDF page to be composed.</param>
        /// <param name="request">The <see cref="TalentFitReport_DevelopmentRequest"/> containing all data for the report.</param>
        private void ComposePage1(PageDescriptor page, TalentFitReport_Request request)
        {
            page.Size(PageSizes.A4);
            page.Margin(0);
            page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Neo Sans Std"));

            page.Content().Column(column =>
            {
                column.Item().Element(c => TalentReport.ComposeHeader(c, request));

                column.Item().Padding(40).Column(contentColumn =>
                {
                    contentColumn.Item().Element(c => TalentReport.ComposeTitle(c, request));
                    contentColumn.Item().PaddingTop(350).Element(c => TalentReport.ComposeEmployeeInfo(c, request));
                });

                column.Item().Element(c => TalentReport.ComposeBottomDecoration(c, request));
            });
        }
        #endregion

        #region Page 2 Methods
        private void ComposePage2(PageDescriptor page, TalentFitReport_Request request)
        {
            page.Size(PageSizes.A4);
            page.Margin(0);
            page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Neo Sans Std"));
            page.Header().ShowOnce().Element(c => TalentReport.ComposePageHeader(c, request));
            page.Footer().ShowOnce().Element(c => TalentReport.ComposePageFooter(c, request, 2));
            page.Content().PaddingHorizontal(20).PaddingVertical(15)
                .Element(c => TalentReport.ComposePage2Content(c));
        }
        #endregion

        #region Page 3 Methods
        private void ComposePage3(PageDescriptor page, TalentFitReport_Request request)
        {
            page.Size(PageSizes.A4);
            page.Margin(0);
            page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Neo Sans Std"));
            page.Header().ShowOnce().Element(c => TalentReport.ComposePageHeader(c, request));
            page.Footer().ShowOnce().Element(c => TalentReport.ComposePageFooter(c, request, 3));
            page.Content().PaddingHorizontal(20).PaddingVertical(15)
                .Element(c => TalentReport.ComposePage3Content(c, request));
        }
        #endregion

        #region Page 4 Methods
        private void ComposePage4(PageDescriptor page, TalentFitReport_Request request)
        {
            page.Size(PageSizes.A4);
            page.Margin(0);
            page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Neo Sans Std"));
            page.Header().ShowOnce().Element(c => TalentReport.ComposePageHeader(c, request));
            page.Footer().ShowOnce().Element(c => TalentReport.ComposePageFooter(c, request, 4));
            page.Content().PaddingHorizontal(20).PaddingVertical(15)
                .Element(c => TalentReport.ComposePage4Content(c, request));
        }
        #endregion

        #region Page 5 Methods
        private void ComposePage5(PageDescriptor page, TalentFitReport_Request request)
        {
            page.Size(PageSizes.A4);
            page.Margin(0);
            page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Neo Sans Std"));
            page.Header().ShowOnce().Element(c => TalentReport.ComposePageHeader(c, request));
            page.Footer().ShowOnce().Element(c => TalentReport.ComposePageFooter(c, request, 5));
            page.Content().PaddingHorizontal(20).PaddingVertical(15)
                .Element(c => TalentReport.ComposePage5Content(c, request));
        }
        #endregion

        #region Page 6 Methods
        private void ComposePage6(PageDescriptor page, TalentFitReport_DevelopmentRequest request)
        {
            page.Size(PageSizes.A4);
            page.Margin(0);
            page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Neo Sans Std"));
            page.Header().ShowOnce().Element(c => TalentReport.ComposePageHeader(c, request));
            page.Footer().ShowOnce().Element(c => TalentReport.ComposePageFooter(c, request, 6));
            page.Content().PaddingHorizontal(20).PaddingVertical(15)
                .Element(c => ComposePage6Content(c, request));
        }
        #endregion


        #region Page 7 Methods
        private void ComposePage7(PageDescriptor page, TalentFitReport_DevelopmentRequest request)
        {
            page.Size(PageSizes.A4);
            page.Margin(0);
            page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Neo Sans Std"));
            page.Header().ShowOnce().Element(c => TalentReport.ComposePageHeader(c, request));
            page.Footer().ShowOnce().Element(c => TalentReport.ComposePageFooter(c, request, 7));
            page.Content().PaddingHorizontal(20).PaddingVertical(15)
                .Element(c => ComposePage7Content(c, request));
        }
        #endregion

        #region Page 8 Methods
        private void ComposePage8(PageDescriptor page, TalentFitReport_DevelopmentRequest request)
        {
            page.Size(PageSizes.A4);
            page.Margin(0);
            page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Neo Sans Std"));
            page.Header().ShowOnce().Element(c => TalentReport.ComposePageHeader(c, request));
            page.Footer().ShowOnce().Element(c => TalentReport.ComposePageFooter(c, request, 8));
            page.Content().PaddingHorizontal(20).PaddingVertical(15)
                .Element(c => ComposePage8Content(c, request));
        }
        #endregion

        #region Page 9 Methods
        private void ComposePage9(PageDescriptor page, TalentFitReport_DevelopmentRequest request)
        {
            page.Size(PageSizes.A4);
            page.Margin(0);
            page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Neo Sans Std"));
            page.Header().ShowOnce().Element(c => TalentReport.ComposePageHeader(c, request));
            page.Footer().ShowOnce().Element(c => TalentReport.ComposePageFooter(c, request, 9));
            page.Content().PaddingHorizontal(20).PaddingVertical(15)
                .Element(c => TalentReport.ComposeLastPageContent(c, request));
        }
        #endregion


        #endregion

        #region Initiate Helper Methods   

        #region Page 6 Method
        /// <summary>
        /// Composes the main content for Page 6 of the Talent Report PDF, including full behavioural styles sections.
        /// </summary>
        /// <param name="container">The <see cref="IContainer"/> used to compose the content layout.</param>
        /// <param name="request">The <see cref="TalentFitReport_SelectionRequest"/> containing Page 4 data.</param>
        private void ComposePage6Content(IContainer container, TalentFitReport_DevelopmentRequest request)
        {
            container.AlignCenter().MaxWidth(480).Column(column =>
            {
                // Title
                column.Item().PaddingBottom(Sizing.SECTION_SPACING).Text("Full Behavioral Styles Profile")
                    .FontSize(Sizing.SECTION_TITLE_FONT_SIZE).Bold().FontColor(Colors.Black);

                // Description paragraph
                column.Item().PaddingBottom(Sizing.PARAGRAPH_SPACING).Text(text =>
                {
                    text.Span("This profile gives a detailed view of your greater and lesser preferences to execute different work-related behaviors that will influence your potential to be successful in your role. These preferences are based on your desires or motives to execute these behaviors, as well as behaviors you perceive as personal strengths.")
                        .FontSize(Sizing.BODY_TEXT_FONT_SIZE).LineHeight(Sizing.LINE_HEIGHT).FontColor(Colors.Black);
                });

                // Scale Labels Above Tables
                column.Item().PaddingBottom(Sizing.ITEM_SPACING).Row(topLabelRow =>
                {
                    topLabelRow.ConstantItem(240);
                    topLabelRow.RelativeItem().Row(labelRow =>
                    {
                        labelRow.ConstantItem(100).AlignLeft().Column(extremelyLowColumn =>
                        {
                            extremelyLowColumn.Item().AlignCenter()
                                .Text("Extremely").FontSize(Sizing.SMALL_TEXT_FONT_SIZE).Bold();
                            extremelyLowColumn.Item().AlignCenter()
                                .Text("Low").FontSize(Sizing.SMALL_TEXT_FONT_SIZE).Bold();
                        });


                        labelRow.RelativeItem().AlignCenter()
                            .Text("Average").FontSize(Sizing.SMALL_TEXT_FONT_SIZE).Bold();

                        labelRow.ConstantItem(100).AlignRight().Column(extremelyHighColumn =>
                        {
                            extremelyHighColumn.Item().AlignCenter()
                                .Text("Extremely").FontSize(Sizing.SMALL_TEXT_FONT_SIZE).Bold();
                            extremelyHighColumn.Item().AlignCenter()
                                .Text("High").FontSize(Sizing.SMALL_TEXT_FONT_SIZE).Bold();
                        });

                    });
                });


                // Problem Solving section
                if (request.Page6.ProblemSolving?.Any() == true)
                {
                    column.Item().PaddingBottom(Sizing.ITEM_SPACING).Element(c => ComposeBehaviouralSection(c, "Problem Solving",
                        request.Page6.ProblemSolving, "#6A7282"));
                }

                // Influencing People section  
                if (request.Page6.InfluencingPeople?.Any() == true)
                {
                    column.Item().PaddingBottom(Sizing.ITEM_SPACING).Element(c => ComposeBehaviouralSection(c, "Influencing People",
                        request.Page6.InfluencingPeople, "#6A7282"));
                }

                // Adapting Approaches section
                if (request.Page6.AdaptingApproaches?.Any() == true)
                {
                    column.Item().PaddingBottom(Sizing.ITEM_SPACING).Element(c => ComposeBehaviouralSection(c, "Adapting Approaches",
                        request.Page6.AdaptingApproaches, "#6A7282"));
                }

                // Delivering Success section
                if (request.Page6.DeliveringSuccess?.Any() == true)
                {
                    column.Item().Element(c => ComposeBehaviouralSection(c, "Delivering Success",
                        request.Page6.DeliveringSuccess, "#6A7282"));
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
        private void ComposeBehaviouralSection(IContainer container, string sectionTitle, List<Models.TalentFitReport.BehaviouralItem> items, string sectionColor)
        {
            container.Column(column =>
            {
                // Section with border
                column.Item().Column(sectionColumn =>
                {
                    // Section header spanning full width
                    sectionColumn.Item().PaddingTop(-2).Row(headerRow =>
                    {
                        // Section title - takes partial width
                        headerRow.ConstantItem(240).Background(sectionColor).CornerRadius(4)
                            .PaddingHorizontal(12).PaddingVertical(4)
                            .AlignMiddle()
                            .Text(sectionTitle)
                            .FontSize(Sizing.SUBSECTION_TITLE_FONT_SIZE).Bold().FontColor(Colors.White);

                        // Scale numbers 1-10 - fills remaining width
                        headerRow.RelativeItem().BorderColor(Colors.Black).Row(scaleRow =>
                        {
                            var colors = new[] {
                                "#E31D25", "#EB1C48", "#F15929", "#F59726", "#F3D520",
                                "#F3D520", "#DAE241", "#D7D820", "#8CC63E", "#39B54A"
                            };

                            for (int i = 1; i <= 10; i++)
                            {
                                scaleRow.ConstantItem(24)
                                    .Height(24)
                                    .Background(colors[i - 1])
                                    .CornerRadius(4)
                                    .AlignCenter()
                                    .AlignMiddle()
                                    .Text(i.ToString())
                                    .FontSize(Sizing.SMALL_TEXT_FONT_SIZE)
                                    .FontColor(Colors.White)
                                    .Bold();
                            }
                        });
                    });

                    // Behavior items
                    for (int index = 0; index < items.Count; index++)
                    {
                        var item = items[index];
                        bool isLast = index == items.Count - 1;

                        sectionColumn.Item().Column(borderColumn =>
                        {
                            // Content row
                            borderColumn.Item().Row(itemRow =>
                            {
                                // Behavior name and sub-behaviors
                                itemRow.ConstantItem(240).Background(Colors.White)
                                    .PaddingHorizontal(2).PaddingVertical(3)
                                    .Column(nameColumn =>
                                    {
                                        nameColumn.Item().Text(item.Name)
                                            .FontSize(Sizing.BODY_TEXT_FONT_SIZE).Bold().FontColor(Colors.Black);

                                        if (item.SubBehaviors?.Any() == true)
                                        {
                                            nameColumn.Item().Text(text =>
                                            {
                                                text.Span(string.Join(", ", item.SubBehaviors.Select(sb => $"{sb.Name} ({sb.Score})")))
                                                    .FontSize(Sizing.SMALL_TEXT_FONT_SIZE).FontColor(Colors.Grey.Darken1);
                                            });
                                        }
                                    });

                                // Score visualization
                                itemRow.RelativeItem().Background(Colors.White).Row(scoreRow =>
                                {
                                    for (int i = 1; i <= 10; i++)
                                    {
                                        var greyColumns = new int[] { 1, 2, 5, 6, 9, 10 };
                                        var cellBackgroundColor = greyColumns.Contains(i) ? "#E5E5E5" : "#FFFFFF";

                                        scoreRow.RelativeItem().Height(38)
                                            .Background(cellBackgroundColor)
                                            .AlignCenter().AlignMiddle()
                                            .Element(scoreCell =>
                                            {
                                                if (i == item.Score)
                                                {
                                                    scoreCell.Width(12).Height(12)
                                                        .Background(GetBehaviorScoreColor(item.Score))
                                                        .Border(1).BorderColor("#BDBEC0")
                                                        .CornerRadius(6);
                                                }
                                            });
                                    }
                                });
                            });

                            // Add line only if NOT last item
                            if (!isLast)
                            {
                                borderColumn.Item()
                                    .PaddingTop(-1.8f)
                                    .PaddingLeft(2f)
                                    .LineHorizontal(2f)
                                    .LineColor("#C8C8CA");
                            }

                        });
                    }


                });
            });
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

        #region Page 7 Method

        private void ComposePage7Content(IContainer container, TalentFitReport_DevelopmentRequest request)
        {
            container.AlignCenter().MaxWidth(480).PaddingTop(-10).Column(column =>
            {
                // --- Page Title ---
                column.Item()
                    .Text("Performance Enhancers / Inhibitors")
                    .FontSize(Sizing.SECTION_TITLE_FONT_SIZE)
                    .Bold();

                // --- Description Text ---
                column.Item()
                    .PaddingBottom(Sizing.PARAGRAPH_SPACING)
                    .Text(text =>
                    {
                        text.Span("Culture/Environment Fit refers to the aspects such as culture, job and environment that are likely to enhance or inhibit performance:")
                            .FontSize(Sizing.BODY_TEXT_FONT_SIZE)
                            .LineHeight(Sizing.LINE_HEIGHT);
                    });

                // ==================== PERFORMANCE ENHANCERS ====================
                column.Item().CornerRadius(4)
                    .Background(Colors.Green.Darken2)
                    .Padding(4)
                    .Text("Performance Enhancers")
                    .FontSize(Sizing.SUBSECTION_TITLE_FONT_SIZE)
                    .Bold()
                    .FontColor(Colors.White);

                if (request.Page7.PerformanceEnhancers != null && request.Page7.PerformanceEnhancers.Any())
                {
                    foreach (var enhancer in request.Page7.PerformanceEnhancers)
                    {
                        column.Item()
                            .PaddingTop(Sizing.ITEM_SPACING).Padding(2)
                            .Background(Colors.Green.Lighten4)
                            .Border(1)
                            .BorderColor(Colors.Green.Lighten2)
                            .Row(row =>
                            {
                                // Plus icon
                                row.AutoItem().PaddingLeft(1).AlignMiddle()
                                    .Width(16).Height(16)
                                    .Svg(SvgIcons.PerformanceEnhancer.Replace("{COLOR}", "#2E7D32"));
                                // Text content
                                row.RelativeItem()
                                 .PaddingLeft(8)
                                    .AlignMiddle()
                                    .PaddingVertical(4)
                                    .Text(enhancer)
                                    .FontSize(10.5f)
                                    .FontColor(Colors.Green.Darken3)
                                    .LineHeight(Sizing.LINE_HEIGHT);
                            });
                    }
                }
                // ==================== PERFORMANCE INHIBITORS ====================
                column.Item().PaddingTop(8)
                      .CornerRadius(4)
                      .Background(Colors.Red.Darken1)
                      .Padding(4)
                      .Text("Performance Inhibitors")
                      .FontSize(Sizing.SUBSECTION_TITLE_FONT_SIZE)
                      .Bold()
                      .FontColor(Colors.White);

                if (request.Page7.PerformanceInhibitors != null && request.Page7.PerformanceInhibitors.Any())
                {
                    foreach (var inhibitor in request.Page7.PerformanceInhibitors)
                    {
                        column.Item().Padding(2)
                            .PaddingTop(Sizing.ITEM_SPACING)
                            .Background(Colors.Red.Lighten4)
                            .Border(1)
                            .BorderColor(Colors.Red.Lighten2)
                            .Row(row =>
                            {
                                // Prohibition icon
                                row.AutoItem().PaddingLeft(1).AlignMiddle()
                                    .Width(16).Height(16)
                                    .Svg(SvgIcons.PerformanceInhibitor.Replace("{COLOR}", "#C62828"));
                                // Text content
                                row.RelativeItem()
                                    .PaddingLeft(8)
                                    .AlignMiddle()
                                    .PaddingVertical(4)
                                    .Text(inhibitor)
                                    .FontSize(10.5f)
                                    .FontColor(Colors.Red.Darken2)
                                    .LineHeight(Sizing.LINE_HEIGHT);
                            });
                    }
                }
            });
        }

        #endregion

        #region Page 8 Method
        private void ComposePage8Content(IContainer container, TalentFitReport_DevelopmentRequest request)
        {
            const float AxisLength = 160f; // uniform for all axes
            const int TotalBars = 10;      // number of gradient bars
            const float VerticalSpacing = 6.5f;
            const float HorizontalSpacing = 5f;

            container.AlignCenter().MaxWidth(480).Column(column =>
            {
                // ===== Header =====
                column.Item().PaddingBottom(3).Text("Team Types")
                    .FontSize(Sizing.SECTION_TITLE_FONT_SIZE).Bold();

                // ===== Team Type Title =====
                column.Item().PaddingBottom(10).AlignCenter()
                    .Text($"Your Team Type is: {request.Page8.TeamType}")
                    //.FontSize(Sizing.SECTION_TITLE_FONT_SIZE)
                    .FontSize(13)
                    .Bold()
                    .FontColor("#6A7282");

                // ===== Two Boxes (Left & Right) with Axes =====
                column.Item().PaddingBottom(10).Row(row =>
                {
                    // ===== LEFT VERTICAL AXIS: ADAPTING APPROACHES =====
                    row.Spacing(2);   //  Removes space between left axis & left box

                    row.ConstantColumn(55).Column(leftAxisCol =>
                    {
                        // Label
                        leftAxisCol.Item()
                            .Width(16)
                            .Height(180f)
                            .Background("#d7c13f")
                            .AlignCenter()
                            .AlignMiddle()
                            .RotateLeft()
                            .Text("ADAPTING APPROACHES")
                            .Bold()
                            .FontSize(10)
                            .FontColor(Colors.White);

                        // Vertical tapering bars
                        leftAxisCol.Item()
                            .PaddingTop(-180f)
                            .PaddingLeft(18)   // Reduced from 18 → 5 (closes the gap)
                            .Column(barsCol =>
                            {
                                barsCol.Spacing(5.5f);
                                int[] widths = { 20, 18, 16, 14, 12, 10, 8, 6, 4, 2 };
                                foreach (var w in widths)
                                    barsCol.Item().AlignLeft().Background("#d7c13f").Height(13).Width(w);
                            });
                    });


                    // ===== LEFT BOX (Warm Team) =====
                    row.RelativeColumn()
                        .PaddingLeft(-12)   // Slight negative padding pulls box closer
                        .Column(contentCol =>
                        {
                            // Grid
                            contentCol.Item().Row(gridRow =>
                            {
                                // Force a square container (change 180 to whatever reference size you need)
                                const float boxSize = 180f;
                                gridRow.RelativeColumn().Element(left =>
                                {
                                    left.Width(boxSize).Height(boxSize)
                                        .Element(c => CreateLeftPlusGrid(c, request.Page8.GridPositions?.WarmTeam));
                                });
                            });

                            // ===== BOTTOM HORIZONTAL AXIS: INFLUENCING PEOPLE =====
                            contentCol.Item().PaddingTop(8).PaddingLeft(-14.5f).Column(bottomAxis =>
                            {
                                float totalAxisLength = 178f; // adjusted to match boxSize
                                float barWidth = (totalAxisLength - ((TotalBars - 1) * HorizontalSpacing)) / TotalBars;
                                int[] heights = { 2, 4, 6, 8, 10, 12, 14, 16, 18, 20 };

                                // Gradient bars (Red)
                                bottomAxis.Item().Height(20).AlignCenter().Row(gradientRow =>
                                {
                                    gradientRow.Spacing(HorizontalSpacing);
                                    for (int i = 0; i < TotalBars; i++)
                                        gradientRow.ConstantItem(barWidth)
                                               .AlignBottom()
                                               .Height(heights[i])
                                               .Background("#d84a3a");
                                });

                                // Axis label (Red)
                                bottomAxis.Item().PaddingTop(3).AlignCenter()
                                      .Width(totalAxisLength)
                                      .Height(16)
                                      .Background("#d84a3a").Padding(2)
                                      .Text("INFLUENCING PEOPLE")
                                      .AlignCenter()
                                      .Bold().FontSize(10)
                                      .FontColor(Colors.White);
                            });
                        });


                    // ===== RIGHT VERTICAL AXIS: SOLVING PROBLEMS =====
                    row.ConstantColumn(55).PaddingLeft(2).Column(rightAxisCol =>
                    {
                        // Label
                        rightAxisCol.Item()
                            .Width(16)
                            .Height(180f)
                            .Background("#0ea4d6")
                            .AlignCenter()
                            .AlignMiddle()
                            .RotateLeft()
                            .Text("SOLVING PROBLEMS")
                            .Bold()
                            .FontSize(10)
                            .FontColor(Colors.White);

                        // Vertical tapering bars
                        rightAxisCol.Item().PaddingTop(-180f).PaddingLeft(18).Column(barsCol =>
                        {
                            barsCol.Spacing(5.5f);
                            int[] widths = { 20, 18, 16, 14, 12, 10, 8, 6, 4, 2 };
                            foreach (var w in widths)
                                barsCol.Item().AlignLeft().Background("#0ea4d6").Height(13).Width(w);
                        });
                    });

                    // ===== RIGHT BOX (Cool Team) =====
                    row.RelativeColumn().PaddingLeft(-12).Column(contentCol =>
                    {
                        // Grid
                        contentCol.Item().Row(gridRow =>
                        {
                            const float boxSize = 180f;
                            gridRow.RelativeColumn().Element(right =>
                            {
                                right.Width(boxSize).Height(boxSize)
                                    .Element(c => CreateRightPlusGrid(c, request.Page8.GridPositions?.CoolTeam));
                            });
                        });

                        // ===== BOTTOM HORIZONTAL AXIS: DELIVERING RESULTS =====
                        contentCol.Item().PaddingTop(8).PaddingLeft(-14.5f).Column(bottomAxis =>
                        {
                            float totalAxisLength = 178f;
                            float barWidth = (totalAxisLength - ((TotalBars - 1) * HorizontalSpacing)) / TotalBars;
                            int[] heights = { 2, 4, 6, 8, 10, 12, 14, 16, 18, 20 }; // tapering upward

                            // Gradient bars (Green)
                            bottomAxis.Item().Height(20).AlignCenter().Row(gradientRow =>
                            {
                                gradientRow.Spacing(HorizontalSpacing);
                                for (int i = 0; i < TotalBars; i++)
                                    gradientRow.ConstantItem(barWidth)
                                        .AlignBottom()
                                        .Height(heights[i])
                                        .Background("#21b15d");
                            });

                            // Axis label (Green)
                            bottomAxis.Item().PaddingTop(3).AlignCenter()
                                .Width(totalAxisLength)
                                .Height(16)
                                .Background("#21b15d").Padding(2)
                                .Text("DELIVERING RESULTS")
                                .AlignCenter()
                                .Bold().FontSize(10)
                                .FontColor(Colors.White);
                        });
                    });
                });

                // ===== Content Sections (2x4 layout) =====
                column.Item().Column(contentColumn =>
                {
                    RenderSectionRow(contentColumn,
                        "Thoughts about myself...", request.Page8.ThoughtsAboutMyself,
                        "My frustrations...", request.Page8.MyFrustrations);

                    RenderSectionRow(contentColumn,
                        "Other's thoughts about me...", request.Page8.OthersThoughtsAboutMe,
                        "Who complements me?", request.Page8.WhoComplementsMe);

                    RenderSectionRow(contentColumn,
                        "Teamwork", request.Page8.Teamwork,
                        "Leadership", request.Page8.Leadership);

                    RenderSectionRow(contentColumn,
                        "How I manage", request.Page8.HowIManage,
                        "Performing at my best", request.Page8.PerformingAtMyBest);
                });
            });
        }

        #region Design Section of 2 boxes & graph
        // ===== LEFT SIDE: Plus grid with 4 boxes =====
        private static void CreateLeftPlusGrid(IContainer container, Models.TalentFitReport.TeamGridBoxes boxes)
        {
            container.Column(col =>
            {
                // Top row
                col.Item().Row(top =>
                {
                    top.RelativeColumn().Element(c => CreateLeftTopLeftBox(c, boxes));
                    top.RelativeColumn().Element(c => CreateLeftTopRightBox(c, boxes));
                });

                // Bottom row
                col.Item().Row(bottom =>
                {
                    bottom.RelativeColumn().Element(c => CreateLeftBottomLeftBox(c, boxes));
                    bottom.RelativeColumn().Element(c => CreateLeftBottomRightBox(c, boxes));
                });
            });
        }

        // ===== RIGHT SIDE: Plus grid with 4 boxes =====
        private static void CreateRightPlusGrid(IContainer container, Models.TalentFitReport.TeamGridBoxes boxes)
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
        private static void CreateLeftTopLeftBox(IContainer container, Models.TalentFitReport.TeamGridBoxes boxes)
        {
            container
                .AspectRatio(1f)  // Makes it a perfect square
                .Border(1)
                .CornerRadiusTopLeft(4)
                .BorderColor(Colors.Grey.Lighten1)
                  .Background(boxes.TopLeft.Color)
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
                                                               .FontSize(11.5f)
                                                                .FontColor("#989898"); // Shadow color

                                                            // Main letter ON TOP
                                                            letterLayers.Layer()
                                                                .AlignCenter()
                                                                .AlignMiddle()
                                                                .Text(letter.ToString())
                                                                .Bold()
                                                                .FontSize(11)
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
        private static void CreateLeftTopRightBox(IContainer container, Models.TalentFitReport.TeamGridBoxes boxes)
        {
            container
                .AspectRatio(1f)
                .Border(1)
                .CornerRadiusTopRight(4)
                .BorderColor(Colors.Grey.Lighten1)
                  .Background(boxes.TopRight.Color)
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
                                                              .FontSize(11.5f)
                                                                .FontColor("#989898"); // Shadow color

                                                            // Main letter ON TOP
                                                            letterLayers.Layer()
                                                                .AlignCenter()
                                                                .AlignMiddle()
                                                                .Text(letter.ToString())
                                                                .Bold()
                                                                .FontSize(11)
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
        private static void CreateLeftBottomLeftBox(IContainer container, Models.TalentFitReport.TeamGridBoxes boxes)
        {
            container
                .AspectRatio(1f)
                .Border(1)
                .CornerRadiusBottomLeft(4)
                .BorderColor(Colors.Grey.Lighten1)
                 .Background(boxes.BottomLeft.Color)
                .Layers(layers =>
                {
                    layers.Layer().Element(c => CreateMeshGrid(c, 0, 4));
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
                                                               .FontSize(11.5f)
                                                                .FontColor("#989898"); // Shadow color

                                                            // Main letter ON TOP
                                                            letterLayers.Layer()
                                                                .AlignCenter()
                                                                .AlignMiddle()
                                                                .Text(letter.ToString())
                                                                .Bold()
                                                                .FontSize(11)
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
        private static void CreateLeftBottomRightBox(IContainer container, Models.TalentFitReport.TeamGridBoxes boxes)
        {
            container
                .AspectRatio(1f)
                .Border(1)
                .CornerRadiusBottomRight(4)
                .BorderColor(Colors.Grey.Lighten1)
                .Background(boxes.BottomRight.Color)
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
                                                              .FontSize(11.5f)
                                                                .FontColor("#989898"); // Shadow color

                                                            // Main letter ON TOP
                                                            letterLayers.Layer()
                                                                .AlignCenter()
                                                                .AlignMiddle()
                                                                .Text(letter.ToString())
                                                                .Bold()
                                                                .FontSize(11)
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
        private static void CreateRightTopLeftBox(IContainer container, Models.TalentFitReport.TeamGridBoxes boxes)
        {
            container
                .AspectRatio(1f)
                .Border(1)
                .CornerRadiusTopLeft(4)
                .BorderColor(Colors.Grey.Lighten1)
                .Background(boxes.TopLeft.Color)
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
                                                          .FontSize(11.5f)
                                                                .FontColor("#989898"); // Shadow color

                                                            // Main letter ON TOP
                                                            letterLayers.Layer()
                                                                .AlignCenter()
                                                                .AlignMiddle()
                                                                .Text(letter.ToString())
                                                                .Bold()
                                                                .FontSize(11)
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
        private static void CreateRightTopRightBox(IContainer container, Models.TalentFitReport.TeamGridBoxes boxes)
        {
            container
                .AspectRatio(1f)
                .Border(1)
                  .CornerRadiusTopRight(4)
                .BorderColor(Colors.Grey.Lighten1)
                .Background(boxes.TopRight.Color)
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
                                                                .FontSize(11.5f)
                                                                .FontColor("#989898"); // Shadow color

                                                            // Main letter ON TOP
                                                            letterLayers.Layer()
                                                                .AlignCenter()
                                                                .AlignMiddle()
                                                                .Text(letter.ToString())
                                                                .Bold()
                                                                .FontSize(11)
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
        private static void CreateRightBottomLeftBox(IContainer container, Models.TalentFitReport.TeamGridBoxes boxes)
        {
            container
                .AspectRatio(1f)
                .Border(1)
                .CornerRadiusBottomLeft(4)
                .BorderColor(Colors.Grey.Lighten1)
                .Background(boxes.BottomLeft.Color)
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
                                                           // Shadow/outline BEHIND each letter
                                                           letterLayers.PrimaryLayer()
                                                               .TranslateX(0f)
                                                               .TranslateY(0f)
                                                               .AlignCenter()
                                                               .AlignMiddle()
                                                               .Text(letter.ToString())
                                                               .Bold()
                                                            .FontSize(11.5f)
                                                               .FontColor("#989898"); // Shadow color

                                                           // Main letter ON TOP
                                                           letterLayers.Layer()
                                                               .AlignCenter()
                                                               .AlignMiddle()
                                                               .Text(letter.ToString())
                                                               .Bold()
                                                               .FontSize(11)
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
        private static void CreateRightBottomRightBox(IContainer container, Models.TalentFitReport.TeamGridBoxes boxes)
        {
            container
                .AspectRatio(1f)
                .Border(1)
                 .CornerRadiusBottomRight(4)
                .BorderColor(Colors.Grey.Lighten1)
                .Background(boxes.BottomRight.Color)
                .Layers(layers =>
                {
                    layers.Layer().Element(c => CreateMeshGrid(c, 1, 0));
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
                                                             .FontSize(11.5f)
                                                                .FontColor("#989898"); // Shadow color

                                                            // Main letter ON TOP
                                                            letterLayers.Layer()
                                                                .AlignCenter()
                                                                .AlignMiddle()
                                                                .Text(letter.ToString())
                                                                .Bold()
                                                                .FontSize(11)
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
                        row.Spacing(1f);

                        // Create 5 columns per row
                        for (int j = 0; j < 5; j++)
                        {
                            // Check if this is the cell to highlight
                            bool isHighlighted = (i == highlightRow && j == highlightCol);

                            row.RelativeColumn(1f).Element(cell =>
                            {
                                cell
                                    .AspectRatio(1f)
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
            column.Item().Background("#6A7282").CornerRadius(4).Padding(4)
                .Text(title).FontSize(7.5f).Bold().FontColor(Colors.White);

            // Section Items
            column.Item()
                .Background(Colors.White)
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
    }
}
