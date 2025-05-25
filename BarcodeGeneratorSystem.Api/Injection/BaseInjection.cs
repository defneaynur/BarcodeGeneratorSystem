using Core.Config.Config;

namespace BarcodeGeneratorSystem.Api.Injection
{
    public class BaseInjection : IBaseInjection
    {
        public IConfigProject ConfigProject { get; set; }
    }

    public interface IBaseInjection
    {
        public IConfigProject ConfigProject { get; set; }
    }
}
