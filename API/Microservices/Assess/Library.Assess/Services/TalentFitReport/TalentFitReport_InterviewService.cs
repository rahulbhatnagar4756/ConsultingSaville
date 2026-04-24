using Library.Assess.Models.TalentFitReport;
using Library.Assess.Models.TalentMatchSelectionReportPDF;
using Library.Assess.Utilities;
using Library.Assess.Utilities.TalentReport;

using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Library.Assess.Services.TalentFitReport
{
    public interface ITalentFitReport_InterviewService
    {
        /// <summary>
        /// Generates a Talent Fit Report PDF based on the provided request and returns a response object.
        /// </summary>
        /// <param name="request">The request containing all data for the Talent Report.</param>
        /// <returns>A <see cref="TalentFitReport_Response"/> containing the PDF data and metadata.</returns>
        Task<TalentFitReport_Response> GenerateReportAsync(TalentFitReport_InterviewRequest request);

        /// <summary>
        /// Generates a Talent Fit Report PDF and returns it as a byte array.
        /// </summary>
        /// <param name="request">The request containing all data for the Talent Report.</param>
        /// <returns>A byte array representing the generated PDF file.</returns>
        Task<byte[]> GeneratePdfBytesAsync(TalentFitReport_InterviewRequest request);
    }


    public class TalentFitReport_InterviewService : ITalentFitReport_InterviewService
    {

        public TalentFitReport_InterviewService()
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
        public async Task<TalentFitReport_Response> GenerateReportAsync(TalentFitReport_InterviewRequest request)
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
        public async Task<byte[]> GeneratePdfBytesAsync(TalentFitReport_InterviewRequest request)
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
                        container.Page(page => ComposePage17(page, request));
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
        private void ComposePage6(PageDescriptor page, TalentFitReport_Request request)
        {
            page.Size(PageSizes.A4);
            page.Margin(0);
            page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Neo Sans Std"));
            page.Header().ShowOnce().Element(c => TalentReport.ComposePageHeader(c, request));
            page.Footer().ShowOnce().Element(c => TalentReport.ComposePageFooter(c, request, 6));
            page.Content().PaddingHorizontal(20).PaddingVertical(15)
                .Element(c => ComposePage6Content(c));
        }
        #endregion

        #region Page 7 Methods
        private void ComposePage7(PageDescriptor page, TalentFitReport_InterviewRequest request)
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
        private void ComposePage8(PageDescriptor page, TalentFitReport_InterviewRequest request)
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
        private void ComposePage9(PageDescriptor page, TalentFitReport_InterviewRequest request)
        {
            page.Size(PageSizes.A4);
            page.Margin(0);
            page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Neo Sans Std"));
            page.Header().ShowOnce().Element(c => TalentReport.ComposePageHeader(c, request));
            page.Footer().ShowOnce().Element(c => TalentReport.ComposePageFooter(c, request, 9));
            page.Content().PaddingHorizontal(20).PaddingVertical(15)
                .Element(c => ComposePage9Content(c, request));
        }
        #endregion

        #region Page 10 Methods
        private void ComposePage10(PageDescriptor page, TalentFitReport_InterviewRequest request)
        {
            page.Size(PageSizes.A4);
            page.Margin(0);
            page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Neo Sans Std"));
            page.Header().ShowOnce().Element(c => TalentReport.ComposePageHeader(c, request));
            page.Footer().ShowOnce().Element(c => TalentReport.ComposePageFooter(c, request, 10));
            page.Content().PaddingHorizontal(20).PaddingVertical(15)
                .Element(c => ComposePage10Content(c, request));
        }
        #endregion

        #region Page 11 Methods
        private void ComposePage11(PageDescriptor page, TalentFitReport_InterviewRequest request)
        {
            page.Size(PageSizes.A4);
            page.Margin(0);
            page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Neo Sans Std"));
            page.Header().ShowOnce().Element(c => TalentReport.ComposePageHeader(c, request));
            page.Footer().ShowOnce().Element(c => TalentReport.ComposePageFooter(c, request, 11));
            page.Content().PaddingHorizontal(20).PaddingVertical(15)
                .Element(c => ComposePage11Content(c, request));
        }
        #endregion

        #region Page 12 Methods
        private void ComposePage12(PageDescriptor page, TalentFitReport_InterviewRequest request)
        {
            page.Size(PageSizes.A4);
            page.Margin(0);
            page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Neo Sans Std"));
            page.Header().ShowOnce().Element(c => TalentReport.ComposePageHeader(c, request));
            page.Footer().ShowOnce().Element(c => TalentReport.ComposePageFooter(c, request, 12));
            page.Content().PaddingHorizontal(20).PaddingVertical(15)
                .Element(c => ComposePage12Content(c, request));
        }
        #endregion

        #region Page 13 Methods
        private void ComposePage13(PageDescriptor page, TalentFitReport_InterviewRequest request)
        {
            page.Size(PageSizes.A4);
            page.Margin(0);
            page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Neo Sans Std"));
            page.Header().ShowOnce().Element(c => TalentReport.ComposePageHeader(c, request));
            page.Footer().ShowOnce().Element(c => TalentReport.ComposePageFooter(c, request, 13));
            page.Content().PaddingHorizontal(20).PaddingVertical(15)
                .Element(c => ComposePage13Content(c, request));
        }
        #endregion

        #region Page 14 Methods
        private void ComposePage14(PageDescriptor page, TalentFitReport_InterviewRequest request)
        {
            page.Size(PageSizes.A4);
            page.Margin(0);
            page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Neo Sans Std"));
            page.Header().ShowOnce().Element(c => TalentReport.ComposePageHeader(c, request));
            page.Footer().ShowOnce().Element(c => TalentReport.ComposePageFooter(c, request, 14));
            page.Content().PaddingHorizontal(20).PaddingVertical(15)
                .Element(c => ComposePage14Content(c, request));
        }
        #endregion

        #region Page 15 Methods
        private void ComposePage15(PageDescriptor page, TalentFitReport_InterviewRequest request)
        {
            page.Size(PageSizes.A4);
            page.Margin(0);
            page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Neo Sans Std"));
            page.Header().ShowOnce().Element(c => TalentReport.ComposePageHeader(c, request));
            page.Footer().ShowOnce().Element(c => TalentReport.ComposePageFooter(c, request, 15));
            page.Content().PaddingHorizontal(20).PaddingVertical(15)
                .Element(c => ComposePage15Content(c, request));
        }
        #endregion

        #region Page 16 Methods
        private void ComposePage16(PageDescriptor page, TalentFitReport_InterviewRequest request)
        {
            page.Size(PageSizes.A4);
            page.Margin(0);
            page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Neo Sans Std"));
            page.Header().ShowOnce().Element(c => TalentReport.ComposePageHeader(c, request));
            page.Footer().ShowOnce().Element(c => TalentReport.ComposePageFooter(c, request, 16));
            page.Content().PaddingHorizontal(20).PaddingVertical(15)
                .Element(c => ComposePage16Content(c, request));
        }
        #endregion

        #region Page 17 Methods
        private void ComposePage17(PageDescriptor page, TalentFitReport_Request request)
        {
            page.Size(PageSizes.A4);
            page.Margin(0);
            page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Neo Sans Std"));
            page.Header().ShowOnce().Element(c => TalentReport.ComposePageHeader(c, request));
            page.Footer().ShowOnce().Element(c => TalentReport.ComposePageFooter(c, request, 17));
            page.Content().PaddingHorizontal(20).PaddingVertical(15)
                .Element(c => TalentReport.ComposeLastPageContent(c, request));
        }
        #endregion


        #endregion


        #region Page 6 Methods

        private void ComposePage6Content(IContainer container)
        {
            container
                .PaddingVertical(15)
                .PaddingHorizontal(25)
                .AlignCenter()
                .MaxWidth(480)
                .Column(column =>
                {
                    column.Spacing(8);

                    // ====== Header ======
                    column.Item().Text(t =>
                    {
                        t.Span("HOW TO USE THIS ").FontSize(Sizing.SUBSECTION_TITLE_FONT_SIZE).FontColor("#666666");
                        t.Span("INTERVIEW GUIDE:").FontSize(Sizing.SUBSECTION_TITLE_FONT_SIZE).Bold().FontColor("#000000");
                    });

                    column.Item().PaddingTop(2).Text("This interview guide contains competency-based questions to guide the interview process. Below is an illustration of the process to follow when using this guide.")
                        .FontSize(Sizing.BODY_TEXT_FONT_SIZE).FontColor(Colors.Black).LineHeight(1.2f);

                    // ====== Main Content with Two Columns ======
                    column.Item().PaddingTop(8).Row(row =>
                    {
                        // Left Column - Steps
                        row.RelativeItem(1).PaddingLeft(11).Column(leftCol =>
                        {
                            leftCol.Spacing(4);

                            // Step 1
                            AddStepCompact(leftCol, 1, "REVIEW JOB DESCRIPTION AND / OR PERSON SPECIFICATION", "#E74C3C", SvgIcons.Briefcase);
                            AddDownArrowCompact(leftCol);

                            // Step 2
                            AddStepCompact(leftCol, 2, "SELECT QUESTIONS FROM THOSE PROVIDED IN EACH CATEGORY", "#E74C3C", SvgIcons.QuestionMark);
                            AddDownArrowCompact(leftCol);

                            // Step 3
                            AddStepCompact(leftCol, 3, "CONDUCT INTERVIEW", "#f1592a", SvgIcons.Users);
                            AddDownArrowCompact(leftCol);

                            // Step 4
                            AddStepCompact(leftCol, 4, "RECORD RESPONSES", "#f59726", SvgIcons.ThinkIcon);
                            AddDownArrowCompact(leftCol);

                            // Step 5
                            AddStepCompact(leftCol, 5, "SCORE AND EVALUATE DATA", "#edd623", SvgIcons.Search);
                            AddDownArrowCompact(leftCol);
                            // Sub-bullets for Step 5
                            leftCol.Item().PaddingTop(-40).PaddingLeft(67).Column(subBullets =>
                            {
                                subBullets.Spacing(2);
                                AddSubBullet(subBullets, "Use the 10-point rating scale consistently, basing every score on clear behavioral evidence from the interview.");
                                AddSubBullet(subBullets, "Align ratings to the critical job requirements, not to other applicants, to reduce subjectivity and bias.");
                                AddSubBullet(subBullets, "Discuss scores openly as a panel to reach a fair, defensible consensus on each competency.");
                            });


                            // Step 6
                            AddStepCompact(leftCol, 6, "SUMMARISE SCORES", "#dae146", SvgIcons.Percentage);
                            AddDownArrowCompact(leftCol);

                            // Step 7
                            AddStepCompact(leftCol, 7, "MAKE RECOMMENDATIONS", "#8bc73c", SvgIcons.Star);
                        });
                        row.AutoItem().PaddingLeft(10).PaddingHorizontal(10).LineVertical(2).LineColor(Colors.Grey.Lighten1);
                        // Right Column - Guidelines
                        row.RelativeItem(1).PaddingLeft(10).PaddingRight(30).Column(rightCol =>
                        {
                            // How to Ask Good Questions
                            rightCol.Item().Text(t =>
                            {
                                t.Span("HOW TO ASK ").FontSize(Sizing.SUBSECTION_TITLE_FONT_SIZE).FontColor("#666666");
                                t.Span("GOOD INTERVIEW QUESTIONS:").FontSize(Sizing.SUBSECTION_TITLE_FONT_SIZE).Bold().FontColor("#000000");
                            });

                            rightCol.Item().PaddingTop(3).Column(bullets =>
                            {
                                bullets.Spacing(2);
                                AddBulletPoint(bullets, "Ask open-ended questions (how?, why?, describe, tell me about).");
                                AddBulletPoint(bullets, "Ask for practical, real-life examples.");
                                AddBulletPoint(bullets, "Always probe to discover more information and do not let the interview structure limit your probing.");
                                AddBulletPoint(bullets, "Avoid asking improper questions by focusing on the inherent job requirements.");
                                AddBulletPoint(bullets, "If the candidate strays off the subject, redirect as quickly as possible.");
                                AddBulletPoint(bullets, "Paraphrase the candidate's answer to show that you have listened.");
                            });

                            // Points to Remember
                            rightCol.Item().PaddingTop(30).Text(t =>
                            {
                                t.Span("POINTS TO ").FontSize(Sizing.SUBSECTION_TITLE_FONT_SIZE).FontColor("#666666");
                                t.Span("REMEMBER:").FontSize(Sizing.SUBSECTION_TITLE_FONT_SIZE).Bold().FontColor("#000000");
                            });

                            rightCol.Item().PaddingTop(3).Column(points =>
                            {
                                points.Spacing(2);
                                AddBulletPoint(points, "Do not rush to a decision or judgment.");
                                AddBulletPoint(points, "Do not make binding contractual statements during the interview.");
                                AddBulletPoint(points, "Remain professional but open and welcoming.");
                                AddBulletPoint(points, "\"Hire for fit, train for skills\"");
                            });
                        });
                    });
                });
        }

        // ====== Helper Methods ======
        private void AddStepCompact(ColumnDescriptor column, int stepNumber, string title, string color, string icon)
        {
            column.Item().Row(r =>
            {
                r.AutoItem().Width(38).Height(38).Border(2).BorderColor("#CCCCCC")
                    .CornerRadius(19).Background("#FFFFFF")
                    .AlignCenter().AlignMiddle()
                    .Container().Width(18).Height(18).AlignCenter().AlignMiddle()
                    .Svg(icon.Replace("{COLOR}", color));

                r.RelativeItem().PaddingLeft(8).AlignMiddle().Column(textCol =>
                {
                    textCol.Item().Row(inner =>
                    {
                        inner.AutoItem().PaddingTop(-2).Text(stepNumber.ToString()).FontSize(20).Bold().FontColor(color);
                        inner.RelativeItem().PaddingLeft(8).Text(title).FontSize(Sizing.BODY_TEXT_FONT_SIZE).FontColor("#000000").LineHeight(1.1f);
                    });
                });
            });
        }

        private void AddDownArrowCompact(ColumnDescriptor column)
        {
            column.Item().PaddingLeft(14).PaddingVertical(2)
                .Container().Width(14).Height(14).AlignCenter().AlignMiddle()
                .Svg(SvgIcons.Down);
        }

        private void AddBulletPoint(ColumnDescriptor column, string text)
        {
            column.Item().Row(r =>
            {
                // ROUND BULLET
                r.ConstantItem(8)
                    .PaddingTop(5)
                    .AlignLeft()
                    .Width(4)
                    .Height(4)
                    .Background("#999999")
                    .CornerRadius(0);

                r.RelativeItem()
                    .PaddingLeft(4)
                    .Text(text)
                    .FontSize(Sizing.BODY_TEXT_FONT_SIZE)
                    .FontColor(Colors.Black)
                    .LineHeight(1.15f);
            });
        }


        private void AddSubBullet(ColumnDescriptor column, string text)
        {
            column.Item().Row(r =>
            {
                r.ConstantItem(8).PaddingTop(3).AlignLeft().Width(4).Height(4).Background("#F39C12");
                r.RelativeItem().PaddingLeft(4).Text(text).FontSize(Sizing.SMALL_TEXT_FONT_SIZE).FontColor(Colors.Black).LineHeight(1.1f);
            });
        }

        //private void ComposePage6Content(IContainer container)
        //{
        //    container
        //        .PaddingVertical(15)
        //        .PaddingHorizontal(25)
        //        .AlignCenter()
        //        .MaxWidth(480)
        //        .Column(column =>
        //        {
        //            column.Spacing(8);

        //            // ====== Header ======
        //            column.Item().Row(row =>
        //            {
        //                row.RelativeItem(1.8f).Column(left =>
        //                {
        //                    left.Item().Text("HOW TO USE THIS").FontSize(Sizing.SUBSECTION_TITLE_FONT_SIZE).FontColor("#999999");
        //                    left.Item().Text("INTERVIEW GUIDE").FontSize(Sizing.SUBSECTION_TITLE_FONT_SIZE).Bold().FontColor("#666666");
        //                    left.Item().PaddingTop(3).Text(text =>
        //                    {
        //                        text.Span("This interview guide contains competency-based questions to guide the interview process. Below is an illustration of the process to follow when using this guide.")
        //                            .FontSize(Sizing.BODY_TEXT_FONT_SIZE).FontColor("#666666").LineHeight(1.15f);
        //                    });
        //                });

        //                row.RelativeItem(3.5f).PaddingLeft(30).Column(right =>
        //                {
        //                    right.Spacing(2);

        //                    // Step 1
        //                    AddStep(right, 1, "PREPARE FOR THE INTERVIEW", "#E74C3C", SvgIcons.Briefcase);
        //                    AddDownArrow(right);

        //                    // Step 2
        //                    AddStep(right, 2, "SELECT QUESTIONS FROM THOSE PRESENTED IN EACH CATEGORY", "#E74C3C", SvgIcons.QuestionMark);
        //                    AddDownArrow(right);
        //                });
        //            });

        //            // ====== Main Section ======
        //            column.Item().PaddingTop(5).Row(row =>
        //            {
        //                // Left Column
        //                row.RelativeItem(1.8f).Column(leftCol =>
        //                {
        //                    // --- How to Ask Good Questions ---
        //                    leftCol.Item().PaddingTop(2).Text(t =>
        //                    {
        //                        t.Span("HOW TO ASK ").FontSize(Sizing.SUBSECTION_TITLE_FONT_SIZE).FontColor("#999999");
        //                        t.Span("GOOD INTERVIEWING QUESTIONS").FontSize(Sizing.SUBSECTION_TITLE_FONT_SIZE).Bold().FontColor("#666666");
        //                    });

        //                    leftCol.Item().PaddingTop(3).Column(bullets =>
        //                    {
        //                        bullets.Spacing(1);
        //                        AddBulletPoint(bullets, "Ask open-ended questions (how?, why?, describe, tell me about).");
        //                        AddBulletPoint(bullets, "Ask for practical, real-life examples.");
        //                        AddBulletPoint(bullets, "Always probe to discover more information and do not let the interview structure limit your probing.");
        //                        AddBulletPoint(bullets, "Avoid asking improper questions by focusing on the inherent job requirements.");
        //                        AddBulletPoint(bullets, "If the candidate strays off the subject, redirect as quickly as possible.");
        //                        AddBulletPoint(bullets, "Paraphrase the candidate's answer to show that you have listened.");
        //                    });

        //                    // --- Points to Remember ---
        //                    leftCol.Item().PaddingTop(8).Text(t =>
        //                    {
        //                        t.Span("POINTS TO ").FontSize(Sizing.SUBSECTION_TITLE_FONT_SIZE).FontColor("#999999");
        //                        t.Span("REMEMBER").FontSize(Sizing.SUBSECTION_TITLE_FONT_SIZE).Bold().FontColor("#666666");
        //                    });

        //                    leftCol.Item().PaddingTop(3).Column(points =>
        //                    {
        //                        points.Spacing(1);
        //                        AddBulletPoint(points, "Do not rush to a decision or judgment.");
        //                        AddBulletPoint(points, "Do not make binding contractual statements during the interview.");
        //                        AddBulletPoint(points, "Remain professional but open and welcoming.");
        //                        AddBulletPoint(points, "\"Hire for fit, train for skills\"");
        //                    });
        //                });

        //                // Right Column
        //                row.RelativeItem(3.5f).PaddingLeft(30).Column(rightCol =>
        //                {
        //                    rightCol.Spacing(1.8f);

        //                    AddStep(rightCol, 3, "CONDUCT THE INTERVIEW", "#f1592a", SvgIcons.Users);
        //                    AddDownArrow(rightCol);

        //                    AddStep(rightCol, 4, "RECORD THE RESULTS", "#f59726", SvgIcons.ThinkIcon);
        //                    AddDownArrow(rightCol);

        //                    AddStep(rightCol, 5, "SCORE AND EVALUATE THE DATA", "#edd623", SvgIcons.Search);
        //                    AddDownArrow(rightCol);
        //                    rightCol.Item().Width(250).PaddingLeft(68).PaddingTop(-30).Column(c =>
        //                    {
        //                        c.Spacing(1.5f);
        //                        AddOrangeBullet(c, "Use the 10-point rating scale consistently, basing every score on clear behavioral evidence from the interview.");
        //                        AddOrangeBullet(c, "Align ratings to the critical job requirements, not to other applicants, to reduce subjectivity and bias.");
        //                        AddOrangeBullet(c, "Discuss scores openly as a panel to reach a fair, defensible consensus on each competency.");
        //                    });


        //                    AddStep(rightCol, 6, "INTERVIEW SCORE", "#dae146", SvgIcons.Percentage);

        //                    //// Score legend aligned under Step 6
        //                    //rightCol.Item().PaddingLeft(68).PaddingTop(-2).Column(score =>
        //                    //{
        //                    //    score.Item().Column(inner =>
        //                    //    {
        //                    //        inner.Spacing(1.3f);
        //                    //        AddScoreLegend(inner, "#E74C3C", "Applicant did not meet expectations", 1);
        //                    //        AddScoreLegend(inner, "#F39C12", "Applicant requires additional competency", 2);
        //                    //        AddScoreLegend(inner, "#F4D03F", "Applicant met expectations", 3);
        //                    //        AddScoreLegend(inner, "#52BE80", "Applicant exceeded expectations", 4);
        //                    //        AddScoreLegend(inner, "#27AE60", "Applicant is an excellent candidate for acceptance", 5);
        //                    //    });
        //                    //});
        //                    AddDownArrow(rightCol);

        //                    AddStep(rightCol, 7, "RECORD THE RESULTS", "#d0d727", SvgIcons.Lines);
        //                    AddDownArrow(rightCol);

        //                    AddStep(rightCol, 8, "MAKE RECOMMENDATION", "#8bc73c", SvgIcons.Star);
        //                });
        //            });
        //        });
        //}
        //// ====== Helper Methods ======
        //private void AddStep(ColumnDescriptor column, int stepNumber, string title, string color, string icon)
        //{
        //    column.Item().PaddingLeft(15).Row(r =>
        //    {
        //        r.AutoItem().Width(42).Height(42).Border(2).BorderColor("#CCCCCC")
        //            .CornerRadius(21).Background("#FFFFFF")
        //            .AlignCenter().AlignMiddle()
        //            .Container().Width(20).Height(20).AlignCenter().AlignMiddle()
        //            .Svg(icon.Replace("{COLOR}", color));

        //        r.RelativeItem().PaddingLeft(6).AlignMiddle().Row(inner =>
        //        {
        //            inner.AutoItem().Text(stepNumber.ToString()).FontSize(26).Bold().FontColor(color);
        //            inner.RelativeItem().PaddingLeft(6).Text(title).FontSize(Sizing.BODY_TEXT_FONT_SIZE).FontColor("#666666");
        //        });
        //    });
        //}

        //private void AddDownArrow(ColumnDescriptor column, float topPadding = 3)
        //{
        //    column.Item().PaddingLeft(30).PaddingTop(topPadding)
        //        .Container().Width(16).Height(16).AlignCenter().AlignMiddle()
        //        .Svg(SvgIcons.Down);
        //}

        //private void AddBulletPoint(ColumnDescriptor column, string text)
        //{
        //    column.Item().Row(r =>
        //    {
        //        r.ConstantItem(6).PaddingTop(4).AlignLeft().Width(4).Height(4).Background("#BBBBBB");
        //        r.RelativeItem().PaddingLeft(3).Text(text).FontSize(Sizing.BODY_TEXT_FONT_SIZE).FontColor("#666666").LineHeight(1.15f);
        //    });
        //}

        //private void AddOrangeBullet(ColumnDescriptor column, string text)
        //{
        //    column.Item().PaddingLeft(18).Row(r =>
        //    {
        //        r.ConstantItem(6).PaddingTop(1).AlignLeft().Width(5).Height(5).Background("#F39C12");
        //        r.RelativeItem().PaddingLeft(3).Text(text).FontSize(Sizing.SMALL_TEXT_FONT_SIZE).FontColor("#666666").LineHeight(1.1f);
        //    });
        //}

        //private void AddScoreLegend(ColumnDescriptor column, string color, string text, int number)
        //{
        //    column.Item().Row(r =>
        //    {
        //        r.ConstantItem(12).Width(10).Height(10).CornerRadius(5).Background(color)
        //            .AlignCenter().AlignMiddle()
        //            .Text(number.ToString()).FontSize(7).Bold().FontColor("#FFFFFF");

        //        r.RelativeItem().PaddingLeft(4).AlignMiddle()
        //            .Text(text).FontSize(Sizing.SMALL_TEXT_FONT_SIZE).FontColor("#666666");
        //    });
        //}

        #endregion

        #region Page 7 Methods
        /// <summary>
        /// Composes the main content for Page 7 of the Talent Report PDF, which includes the "About" section,
        /// details on the success profile, assessment methods, scores, color-coded performance boxes,
        /// percentile information, and guidance on interpreting the report.
        /// </summary>
        /// <param name="container">The <see cref="IContainer"/> used to structure and render the page content.</param>
        /// <param name="request">The <see cref="TalentMatchReportRequest"/> containing Page 5 data and related content.</param>
        private void ComposePage7Content(IContainer container, TalentFitReport_InterviewRequest request)
        {
            container.AlignCenter().MaxWidth(480).Column(column =>
            {
                column.Spacing(10);

                // Header
                column.Item().Text("Interview Questions")
                    .FontSize(Sizing.SECTION_TITLE_FONT_SIZE)
                    .Bold()
                    .FontColor(Colors.Black);

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
        /// <param name="request">The <see cref="TalentMatchReportRequest"/> containing Page 6 data and related content.</param>
        private void ComposePage8Content(IContainer container, TalentFitReport_InterviewRequest request)
        {
            container.AlignCenter().MaxWidth(480).Column(column =>
            {
                column.Spacing(10);
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
        /// <param name="request">The <see cref="TalentMatchReportRequest"/> containing Page 7 data and related content.</param>
        private void ComposePage9Content(IContainer container, TalentFitReport_InterviewRequest request)
        {
            container.AlignCenter().MaxWidth(480).Column(column =>
            {
                column.Spacing(10);
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
        /// <param name="request">The <see cref="TalentMatchReportRequest"/> containing Page 8 data and related content.</param>
        private void ComposePage10Content(IContainer container, TalentFitReport_InterviewRequest request)
        {
            container.AlignCenter().MaxWidth(480).Column(column =>
            {
                column.Spacing(10);
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
        /// <param name="request">The <see cref="TalentMatchReportRequest"/> containing Page 9 data and related content.</param>
        private void ComposePage11Content(IContainer container, TalentFitReport_InterviewRequest request)
        {
            container.AlignCenter().MaxWidth(480).Column(column =>
            {
                column.Spacing(10);
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
        /// <param name="request">The <see cref="TalentMatchReportRequest"/> containing Page 10 data and related content.</param>
        private void ComposePage12Content(IContainer container, TalentFitReport_InterviewRequest request)
        {
            container.AlignCenter().MaxWidth(480).Column(column =>
            {
                column.Spacing(8);
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
        /// <param name="request">The <see cref="TalentMatchReportRequest"/> containing Page 11 data and related content.</param>
        private void ComposePage13Content(IContainer container, TalentFitReport_InterviewRequest request)
        {
            container.AlignCenter().MaxWidth(480).Column(column =>
            {
                column.Spacing(10);
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
        /// <param name="request">The <see cref="TalentMatchReportRequest"/> containing Page 12 data and related content.</param>
        private void ComposePage14Content(IContainer container, TalentFitReport_InterviewRequest request)
        {
            container.AlignCenter().MaxWidth(480).Column(column =>
            {
                column.Spacing(10);
                ComposeInterviewQuestionsSection(column, request.Page14.InterviewSection, "Interview Rating");
            });
        }

        #endregion

        #region Page 15 Methods

        /// <summary>
        /// Composes the main content for Page 15 of the Talent Report PDF, which includes technical information,
        /// job/role data, assessment methods, and input data sections.
        /// </summary>
        /// <param name="container">The <see cref="IContainer"/> used to structure and render the page content.</param>
        /// <param name="request">The <see cref="TalentMatchReportRequest"/> containing Page 13 data and related content.</param>
        private void ComposePage15Content(IContainer container, TalentFitReport_InterviewRequest request)
        {
            container.AlignCenter().MaxWidth(480).Column(column =>
            {
                column.Spacing(10);
                ComposeInterviewQuestionsSection(column, request.Page15.InterviewSection, "Interview Rating");
            });
        }

        #endregion

        #region Page 16 Methods

        /// <summary>
        /// Composes the main content for Page 16 of the Talent Report PDF, which includes technical information,
        /// job/role data, assessment methods, and input data sections.
        /// </summary>
        /// <param name="container">The <see cref="IContainer"/> used to structure and render the page content.</param>
        /// <param name="request">The <see cref="TalentMatchReportRequest"/> containing Page 14 data and related content.</param>
        private void ComposePage16Content(IContainer container, TalentFitReport_InterviewRequest request)
        {
            container.AlignCenter().MaxWidth(480).Column(column =>
            {
                // Title
                column.Item().Text("Interview Ratings Summary")
                    .FontSize(Sizing.SECTION_TITLE_FONT_SIZE)
                    .Bold()
                    .FontColor(Colors.Black);

                // Description paragraph
                column.Item().PaddingTop(2).PaddingBottom(2).Text(
                    "Use the table below to record the average score for each competency. " +
                    "Combine these scores to determine the Overall Interview Score, which should be " +
                    "considered alongside qualitative observations and panel discussion when forming " +
                    "the final recommendation."
                )
                .FontSize(10)
                .FontColor(Colors.Black)
                .LineHeight(1.4f);

                // Details Section
                column.Item().Element(c => ComposeDetailsSection(c, request));

                // Summary of Interview Scores Section
                column.Item().Element(ComposeInterviewScoresSection);

                // Commentary Section
                column.Item().PaddingTop(2.5f).Element(ComposeCommentarySection);

                // Recommendation Section
                column.Item().PaddingTop(2.5f).Element(ComposeRecommendationSection);

                // Final Selection Recommendation
                column.Item().PaddingTop(2.5f).Element(ComposeFinalSelectionSection);
                // Parent row containing LEFT + RIGHT sections
                column.Item().Row(parent =>
                {
                    // LEFT section (Overall Competency Score)
                    parent.RelativeItem().PaddingLeft(2).AlignLeft().Row(row =>
                    {
                        row.AutoItem().PaddingTop(12)
                            .PaddingRight(10)
                            .Text("Overall\nInterview :\nScore")
                            .ExtraBold()
                            .FontSize(Sizing.SMALL_TEXT_FONT_SIZE)
                            .FontColor("#666666");

                        row.AutoItem()
                            .PaddingTop(10)
                            .Width(60)
                            .Height(35)
                            .Border(1)
                            .BorderColor("#CCCCCC")
                            .Background("#FAFAFA");
                    });

                    // RIGHT section (Signed)
                    parent.RelativeItem().AlignRight().PaddingRight(1).Row(row =>
                    {
                        row.AutoItem()
                            .PaddingTop(20)
                            .PaddingRight(10)
                            .Text("Signed :")
                            .ExtraBold()
                            .FontSize(Sizing.SMALL_TEXT_FONT_SIZE)
                            .FontColor("#666666");

                        row.AutoItem()
                            .PaddingTop(10)
                            .Width(120)
                            .Height(35)
                            .Border(1)
                            .BorderColor("#CCCCCC")
                            .Background("#FAFAFA");
                    });
                });

            });
        }

        private void ComposeDetailsSection(IContainer container, TalentFitReport_InterviewRequest request)
        {
            container.Column(column =>
            {
                // Header
                column.Item().CornerRadius(5).Background("#A9A9A9").Padding(4).Text("Details")
                    .FontSize(Sizing.BODY_TEXT_FONT_SIZE)
                    .Bold()
                    .FontColor("#FFFFFF");

                // Details Grid
                column.Item().Width(480).PaddingLeft(1).Table(table =>
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
                            .Text(label).FontSize(Sizing.SMALL_TEXT_FONT_SIZE).FontColor("#666666");

                        // Vertical dotted separator
                        table.Cell().BorderBottom(1).BorderColor("#CCCCCC").PaddingVertical(2)
                            .Element(e => e.LineVertical(1)
                                      .LineColor("#666666")   // darker grey
                                .LineDashPattern(new float[] { 2, 2 }, Unit.Point));  // dotted style


                        // Right text (value)
                        table.Cell().BorderBottom(1).BorderColor("#CCCCCC").Padding(5)
                            .Text(value).FontSize(Sizing.SMALL_TEXT_FONT_SIZE);
                    }

                    AddRow("Interviewer Name:", request.Page16.InterviewerName ?? "");
                    AddRow("Interview date:", request.Page16.InterviewDate ?? "");
                    AddRow("Role applied for:", request.Page16.RoleAppliedFor ?? "");
                });
            });
        }

        private void ComposeInterviewScoresSection(IContainer container)
        {
            container.Column(column =>
            {
                // =========================================================
                // HEADER ROW 1 (Text Labels)
                // =========================================================
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(3.5f);      // Larger summary column
                        for (int i = 0; i < 10; i++)
                            columns.RelativeColumn(1);     // 1–10 equal width
                    });

                    // Empty cell above Summary label
                    table.Cell().MinHeight(16).Text("");

                    // Well Below (1–3)
                    table.Cell().ColumnSpan(3).PaddingLeft(-35)
                        .AlignCenter()
                        .MinHeight(16)
                        .PaddingTop(4)
                        .Element(e =>
                        {
                            e.Column(col =>
                            {
                                col.Item().AlignCenter().Text("Well Below").FontSize(9).Bold();
                                col.Item().AlignCenter().Text("Expectations").FontSize(9).Bold();
                            });
                        });

                    // Meet (4–6)
                    table.Cell().ColumnSpan(3).PaddingLeft(28)
                        .AlignCenter()
                        .MinHeight(16)
                        .PaddingTop(4)
                        .Element(e =>
                        {
                            e.Column(col =>
                            {
                                col.Item().AlignCenter().Text("Meets").FontSize(9).Bold();
                                col.Item().AlignCenter().Text("Expectations").FontSize(9).Bold();
                            });
                        });

                    // Well Above (7–10)
                    table.Cell().ColumnSpan(4).PaddingRight(5)
                        .AlignRight()
                        .MinHeight(16)
                        .PaddingTop(4)
                        .Element(e =>
                        {
                            e.Column(col =>
                            {
                                col.Item().AlignCenter().Text("Well Above").FontSize(9).Bold();
                                col.Item().AlignCenter().Text("Expectations").FontSize(9).Bold();
                            });
                        });
                });


                // =========================================================
                // HEADER ROW 2 (Summary + Colored 1–10 Boxes)
                // =========================================================
                column.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(3.5f);
                        for (int i = 0; i < 10; i++)
                            columns.RelativeColumn(1);
                    });

                    // Summary Label
                    table.Cell().CornerRadius(6)
                        .Background("#A9A9A9")
                        .Padding(6)
                        .MinHeight(19)
                        .AlignMiddle()
                        .Text("Summary of Interview Scores")
                        .FontSize(11)
                        .Bold()
                        .FontColor("#FFFFFF");

                    // Colors for boxes 1–10
                    var colors = new[]
                    {
                "#E31D25", "#EB1C48", "#F15929", "#F59726", "#F3D520",
                "#F3D520", "#DAE241", "#D7D820", "#8CC63E", "#39B54A"
            };

                    for (int i = 0; i < 10; i++)
                    {
                        table.Cell().CornerRadius(4)
                            .Background(colors[i])
                            .Border(1)
                            .BorderColor("#FFFFFF")
                            .Padding(4)
                            .MinHeight(20)
                            .AlignCenter()
                            .AlignMiddle()
                            .Text((i + 1).ToString())
                            .FontSize(9)
                            .Bold()
                            .FontColor("#FFFFFF");
                    }
                });


                // =========================================================
                // CRITERIA TABLE (Reduced Height)
                // =========================================================
                string[] criteria =
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
                        columns.RelativeColumn(3.5f);
                        for (int i = 0; i < 10; i++)
                            columns.RelativeColumn(1);
                    });

                    foreach (var criterion in criteria)
                    {
                        // Criterion Name Cell
                        table.Cell()
                            .Border(1)
                            .BorderColor("#CCCCCC")
                            .Padding(3)
                            .MinHeight(16)
                            .AlignMiddle()
                            .Text(criterion)
                            .FontSize(10);

                        // 1–10 cells
                        for (int i = 0; i < 10; i++)
                        {
                            table.Cell()
                                .Border(1)
                                .BorderColor("#CCCCCC")
                                .Padding(3)
                                .MinHeight(16)
                                .Text("");
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
                    .FontSize(Sizing.SUBSECTION_TITLE_FONT_SIZE)
                    .Bold()
                    .FontColor("#FFFFFF");

                // Commentary Grid
                column.Item().Width(479).PaddingLeft(1.5f).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(1);
                        columns.RelativeColumn(1);
                    });

                    // Key concerns header
                    table.Cell().BorderBottom(1).BorderColor("#CCCCCC").Padding(5)
                        .Text("Key concerns about the candidate").FontSize(Sizing.SMALL_TEXT_FONT_SIZE).FontColor("#666666");

                    // Key strengths header
                    table.Cell().BorderBottom(1).BorderColor("#CCCCCC").Padding(5)
                        .Text("Key strengths of this candidate").FontSize(Sizing.SMALL_TEXT_FONT_SIZE).FontColor("#666666");

                    // Empty content areas
                    table.Cell().Border(1).BorderColor("#CCCCCC").Padding(5).MinHeight(60).Text("");
                    table.Cell().Border(1).BorderColor("#CCCCCC").Padding(5).MinHeight(60).Text("");
                });
            });
        }

        private void ComposeRecommendationSection(IContainer container)
        {
            container.Column(column =>
            {
                // Header
                column.Item().CornerRadius(5).Background("#A9A9A9").Padding(4).Text("Recommendation")
                    .FontSize(Sizing.SUBSECTION_TITLE_FONT_SIZE)
                    .Bold()
                    .FontColor("#FFFFFF");

                // Empty content area
                column.Item().Width(479).PaddingLeft(1.5f).Border(1).BorderColor("#CCCCCC").Padding(5).MinHeight(50).Text("");
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
                         .Text("Final Selection Recommendation")
                         .FontSize(Sizing.SUBSECTION_TITLE_FONT_SIZE)
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
                         .FontSize(Sizing.SMALL_TEXT_FONT_SIZE)
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
                             col.Item().Text("Recommended").FontSize(Sizing.SMALL_TEXT_FONT_SIZE).Bold().FontColor("#000000");
                             col.Item().Text("With Reservation").FontSize(Sizing.SMALL_TEXT_FONT_SIZE).Bold().FontColor("#000000");
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
                         .FontSize(Sizing.SMALL_TEXT_FONT_SIZE)
                         .Bold()
                         .FontColor("#FFFFFF");
                    });
                });
            });
        }
        #endregion

        #region Helper Method For Q/A Pages
        private void ComposeInterviewQuestionsSection(ColumnDescriptor column, Models.TalentFitReport.InterviewSection interviewSection, string ratingTitle)
        {
            // Main content box
            column.Item().Column(contentColumn =>
            {
                // Section header
                contentColumn.Item()
                    .Background(Colors.Grey.Medium)
                    .CornerRadius(5)
                    .Padding(3)
                    .Text(interviewSection?.Title)
                    .FontSize(Sizing.SUBSECTION_TITLE_FONT_SIZE)
                    .Bold()
                    .FontColor(Colors.White);

                // Description
                contentColumn.Item()
                    .PaddingTop(5)
                    .PaddingBottom(5)
                    .Text(interviewSection?.Description)
                    .FontSize(Sizing.BODY_TEXT_FONT_SIZE)
                    .FontColor(Colors.Black);

                // Top divider line
                contentColumn.Item()
                    .LineHorizontal(2)
                    .LineColor(Colors.Grey.Lighten1);

                // Loop through questions
                if (interviewSection?.Questions != null)
                {
                    for (int i = 0; i < interviewSection.Questions.Count; i++)
                    {
                        var question = interviewSection.Questions[i];

                        // Full-width question with writing space below
                        contentColumn.Item().PaddingTop(5).Column(qColumn =>
                        {
                            // Question text
                            qColumn.Item()
                                .Text(question.Question)
                                .FontSize(Sizing.BODY_TEXT_FONT_SIZE)
                                .FontColor(Colors.Grey.Darken2);

                            // Sub-questions (bullets)
                            qColumn.Item()
                                .PaddingTop(5)
                                .PaddingLeft(15)
                                .Column(bulletColumn =>
                                {
                                    foreach (var subQuestion in question.SubQuestions)
                                    {
                                        bulletColumn.Item()
                                            .PaddingTop(subQuestion == question.SubQuestions.First() ? 2 : 5)
                                            .Row(row =>
                                            {
                                                row.AutoItem().Text("•").FontSize(9);
                                                row.RelativeItem()
                                                    .PaddingLeft(5)
                                                    .Text(subQuestion)
                                                    .FontSize(Sizing.SMALL_TEXT_FONT_SIZE);
                                            });
                                    }
                                });

                            // Add small gap
                            qColumn.Item().PaddingTop(8);

                            // ============================================
                            //  WRITING SPACE WITH TOP-RIGHT SCORE BOX
                            // ============================================
                            qColumn.Item()
                                .Height(69)
                                .Border(1)
                                .BorderColor(Colors.Grey.Lighten1)
                                .Background(Colors.White)
                                .Padding(1)
                                .Layers(layers =>
                                {
                                    // Background writing space
                                    layers.PrimaryLayer().Element(e => { });

                                    // TOP-RIGHT SCORE BOX
                                    layers.Layer().AlignTop().AlignRight().Column(rightCol =>
                                    {
                                        // SCORE BOX with only left + bottom borders visible
                                        rightCol.Item()
                                            .Width(35)
                                            .Height(15)
                                            .Background(Colors.White)
                                            .BorderLeft(1)
                                            .BorderBottom(1)
                                            .BorderColor(Colors.Grey.Lighten1); // SAME COLOR AS OUTER BOX

                                        // SCORE label
                                        rightCol.Item()
                                            .PaddingTop(2)
                                            .AlignCenter()
                                            .Text("Score")
                                            .FontSize(8)
                                            .FontColor(Colors.Black);
                                    });
                                });

                        });

                        // Divider line (not after last question)
                        if (i < interviewSection.Questions.Count - 1)
                        {
                            contentColumn.Item()
                                .PaddingTop(10)
                                .LineHorizontal(1)
                                .LineColor(Colors.Grey.Darken1);
                        }
                    }
                }

                // Bottom divider line
                contentColumn.Item()
                    .LineHorizontal(2)
                    .LineColor(Colors.Grey.Lighten1);
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
                row.RelativeItem(3).Text(""); // Empty space for label area

                row.RelativeItem(10).PaddingBottom(1.5f).Row(labelRow =>
                {
                    // "Well below expectations" aligned with boxes 1-2
                    labelRow.RelativeItem(2).AlignCenter().Text("Well Below\nExpectations")
                        .FontSize(Sizing.SMALL_TEXT_FONT_SIZE)
                        .FontColor(Colors.Black);

                    // Empty space
                    labelRow.RelativeItem(2).Text("");

                    // "Meets expectations" aligned with boxes 5-6
                    labelRow.RelativeItem(2).AlignCenter().Text("Meets\nExpectations")
                        .FontSize(Sizing.SMALL_TEXT_FONT_SIZE)
                        .FontColor(Colors.Black);

                    // Empty space
                    labelRow.RelativeItem(2).Text("");

                    // "Well above expectations" aligned with boxes 9-10
                    labelRow.RelativeItem(2).AlignCenter().Text("Well Above\nExpectations")
                         .FontSize(Sizing.SMALL_TEXT_FONT_SIZE)
                        .FontColor(Colors.Black);
                });
            });

            // Rating boxes section with colored boxes
            column.Item().PaddingTop(-10).Row(ratingRow =>
            {
                // Label section
                ratingRow.RelativeItem(3).CornerRadius(8)
                    .Background(Colors.Grey.Medium)
                    .Padding(6)
                    .AlignMiddle()
                    .Text(labelText)
                    .FontSize(Sizing.SUBSECTION_TITLE_FONT_SIZE)
                    .Bold()
                    .FontColor(Colors.White);

                // Rating boxes section
                ratingRow.RelativeItem(10).Row(boxRow =>
                {
                    // Define rating box configurations with your color codes
                    var ratingConfigs = new[]
                    {
                new { Rating = "1", Color = "#E31D25" },
                new { Rating = "2", Color = "#EB1C48" },
                new { Rating = "3", Color = "#F15929" },
                new { Rating = "4", Color = "#F59726" },
                new { Rating = "5", Color = "#F3D520" },
                new { Rating = "6", Color = "#F3D520" },
                new { Rating = "7", Color = "#DAE241" },
                new { Rating = "8", Color = "#D7D820" },
                new { Rating = "9", Color = "#8CC63E" },
                new { Rating = "10", Color = "#39B54A" }
            };

                    foreach (var config in ratingConfigs)
                    {
                        boxRow.RelativeItem().CornerRadius(4)
                            .Background(config.Color)
                            .Border(1)
                            .BorderColor(Colors.White)
                            .Padding(4)
                            .AlignMiddle()
                            .AlignCenter()
                            .Text(config.Rating)
                            .FontSize(Sizing.SMALL_TEXT_FONT_SIZE)
                            .Bold()
                            .FontColor(Colors.White);
                    }
                });
            });

            // ============================================
            // AVERAGE SCORE BOX 
            // ============================================
            column.Item().PaddingLeft(5).Column(avgCol =>
            {
                // Apply padding to the container (Item)
                avgCol.Item()
                    .PaddingBottom(5)
                    .Text("Average Score")
                    .FontSize(Sizing.BODY_TEXT_FONT_SIZE)
                    .Bold()
                    .FontColor(Colors.Black);

                // writeable box for average value
                avgCol.Item().PaddingLeft(1)
                    .Height(30).Width(73)
                    .Border(1)
                    .BorderColor(Colors.Grey.Lighten1)
                    .Background(Colors.White);
            });

        }
        #endregion

    }
}
