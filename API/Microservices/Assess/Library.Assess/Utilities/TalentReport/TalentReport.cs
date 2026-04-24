using Library.Assess.Models.TalentFitReport;

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Assess.Utilities.TalentReport
{
    public static class TalentReport
    {
        #region Page 1 Methods

        /// <summary>
        /// Composes the top header section of the report page with company logo and name.
        /// </summary>
        /// <param name="container">The <see cref="IContainer"/> used to compose the header layout.</param>
        /// <param name="request">The <see cref="TalentFitReport_DevelopmentRequest"/> containing company logo and name.</param>
        public static void ComposeHeader(IContainer container, TalentFitReport_Request request)
        {
            container.AlignLeft().MaxWidth(480).PaddingTop(20).PaddingHorizontal(40).Row(row =>
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
                                    .Width(220).Height(120).Image(imageBytes);
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
        /// <param name="request">The <see cref="TalentFitReport_DevelopmentRequest"/> containing the title and subtitle.</param>
        public static void ComposeTitle(IContainer container, TalentFitReport_Request request)
        {
            container.AlignLeft().PaddingLeft(15).MaxWidth(480).PaddingTop(100).Column(titleColumn =>
            {
                titleColumn.Item().Text(request.Page1.ReportTitle)
                    .FontSize(28).FontColor(Colors.Black);

                titleColumn.Item().PaddingTop(5).Text(request.Page1.ReportSubtitle)
                    .FontSize(28).SemiBold().FontColor(Colors.Black);
            });
        }

        /// <summary>
        /// Composes the employee information section including name, company, and report date.
        /// </summary>
        /// <param name="container">The <see cref="IContainer"/> used to compose the employee info layout.</param>
        /// <param name="request">The <see cref="TalentFitReport_DevelopmentRequest"/> containing employee and company details.</param>
        public static void ComposeEmployeeInfo(IContainer container, TalentFitReport_Request request)
        {
            container.AlignLeft().MaxWidth(480).Padding(15).PaddingTop(-100).Column(infoColumn =>
            {
                infoColumn.Item().Text($"{request.CompanyName} - {request.Position}")
                    .FontSize(14).SemiBold().FontColor(Colors.Black);

                infoColumn.Item().PaddingTop(5).Text(request.ReportDate.ToString("dd MMM yyyy"))
                    .FontSize(12).FontColor(Colors.Grey.Darken1);

                infoColumn.Item().PaddingTop(15).Text(request.EmployeeName)
                    .FontSize(26).SemiBold().FontColor(Colors.Black);
            });
        }

        /// <summary>
        /// Composes the bottom decoration for Page 1, including "Powered By" branding.
        /// </summary>
        /// <param name="container">The <see cref="IContainer"/> used to compose the bottom decoration.</param>
        /// <param name="request">The <see cref="TalentFitReport_DevelopmentRequest"/> containing branding details.</param>
        public static void ComposeBottomDecoration(IContainer container, TalentFitReport_Request request)
        {
            container
                .AlignBottom()
                .AlignRight()
                .PaddingRight(90)
                .PaddingBottom(10)
                .MaxWidth(480)   // allow space for the footer to breathe
                .Background(Colors.White)
                .PaddingVertical(6)
                .PaddingHorizontal(12)
                .Row(row =>
                {
                    row.Spacing(6);

                    // Left Text
                    row.AutoItem().AlignMiddle().Text(text =>
                    {
                        text.Span("© POWERED BY ")
                            .FontSize(8)
                            .FontColor(Colors.Grey.Darken4);
                    });

                    // Logo
                    row.AutoItem().AlignMiddle().Element(e =>
                    {
                        e.Width(90) // ✅ Constrain width instead of padding
                         .Height(40)
                         .Image(Convert.FromBase64String(request.PoweredByLogo), ImageScaling.FitArea);
                    });
                });
        }

        #endregion 

        #region Page 2 Methods
        public static void ComposePage2Content(IContainer container)
        {
            container.AlignCenter().MaxWidth(480).Column(
            column =>
            {
                // Section Title
                column.Item().PaddingBottom(Sizing.SECTION_SPACING)
                    .Text("Disclaimer and Report Overview")
                    .FontSize(Sizing.SECTION_TITLE_FONT_SIZE)
                    .Bold()
                    .FontColor(Colors.Black);

                // Paragraph 1
                column.Item().PaddingBottom(Sizing.PARAGRAPH_SPACING)
                    .Text("This report is derived from a self-report questionnaire that assesses an individual's work styles, cultural preferences, core values, and competencies. It summarizes the individual's competency potential compared to the profiled role. Results are presented on a 1–10 scale, benchmarked against a normative sample of professionals from diverse industries and roles, and primarily capture the individual's self-perceptions. Scores are based on the individual's responses to the assessments detailed herein. Rigorous independent research validates its reliability in forecasting specific workplace competency and behaviors, subject to contextual factors.")
                    .FontSize(Sizing.BODY_TEXT_FONT_SIZE)
                    .LineHeight(Sizing.LINE_HEIGHT)
                    .AlignLeft();

                // Paragraph 2
                column.Item().PaddingBottom(Sizing.PARAGRAPH_SPACING)
                    .Text("The report includes sensitive personal data pertaining to personality and behavioral insights, which must be stored securely in accordance with applicable data protection protocols to safeguard against unauthorized disclosure or misuse. Access and distribution of this report are limited exclusively to authorized employees, agents, and clients bound by confidentiality agreements, and it may not be disseminated, copied, or repurposed without prior written approval.")
                    .FontSize(Sizing.BODY_TEXT_FONT_SIZE)
                    .LineHeight(Sizing.LINE_HEIGHT)
                    .AlignLeft();

                // Paragraph 3
                column.Item().PaddingBottom(Sizing.PARAGRAPH_SPACING)
                    .Text("It was produced using proprietary software directly from the individual's unmodified responses, maintaining integrity in data processing. The assessment provider expressly disclaims any warranties regarding the report's accuracy, comprehensiveness, or fitness for any intended purpose and assumes no responsibility for any outcomes, damages, or liabilities stemming from its utilization.")
                    .FontSize(Sizing.BODY_TEXT_FONT_SIZE)
                    .LineHeight(Sizing.LINE_HEIGHT)
                    .AlignLeft();

                // Paragraph 4
                column.Item().PaddingBottom(Sizing.PARAGRAPH_SPACING)
                    .Text("This assessment can be applied in employment contexts, such as recruitment, professional development, performance appraisals, or talent management, provided it adheres to relevant federal and state regulations promoting equity, validity, and the prevention of discrimination. When interpreting this information, focus on the role's inherent competency requirements. This report evaluates the individual's potential solely for this specific role; do not generalize the talent match score to other positions. For critical decisions, do not use these results in isolation—integrate them with multiple evaluation techniques, including interviews, reference verifications, assessment centers or simulations, and other relevant recruitment and selection data. It is strongly recommended to routinely assess for potential biases through data analysis; to meticulously record all related decisions for transparency; and to ensure legal compliance with jurisdiction-specific requirements.")
                    .FontSize(Sizing.BODY_TEXT_FONT_SIZE)
                    .LineHeight(Sizing.LINE_HEIGHT)
                    .AlignLeft();
            });
        }
        #endregion

        #region Page 3 Methods
        public static void ComposePage3Content(IContainer container, TalentFitReport_Request request)
        {
            container.AlignCenter().MaxWidth(480).Column(column =>
            {
                // Section Title
                column.Item().PaddingBottom(Sizing.SECTION_SPACING)
                    .Text("Introduction")
                    .FontSize(Sizing.SECTION_TITLE_FONT_SIZE)
                    .Bold()
                    .FontColor(Colors.Black);

                // Paragraph 1
                column.Item().PaddingBottom(Sizing.PARAGRAPH_SPACING)
                    .Text("Effective performance in most roles depends on how well an individual's likely behavior aligns with the behavioral requirements for success in that role.")
                    .FontSize(Sizing.BODY_TEXT_FONT_SIZE)
                    .LineHeight(Sizing.LINE_HEIGHT);

                // Paragraph 2
                column.Item().PaddingBottom(Sizing.PARAGRAPH_SPACING).Text(text =>
                {
                    text.Span("This report aims to indicate ").FontSize(Sizing.BODY_TEXT_FONT_SIZE).LineHeight(Sizing.LINE_HEIGHT);
                    text.Span(request.EmployeeName ?? "").Bold().FontSize(Sizing.BODY_TEXT_FONT_SIZE).LineHeight(Sizing.LINE_HEIGHT);
                    text.Span("'s potential fit with the key competency requirements for the ").FontSize(Sizing.BODY_TEXT_FONT_SIZE).LineHeight(Sizing.LINE_HEIGHT);
                    text.Span(request.Role ?? "").Bold().FontSize(Sizing.BODY_TEXT_FONT_SIZE).LineHeight(Sizing.LINE_HEIGHT);
                    text.Span(" role.").FontSize(Sizing.BODY_TEXT_FONT_SIZE).LineHeight(Sizing.LINE_HEIGHT);
                });

                // Paragraph 3
                column.Item().PaddingBottom(Sizing.PARAGRAPH_SPACING)
                    .Text("Multiple factors determine success in a role. Some are retrospective, such as qualifications and experience, while others relate to the current environment, including culture, values, relationships with peers, managers, and teams, and organizational leadership styles.")
                    .FontSize(Sizing.BODY_TEXT_FONT_SIZE)
                    .LineHeight(Sizing.LINE_HEIGHT);

                // Paragraph 4
                column.Item().PaddingBottom(Sizing.PARAGRAPH_SPACING)
                    .Text("This report provides a forward-looking perspective and highlights potential in the context of the profiled role.")
                    .FontSize(Sizing.BODY_TEXT_FONT_SIZE)
                    .LineHeight(Sizing.LINE_HEIGHT);

                // Subsection Title
                column.Item().PaddingTop(Sizing.SECTION_SPACING).PaddingBottom(Sizing.SUBSECTION_SPACING)
                    .Text("Scoring System Guideline")
                    .FontSize(Sizing.SECTION_TITLE_FONT_SIZE)
                    .Bold()
                    .FontColor(Colors.Black);

                // Paragraph 5
                column.Item().PaddingBottom(Sizing.PARAGRAPH_SPACING)
                    .Text("This system groups 1–10 scores into three zones for quick evaluation of competencies, culture, values, and organizational fit.")
                    .FontSize(Sizing.BODY_TEXT_FONT_SIZE)
                    .LineHeight(Sizing.LINE_HEIGHT);

                // Bullet Points
                column.Item().PaddingBottom(Sizing.ITEM_SPACING)
                    .Text("• Strong Match: Scores 7–10. High potential, proceed confidently.")
                    .FontSize(Sizing.BODY_TEXT_FONT_SIZE)
                    .LineHeight(Sizing.LINE_HEIGHT);

                column.Item().PaddingBottom(Sizing.ITEM_SPACING)
                    .Text("• Moderate Match: Scores 4–6. Average performance, monitor and support.")
                    .FontSize(Sizing.BODY_TEXT_FONT_SIZE)
                    .LineHeight(Sizing.LINE_HEIGHT);

                column.Item().PaddingBottom(Sizing.ITEM_SPACING)
                    .Text("• Needs Attention: Scores 1–3. Low potential, investigate further.")
                    .FontSize(Sizing.BODY_TEXT_FONT_SIZE)
                    .LineHeight(Sizing.LINE_HEIGHT);

                column.Item().PaddingBottom(Sizing.SECTION_SPACING)
                    .Text("Always combine with other data, check for biases, and follow legal guidelines for fair decisions.")
                    .FontSize(Sizing.BODY_TEXT_FONT_SIZE)
                    .LineHeight(Sizing.LINE_HEIGHT);

                // Subsection Title
                column.Item().PaddingTop(Sizing.SECTION_SPACING).PaddingBottom(Sizing.SUBSECTION_SPACING)
                    .Text("Detailed Score Descriptors")
                    .FontSize(Sizing.SECTION_TITLE_FONT_SIZE)
                    .Bold()
                    .FontColor(Colors.Black);

                // Score Descriptors
                var descriptors = new[]
                {
                    "10: Extremely High (higher potential than about 99% of the comparison group)",
                    "9: Very High (higher potential than about 95% of the comparison group)",
                    "8: High (higher potential than about 90% of the comparison group)",
                    "7: Fairly High (higher potential than about 75% of the comparison group)",
                    "6: Average (higher potential than about 60% of the comparison group)",
                    "5: Average (higher potential than about 40% of the comparison group)",
                    "4: Fairly Low (higher potential than about 25% of the comparison group)",
                    "3: Low (higher potential than about 10% of the comparison group)",
                    "2: Very Low (higher potential than about 5% of the comparison group)",
                    "1: Extremely Low (higher potential than about 1% of the comparison group)"
                };

                foreach (var line in descriptors)
                {
                    column.Item().PaddingBottom(Sizing.ITEM_SPACING / 2)
                        .Text(line)
                        .FontSize(Sizing.BODY_TEXT_FONT_SIZE)
                        .LineHeight(Sizing.LINE_HEIGHT);
                }

                // Scale Description
                column.Item().PaddingTop(Sizing.SUBSECTION_SPACING).PaddingBottom(Sizing.PARAGRAPH_SPACING)
                    .Text("The individual profile scores from the assessments have been compared with other individuals who have previously completed the assessment (more about this in the technical information section at the back of the report). Results are based on a 1 to 10 scale as shown below.")
                    .FontSize(Sizing.BODY_TEXT_FONT_SIZE)
                    .LineHeight(Sizing.LINE_HEIGHT);

                // Scale Labels Row
                column.Item().PaddingBottom(Sizing.ITEM_SPACING).Row(scaleRow =>
                {
                    scaleRow.RelativeItem(1).AlignCenter().PaddingLeft(-5).Text("Extremely\nLow").FontSize(Sizing.SMALL_TEXT_FONT_SIZE);
                    scaleRow.RelativeItem(1).AlignCenter().PaddingLeft(-5).Text("Very Low").FontSize(Sizing.SMALL_TEXT_FONT_SIZE);
                    scaleRow.RelativeItem(1).AlignCenter().PaddingLeft(-10).Text("Low").FontSize(Sizing.SMALL_TEXT_FONT_SIZE);
                    scaleRow.RelativeItem(1).AlignCenter().Text("Fairly Low").FontSize(Sizing.SMALL_TEXT_FONT_SIZE);
                    scaleRow.RelativeItem(2).AlignCenter().Text("Average").FontSize(Sizing.SMALL_TEXT_FONT_SIZE);
                    scaleRow.RelativeItem(1).AlignCenter().Text("Fairly High").FontSize(Sizing.SMALL_TEXT_FONT_SIZE);
                    scaleRow.RelativeItem(1).AlignCenter().Text("High").FontSize(Sizing.SMALL_TEXT_FONT_SIZE);
                    scaleRow.RelativeItem(1).AlignCenter().Text("Very High").FontSize(Sizing.SMALL_TEXT_FONT_SIZE);
                    scaleRow.RelativeItem(1).AlignCenter().PaddingRight(-10).Text("Extremely\nHigh").FontSize(Sizing.SMALL_TEXT_FONT_SIZE);
                });

                // Color Bar with Numbers
                column.Item()
                      .PaddingBottom(Sizing.ITEM_SPACING / 2)
                      .Row(colorRow =>
                      {
                          colorRow.Spacing(26); // Add 26pt gap between all boxes

                          var colors = new[] {
             "#E31D25", "#EB1C48", "#F15929", "#F59726", "#F3D520",
             "#F3D520", "#DAE241", "#D7D820", "#8CC63E", "#39B54A"
                          };
                          for (int i = 1; i <= 10; i++)
                          {
                              colorRow.ConstantItem(24)
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

                // Percentile Row
                string[] percentiles = { "1%", "5%", "10%", "25%", "40%", "60%", "75%", "90%", "95%", "99%" };
                column.Item().PaddingBottom(Sizing.PARAGRAPH_SPACING).Row(percentileRow =>
                {
                    percentileRow.Spacing(26); // Match the spacing from color bar

                    foreach (var p in percentiles)
                    {
                        percentileRow.ConstantItem(24).AlignCenter().AlignMiddle()
                            .Text(p).FontSize(Sizing.SMALL_TEXT_FONT_SIZE);
                    }
                });
                // Color Underline
                column.Item().PaddingBottom(Sizing.PARAGRAPH_SPACING).Row(underlineRow =>
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

                // Bottom Development Categories
                column.Item().PaddingBottom(Sizing.PARAGRAPH_SPACING).Row(bottomRow =>
                {
                    bottomRow.RelativeItem(2).AlignCenter().Text("Significant Development\nwould be needed")
                        .FontSize(Sizing.SMALL_TEXT_FONT_SIZE).Bold();
                    bottomRow.RelativeItem(2).AlignCenter().Text("Development\nwould be needed")
                        .FontSize(Sizing.SMALL_TEXT_FONT_SIZE).Bold();
                    bottomRow.RelativeItem(2).AlignCenter().Text("Effective")
                        .FontSize(Sizing.SMALL_TEXT_FONT_SIZE).Bold();
                    bottomRow.RelativeItem(2).AlignCenter().Text("Strength")
                        .FontSize(Sizing.SMALL_TEXT_FONT_SIZE).Bold();
                    bottomRow.RelativeItem(2).AlignCenter().Text("Significant\nstrength")
                        .FontSize(Sizing.SMALL_TEXT_FONT_SIZE).Bold();
                });
            });
        }
        #endregion

        #region Page 4 Methods

        public static void ComposePage4Content(IContainer container, TalentFitReport_Request request)
        {
            container.AlignCenter().MaxWidth(480).Column(column =>
            {
                // Title
                column.Item().PaddingBottom(Sizing.SUBSECTION_SPACING)
                    .Text(request.Page4.PageTitle)
                    .FontSize(Sizing.SECTION_TITLE_FONT_SIZE)
                    .Bold()
                    .FontColor(Colors.Black);

                // Introduction paragraphs
                column.Item().PaddingBottom(Sizing.PARAGRAPH_SPACING).Column(textCol =>
                {
                    textCol.Item().PaddingBottom(Sizing.ITEM_SPACING).Text(
                         $"Based on the provided information, {request.EmployeeName} scored Fairly High on the {request.Role} role fit, demonstrating higher potential than about 75% of the comparison group with a score of 7.1. Barry's culture fit is also Fairly High at 7.3, indicating better alignment than about 75% of the comparison group with Wamly."
                           )
                         .FontSize(Sizing.BODY_TEXT_FONT_SIZE)
                         .LineHeight(Sizing.LINE_HEIGHT);
                });

                // Summary Profile title
                column.Item().PaddingBottom(Sizing.ITEM_SPACING / 2)
                    .Text(request.Page4.SummaryProfileTitle)
                    .FontSize(Sizing.SUBSECTION_TITLE_FONT_SIZE)
                    .Bold()
                    .FontColor(Colors.Black)
                    .WrapAnywhere();

                // Talent Match Section
                column.Item().PaddingBottom(10)
                    .Element(c => ComposeTalentMatchSection(c, request));

                // First row of quadrants
                column.Item().Row(row =>
                {
                    row.RelativeItem(1).Element(c => ComposeQuadrantBox(c,
                        request.Page4.KeyStrengthTitle,
                         request.Page4.KeyStrengthData,
                        request.Page4.KeyStrengthContent,
                        "#39B54A"));

                    row.RelativeItem(1).PaddingLeft(13).Element(c => ComposeQuadrantBox(c,
                        request.Page4.DevelopmentNeedsTitle,
                         request.Page4.DevelopmentNeedsData,
                        request.Page4.DevelopmentNeedsContent,
                        "#F39726"));
                });


                // Second row of quadrants
                column.Item().Row(row =>
                {
                    row.RelativeItem(1).Element(c => ComposeQuadrantBox(c,
                       request.Page4.PotentialRisksTitle,
                        request.Page4.PotentialRisksData,
                       request.Page4.PotentialRisksContent,
                       "#E21E26"));

                    row.RelativeItem(1).PaddingLeft(13).Element(c => ComposeQuadrantBox(c,
                        request.Page4.UntappedPotentialTitle,
                        request.Page4.UntappedPotentialData,
                         request.Page4.UntappedPotentialContent,
                       "#D7D724"));
                });
            });
        }

        public static void ComposeQuadrantBox(IContainer container, string title, string data, List<string> items, string color)
        {
            container.PaddingLeft(3).Column(column =>
            {
                // Header
                column.Item()
                      .Width(220)
                      .Height(30)
                      .Background(color)
                      .CornerRadius(6)
                      .Padding(Sizing.ITEM_SPACING)
                      .AlignCenter()
                      .AlignMiddle()
                      .Text(title ?? "")
                      .FontSize(Sizing.SUBSECTION_TITLE_FONT_SIZE)
                      .Bold()
                      .FontColor(Colors.White);



                // Data section
                if (!string.IsNullOrWhiteSpace(data))
                {
                    column.Item().Width(220)
                        .PaddingVertical(Sizing.ITEM_SPACING / 2)
                        .PaddingHorizontal(Sizing.PARAGRAPH_SPACING)
                        .Background(Colors.White)
                        .Text(data)
                        .FontSize(Sizing.SMALL_TEXT_FONT_SIZE)
                        .FontColor(Colors.Grey.Darken2)
                        .LineHeight(Sizing.LINE_HEIGHT)
                        .WrapAnywhere();
                }

                // Content items
                column.Item()
                    .MinHeight(50)
                    .Background(Colors.White)
                    .Padding(Sizing.PARAGRAPH_SPACING)
                    .Column(contentCol =>
                    {
                        if (items != null && items.Any())
                        {
                            foreach (var item in items.Take(10))
                            {
                                if (!string.IsNullOrEmpty(item))
                                {
                                    contentCol.Item()
                                        .PaddingBottom(Sizing.ITEM_SPACING / 2)
                                        .Text(item)
                                        .FontSize(Sizing.SMALL_TEXT_FONT_SIZE)
                                        .LineHeight(Sizing.LINE_HEIGHT);
                                }
                            }

                            if (items.Count > 10)
                            {
                                contentCol.Item()
                                    .Text("...")
                                    .FontSize(Sizing.SMALL_TEXT_FONT_SIZE)
                                    .FontColor(Colors.Grey.Darken1);
                            }
                        }
                        else
                        {
                            contentCol.Item()
                                .AlignCenter()
                                .AlignMiddle()
                                .Text("")
                                .FontSize(Sizing.SMALL_TEXT_FONT_SIZE)
                                .FontColor(Colors.Grey.Medium);
                        }
                    });
            });
        }

        public static void ComposeTalentMatchSection(IContainer container, TalentFitReport_Request request)
        {
            container
                .Column(column =>
                {
                    // Full-width grey header bar
                    column.Item()
                        .ExtendHorizontal()                 //  Make the bar full width 
                        .Background(Colors.Grey.Darken2)
                        .CornerRadius(6)
                        .PaddingVertical(Sizing.ITEM_SPACING)
                        .AlignCenter()                      //  Center text 
                        .Text("ROLE SPECIFIC TALENT MATCH")
                        .FontSize(Sizing.SUBSECTION_TITLE_FONT_SIZE + 1)
                        .Bold()
                        .FontColor(Colors.White);

                    // Scale + Fit score  (INVERTED)
                    column.Item().PaddingLeft(-22).PaddingVertical(Sizing.PARAGRAPH_SPACING).Row(row =>
                    {
                        // LEFT SECTION — FIT SCORE CARD
                        row.RelativeItem(2).AlignMiddle().Column(scoreCol =>
                        {
                            scoreCol.Item().AlignCenter()
                                .Text("Fit for this role")
                                .FontSize(Sizing.SMALL_TEXT_FONT_SIZE);

                            scoreCol.Spacing(Sizing.PARAGRAPH_SPACING);

                            scoreCol.Item().PaddingTop(-8)
                                    .AlignCenter()
                                    .AlignMiddle()
                                    .Background("#dcddde")
                                    .Border(0.5f)
                                    .BorderColor("#dcddde")
                                    .CornerRadius(8)
                                    .MinWidth(50)
                                    .PaddingVertical(6)
                                    .PaddingHorizontal(16)
                                    .Text(request.Page4.FitForThisRole.ToString("0.0"))
                                    .FontSize(Sizing.SUBSECTION_TITLE_FONT_SIZE + 1)
                                    .Bold();
                        });

                        // DOTTED LINE
                        row.AutoItem()
                            .PaddingRight(30)
                            .LineVertical(3)
                            .LineDashPattern(new float[] { 4f, 4f })
                            .LineColor(Colors.Grey.Lighten1);

                        // RIGHT SECTION — SCALE + LABELS
                        row.RelativeItem(7).Column(labelScaleCol =>
                        {
                            // Labels
                            labelScaleCol.Item().Row(labelRow =>
                            {
                                labelRow.RelativeItem(2).AlignMiddle().Column(col =>
                                {
                                    col.Item().Text("Needs Attention")
                                        .FontSize(Sizing.SMALL_TEXT_FONT_SIZE)
                                        .SemiBold();
                                });

                                labelRow.RelativeItem(2).AlignMiddle().PaddingLeft(3).Column(col =>
                                {
                                    col.Item().AlignCenter().PaddingLeft(-30).Text("Moderate Match")
                                        .FontSize(Sizing.SMALL_TEXT_FONT_SIZE)
                                        .SemiBold();
                                });

                                labelRow.RelativeItem(2).AlignMiddle().PaddingRight(28).Column(col =>
                                {
                                    col.Item().AlignRight().Text("Strong Match")
                                        .FontSize(Sizing.SMALL_TEXT_FONT_SIZE)
                                        .SemiBold();
                                });
                            });

                            // 1–10 SCALE BAR (same, no inversion)
                            labelScaleCol.Item()
                             .PaddingTop(Sizing.ITEM_SPACING / 2)
                             .Row(scaleRow =>
                             {
                                 scaleRow.Spacing(11);

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
                    });


                    // Assessment table
                    column.Item().PaddingTop(Sizing.PARAGRAPH_SPACING).Column(assessCol =>
                    {
                        ComposeAssessmentRow(assessCol, "Culture Fit",
                            request.Page4.CultureFitScore,
                            request.Page4.CultureFitStyles);
                    });
                });
        }

        public static void ComposeAssessmentRow(ColumnDescriptor assessCol, string label, int score, string resultLabel)
        {
            assessCol.Item().PaddingBottom(Sizing.ITEM_SPACING).Column(col =>
            {
                col.Item().Row(row =>
                {
                    // LEFT → RESULT BOX
                    row.RelativeItem(2)
                        .PaddingTop(6)
                        .PaddingRight(22)
                        .AlignMiddle()
                        .AlignCenter()
                        .Background(resultLabel.Contains("High") ? Colors.Yellow.Lighten2 : Colors.Yellow.Lighten3)
                        .PaddingVertical(Sizing.ITEM_SPACING / 2)
                        .PaddingHorizontal(Sizing.PARAGRAPH_SPACING)
                        .Text(resultLabel)
                        .FontSize(Sizing.SMALL_TEXT_FONT_SIZE)
                        .Bold();

                    // LEFT → DOTTED LINE
                    row.AutoItem()
                        .PaddingRight(65)
                        .LineVertical(3)
                        .LineDashPattern(new float[] { 4f, 4f })
                        .LineColor(Colors.Grey.Lighten1);

                    // CENTER → SCALE + HEADINGS
                    row.RelativeItem(6).PaddingLeft(-36).Column(wordingCol =>
                    {
                        // Headings (Needs Attention | Moderate Match | Strong Match)
                        wordingCol.Item().Row(wordingRow =>
                        {
                            wordingRow.RelativeItem(3).AlignLeft()
                                .Text("Needs Attention")
                                .FontSize(Sizing.SMALL_TEXT_FONT_SIZE)
                                .SemiBold();

                            wordingRow.RelativeItem(3).AlignCenter()
                                .Text("Moderate Match")
                                .FontSize(Sizing.SMALL_TEXT_FONT_SIZE)
                                .SemiBold();

                            wordingRow.RelativeItem(3).AlignRight()
                                .Text("Strong Match")
                                .FontSize(Sizing.SMALL_TEXT_FONT_SIZE)
                                .SemiBold();
                        });

                        // Scale 1–10
                        wordingCol.Item().Row(scoreRow =>
                        {
                            for (int i = 1; i <= 10; i++)
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
                                    .FontSize(Sizing.SMALL_TEXT_FONT_SIZE)
                                    .Bold()
                                    .FontColor(textColor);
                            }
                        });
                    });

                    // RIGHT → LABEL (Culture Fit, Work Style, etc.)
                    row.RelativeItem(2).PaddingLeft(-10)
                        .AlignCenter()
                        .PaddingTop(12)
                        .Text(label)
                        .FontSize(Sizing.BODY_TEXT_FONT_SIZE)
                        .Bold();
                });
            });
        }


        public static string GetScaleColor(int value)
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
                10 => "#d7b226",
                _ => Colors.White
            };
        }

        #endregion

        #region Page 5 Methods
        public static void ComposePage5Content(IContainer container, TalentFitReport_Request request)
        {
            container.AlignCenter().MaxWidth(480).Column(column =>
            {
                // Page Title
                column.Item().PaddingBottom(Sizing.SECTION_SPACING)
                    .Text(request.Page5.ProfileName)
                    .FontSize(Sizing.SECTION_TITLE_FONT_SIZE)
                    .Bold()
                    .FontColor(Colors.Black);

                // Description
                column.Item().PaddingBottom(Sizing.PARAGRAPH_SPACING).Text(text =>
                {
                    text.Span("This profile provides a summary of your preferences and capabilities compared against the essential and important work-related behaviors and abilities required for success in the role of ")
                        .FontSize(Sizing.BODY_TEXT_FONT_SIZE).LineHeight(Sizing.LINE_HEIGHT);
                    text.Span($"{request.CompanyName}").Bold().FontSize(Sizing.BODY_TEXT_FONT_SIZE);
                    text.Span(" - ").FontSize(Sizing.BODY_TEXT_FONT_SIZE);
                    text.Span($"{request.Position}").Bold().FontSize(Sizing.BODY_TEXT_FONT_SIZE);
                    text.Span(".").FontSize(Sizing.BODY_TEXT_FONT_SIZE);
                });

                // Scale Labels Above Tables
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

                //  Section1
                if (request.Page5.Section1?.Any() == true)
                {
                    column.Item().PaddingBottom(Sizing.SUBSECTION_SPACING)
                        .Element(c => ComposeProfileSection(c,
                            $"{request.Page5.Section1Name}",
                            request.Page5.Section1,
                            "#6B7280"));
                }

                //  Section2
                if (request.Page5.Section2?.Any() == true)
                {
                    column.Item().PaddingBottom(Sizing.SUBSECTION_SPACING)
                        .Element(c => ComposeProfileSection(c,
                            $"{request.Page5.Section2Name}",
                            request.Page5.Section2,
                            "#6B7280"));
                }
            });
        }

        public static void ComposeProfileSection(IContainer container, string sectionTitle, List<ProfileItem> items, string headerColor)
        {
            container.Column(column =>
            {
                column.Item().Column(tableColumn =>
                {
                    // Section Header with Color Scale
                    tableColumn.Item().Row(headerRow =>
                    {
                        // Section Title
                        headerRow.ConstantItem(240).Background(headerColor)
                            .CornerRadius(4).PaddingHorizontal(Sizing.PARAGRAPH_SPACING)
                            .PaddingVertical(Sizing.ITEM_SPACING)
                            .Text(sectionTitle)
                            .FontSize(Sizing.SUBSECTION_TITLE_FONT_SIZE)
                            .Bold()
                            .FontColor(Colors.White);

                        // Color Scale
                        // 1. Header Row
                        headerRow.RelativeItem().Row(scaleRow =>
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

                    // Data Rows
                    foreach (var item in items)
                    {
                        tableColumn.Item().Row(dataRow =>
                        {
                            // Item Name
                            dataRow.ConstantItem(240).Background(Colors.White)
                                .PaddingHorizontal(Sizing.PARAGRAPH_SPACING)
                                .PaddingVertical(Sizing.ITEM_SPACING)
                                .AlignMiddle()
                                .Text(item.Name)
                                .FontSize(Sizing.BODY_TEXT_FONT_SIZE);

                            // Score Scale
                            dataRow.RelativeItem().Background(Colors.White).Row(scoreRow =>
                            {
                                for (int i = 1; i <= 10; i++)
                                {
                                    var greyColumns = new int[] { 1, 2, 5, 6, 9, 10 };
                                    var cellBg = greyColumns.Contains(i) ? "#E5E5E5" : "#FFFFFF";

                                    scoreRow.RelativeItem().Height(24)
                                        .Background(cellBg)
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

                        // Horizontal Separator
                        tableColumn.Item().PaddingLeft(5).Width(473.9f).LineHorizontal(2f).LineColor("#C8C8CA");
                    }
                });
            });
        }

        public static string GetScoreColor(int score)
        {
            var colors = new[]
            {
                "#E31D25", "#EB1C48", "#F15929", "#F59726", "#F3D520",
                "#F3D520", "#DAE241", "#D7D820", "#8CC63E", "#39B54A"
            };
            return colors[Math.Max(0, Math.Min(9, score - 1))];
        }
        #endregion

        #region Last Methods
        public static void ComposeLastPageContent(IContainer container, TalentFitReport_Request request)
        {
            container.AlignCenter().MaxWidth(480).Column(column =>
            {
                // Page Title
                column.Item().PaddingBottom(Sizing.SECTION_SPACING)
                    .Text("Technical Information")
                    .FontSize(Sizing.SECTION_TITLE_FONT_SIZE)
                    .Bold()
                    .FontColor(Colors.Black);

                // JOB/ROLE DATA Section
                column.Item().PaddingBottom(Sizing.SECTION_SPACING).Column(section =>
                {
                    // Section Header
                    section.Item().Background("#6A7282").CornerRadius(4)
                        .Padding(Sizing.ITEM_SPACING)
                        .Text("JOB / ROLE DATA")
                        .FontSize(Sizing.BODY_TEXT_FONT_SIZE)
                        .Bold()
                        .FontColor(Colors.White);

                    // Table Content
                    section.Item().Padding(Sizing.PARAGRAPH_SPACING).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(120);
                            columns.ConstantColumn(15);
                            columns.RelativeColumn();
                            columns.ConstantColumn(15);
                            columns.ConstantColumn(80);
                        });
                        // Row 1
                        table.Cell().Text("Job or role involved").FontSize(Sizing.SMALL_TEXT_FONT_SIZE);
                        table.Cell().AlignCenter().LineVertical(1).LineDashPattern([2f, 2f]).LineColor("#C8C8CA");
                        table.Cell().Text($"{request.CompanyName ?? "-"} - {request.Position ?? "-"}")
                            .FontSize(Sizing.SMALL_TEXT_FONT_SIZE);
                        table.Cell().RowSpan(2).AlignCenter().LineVertical(1).LineDashPattern([2f, 2f]).LineColor("#C8C8CA"); // Added RowSpan(2)
                        table.Cell().AlignLeft().Text(request.LastPage?.JobAnalysisDate ?? "-").FontSize(Sizing.SMALL_TEXT_FONT_SIZE);

                        // Row 2
                        table.Cell().Text("Job Analysis").FontSize(Sizing.SMALL_TEXT_FONT_SIZE);
                        table.Cell().AlignCenter().LineVertical(1).LineDashPattern([2f, 2f]).LineColor("#C8C8CA");
                        table.Cell().Text(request.LastPage?.JobAnalysis ?? "-").FontSize(Sizing.SMALL_TEXT_FONT_SIZE);
                        // Note: No cell here for the separator because it's handled by RowSpan(2) above
                        table.Cell().AlignRight().Text("").FontSize(Sizing.SMALL_TEXT_FONT_SIZE); // Or use appropriate date field
                    });
                });

                // ASSESSMENT METHODS Section
                column.Item().PaddingBottom(Sizing.SECTION_SPACING).Column(section =>
                {
                    // Section Header
                    section.Item().Background("#6A7282").CornerRadius(4)
                        .Padding(Sizing.ITEM_SPACING)
                        .Text("ASSESSMENT METHODS")
                        .FontSize(Sizing.BODY_TEXT_FONT_SIZE)
                        .Bold()
                        .FontColor(Colors.White);

                    // Table Content
                    section.Item().Padding(Sizing.PARAGRAPH_SPACING).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(120);
                            columns.ConstantColumn(15);
                            columns.RelativeColumn();
                            columns.ConstantColumn(15);
                            columns.ConstantColumn(80);
                        });

                        // Header Row
                        table.Cell().Text("Test").FontSize(Sizing.SMALL_TEXT_FONT_SIZE).Bold();
                        table.Cell();
                        table.Cell().Text("Details").FontSize(Sizing.SMALL_TEXT_FONT_SIZE).Bold();
                        table.Cell();
                        table.Cell().Text("Date").FontSize(Sizing.SMALL_TEXT_FONT_SIZE).Bold();

                        // Assessment Rows
                        if (request.LastPage?.Assessments != null && request.LastPage.Assessments.Any())
                        {
                            foreach (var test in request.LastPage.Assessments)
                            {
                                table.Cell().PaddingTop(2).PaddingBottom(3).Text(test.TestName ?? "-").FontSize(Sizing.SMALL_TEXT_FONT_SIZE);
                                table.Cell().AlignCenter().LineVertical(1).LineDashPattern([2f, 2f]).LineColor("#C8C8CA");
                                table.Cell().PaddingTop(2).PaddingBottom(3).Text(text =>
                                {
                                    if (!string.IsNullOrEmpty(test.Norm))
                                        text.Span($"Norm: {test.Norm}\n").FontSize(Sizing.SMALL_TEXT_FONT_SIZE);
                                    if (!string.IsNullOrEmpty(request.EmployeeName))
                                        text.Span($"Completed by: {request.EmployeeName}").FontSize(Sizing.SMALL_TEXT_FONT_SIZE);
                                });
                                table.Cell().AlignCenter().LineVertical(1).LineDashPattern([2f, 2f]).LineColor("#C8C8CA"); ;
                                table.Cell().PaddingTop(2).PaddingBottom(3).Text(test.Date ?? "-").FontSize(Sizing.SMALL_TEXT_FONT_SIZE);

                                // Separator Line
                                table.Cell().ColumnSpan(5).LineHorizontal(0.5f).LineColor("#C8C8CA");
                            }
                        }
                        else
                        {
                            table.Cell().ColumnSpan(5).Text("No assessments available").FontSize(Sizing.SMALL_TEXT_FONT_SIZE);
                        }
                    });
                });

                // ==========================
                // RESPONSE QUALITY INDICATORS
                // ==========================
                column.Item().PaddingBottom(Sizing.SECTION_SPACING).Column(section =>
                {
                    // Section Header
                    section.Item().Background("#6A7282").CornerRadius(4)
                        .Padding(Sizing.ITEM_SPACING)
                        .Text("RESPONSE QUALITY INDICATORS")
                        .FontSize(Sizing.BODY_TEXT_FONT_SIZE)
                        .Bold()
                        .FontColor(Colors.White);

                    // Section Body
                    section.Item().Padding(Sizing.PARAGRAPH_SPACING).Column(inner =>
                    {
                        // ==================================================
                        // ABILITY SUBTEST TABLE (FULL GRID)
                        // ==================================================
                        inner.Item().PaddingBottom(20).Table(table =>
                        {
                            // -------------------------------
                            // Column Definitions
                            // -------------------------------
                            table.ColumnsDefinition(cols =>
                            {
                                cols.ConstantColumn(120); // Ability
                                cols.ConstantColumn(55);  // Percentile
                                cols.ConstantColumn(35);  // Pace

                                // 10 marker columns (consistent with header)
                                for (int i = 0; i < 10; i++)
                                    cols.ConstantColumn(25);
                            });

                            // -------------------------------
                            // HEADER CELL FUNCTION
                            // -------------------------------
                            void Header(string text, bool center = false)
                            {
                                var cell = table.Cell()
                                    .Border(0.5f)
                                    .BorderColor(Colors.Grey.Lighten2)
                                    .Background("#8C919D")
                                    .CornerRadius(4)
                                    .Padding(3)
                                    .AlignMiddle();

                                if (center) cell.AlignCenter();

                                cell.Text(text)
                                    .FontColor(Colors.White)
                                    .FontSize(Sizing.SMALL_TEXT_FONT_SIZE)
                                    .Bold();
                            }

                            // -------------------------------
                            // HEADER ROW
                            // -------------------------------
                            Header("Ability Subtest");
                            Header("Percentile", center: true);
                            Header("Pace", center: true);

                            var colors = new[]
                            {
                                 "#E31D25", "#EB1C48", "#F15929", "#F59726", "#F3D520",
                                 "#F3D520", "#DAE241", "#D7D820", "#8CC63E", "#39B54A"
                             };

                            for (int i = 1; i <= 10; i++)
                            {
                                table.Cell()
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

                            // -------------------------------
                            // DATA ROWS (CORRECT TABLE FORMAT)
                            // -------------------------------
                            if (request.LastPage?.AbilityScores != null)
                            {
                                foreach (var a in request.LastPage.AbilityScores)
                                {
                                    // Ability Name
                                    table.Cell().Element(cell =>
                                    {
                                        cell.Row(row =>
                                        {   
                                            // Actual text container
                                            row.RelativeItem().Padding(3)
                                               .AlignLeft().AlignMiddle()
                                               .Text(a.AbilityName ?? "-")
                                               .FontSize(Sizing.SMALL_TEXT_FONT_SIZE);

                                            // Draw dashed vertical line
                                            row.ConstantItem(1).Element(e =>
                                            {
                                                e.TranslateX(0.5f).LineVertical(1)
                                                 .LineDashPattern(new float[] { 2, 2 })
                                                 .LineColor("#C8C8CA");
                                            });
                                        });
                                    });

                                    // Percentile
                                    table.Cell().Element(cell =>
                                    {
                                        cell.Row(row =>
                                        {  
                                            row.RelativeItem().Padding(3)
                                               .AlignCenter().AlignMiddle()
                                               .Text(a.Percentile.ToString())
                                               .FontSize(Sizing.SMALL_TEXT_FONT_SIZE);
                                            row.ConstantItem(1).Element(e =>
                                            {
                                                e.TranslateX(0.5f).LineVertical(1)
                                                 .LineDashPattern(new float[] { 2, 2 })
                                                 .LineColor("#C8C8CA");
                                            });
                                        });
                                    });


                                    table.Cell().Element(cell =>
                                    {
                                        cell.Row(row =>
                                        {   
                                            row.RelativeItem().Padding(3)
                                               .AlignCenter().AlignMiddle()
                                               .Text(a.Pace.ToString())
                                               .FontSize(Sizing.SMALL_TEXT_FONT_SIZE);
                                            //row.ConstantItem(1).Element(e =>
                                            //{
                                            //    e.TranslateX(0.5f).LineVertical(1)
                                            //     .LineDashPattern(new float[] { 2, 2 })
                                            //     .LineColor("#C8C8CA");
                                            //});
                                        });
                                    });

                                    // 10 MARKER CELLS
                                    for (int pos = 1; pos <= 10; pos++)
                                    {
                                        bool highlight = pos == a.MarkerPosition;
                                        var greyColumns = new[] { 1, 2, 5, 6, 9, 10 };

                                        string bg = greyColumns.Contains(pos)
                                            ? "#E5E5E5"
                                            : "#FFFFFF";

                                       table.Cell().Padding(0)                 // critical – removes tiny inner gaps
                                            .Border(0)                  // remove leftover cell border
                                            .Background(bg)
                                            .AlignCenter().AlignMiddle()
                                            .Height(22)
                                            .Element(cell =>
                                            {
                                                if (highlight)
                                                {
                                                    cell.PaddingTop(6).Width(10).Height(10)
                                                        .Background(GetScoreColor(pos))
                                                        .Border(1)
                                                        .BorderColor("#BDBEC0")
                                                        .CornerRadius(6);
                                                }
                                            }); 
                                    }

                                    // SEPARATOR ROW (FULL WIDTH)
                                    table.Cell().ColumnSpan(13)
                                        .Element(e =>
                                        {
                                            e.LineHorizontal(1.2f).LineColor("#C8C8CA");
                                        });

                                }
                            }
                        });

                        // ==================================================
                        // PERSONALITY TABLE (FULL GRID)
                        // ==================================================
                        inner.Item().PaddingBottom(20).Table(table =>
                        {
                            table.ColumnsDefinition(cols =>
                            {
                                cols.ConstantColumn(170); // Trait name
                                cols.ConstantColumn(40);  // Score

                                // 10 score markers
                                for (int i = 0; i < 10; i++)
                                    cols.ConstantColumn(25);
                            });

                            // -------------------------------
                            // HEADER CELL FUNCTION
                            // -------------------------------
                            void Header(string text, bool center = false)
                            {
                                var cell = table.Cell()
                                    .Border(0.5f)
                                    .BorderColor(Colors.Grey.Lighten2)
                                    .Background("#8C919D")
                                    .CornerRadius(4)
                                    .Padding(3)
                                    .AlignMiddle();

                                if (center) cell.AlignCenter();

                                cell.Text(text)
                                    .FontColor(Colors.White)
                                    .FontSize(Sizing.SMALL_TEXT_FONT_SIZE)
                                    .Bold();
                            }

                            // -------------------------------
                            // HEADER ROW
                            // -------------------------------
                            Header("Personality");
                            Header("Score", center: true);

                            var colors = new[]
                            {
                                "#E31D25", "#EB1C48", "#F15929", "#F59726", "#F3D520",
                                "#F3D520", "#DAE241", "#D7D820", "#8CC63E", "#39B54A"
                            };

                            // Score scale header
                            for (int i = 1; i <= 10; i++)
                            {
                                table.Cell()
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

                            // -------------------------------
                            // DATA ROWS
                            // -------------------------------
                            if (request.LastPage?.PersonalityScores != null)
                            {
                                foreach (var p in request.LastPage.PersonalityScores)
                                {
                                    table.Cell().Element(cell =>
                                    {
                                        cell.Row(row =>
                                        {
                                            row.RelativeItem().Padding(3)
                                               .AlignLeft().AlignMiddle()
                                               .Text(p.TraitName ?? "-")
                                               .FontSize(Sizing.SMALL_TEXT_FONT_SIZE);
                                            row.ConstantItem(1).Element(e =>
                                            {
                                                e.TranslateX(0.5f).LineVertical(1)
                                                 .LineDashPattern(new float[] { 2, 2 })
                                                 .LineColor("#C8C8CA");
                                            });
                                        });
                                    });


                                    table.Cell().Element(cell =>
                                    {
                                        cell.Row(row =>
                                        {
                                            row.RelativeItem().Padding(3)
                                               .AlignCenter().AlignMiddle()
                                               .Text(p.Score.ToString())
                                               .FontSize(Sizing.SMALL_TEXT_FONT_SIZE);
                                            //row.ConstantItem(1).Element(e =>
                                            //{
                                            //    e.TranslateX(0.5f).LineVertical(1)
                                            //     .LineDashPattern(new float[] { 2, 2 })
                                            //     .LineColor("#C8C8CA");
                                            //});
                                        });
                                    });                                    

                                    // Score markers
                                    for (int pos = 1; pos <= 10; pos++)
                                    {
                                        bool highlight = (pos == p.Score);
                                        var greyColumns = new[] { 1, 2, 5, 6, 9, 10 };
                                        string bg = greyColumns.Contains(pos) ? "#E5E5E5" : "#FFFFFF";

                                        table.Cell()
                                          .Padding(0)                 // critical – removes tiny inner gaps
                                            .Border(0)                  // remove leftover cell border
                                           .Background(bg)
                                           .AlignCenter().AlignMiddle()
                                           .Height(22)
                                           .Element(cell =>
                                           {
                                               if (highlight)
                                               {
                                                   cell.PaddingTop(6).Width(10).Height(10)
                                                       .Background(GetScoreColor(pos))
                                                       .Border(1)
                                                       .BorderColor("#BDBEC0")
                                                       .CornerRadius(6);
                                               }
                                           });
                                    }

                                    // ----------------------------------------
                                    // SEPARATOR LINE (full row span)
                                    // ----------------------------------------
                                    table.Cell().ColumnSpan(12)
                                        .Element(e =>
                                        {
                                            e.LineHorizontal(1.2f)
                                             .LineColor("#C8C8CA");
                                        });
                                }
                            }
                            else
                            {
                                table.Cell().ColumnSpan(12)
                                    .Border(0.5f).BorderColor(Colors.Grey.Lighten2)
                                    .Padding(4)
                                    .Text("No personality data available")
                                    .FontColor(Colors.Grey.Darken1);
                            }
                        });

                    });



                });

                // INPUT DATA Section
                column.Item().Column(section =>
                {
                    // Section Header
                    section.Item().Background("#6A7282").CornerRadius(4)
                        .Padding(Sizing.ITEM_SPACING)
                        .Text("INPUT DATA")
                        .FontSize(Sizing.BODY_TEXT_FONT_SIZE)
                        .Bold()
                        .FontColor(Colors.White);

                    // Content
                    section.Item().Padding(Sizing.PARAGRAPH_SPACING).Column(inner =>
                    {
                        inner.Item()
                            .Text(request.LastPage?.InputData ?? "-")
                            .FontSize(Sizing.SMALL_TEXT_FONT_SIZE)
                            .LineHeight(Sizing.LINE_HEIGHT);

                        if (!string.IsNullOrEmpty(request.LastPage?.TemplateVersion))
                        {
                            inner.Item().PaddingTop(Sizing.PARAGRAPH_SPACING)
                                .Text(request.LastPage.TemplateVersion)
                                .FontSize(Sizing.SMALL_TEXT_FONT_SIZE)
                                .LineHeight(Sizing.LINE_HEIGHT);
                        }
                    });
                });
            });
        }
        #endregion

        #region Helper Methods

        // Unified Scale Bar Helper Method
        public static void RenderScaleBar(IContainer container)
        {
            var colors = new[] {
        "#E31D25", "#EB1C48", "#F15929", "#F59726", "#F3D520",
        "#F3D520", "#DAE241", "#D7D820", "#8CC63E", "#39B54A"
    };

            const float boxWidth = 24f;
            const float boxHeight = 24f;
            const float cornerRadius = 4f;

            container.Row(scaleRow =>
            {
                for (int i = 1; i <= 10; i++)
                {
                    scaleRow.ConstantItem(boxWidth)
                        .Height(boxHeight)
                        .Background(colors[i - 1])
                        .CornerRadius(cornerRadius)
                        .AlignCenter()
                        .AlignMiddle()
                        .Text(i.ToString())
                        .FontSize(Sizing.SMALL_TEXT_FONT_SIZE)
                        .FontColor(Colors.White)
                        .Bold();
                }
            });
        }

        /// <summary>
        /// Validates if the provided string is a valid base64 string
        /// </summary>
        /// <param name="base64String">The base64 string to validate</param>
        /// <returns>True if valid base64, false otherwise</returns>
        public static bool IsValidBase64(string base64String)
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
        public static string ExtractBase64FromDataUrl(string base64String)
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

        #region Generic Header/Footer Methods from page 2-7
        /// <summary>
        /// Composes the header section for a PDF page, including employee information, company details, position, and company logo.
        /// </summary>
        /// <param name="container">The <see cref="IContainer"/> used to compose the header layout.</param>
        /// <param name="request">The <see cref="TalentFitReport_DevelopmentRequest"/> containing all data required for the header.</param>
        public static void ComposePageHeader(IContainer container, TalentFitReport_Request request)
        {
            container
                .AlignCenter().MaxWidth(480)
                .Height(55) // Slightly increased to give breathing room
                .Background(Colors.White)
                .BorderBottom(1)
                .BorderColor(Colors.Grey.Lighten2)
                .Row(row =>
                {
                    // Left side: Employee info + Company + Position
                    row.RelativeItem()
                    .PaddingTop(8)
                        .PaddingLeft(0)
                        .PaddingVertical(10)
                        .Column(col =>
                        {
                            col.Item().Text(text =>
                            {
                                text.DefaultTextStyle(x => x.FontSize(9).FontColor(Colors.Black));

                                text.Span(request.EmployeeName).Bold();
                                text.Span(" | ");
                                text.Span($"{request.ReportDate:dd MMM yyyy}");
                                text.Span(" | ");
                                text.Span(request.ReportType).Bold();
                            });

                            col.Item().Text($"{request.CompanyName} - {request.Position}")
                                .FontSize(8)
                                .FontColor(Colors.Black)
                                .WrapAnywhere();
                        });

                    // Right side: Logo + optional Wamly text
                    row.ConstantItem(250) // give more width to accommodate larger logo
                        .PaddingRight(-60)
                        .PaddingVertical(6)
                        .AlignRight()
                        .AlignMiddle()
                        .Row(rightRow =>
                        {
                            // Larger logo, but controlled by FitWidth instead of FitHeight
                            const float logoWidth = 100;  // increased width
                            const float logoHeight = 42; // increased height safely

                            rightRow.AutoItem().AlignMiddle().Container().Width(logoWidth).Height(logoHeight).Element(logo =>
                            {
                                if (!string.IsNullOrEmpty(request.CompanyLogo) && TalentReport.IsValidBase64(request.CompanyLogo))
                                {
                                    try
                                    {
                                        var cleanBase64 = TalentReport.ExtractBase64FromDataUrl(request.CompanyLogo);
                                        var imageBytes = Convert.FromBase64String(cleanBase64);
                                        logo.Image(imageBytes, ImageScaling.FitArea); // ✅ FitArea keeps proportion
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
        /// <param name="request">The <see cref="TalentFitReport_DevelopmentRequest"/> containing all data required for the footer.</param>
        /// <param name="pageNumber">The current page number to display in the footer.</param>
        public static void ComposePageFooter(IContainer container, TalentFitReport_Request request, int pageNumber)
        {
            container
                .AlignCenter().MaxWidth(480)
                .Background(Colors.White)
                .PaddingHorizontal(0)
                .PaddingVertical(0)
                .Column(column =>
                {
                    // --- Top thin grey line ---
                    column.Item().PaddingTop(-10)
                        .Height(1)
                        .Background(Colors.Grey.Lighten2).ExtendHorizontal();

                    // --- Footer content row ---
                    column.Item().Row(row =>
                    {
                        // --- Left: Logo ---
                        row.RelativeItem().PaddingTop(20).PaddingLeft(0).AlignLeft().Container().Height(55).Width(60).Element(element =>
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

                        // --- Center: URL ---
                        row.RelativeItem().PaddingTop(-8).AlignMiddle().AlignCenter()
                            .Text(request.FooterUrl ?? "www.talentsolved.com")
                            .FontSize(9)
                            .FontColor(Colors.Grey.Darken1);

                        // --- Right: Page number ---
                        row.RelativeItem().PaddingTop(-8).PaddingRight(20).AlignRight().AlignMiddle()
                            .Text(pageNumber.ToString())
                            .FontSize(10)
                            .FontColor(Colors.Grey.Darken1);
                    });
                });
        }

        #endregion
    }
}
