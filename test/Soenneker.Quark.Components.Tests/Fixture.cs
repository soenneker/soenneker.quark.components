﻿using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Soenneker.Blazor.MockJsRuntime.Registrars;
using Soenneker.Fixtures.Unit;
using Soenneker.Utils.Test;

namespace Soenneker.Quark.Components.Tests;

public sealed class Fixture : UnitFixture
{
    public override ValueTask InitializeAsync()
    {
        SetupIoC(Services);

        Services.AddMockJsRuntimeAsScoped();

        return base.InitializeAsync();
    }

    private static void SetupIoC(IServiceCollection services)
    {
        services.AddLogging(builder =>
        {
            builder.AddSerilog(dispose: false);
        });

        IConfiguration config = TestUtil.BuildConfig();
        services.AddSingleton(config);
    }
}
