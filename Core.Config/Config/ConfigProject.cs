﻿namespace Core.Config.Config
{
    public partial class ConfigProject : IConfigProject
    {
        public ApiInformations ApiInformations { get; set; }
    }

    public interface IConfigProject
    {
        public ApiInformations ApiInformations { get; set; }
    }
}
