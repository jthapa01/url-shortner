using System;
using System.Runtime.InteropServices;
using FluentAssertions;
using Xunit;

namespace UrlShortener.Api.Core.Tests
{
    public class Base62EncodingScenarios
    {
        [Theory]
        [InlineData(1, "0")]
        public void Test1(int number, string expected)
        {
            number.EncodeToBase62()
                .Should()
                .Be(expected);
        }
    }
}

public static class Base62EncodingExtensions
{
    public static string EncodeToBase62(this int number)
    {
        return String.Empty;
    }
}