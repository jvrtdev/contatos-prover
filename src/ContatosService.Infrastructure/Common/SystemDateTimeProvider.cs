using ContatosService.Application.Common.Interfaces;

namespace ContatosService.Infrastructure.Common;

public sealed class SystemDateTimeProvider : IDateTimeProvider
{
    public DateOnly Hoje => DateOnly.FromDateTime(DateTime.Today);
}
