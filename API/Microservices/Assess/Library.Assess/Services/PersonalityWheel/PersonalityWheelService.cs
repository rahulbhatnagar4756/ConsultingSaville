using Library.Assess.Models.PersonalityWheel;

using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Assess.Services.PersonalityWheel
{
    // Repository Interface and Implementation
    public interface IPersonalityWheelRepository
    {
        /// <summary>
        /// Generates a personality wheel image based on the provided request and returns a response with Base64 image and statistics.
        /// </summary>
        /// <param name="request">The wheel request containing categories, colors, and options.</param>
        /// <returns>A <see cref="PersonalityWheelResponse"/> containing the Base64 image and statistics.</returns>

        Task<PersonalityWheelResponse> GenerateWheelImageAsync(PersonalityWheelRequest request);
        /// <summary>
        /// Generates a personality wheel image as raw PNG bytes.
        /// </summary>
        /// <param name="request">The wheel request containing categories, colors, and options.</param>
        /// <returns>Byte array containing the PNG image.</returns>

        Task<byte[]> GenerateWheelBytesAsync(PersonalityWheelRequest request);
    }

    public class PersonalityWheelRepository : IPersonalityWheelRepository
    {
        /// <summary>
        /// Generates a personality wheel image based on the provided request and returns a response with Base64 image and statistics.
        /// </summary>
        /// <param name="request">The wheel request containing categories, colors, and options.</param>
        /// <returns>A <see cref="PersonalityWheelResponse"/> containing the Base64 image and statistics.</returns>

        public async Task<PersonalityWheelResponse> GenerateWheelImageAsync(PersonalityWheelRequest request)
        {
            try
            {
                // Validate request
                var validationResult = ValidateRequest(request);
                if (!validationResult.IsValid)
                {
                    return new PersonalityWheelResponse
                    {
                        Success = false,
                        ErrorMessage = validationResult.ErrorMessage
                    };
                }

                var imageBytes = await GenerateWheelBytesAsync(request);
                var base64String = Convert.ToBase64String(imageBytes);

                // Calculate statistics
                var statistics = CalculateStatistics(request);

                return new PersonalityWheelResponse
                {
                    Base64Image = base64String,
                    Success = true,
                    ImageSize = request.Size,
                    Format = "PNG",
                    Statistics = statistics
                };
            }
            catch (Exception ex)
            {
                return new PersonalityWheelResponse
                {
                    Success = false,
                    ErrorMessage = $"Error generating wheel: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Generates a personality wheel image as raw PNG bytes.
        /// </summary>
        /// <param name="request">The wheel request containing categories, colors, and options.</param>
        /// <returns>Byte array containing the PNG image.</returns>

        public async Task<byte[]> GenerateWheelBytesAsync(PersonalityWheelRequest request)
        {
            return await Task.Run(() =>
            {
                var wheel = new Library.Assess.Services.PersonalityWheel.PersonalityWheel
                {
                    Categories = ConvertToWheelCategories(request.Categories),
                    ShowLevelNumbers = request.ShowLevelNumbers,
                    ShowGrid = false
                };

                // Set custom level colors if provided
                if (request.LevelColors?.Count == 10)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        if (TryParseColor(request.LevelColors[i], out var color))
                        {
                            wheel.LevelColors[i] = color;
                        }
                    }
                }

                // Set category colors if provided
                if (request.CategoryColors != null)
                {
                    foreach (var kvp in request.CategoryColors)
                    {
                        if (TryParseColor(kvp.Value, out var color))
                        {
                            wheel.CategoryColors[kvp.Key.ToUpper()] = color;
                        }
                    }
                }

                using var bitmap = wheel.Draw(request.Size);
                using var stream = new MemoryStream();
                bitmap.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            });
        }

        /// <summary>
        /// Converts request category models into wheel-compatible category objects.
        /// </summary>
        /// <param name="requestCategories">The list of categories provided in the request.</param>
        /// <returns>A list of wheel-specific high-level categories.</returns>
        private List<HighLevelCategory> ConvertToWheelCategories(
            List<HighLevelCategoryRequest> requestCategories)
        {
            var wheelCategories = new List<HighLevelCategory>();

            foreach (var reqCategory in requestCategories)
            {
                var wheelCategory = new HighLevelCategory
                {
                    Name = reqCategory.Name,
                    SubCategories = new List<SubCategory>()
                };

                foreach (var reqSubCategory in reqCategory.SubCategories)
                {
                    var wheelSubCategory = new SubCategory
                    {
                        Name = reqSubCategory.Name,
                        Competencies = new List<Competency>()
                    };

                    foreach (var reqCompetency in reqSubCategory.Competencies)
                    {
                        wheelSubCategory.Competencies.Add(new Competency
                        {
                            Label = reqCompetency.Label,
                            Value = reqCompetency.Value
                        });
                    }

                    wheelCategory.SubCategories.Add(wheelSubCategory);
                }

                wheelCategories.Add(wheelCategory);
            }

            return wheelCategories;
        }

        /// <summary>
        /// Validates the personality wheel request for required fields and constraints.
        /// </summary>
        /// <param name="request">The wheel request containing categories and competencies.</param>
        /// <returns>
        /// A <see cref="ValidationResult"/> indicating whether the request is valid, with error details if not.
        /// </returns>
        private ValidationResult ValidateRequest(PersonalityWheelRequest request)
        {
            if (request.Categories == null || request.Categories.Count == 0)
            {
                return new ValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "At least one category is required."
                };
            }

            if (request.Categories.Count > 8)
            {
                return new ValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "Maximum 8 categories allowed."
                };
            }

            var totalCompetencies = 0;
            foreach (var category in request.Categories)
            {
                if (category.SubCategories == null || category.SubCategories.Count == 0)
                {
                    return new ValidationResult
                    {
                        IsValid = false,
                        ErrorMessage = $"Category '{category.Name}' must have at least one subcategory."
                    };
                }

                foreach (var subCategory in category.SubCategories)
                {
                    if (subCategory.Competencies == null || subCategory.Competencies.Count == 0)
                    {
                        return new ValidationResult
                        {
                            IsValid = false,
                            ErrorMessage = $"Subcategory '{subCategory.Name}' must have at least one competency."
                        };
                    }
                    totalCompetencies += subCategory.Competencies.Count;
                }
            }

            if (totalCompetencies > 64)
            {
                return new ValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "Maximum 64 total competencies allowed."
                };
            }

            return new ValidationResult { IsValid = true };
        }

        /// <summary>
        /// Calculates statistics about the wheel, including totals and category averages.
        /// </summary>
        /// <param name="request">The wheel request with categories and competency values.</param>
        /// <returns>A <see cref="WheelStatistics"/> object with computed totals and averages.</returns>
        private WheelStatistics CalculateStatistics(PersonalityWheelRequest request)
        {
            var stats = new WheelStatistics
            {
                TotalCategories = request.Categories.Count,
                TotalSubCategories = 0,
                TotalCompetencies = 0,
                CategoryAverages = new Dictionary<string, double>()
            };

            foreach (var category in request.Categories)
            {
                stats.TotalSubCategories += category.SubCategories.Count;

                var categoryValues = new List<int>();
                foreach (var subCategory in category.SubCategories)
                {
                    stats.TotalCompetencies += subCategory.Competencies.Count;
                    categoryValues.AddRange(subCategory.Competencies.Select(c => c.Value));
                }

                if (categoryValues.Count > 0)
                {
                    stats.CategoryAverages[category.Name] = categoryValues.Average();
                }
            }

            return stats;
        }

        /// <summary>
        /// Attempts to parse a color string into a <see cref="System.Drawing.Color"/>.
        /// </summary>
        /// <param name="colorString">The color string (Hex, RGB, or known name).</param>
        /// <param name="color">The parsed <see cref="System.Drawing.Color"/> if successful.</param>
        /// <returns><c>true</c> if parsing succeeded; otherwise <c>false</c>.</returns>

        private bool TryParseColor(string colorString, out System.Drawing.Color color)
        {
            color = System.Drawing.Color.Black;
            try
            {
                if (colorString.StartsWith("#"))
                {
                    color = System.Drawing.ColorTranslator.FromHtml(colorString);
                    return true;
                }
                else if (colorString.StartsWith("rgb("))
                {
                    var values = colorString.Replace("rgb(", "").Replace(")", "").Split(',');
                    if (values.Length == 3)
                    {
                        color = System.Drawing.Color.FromArgb(
                            int.Parse(values[0].Trim()),
                            int.Parse(values[1].Trim()),
                            int.Parse(values[2].Trim())
                        );
                        return true;
                    }
                }
                else
                {
                    color = System.Drawing.Color.FromName(colorString);
                    return color.IsKnownColor;
                }
            }
            catch
            {
                return false;
            }
            return false;
        }        
    }
}
