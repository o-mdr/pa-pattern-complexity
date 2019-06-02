using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using PatternPA.Core.Interfaces;
using PatternPA.Core.Interfaces.Convertor;
using PatternPA.Core.Interfaces.FileOperation;
using PatternPA.Core.Interfaces.Nhanes;
using PatternPA.Core.Model;
using PatternPA.Core.Model.Nhanes;
using PatternPA.Utils;

namespace PatternPA.Container
{
    public class Container : IContainer, IDisposable
    {
        private static bool isConfigured;
        private static readonly object synchRoot = new object();

        private IWindsorContainer container;

        #region IContainer Members

        public void Register(params IRegistration[] regParams)
        {
            Initialize();
            container.Register(regParams);
        }

        public void Dispose()
        {
            if (container != null)
            {
                container.Dispose();
                container = null;
            }
        }

        public T Resolve<T>()
        {
            return container.Resolve<T>();
        }

        public Type Resolve(Type type)
        {
            return (Type) container.Resolve(type);
        }

        #endregion

        public IWindsorContainer Initialize()
        {
            lock (synchRoot)
            {
                if (!isConfigured)
                {
                    container = new WindsorContainer();

                    container.Register(Component.For<ICsvParser>().ImplementedBy<CsvParser>());
                    container.Register(Component.For<INhanesCsvParser>().ImplementedBy<NhanesCsvParser>());
                    container.Register(Component.For<IRecordConverter>().ImplementedBy<PalRecordsConverter>());
                    container.Register(Component.For<ICsvFileWriter>().ImplementedBy<CsvFileWriter>());
                    container.Register(Component.For<IFileWriter>().ImplementedBy<FileWriter>());
                    container.Register(Component.For<IArchiver>().ImplementedBy<GZipArchiver>());
                    container.Register(Component.For<IAlphabet>().ImplementedBy<ActivePalAlphabet>());
                    container.Register(Component.For<ICheckpointFactory>().ImplementedBy<CheckpointFactory>());
                    container.Register(Component.For<IBinaryConverter>().ImplementedBy<BinaryConverter>());
                    container.Register(Component.For<IRandomEventGenerator>().ImplementedBy<RandomEventGenerator>());
                    container.Register(Component.For<IBinaryFileWriter>().ImplementedBy<BinaryFileWriter>());
                    container.Register(Component.For<IRandomBitGenerator>().ImplementedBy<RandomBitGenerator>());

                    isConfigured = true;
                }
            }

            return container;
        }
    }
}