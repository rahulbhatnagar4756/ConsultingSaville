using Library.Assess.Models.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Assess.BusinessLogic.Projects.TestAssignmentProcessors
{
    internal class DefaultTestAssignmentProcessor : ITestAssignmentProcessor
    {
        public async Task ProcessAsync(TestAssignmentModel assignment)
        {
            // Default processing logic
            //await Task.CompletedTask;
        }
    }
}
