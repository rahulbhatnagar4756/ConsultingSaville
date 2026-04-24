using Azure;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Assess.Models.PersonalityWheel
{
    /// <summary>
    /// Request model used to generate a personality wheel visualization.
    /// </summary>
    public class PersonalityWheelRequest
    {
        /// <summary>
        /// The hierarchical structure of categories, subcategories, and competencies
        /// that define the content of the wheel.
        /// </summary>
        [Required]
        public List<HighLevelCategoryRequest> Categories { get; set; } = new();

        /// <summary>
        /// Optional list of colors (hex or named) used to shade the concentric scoring levels.
        /// </summary>
        public List<string>? LevelColors { get; set; }

        /// <summary>
        /// Optional mapping of category names to color values (hex or named).
        /// </summary>
        public Dictionary<string, string>? CategoryColors { get; set; }

        /// <summary>
        /// Output image size in pixels (width and height).  
        /// Must be between 200 and 2000. Default is 1200.
        /// </summary>
        [Range(200, 2000)]
        public int Size { get; set; } = 1200;

        /// <summary>
        /// Whether to display level numbers (0–10) around the wheel.
        /// </summary>
        public bool ShowLevelNumbers { get; set; } = true;

        /// <summary>
        /// Whether to render the background grid (dividers and concentric circles).
        /// </summary>
        public bool ShowGrid { get; set; } = true;
    }


    /// <summary>
    /// Request model for a high-level category, such as
    /// "Delivering Results" or "Solving Problems".
    /// </summary>
    public class HighLevelCategoryRequest
    {
        /// <summary>
        /// The name of the high-level category.
        /// </summary>
        [Required]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// A list of subcategories contained within this category.
        /// </summary>
        [Required]
        public List<SubCategoryRequest> SubCategories { get; set; } = new();
    }

    /// <summary>
    /// Request model for a subcategory that groups multiple competencies.
    /// </summary>
    public class SubCategoryRequest
    {
        /// <summary>
        /// The name of the subcategory.
        /// </summary>
        [Required]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// A list of competency items belonging to this subcategory.
        /// </summary>
        [Required]
        public List<CompetencyRequest> Competencies { get; set; } = new();
    }

    /// <summary>
    /// Request model representing an individual competency
    /// with a label and numeric score (0–10).
    /// </summary>
    public class CompetencyRequest
    {
        /// <summary>
        /// The descriptive label for the competency.
        /// </summary>
        [Required]
        public string Label { get; set; } = string.Empty;

        /// <summary>
        /// The competency score on a 0–10 scale.
        /// </summary>
        [Range(0, 10)]
        public int Value { get; set; }
    }

    /// <summary>
    /// Response model returned after generating a personality wheel.
    /// </summary>
    public class PersonalityWheelResponse
    {
        /// <summary>
        /// Base64-encoded string of the generated wheel image.
        /// </summary>
        public string Base64Image { get; set; } = string.Empty;

        /// <summary>
        /// Whether the generation request was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// An error message if generation failed; otherwise null.
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// The size of the generated image in pixels.
        /// </summary>
        public int ImageSize { get; set; }

        /// <summary>
        /// The image format (e.g., PNG, JPEG). Default is PNG.
        /// </summary>
        public string Format { get; set; } = "PNG";

        /// <summary>
        /// Statistics about the wheel content, such as totals and averages.
        /// </summary>
        public WheelStatistics? Statistics { get; set; }
    }

    /// <summary>
    /// Summary statistics about the content of a generated wheel.
    /// </summary>
    public class WheelStatistics
    {
        /// <summary>
        /// The total number of high-level categories.
        /// </summary>
        public int TotalCategories { get; set; }

        /// <summary>
        /// The total number of subcategories across all categories.
        /// </summary>
        public int TotalSubCategories { get; set; }

        /// <summary>
        /// The total number of competencies across all subcategories.
        /// </summary>
        public int TotalCompetencies { get; set; }

        /// <summary>
        /// The average competency score for each category, keyed by category name.
        /// </summary>
        public Dictionary<string, double> CategoryAverages { get; set; } = new();
    }

    // ================================
    // Internal Model Classes
    // ================================

    /// <summary>
    /// Internal representation of a high-level category.
    /// </summary>
    public class HighLevelCategory
    {
        /// <summary>
        /// The category name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Subcategories contained within this category.
        /// </summary>
        public List<SubCategory> SubCategories { get; set; } = new();

        /// <summary>
        /// Calculates the total number of competencies across all subcategories.
        /// </summary>
        public int GetTotalCompetencyCount()
        {
            return SubCategories.Sum(sc => sc.Competencies.Count);
        }
    }

    /// <summary>
    /// Internal representation of a subcategory.
    /// </summary>
    public class SubCategory
    {
        /// <summary>
        /// The subcategory name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Competencies contained within this subcategory.
        /// </summary>
        public List<Competency> Competencies { get; set; } = new();
    }

    /// <summary>
    /// Internal representation of an individual competency.
    /// </summary>
    public class Competency
    {
        /// <summary>
        /// The competency label (display name).
        /// </summary>
        public string Label { get; set; } = string.Empty;

        /// <summary>
        /// The competency score (0–10).
        /// </summary>
        public int Value { get; set; }
    }

    /// <summary>
    /// Represents the result of a request validation check.
    /// </summary>
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
