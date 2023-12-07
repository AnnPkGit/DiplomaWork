using Application.Common.Interfaces;

namespace Infrastructure.Services
{
    using System;

    public class FourDigitCodeGeneratorService : IFourDigitCodeGenerator
    {
        private readonly Random _random = new();

        public string GenerateCode()
        {
            var code = _random.Next(1000, 10000);
            return code.ToString();
        }
    }
}