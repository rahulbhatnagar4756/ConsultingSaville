using Library.Assess.Models.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Assess.BusinessLogic.Projects.TestAssignmentProcessors
{
    internal class JawaTestAssignmentProcessor : ITestAssignmentProcessor
    {
        public async Task ProcessAsync(TestAssignmentModel assignment)
        {
            // Implement Jawa-specific processing logic
            // Call Jawa API, format data, etc.
            await Task.CompletedTask;
        }
    }
}
