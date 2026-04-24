using Library.Assess.Models.PersonalityWheel;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Assess.Models.PersonalityPDF
{
    public class PersonalityPdfRequest
    {
        [Required]
        public string CompanyLogo { get; set; }
        /// <summary>
        /// Content for page 1.
        /// </summary>
        [Required]
        public PersonalityPage1 Page1 { get; set; } = new PersonalityPage1();

        /// <summary>
        /// Content for page 2.
        /// </summary>
        [Required]
        public PersonalityPage2 Page2 { get; set; } = new PersonalityPage2();

        /// <summary>
        /// Content for page 3.
        /// </summary>
        [Required]
        public PersonalityPage3 Page3 { get; set; } = new PersonalityPage3();

        /// <summary>
        /// Footer information
        /// </summary>
        public PersonalityFooter Footer { get; set; } = new PersonalityFooter();

    }

    /// <summary>
    /// Page 1 content of the Personality Report.
    /// </summary>
    public class PersonalityPage1
    {
        /// <summary>
        /// Personality wheel data to render on page 1.
        /// </summary>
        [Required]
        public PersonalityWheelRequest Wheel { get; set; } = new PersonalityWheelRequest();
    }

    /// <summary>
    /// Page 2 content of the Personality Report.
    /// </summary>
    public class PersonalityPage2
    {
        [Required]
        public List<string> PerformanceEnhancers { get; set; }
        [Required]
        public List<string> PerformanceInhibitors { get; set; }
    }

    /// <summary>
    /// Page 3 content of the Personality Report.
    /// </summary>
    public class PersonalityPage3
    {
        public string TeamType { get; set; }
        public GridPositions GridPositions { get; set; }
        public List<string> ThoughtsAboutMyself { get; set; }
        public List<string> MyFrustrations { get; set; }
        public List<string> OthersThoughtsAboutMe { get; set; }
        public List<string> WhoComplementsMe { get; set; }
        public List<string> Teamwork { get; set; }
        public List<string> Leadership { get; set; }
        public List<string> HowIManage { get; set; }
        public List<string> PerformingAtMyBest { get; set; }
    }

    public class GridPositions
    {
        public TeamGridBoxes WarmTeam { get; set; }
        public TeamGridBoxes CoolTeam { get; set; }
    }

    // Class to hold name and color
    public class GridBox
    {
        public string Name { get; set; }
        public string Color { get; set; }
    }

    // Updated TeamGridBoxes class
    public class TeamGridBoxes
    {
        public GridBox TopLeft { get; set; }
        public GridBox TopRight { get; set; }
        public GridBox BottomLeft { get; set; }
        public GridBox BottomRight { get; set; }
    }

    public class GridPosition
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Intensity { get; set; } // "light", "medium", "dark"
    }

    public class PersonalityFooter
    {
        public string CandidateName { get; set; }
        public string Date { get; set; }
        public string Copyright { get; set; }
    }

    /// <summary>
    /// Response returned after generating a Talent Report.
    /// </summary>
    public class PersonalityPdfResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public byte[]? PdfData { get; set; }
        public string? FileName { get; set; }
        public DateTime GeneratedAt { get; set; }
    }
}
