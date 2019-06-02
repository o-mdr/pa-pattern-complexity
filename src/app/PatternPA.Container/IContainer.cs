using System;
using Castle.MicroKernel.Registration;

namespace PatternPA.Container
{
    /// <summary>
    /// Describes interface of IoC contianer
    /// </summary>
    public interface IContainer
    {
        void Register(params IRegistration[] regParams);
        T Resolve<T>();
        Type Resolve(Type type);
        void Dispose();
    }
}