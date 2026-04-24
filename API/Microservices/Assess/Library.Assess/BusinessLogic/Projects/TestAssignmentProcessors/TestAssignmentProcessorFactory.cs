using Library.Assess.Enumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Assess.BusinessLogic.Projects.TestAssignmentProcessors
{
    internal static class TestAssignmentProcessorFactory
    {
        private static readonly Dictionary<string, Func<ITestAssignmentProcessor>> ProcessorFactories = new()
        {
            { IntegrationsUUID.Values[IntegrationsEnum.Jawa], () => new JawaTestAssignmentProcessor() },
            { IntegrationsUUID.Values[IntegrationsEnum.Oasys], () => new OasysTestAssignmentProcessor() }
        };

        public static ITestAssignmentProcessor CreateProcessor(string? integrationUUID)
        {
            if (string.IsNullOrEmpty(integrationUUID) ||
                !ProcessorFactories.TryGetValue(integrationUUID, out var factory))
            {
                return new DefaultTestAssignmentProcessor();
            }

            return factory();
        }
    }
}
