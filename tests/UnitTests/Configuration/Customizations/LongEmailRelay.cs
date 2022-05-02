using AutoFixture;
using AutoFixture.Kernel;
using System.Reflection;

namespace UnitTests.Configuration
{
    public class LongEmailRelay : ISpecimenBuilder
    {
        private static readonly string ParameterName = "longEmailPassword";
        private static readonly int LengthFloor = 101;
        private static readonly int LengthCeil = 200;

        public object Create(object request, ISpecimenContext context)
        {
            if (request is not ParameterInfo parameterInfo)
            {
                return new NoSpecimen();
            }

            if (parameterInfo.ParameterType != typeof(string) && parameterInfo.Name != ParameterName)
            {
                return new NoSpecimen();
            }

            return new ConstrainedStringGenerator()
                .Create(new ConstrainedStringRequest(LengthFloor, LengthCeil), context);
        }
    }
}