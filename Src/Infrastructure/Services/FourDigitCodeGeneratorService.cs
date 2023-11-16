using Application.Common.Interfaces;

namespace Infrastructure.Services
{
    using System;

    public class FourDigitCodeGeneratorService : IFourDigitCodeGenerator
    {
        private Random random;

        public FourDigitCodeGeneratorService()
        {
            random = new Random();
        }

        public string GenerateCode()
        {
            int code = random.Next(1000, 10000);
            return code.ToString();
        }
    }
}