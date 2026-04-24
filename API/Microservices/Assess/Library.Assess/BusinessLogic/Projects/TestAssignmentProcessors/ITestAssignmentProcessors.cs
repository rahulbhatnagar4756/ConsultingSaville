using Library.Assess.Models.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Assess.BusinessLogic.Projects.TestAssignmentProcessors
{
    internal interface ITestAssignmentProcessor
    {
        Task ProcessAsync(TestAssignmentModel assignment);
    }
}
