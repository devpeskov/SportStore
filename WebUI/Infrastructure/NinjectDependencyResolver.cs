using Moq;
using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Abstract;
using Domain.Entities;
using Domain.EF;

namespace WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }
        private void AddBindings() 
        {
            kernel.Bind<IProductRepository>().To<EFProductRepository>();
            kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>();
        }

        public object GetService(Type serviecType)
        {
            return kernel.TryGet(serviecType);
        }

        public IEnumerable<object> GetServices(Type serviecType)
        {
            return kernel.GetAll(serviecType);
        }
    }
}