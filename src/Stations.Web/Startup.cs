using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore.Mvc;
using SimpleInjector.Lifestyles;
using Stations.Core.Interfaces;
using Stations.Core.Interfaces.Commands;
using Stations.Core.Interfaces.Events;
using Stations.Core.Interfaces.Queries;
using Stations.Infrastructure;
using Stations.Infrastructure.Cache;
using Stations.Infrastructure.Data;
using Stations.Infrastructure.Decorators;
using Stations.Web.Middleware;

namespace Stations.Web
{
    public class Startup
    {
        private readonly Container _container = new Container();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            IntegrateSimpleInjector(services);
        }

        private void IntegrateSimpleInjector(IServiceCollection services)
        {
            _container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            services.AddMemoryCache();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<IControllerActivator>(new SimpleInjectorControllerActivator(_container));
            services.AddSingleton<IViewComponentActivator>(new SimpleInjectorViewComponentActivator(_container));

            services.EnableSimpleInjectorCrossWiring(_container);
            services.UseSimpleInjectorAspNetRequestScoping(_container);
        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            InitializeContainer(app);
            app.UseMiddleware(typeof(ExceptionMiddleware));
            _container.Verify();

            if (!env.IsDevelopment())
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        private void InitializeContainer(IApplicationBuilder app)
        {
            // Application presentation components
            _container.RegisterMvcControllers(app);
            _container.RegisterMvcViewComponents(app);

            _container.Register<ICache, InMemCache>(Lifestyle.Scoped);

            // Commands
            _container.Register<ICommandDispatcher, CommandDispatcher>(Lifestyle.Scoped);
            _container.Register(typeof(ICommandValidator<>), typeof(ICommandValidator<>).Assembly, Lifestyle.Scoped);
            _container.RegisterDecorator(typeof(ICommandHandler<>), typeof(ValidationCommandHandlerDecorator<>), Lifestyle.Scoped);
            _container.Register(typeof(ICommandHandler<>), typeof(ICommandHandler<>).Assembly, Lifestyle.Scoped);

            // Events
            _container.Register<IDomainEventDispatcher, DomainEventDispatcher>(Lifestyle.Scoped);
            _container.Collection.Register(typeof(IDomainEventHandler<>), typeof(IDomainEventHandler<>).Assembly);
           
            // Queries
            _container.Register<IQueryProcessor, QueryProcessor>(Lifestyle.Scoped);
            _container.Register(typeof(IQueryHandler<,>), typeof(IQueryHandler<,>).Assembly, Lifestyle.Scoped);
            //_container.RegisterDecorator(typeof(IQueryHandler<,>), typeof(CachingQueryHandlerDecorator<,>), Lifestyle.Scoped);

            // Data
            _container.Register(typeof(IRepository<>), typeof(FlatFileRepository<>), Lifestyle.Scoped);
            _container.RegisterDecorator(typeof(IRepository<>), typeof(CachingRepositoryDecorator<>), Lifestyle.Scoped);

            // Allow Simple Injector to resolve services from ASP.NET Core.
            _container.AutoCrossWireAspNetComponents(app);
        }
    }
}
