using System.Reflection;
using Autofac.Extensions.DependencyInjection;

namespace MyForum.Helpers
{
    public static class AssemblyHelper
    {
        public static Assembly[] GetSolutionAssemblies()
        {
            return new[]
            {
                typeof(ServiceCollectionExtensions).Assembly,
                typeof(Business.Infrastructure.IRegisterDependency).Assembly,
                typeof(Core.Interfaces.Infrastructure.IRegisterDependency).Assembly,
                typeof(Data.Infrastructure.IRegisterDependency).Assembly
            };
        }
    }
}