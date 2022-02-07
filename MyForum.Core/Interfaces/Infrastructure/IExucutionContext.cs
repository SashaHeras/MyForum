using System;

namespace MyForum.Core.Interfaces.Infrastructure
{
    public interface IExecutionContext : ISingletonService
    {
        DateTime GetDate();

        string GetLoginName();
    }
}