using Library.Assess.Models.PersonalityWheel;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Linq;

namespace Library.Assess.Services.PersonalityWheel
{
    public class PersonalityWheel
    {
        // List of high-level categories (e.g., Delivering Results, Solving Problems, etc.)
        public List<HighLevelCategory> Categories { get; set; } = new();

        // Array of colors for 10 concentric levels (center → outer ring)
        public Color[] LevelColors { get; set; } = new Color[10];

        // Dictionary to store category-specific colors
        public Dictionary<string, Color> CategoryColors { get; set; } = new();

        // Whether to show numbers inside level circles
        public bool ShowLevelNumbers { get; set; } = true; // Changed to true by default

        // Whether to show grid dividers
        public bool ShowGrid { get; set; } = true;

        // List of competencies to hide/exclude
        public HashSet<string> ExcludedCompetencies { get; set; } = new HashSet<string>();

        /// <summary>
        /// Initializes default colors (purple gradient + category-specific colors).
        /// </summary>
        public PersonalityWheel()
        {
            InitializeDefaultColors();
        }

        /// <summary>
        /// Initializes default color scheme (10 purple shades and predefined category colors).
        /// </summary>
        private void InitializeDefaultColors()
        {
            // Define a gradient of 10 purple shades from white → deep purple
            var purpleColors = new[]
            {
                Color.FromArgb(255, 255, 255),  // 1 - White center
                Color.FromArgb(245, 242, 248),  // 2
                Color.FromArgb(235, 229, 241),  // 3
                Color.FromArgb(221, 212, 232),  // 4
                Color.FromArgb(203, 190, 219),  // 5
                Color.FromArgb(183, 166, 204),  // 6
                Color.FromArgb(161, 141, 186),  // 7
                Color.FromArgb(139, 116, 168),  // 8
                Color.FromArgb(117, 91, 150),   // 9
                Color.FromArgb(95, 66, 132)     // 10 - Deep purple outer
            };

            for (int i = 0; i < 10; i++)
            {
                LevelColors[i] = purpleColors[i];
            }

            CategoryColors = new Dictionary<string, Color>
            {
                ["DELIVERING RESULTS"] = Color.FromArgb(76, 175, 80),    // Green
                ["SOLVING PROBLEMS"] = Color.FromArgb(33, 150, 243),     // Blue
                ["INFLUENCING PEOPLE"] = Color.FromArgb(244, 67, 54),    // Red
                ["ADAPTING APPROACHES"] = Color.FromArgb(255, 152, 0)    // Orange
            };
        }

        /// <summary>
        /// Renders the full personality wheel visualization into a <see cref="Bitmap"/> image.  
        /// </summary>
        /// <param name="size">
        /// The width and height of the bitmap in pixels. The wheel will be scaled 
        /// proportionally to fit within this square area.
        /// </param>
        /// <returns>
        /// A <see cref="Bitmap"/> containing the fully drawn wheel, including category 
        /// backgrounds, concentric level circles, median cut line, competency bars, 
        /// dividers, score labels (if enabled), and text labels.
        /// </returns>
        /// <remarks>
        /// The method configures high-quality graphics settings (smoothing, anti-aliasing,
        /// interpolation, etc.), applies a slight rotation to the entire wheel for 
        /// aesthetics, and layers the visual elements in the following order:
        /// <list type="number">
        ///   <item><description>Category sector backgrounds</description></item>
        ///   <item><description>Concentric level circles</description></item>
        ///   <item><description>Median scoring cut sector</description></item>
        ///   <item><description>Radial bar dividers</description></item>
        ///   <item><description>Competency bars</description></item>
        ///   <item><description>Dynamic score labels (optional)</description></item>
        ///   <item><description>Category, subcategory, and competency labels</description></item>
        /// </list>
        /// </remarks>
        public Bitmap Draw(int size)
        {
            var bitmap = new Bitmap(size, size);
            using var graphics = Graphics.FromImage(bitmap);
            // Configure graphics quality settings
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            // White background
            graphics.Clear(Color.White);
            // Center coordinates
            var centerX = size / 2f;
            var centerY = size / 2f;
            // Max radius of the wheel (slightly reduced for margins)
            var maxRadius = Math.Min(centerX, centerY) * 0.58f;

            // Rotate entire wheel slightly (+5°)
            graphics.TranslateTransform(centerX, centerY);
            graphics.RotateTransform(+5f);
            graphics.TranslateTransform(-centerX, -centerY);

            // Draw layered components
            DrawCategorySectorBackgrounds(graphics, centerX, centerY, maxRadius);
            DrawLevelCircles(graphics, centerX, centerY, maxRadius);
            CutMedianSector(graphics, centerX, centerY, maxRadius);
            DrawBarDividers(graphics, centerX, centerY, maxRadius);
            DrawCompetencyBars(graphics, centerX, centerY, maxRadius);

            // NEW: Draw dynamic scoring labels on all category lines
            if (ShowLevelNumbers)
            {
                DrawDynamicScoreLabels(graphics, centerX, centerY, maxRadius);
            }
            // Draw category, subcategory, and competency labels
            DrawLabels(graphics, centerX, centerY, maxRadius);

            return bitmap;
        }

        /// <summary>
        /// Draws semi-transparent background sectors for each high-level category
        /// in the wheel visualization.  
        /// 
        /// Each category is assigned a distinct color and rendered as a wide
        /// pie slice spanning its allocated angular range. These backgrounds
        /// provide subtle visual grouping for the competencies within each category.
        /// </summary>
        /// <param name="graphics">The <see cref="Graphics"/> surface used for drawing.</param>
        /// <param name="centerX">The X coordinate of the wheel's center.</param>
        /// <param name="centerY">The Y coordinate of the wheel's center.</param>
        /// <param name="maxRadius">The maximum outer radius of the wheel.</param>
        private void DrawCategorySectorBackgrounds(Graphics graphics, float centerX, float centerY, float maxRadius)
        {
            var totalAngle = 360f;
            var anglePerCategory = totalAngle / Categories.Count;

            var innerRadius = 0f;
            var outerRadius = maxRadius;

            float currentAngle = -90f; // Start from top

            foreach (var category in Categories)
            {
                var categoryColor = CategoryColors[category.Name];
                // Very transparent brush for background
                using var brush = new SolidBrush(Color.FromArgb(8, categoryColor));

                var rect = new RectangleF(centerX - outerRadius, centerY - outerRadius, outerRadius * 2, outerRadius * 2);
                graphics.FillPie(brush, rect, currentAngle, anglePerCategory);

                currentAngle += anglePerCategory;
            }
        }

        /// <summary>
        /// Draws 10 concentric circular scoring levels around the wheel, 
        /// each filled with a gradient color and outlined with a grey border.  
        /// 
        /// The innermost region is filled with solid white to create
        /// a clean hollow space at the wheel’s center.
        /// </summary>
        /// <param name="graphics">The <see cref="Graphics"/> surface used for drawing.</param>
        /// <param name="centerX">The X coordinate of the wheel's center.</param>
        /// <param name="centerY">The Y coordinate of the wheel's center.</param>
        /// <param name="maxRadius">The maximum outer radius of the wheel.</param>
        private void DrawLevelCircles(Graphics graphics, float centerX, float centerY, float maxRadius)
        {
            var levelStep = maxRadius / 10f; // Distance between levels
            var innerCutRadius = levelStep * 2.2f; // Inner hollow space
            var scoringStep = levelStep;

            using var outlinePen = new Pen(ColorTranslator.FromHtml("#bbbdbe"), 1f); // Solid grey outline

            // Draw concentric scoring levels
            for (int level = 10; level >= 1; level--)
            {
                var radius = innerCutRadius + (level * scoringStep);
                var rect = new RectangleF(centerX - radius, centerY - radius, radius * 2, radius * 2);

                // Fill with gradient (or predefined) color
                using var levelBrush = new SolidBrush(LevelColors[level - 1]);
                graphics.FillEllipse(levelBrush, rect);

                // Outline circle
                graphics.DrawEllipse(outlinePen, rect);
            }

            // Fill the inner circle with white
            using var innerBrush = new SolidBrush(Color.White);
            var innerRect = new RectangleF(
                centerX - innerCutRadius,
                centerY - innerCutRadius,
                innerCutRadius * 2,
                innerCutRadius * 2
            );
            graphics.FillEllipse(innerBrush, innerRect);

            // Optional: draw a border for inner circle (cleaner edge)
            graphics.DrawEllipse(outlinePen, innerRect);
        }

        /// <summary>
        /// Cuts out a thin median scoring sector (0–10 scale) from the wheel visualization
        /// and draws two boundary lines on its edges.  
        /// 
        /// The cut represents a neutral reference line to help visually align and interpret
        /// competency scores around the wheel.
        /// </summary>
        /// <param name="graphics">The <see cref="Graphics"/> surface used for drawing.</param>
        /// <param name="centerX">The X coordinate of the wheel's center.</param>
        /// <param name="centerY">The Y coordinate of the wheel's center.</param>
        /// <param name="maxRadius">The maximum outer radius of the wheel.</param>
        private void CutMedianSector(Graphics graphics, float centerX, float centerY, float maxRadius)
        {
            var levelStep = maxRadius / 10f;
            float innerCutRadius = levelStep * 2.2f;

            // Same divider as score labels
            int startPosition = 0;
            var totalCompetencies = Categories.Sum(c => c.GetTotalCompetencyCount());
            var angleStep = 360f / totalCompetencies;
            float categoryAngle = startPosition * angleStep - 90 - (angleStep * 0.5f);

            // Define small sector width 
            float sectorWidth = 7f; // adjust for thickness

            using (var eraserBrush = new SolidBrush(Color.White))
            {
                var rect = new RectangleF(centerX - maxRadius * 1.3f,
                                          centerY - maxRadius * 1.3f,
                                          maxRadius * 2.6f,
                                          maxRadius * 2.6f);

                // Paint over this thin sector to "cut" it out
                graphics.FillPie(eraserBrush, rect, categoryAngle - sectorWidth / 2, sectorWidth);
            }

            //two thick black lines at the edges of the cut
            using (var thickPen = new Pen(ColorTranslator.FromHtml("#bbbdbe"), 4)) // thickness adjustable
            {
                for (int i = -1; i <= 1; i += 2) // -1 and +1 → both edges
                {
                    float edgeAngle = categoryAngle + (i * sectorWidth / 2);
                    float radians = edgeAngle * (float)Math.PI / 180f;

                    float startX = centerX + (float)(Math.Cos(radians) * innerCutRadius);
                    float startY = centerY + (float)(Math.Sin(radians) * innerCutRadius);

                    float endX = centerX + (float)(Math.Cos(radians) * maxRadius * 1.3f);
                    float endY = centerY + (float)(Math.Sin(radians) * maxRadius * 1.3f);

                    graphics.DrawLine(thickPen, startX, startY, endX, endY);
                }
            }
        }

        /// <summary>
        /// Draws trapezoidal competency bars on the wheel visualization for each
        /// competency score across all categories.  
        /// 
        /// Each bar extends radially outward from the inner cut radius, with length
        /// proportional to the competency's score (0–10). Bars are colored by category
        /// and outlined for clarity.
        /// </summary>
        /// <param name="graphics">The <see cref="Graphics"/> surface used for drawing.</param>
        /// <param name="centerX">The X coordinate of the wheel's center.</param>
        /// <param name="centerY">The Y coordinate of the wheel's center.</param>
        /// <param name="maxRadius">The maximum outer radius of the wheel.</param>
        private void DrawCompetencyBars(Graphics graphics, float centerX, float centerY, float maxRadius)
        {
            if (Categories == null || Categories.Count == 0) return;
            var totalCompetencies = Categories.Sum(c => c.GetTotalCompetencyCount());
            if (totalCompetencies == 0) return;

            var angleStep = (360f / totalCompetencies) * 0.98f; // 80% of original spacing
            var levelStep = maxRadius / 10f;
            var competencyIndex = 0;

            float innerCutRadius = levelStep * 2.2f;

            foreach (var category in Categories)
            {
                var categoryColor = CategoryColors.ContainsKey(category.Name.ToUpper())
                    ? CategoryColors[category.Name.ToUpper()]
                    : Color.Gray;

                foreach (var subCategory in category.SubCategories)
                {
                    foreach (var competency in subCategory.Competencies)
                    {
                        if (competency.Value > 0)
                        {
                            var angle = competencyIndex * angleStep - 90 + 4f;
                            var radians = angle * Math.PI / 180;

                            var innerRadius = innerCutRadius;
                            var outerRadius = innerCutRadius + competency.Value * levelStep ;


                            float innerWidth = maxRadius * 0.009f;
                            float outerWidth = maxRadius * 0.025f;

                            float dx = (float)Math.Cos(radians);
                            float dy = (float)Math.Sin(radians);
                            // Four trapezoid corners
                            var p1 = new PointF(centerX + dx * innerRadius - dy * innerWidth, centerY + dy * innerRadius + dx * innerWidth);
                            var p2 = new PointF(centerX + dx * innerRadius + dy * innerWidth, centerY + dy * innerRadius - dx * innerWidth);
                            var p3 = new PointF(centerX + dx * outerRadius + dy * outerWidth, centerY + dy * outerRadius - dx * outerWidth);
                            var p4 = new PointF(centerX + dx * outerRadius - dy * outerWidth, centerY + dy * outerRadius + dx * outerWidth);

                            PointF[] trapezoid = { p1, p2, p3, p4 };

                            using (var brush = new SolidBrush(categoryColor))
                                graphics.FillPolygon(brush, trapezoid);

                            using (var pen = new Pen(Color.Black, 1))
                            {
                                pen.Alignment = PenAlignment.Center;
                                graphics.DrawPolygon(pen, trapezoid);
                            }
                        }
                        competencyIndex++;
                    }
                }
            }
        }

        /// <summary>
        /// Draws circular score labels (1–10) along a chosen divider line of the wheel.
        /// Each score is positioned at the radius of its corresponding level circle,
        /// aligned with one radial line (the first category divider).
        /// </summary>
        /// <param name="graphics">The <see cref="Graphics"/> object used for drawing.</param>
        /// <param name="centerX">The X-coordinate of the wheel center.</param>
        /// <param name="centerY">The Y-coordinate of the wheel center.</param>
        /// <param name="maxRadius">The maximum radius of the wheel (outer edge).</param>
        private void DrawDynamicScoreLabels(Graphics graphics, float centerX, float centerY, float maxRadius)
        {
            if (Categories == null || Categories.Count == 0) return;

            var totalCompetencies = Categories.Sum(c => c.GetTotalCompetencyCount());
            var angleStep = 360f / totalCompetencies;
            var levelStep = maxRadius / 10f;

            // Same as used in DrawLevelCircles
            float innerCutRadius = levelStep * 2.2f;

            // Font for score labels
            using var scoreFont = new Font("Arial", Math.Max(maxRadius * 0.022f, 9f), FontStyle.Bold);
            using var scoreBrush = new SolidBrush(Color.FromArgb(240, 20, 20, 20));

            // Pick ONE divider line (first category)
            int startPosition = 0;
            float categoryAngle = startPosition * angleStep - 90 - (angleStep * 0.5f);
            float categoryRadians = categoryAngle * (float)Math.PI / 180f;

            float dx = (float)Math.Cos(categoryRadians);
            float dy = (float)Math.Sin(categoryRadians);

            // Draw labels 1–10 aligned with concentric circles
            for (int score = 1; score <= 10; score++) // start from 1 instead of 0
            {
                float radius = innerCutRadius + (score * levelStep); // exactly on each level circle

                float labelX = centerX + dx * radius;
                float labelY = centerY + dy * radius;

                string labelText = score.ToString();

                graphics.DrawString(labelText, scoreFont, scoreBrush, labelX, labelY,
                    new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    });
            }
        }


        /// <summary>
        /// Draws all labels on the wheel visualization, including:
        /// - Category names (outermost, larger font, color-coded).
        /// - Subcategory names (middle layer, bold, unified color).
        /// - Competency labels (innermost, smaller font, arranged along dividers).
        /// Labels are rotated and positioned along the wheel circumference
        /// so they align with their respective sectors.
        /// </summary>
        /// <param name="graphics">The <see cref="Graphics"/> object used for drawing.</param>
        /// <param name="centerX">The X-coordinate of the wheel center.</param>
        /// <param name="centerY">The Y-coordinate of the wheel center.</param>
        /// <param name="maxRadius">The maximum radius of the wheel (outer edge).</param>
        private void DrawLabels(Graphics graphics, float centerX, float centerY, float maxRadius)
        {
            if (Categories == null || Categories.Count == 0) return;
            var totalCompetencies = Categories.Sum(c => c.GetTotalCompetencyCount());
            if (totalCompetencies == 0) return;

            var angleStep = (360f / totalCompetencies) * 0.98f; // 80% of original spacing
            var competencyIndex = 0;
            var levelStep = maxRadius / 10f;

            float dividerEndRadius = maxRadius * 1.1f;
            float angleOffset = angleStep * -0.3f;

            using var level1Font = new Font("Arial", maxRadius * 0.050f, FontStyle.Bold);
            using var level2Font = new Font("Arial", maxRadius * 0.034f, FontStyle.Bold);
            using var level3Font = new Font("Arial", maxRadius * 0.031f, FontStyle.Bold);

            // Using same color for subcategories and competencies
            using var textBrush = new SolidBrush(ColorTranslator.FromHtml("#323031"));

            foreach (var category in Categories)
            {
                var categoryStartIndex = competencyIndex;
                var categoryCompetencyCount = category.GetTotalCompetencyCount();

                var categoryMidAngle = (categoryStartIndex + categoryCompetencyCount / 2f) * angleStep - 90;
                DrawRotatedLabel(graphics, category.Name.ToUpper(), centerX, centerY,
                    maxRadius * 1.6f, categoryMidAngle, level1Font,
                    new SolidBrush(CategoryColors.ContainsKey(category.Name.ToUpper())
                        ? CategoryColors[category.Name.ToUpper()]
                        : Color.Gray));

                foreach (var subCategory in category.SubCategories)
                {
                    var subCategoryStartIndex = competencyIndex;
                    var subCategoryCompetencyCount = subCategory.Competencies.Count;

                    if (subCategoryCompetencyCount > 0)
                    {
                        var subCategoryMidAngle = (subCategoryStartIndex + subCategoryCompetencyCount / 2f) * angleStep - 90;
                        DrawRotatedLabel(graphics, subCategory.Name.ToUpper(), centerX, centerY,
                            maxRadius * 1.45f, subCategoryMidAngle, level2Font, textBrush); // ✅ unified color
                    }

                    foreach (var competency in subCategory.Competencies)
                    {
                        float dividerAngle = competencyIndex * angleStep - 90 + angleOffset + 4f;
                        float dividerRadians = dividerAngle * (float)Math.PI / 180f;

                        var x = centerX + (float)(Math.Cos(dividerRadians) * dividerEndRadius);
                        var y = centerY + (float)(Math.Sin(dividerRadians) * dividerEndRadius);

                        var state = graphics.Save();
                        graphics.TranslateTransform(x, y);

                        float textRotation = dividerAngle;

                        if (dividerAngle > 85 && dividerAngle < 270)
                        {
                            textRotation = dividerAngle + 180;
                        }

                        graphics.RotateTransform(textRotation);

                        graphics.DrawString(competency.Label, level3Font, textBrush, 0, 0, //  unified color
                            new StringFormat
                            {
                                Alignment = StringAlignment.Center,
                                LineAlignment = StringAlignment.Center
                            });

                        graphics.Restore(state);
                        competencyIndex++;
                    }
                }
            }
        }

        /// <summary>
        /// Draws a text label at a given angle and radius around the circle, rotating it so it is readable.
        /// </summary>
        /// <param name="graphics">The <see cref="Graphics"/> object used for drawing.</param>
        /// <param name="text">The text string to draw as the label.</param>
        /// <param name="centerX">The X-coordinate of the circle center.</param>
        /// <param name="centerY">The Y-coordinate of the circle center.</param>
        /// <param name="radius">The radius from the circle center where the text will be placed.</param>
        /// <param name="angleDegrees">The angle (in degrees) around the circle where the label will be drawn.</param>
        /// <param name="font">The <see cref="Font"/> used to render the text.</param>
        /// <param name="brush">The <see cref="Brush"/> used to fill the text.</param>
        private void DrawRotatedLabel(Graphics graphics, string text, float centerX, float centerY,
            float radius, float angleDegrees, Font font, Brush brush)
        {
            var radians = angleDegrees * Math.PI / 180;
            var x = centerX + (float)(Math.Cos(radians) * radius);
            var y = centerY + (float)(Math.Sin(radians) * radius);

            var state = graphics.Save();
            graphics.TranslateTransform(x, y);

            float rotation = angleDegrees + 90;
            if (angleDegrees > 0 && angleDegrees <= 180)
            {
                rotation = angleDegrees - 90;
            }

            graphics.RotateTransform(rotation);

            var format = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            graphics.DrawString(text, font, brush, 0, 0, format);
            graphics.Restore(state);
        }

        /// <summary>
        /// Draws divider lines on the wheel visualization, including both:
        /// - Thick lines separating high-level categories.
        /// - Thin lines separating individual competencies within categories.
        /// 
        /// Divider lines start outside the white inner circle, leaving the center clean.
        /// </summary>
        /// <param name="graphics">The <see cref="Graphics"/> object used for drawing.</param>
        /// <param name="centerX">The X-coordinate of the circle center.</param>
        /// <param name="centerY">The Y-coordinate of the circle center.</param>
        /// <param name="maxRadius">The maximum radius of the wheel (outer edge).</param>
        private void DrawBarDividers(Graphics graphics, float centerX, float centerY, float maxRadius)
        {
            if (Categories == null || Categories.Count == 0) return;
            var totalCompetencies = Categories.Sum(c => c.GetTotalCompetencyCount());
            if (totalCompetencies == 0) return;

            var angleStep = (360f / totalCompetencies) * 0.98f;
            var levelStep = maxRadius / 10f;

            // Match inner cut radius from DrawLevelCircles (white hollow space)
            float innerCutRadius = levelStep * 2.2f;
            float dividerStartRadius = innerCutRadius + 4f; // start just outside white circle
            float dividerEndRadius = maxRadius * 1.3f;
            float angleOffset = angleStep * 0.5f;

            using var regularDividerPen = new Pen(ColorTranslator.FromHtml("#bbbdbe"), 1.8f);
            using var categoryDividerPen = new Pen(ColorTranslator.FromHtml("#bbbdbe"), 8.0f);

            int competencyIndex = 0;

            // Category dividers
            var categoryStartPositions = new List<int>();
            int tempIndex = 0;
            foreach (var category in Categories)
            {
                categoryStartPositions.Add(tempIndex);
                tempIndex += category.GetTotalCompetencyCount();
            }

            // Skip the first category divider
            for (int i = 1; i < categoryStartPositions.Count; i++)
            {
                float categoryAngle = categoryStartPositions[i] * angleStep - 90 - (angleStep * 0.5f) + 4f;
                float categoryRadians = categoryAngle * (float)Math.PI / 180f;

                float startX = centerX + (float)(Math.Cos(categoryRadians) * dividerStartRadius);
                float startY = centerY + (float)(Math.Sin(categoryRadians) * dividerStartRadius);

                float endX = centerX + (float)(Math.Cos(categoryRadians) * dividerEndRadius);
                float endY = centerY + (float)(Math.Sin(categoryRadians) * dividerEndRadius);

                graphics.DrawLine(categoryDividerPen, startX, startY, endX, endY);
            }

            // Competency dividers
            competencyIndex = 0;
            foreach (var category in Categories)
            {
                foreach (var subCategory in category.SubCategories)
                {
                    foreach (var competency in subCategory.Competencies)
                    {
                        float angle = competencyIndex * angleStep - 90 + angleOffset + 4f;
                        float radians = angle * (float)Math.PI / 180f;

                        float startX = centerX + (float)(Math.Cos(radians) * dividerStartRadius);
                        float startY = centerY + (float)(Math.Sin(radians) * dividerStartRadius);

                        float endX = centerX + (float)(Math.Cos(radians) * dividerEndRadius);
                        float endY = centerY + (float)(Math.Sin(radians) * dividerEndRadius);

                        graphics.DrawLine(regularDividerPen, startX, startY, endX, endY);

                        competencyIndex++;
                    }
                }
            }
        }

    }
}