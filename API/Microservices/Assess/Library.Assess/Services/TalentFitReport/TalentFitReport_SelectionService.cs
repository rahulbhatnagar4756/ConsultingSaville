using Library.Assess.Models.TalentFitReport;
using Library.Assess.Utilities.TalentReport;

using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Library.Assess.Services.TalentFitReport
{
    public interface ITalentFitReport_SelectionService
    {
        /// <summary>
        /// Generates a Talent Fit Report PDF based on the provided request and returns a response object.
        /// </summary>
        /// <param name="request">The request containing all data for the Talent Report.</param>
        /// <returns>A <see cref="TalentFitReport_DevelopmentResponse"/> containing the PDF data and metadata.</returns>
        Task<TalentFitReport_Response> GenerateReportAsync(TalentFitReport_SelectionRequest request);

        /// <summary>
        /// Generates a Talent Fit Report PDF and returns it as a byte array.
        /// </summary>
        /// <param name="request">The request containing all data for the Talent Report.</param>
        /// <returns>A byte array representing the generated PDF file.</returns>
        Task<byte[]> GeneratePdfBytesAsync(TalentFitReport_SelectionRequest request);
    }

    public class TalentFitReport_SelectionService : ITalentFitReport_SelectionService
    {

        public TalentFitReport_SelectionService()
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
        public async Task<TalentFitReport_Response> GenerateReportAsync(TalentFitReport_SelectionRequest request)
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
        public async Task<byte[]> GeneratePdfBytesAsync(TalentFitReport_SelectionRequest request)
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
        private void ComposePage6(PageDescriptor page, TalentFitReport_SelectionRequest request)
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
        private void ComposePage7(PageDescriptor page, TalentFitReport_Request request)
        {
            page.Size(PageSizes.A4);
            page.Margin(0);
            page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Neo Sans Std"));
            page.Header().ShowOnce().Element(c => TalentReport.ComposePageHeader(c, request));
            page.Footer().ShowOnce().Element(c => TalentReport.ComposePageFooter(c, request, 7));
            page.Content().PaddingHorizontal(20).PaddingVertical(15)
                .Element(c => TalentReport.ComposeLastPageContent(c, request));
        }
        #endregion
        #endregion

        #region Initiate Helper Methods              

        #region Page 6 Methods
        private void ComposePage6Content(IContainer container, TalentFitReport_SelectionRequest request)
        {
            container.AlignCenter().MaxWidth(480).Column(column =>
            {
                // Page Title
                column.Item().PaddingBottom(Sizing.SECTION_SPACING)
                    .Text("Development Tips")
                    .FontSize(Sizing.SECTION_TITLE_FONT_SIZE)
                    .Bold()
                    .FontColor(Colors.Black);

                // Introduction Paragraph 1
                column.Item().PaddingBottom(Sizing.PARAGRAPH_SPACING)
                    .Text("This section outlines actions that may improve your behavioral competence. Start by reviewing both your strengths and development areas as part of your development strategy. If none are highlighted, identify the behaviors that would benefit you most in your current work context.")
                    .FontSize(Sizing.BODY_TEXT_FONT_SIZE)
                    .LineHeight(Sizing.LINE_HEIGHT);

                // Introduction Paragraph 2
                column.Item().PaddingBottom(Sizing.PARAGRAPH_SPACING)
                    .Text("Read the relevant development tips and choose one practical action for each behavioral dimension you plan to focus on.")
                 .FontSize(Sizing.BODY_TEXT_FONT_SIZE)
                    .LineHeight(Sizing.LINE_HEIGHT);

                // Development Sections
                if (request.Page6?.DevelopmentSections != null && request.Page6.DevelopmentSections.Any())
                {
                    foreach (var section in request.Page6.DevelopmentSections)
                    {
                        column.Item().PaddingBottom(Sizing.SUBSECTION_SPACING).Column(sectionColumn =>
                        {
                            // Section Title
                            sectionColumn.Item().PaddingBottom(Sizing.PARAGRAPH_SPACING)
                                .Background("#8C919D").CornerRadius(4)
                                .Padding(Sizing.ITEM_SPACING)
                                .AlignCenter()
                                .Text(section.Title)
                                 .FontSize(Sizing.BODY_TEXT_FONT_SIZE)
                                .Bold()
                                .FontColor(Colors.White);

                            // Section Description
                            if (!string.IsNullOrEmpty(section.Description))
                            {
                                sectionColumn.Item().PaddingBottom(Sizing.PARAGRAPH_SPACING)
                                    .Text(section.Description)
                                     .FontSize(9.5f)
                                    .LineHeight(Sizing.LINE_HEIGHT);
                            }

                            // Section Tips
                            if (section.Tips != null && section.Tips.Any())
                            {
                                sectionColumn.Item().PaddingLeft(13).Column(bulletColumn =>
                                {
                                    foreach (var tip in section.Tips)
                                    {
                                        bulletColumn.Item().PaddingBottom(Sizing.ITEM_SPACING).Row(row =>
                                        {
                                            row.ConstantItem(15).AlignTop()
                                                .PaddingTop(0)
                                                .Text("•")
                                                .FontSize(Sizing.SUBSECTION_TITLE_FONT_SIZE);

                                            row.RelativeItem()
                                                .Text(tip)
                                                .FontSize(10)
                                                .LineHeight(Sizing.LINE_HEIGHT);
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

        #endregion       
    }
}
