using Autofac;
using Autofac.Features.AttributeFilters;
using Library.Common;

namespace SmaugCS.Logging;

public class LoggingModule(Config.Configuration.Constants constants) : Module
{
  protected override void Load(ContainerBuilder builder)
  {
    builder.RegisterType<CommonTimer>().Named<ITimer>("LogDumpTimer")
      .OnActivated(x => x.Instance.Interval = constants.LogDumpFrequencyMS);

    builder.RegisterType<LogManager>().As<ILogManager>()
      .SingleInstance()
      .WithAttributeFiltering();
  }
}