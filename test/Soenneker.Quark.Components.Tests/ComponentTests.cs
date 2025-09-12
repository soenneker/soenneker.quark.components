using Soenneker.Tests.FixturedUnit;
using Xunit;

namespace Soenneker.Quark.Components.Tests;

[Collection("Collection")]
public sealed class ComponentTests : FixturedUnitTest
{

    public ComponentTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
    }

    [Fact]
    public void Default()
    {

    }
}
