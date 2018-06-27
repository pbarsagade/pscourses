using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebAPI.Data;
using WebAPI.Repository;
using WebAPI.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(setupAction=> {
                setupAction.ReturnHttpNotAcceptable = true;
                setupAction.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
            });

            string conString = Configuration["ConnectionStrings:DefaultConnection"];
            services.AddDbContext<DataContext>(options => options.UseSqlServer(conString));

            services.AddScoped<ILiberaryRepository, LiberaryRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, DataContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                context.DataContextSeed();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async ctx =>
                    {
                        ctx.Response.StatusCode = 500;
                        await ctx.Response.WriteAsync("An unexpected fault happened. Try again later");
                    });
                });
            }
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Data.Author, Models.AuthorDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.GetCurrentAge()));

                cfg.CreateMap<Data.Book, Models.BookDto>();
                cfg.CreateMap<Models.NewAuthorDto, Data.Author>();
                cfg.CreateMap<Models.NewBookDto, Data.Book>();
                cfg.CreateMap<Models.EditBookDto, Data.Book>();
                cfg.CreateMap<Data.Book, Models.EditBookDto>();
            });
            app.UseMvc();
        }
    }
}
